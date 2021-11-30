def xy_to_a(x, y, width):
    return width*y+x

def a_to_y(a, width):
    return int(a/width)

def a_to_x(a, width):
    return int(a%width)

def u_to_list(u, width):
    r1 = xy_to_a(a_to_x(u, width)-1, a_to_y(u, width), width)
    r2 = xy_to_a(a_to_x(u, width)+1, a_to_y(u, width), width)
    r3 = xy_to_a(a_to_x(u, width), a_to_y(u, width)+1, width)
    r4 = xy_to_a(a_to_x(u, width), a_to_y(u, width)-1, width)
    return [r1, r2, r3, r4]

def flood(x, y, c, width, height, image): 
    W = {xy_to_a(x, y, width)} #Wを集合にするお！！（Wへの判定をなくすため）
    while len(W) != 0:
        u = W.pop() 
        image[a_to_x(u, width), a_to_y(u, width)] = c
        for v in u_to_list(u, width):
            if v>=0 and v<width*height and image[a_to_x(v, width), a_to_y(v, width)] == 0:
                W.add(v)
    return 0