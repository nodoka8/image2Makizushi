using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SceneManager : MonoBehaviour
{

    public enum OVERALL_STATUS
    {
        SETUP=0,
        INDEPENDENCE_PARTS=1,
        PARENT_PARTS=2,
        FINISH=3,
    }


    public enum PARENT_STATUS
    {
        SETUP,
        INDEPENDENCE_PARTS,
        PARENT_PARTS,
        FINISH,
    }

    public int stateindex;

    public int stateindid;
    public int stateparid;


    public OVERALL_STATUS oVERALL_STATUS;

    public CSVLoader CSVLoader;


    [SerializeField]
    private GuideData guideData;


    //slide
    public List<GameObject> setupSlide = new List<GameObject>(); //独立パーツ番号
    public List<GameObject> indSlide = new List<GameObject>(); //独立パーツ番号
    public List<GameObject> parentSlide = new List<GameObject>(); //独立パーツ番号
    public GameObject finishslide;



    //UI


    public Text indidtext;

    public Text noriinfotext;
    public Text volumeinfotext;



    public Text noritext;
    public Text volumetext;

    public Text pidtext;
    public Text pnoritext;
    public Text pvolumetext;
    public Text ppartstext;

    public Image indinfoimg;


    public Image parentinfoimg;

    public List<Image> colorlist = new List<Image>(); //独立パーツ番号

    public Image indColor;
    public Image PColor;

    public Image ppimg;

    public Image ppimg2;

    public Image rollimg;
    int pcount = 0;
    // Start is called before the first frame update
    void Start()
    {
        guideData = CSVLoader.guideData;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Actions();
        }
    }

	public void Actions()
	{
        stateindex += 1;

        switch (oVERALL_STATUS)
        {
            case OVERALL_STATUS.SETUP:

                switch(stateindex)
                {
                    case 1:
                        Debug.Log("処理1");
                        setupSlide[stateindex-1].SetActive(false);
                        setupSlide[stateindex].SetActive(true);
                        break;

                    case 2:
                        Debug.Log("処理2");
                        setupSlide[stateindex - 1].SetActive(false);
                        setupSlide[stateindex].SetActive(true);

                        // noriinfotext.text = 
                        string t = "必要なのり:";
                        for (int k = 0; k < guideData.parent_noriwidth.Count; k++)
                        {
                            t += guideData.parent_noriheight[k].ToString();


                            t += "\n";

                            //Debug.Log(guideData.parent_indid[j - 1][k]);
                        }


                        for (int k = 0; k < guideData.ind_noriwidth.Count; k++)
                        {
                            t += guideData.ind_noriheight[k].ToString();
                            t += "\n";


                            //Debug.Log(guideData.parent_indid[j - 1][k]);
                        }

                        noriinfotext.text = t;


                        int n = 0;


                        t = "必要なごはん:";
                        for (int k = 0; k < guideData.parent_noriheight.Count; k++)
                        {
                            t += guideData.parent_noriwidth[k].ToString();

                            t += "g\n";


                            colorlist[n].color = guideData.parent_color[k];
                            n++;
                        }

                        for (int k = 0; k < guideData.ind_noriheight.Count; k++)
                        {
                            t += guideData.ind_noriwidth[k].ToString();

                            t += "g\n";


                            colorlist[n].color = guideData.ind_color[k];
                            n++;
                        }

                        volumeinfotext.text = t;
                        break;

                    default:
                        setupSlide[stateindex - 1].SetActive(false);
                        stateindex = 0;
                        oVERALL_STATUS = OVERALL_STATUS.INDEPENDENCE_PARTS;
                        Actions();
                        break;
                }


                break;

            case OVERALL_STATUS.INDEPENDENCE_PARTS:

                int i = stateindid + 1;

                indidtext.text = "独立パーツ作成" + i.ToString() + " / " + guideData.ind_id.Count;

                switch (stateindex)
                {
                    case 1:
                        Debug.Log("処理2-1");
                        indSlide[stateindex - 1].SetActive(true);
                        //  indSlide[stateindex].SetActive(true);
                        indinfoimg.sprite = Sprite.Create((Texture2D)guideData.ind_image[i-1], new Rect(0, 0, guideData.ind_image[i - 1].width, guideData.ind_image[i - 1].height), Vector2.zero);

                        noritext.text = "のり：" + guideData.ind_noriheight[i-1]+"";
                        volumetext.text = "ごはん："+guideData.ind_noriwidth[i-1]+"g";
                        indColor.color = guideData.ind_color[i - 1];
                        break;


                    default:
                        stateindex = 0;
                        stateindid += 1;
                        Actions();
                        break;
                }

                if(stateindid > guideData.ind_id.Count-2)
                //if (stateindid > 2)
                {
                    Debug.Log("aaaaaaaaaaaaaaaaaa");
                    stateindex = 0;
                    oVERALL_STATUS = OVERALL_STATUS.PARENT_PARTS;
                }

                break;

            case OVERALL_STATUS.PARENT_PARTS:
                indSlide[0].SetActive(false);
                int j = guideData.parent_id.Count - stateparid;
                pidtext.text = "親パーツ作成" + j.ToString() + " / " + guideData.parent_id.Count;
                if (stateindex == 3)
                {
                    if (pcount < guideData.parent_num[j - 1]-1)
                    {
                        stateindex = 1;
                        Debug.Log("内包パーツ取付" + pcount.ToString());
                        pcount++;
                    }
                    else
                    {
                        pcount = 0;
                    }
                }
                switch (stateindex)
                {
                    case 1:
                        Debug.Log("処理3-1");
                        parentSlide[parentSlide.Count-1].SetActive(false);
                        parentSlide[stateindex - 1].SetActive(true);
                        parentSlide[stateindex].SetActive(false);

                        pnoritext.text = "のり：" + guideData.parent_noriheight[j - 1] + "";
                        pvolumetext.text = "ごはん：" + guideData.parent_noriwidth[j - 1] + "g";
                        PColor.color = guideData.parent_color[j - 1];
                        ppimg.sprite = Sprite.Create((Texture2D)guideData.parent_img2[j-1][pcount], new Rect(0, 0, guideData.parent_img2[j - 1][pcount].width, guideData.parent_img2[j - 1][pcount].height), Vector2.zero);
                        string t = "使用するパーツ:";
                        for(int k=0; k < guideData.parent_indid[j - 1].Count; k++)
                        {
                            t += guideData.parent_indid[j - 1][k].ToString();
                            t += ",";
                            Debug.Log(guideData.parent_indid[j - 1][k]);
                        }
                       // ppartstext.text = t;

                        parentinfoimg.sprite = Sprite.Create((Texture2D)guideData.parent_image[j - 1], new Rect(0, 0, guideData.parent_image[j - 1].width, guideData.parent_image[j - 1].height), Vector2.zero);

                        break;

                    case 2:
                        Debug.Log("処理3-2");
                        parentSlide[stateindex - 2].SetActive(false);
                        parentSlide[stateindex-1].SetActive(true);
                        ppimg2.sprite = Sprite.Create((Texture2D)guideData.parent_img2[j - 1][pcount], new Rect(0, 0, guideData.parent_img2[j - 1][pcount].width, guideData.parent_img2[j - 1][pcount].height), Vector2.zero);

                        break;

                    case 3:
                        Debug.Log("処理3-3");
                        parentSlide[stateindex - 2].SetActive(false);
                        parentSlide[stateindex-1].SetActive(true);
                        rollimg.sprite = Sprite.Create((Texture2D)guideData.parent_image[j - 1], new Rect(0, 0, guideData.parent_image[j - 1].width, guideData.parent_image[j - 1].height), Vector2.zero);
                        break;

                    default:
                        stateindex = 0;
                        stateparid += 1;
                        //oVERALL_STATUS = OVERALL_STATUS.FINISH;
                        if (stateparid > guideData.parent_id.Count - 1)
                        //if (stateindid > 2)
                        {
                            Debug.Log("bbbbbbbbbb");
                            stateindex = 0;
                            oVERALL_STATUS = OVERALL_STATUS.FINISH;
                        }

                        Actions();
                        break;
                }

                if (stateparid > guideData.parent_id.Count-1)
                //if (stateindid > 2)
                {
                    Debug.Log("bbbbbbbbbb");
                    stateindex = 0;
                    oVERALL_STATUS = OVERALL_STATUS.FINISH;
                }

                break;

            case OVERALL_STATUS.FINISH:
                parentSlide[2].SetActive(false);
                switch (stateindex)
                {
                    case 1:
                        finishslide.SetActive(true);
                        Debug.Log("Finish");
                        break;

                    case 2:
                        Debug.Log("処理2-2");

                        break;

                    default:
                        stateindex = 0;
                        oVERALL_STATUS = OVERALL_STATUS.FINISH;
                        Actions();
                        break;
                }

                break;
        }


    }

    void texserch(Texture2D tex)
    {
        Color32[] colors = new Color32[tex.width * tex.height]; ;

        Color color;
        int width = tex.width;
        int height = tex.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                color = tex.GetPixel(x,y);

                if(color.r == 0)
                {
                    Debug.Log("p1");
                }
                else
                {
                    Debug.Log("p2");
                }
            }
        }
    }
}
