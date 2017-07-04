using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using System;

public class Object_Controller : MonoBehaviour
{
    [Tooltip("A list of all associated Aspects displayable in the enviroment")]
    public List<string> AspectNames = new List<string>();

    protected GameObject AspectMenu; // The Associated Aspect menu

    [Tooltip("A list of all spawned Aspects")]
    public List<GameObject> AspectWindows = new List<GameObject>();

    private Renderer[] Rendlist; //Used For dimming Object when Aspect Menu is shown

    void Start() {
        
        Rendlist = gameObject.GetComponentsInChildren<Renderer>(); // Get Components to use for dimming

        if (AspectNames.Count == 0) //if List is empty, populate with demo Objects
        {
            for (int i = 1; i <= 15; i++) //Generate Examples, since nothing to actually implement.
            {
                AspectNames.Add("<Aspect " + i.ToString() + ">");
            } // DemoCode
        }

        AspectMenu = Instantiate(Resources.Load("Aspect_Menu")) as GameObject;
        AspectMenu.transform.SetParent(transform, false);
        AspectMenu.name = "Aspect_Menu";
        AspectMenu.SetActive(true);
        InitializeOnStart();
        AspectMenu.SetActive(false);
        
    } 
    // Update is called once per frame
    void Update(){
    } //Does Nothing ATM

    void OnTap() {
        if (AspectMenu.activeSelf == false)
        {
            OnOpen();
        }
        else
        {
            OnClose();
        }
    } //Toggle Open or Closed

    void OnOpen()
    {
        AspectMenu.SetActive(true);
        foreach (Renderer r in Rendlist)
        {
            if (r.gameObject.tag != "Hightlight") { r.material.color = new Color(1, 1, 1, 0.2f); }
        }
    } //Show Aspect Menu and Dim Object

    void OnClose()
    {
        AspectMenu.SetActive(false);
        foreach (Renderer r in Rendlist)
        {
            if (r.gameObject.tag != "Hightlight") { r.material.color = new Color(1, 1, 1, 1f);}
        }
        //Minimize All children
    } //Hide Aspect Menu and Solidify Object

    void OnCloseAll()
    {
        OnClose();
    } //OnClose

    void GetAspectWindow(string AspectName)
    {
        GameObject g = Instantiate(Resources.Load("Aspect_Window"), null) as GameObject;
        g.name = AspectName;
        AspectWindows.Add(g);
    } //Create New Window associated with given AspectName

    void CloseAspectWindow(string AspectName)
    {
        foreach (GameObject g in AspectWindows)
        {
            if (g.name == AspectName)
            {
                AspectWindows.Remove(g);
                GameObject.Destroy(g);
                break;
            }
        }
        //Close the associated Aspect Window, if any
    } //Close Window With Associated Aspect Name
    void InitializeOnStart()
    {
        transform.Rotate(Vector3.up, 180f);
        transform.LookAt(Camera.main.transform);
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
}
