using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour {

    public List<GameItem> Layer1;
    public List<GameItem> Layer2;
    public List<GameItem> Layer3;
    Item[] ItemsList { get{return Scene_Controller.ItemsList;}}
	// Use this for initialization
	public void Load (string path) {
            Layer1 = Layer2 = Layer3 = new List<GameItem>();
            XmlManager<List<GameItem>> xm = new XmlManager<List<GameItem>>();
            Layer1 = xm.Load("Assets/saves/" + path + "/layer1.xml");
            for (int i = 0; i < Layer1.Count; i++)
            {
                Layer1[i].Load(ItemsList);
            }
            Layer2 = xm.Load("Assets/saves/" + path + "/layer2.xml");
            for (int i = 0; i < Layer2.Count; i++)
            {
                Layer2[i].Load(ItemsList);
            }
            Layer3 = xm.Load("Assets/saves/" + path + "/layer3.xml");
            for (int i = 0; i < Layer3.Count; i++)
            {
                Layer3[i].Load(ItemsList);
            }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
