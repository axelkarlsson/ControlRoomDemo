using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class MenuCommandLink : MonoBehaviour, IInputClickHandler
{
    public string methodName;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        GameObject.FindGameObjectWithTag("PathFinder").GetComponent<PathFinder>().SendMessage(methodName);
    }
}