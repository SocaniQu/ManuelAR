#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using ZXing;
using ZXing.QrCode;

public class NewProductWindow : EditorWindow
{
    static string text;
    string objName;

    [MenuItem("AR Fashion/New Product")]
    static void OpenWindow(){
        NewProductWindow window = (NewProductWindow)GetWindow(typeof(NewProductWindow));
        window.minSize = new Vector2(300,150);
        window.Show();
    }

    void OnGUI(){
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Product Name");
        objName = EditorGUILayout.TextField(objName);

        if(GUILayout.Button("Generate")){
            
            GenerateQR(objName);
        }
        GUIUtility.ExitGUI();
    }

    static void GenerateQR(string encodeText){
        text = (string)encodeText;
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();

        //create png image
        byte[] bytes = encoded.EncodeToPNG();
        var dirPath = Application.dataPath + "/Products/QR Codes/";
        if(!Directory.Exists(dirPath)) {
            Directory.CreateDirectory(dirPath);
        }
        string noSpace = text.Replace(" ", string.Empty);
        File.WriteAllBytes(dirPath + noSpace + ".png", bytes);

        //create new product data
        dirPath = "Assets/Products/Product Data/";
        if(!Directory.Exists(dirPath)) {
            Directory.CreateDirectory(dirPath);
        }
        var path = "Assets/Products/Product Data/"+noSpace+".asset";
        ProductData newProduct = ScriptableObject.CreateInstance<ProductData>();
        AssetDatabase.CreateAsset(newProduct, path);
        newProduct.name = noSpace;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        newProduct.productName = text;
    }

    private static Color32[] Encode(string textToEncode, int width, int height){
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