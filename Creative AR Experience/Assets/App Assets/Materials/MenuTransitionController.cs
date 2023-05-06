using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTransitionController : MonoBehaviour
{
    /// <summary>
    /// handles the activation and deactivation of UI canvases (swapping between menu depths)
    /// and activates menu transition animations
    /// </summary>
    
    private GameObject transitionCanvas;
    private Animation canvasAnim;
    private GameObject oldMenu;
    private GameObject newMenu = null;

    void Awake(){
        transitionCanvas = GameObject.Find("TransitionCover");
        oldMenu = GameObject.Find("LoginScreen");
        canvasAnim = transitionCanvas.GetComponent<Animation>();
    }

    public void LeftAnim(GameObject newObj){
        newMenu = newObj;
        canvasAnim.Play("MenuTransitionLeft");
    }

    public void RightAnim(GameObject newObj){
        newMenu = newObj;
        canvasAnim.Play("MenuTransitionRight");
    }

    private void SwapMenu(){
        if(oldMenu != null) oldMenu.gameObject.SetActive(false);

        newMenu.gameObject.SetActive(true);
        oldMenu = newMenu;
        newMenu = null;
    }
}
