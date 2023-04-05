using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductDisplay : MonoBehaviour
{
    public ProductData product;

    public GameObject containerParent;
    private Animator conAnim;
    private Transform prevCon;
    private Transform newCon;

    void Start(){
        conAnim = this.GetComponent<Animator>();
        prevCon = containerParent.transform.GetChild(0);
        newCon = containerParent.transform.GetChild(1);
    }
    
    void OnEnable()
    {
        if(product != null){

        }
    }

    public void LoadNewProduct(ProductData newProd){
        product = newProd;
        LoadNewObject(product.prefab);
    }

    public void FabricCall(){
        
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

    private void LoadNewObject(GameObject prefab){
        var newOBJ = Instantiate(prefab).transform;
        newOBJ.SetParent(newCon);
        newOBJ.localPosition = Vector3.zero;
        newOBJ.localRotation = newCon.rotation;
        conAnim.Play("SwapObject");
    }

    public void SwapOldToNew(){
        Destroy(prevCon.GetChild(0).gameObject);
        newCon.GetChild(0).SetParent(prevCon);
        prevCon.localScale = new Vector3(2,2,2);
        newCon.localScale = Vector3.zero;
    }
}
