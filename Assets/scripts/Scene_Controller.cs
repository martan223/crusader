using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Controller : MonoBehaviour
{

    public static Item[] ItemsList;
    public static Inventory ItemSheet;
    public static Scene scene;
    public static bool transition;
    public static ScreenTransition scrtransition;
    public static bool pause;
    public static GameObject player;
    public static string SceneName;
    public static bool Freze;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("player");
        string[] q = System.IO.File.ReadAllLines(@"Assets/saves/items_list.csv");
        ItemsList = new Item[q.Length - 1];
        for (int i = 1; i < q.Length; i++)
        {
            string s = q[i];
            ItemsList[i - 1] = new Item();
            ItemsList[i - 1].name = s.Split(';')[1];
            ItemsList[i - 1].texture = s.Split(';')[1];
            ItemsList[i - 1].Layer = int.Parse(s.Split(';')[3]);
            ItemsList[i - 1].colliding = bool.Parse(s.Split(';')[4]);
            ItemsList[i - 1].parameter = bool.Parse(s.Split(';')[5]);
            s = s.Split(';')[2];
            if (s == 1.ToString())
                ItemsList[i - 1].movable = true;

        }
        //uvodni scena
        scene = new Scene();
        scrtransition = new FadeOut(10f, 10F, "cb_home", true, new Vector2(3, 4));
        transition = true;
        ItemSheet = new Inventory();
        ItemSheet.LoadItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (transition)
        {
            GameObject.Find("Main Camera").GetComponent<GUITexture>().pixelInset = new Rect(new Vector2(-GameObject.Find("Main Camera").GetComponent<Camera>().pixelWidth * GameObject.Find("Main Camera").transform.position.x, -GameObject.Find("Main Camera").GetComponent<Camera>().pixelHeight * GameObject.Find("Main Camera").transform.position.y), new Vector2(1000, 1000));
            scrtransition.Update();
        }
    }
}
