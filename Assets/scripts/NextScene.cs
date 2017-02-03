using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NextScene : GameItem{
    public string nextScene;
	// Use this for initialization
    public NextScene() { }
    public NextScene(GameObject go, string path)
    {
        position = go.transform.position;
        scale = go.transform.localScale;
        Name = go.name;
        nextScene = path;
    }
	// Update is called once per frame
	void Update () {
	    	
	}
}