using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class PlaceTrackedImage : MonoBehaviour{
    private ARTrackedImageManager trackedImageManager;

    public ProductData[] products;

    private readonly Dictionary<string, GameObject> instantiatedPrefabs = new Dictionary<string, GameObject>();

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

                    var newPrefab = Instantiate(curProduct.prefab, trackedImage.transform);
                    //add instantiated prefab to list of active prefabs
                    instantiatedPrefabs[imageName] = newPrefab;

                    }
            }
        }

        //if image is update, make sure instantiated objects are tracking to proper tracked images
        foreach(var trackedImage in eventArgs.updated){
            instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState == TrackingState.Tracking);
        }

        //when tracked object is removed from tracking order, destroy object model and remove image from list
        foreach(var trackedImage in eventArgs.removed){
            Destroy(instantiatedPrefabs[trackedImage.referenceImage.name]);
            instantiatedPrefabs.Remove(trackedImage.referenceImage.name);
        }
    }
}
