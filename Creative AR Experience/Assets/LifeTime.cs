using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    void Start(){
        Destroy(this, 5f);
    }
}
