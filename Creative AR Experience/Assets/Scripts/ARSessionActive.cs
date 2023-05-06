using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSessionActive : MonoBehaviour
{
    /// <summary>
    /// handles the enabling and disabling of all AR session objects
    /// also ensures one of the two cameras are currently active in scene
    /// 
    /// NOTE: only object with this script is required in the scene
    /// call ActivateAR in same event call that would open UI for an AR active menu
    /// </summary>

    [SerializeField]
    private List<GameObject> arObjects;
    [SerializeField]
    private GameObject uiCamera;

    public void ActivateAR(){
        foreach(GameObject obj in arObjects){
            obj.SetActive(true);
        }
        uiCamera.SetActive(false);
    }

    public void DeactivateAR(){
        uiCamera.SetActive(true);
        foreach(GameObject obj in arObjects){
            obj.SetActive(false);
        }
    } 
}
