using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBob : MonoBehaviour
{
    [Tooltip("Distance (up/down) from start")]
    public float bobScale;
    [Tooltip("Time for transition between points")]
    public float bobTime;
    
    //variables for enumerator use
    private float time = 0;
    private Vector3 preBobPos;
    private float veloc = 0.0f;

    void Start(){
        //set initial position of object, and start bobbing
        preBobPos = transform.position;
        StartCoroutine(Bob());
    }

    private IEnumerator Bob(){
        //find end position using scale
        float endPos = preBobPos.y + bobScale;
        time = 0;
        //using double bob time for transition allows for smoother movement
        float maxTime = bobTime * 2;
        while(time < maxTime){
            //smoothly move towards point
            float vert = Mathf.SmoothDamp(transform.position.y, endPos, ref veloc, bobTime);
            transform.position = new Vector3(preBobPos.x, vert, preBobPos.z);
            
            //after updating position, increase time elapsed, and wait for next frame call
            time += Time.deltaTime;
            yield return 0;
        }
        //set bob scale to opposite value, start another bob loop
        bobScale *= -1;
        StartCoroutine(Bob());
    }
}
