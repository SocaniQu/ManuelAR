using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

[RequireComponent(typeof(ARTrackedImageManager))]
public class PlaceTrackedImage : MonoBehaviour{
    private ARTrackedImageManager trackedImageManager;

    public GameObject uiPrefab;

    public ProductData[] products;

    public ProductData test;

    private readonly Dictionary<string, GameObject> instantiatedPrefabs = new Dictionary<string, GameObject>();

    void Awake(){
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        string productFilePath = "Assets/Products/Product Data";
        products = Resources.LoadAll<ProductData>(productFilePath);
    }

    void OnEnable(){
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable(){
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs){
        
        //called when new tracking image is detected
        foreach(var trackedImage in eventArgs.added){
            
            //get tracked image name
            var imageName = trackedImage.referenceImage.name;

            foreach(var curProduct in products){

                //check tracked image name against available products, not case sensitive
                //if tracked image matches product, instantiate model of product at the tracked image location
                if(string.Compare(curProduct.name, imageName, StringComparison.OrdinalIgnoreCase) == 0 
                && !instantiatedPrefabs.ContainsKey(imageName)){
                    //find ui element, set parent to tracked image transform
                    if(!GameObject.Find("prefabTest")){
                        NewUI(trackedImage.transform, curProduct);
                    }
                    else{
                        Destroy(GameObject.Find("prefabTest"));
                        NewUI(trackedImage.transform, curProduct);
                    }
                }
            }
        }
        //old update function, would cause objects to flicker in and out of view
        
        //if image is update, make sure instantiated objects are tracking to proper tracked images
        /*foreach(var trackedImage in eventArgs.updated){
            instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState == TrackingState.Tracking);
        }*/

        //when tracked object is removed from tracking order, destroy object model and remove image from list
        /*foreach(var trackedImage in eventArgs.removed){
            Destroy(instantiatedPrefabs[trackedImage.referenceImage.name]);
            instantiatedPrefabs.Remove(trackedImage.referenceImage.name);
        }*/
    }

    public void Reset(ARTrackedImagesChangedEventArgs eventArgs){
        foreach(var trackedImage in eventArgs.removed){
            Destroy(instantiatedPrefabs[trackedImage.referenceImage.name]);
            instantiatedPrefabs.Remove(trackedImage.referenceImage.name);
        }
    }

    private void DestroyTrackedComponent(ARTrackedImagesChangedEventArgs eventArgs){
        Destroy(instantiatedPrefabs[name]);
        instantiatedPrefabs.Remove(name);
    }

    private void NewUI(Transform trackedImage, ProductData curProduct){
        var ui = Instantiate(uiPrefab).transform;
        ui.name = "prefabTest";
        ui.SetParent(trackedImage, false);
        ui.localPosition = Vector3.zero;
        ui.localRotation = Quaternion.Euler(Vector3.zero);
        ui.GetComponent<ProductDisplay>().LoadNewProduct(curProduct);
        //return ui.gameObject;
    }

    public void TestUI(){
        var ui = Instantiate(uiPrefab);
        ui.transform.name = "prefabTest";
        ui.transform.localPosition = new Vector3(0f,0f, 1.0f);
        ui.transform.localRotation = Quaternion.Euler(Vector3.zero);
        ui.GetComponent<ProductDisplay>().LoadNewProduct(test);
    }
}
