using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MatchScreenSize : MonoBehaviour
{
    [Tooltip("Enable to set height to half of screen.")]
    public bool horizontalBisect = false;
    [Tooltip("Enable to set width to half of screen.")]
    public bool verticalBisect = false;
    RectTransform rect;
    void Start(){
        int horizontal = verticalBisect ? (int)Screen.width/2 : Screen.width;
        int vertical = horizontalBisect ? (int)Screen.height/2 : Screen.height;
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(horizontal, vertical);


        Canvas.ForceUpdateCanvases();
    }
}
