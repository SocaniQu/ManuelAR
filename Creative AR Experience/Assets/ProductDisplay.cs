using System;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    };

    public WorldIcons[] icons;

    public Dictionary<string, GameObject> worldSpaceIcons = new Dictionary<string, GameObject>();

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

        this.transform.GetChild(1).GetComponent<Canvas>().worldCamera = GameObject.Find("AR Camera").GetComponent<Camera>();

        foreach(var icon in icons){
            try{
                worldSpaceIcons.Add(icon.name, icon.obj);
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
        }else{
            currentDisplay = (DisplayState)DisplayState.Parse(typeof(DisplayState), current);
            StartCoroutine(LoadNewObject(worldSpaceIcons[current]));
        }
    }

    /*
    private void SwapOldToNew(){
        //yield return new WaitForSeconds(0.5f);
        //if(prevCon.GetChild(0) != null)
        if(prevCon.transform.childCount != 0) Destroy(prevCon.transform.GetChild(0).gameObject);
        prevCon.transform.localScale = new Vector3(1,1,1);
        newCon.transform.GetChild(0).SetParent(prevCon.transform, false);
        newCon.transform.localScale = Vector3.zero;
    }*/
}
