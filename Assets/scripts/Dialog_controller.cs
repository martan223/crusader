using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_controller : MonoBehaviour {
    public string[] dialogue;
    public int phrase;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Load(string path)
    {
        phrase = 0;
        dialogue = System.IO.File.ReadAllText("Assets/saves/DIALOGS/" + path + ".txt").Split(';');
    }
}
