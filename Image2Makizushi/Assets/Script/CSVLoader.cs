using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class CSVLoader : MonoBehaviour
{

	public GuideData guideData;

	// Use this for initialization
	void Start()
	{
		guideData = new GuideData();
	}


	void LoadCSV()
	{
		string filePath = Application.dataPath + @"\Scripts\File\test.txt";

		using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8))
		{
			while (!streamReader.EndOfStream)
			{
				Debug.Log("ストリームで読み込み：" + streamReader.ReadLine());
			}
		}
	}

}
