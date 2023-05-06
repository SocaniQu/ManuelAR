using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProductDisplay : MonoBehaviour
{
    /// <summary>
    /// manages the AR product display, will fetch product information from ProductData scriptable objects
    /// 
    /// 
    /// </summary>


    public ProductData product;

    private Animation conAnim;
    private Transform containerParent;
    private GameObject prevCon;
    private GameObject newCon;

    //WorldIcons is used as a temporary storage for the icon images
    //while remaining editable in the unity inspector
    [SerializableAttribute]
    public struct WorldIcons{
        public string name;
        public GameObject obj;
    };

    //this array is only used at the start of runtime to generate a dictionary using its contents
    public WorldIcons[] icons;

    public Dictionary<string, GameObject> worldSpaceIcons = new Dictionary<string, GameObject>();
    
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

        //move all world icons from the icons array to the worldSpaceIcons dictionary
        foreach(var icon in icons){
            try{
                
                worldSpaceIcons.Add(icon.name, icon.obj);
            }catch{
                Debug.Log("ProductDisplay Error");
            }
        }
    }

    public void LoadNewProduct(ProductData newProd){
        product = newProd;
        text.text = product.productDescription;
        StartCoroutine(LoadNewObject(product.prefab));
    }

    //this method will generate a new object, play an animation of the object growing using the
    //SwapObject anim, move the newly generated object to a separate parent, and delete any old object
    private IEnumerator LoadNewObject(GameObject prefab){
        var newOBJ = Instantiate(prefab);
        newOBJ.transform.SetParent(newCon.transform, false);
        newOBJ.transform.localPosition = Vector3.zero;
        conAnim.Play("SwapObject");
        yield return new WaitForSeconds(1.0f);
        if(prevCon.transform.childCount != 0) Destroy(prevCon.transform.GetChild(0).gameObject);
        prevCon.transform.localScale = new Vector3(1,1,1);
        newCon.transform.GetChild(0).SetParent(prevCon.transform, false);
        newCon.transform.localScale = Vector3.zero;
    }

    //this method is to be called from the UI associated with the AR product display
    //it will take a string input, compare it against the currently selected display state
    //and if it matches it will swap the displayed object back to the product display
    //if it does match it will simply swap to the newly selected object
    public void CompareDisplay(string current){
        if(currentDisplay == (DisplayState)DisplayState.Parse(typeof(DisplayState), current)){
            StartCoroutine(LoadNewObject(product.prefab));
            currentDisplay = DisplayState.product;
            UpdateText("product");
            
        }else{
            currentDisplay = (DisplayState)DisplayState.Parse(typeof(DisplayState), current);
            StartCoroutine(LoadNewObject(worldSpaceIcons[current]));
            UpdateText(current);
        }
    }

    //called to update the display text in the AR display to the current selection
    //i could not think of a cleaner way to do this, god have mercy on me
    private void UpdateText(string descriptionType){
        switch(descriptionType){
            case "product":
                text.text = product.productDescription;
                break;
            case "fabric":
                text.text = product.fabricInfo;
                break;
            case "ironing":
                text.text = product.ironingInfo;
                break;
            case "washing":
                text.text = product.washingInfo;
                break;
            case "techniques":
                text.text = product.techniquesInfo;
                break;
            case "dye":
                text.text = product.dyeInfo;
                break;
            case "community":
                text.text = product.communityInfo;
                break;
            default:
                text.text = "Error, Unknown data type:"+descriptionType;
                break;
        }
    }
}
