import cv2
from PIL import Image, ImageDraw
from functools import reduce
import csv
import pprint
import glob

imagename = "shape"

#画像の読み込み
image = Image.open(imagename + ".png")
#グレースケール化
img = cv2.cvtColor(cv2.imread(imagename + ".png"), cv2.COLOR_BGR2GRAY)
#色を反転させて2値化
_, img_2 = cv2.threshold(img, 128, 255, cv2.THRESH_BINARY_INV)
#輪郭抽出
contours, _ = cv2.findContours(img_2, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
#小さい領域を削除
contours = list(filter(lambda x: cv2.contourArea(x) > 10, contours))
contour = contours[0]
# 輪郭データをタプルに
area = tuple(map(lambda c: tuple(c[0]), contour.tolist()))

#--------マスク画像--------#
#黒塗り画像
mask = Image.new("L", image.size, 0)
#白塗りの輪郭に沿った図形を貼り付け
ImageDraw.Draw(mask).polygon(area, fill=255)
#元画像にアルファマットを重ねる
copy = image.copy()
copy.putalpha(mask)
#切り抜き
box = reduce(lambda p, c: [
    min(p[0], c[0]), min(p[1], c[1]),
    max(p[2], c[0]), max(p[3], c[1])
], area, [image.width, image.height, 0, 0])
bbox = [box[0]-5, box[1]-5, box[2]+5, box[3]+5]
output = copy.crop(bbox)
output.save(imagename + '0.png')

#---------輪郭を取得----------
def find_contours(imagename):
    #画像の取得
    img = cv2.imread(imagename  + ".png")
    #グレースケール化
    img_gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    #色を反転させて2値化
    _, img_bw_before = cv2.threshold(img_gray, 128, 255, cv2.THRESH_BINARY_INV)
    #最外輪郭抽出
    contours, _ = cv2.findContours(img_bw_before, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    #最外輪郭を削除（背景と同色に塗りつぶす）
    img_bw = cv2.drawContours(img_bw_before, contours, -1, color=(0,0,0), thickness=2)
    #輪郭抽出
    contours, _ = cv2.findContours(img_bw, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    #小さい領域を削除
    contours = list(filter(lambda x: cv2.contourArea(x) > 10, contours))
    if len(contours) == 0:
        #内包パーツがなかったら0を返す
        return 0
    #内包パーツがあったら、内包パーツの輪郭データを返す
    return contours

#---------パーツ分割----------
def div_parts(imagename):
    image = Image.open(imagename + ".png")
    #輪郭を取得
    contours = find_contours(imagename)
    if contours == 0:
        #内包パーツがなかったら次のパーツへ
        return 0

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
        bbox = [box[0]-10, box[1]-10, box[2]+10, box[3]+10]
        output = copy.crop(bbox)
        output.save(imagename + str(i+1) + '.png')
        div_parts(imagename + str(i+1))

div_parts(imagename + '0')

#-------csv書き込み--------
def csv_w(filename):
    parts_image = Image.open(filename)
    with open('parts.csv', 'w') as f:
        writer = csv.writer(f)
        writer.writerow([filename, parts_image])

partsimage_list = glob.glob('*.png')

for i in partsimage_list:
    csv_w(i)


