using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class CloseButtonFunction : MonoBehaviour, IInputHandler {
    public void OnInputDown(InputEventData eventData)
    {
        Destroy(transform.root.gameObject);
    }

    public void OnInputUp(InputEventData eventData)
    {
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
