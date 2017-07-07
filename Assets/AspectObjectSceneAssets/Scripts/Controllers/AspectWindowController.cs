using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class AspectWindowController : MonoBehaviour {

    TapToPlace Placer;
    // Use this for initialization
    void Start () {
        Placer = GetComponentInChildren<TapToPlace>();
        Placer.OnInputClicked(eventData: null);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
