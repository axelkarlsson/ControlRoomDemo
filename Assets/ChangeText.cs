using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.UI.Keyboard;
using System;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour, IHoldHandler
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnHoldStarted(HoldEventData eventData)
    {
        if (transform.parent.parent.parent.parent.Find("MoveButton").gameObject.activeSelf)
        {
            //Show the keyboard prefab included in holotoolkit and subscribe to relevant events
            Keyboard.Instance.Close();
            Keyboard.Instance.PresentKeyboard();
            Keyboard.Instance.RepositionKeyboard(Camera.main.transform.position + Camera.main.transform.forward * 2);
            Keyboard.Instance.onTextSubmitted += this.Keyboard_onTextSubmitted;
        }
    }

    private void Keyboard_onTextSubmitted(object sender, EventArgs e)
    {
        string tmp;
        var keyboard = sender as Keyboard;
        tmp = keyboard.m_InputField.text;
        GetComponent<Text>().text = tmp;
        Keyboard.Instance.onTextSubmitted -= this.Keyboard_onTextSubmitted;
    }

    public void OnHoldCompleted(HoldEventData eventData)
    {
        //NoTUsed
    }

    public void OnHoldCanceled(HoldEventData eventData)
    {
        //Notused
    }
}
