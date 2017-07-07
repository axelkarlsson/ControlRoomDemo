using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PathFinder : MonoBehaviour , ISpeechHandler
{
    //The object which is currently selected
    public GameObject activeObject = null;

    [Tooltip("The prefab of a node used in PathFinding.")]
    public GameObject prefabNode;

    [Tooltip("The prefab of a generic object to place in the world")]
    public GameObject prefabObject;

    [Tooltip("The prefab of a line renderer for showing which nodes are connected.")]
    public GameObject prefabLineRenderer;

    //List of nodes that can be searched for destinations to navigate to    
    public List<PathNode> destinationNodes;

    //Stack of nodes in the current path, destination is the last node in the stack.
    public Stack<PathNode> currentPath = new Stack<PathNode>();

    public bool editMode;

    [Tooltip("After how many frames the camera node should check what neighbours it can see")]
    public int frameUpdateInterval;

    private int updateCount = 0;
    private int nodeId;
    private PathNode cameraNode;
    private GameObject commandMenu;


    // Use this for initialization
    void Start()
    {
        //Register as a fallback listener for when no hologram is looked at
        InputManager.Instance.PushFallbackInputHandler(gameObject);

        //Create the node that follows the camera
        GameObject newNode = Instantiate(prefabNode, transform);            //Set this object as parent so the cameraNode is created in the same scene as the PathFinder
        newNode.GetComponentInChildren<TapToPlaceNode>().IsBeingPlaced = false;
        newNode.transform.parent = null;                                    //Remove PathFinder as parent so it is not included when searching for other nodes
        cameraNode = newNode.GetComponent<PathNode>();
        newNode.name = "Node " + nodeId;
        nodeId++;
        newNode.GetComponentInChildren<TapToPlaceNode>().IsBeingPlaced = false;

        //Find the command menu
        commandMenu = GameObject.FindGameObjectWithTag("CommandMenu");
        
    }

    // Update is called once per frame
    void Update()
    {
        updateCount++;
        if (updateCount > frameUpdateInterval)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            //TODO raycast to find ground
            cameraPos.y -= 0.8f;
            //Move cameranode to camera position
            cameraNode.transform.position = cameraPos;
          
            updateCount = 0;


            //See if you are at node, then you win!
            if (currentPath.Count > 0)
            {
                PathNode upcoming = currentPath.Pop();
                if(upcoming.DistanceTo(cameraNode) < 2)
                {
                    upcoming.ResetNode();
                    if(currentPath.Count == 0)
                    {
                        commandMenu.SetActive(true);
                        //Todo
                        //Find("AspectMenuObjectRoot").SendMessage("RandomFunctionName", upcoming);
                        //upcoming.GetComponent<PathFinder>();
                        
                    }
                }
                else
                {
                    currentPath.Push(upcoming);
                }
            }
        }


    }


    //Starts navigation from camera position to selected node
    public void StartNavigation(PathNode destination)
    {
        if (!editMode)
        {
            //Set node layers to 0 to enable collision so that camera can look for neihbours
            SetNodeLayer(0);
            UpdateNode(cameraNode);
            //Set node layers to 2 to disable collision
            SetNodeLayer(2);
            commandMenu.SetActive(false);
            FindPath(cameraNode, destination);
        }
    }

    //Resets all nodes in preparation to find a new path
    public void ResetNodes()
    {
        foreach(PathNode node in GetChildNodes())
        {
            node.ResetNode();
        }
    }
 
    //Creates a new node to be placed. Returns null if an element is being moved
    public void CreateNode()
    {
           
        //Don't do anything if another node is being placed
        if (IsChildBeingPlaced()) { return; }

        GameObject newNode = Instantiate(prefabNode, transform);
        PathNode newPathNode = newNode.GetComponent<PathNode>();
        newNode.name = "Node " + nodeId;
        nodeId++;


        //TMP
        destinationNodes.Add(newPathNode);

    }

    //Creates a new factory object to be placed
    public void CreateObject()
    {
        //Do not create object if child is being placed
        if (IsChildBeingPlaced()) { return; }

        GameObject newObject = Instantiate(prefabObject, transform);
        PathNode newPathNode = newObject.GetComponent<PathNode>();
        newObject.name = "TestObjectNameIDGoesHere";
        destinationNodes.Add(newPathNode);

    }

    //Returns if any child nodes are currently being moved
    private bool IsChildBeingPlaced()
    {

        PathNode[] children = GetChildNodes();
        foreach (PathNode node in children)
        {
            TapToPlaceNode placer = node.GetComponentInChildren<TapToPlaceNode>();
            if ((placer != null) && (placer.IsBeingPlaced)) { return true; }
        }
        return false;
    }

    //Returns all the childs PathNode components
    public PathNode[] GetChildNodes()
    {
        return GetComponentsInChildren<PathNode>();
    }

    //Change the layer of all childnodes that have PathNodes attached. Use 2 to ignore raycasting and 0 for default.
    public void SetNodeLayer(int layer)
    {
        foreach(PathNode node in GetChildNodes())
        {
            node.GetComponentInChildren<MeshRenderer>().gameObject.layer = layer;
        }
    }

    //Updates the neighbours of a node
    public void UpdateNode(PathNode node)
    {
        PathNode[] children = GetChildNodes();
        foreach(PathNode child in children)
        {
            if(node != child)
            {
                if (!node.TryLink(child))
                {
                    node.RemoveNeighbour(child);
                }
            }
        }
        //Might be a bad idea to always do this
        if (cameraNode != node)
        {
            RemoveLines();
            DrawLines();
        }
    }

    //Removes the current active node
    private void RemoveNode()
    {
        if (activeObject != null)
        {
            Destroy(activeObject);
        }
    }

    // A * pathfinding
    public void FindPath(PathNode start, PathNode end)
    {
        ResetNodes();
        bool pathFound = false;
        PathNode current = null;
        Priority_Queue.FastPriorityQueue<PathNode.PathFinderNode> queue = new Priority_Queue.FastPriorityQueue<PathNode.PathFinderNode>(100);
        queue.Enqueue(start.node, start.DistanceTo(end));
        start.costSoFar = 0f;
        float newCost = 0;
        float heuristic = 0;
        
        while (!pathFound)
        {
            if (queue.Count == 0) break;

            current = queue.Dequeue().parent;
            current.visited = true;

            if (current == end)
            {
                pathFound = true;
            }
            else
            {
                foreach (PathNode neighbour in current.neighbours)
                {
                    if (!neighbour.visited)
                    {
                        newCost = current.costSoFar + current.DistanceTo(neighbour);

                        if (queue.Contains(neighbour.node))
                        {
                            if (newCost < neighbour.costSoFar)
                            {
                                heuristic = neighbour.DistanceTo(end);
                                neighbour.costSoFar = newCost;
                                neighbour.cameFrom = current;
                                queue.UpdatePriority(neighbour.node, newCost + heuristic);
                            }
                        }
                        else
                        {
                            heuristic = neighbour.DistanceTo(end);
                            neighbour.costSoFar = newCost;
                            neighbour.cameFrom = current;
                            queue.Enqueue(neighbour.node, newCost + heuristic);
                        }
                    }
                }
            }


        }

        //Search has been done
        //If a path was found, navigate back from the endNode to the start while pushing them to the navigation stack
        if (pathFound)
        {
            current = end;

            current.inActivePath = true;
            currentPath.Push(current);
            current.RotateEndPoint();
            PathNode tmp;

            while (current != start)
            {
                tmp = current;
                current = current.cameFrom;
                current.inActivePath = true;
                currentPath.Push(current);
                current.Rotate(tmp);
                
            }
        }

    }

    //Shows line betweens nodes that are neighbours
    public void DrawLines()
    {
        PathNode[] childNodes = GetChildNodes();

        for(uint i = 0; i < childNodes.Length; i++)
        {
            for (uint j = i + 1; j < childNodes.Length; j++)
            {
                if (childNodes[i].neighbours.Contains(childNodes[j]))
                {
                    GameObject line = Instantiate(prefabLineRenderer, transform);
                    Vector3[] pos = new Vector3[] {childNodes[i].transform.position, childNodes[j].transform.position };
                    LineRenderer renderedLine = line.GetComponent<LineRenderer>();
                    renderedLine.SetPositions(pos);

                }
            }
        }
    }

    //Shows lines between one node and it's neighbours
    public void DrawLines(PathNode node)
    {
        foreach(PathNode otherNode in node.neighbours)
        {
            GameObject line = Instantiate(prefabLineRenderer, transform);
            LineRenderer renderedLine = line.GetComponent<LineRenderer>();
            Vector3[] pos = new Vector3[] { node.transform.position, otherNode.transform.position };
            renderedLine.SetPositions(pos);
                    
        }
    }

    //Removes all lines drawn by DrawLines
    public void RemoveLines()
    {
        GameObject[] lines = GameObject.FindGameObjectsWithTag("TempLine");
        foreach(GameObject obj in lines)
        {
            GameObject.Destroy(obj);
        }
    }

    //Listen voice commands
    public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        switch (eventData.RecognizedText.ToLower())
        {
            case "input":
                CreateNodeCommand();
                break;
            case "delete node":
                DeleteNodeCommand();
                break;
            case "draw lines":
                LineNeighboursCommand();
                break;
            case "edit":
                ToggleModeCommand();
                break;
            case "show menu":
                ShowNavigationMenuCommand();
                break;
        }
    }

    #region Voice Command Methods
    //Creates a new node
    public void CreateNodeCommand()
    {
        if (editMode)
        {
            CreateNode();
        }
    }

    //Creates a new object
    public void CreateObjectCommand()
    {
        if (editMode)
        {
            CreateObject();
        }
    }


    //Deletes the currently held node
    public void DeleteNodeCommand()
    {
        if (editMode)
        {
            RemoveNode();
        }
    }


    //Connects neighbouring nodes with lines
    public void LineNeighboursCommand()
    {
        if (editMode)
        {
            RemoveLines();
            PathNode node = activeObject == null ? null : activeObject.GetComponent<PathNode>();
            if(node != null)
            {
                DrawLines(node);
            }
            else{
                DrawLines();
            }
            
        }
    }


    //Toggle editMode
    public void ToggleModeCommand()
    {
        if (!IsChildBeingPlaced())
        {
            editMode = !editMode;
            if (!editMode)
            {
                RemoveLines();
            }
            else
            {
                GameObject.FindGameObjectWithTag("NodeMenu").GetComponent<NodeItemPlacer>().ResetMenu();
            }
            HoloToolkit.Unity.SpatialMapping.SpatialMappingManager.Instance.DrawVisualMeshes = editMode;
            foreach (PathNode childNode in GetComponentsInChildren<PathNode>())
            {
                childNode.ShowNode(editMode);
            }
        }
    }


    //Show the navigation menu
    public void ShowNavigationMenuCommand()
    {
        commandMenu.SetActive(!commandMenu.activeSelf);
        if (!editMode)
        {
            GameObject.FindGameObjectWithTag("NodeMenu").GetComponent<NodeItemPlacer>().ShowMenu();
        }
    }

#endregion
}
