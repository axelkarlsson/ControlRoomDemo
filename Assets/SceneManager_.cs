using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_ : MonoBehaviour {

    static public Vector3 LoadAtPosition;
    bool hasLoaded = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene().name == "TheHolographicTest" &&  !hasLoaded && LoadAtPosition != null)
        {
            hasLoaded = PlaceDemoObject(LoadAtPosition);
        }
	}
    bool PlaceDemoObject(Vector3 pos)
    {
        GameObject g = Instantiate(Resources.Load("ObjectName") as GameObject, null);
        g.name = "Demo Object";
        g.transform.position = pos;
        g.transform.LookAt(Camera.main.transform);
        g.transform.Rotate(Vector3.up, 180f);
        g.active = true;
        return (g.activeInHierarchy);
    }
}
