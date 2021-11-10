import cv2
import numpy as np

img = cv2.imread("uho.jpeg")
img_gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
ret, img_2 = cv2.threshold(img_gray, 128, 255, cv2.THRESH_OTSU)
contours, _ = cv2.findContours(img_2, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
print(contours)
for i in range(11):
    img_contour = cv2.drawContours(img, contours, i, (0,255,0), 3)
    cv2.imwrite("uhhohoi" + str(i)+ ".jpg", img_contour)