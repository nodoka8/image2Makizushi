using Microsoft.Azure.Kinect.Sensor;
using System.Threading.Tasks;
using UnityEngine;

public class KinectScript : MonoBehaviour
{
    private Device _kinectDevice = null;

    [SerializeField] private UnityEngine.UI.RawImage _viewerRawImage = null;

    private void Start()
    {
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
                Image colorImage = capture.Color;
                int pixelWidth = colorImage.WidthPixels;
                int pixelHeight = colorImage.HeightPixels;
                BGRA[] bgraArr = colorImage.GetPixels<BGRA>().ToArray();
                Color32[] colorArr = new Color32[bgraArr.Length];

                // BGRA配列 => Color32配列
                for (int i = 0; i < colorArr.Length; i++)
                {
                    int index = colorArr.Length - 1 - i;
                    colorArr[i] = new Color32(
                        bgraArr[index].R,
                        bgraArr[index].G,
                        bgraArr[index].B,
                        bgraArr[index].A
                    );
                }

                // Texture2Dの作成
                Texture2D resultTex = new Texture2D(pixelWidth, pixelHeight);
                resultTex.SetPixels32(colorArr);
                resultTex.Apply();

                // RawImageの更新
                _viewerRawImage.texture = GetTexture2D(pixelWidth, pixelHeight, colorArr);
                _viewerRawImage.rectTransform.sizeDelta = new Vector2(500, 500); // rectTransformのサイズ変更
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
}
