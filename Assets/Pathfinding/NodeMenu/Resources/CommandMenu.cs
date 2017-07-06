using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMenu : MonoBehaviour  {

    public GameObject prefabCommand;

	// Use this for initialization
	void Start () {
        GameObject firstCommand = Instantiate(prefabCommand, transform);
        firstCommand.GetComponent<MenuCommandLink>().methodName = "CreateNodeCommand";
        firstCommand.GetComponentInChildren<TextMesh>().text = "Create";
        firstCommand.transform.position += new Vector3(0, 0, 0);
        GameObject secondCommand = Instantiate(prefabCommand, transform);
        secondCommand.GetComponent<MenuCommandLink>().methodName = "DeleteNodeCommand";
        secondCommand.GetComponentInChildren<TextMesh>().text = "Delete";
        secondCommand.transform.position += new Vector3(0, 0.08f, 0);
        GameObject thirdCommand = Instantiate(prefabCommand, transform);
        thirdCommand.GetComponent<MenuCommandLink>().methodName = "LineNeighboursCommand";
        thirdCommand.GetComponentInChildren<TextMesh>().text = "Lines";
        thirdCommand.transform.position += new Vector3(0, 0.16f, 0);
        GameObject fourthCommand = Instantiate(prefabCommand, transform);
        fourthCommand.GetComponent<MenuCommandLink>().methodName = "ToggleModeCommand";
        fourthCommand.GetComponentInChildren<TextMesh>().text = "Toggle";
        fourthCommand.transform.position += new Vector3(0, -0.16f, 0);
        GameObject fifthCommand = Instantiate(prefabCommand, transform);
        fifthCommand.GetComponent<MenuCommandLink>().methodName = "ShowNavigationMenuCommand";
        fifthCommand.GetComponentInChildren<TextMesh>().text = "Navigation";
        fifthCommand.transform.position += new Vector3(0, -0.08f, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
