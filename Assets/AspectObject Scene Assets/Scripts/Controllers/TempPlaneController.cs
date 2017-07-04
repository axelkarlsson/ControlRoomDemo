using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class TempPlaneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<TapToPlace>().IsBeingPlaced = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
