using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductDisplay : MonoBehaviour
{
    public ProductData product;

    public Material mainMat;

    private int colorIndex;

    void OnEnable()
    {
        if(product != null){

        }
    }

    public void ChangeColor(){
        if(colorIndex+1 >= product.availableColors.Length) colorIndex = 0;
        else colorIndex++;
        mainMat.color = product.availableColors[colorIndex];
    }

    public void LoadNewProduct(ProductData newProd){
        product = newProd;
        colorIndex = 0;
        mainMat.color = product.availableColors[colorIndex];
    }
}
