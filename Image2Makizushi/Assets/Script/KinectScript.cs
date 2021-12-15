using Microsoft.Azure.Kinect.Sensor;
using System.Threading.Tasks;
using UnityEngine;

public class KinectScript : MonoBehaviour
{
    private Device _kinectDevice = null;

    [SerializeField] private UnityEngine.UI.RawImage _viewerRawImage = null;

    [SerializeField] private UnityEngine.UI.RawImage testRawImage = null;

    [SerializeField] private int _depthDistanceMin = 200;
    [SerializeField] private int _depthDistanceMax = 3000;

    private int width = 500;
    private int height = 500;
    public Texture2D texr;
    private Color color;


    private Color32[] colors;
    private int length;

    private int maxval=0;
    private int minval=1000;
    private void Start()
    {

       // texr = new Texture2D(1, 1, TextureFormat.RGB24, false);

        Init();
        StartLoop();
    }

    private void OnDestroy()
    {
        _kinectDevice.StopCameras();  // Kinectの終了処理
    }

    private void Init()
    {
        _kinectDevice = Device.Open(0);
        _kinectDevice.StartCameras(new DeviceConfiguration
        {
            ColorFormat = ImageFormat.ColorBGRA32,
            ColorResolution = ColorResolution.R1080p,
            DepthMode = DepthMode.NFOV_2x2Binned,
            SynchronizedImagesOnly = true,
            CameraFPS = FPS.FPS30
        });  // Kinectの開始処理。設定はお好みで。
    }

    private async void StartLoop()
    {
        while (true)
        {
            using (Capture capture = await Task.Run(() => _kinectDevice.GetCapture()).ConfigureAwait(true))
            {
                // 必要な情報を用意
                Image depthImage = capture.Depth;
                int pixelWidth = depthImage.WidthPixels;
                int pixelHeight = depthImage.HeightPixels;
                ushort[] depthByteArr = depthImage.GetPixels<ushort>().ToArray();
                Color32[] colorArr = new Color32[depthByteArr.Length];
               // colors = new Color32[depthByteArr.Length];

                // ushort配列 => Color32配列
                for (int i = 0; i < colorArr.Length; i++)
                {
                    int index = colorArr.Length - 1 - i;

                    int depthVal = 255 - (255 * (depthByteArr[index] - _depthDistanceMin) / _depthDistanceMax);  // 近いほど値が大きくなるよう計算
                    if (depthVal < 0)
                    {
                        depthVal = 0;
                    }
                    else if (depthVal > 255)
                    {
                        depthVal = 255;
                    }

                    Debug.Log(Mathf.Max(depthVal));

                    colorArr[i] = new Color32(
                        (byte)depthVal,
                        (byte)depthVal,
                        (byte)depthVal,
                        255
                    );
                }
              //  colors = colorArr;
                // Texture2Dの作成
                Texture2D resultTex = new Texture2D(pixelWidth, pixelHeight);
                resultTex.SetPixels32(colorArr);
                resultTex.Apply();
               // texr = resultTex;
                // RawImageの更新
                _viewerRawImage.texture = GetTexture2D(pixelWidth, pixelHeight, colorArr);
                _viewerRawImage.rectTransform.sizeDelta = new Vector2(width, width); // rectTransformのサイズ変更
            }
        }
    }


        // カラー配列 -> Texture2D
        private Texture2D GetTexture2D(int width, int height, Color32[] colorArr)
    {
        _viewerRawImage.rectTransform.sizeDelta = new Vector2(width, height);

        Texture2D resultTex = new Texture2D(width, height);
        resultTex.SetPixels32(colorArr);
        resultTex.Apply();
        return resultTex;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            click();
        }


    }

    void click()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            int j = colors[i].r;

            if (j > maxval) maxval = j;
            if (j < minval) minval = j;
        }

        Debug.Log("min:" + minval.ToString());
        Debug.Log("max:" + maxval.ToString());

    }

}
