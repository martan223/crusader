﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PathEditor : MonoBehaviour {
    Vector2 worldPoint;
    public Button but,butLoad,butClear;
    public Toggle tog;
    public InputField inpf;
    public Material mat;
    public GameObject Dot,dot;
    public List<Vector2> Stops;
    GameObject positionText;
    public bool closed;
	// Use this for initialization
	void Start () {
        //but = GameObject.Find("Button").GetComponent<Button>();
        but.onClick.AddListener(SavePath);
        butLoad.onClick.AddListener(LoadPath);
        butClear.onClick.AddListener(Clear);
        Instantiate(Dot);
        dot = GameObject.Find("Dot(Clone)");
        positionText = GameObject.Find("Position");
        positionText.GetComponent<Text>();
        
	}

    //save path to file
	void SavePath()
    {
        string path = GameObject.Find("Path").GetComponent<InputField>().text;
        StreamWriter st = new StreamWriter("Assets/saves/action_scripts/" + path + ".csv");
        st.WriteLine(closed);
        for (int i = 0; i < Stops.Count; i++)
        {
            st.WriteLine(Stops[i].x+";"+Stops[i].y);
        }
        
        st.Flush();
        Debug.Log("saved to: " + path);
    }

    //loads path from file
    void LoadPath()
    {
        //clear workspace
        Clear();
        Stops.Clear();
        //add dots with lines
        string[] files = File.ReadAllLines("Assets/saves/action_scripts/" + GameObject.Find("Path").GetComponent<InputField>().text + ".csv");
        closed = bool.Parse(files[0]);
        for (int i = 1; i < files.Length; i++)
        {
            string[] s = files[i].Split(';');
            Vector2 v = new UnityEngine.Vector2(float.Parse(s[0]), float.Parse(s[1]));
            if (Stops.Count > 0)
            {
                GameObject.Find("Dot" + Stops.Count).AddComponent<LineRenderer>();
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().positionCount = 2;
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().material = mat;
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().startWidth = 0.25f;
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().endWidth = 0.25f;
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().SetPosition(0, Stops[Stops.Count - 1]);
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().SetPosition(1, v);
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().useWorldSpace = true;
            }

            dot.transform.position = v;
            Stops.Add(v);
            GameObject.Find("Dot(Clone)").name = "Dot" + Stops.Count;
            dot = null;
            Instantiate(Dot);
            dot = GameObject.Find("Dot(Clone)");
        }
    }

    //clear workspace
    void Clear()
    {
        for (int i = 1; i < Stops.Count+1; i++)
        {
            DestroyImmediate(GameObject.Find("Dot" + i));
        }
        Stops.Clear();
    }
	// Update is called once per frame
	void Update () {
        positionText.GetComponent<Text>().text = dot.transform.position.x +" : " + dot.transform.position.y.ToString();
        if (Stops.Count == 0)
        {
            worldPoint = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x/* - Input.mousePosition.x % float.Parse(inpf.text) + float.Parse(inpf.text)/2*/, Input.mousePosition.y/* - Input.mousePosition.y % float.Parse(inpf.text) + float.Parse(inpf.text) / 2*/));
            worldPoint = new Vector2(worldPoint.x - worldPoint.x % float.Parse(inpf.text) + float.Parse(inpf.text) / 2, worldPoint.y - worldPoint.y % float.Parse(inpf.text) + float.Parse(inpf.text) / 2);
        }
        else
        {
            worldPoint = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x/* - Input.mousePosition.x % float.Parse(inpf.text) + float.Parse(inpf.text)/2*/, Input.mousePosition.y/* - Input.mousePosition.y % float.Parse(inpf.text) + float.Parse(inpf.text) / 2*/));
            if (Mathf.Abs(Stops[Stops.Count - 1].x - worldPoint.x) < Mathf.Abs(Stops[Stops.Count - 1].y - worldPoint.y))
                worldPoint = new Vector2(Stops[Stops.Count - 1].x, worldPoint.y - worldPoint.y % float.Parse(inpf.text) + float.Parse(inpf.text) / 2);
            else
                worldPoint = new Vector2(worldPoint.x - worldPoint.x % float.Parse(inpf.text) + float.Parse(inpf.text) / 2, Stops[Stops.Count - 1].y);
        }
        dot.transform.position = worldPoint;
       
        //remove point
        if(Input.GetMouseButtonDown(1) && Stops.Count > 0)
        {
            Destroy(GameObject.Find("Dot" + (Stops.Count)));
            Stops.RemoveAt(Stops.Count - 1);
        }
        //add new point
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            
            if(Stops.Count > 0)
            {
                GameObject.Find("Dot" + Stops.Count).AddComponent<LineRenderer>();
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().positionCount = 2;
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().material = mat;
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().startWidth= 0.25f;
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().endWidth = 0.25f;
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().SetPosition(0, Stops[Stops.Count-1]);
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().SetPosition(1, worldPoint);
                GameObject.Find("Dot" + Stops.Count).GetComponent<LineRenderer>().useWorldSpace = true;
                if (Stops.Contains(worldPoint))
                    closed = true;
            }
            Stops.Add(worldPoint);
            GameObject.Find("Dot(Clone)").name = "Dot" + Stops.Count;
            dot = null;
            Instantiate(Dot);
            dot = GameObject.Find("Dot(Clone)");
            dot.transform.position = worldPoint;
        }
	}
}
