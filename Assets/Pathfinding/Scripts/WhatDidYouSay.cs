using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhatDidYouSay : MonoBehaviour {
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = Camera.main.transform.position + Camera.main.transform.forward;
        transform.position = newPos;
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(0, 180, 0);
	}

    public void UpdateText(string newText)
    {
        GetComponentInChildren<TextMesh>().text = newText;
    }
}
