using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using TMPro;
using UnityEngine.UI;

public class QRScanner : MonoBehaviour
{
    /// <summary>
    /// handles the detection of QR codes in the scan area, activates the AR session
    /// and updates the product display to the associated product if a QR code matches
    /// 
    /// NOTE: currently the QR code scanner is using the product name as a unique identifier
    /// if it gets to the point where there are many similar products, it will be wise to
    /// update this code to use unique product identifiers, this will require generate of new
    /// associated QR codes
    /// 
    /// current editor tools (AR Fashion/Generate New Product) will set the name of the generated
    /// QR code to the data contained inside (with no white spaces)
    /// </summary>


    [SerializeField]
    private RawImage rawBackground;
    [SerializeField]
    private AspectRatioFitter ratioFitter;
    [SerializeField]
    private RectTransform scanZone;

    [SerializeField]
    private ProductDisplay display;

    private bool isCamAvailable;
    private WebCamTexture camTexture;

    void Start(){
        SetUpCamera();
    }

    void Update(){
        UpdateCamera();
        Scan();
    }

    private void SetUpCamera(){
        //set the output image to the back facing camera
        WebCamDevice[] devices = WebCamTexture.devices;
        if(devices.Length == 0){
            isCamAvailable = false;
            return;
        }
        camTexture = new WebCamTexture(devices[0].name, (int)scanZone.rect.width, (int)scanZone.rect.height);
        camTexture.Play();
        rawBackground.texture = camTexture;
        isCamAvailable = true;
    }

    private void UpdateCamera(){
        if(isCamAvailable == false){
            return;
        }
        //make sure the output texture, and the camera remain to the proper size
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
                CompareQRData(result.Text);
            }
        }catch{
            Debug.Log("QR SCANNER ERROR");
        }
    }

    //loads the product data, and compares the qr code value to the product data name
    private void CompareQRData(string input){
        string productFilePath = "Products/Product Data";
        ProductData[] data = Resources.LoadAll<ProductData>(productFilePath);
        foreach(ProductData current in data){
            if(current.productName.Equals(input)){
                Debug.Log("Product Seen");
                display.gameObject.SetActive(true);
                display.LoadNewProduct(current);
                this.gameObject.SetActive(false);
            }
        }
    }
}
