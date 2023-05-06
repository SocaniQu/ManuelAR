using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    /// <summary>
    /// called start menu, but can be used in any menu where you would like there
    /// to be a transition like the one used in the start menu
    /// (bisected menu and panel slide upwards)
    /// 
    /// NOTE: this requires the usage of the UI materials found in the materials folder
    /// marked opaque and transparent, the transparent object will cut render away from
    /// the opaque one
    /// </summary>


    public RectTransform transparent;
    public RectTransform top;
    public RectTransform bottom;
    //sweet spot for animation is 0.3f
    [Range(0.0f, 0.7f)]
    public float animationTime;

    private float topFinalPos = 400;
    private float bottomFinalPos = -400;
    private float transparentFinalPos = Screen.height;

    //separate refs are required for each menu piece
    private float veloc = 0.0f;
    private float veloc1 = 0.0f;
    private float veloc2 = 0.0f;

    public void MenuStart(){
        StartCoroutine(Anim());
    }
    
    
    private IEnumerator Anim(){
        float time = 0;
        float maxTime = animationTime * 3f;
        while(time < maxTime){
            transparent.sizeDelta = new Vector2(0, Mathf.SmoothDamp(transparent.sizeDelta.y, transparentFinalPos, 
            ref veloc, animationTime));
            if(time >= 0.2f && top != null && bottom != null){
                top.anchoredPosition = new Vector2(0, Mathf.SmoothDamp(top.anchoredPosition.y, topFinalPos, 
                ref veloc1, animationTime));
                bottom.anchoredPosition = new Vector2(0, Mathf.SmoothDamp(bottom.anchoredPosition.y, bottomFinalPos, 
                ref veloc2, animationTime));
            }
            time+= Time.deltaTime;
            yield return 0;
        }
        transparent.sizeDelta = new Vector2(0, transparentFinalPos);
        top.anchoredPosition = new Vector2(0, topFinalPos);
        bottom.anchoredPosition = new Vector2(0, bottomFinalPos);
        this.gameObject.SetActive(false);
    }
}
