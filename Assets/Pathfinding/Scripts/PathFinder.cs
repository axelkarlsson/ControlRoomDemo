using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PathFinder : MonoBehaviour , ISpeechHandler
{
    public string Path;
    //The object which is currently selected
    public GameObject activeObject = null;
    public GameObject prefabNode;

    //List of nodes that can be searched for destinations to navigate to    
    public List<PathNode> destinationNodes;
    //Sorted list of what node is next in the path
    public Stack<PathNode> currentPath = new Stack<PathNode>();
    public bool editMode;

    [Tooltip("After how many frames the camera node should check what neighbours it can see")]
    public int frameUpdateInterval;
    private int updateCount = 0;
    private int nodeId;
    private PathNode cameraNode;


    // Use this for initialization
    void Start()
    {
        //Register as a fallback listener for when no hologram is looked at
        InputManager.Instance.PushFallbackInputHandler(gameObject);

        //Create the node that follows the camera
        GameObject newNode = Instantiate(prefabNode, Camera.main.transform.position, Quaternion.identity);
        cameraNode = newNode.GetComponent<PathNode>();
        newNode.name = "Node " + nodeId;
        nodeId++;
        newNode.GetComponentInChildren<TapToPlaceNode>().IsBeingPlaced = false;
        
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
            //Raycast to nearby nodes to see where you can travel to
            UpdateNode(cameraNode);
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
                        Debug.Log("Time to switch scene");
                        SceneManager.LoadScene(1);
                    }
                }
                else
                {
                    currentPath.Push(upcoming);
                }
            }
        }


    }

    public void StartNavigation(PathNode destination)
    {
        if (!editMode)
        {
            FindPath(cameraNode, destination);
        }
    }

    //Resets all nodes in preparation to find a new path
    public void ResetNodes()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            PathNode test = transform.GetChild(i).gameObject.GetComponent<PathNode>();
            test.ResetNode();
        }
    }

 
    //Creates a new node to be placed. Returns null if an element is being moved
    public GameObject CreateNode()
    {
           
        //Don't do anything if another node is being placed
        if (ChildBeingPlaced()) { return null; }

        GameObject newNode = Instantiate(prefabNode, transform);
        PathNode newPathNode = newNode.GetComponent<PathNode>();
        newNode.name = "Node " + nodeId;
        nodeId++;

        //TMP
        destinationNodes.Add(newPathNode);
        //TMP

        return newNode;
    }

    //Returns if any child nodes are currently being moved
    private bool ChildBeingPlaced()
    {

        PathNode[] children = GetChildNodes();
        foreach (PathNode node in children)
        {
            if (node.GetComponentInChildren<TapToPlaceNode>().IsBeingPlaced) { return true; }
        }
        return false;
    }

    public PathNode[] GetChildNodes()
    {
        return GetComponentsInChildren<PathNode>();
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
        
        if (pathFound)
        {
            string test = "";
            current = end;

            test += current.transform.name;
            current.inActivePath = true;
            currentPath.Push(current);
            current.RotateEndPoint();
            PathNode tmp;

            while (current != start)
            {
                tmp = current;
                current = current.cameFrom;
                test += " " + current.transform.name + " ";
                current.inActivePath = true;
                currentPath.Push(current);
                current.Rotate(tmp);
                
            }
            Path = test;
        }

    }

    //Shows line betweens nodes that are neighbours
    //I am very proud that this worked on the first try!
    public void DrawLines()
    {
        PathNode[] childNodes = GetChildNodes();

        for(uint i = 0; i < childNodes.Length; i++)
        {
            for (uint j = i + 1; j < childNodes.Length; j++)
            {
                if (childNodes[i].neighbours.Contains(childNodes[j]))
                {
                    GameObject line = new GameObject();
                    line.tag = "TempLine";
                    LineRenderer renderedLine = line.AddComponent<LineRenderer>();
                    Vector3[] pos = new Vector3[] {childNodes[i].transform.position, childNodes[j].transform.position };
                    renderedLine.SetPositions(pos);
                    renderedLine.startWidth = 0.01f;
                    renderedLine.endWidth = 0.01f;
                    renderedLine.startColor = Color.blue;
                    renderedLine.endColor = Color.green;

                }
            }
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
    public void CreateNodeCommand()
    {
        if (editMode)
        {
            CreateNode();
        }
    }

    public void DeleteNodeCommand()
    {
        if (editMode)
        {
            RemoveNode();
        }
    }

    public void LineNeighboursCommand()
    {
        if (editMode)
        {
            RemoveLines();
            DrawLines();
        }
    }

    public void ToggleModeCommand()
    {
        if (!ChildBeingPlaced())
        {
            editMode = !editMode;
            if (!editMode)
            {
                RemoveLines();
            }
            HoloToolkit.Unity.SpatialMapping.SpatialMappingManager.Instance.DrawVisualMeshes = editMode;
            foreach (PathNode childNode in GetComponentsInChildren<PathNode>())
            {
                childNode.ShowNode(editMode);
            }
        }
    }

    public void ShowNavigationMenuCommand()
    {
        GameObject.FindGameObjectWithTag("NodeMenu").GetComponent<NodeItemPlacer>().ShowMenu();
    }

#endregion
}
