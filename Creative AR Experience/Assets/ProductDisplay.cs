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

    public GameObject fabric;

    void Start(){
        conAnim = this.GetComponent<Animation>();
        containerParent = this.transform.GetChild(0);
        prevCon = containerParent.GetChild(0).gameObject;
        newCon = containerParent.GetChild(1).gameObject;

        this.transform.GetChild(1).GetComponent<Canvas>().worldCamera = GameObject.Find("AR Camera").GetComponent<Camera>();
    }
    
    void OnEnable()
    {
        if(product != null){

        }
    }

    void Update(){

    }

    public void LoadNewProduct(ProductData newProd){
        product = newProd;
        StartCoroutine(LoadNewObject(product.prefab));
    }

    public void FabricCall(){
        StartCoroutine(LoadNewObject(fabric));
    }

    public void IroningCall(){

    }

    public void WashingCall(){

    }

    public void TechniquesCall(){

    }

    public void DyesCall(){

    }

    public void CommunityCall(){

    }

    public void Back(){
        StartCoroutine(LoadNewObject(product.prefab));
    }

    private IEnumerator LoadNewObject(GameObject prefab){
        var newOBJ = Instantiate(prefab);
        newOBJ.transform.SetParent(newCon.transform, false);
        newOBJ.transform.localPosition = Vector3.zero;
        //newOBJ.localRotation = newCon.rotation;
        conAnim.Play("SwapObject");
        yield return new WaitForSeconds(0.5f);
        //StartCoroutine(SwapOldToNew());
        if(prevCon.transform.GetChild(0) != null) Destroy(prevCon.transform.GetChild(0).gameObject);
        prevCon.transform.localScale = new Vector3(1,1,1);
        newCon.transform.GetChild(0).SetParent(prevCon.transform, false);
        newCon.transform.localScale = Vector3.zero;
    }

    private IEnumerator SwapOldToNew(){
        yield return new WaitForSeconds(0.5f);
        //if(prevCon.GetChild(0) != null)
        if(prevCon.transform.GetChild(0) != null) Destroy(prevCon.transform.GetChild(0).gameObject);
        prevCon.transform.localScale = new Vector3(1,1,1);
        newCon.transform.GetChild(0).SetParent(prevCon.transform, false);
        newCon.transform.localScale = Vector3.zero;
    }
}
