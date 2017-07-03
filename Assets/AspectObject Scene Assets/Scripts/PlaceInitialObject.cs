using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class PlaceInitialObject : MonoBehaviour {
    //This is a DEMO Only script
    // Use this for initialization
    RaycastHit hitInfo;
    int Frames = 0;
    GameObject g;
    void Start () {

        g = Instantiate(Resources.Load("ObjectName") as GameObject,null);
        g.name = "Demo Object";

    }
	
	// Update is called once per frame
	void Update ()
    {
        hitInfo = GazeManager.Instance.HitInfo;
        Frames++;
        if (Frames == 60)
        {
            
            Debug.Log(hitInfo.point);
            placeObject(g, hitInfo.point + new Vector3(0,0,-0.1f));
        }
        
	}
    void placeObject(GameObject g, Vector3 Pos)
    {
        g.transform.position = Pos;
        g.transform.LookAt(hitInfo.normal);
    }
}
