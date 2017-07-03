using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class HoldToFocus : MonoBehaviour , IHoldHandler
{
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnHoldCanceled(HoldEventData eventData)
    {
        //Needed for interface
    }

    public void OnHoldCompleted(HoldEventData eventData)
    {
        GetComponentInParent<PathNode>().NodeSelected();
    }

    public void OnHoldStarted(HoldEventData eventData)
    {
        //Needed for interface
    }


}
