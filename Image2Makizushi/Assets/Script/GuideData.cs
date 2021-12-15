using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class GuideData
{

    public Image inputimage;
    public Image outputimage;

    public List<int> ind_id = new List<int>(); //独立パーツ番号
    public List<int> ind_shapeid = new List<int>(); //独立パーツ形状番号
    public List<Color> ind_color = new List<Color>(); //独立パーツカラー
    public List<Texture> ind_image = new List<Texture>(); //独立パーツ画像
    public List<float> ind_noriwidth = new List<float>(); //独立パーツご飯量
    public List<string> ind_noriheight = new List<string>(); //独立パーツ海苔縦

    public List<int> ind_y = new List<int>(); //独立パーツ形状番号


    public List<int>　parent_id = new List<int>(); //親パーツ番号
    public List<Texture2D> parent_image = new List<Texture2D>(); //親パーツ画像
    public List<Texture2D> parent_idimage = new List<Texture2D>(); //パーツID変換画像
    public List<Color> parent_color = new List<Color>(); //独立パーツカラー

    public List<int> parent_num = new List<int>(); //内包パーツ数
    public List<List<int>> parent_indid = new List<List<int>>();//内包独立パーツ番号


    public List<List<Texture2D>> parent_img2 = new List<List<Texture2D>>();//内包独立パーツ番画像


    public List<float> parent_noriwidth = new List<float>(); //親パーツご飯量
    public List<string> parent_noriheight = new List<string>(); //親パーツ海苔縦

    public List<int> parent_y = new List<int>(); //独立パーツ形状番号




    public GuideData()
    {

    }

    public int GetNoriNum()
    { 
        return ind_noriheight.Count + parent_noriheight.Count;
    }
}
