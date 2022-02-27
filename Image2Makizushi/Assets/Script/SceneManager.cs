using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


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

    public bool flaggg=false;
    public OVERALL_STATUS oVERALL_STATUS;

    public CSVLoader CSVLoader;


    [SerializeField]
    private GuideData guideData;


    //slide
    public List<GameObject> setupSlide = new List<GameObject>(); //独立パーツ番号
    public List<GameObject> indSlide = new List<GameObject>(); //独立パーツ番号
    public List<GameObject> parentSlide = new List<GameObject>(); //独立パーツ番号



    public GameObject finfinslide;
    public GameObject finishslide;



    //UI


    public Text indidtext;

    public Text noriinfotext;
    public Text volumeinfotext;



    public Text noritext;
    public Text volumetext;

    public Text pidtext;
    public Text pidtext2;
    public Text pidtext3;

    public Text pnoritext;
    public Text pvolumetext;
    public Text ppartstext;

    public Image indinfoimg;

    public Image cut1;
    public Image cut2;
    public Sprite defoimg;

    public Image parentinfoimg;

    public List<Image> colorlist = new List<Image>(); //独立パーツ番号
    public List<Image> imlist = new List<Image>();


    public List<Image> norilist = new List<Image>();

    public List<Text> vtex = new List<Text>();

    public Image indColor;
    public Image PColor;

    public Image ppimg;

    public Image ppimg2;
    public Image ppimg3;
    public Text pptext;

    public Image ppimg4;


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
                        string t = "";
                        for (int k = 0; k < guideData.parent_noriwidth.Count; k++)
                        {
                            bool fl = false;
                            for (int l = 0; l < k; l++)
                            {
                                if(guideData.parent_noriheight[k] == guideData.parent_noriheight[l])
                                {
                                    fl = true;
                                }
                            }

                            if (fl)
                            {
                                break;
                            }


                                t += guideData.parent_noriheight[k].ToString();


                            int nm = guideData.parent_noriheight.Count(s => s.Contains(guideData.parent_noriheight[k].ToString()))+ guideData.ind_noriheight.Count(s => s.Contains(guideData.parent_noriheight[k].ToString()));

                            t += " " + nm + " 枚";
                            t += "\n";

                            //Debug.Log(guideData.parent_indid[j - 1][k]);
                        }


                        for (int k = 0; k < guideData.ind_noriwidth.Count; k++)
                        {
                            bool fl = false;
                            for (int l = 0; l < guideData.parent_noriwidth.Count; l++)
                            {
                                if (guideData.ind_noriheight[k] == guideData.parent_noriheight[l])
                                {
                                    fl = true;
                                    Debug.Log("oya:" + guideData.parent_noriheight[k]);
                                }
                            }

                            for (int l = 0; l < k; l++)
                            {
                                if (guideData.ind_noriheight[k] == guideData.ind_noriheight[l])
                                {
                                    fl = true;
                                    Debug.Log("dokuritu:"+guideData.ind_noriheight[k]);
                                }
                            }

                            if (fl)
                            {
                                Debug.Log("break"+ guideData.ind_noriheight[k]);
                                continue;
                            }

                            Debug.Log("roop"+k);

                            t += guideData.ind_noriheight[k].ToString();


                            int nm = guideData.parent_noriheight.Count(s => s.Contains(guideData.ind_noriheight[k].ToString())) + guideData.ind_noriheight.Count(s => s.Contains(guideData.ind_noriheight[k].ToString()));

                            t += " " + nm + " 枚";
                            t += "\n";


                            //Debug.Log(guideData.parent_indid[j - 1][k]);
                        }



                        noriinfotext.text = t;


                        int n = 0;
                        t = "";
                        for (int k = 0; k < guideData.comp_rice.Count; k++)
                        {
                            t += guideData.comp_rice[k].ToString();

                            t += "g\n";


                            colorlist[k].color = guideData.comp_color[k];
                            n++;
                        }

                        t += "------------------\n";
                        t += "合計　　" + guideData.comp_rice.Sum().ToString()+"g";
                        /* for (int k = 0; k < guideData.parent_noriheight.Count; k++)
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
                         }*/

                        volumeinfotext.text = t;
                        int rn=0;
                        for (int k = 0; k < guideData.ind_image.Count; k++) {
                            Debug.Log("k"+k);
                            Debug.Log(guideData.ind_image.Count - k - 1);
                            imlist[k].sprite = Sprite.Create((Texture2D)guideData.ind_image[guideData.ind_image.Count - k-1], new Rect(0, 0, guideData.ind_image[guideData.ind_image.Count - k-1].width, guideData.ind_image[guideData.ind_image.Count - k-1].height), Vector2.zero);
                            //imlist[k].sprite = Sprite.Create((Texture2D)guideData.ind_image[0], new Rect(0, 0, guideData.ind_image[0].width, guideData.ind_image[0].height), Vector2.zero);
                            vtex[k].text = guideData.ind_noriwidth[guideData.ind_image.Count - k - 1].ToString()+"g";
                            rn = k;
                        }


                        for (int k = 0; k < guideData.parent_image.Count; k++)
                        {

                            imlist[k+rn+1].sprite = Sprite.Create((Texture2D)guideData.parent_image[guideData.parent_image.Count - k - 1], new Rect(0, 0, guideData.parent_image[guideData.parent_image.Count - k - 1].width, guideData.parent_image[guideData.parent_image.Count - k - 1].height), Vector2.zero);
                            //imlist[k].sprite = Sprite.Create((Texture2D)guideData.ind_image[0], new Rect(0, 0, guideData.ind_image[0].width, guideData.ind_image[0].height), Vector2.zero);
                            vtex[k + rn + 1].text = guideData.parent_noriwidth[guideData.parent_image.Count - k - 1].ToString() + "g";

                        }
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
                          indSlide[stateindex].SetActive(false);
                        indinfoimg.sprite = Sprite.Create((Texture2D)guideData.ind_image[i-1], new Rect(0, 0, guideData.ind_image[i - 1].width, guideData.ind_image[i - 1].height), Vector2.zero);

                        noritext.text = "のり：" + guideData.ind_noriheight[i-1]+"";
                        volumetext.text = "ごはん："+guideData.ind_noriwidth[i-1]+"g";
                        indColor.color = guideData.ind_color[i - 1];

                        if (guideData.ind_shapeid[i - 1] == 4)
                        {
                            //半切り
                            indinfoimg.sprite = defoimg;
                            indinfoimg.color = guideData.ind_color[i - 1];
                           
                        }
                        else
                        {
                            stateindex += 100;
                        }
                        break;

                    case 2:
                        indSlide[stateindex - 1].SetActive(true);
                        indSlide[stateindex - 2].SetActive(false);
                        cut1.color = guideData.ind_color[i - 1];
                        cut2.color = guideData.ind_color[i - 1];


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

                pidtext.text = "親パーツ作成" + (stateparid+1).ToString() + " / " + guideData.parent_id.Count;
                pidtext2.text = "親パーツ作成" + (stateparid + 1).ToString() + " / " + guideData.parent_id.Count;
                pidtext3.text = "親パーツ作成" + (stateparid + 1).ToString() + " / " + guideData.parent_id.Count;

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

                    if(pcount == guideData.parent_num[j - 1] - 1&& !flaggg)
                    {
                        stateindex = 5;
                        flaggg = true;
                        //pcount = -10;
                    }
                }
                switch (stateindex)
                {
                    case 1:
                        Debug.Log("処理3-1");
                        parentSlide[parentSlide.Count-1].SetActive(false);
                        parentSlide[stateindex - 1].SetActive(true);
                        parentSlide[stateindex].SetActive(false);
                        parentSlide[2].SetActive(false);

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
                        if (true) {
                            ppimg3.sprite = Sprite.Create((Texture2D)guideData.ind_image[pcount], new Rect(0, 0, guideData.ind_image[pcount].width, guideData.ind_image[pcount].height), Vector2.zero);
                        }
                        else
                        {
                            ppimg3.sprite = Sprite.Create((Texture2D)guideData.parent_image[pcount], new Rect(0, 0, guideData.ind_image[pcount].width, guideData.ind_image[pcount].height), Vector2.zero);
                        }
                        break;

                    case 3:
                        Debug.Log("処理3-3");
                        parentSlide[stateindex - 2].SetActive(false);
                        parentSlide[stateindex-1].SetActive(true);
                        parentSlide[3].SetActive(false);

                        rollimg.sprite = Sprite.Create((Texture2D)guideData.parent_image[j - 1], new Rect(0, 0, guideData.parent_image[j - 1].width, guideData.parent_image[j - 1].height), Vector2.zero);
                        break;

                    case 5:
                        Debug.Log("処理3-4");
                        parentSlide[stateindex - 4].SetActive(false);
                        parentSlide[stateindex - 2].SetActive(true);
                        stateindex = 2;
                        ppimg4.sprite = Sprite.Create((Texture2D)guideData.parent_image[j - 1], new Rect(0, 0, guideData.parent_image[j - 1].width, guideData.parent_image[j - 1].height), Vector2.zero);


                        //rollimg.sprite = Sprite.Create((Texture2D)guideData.parent_image[j - 1], new Rect(0, 0, guideData.parent_image[j - 1].width, guideData.parent_image[j - 1].height), Vector2.zero);
                        break;

                    default:
                        stateindex = 0;
                        stateparid += 1;
                        flaggg = false;
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
                        finfinslide.SetActive(true);

                        for (int k = 0; k < guideData.nori_image.Count; k++)
                        {

                            norilist[k].sprite = Sprite.Create((Texture2D)guideData.nori_image[k], new Rect(0, 0, guideData.nori_image[k].width, guideData.nori_image[k].height), Vector2.zero);
                        }
                        Debug.Log("Finish");
                        break;

                    case 2:
                        Debug.Log("処理2-2");
                        finfinslide.SetActive(false);
                        finishslide.SetActive(true);
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
