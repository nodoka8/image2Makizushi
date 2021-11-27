from os import environ
import cv2
from PIL import Image, ImageDraw
from functools import reduce
import csv 

imagename = "me"
#画像の読み込み
image = Image.open(imagename + ".png")

#-------csv書き込み--------
def csv_w(row, mode):
    with open('parts.csv', mode) as f:
        writer = csv.writer(f)
        writer.writerow(row)

def find_contours(imagename, mode):
    #グレースケール化
    img = cv2.cvtColor(cv2.imread(imagename + ".png"), cv2.COLOR_BGR2GRAY)
    #色を反転させて2値化
    _, img_2 = cv2.threshold(img, 128, 255, mode)
    #輪郭抽出
    contours, hierarchy = cv2.findContours(img_2, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
    #print(len(labels))
    contours_list = list(contours)
    hierarchy_list = list(hierarchy[0])
    #labels_list = list(labels[0])
    for i in reversed(range(len(contours_list))):
        if cv2.contourArea(contours_list[i]) < 30:
            if hierarchy_list[i][1] != -1:
                hierarchy_list[hierarchy_list[i][1]][0] = hierarchy_list[i][0]
            if hierarchy_list[i][0] != -1:
                hierarchy_list[hierarchy_list[i][0]][1] = hierarchy_list[i][1]
            if hierarchy_list[i][3] != -1 and hierarchy_list[hierarchy_list[i][3]][2] == i:
                hierarchy_list[hierarchy_list[i][3]][2] = hierarchy_list[i][0]
            contours_list.pop(i)
            hierarchy_list.pop(i)
            #labels_list.pop(i)
    return [contours_list, hierarchy_list]

#図形近似
def app_shape(contours):
    num_cnts = len(contours)
    matches = [0] * num_cnts
    shape = [0,0,0,0]
    circle_contours = find_contours("base_shape/circle", cv2.THRESH_OTSU)[0]
    square_contours = find_contours("base_shape/square", cv2.THRESH_OTSU)[0]
    half_circle_contours = find_contours("base_shape/half_circle", cv2.THRESH_OTSU)[0]
    line_contours = find_contours("base_shape/line", cv2.THRESH_OTSU)[0]
    for i in range(len(matches)):
        shape[0] = cv2.matchShapes(contours[i], circle_contours[0], cv2.CONTOURS_MATCH_I1, 0)
        shape[1] = cv2.matchShapes(contours[i], square_contours[0], cv2.CONTOURS_MATCH_I1, 0)
        shape[2] = cv2.matchShapes(contours[i], half_circle_contours[0], cv2.CONTOURS_MATCH_I1, 0)
        shape[3] = cv2.matchShapes(contours[i], line_contours[0], cv2.CONTOURS_MATCH_I1, 0)
        matches[i] = shape.index(min(shape))
    return matches

def image_numberdraw(parts_imagename):
    parts_image = Image.open("parts_image/" + parts_imagename + ".png")
    contours = find_contours("parts_image/" + parts_imagename, cv2.THRESH_BINARY)[0]
    for i in range(len(contours)):
        # 輪郭データをタプルに
        area = tuple(map(lambda c: tuple(c[0]), contours[i]))
        #インデックスカラーに塗り潰し
        ImageDraw.Draw(parts_image).polygon(area, fill=(i, 0, 0))
    parts_image.save("IDcolor_image/" + parts_imagename + '_IDcolor.png')
    return parts_imagename + '_IDcolor.png'

def calc_size(contours, hierarchy):
    rice = [0]*len(contours)
    nori = [0]*len(contours)
    for i in range(len(contours)):
        rice[i] = cv2.contourArea(contours[i])
        nori[i] = cv2.arcLength(contours[i], True)
        if hierarchy[i][3] != -1:
            rice[hierarchy[i][3]] = rice[hierarchy[i][3]] - rice[i]
    return [rice, nori]

def count_child(n, hierarchy):
    count = 0
    for i in range(len(hierarchy)):
        if hierarchy[i][3] == n:
            count = count + 1
    return count

def div_parts(imagename):
    contours = find_contours(imagename, cv2.THRESH_BINARY)[0]
    hierarchy = find_contours(imagename, cv2.THRESH_BINARY)[1]
    matches = app_shape(contours)
    size_rice = calc_size(contours, hierarchy)[0]
    size_nori = calc_size(contours, hierarchy)[1]
    for i in range(len(contours)):
        # 輪郭データをタプルに
        area = tuple(map(lambda c: tuple(c[0]), contours[i]))
        # マスク画像
        mask = Image.new("L", image.size, 0)
        ImageDraw.Draw(mask).polygon(area, fill=255)
        copy = image.copy()
        copy.putalpha(mask)
        # 切り抜き
        box = reduce(lambda p, c: [
            min(p[0], c[0]), min(p[1], c[1]),
            max(p[2], c[0]), max(p[3], c[1])
        ], area, [image.width, image.height, 0, 0])
        bbox = [box[0]-5, box[1]-5, box[2]+5, box[3]+5]
        output = copy.crop(bbox)
        image_name = imagename + str(i)
        image_name_numberdraw = image_numberdraw(image_name)
        output.save("parts_image/" + image_name + '.png')
        csv_w([i, image_name + ".png", image_name_numberdraw, matches[i], hierarchy[i][2], hierarchy[i][3], image.getpixel(area[i]), count_child(i, hierarchy), size_rice[i], size_nori[i]], 'a')

csv_w(["パーツ番号", "パーツ画像", "ID変換画像", "-1だったら独立パーツ", "親パーツ番号", "パーツカラー", "内包パーツ数", "ご飯の量", "海苔のサイズ"], "w")      
div_parts(imagename)


    

