import cv2
from PIL import Image, ImageDraw
    
img1 = cv2.imread("face3.png", -1)
img2 = cv2.imread("face3.png")

cv2.imwrite("img1.png", img1)
cv2.imwrite("img2.png", img2)

img1_gray = cv2.cvtColor(img1, cv2.COLOR_BGR2GRAY)
img2_gray = cv2.cvtColor(img2, cv2.COLOR_BGR2GRAY)

cv2.imwrite("img1_gray.png", img1_gray)
cv2.imwrite("img2_gray.png", img2_gray)
