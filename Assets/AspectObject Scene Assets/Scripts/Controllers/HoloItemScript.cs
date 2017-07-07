using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using UnityEngine.SceneManagement;
using System;

public class HoloItemScript : MonoBehaviour, IInputHandler
{
    [Tooltip("A list of all associated Aspects displayable in the enviroment")]
    public List<string> AspectNames = new List<string>();
    Vector3 Scaler = new Vector3(0.2f, 0.2f, 0.2f);
    private void Awake()
    {
        //Initialize as Object representation 
        gameObject.AddComponent<PathNode>();
        transform.Find("Specific Content").Find("Center Content").Find("Center_Canvas").Find("Text").GetComponent<Text>().text = gameObject.name;
        AspectNames = PopulateAspectNames();
        CreateAspectMenu();
    }
    void Start()
    {
        transform.Find("Always Present Graphics").Find("Border").GetComponent<MeshRenderer>().enabled = true;
        if (!transform.Find("Specific Content").Find("Center Content").gameObject.activeInHierarchy)
        {
            transform.Find("Specific Content").Find("Center Content").gameObject.SetActive(true);
        }
        transform.localScale = Scaler;
    } 
    void Update()
    {
        if (gameObject == GazeManager.Instance.HitObject)
        { 
            gameObject.transform.Find("Always Present Graphics").Find("Highlight").GetComponent<Renderer>().enabled = true;
        }
        else
        {
            gameObject.transform.Find("Always Present Graphics").Find("Highlight").GetComponent<Renderer>().enabled = false;
        }
    }
    List<string> PopulateAspectNames()
    {
        List<string> tempList = new List<string>();
            for (int i = 1; i <= 9; ++i)
            {
                tempList.Add("Aspect " + i.ToString());
            }
        return tempList;
    }
    void CreateAspectMenu()
    {
        float ItemLength = 0;
        float ItemHeight = 0;
        float LengthLimit = 600f;
        float HeightLimit = 100f;
        GameObject AspectMenu = Instantiate(Resources.Load("Aspect_Menu")) as GameObject; // The Associated Aspect menu 
        AspectMenu.transform.SetParent(transform, false);
        AspectMenu.name = "Aspect_Menu";

        foreach (string s in AspectNames)
        {
            GameObject Temp = Instantiate(Resources.Load("HoloItem_Menu"), AspectMenu.transform) as GameObject;
            Temp.name = s;
            Temp.transform.Find("Specific Content").Find("Center Content").Find("Center_Canvas").Find("Text").GetComponent<Text>().text = s;
            Text tmp = Temp.transform.Find("Specific Content").Find("Center Content").Find("Center_Canvas").Find("Text").GetComponent<Text>();
            if (tmp.preferredWidth > ItemLength && tmp.preferredWidth < LengthLimit) { ItemLength = tmp.preferredWidth; }
            if (tmp.preferredHeight > ItemHeight && tmp.preferredHeight < HeightLimit) { ItemHeight = tmp.preferredHeight; }
        } //Create Associated Aspect Menu Items from Prefabs    
        /*
        foreach (Transform t in AspectMenu.transform.GetComponentsInChildren<Transform>())
        {
            if (t.parent == AspectMenu.transform && t.gameObject != AspectMenu)
                {
                if (t.Find("Specific Content").Find("Center Content").Find("Center_Canvas").Find("Image") != null)
                {
                    RectTransform tmp = t.Find("Specific Content").Find("Center Content").Find("Center_Canvas").Find("Image").GetComponent<RectTransform>();
                    if (tmp.rect.width > ItemLength && tmp.rect.width < LengthLimit) { ItemLength = tmp.rect.width; }
                    if (tmp.rect.height > ItemHeight && tmp.rect.height < HeightLimit) { ItemHeight = tmp.rect.height; }
                }
                if (t.Find("Specific Content").Find("Center Content").Find("3D Object") != null)
                {
                    Transform tmp = t.Find("Specific Content").Find("Center Content").Find("3D Object");
                    if (tmp.localScale.x > ItemLength && tmp.localScale.x < LengthLimit) { ItemLength = tmp.localScale.x; }
                    if (tmp.localScale.y > ItemHeight && tmp.localScale.y < HeightLimit) { ItemHeight = tmp.localScale.y; }
                }
            }
        } //Get Optimal Item Length and Height from approved center content
        */
        float N_row = Mathf.CeilToInt((Mathf.Sqrt((float)AspectMenu.transform.childCount + ItemLength / ItemHeight)) * 0.618f);
        float N_Col = Mathf.CeilToInt((float)AspectMenu.transform.childCount / (float)N_row);
        float Rpos = 1-(N_row + 1) / 2;
        float Cpos = 1-(N_Col + 1) / 2;
        if (ItemHeight != 0 && ItemLength != 0)
        {
            foreach (Transform t in AspectMenu.transform.GetComponentsInChildren<Transform>())
            {
                if (t.parent == AspectMenu.transform && t.gameObject != AspectMenu)
                {
                    AlignItem(t.gameObject, N_Col, N_row, Cpos, Rpos, ItemHeight * Scaler.y, ItemLength * Scaler.y);
                    ResizeItem(t, ItemHeight * Scaler.y, ItemLength * Scaler.y);
                    if (Rpos >= (N_row + 1) / 2 - 1)
                    {
                        Rpos = 1 - (N_row + 1) / 2;
                        Cpos++;
                    }
                    else
                    {
                        Rpos++;
                    }
                }

            }
        }
    }
    void AlignItem(GameObject g, float N_Col, float N_row, float Cpos, float Rpos, float Item_Height, float Item_Length)
    {
        Vector3 CenterPoint = transform.root.position + new Vector3(0, 0, (-1 - N_Col * Item_Length) * transform.root.localScale.z);
        Vector3 MenuCenter = transform.root.position + new Vector3(0, 0, -1 * transform.root.localScale.z);
        float MenuRadius = Vector3.Distance(CenterPoint, MenuCenter);
        float deltaY = (N_row) / 2f - 2.1f * (Item_Height * Rpos);
        g.transform.position = MenuCenter + new Vector3(0f, deltaY, 0f) * transform.root.localScale.y;
        float deg = Mathf.Rad2Deg*
                    Mathf.Atan(Item_Length * 1.15f / MenuRadius)*
                    Cpos*
                    Mathf.Sqrt(
                        Mathf.Pow(transform.localScale.x, 2)
                        +
                        Mathf.Pow(transform.localScale.z, 2));
        g.transform.RotateAround(CenterPoint, Vector3.up, deg);
    }
    void ResizeItem(Transform t, float Item_Height, float Item_Length)
    {
        Vector3 BorderScale = new Vector3(Item_Length * 0.6f, Item_Height, Mathf.Min(Item_Height, Item_Length));
        t.Find("Always Present Graphics").Find("Border").localScale = BorderScale;
        t.GetComponent<BoxCollider>().size = BorderScale * 2f;
        Vector3 HighlightScale = new Vector3(Item_Length * 0.6f, Mathf.Min(Item_Height, Item_Length) / 10, (Item_Height));
        t.Find("Always Present Graphics").Find("Highlight").localScale = HighlightScale;
        t.Find("Specific Content").Find("Center Content").Find("Center_Canvas").GetComponent<RectTransform>().sizeDelta = new Vector2(Item_Length, Item_Height);
        t.Find("Specific Content").Find("Center Content").Find("Center_Canvas").Find("Text").GetComponent<RectTransform>().sizeDelta = new Vector2(Item_Length, Item_Height);
    }
    public void OnInputUp(InputEventData eventData)
    {
            GameObject g = transform.Find("Aspect_Menu").gameObject;
            g.SetActive(!g.activeSelf);

    }
    public void OnInputDown(InputEventData eventData)
    {
    }
}
