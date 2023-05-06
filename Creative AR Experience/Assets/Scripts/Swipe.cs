using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Swipe : MonoBehaviour, IDragHandler, IEndDragHandler{

    /// <summary>
    /// THIS SCRIPT IS DEPRICATED
    /// 
    /// for swiping menus please use the SideSwipe and MenuSwipe scripts
    /// </summary>
    
    private Vector2 startPosition;
    private Vector2 endPosition;

    public bool menuOpen;

    [Header("Menu Parameters")]
    public Vector3 menuOpenPosition;
    public Vector3 menuClosedPosition;
    public float swipeAreaMax;
    public float openMenuSwipeAreaMax;

    [Header("If there is a sideswipe submenu")]
    public GameObject subMenu;

    void Awake(){
        CloseMenu();
    }

    public void OnDrag(PointerEventData data){

    }

    public void OnEndDrag(PointerEventData data){

    }

    void Update(){
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            startPosition = Input.GetTouch(0).position;
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended){
            endPosition = Input.GetTouch(0).position;


            if(menuOpen && startPosition.y < openMenuSwipeAreaMax){
                if(endPosition.y < startPosition.y && menuOpen){
                    menuOpen = false;
                    CloseMenu();
                }

            }else if(!menuOpen && startPosition.y < swipeAreaMax){
                if(endPosition.y > startPosition.y && !menuOpen){
                    menuOpen = true;
                    OpenMenu();
                }
            }
        }
    }

    private void OpenMenu(){
        this.transform.position = menuOpenPosition;
        if(subMenu != null){
            //subMenu.GetComponent<SideSwipe>().UpdateMenuPos();
        }
        
    }

    private void CloseMenu(){
        this.transform.position = menuClosedPosition;
        if(subMenu != null){
            //subMenu.GetComponent<SideSwipe>().UpdateMenuPos();
        }
    }
}
