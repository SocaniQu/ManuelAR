using System;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProductDisplay : MonoBehaviour
{
    public ProductData product;

    private Animation conAnim;
    private Transform containerParent;
    private GameObject prevCon;
    private GameObject newCon;

    [SerializableAttribute]
    public struct WorldIcons{
        public string name;
        public GameObject obj;
        public string info;
    };

    public WorldIcons[] icons;

    public Dictionary<string, GameObject> worldSpaceIcons = new Dictionary<string, GameObject>();

    public Dictionary<string, string> iconText = new Dictionary<string, string>();
    
    public GameObject textBox;
    private TextMeshProUGUI text;


    public enum DisplayState{
        product,
        fabric,
        ironing,
        washing,
        techniques,
        dye,
        community
    }

    public DisplayState currentDisplay;

    void Awake(){
        conAnim = this.GetComponent<Animation>();
        containerParent = this.transform.GetChild(0);
        prevCon = containerParent.GetChild(0).gameObject;
        newCon = containerParent.GetChild(1).gameObject;
        text = textBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        this.transform.GetChild(1).GetComponent<Canvas>().worldCamera = GameObject.Find("AR Camera").GetComponent<Camera>();

        foreach(var icon in icons){
            try{
                worldSpaceIcons.Add(icon.name, icon.obj);
                iconText.Add(icon.name, icon.info);
            }catch{
                Debug.Log("ERROR 1");
            }
        }
    }

    public void LoadNewProduct(ProductData newProd){
        product = newProd;
        StartCoroutine(LoadNewObject(product.prefab));
    }

    private IEnumerator LoadNewObject(GameObject prefab){
        var newOBJ = Instantiate(prefab);
        newOBJ.transform.SetParent(newCon.transform, false);
        newOBJ.transform.localPosition = Vector3.zero;
        //newOBJ.localRotation = newCon.rotation;
        conAnim.Play("SwapObject");
        yield return new WaitForSeconds(1.0f);
        //StartCoroutine(SwapOldToNew());
        if(prevCon.transform.childCount != 0) Destroy(prevCon.transform.GetChild(0).gameObject);
        prevCon.transform.localScale = new Vector3(1,1,1);
        newCon.transform.GetChild(0).SetParent(prevCon.transform, false);
        newCon.transform.localScale = Vector3.zero;
    }

    public void CompareDisplay(string current){
        if(currentDisplay == (DisplayState)DisplayState.Parse(typeof(DisplayState), current)){
            StartCoroutine(LoadNewObject(product.prefab));
            currentDisplay = DisplayState.product;
            UpdateText(product.productDescription);
            
        }else{
            currentDisplay = (DisplayState)DisplayState.Parse(typeof(DisplayState), current);
            StartCoroutine(LoadNewObject(worldSpaceIcons[current]));
            UpdateText(iconText[current]);
        }
    }

    private void UpdateText(string newDescription){
        text.text = newDescription;
    }
}
