#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using ZXing;
using ZXing.QrCode;

public class QRGenerator : MonoBehaviour
{
    string text;
    
    //[MenuItem("AR Fashion/Generate New QR")]
    public void GenerateQR(string encodeText){
        text = (string)encodeText;
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();

        //create png image
        byte[] bytes = encoded.EncodeToPNG();
        var dirPath = Application.dataPath + "Products/QR Codes/";
        if(!Directory.Exists(dirPath)) {
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(dirPath + "Image" + ".png", bytes);

        AssetDatabase.Refresh();
    }

    private Color32[] Encode(string textToEncode, int width, int height){
        BarcodeWriter writer = new BarcodeWriter{
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions{
                Height = height,
                Width = width
            }
        };
        return writer.Write(textToEncode);
    }
}
#endif