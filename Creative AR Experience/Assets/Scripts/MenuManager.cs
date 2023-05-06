using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;

    public void CloseMenu(){
        menu.SetActive(false);
    }

    public void OpenMenu(){
        menu.SetActive(true);
    }
}
