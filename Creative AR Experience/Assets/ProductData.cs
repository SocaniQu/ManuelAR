using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductData", menuName = "AR Fashion/New Product", order = 0)]
public class ProductData : ScriptableObject {
    public string productName;
    public int productID;
    [TextArea(5,20)]
    public string productDescription;
    public GameObject prefab;
    public Color[] availableColors;

    public enum ProductType{
        shirt,
        pants,
        shoes
    };

    public ProductType type;

    public string fabric;
}