using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeItemPlacer : MonoBehaviour {
    private List<GameObject> NodeItems = new List<GameObject>();
    public PathFinder finder;
	// Use this for initialization
	void Start () {

        finder = GameObject.FindGameObjectWithTag("PathFinder").GetComponent<PathFinder>();
        
    }

    public void ShowMenu()
    {
        //Instantiate a menu object for each Node
        ResetMenu();
        if (finder.destinationNodes.Count > 0)
        {
            foreach (PathNode G in finder.destinationNodes)
            {
                GameObject NodeItem = Instantiate(Resources.Load("NodeMenuItem") as GameObject, transform);
                NodeItem.GetComponentInChildren<TextMesh>().text = G.gameObject.name;
                NodeItems.Add(NodeItem);
                NodeItem.GetComponent<MenuItemLink>().correspondingNode = G;
            }
            Vector3 Menu_Bounds = NodeItems[0].GetComponent<BoxCollider>().bounds.size;
            //Resize Entire Menu
            GetComponent<RectTransform>().sizeDelta = new Vector2(Menu_Bounds.x, Menu_Bounds.y * NodeItems.Count);
            //Fit Each Menu Object
            float objC = 0;

            foreach (GameObject G in NodeItems)
            {
                G.transform.position = new Vector3(G.transform.position.x, objC * Menu_Bounds.y + G.transform.position.y, G.transform.position.z);
                objC++;
            }



            transform.position = Camera.main.transform.position + Camera.main.transform.forward;
            transform.LookAt(Camera.main.transform.position);
            transform.Rotate(0, 180, 0);
        }
    }

    public void ResetMenu()
    {
        foreach (GameObject node in NodeItems)
        {
            GameObject.Destroy(node);
        }
        NodeItems.Clear();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
