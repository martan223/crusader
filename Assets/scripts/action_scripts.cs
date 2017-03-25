using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class action_scripts : MonoBehaviour {

    public string[] actions;
    public List<string[]> parameters;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void load(string path)
    {
        string[] files = File.ReadAllLines(path);
        actions = new string[files.Length];
        parameters = new List<string[]>();
        for (int i = 0; i < files.Length; i++)
        {
            string[] s = files[i].Split(';');
            actions[i] = s[0];
            if (actions[i].Contains("#"))
            {
                string[] q = new string[s.Length-1];
                for (int w = 0; w < s.Length-2; w++)
                {
                    q[w] = s[w + 1];
                }
                    parameters.Add(q);
            }
            else 
                parameters.Add(new string[0]);
        }
    }
}
