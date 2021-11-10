from PIL import Image
import csv
import glob

#-------csv書き込み--------
def csv_w(filename):
    with open('parts.csv', 'w') as f:
        writer = csv.writer(f)
        for i in partsimage_list:
            parts_image = Image.open(i)
            writer.writerow([i, parts_image])

partsimage_list = glob.glob('*.png')
print(partsimage_list)

csv_w(partsimage_list)