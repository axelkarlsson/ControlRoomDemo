using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class AllComands : MonoBehaviour, ISpeechHandler {

    private int waitFrames;
    public int commandDisplayTime;

    // Use this for initialization
    void Start () {
        InputManager.Instance.AddGlobalListener(gameObject);
	}


    //Can use this function to display an aribtrary string to the user.
    public void TextLog(string text)
    {
        GetComponent<TextMesh>().text = text;
        waitFrames = 0;
    }
	
	// Update is called once per frame
	void Update () {
        
        if(waitFrames > commandDisplayTime * 2)
        {
            GetComponent<TextMesh>().text = "";
        }
        else
        {
            waitFrames++;
        }
        

	}


    public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        GetComponent<TextMesh>().text = eventData.RecognizedText;
        waitFrames = 0;
    }


 
}
