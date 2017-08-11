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
using System.Threading.Tasks;
using Windows.Storage;
#endif
using System.ComponentModel;
public class ObjectMenu : MonoBehaviour {

    private class NameClass
    {
        public string name;
    }

    private List<string> objectNames = new List<string>();
    // Use this for initialization
    void Start() { 
	}
	// Update is called once per frame
	void Update () {

    }
#if NETFX_CORE
    void readConfig()
    {
        objectNames.Clear();
        string fullPath = Path.Combine(KnownFolders.CameraRoll.Path, "cfg.xml");

        try
        {
            XDocument objectList = XDocument.Load(fullPath);
            var names = from query in objectList.Descendants("Object")
                        select new NameClass
                        {
                            name = (string)query.Attribute("name")
                        };
            foreach (NameClass s in names)
            {
                objectNames.Add(s.name);
            }
        }
        catch (Exception s)
        {
            GameObject.FindGameObjectWithTag("CommandDisplayer").GetComponent<AllComands>().commandDisplayTime = 10000;
            GameObject.FindGameObjectWithTag("CommandDisplayer").GetComponent<AllComands>().TextLog(s.ToString());
            objectNames.Add("Example A");
            objectNames.Add("Example B");
            objectNames.Add("Example C");
        }
    }
#else
    void readConfig()
    { 
        objectNames.Clear();
        objectNames.Add("Example A");
        objectNames.Add("Example B");
        objectNames.Add("Example C");
    }
#endif

    public void Run()
    {
        
        if (transform.childCount == 0)
        {
            float tHeight = 0.8f;
#if NETFX_CORE
            var task = Task.Run(() => readConfig());
            task.Wait();
#else
            readConfig();
#endif
            foreach (string s in objectNames)
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
