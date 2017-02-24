using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class action_scripts : MonoBehaviour {

    public string[] actions;
    public List<Vector2[]> positions;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void load(string path)
    {
        string[] files = File.ReadAllLines(path);
        actions = new string[files.Length];
        positions = new List<Vector2[]>();
        for (int i = 0; i < files.Length; i++)
        {
            string[] s = files[i].Split(';');
            actions[i] = s[0];
            Vector2[] v = new Vector2[s.Length - 1];
            for (int p = 0; p < s.Length - 1; p++)
            {
                v[p] = new Vector2(float.Parse(s[p+1].Split(':')[0]),float.Parse(s[p+1].Split(':')[1]));
            }
            positions.Add(v);

        }
    }
}
