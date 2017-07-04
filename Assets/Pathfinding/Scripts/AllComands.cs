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
	
	// Update is called once per frame
	void Update () {
        if(waitFrames > commandDisplayTime)
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
