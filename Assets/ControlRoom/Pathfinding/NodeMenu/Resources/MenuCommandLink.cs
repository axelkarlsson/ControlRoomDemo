using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class MenuCommandLink : MonoBehaviour, IInputClickHandler
{
    private TextMesh textMesh;
    private Color originalColor;

    public string methodName;

    public void Awake()
    {
        textMesh = GetComponentInChildren<TextMesh>();
        originalColor = textMesh.color;
    }
    // Use this for initialization
    void Start()
    {

    }

    private void Update()
    {
        IsHighlighted = GazeManager.Instance.HitObject == this.gameObject;
    }

    private bool IsHighlighted
    {
        set
        {
            textMesh.color = value ? Color.yellow : originalColor;
        }
    }


    public void OnInputClicked(InputClickedEventData eventData)
    {
        GameObject.FindGameObjectWithTag("PathFinder").GetComponent<PathFinder>().SendMessage(methodName);
    }
}