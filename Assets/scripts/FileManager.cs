using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string[] Load(string path)
    {
        return File.ReadAllLines(path);
    }
    public Vector2[] LoadPositions(string path)
    {
        string[] files = File.ReadAllLines(path);
        Vector2[] converted = new Vector2[files.Length];
        for (int i = 0; i < files.Length; i++)
        {
            converted[i] = new Vector2(int.Parse(files[i].Split(';')[0]), int.Parse(files[i].Split(';')[1]));
        }
        return converted;
    }
}
