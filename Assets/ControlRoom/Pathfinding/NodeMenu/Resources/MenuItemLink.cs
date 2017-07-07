using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class MenuItemLink : MonoBehaviour , IInputClickHandler{
    public PathNode correspondingNode;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        NodeItemPlacer menu = GetComponentInParent<NodeItemPlacer>();
        menu.finder.StartNavigation(correspondingNode);
        menu.ResetMenu();
    }

}
