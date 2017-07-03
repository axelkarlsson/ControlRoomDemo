using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;
using System;

public class Object_Text_Controler : MonoBehaviour, IInputHandler {
    public void OnInputDown(InputEventData eventData)
    {
    }

    public void OnInputUp(InputEventData eventData)
    {
        transform.root.SendMessage("OnTap");
    }

    // Use this for initialization
    void Start () {
        gameObject.GetComponent<Text>().text = transform.root.name;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
