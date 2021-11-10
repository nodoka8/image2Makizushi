import cv2
from PIL import Image, ImageDraw
from functools import reduce
import numpy as np  

# 外側の輪郭を取得
def find_contours(name):
    im = cv2.imread(name, -1)
    im_gray = cv2.cvtColor(im, cv2.COLOR_BGR2GRAY)
    _, im_bw = cv2.threshold(im_gray, 128, 255, cv2.THRESH_BINARY_INV)
    cv2.imwrite("im_bw.png", im_bw)
    contours, _ = cv2.findContours(im_bw, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    return contours

filename = "shape_1.png"
image = Image.open(filename)
contours = find_contours(filename)

for i, contour in enumerate(contours):
    # 輪郭データをタプルに
    area = tuple(map(lambda c: tuple(c[0]), contour.tolist()))
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
    print(bbox)
    output = copy.crop(bbox)
    output.save('shape_' + str(i+2) + '.png')