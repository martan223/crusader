using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Controller : MonoBehaviour {

    public static Item[] ItemsList;
    public static Scene scene;
    public static bool transition;
    public static ScreenTransition scrtransition;
	// Use this for initialization
	void Start () {
        string[] q = System.IO.File.ReadAllLines(@"Assets/saves/items_list.csv");
        ItemsList = new Item[q.Length - 1];
        for (int i = 1; i < q.Length; i++)
        {
            string s = q[i];
            ItemsList[i - 1] = new Item();
            ItemsList[i - 1].name = s.Split(';')[1];
            ItemsList[i - 1].texture = s.Split(';')[1];
            ItemsList[i - 1].Layer = int.Parse(s.Split(';')[3]);
            s = s.Split(';')[2];
            if (s == 1.ToString())
                ItemsList[i - 1].movable = true;

        } 
        scene = new Scene();
        //scrtransition = new FadeOut(10f, 0.1F, "test_scene",true);
        //transition = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(transition)
        {
            scrtransition.Update();
        }
	}
}
