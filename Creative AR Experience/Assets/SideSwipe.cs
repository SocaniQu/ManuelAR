using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SideSwipe : MonoBehaviour, IDragHandler, IEndDragHandler {
    
    public Vector3 currentLocation;
    public float swipeThreshold = 0.2f;
    public float panelDistance = 300f;
    public float lerpTime = 0.5f;

    [Header("Parent Menu")]
    public GameObject parentMenu;
    private MenuSwipe parent;

    void Start(){
        currentLocation = transform.localPosition;
        try{  
            parent = parentMenu.GetComponent<MenuSwipe>();
        }catch{}
    }

    public void OnDrag(PointerEventData data){
        if(parent != null) parent.bottomMenuSwipe = true;
        float dif = data.pressPosition.x - data.position.x;
        transform.localPosition = currentLocation - new Vector3(dif, 0f, 0f);
    }

    public void OnEndDrag(PointerEventData data){
        if(parent != null) parent.bottomMenuSwipe = false;
        float percent = (data.pressPosition.x - data.position.x)/Screen.width;
        if(Mathf.Abs(percent) >= swipeThreshold){
            Vector3 newLocation = currentLocation;
            if(percent > 0){
                newLocation += new Vector3(-panelDistance, 0f, 0f);
            }else if(percent < 0){
                newLocation += new Vector3(panelDistance, 0f, 0f);
            }
            StartCoroutine(PanelLerp(transform.localPosition, newLocation, lerpTime));
            currentLocation = newLocation;
        }else{
            StartCoroutine(PanelLerp(transform.localPosition, currentLocation, lerpTime));
        }
    }

    IEnumerator PanelLerp(Vector3 startPos, Vector3 endPos, float seconds){
        float t = 0f;
        while(t <= 1.0){
            t += Time.deltaTime/seconds;
            transform.localPosition = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
