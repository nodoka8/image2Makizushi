using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

public class CSVLoader : MonoBehaviour
{

	public GuideData guideData;

	public TextAsset csvFile; // CSVファイル
	public List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    private string path;

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
                string readTxt = sr.ReadToEnd();
                Debug.Log(readTxt);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

}
