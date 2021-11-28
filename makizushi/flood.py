from PIL import Image

image = Image.open("japan.png")
(width, height) = image.size

def xy_to_a(x, y, width):
    return width*y+x

def a_to_y(a, width):
    return int(a/width)

def a_to_x(a, width):
    return int(a%width)

def u_to_list(u):
    r1 = xy_to_a(a_to_x(u, width)-1, a_to_y(u, width), width)
    r2 = xy_to_a(a_to_x(u, width)+1, a_to_y(u, width), width)
    r3 = xy_to_a(a_to_x(u, width), a_to_y(u, width)+1, width)
    r4 = xy_to_a(a_to_x(u, width), a_to_y(u, width)-1, width)
    return [r1, r2, r3, r4]

def flood(x, y, c): 
    W = {xy_to_a(x, y, width)} #Wを集合にするお！！（Wへの判定をなくすため）
    while len(W) != 0:
        u = W.pop() 
        image.putpixel((a_to_x(u, width), a_to_y(u, width)), c)
        #print("M:" + str(M))
        #print("u_to_list(u):" + str(u_to_list(u)))
        for v in u_to_list(u):
            if v>=0 and v<width*height and image.getpixel((a_to_x(v, width), a_to_y(v, width))) == (255, 255, 255):
                W.add(v)
    return 0

if __name__ == '__main__':
    flood(0, 0, 0)

    image.save("a.png")