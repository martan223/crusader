using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;

[XmlInclude(typeof(NextScene))]
public class GameItem{
    public Vector3 position;
    public Vector3 scale;
    public string Name;

    public GameItem() { }
    public GameItem(GameObject go)
    {
        position = go.transform.position;
        scale = go.transform.localScale;
        Name = go.name;
    }

    public virtual void Load(Item[] ItemsList)
    {
        GameObject.Instantiate(Resources.Load(Array.Find(ItemsList, (p => p.texture == Name.Split(';')[0])).texture), position, Quaternion.identity).name = Name;
        //Go.name = Name;
    }
    public void Start()
    {
        //GameObject.Instantiate(new GameObject(Name),position,Quaternion.identity);
    }
}
