using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public OVERALL_STATUS oVERALL_STATUS;

    public CSVLoader CSVLoader;

    private GuideData guideData;



    public List<GameObject> setupSlide = new List<GameObject>(); //独立パーツ番号

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
                        break;

                    default:
                        stateindex = 0;
                        oVERALL_STATUS = OVERALL_STATUS.INDEPENDENCE_PARTS;
                        Actions();
                        break;
                }


                break;

            case OVERALL_STATUS.INDEPENDENCE_PARTS:

                switch (stateindex)
                {
                    case 1:
                        Debug.Log("処理2-1");
                        break;

                    case 2:
                        Debug.Log("処理2-2");

                        break;

                    default:
                        stateindex = 0;
                        stateindid += 1;
                        Actions();
                        break;
                }

                if(stateindid > guideData.ind_id.Count)
                {
                    oVERALL_STATUS = OVERALL_STATUS.PARENT_PARTS;
                }

                break;

            case OVERALL_STATUS.PARENT_PARTS:
                switch (stateindex)
                {
                    case 1:
                        Debug.Log("処理2-1");
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

            case OVERALL_STATUS.FINISH:
                switch (stateindex)
                {
                    case 1:
                        Debug.Log("処理2-1");
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
}
