using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine.UI;

public class CSVLoader : MonoBehaviour
{
    [SerializeField]
	public GuideData guideData;

	public TextAsset csvFile; // CSVファイル
	public List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    private string path;

    public Image image;
    public RawImage img;

    // Use this for initialization
    void Start()
	{
		guideData = new GuideData();
        path = Application.dataPath + "/parts.csv";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) LoadCSV2();
    }
    void LoadCSV()
	{
        csvFile = Resources.Load("parts") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加

        }

        // csvDatas[行][列]を指定して値を自由に取り出せる
        Debug.Log(csvDatas[0][1]);
    }


    void LoadCSV2()
    {
        FileInfo fi = new FileInfo(path);
        try
        {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
              //  string readTxt = sr.ReadToEnd();
                //string[] arr = readTxt.Split(',');


                string line;
                // Read and display lines from the file until the end of
                // the file is reached.
                int itr = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] arr = line.Split(',');

                    if (itr != 1)
                    {

                            for (int i = 0; i < 13; i++)
                            {
                                Debug.Log(arr[i]);
                            }
                            if (int.Parse(arr[10]) == 0) {
                            Debug.Log("texture");

                                guideData.ind_id.Add(int.Parse(arr[0]));
                                guideData.ind_shapeid.Add(int.Parse(arr[3]));
                                /*Color color = new Color(int.Parse(arr[6]), int.Parse(arr[7]), int.Parse(arr[8]), int.Parse(arr[9]));
                                guideData.ind_color.Add(color);*/
                                guideData.ind_noriwidth.Add(float.Parse(arr[11]));
                                guideData.ind_noriheight.Add(float.Parse(arr[12]));

                                Texture2D tex = readByBinary(readPngFile(Application.dataPath + "/parts_image/" + arr[1]));
                                guideData.ind_image.Add(tex);
                            //image.sprite = null;
                                image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

                                guideData.parent_indid[int.Parse(arr[5])].Add(int.Parse(arr[0]));
                            guideData.ind_y.Add(int.Parse(arr[13]));
                            //image.overrideSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero); 
                            //Debug.Log(tex);
                                //img.texture = tex;
                            }
                            else
                            {
                                guideData.parent_id.Add(int.Parse(arr[0]));
                                guideData.parent_num.Add(int.Parse(arr[7]));
                                guideData.parent_noriwidth.Add(float.Parse(arr[11]));
                                guideData.parent_noriheight.Add(float.Parse(arr[12]));

                                Texture2D tex = readByBinary(readPngFile(Application.dataPath + "/parts_image/" + arr[1]));
                                guideData.parent_image.Add(tex);

                                Texture2D tex2 = readByBinary(readPngFile(Application.dataPath + "/IDcolor_image/" + arr[2]));
                                guideData.parent_idimage.Add(tex2);

                                List<int> l = new List<int>(); 
                                guideData.parent_indid.Add(l);
                                guideData.parent_y.Add(int.Parse(arr[13]));

                            // texserch(tex2);
                        }

                    }
                    else
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {

                            Debug.Log(arr[i]);
                        }
                    }



                    itr++;
                }

                //guideData.ind_shapeid.Add(int.Parse(arr[1]));
                //guideData.parent_num.Add(int.Parse(arr[2]));

            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public byte[] readPngFile(string path)
    {
        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            BinaryReader bin = new BinaryReader(fileStream);
            byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);
            bin.Close();
            return values;
        }
    }

    public Texture2D readByBinary(byte[] bytes)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        return texture;
    }

    void texserch(Texture2D tex)
    {
        Color color;
        int width = tex.width;
        int height = tex.height;

        Color[] pix = tex.GetPixels();
       // Color[] pix = null;


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                color = pix[(width * y) + x];
                //color = new Color() ;
                if (color.r != 0)
                {
                    Debug.Log("p1");
                }

            }
        }
    }

}
