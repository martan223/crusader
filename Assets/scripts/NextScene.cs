using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NextScene : GameItem{
    public string nextScene;
    public Vector2 pos;
	// Use this for initialization
    public NextScene() { }
    public NextScene(GameObject go, string parameter)
    {
        position = go.transform.position;
        scale = go.transform.localScale;
        Name = go.name;
        nextScene = parameter.Split(';')[0];
        pos = new Vector2(float.Parse(parameter.Split(';')[1]),float.Parse(parameter.Split(';')[2]));
    }
	// Update is called once per frame
	void Update () {
	    	
	}
}