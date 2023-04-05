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

    private readonly Dictionary<string, GameObject> instantiatedPrefabs = new Dictionary<string, GameObject>();

    private string previousPrefab = null;

    void Awake(){
        trackedImageManager = GetComponent<ARTrackedImageManager>();
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
                    //if there has been a previous instance of the object destroy it
                    //if(previousPrefab != null) DestroyTrackedComponent(previousPrefab);
                    //get 3d object of correct product, and create at tracked object position
                    var newPrefab = Instantiate(curProduct.prefab, trackedImage.transform);
                    //add instantiated prefab to list of active prefabs
                    instantiatedPrefabs[imageName] = newPrefab;

                    //add key for prefab
                    previousPrefab = newPrefab.name;

                    //find ui element, set parent to tracked image transform
                    var ui = GameObject.Find("prefabTest").transform;
                    ui.SetParent(trackedImage.transform, false);
                    //set position relative to image to zero, centered on object for easier edits
                    //set rotation to zero relative to worldspace
                    ui.localPosition = Vector3.zero;
                    ui.rotation = Quaternion.Euler(Vector3.zero);

                    //if the ui element is not active, activate it
                    if(!ui.gameObject.activeSelf) ui.gameObject.SetActive(true);
                    //send product data to ui element for display purposes
                    ui.GetComponent<ProductDisplay>().LoadNewProduct(curProduct);
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
}
