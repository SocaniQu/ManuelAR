using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSwipe : MonoBehaviour, IDragHandler, IEndDragHandler{
    private Vector2 startPos;
    private Vector2 endPos;

    [Header("General Parameters")]
    public float menuSmoothTime;
    public float swipeDeadZoneBorder;
    private float swipeDeadTop;
    private float swipeDeadBottom;


    [Header("Top Menu Parameters")]
    public GameObject topMenu;
    public Vector3 topOpenPos;
    public Vector3 topClosedPos;
    private bool topMenuOpen = false;

    [Header("Bottom Menu Parameters")]
    public GameObject bottomMenu;
    public Transform bottomArrow;
    private bool bottomMenuOpen = false;

    public float bottomOpenHeight;
    private Vector3 bottomOpenPos;

    public float bottomClosedHeight;
    private Vector3 bottomClosedPos;

    public bool bottomMenuSwipe;

    void Awake(){
    
    }

    void Start(){
        swipeDeadBottom = swipeDeadZoneBorder;
        swipeDeadTop = Screen.height - swipeDeadZoneBorder;
        bottomClosedPos = new Vector3(0f, -(Screen.height/2f), 0f);
        bottomOpenPos = new Vector3(0f, -(Screen.height/2f)+400f, 0f);
    }

    public void OnDrag(PointerEventData data){
    }

    public void OnEndDrag(PointerEventData data){
        Vector2 dif = data.pressPosition - data.position;
        DetectZone(data.pressPosition, data.position, dif);

    }

    private void DetectZone(Vector2 swipeStart, Vector2 swipeEnd, Vector2 dif){
        if(swipeStart.y > swipeDeadTop && dif.y > 0){
            Debug.Log("top menu");
        }else if(swipeStart.y < swipeDeadBottom){
            if(!bottomMenuSwipe){
                if(dif.y < 0 && !bottomMenuOpen){
                    //open bottom menu
                    bottomMenuOpen = true;
                    StartCoroutine(RotLerp(bottomArrow, bottomArrow.rotation.eulerAngles,
                    new Vector3(0, 0, 180), menuSmoothTime));

                    StartCoroutine(PosLerp(bottomMenu, bottomMenu.transform.position,
                    bottomOpenPos, menuSmoothTime));
                }else if(dif.y > 0 && bottomMenuOpen){
                    //close bottom menu
                    bottomMenuOpen = false;
                    StartCoroutine(RotLerp(bottomArrow, bottomArrow.rotation.eulerAngles,
                    Vector3.zero, menuSmoothTime));

                    StartCoroutine(PosLerp(bottomMenu, bottomMenu.transform.position,
                    bottomClosedPos, menuSmoothTime));
                }
            }
        }
    }




    IEnumerator PosLerp(GameObject menu, Vector3 startPos, Vector3 endPos, float seconds){
        float t = 0f;
        while(t <= 1.0){
            t += Time.deltaTime/seconds;
            menu.transform.localPosition = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    IEnumerator RotLerp(Transform obj, Vector3 startRot, Vector3 endRot, float seconds){
        float t = 0f;
        while(t <= 1.0){
            t += Time.deltaTime/seconds;
            obj.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRot, endRot, Mathf.SmoothStep(0f, 1f, t)));
            yield return null;
        }
    }
}
