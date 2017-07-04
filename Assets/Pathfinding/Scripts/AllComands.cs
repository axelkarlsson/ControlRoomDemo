using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class AllComands : MonoBehaviour, ISpeechHandler {
   

    // Use this for initialization
    void Start () {
        InputManager.Instance.AddGlobalListener(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        GetComponent<TextMesh>().text = eventData.RecognizedText;
    }
}
