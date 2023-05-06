using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductData", menuName = "AR Fashion/New Product", order = 0)]
public class ProductData : ScriptableObject {
    public string productName;
    //ideally you would have a productID as a unique identifier,
    //however in its current state, all associated scripts use the productName
    public int productID;
    public GameObject prefab;
    [TextArea(5,20)]
    public string productDescription;
    public Color[] availableColors;

    public enum ProductType{
        shirt,
        pants,
        shoes
    };

    public ProductType type;

    [TextArea(5,20)]
    public string fabricInfo;

    [TextArea(5,20)]
    public string ironingInfo;

    [TextArea(5,20)]
    public string washingInfo;

    [TextArea(5,20)]
    public string techniquesInfo;

    [TextArea(5,20)]
    public string dyeInfo;

    [TextArea(5,20)]
    public string communityInfo;
}