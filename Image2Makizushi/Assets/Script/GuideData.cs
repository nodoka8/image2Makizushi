using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideData : MonoBehaviour
{

    public Image inputimage;
    public Image outputimage;

    public List<int> ind_id = new List<int>(); //独立パーツ番号
    public List<Color> ind_color = new List<Color>(); //独立パーツカラー
    public List<Image> ind_image = new List<Image>(); //独立パーツ画像
    public List<float> ind_noriwidth = new List<float>(); //独立パーツ海苔横
    public List<float> ind_noriheight = new List<float>(); //独立パーツ海苔縦

    public List<int>　parent_id = new List<int>(); //親パーツ番号
    public List<Image> parent_image = new List<Image>(); //親パーツ画像
    public List<Image> parent_idimage = new List<Image>(); //パーツID変換画像

    public List<float> parent_noriwidth = new List<float>(); //親パーツ海苔横
    public List<float> parent_noriheight = new List<float>(); //親パーツ海苔縦


    public GuideData()
    {

    }

    public int GetNoriNum()
    { 
        return ind_noriheight.Count + parent_noriheight.Count;
    }
}
