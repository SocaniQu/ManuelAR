using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using TMPro;
using UnityEngine.UI;

public class QRScanner : MonoBehaviour
{
    [SerializeField]
    private RawImage rawBackground;
    [SerializeField]
    private AspectRatioFitter ratioFitter;
    [SerializeField]
    private TextMeshProUGUI outputText;
    [SerializeField]
    private RectTransform scanZone;

    private bool isCamAvailable;
    [SerializeField]
    private WebCamTexture camTexture;

    void Start(){
        SetUpCamera();
    }

    void Update(){
        UpdateCamera();
        Scan();
    }

    private void SetUpCamera(){
        WebCamDevice[] devices = WebCamTexture.devices;
        if(devices.Length == 0){
            isCamAvailable = false;
            Debug.Log("Camera Unavailable!");
            return;
        }
        for(int i = 0; i <devices.Length; i++){
            if(devices[i].isFrontFacing==false){
                camTexture = new WebCamTexture(devices[i].name, (int)scanZone.rect.width, (int)scanZone.rect.height);
            }
        }
        camTexture.Play();
        rawBackground.texture = camTexture;
        isCamAvailable = true;
    }

    private void UpdateCamera(){
        if(isCamAvailable == false){
            return;
        }
        float ratio = (float)camTexture.width / (float)camTexture.height;
        ratioFitter.aspectRatio = ratio;

        int orientation = -camTexture.videoRotationAngle;
        rawBackground.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }

    private void Scan(){
        try{
            BarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
            if(result != null){
                outputText.text = result.Text;
            }else{
                outputText.text = "ERROR: NO QR DETECTED";
            }
        }catch{}
    }
}
