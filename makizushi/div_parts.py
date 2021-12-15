from os import environ
import cv2
from PIL import Image, ImageDraw
from functools import reduce
import csv 
import flood
import numpy as np

imagename = "mee"
#画像の読み込み
image = Image.open(imagename + ".png")

#cannyによるエッジ検出
def canny(imagename):
    img = cv2.imread(imagename + ".png")
    img_gauss = cv2.GaussianBlur(img, (5, 5), 3)
    img_canny = cv2.Canny(img_gauss, 100, 200)
    cv2.imwrite("canny.png", img_canny)
    return img_canny

#エッジ検出した画像をもとにした2値化
def img2bw(imagename):
    img_canny = canny(imagename)
    width, height = img_canny.shape
    for i in range(width):
        for j in range(height):
            if img_canny[i, j] == 0:
                if img_canny[i-2, j] == 128:
                    flood.flood(i, j, 255, width, height, img_canny)
                else:
                    flood.flood(i, j, 128, width, height, img_canny)
    for i in range(width):
        for j in range(height):
            if img_canny[i, j] == 128:
                img_canny[i, j] = 255
            else:
                img_canny[i, j] = 0
    return img_canny

#-------csv書き込み--------
def csv_w(row, mode):
    with open('parts.csv', mode, encoding="utf_8_sig") as f:
        writer = csv.writer(f)
        writer.writerow(row)

#-------輪郭抽出(mode=0:複数輪郭の検出　mode=1:単体輪郭の検出)--------
def find_contours(imagename, mode):
    #色を反転させて2値化
    if mode == 0:
        img_2 = img2bw(imagename)
        cv2.imwrite("image2bw.png", img_2)
    else:
        img = cv2.cvtColor(cv2.imread(imagename + ".png"), cv2.COLOR_BGR2GRAY)
        _, img_2 = cv2.threshold(img, 0, 255, cv2.THRESH_OTSU)
    #輪郭抽出
    contours, hierarchy = cv2.findContours(img_2, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
    #print(len(labels))
    contours_list = list(contours)
    hierarchy_list = list(hierarchy[0])
    #labels_list = list(labels[0])
    """
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
    """
    return [contours_list, hierarchy_list]

#図形近似
def app_shape(contours):
    num_cnts = len(contours)
    matches = [0] * num_cnts
    shape = [0,0,0,0]
    circle_contours = find_contours("base_shape/circle", 1)[0]
    square_contours = find_contours("base_shape/square", 1)[0]
    half_circle_contours = find_contours("base_shape/half_circle", 1)[0]
    line_contours = find_contours("base_shape/line", 1)[0]
    for i in range(len(matches)):
        shape[0] = cv2.matchShapes(contours[i], circle_contours[0], cv2.CONTOURS_MATCH_I1, 0)
        shape[1] = cv2.matchShapes(contours[i], square_contours[0], cv2.CONTOURS_MATCH_I1, 0)
        shape[2] = cv2.matchShapes(contours[i], half_circle_contours[0], cv2.CONTOURS_MATCH_I1, 0)
        shape[3] = cv2.matchShapes(contours[i], line_contours[0], cv2.CONTOURS_MATCH_I1, 0)
        matches[i] = shape.index(min(shape))
    return matches

#いらんやつ
def image_numberdraw(contours):
    number_image = image.copy()
    for i in range(len(contours)):
        # 輪郭データをタプルに
        area = tuple(map(lambda c: tuple(c[0]), contours[i]))
        #インデックスカラーに塗り潰し
        ImageDraw.Draw(number_image).polygon(area, fill=(i, 0, 0))
    number_image.save("number_image.png")
    return "number_image"

#ご飯と海苔の量計算
def calc_size(contours):
    rice = [0]*len(contours)
    nori = [0]*len(contours)
    for i in range(len(contours)):
        rice[i] = cv2.contourArea(contours[i])
        nori[i] = cv2.arcLength(contours[i], True)
    return [rice, nori]

#内包パーツ数の計算
def count_child(n, hierarchy):
    count = 0
    for i in range(len(hierarchy)):
        if hierarchy[i][3] == n:
            count = count + 1
    return count

#求めた輪郭情報をもとにパーツのトリミング
def image2parts(imagename, contours, i):
    # 輪郭データをタプルに
    area = tuple(map(lambda c: tuple(c[0]), contours[i]))
    # マスク画像
    mask = Image.new("L", image.size, 0)
    ImageDraw.Draw(mask).polygon(area, fill=255)
    base_image = Image.open(imagename + ".png")
    base_image.putalpha(mask)
    # 切り抜き
    box = reduce(lambda p, c: [
        min(p[0], c[0]), min(p[1], c[1]),
        max(p[2], c[0]), max(p[3], c[1])
    ], area, [base_image.width, base_image.height, 0, 0])
    bbox = [box[0]-5, box[1]-5, box[2]+5, box[3]+5]
    output = base_image.crop(bbox)
    image_name = imagename + str(i)
    return [output, image_name]

def resipi_line_image(contours):
    line_imagename = []
    for i in range(len(contours)):
        line_image = Image.open(imagename + ".png")
        y = 0
        for j in contours[i]:
            if j[0][1] >= y:
                y = j[0][1]
        for l in range(3):
            for k in range(line_image.width):
                line_image.putpixel((k, y+l-2), (0, 255, 255))
        line_image.save("line_image_a/line_image" + str(i) + ".png")
        y_image = cv2.imread("line_image_a/line_image" + str(i) + ".png")
        _, width, _ = y_image.shape[:3]
        for k in range(y-2):
            for j in range(width):
                y_image[k, j] = y_image[k, j, 0]*0.4+75
        cv2.imwrite("line_image/line_image" + str(i) + ".png", y_image)
        line_imagename.append("line_image" + str(i))
    return line_imagename


#各パーツの最下ピクセルのy座標（制作順序のため）
def start_point(contours):
    y = 0
    for i in contours:
        if i[0][1] >= y:
            y = i[0][1]
    return y

def div_parts(imagename):
    #各パーツ情報のlist
    data = []
    #独立パーツのパーツ番号
    a = 0
    #後付けパーツのパーツ番号
    b = 0
    #その他のパーツ番号
    c = 0

    ch = find_contours(imagename, 0)
    contours = ch[0]
    hierarchy = ch[1]
    number_imagename = image_numberdraw(contours)
    line_imagename = resipi_line_image(contours)
    matches = app_shape(contours)
    size_rice = calc_size(contours)[0]
    size_nori = calc_size(contours)[1]
    nori_base_size = size_nori[1]
    rice_base_size = size_rice[1]
    for i in range(len(contours)):
        line_imagename_list = ""
        start_parts = start_point(contours[i])
        partsimage = image2parts(imagename, contours, i)
        IDcolor_partsimage = image2parts(number_imagename, contours, i)
        for j in range(len(contours)):
            if hierarchy[j][3] == i:
                resipi_line_partsimage = image2parts("line_image/" + line_imagename[j], contours, i)
                #ガイドラインイメージ保存
                resipi_line_partsimage[0].save("resipi_line_image/" + resipi_line_partsimage[1].split("/")[1] + '.png')
                line_imagename_list = line_imagename_list + resipi_line_partsimage[1].split("/")[1] + '.png, '
    
        #入力画像(0)と完成イメージ(1)
        if i == 1 and i == 0:
            partsimage[0].save("input_image/" + partsimage[1] + '.png')
        #その他
        else:
            partsimage[0].save("parts_image/" + partsimage[1] + '.png')

        #IDカラーイメージ保存
        IDcolor_partsimage[0].save("IDcolor_image/" + IDcolor_partsimage[1] + '.png')

        #ノリ枚数分母
        nori_mai = round(nori_base_size/size_nori[i])
        #ノリ枚数の分母>4の場合は後付けパーツ(0)
        if nori_mai > 4:
            hierarchy[i][2] = 0
            data[hierarchy[i][3]][4] = -1


        data.append([i, partsimage[1] + ".png", IDcolor_partsimage[1] + ".png", matches[i], hierarchy[i][2], hierarchy[i][3], None, count_child(i, hierarchy), round(30*size_rice[i]/rice_base_size)*10, "'1/"+str(nori_mai), start_parts, line_imagename_list[:-2]])
    
    for k in range(len(data)):
        #独立パーツ番号編集
        if data[k][4] == -1:
            data[k][0] = a
            #パーツ番号編集にともなった核発の親パーツ番号編集
            for i in range(len(data)):
                if data[i][5] == k:
                    data[i][5] = a
            a = a+1
        #後付けパーツ番号編集
        elif data[k][4] == 0:
            data[k][0] = b
            b = b+1
        #その他のパーツ番号編集
        else:
            if data[k][0] != 0:
                data[k][0] = c
                #パーツ番号編集にともなった核発の親パーツ番号編集
                for i in range(len(data)):
                    if data[i][5] == k:
                        data[i][5] = c
                c = c+1
          
    #親パーツ並べて
    for j in data:
        if j[4] != -1 and j[4] != 0 and j[5] != -1:
            csv_w(j, "a")
    
    #その下に独立パーツ
    for j in data:
        if j[4] == -1:
            csv_w(j, "a")
    
    #その下に後付けパーツ
    for j in data:
        if j[4] == 0:
            csv_w(j, "a")

csv_w(["パーツ番号", "パーツ画像", "ID変換画像", "形状番号", "独立(-1)後付け(0)", "親パーツ番号", "色", "内包パーツ数", "ご飯の量", "海苔のサイズ", "パーツの最下部", "ガイドライン付き画像"], "w")      
div_parts(imagename)

#sobel(imagename)
#lap(imagename)
#canny(imagename)

#img2bw(imagename)
