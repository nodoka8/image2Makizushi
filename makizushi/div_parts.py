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
    cv2.imwrite("2color.png", img_2)
    #輪郭抽出
    contours, hierarchy = cv2.findContours(img_2, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
    contours_list = list(contours)
    hierarchy_list = list(hierarchy[0])
    for i in reversed(range(len(contours_list))):
        if cv2.contourArea(contours_list[i]) < 30:
            contours_list.pop(i)
            hierarchy_list.pop(i)
    return [contours_list, hierarchy_list]

#図形近似
def app_shape(imagename):
    contours = find_contours(imagename, cv2.THRESH_BINARY)[0]
    num_cnts = len(contours)
    matches = [0] * num_cnts
    shape = [0,0,0,0]
    circle_contours = find_contours("circle", cv2.THRESH_OTSU)[0]
    square_contours = find_contours("square", cv2.THRESH_OTSU)[0]
    half_circle_contours = find_contours("half_circle", cv2.THRESH_OTSU)[0]
    line_contours = find_contours("line", cv2.THRESH_OTSU)[0]
    for i in range(len(matches)):
        shape[0] = cv2.matchShapes(contours[i], circle_contours[0], cv2.CONTOURS_MATCH_I1, 0)
        shape[1] = cv2.matchShapes(contours[i], square_contours[0], cv2.CONTOURS_MATCH_I1, 0)
        shape[2] = cv2.matchShapes(contours[i], half_circle_contours[0], cv2.CONTOURS_MATCH_I1, 0)
        shape[3] = cv2.matchShapes(contours[i], line_contours[0], cv2.CONTOURS_MATCH_I1, 0)
        matches[i] = shape.index(min(shape))
    return matches

def image_numberdraw(imagename):
    contours = find_contours(imagename, cv2.THRESH_BINARY)[0]
    copy = image.copy()
    for i in range(len(contours)):
        # 輪郭データをタプルに
        area = tuple(map(lambda c: tuple(c[0]), contours[i].tolist()))
        print(len(contours))
        #インデックスカラーに塗り潰し
        ImageDraw.Draw(copy).polygon(area, fill=(i, 0, 0))
    copy.save(imagename + '_numbercolor.png')
    csv_w([copy], 'w')

def calc_rice(imagename):
    contours = find_contours(imagename, cv2.THRESH_BINARY)[0]
    hierarchy = find_contours(imagename, cv2.THRESH_BINARY)[1]
    rice = []
    for i in range(len(contours)):
        rice[i] = cv2.contourArea(contours[i])
        if hierarchy[i][3] != -1:
            rice[hierarchy[i][3]] = rice[hierarchy[i][3]] - rice[i]
    return rice

def div_parts(imagename):
    contours = find_contours(imagename, cv2.THRESH_BINARY)[0]
    hierarchy = find_contours(imagename, cv2.THRESH_BINARY)[1]
    matches = app_shape(imagename)
    rice = calc_rice(imagename)
    for i in range(len(contours)):
        # 輪郭データをタプルに
        area = tuple(map(lambda c: tuple(c[0]), contours[i].tolist()))
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
        output.save(imagename + str(i) + '.png')
        csv_w([output, matches[i], hierarchy[i][3], rice[i]], 'a')

def calc_rice(imagename):
    contours = find_contours(imagename, cv2.THRESH_BINARY)[0]
    hierarchy = find_contours(imagename, cv2.THRESH_BINARY)[1]
    rice = []
    for i in range(len(contours)):
        rice[i] = cv2.contourArea(contours[i])
        if hierarchy[i][3] != -1:
            rice[hierarchy[i][3]] = rice[hierarchy[i][3]] - rice[i]
    return rice

image_numberdraw(imagename)       
div_parts(imagename)


    

