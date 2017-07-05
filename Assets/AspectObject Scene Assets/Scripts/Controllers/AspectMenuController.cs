using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

public class AspectMenuController : MonoBehaviour
{
    Object_Controller POS;
    public float ItemLength = 0; //Max Length
    public float ItemHeight = 0; //Max Height
    private float LengthLimit = 6f;
    private float HeightLimit = 1f;
    int Nrows;
    int NColumns;
    List<string> AspectNames = new List<string>();
    private Text tmp;
    // Use this for initialization
    void Start() {
        POS = gameObject.GetComponentInParent<Object_Controller>();
        if (POS != null)
        {
            AspectNames = POS.AspectNames;
        } //Get Aspect Names
        foreach (string s in AspectNames)
        {
                var Temp = Instantiate(Resources.Load("Aspect_Menu_Item")) as GameObject;
                Temp.transform.parent = gameObject.transform;
                Temp.name = s;
                Temp.transform.localScale = new Vector3(1, 1, 1);
                Temp.transform.Find("Aspect_Menu_Item_Canvas").transform.Find("Aspect_Menu_Item_Text").GetComponent<Text>().text = s;
        } //Create Associated Aspect Menu Items from Prefabs

        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Aspect_Menu_Item_Text"))
        {
            if (t.transform.root == transform.root)
            {
                tmp = t.GetComponent<Text>();
                if (tmp.preferredWidth> ItemLength && tmp.preferredWidth < LengthLimit) { ItemLength = tmp.preferredWidth; }
                if (tmp.preferredHeight> ItemHeight && tmp.preferredHeight < HeightLimit) { ItemHeight = tmp.preferredHeight; }
            }
        } // Get Menu Item Size from Name Lengths

        // Align Items Accordingly
        Nrows = Mathf.CeilToInt((Mathf.Sqrt((float)transform.childCount + ItemLength /ItemHeight))*0.618f);
        NColumns = Mathf.CeilToInt((float)transform.childCount / (float)Nrows);
        GameObject[,] AMIs = new GameObject[NColumns, Nrows]; // Aspect Menu Items

        float[] CanvasScale = new float[2];
        CanvasScale[0] = ItemLength;
        CanvasScale[1] = ItemHeight * 2.1f;

        for (int i = 0; i<transform.childCount; i++)
        {
            AMIs[(i-(i % Nrows))/Nrows,i % Nrows] = transform.GetChild(i).gameObject;
            transform.GetChild(i).Find("Aspect_Menu_Item_Canvas").SendMessage("UpdateSize", CanvasScale);
        } //Arrange Array according to gridview wishes

        Vector3 CenterPoint = transform.root.position + new Vector3(0, 0, (-1 - NColumns * ItemLength) * transform.root.localScale.z);
        Vector3 MenuCenter = transform.root.position + new Vector3(0, 0, -1 * transform.root.localScale.z); //Find Center of Menu
        float MenuRadius = Vector3.Distance(CenterPoint, MenuCenter);
        for (int i = 0; i < NColumns; i++)
        {
            for (int j = 0; j < Nrows; j++)
            {
                if (AMIs[i, j] != null)
                {

                    float ColumnPosition = (1f + i - ((float)NColumns + 1f) / 2f);
                    float RowPosition = (1f + j - ((float)Nrows + 1f) / 2f);
                    float deltaY = ((float)Nrows)/2f 
                                    - 
                                    2.1f*(ItemHeight * RowPosition);
                    AMIs[i, j].transform.position = MenuCenter + 
                                                    new Vector3(0f, deltaY, 0f) * transform.root.localScale.y;
                    float deg = Mathf.Rad2Deg 
                                * 
                                Mathf.Atan(ItemLength* 1.15f / MenuRadius) 
                                * 
                                ColumnPosition 
                                * 
                                Mathf.Sqrt(
                                    Mathf.Pow(transform.root.localScale.x,2) 
                                    + 
                                    Mathf.Pow(transform.root.localScale.z,2));
                    AMIs[i, j].transform.RotateAround(CenterPoint, Vector3.up, deg);
                }
            }
        } //Calculate Menu Item Positions
        transform.LookAt(Camera.main.transform);
        transform.Rotate(Vector3.up, 180f);
    }

    // Update is called once per frame
    void Update()
    {
        //Highlight on mouseover
    }
}
