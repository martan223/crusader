using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class fence_controller : MonoBehaviour {
    public List<Fence> fences;
	// Use this for initialization
	void Start () {
        fences = new List<Fence>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
            load();
	}

    void save()
    {
        string s;
        s = "index;start;end\n";
        for (int i = 0; i < fences.Count; i++)
        {
            s += i + ";" + fences[i].Stops[0].x + ":" + fences[i].Stops[0].y + ";" + fences[i].Stops[2].x + ":" + fences[i].Stops[2].y + "\n";
        }
        System.IO.File.WriteAllText(@"Assets/saves/fences.csv", s);
    }

    void load()
    {
        string[] s = System.IO.File.ReadAllLines(@"Assets/saves/fences.csv");
        for (int i = 1; i < s.Length; i++)
        {
            loadFence(new Vector2(float.Parse(s[i].Split(';')[1].Split(':')[0]), float.Parse(s[i].Split(';')[1].Split(':')[1])), new Vector2(float.Parse(s[i].Split(';')[2].Split(':')[0]), float.Parse(s[i].Split(';')[2].Split(':')[1])));
        }
        gameObject.GetComponent<Fence>().dot = GameObject.Find("Dot(Clone)");
    }

    void loadFence(Vector2 start, Vector2 stop)
    {
        Material mat = Resources.Load<Material>("blank");
        Fence f = new Fence();
        GameObject Dot = Resources.Load("Dot") as GameObject;
        //Instantiate(Dot);
        GameObject dot = GameObject.Find("Dot(Clone)");
        new GameObject("fence" + (fences.Count + 1).ToString());
        f.Stops = new List<Vector2>();
        f.Stops.Add(start);
        GameObject.Find("Dot(Clone)").name = "Dot" + f.Stops.Count;
        dot.transform.SetParent(GameObject.Find("fence" + (fences.Count + 1).ToString()).transform);
        dot.transform.position = start;
        dot = null;
        Instantiate(Dot);
        dot = GameObject.Find("Dot(Clone)");
        dot.transform.position = new Vector2(stop.x, start.y);
        GameObject.Find("Dot" + f.Stops.Count).AddComponent<LineRenderer>();
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().positionCount = 2;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().material = mat;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().startWidth = 0.25f;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().endWidth = 0.25f;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().SetPosition(0, start);
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().SetPosition(1, new Vector2(stop.x, start.y));
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().useWorldSpace = true;
        f.Stops.Add(new Vector2(stop.x, f.Stops[0].y));
        GameObject.Find("Dot(Clone)").name = "Dot" + f.Stops.Count;
        dot.transform.SetParent(GameObject.Find("fence" + (fences.Count + 1).ToString()).transform);
        dot = null;
        Instantiate(Dot);
        dot = GameObject.Find("Dot(Clone)");
        dot.transform.position = stop;
        GameObject.Find("Dot" + f.Stops.Count).AddComponent<LineRenderer>();
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().positionCount = 2;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().material = mat;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().startWidth = 0.25f;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().endWidth = 0.25f;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().SetPosition(0, f.Stops[f.Stops.Count - 1]);
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().SetPosition(1, new Vector2(stop.x, stop.y));
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().useWorldSpace = true;
        f.Stops.Add(stop);
        GameObject.Find("Dot(Clone)").name = "Dot" + f.Stops.Count;
        dot.transform.SetParent(GameObject.Find("fence" + (fences.Count + 1).ToString()).transform);
        dot = null;
        Instantiate(Dot);
        dot = GameObject.Find("Dot(Clone)");
        dot.transform.position = new Vector2(f.Stops[0].x, stop.y);
        GameObject.Find("Dot" + f.Stops.Count).AddComponent<LineRenderer>();
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().positionCount = 2;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().material = mat;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().startWidth = 0.25f;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().endWidth = 0.25f;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().SetPosition(0, stop);
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().SetPosition(1, new Vector2(f.Stops[0].x, stop.y));
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().useWorldSpace = true;
        f.Stops.Add(stop);
        GameObject.Find("Dot(Clone)").name = "Dot" + f.Stops.Count;
        dot.transform.SetParent(GameObject.Find("fence" + (fences.Count + 1).ToString()).transform);
        dot = null;
        Instantiate(Dot);
        dot = GameObject.Find("Dot(Clone)");
        dot.transform.position = f.Stops[0];
        GameObject.Find("Dot" + f.Stops.Count).AddComponent<LineRenderer>();
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().positionCount = 2;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().material = mat;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().startWidth = 0.25f;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().endWidth = 0.25f;
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().SetPosition(0, new Vector2(f.Stops[0].x, stop.y));
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().SetPosition(1, f.Stops[0]);
        GameObject.Find("Dot" + f.Stops.Count).GetComponent<LineRenderer>().useWorldSpace = true;
        for (int x = 0; x < Mathf.Round((Mathf.Abs(f.Stops[0].x - f.Stops[2].x) - 0.64f) / 0.64f); x++)
        {
            for (int y = 0; y < Mathf.Round((Mathf.Abs(f.Stops[0].y - f.Stops[2].y) - 0.64f) / 0.64f); y++)
            {
                //InnerFields.Add(new Vector2(f.Stops[0].x + x * 0.64f + 0.64f, f.Stops[0].y + y * 0.64f + 0.64f));
            }
        }
        for (int x = 0; x < Mathf.Round((Mathf.Abs(f.Stops[0].x - f.Stops[2].x) - 0.64f) / 0.64f); x++)
        {
            for (int y = 0; y < Mathf.Round((Mathf.Abs(f.Stops[0].y - f.Stops[2].y) - 0.64f) / 0.64f); y++)
            {
                Instantiate(Resources.Load("trava"), new Vector2(f.Stops[0].x + x * 0.64f + 0.64f, f.Stops[0].y + y * 0.64f + 0.64f), Quaternion.identity);
            }
        }
            f.built = true;
            GameObject.Find("Dot1").name = "DOT" + (fences.Count + 1).ToString() + 1;
            GameObject.Find("Dot2").name = "DOT" + (fences.Count + 1).ToString() + 2;
            GameObject.Find("Dot3").name = "DOT" + (fences.Count + 1).ToString() + 3;
            GameObject.Find("Dot4").name = "DOT" + (fences.Count + 1).ToString() + 4;
            fences.Add(f);
        }
    }

