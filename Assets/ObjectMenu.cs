using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
#if NETFX_CORE
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.System;
using Windows.Storage;
#endif
using System.ComponentModel;
public class ObjectMenu : MonoBehaviour {
    private class Object_
    {
        public string name;
        public List<object> Aspects;
    }

    // Use this for initialization
    void Start() { 
	}
	// Update is called once per frame
	void Update () {

    }
#if NETFX_CORE
    async List<string> readConfig()
    {
        string fullPath = Path.Combine(KnownFolders.CameraRoll, "cfg.xml")´;
        bool fileExists = true;
        try
        {
            StorageFile s = await StorageFile.GetFileFromPathAsync(fullPath);
        }
        catch (FileNotFoundException)
        {
            fileExists = false;
        }

        if (fileExists)
        {
            XDocument objectList = XDocument.Load(fullPath);
            var Objects = from query in objectList.Descendants("Object")
                             select new Object_;
            return Objects.Select(x=>x.name).ToList();
        }
        else
        {
        return new List<string>
        {
            "Non-existant Object A",
            "Non-existant Object B",
            "Non-existant Object C"
        };
        }
    }
#else
    List<string> readConfig()
    { 
        return new List<string>
        {
            "Non-existant Object A",
            "Non-existant Object B",
            "Non-existant Object C"
        };
    }
#endif

    public void Run()
    {
        
        if (transform.childCount == 0)
        {
            float tHeight = 0.8f;
            foreach (string s in readConfig())
            {
                GameObject g = Instantiate(Resources.Load("ObjectMenuItem") as GameObject, transform);
                g.name = s;
                Text t = g.GetComponent<Text>();
                t.text = s;
                g.transform.localPosition = new Vector3(0, tHeight, 0);
                tHeight -= 0.2f;
                ObjectMenuItem i = g.GetComponent<ObjectMenuItem>();
                i.setFor = transform.parent.gameObject;
            }
        }
    }

}
