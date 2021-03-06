﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.IO;

public class map_editor : MonoBehaviour
{

    Item[] ItemsList { get { return Scene_Controller.ItemsList; } }
    public float oneTile;
    List<GameObject> ItemsMap;
    bool show = false;
    RaycastHit2D hit;
    public GameObject MOVING;
    Vector2 worldPoint;
    Material blankmaterial;
    Scene scene;
    public string path,parameter;
    // in-game ui
    public Button load, clear, save;
    public InputField parameterField, MapPath, OneTile;
    // Use this for initialization
    void Start()
    {
        load.onClick.AddListener(Load);
        save.onClick.AddListener(Save);
        clear.onClick.AddListener(Clear);
        OneTile.onValueChanged.AddListener(delegate { oneTileChange(); });
        ItemsMap = new List<GameObject>();
        parameterField.onValueChanged.AddListener(delegate { parameterChange(); });
        MapPath.onValueChanged.AddListener(delegate { pathChange(); });
        scene = Scene_Controller.scene;
        MapPath.text = "test_scene";
        path = "test_scene";
        oneTile = 0.64f;
        OneTile.text = oneTile.ToString();
        //DESERIALIZE

        //READ TILES FROM TABLE
        //INSTANTIATIZE
        for (int i = 0; i < ItemsList.Length; i++)
        {
            ItemsMap.Add((GameObject)GameObject.Instantiate(Resources.Load(ItemsList[i].texture), new Vector3(), Quaternion.identity));
            //Debug.Log(Resources.Load(ItemsList[i].texture) + " good");
            ItemsMap[i].transform.position = new Vector3(i * oneTile, GameObject.Find("Main Camera").GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.0F, 1.0F, GameObject.Find("Main Camera").GetComponent<Camera>().nearClipPlane)).y - oneTile / 2);
            ItemsMap[i].transform.SetParent(GameObject.Find("meCanvas").GetComponent<Transform>());
            GameObject.Find("meCanvas").transform.GetChild(GameObject.Find("meCanvas").transform.childCount-1).gameObject.tag = "editortool";
        }

        //GRID
        Grid();
    }

    void Update()
    {
        //LOAD F9
        if (Input.GetKeyDown(KeyCode.F9))
        {
            Load();
        }
        //SAVE F5
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Save();
        }

        //INITIALIZE CAMERA
        Vector2 worldPoint = new Vector2((float)(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (float)(Camera.main.ScreenToWorldPoint(Input.mousePosition).y/* - (Camera.main.ScreenToWorldPoint(Input.mousePosition).y % oneTile + oneTile / 2)*/));
        hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        //SHOW/HIDE EDITOR
        if (Input.GetKeyDown(KeyCode.Home) && show == false)
        {
            show = true;
            for (int i = 0; i < GameObject.Find("map_editor").transform.childCount; i++)
            {
                GameObject.Find("map_editor").transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
            if (Input.GetKeyDown(KeyCode.Home) && show == true)
            {
                show = false;
                for (int i = 0; i < GameObject.Find("map_editor").transform.childCount; i++)
                {
                    GameObject.Find("map_editor").transform.GetChild(i).gameObject.SetActive(false);
                }

            }
        if (show)
        {
            if (Input.GetAxis("Mouse ScrollWheel")>0) // forward
            {
                GameObject[] g = GameObject.FindGameObjectsWithTag("editortool");
                for (int i = 0; i < g.Length; i++)
                {
                    g[i].transform.position = new Vector3(g[i].transform.position.x + 0.64f, g[i].transform.position.y);
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel")<0) // backwards
            {
                GameObject[] g = GameObject.FindGameObjectsWithTag("editortool");
                for (int i = 0; i < g.Length; i++)
                {
                    g[i].transform.position = new Vector3(g[i].transform.position.x - 0.64f, g[i].transform.position.y);
                }
            }
        }
        move();
    }

    //UPDATE MOVING
    void move()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MOVING = null;
            if (GameObject.Find("moving") != null)
                GameObject.Find("moving").GetComponent<Text>().text = "";
            Debug.Log(scene.Layer1.Count);
            Debug.Log(scene.Layer2.Count);
            Debug.Log(scene.Layer3.Count);
        }
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //MOVE CAMERA
        //Vector3 cam = GameObject.Find("Main Camera").transform.position;
        //if (Input.GetKey(KeyCode.W))
        //    GameObject.Find("Main Camera").transform.position = new Vector3(cam.x, cam.y + 0.1f, -10);
        //if (Input.GetKey(KeyCode.S))
        //    GameObject.Find("Main Camera").transform.position = new Vector3(cam.x, cam.y - 0.1f, -10);
        //if (Input.GetKey(KeyCode.A))
        //    GameObject.Find("Main Camera").transform.position = new Vector3(cam.x - 0.1f, cam.y, -10);
        //if (Input.GetKey(KeyCode.D))
        //    GameObject.Find("Main Camera").transform.position = new Vector3(cam.x + 0.1f, cam.y, -10);
        //PICK TILE

        if (hit.collider != null && hit.collider.tag == "editortool" && Input.GetMouseButton(0))
        {
            MOVING = hit.collider.gameObject;
        }
        else
            if (Input.GetMouseButton(0) && MOVING != null)
            {
                string nam = MOVING.name.Remove(MOVING.name.Length - 7) + ";" + new Vector2((worldPoint.x - worldPoint.x % oneTile + oneTile / 2) / oneTile + 0.5f, (worldPoint.y - worldPoint.y % oneTile + oneTile / 2) / oneTile + 0.5f);
                if (!Array.Find<Item>(ItemsList, p => p.name == MOVING.name.ToString().Remove(MOVING.name.ToString().Length - 7)).parameter)
                {
                    switch (Array.Find<Item>(ItemsList, p => p.name == MOVING.name.ToString().Remove(MOVING.name.ToString().Length - 7)).Layer)
                    {
                        case 1:
                            {
                                if (!scene.Layer1.Exists(p => p.position == new Vector3(worldPoint.x - worldPoint.x % oneTile + oneTile / 2, worldPoint.y - worldPoint.y % oneTile + oneTile / 2, 3)))
                                {
                                    GameObject.Instantiate(MOVING).name = nam;
                                    GameObject.Find(nam).transform.position = new Vector3(worldPoint.x - worldPoint.x % oneTile + oneTile / 2, worldPoint.y - worldPoint.y % oneTile + oneTile / 2, 3);
                                    GameObject.Find(nam).transform.localScale = new Vector3(1, 1, 1);
                                    GameObject.Find(nam).tag = "Untagged";
                                    //GameObject.Find(nam).GetComponent<BoxCollider2D>().isTrigger = true;
                                    scene.Layer1.Add(new GameItem(GameObject.Find(nam)));
                                    Debug.Log(GameObject.Find(nam).transform.position + "layer1" + new Vector3(worldPoint.x - worldPoint.x % oneTile + oneTile / 2, worldPoint.y - worldPoint.y % oneTile + oneTile / 2));
                                }
                            }
                            break;
                        case 2:
                            {
                                if (!scene.Layer2.Exists(p => p.position == new Vector3(worldPoint.x - worldPoint.x % oneTile + oneTile / 2, worldPoint.y - worldPoint.y % oneTile + oneTile / 2, 2)))
                                {
                                    GameObject.Instantiate(MOVING).name = nam;
                                    GameObject.Find(nam).transform.position = new Vector3(worldPoint.x - worldPoint.x % oneTile + oneTile / 2, worldPoint.y - worldPoint.y % oneTile + oneTile / 2, 2);
                                    GameObject.Find(nam).transform.localScale = new Vector3(1, 1, 1);
                                    GameObject.Find(nam).tag = "Untagged";
                                    scene.Layer2.Add(new GameItem(GameObject.Find(nam)));
                                    Debug.Log("layer2");
                                }
                            }
                            break;
                        case 3:
                            {
                                if (!scene.Layer3.Exists(p => p.position == new Vector3(worldPoint.x - worldPoint.x % oneTile + oneTile / 2, worldPoint.y - worldPoint.y % oneTile + oneTile / 2, 1)))
                                {
                                    GameObject.Instantiate(MOVING).name = nam;
                                    GameObject.Find(nam).transform.position = new Vector3(worldPoint.x - worldPoint.x % oneTile + oneTile / 2, worldPoint.y - worldPoint.y % oneTile + oneTile / 2, 1);
                                    GameObject.Find(nam).transform.localScale = new Vector3(1, 1, 1);
                                    GameObject.Find(nam).tag = "Untagged";
                                    scene.Layer3.Add(new GameItem(GameObject.Find(nam)));
                                    Debug.Log("layer3");
                                }
                            }
                            break;
                    }
                }
                else
                {
                    //Debug.Log(Array.Find<Item>(ItemsList, p => p.name == MOVING.name.ToString().Remove(MOVING.name.ToString().Length - 7)).name);
                    switch(Array.Find<Item>(ItemsList, p => p.name == MOVING.name.ToString().Remove(MOVING.name.ToString().Length - 7)).name)
                    {
                        case "doors_1":
                            {
                                if (!scene.Layer2.Exists(p => p.position == new Vector3(worldPoint.x - worldPoint.x % oneTile + oneTile / 2, worldPoint.y - worldPoint.y % oneTile + oneTile / 2, 2)))
                                {
                                    GameObject.Instantiate(MOVING).name = nam;
                                    GameObject.Find(nam).transform.position = new Vector3(worldPoint.x - worldPoint.x % oneTile + oneTile / 2, worldPoint.y - worldPoint.y % oneTile + oneTile / 2, 2);
                                    GameObject.Find(nam).transform.localScale = new Vector3(1, 1, 1);
                                    GameObject.Find(nam).tag = "Untagged";
                                    scene.Layer2.Add(new NextScene(GameObject.Find(nam),parameter));
                                }
                            }
                            break;
                        case "doors_hidden":
                            {
                                if (!scene.Layer2.Exists(p => p.position == new Vector3(worldPoint.x - worldPoint.x % oneTile + oneTile / 2, worldPoint.y - worldPoint.y % oneTile + oneTile / 2, 2)))
                                {
                                    GameObject.Instantiate(MOVING).name = nam;
                                    GameObject.Find(nam).transform.position = new Vector3(worldPoint.x - worldPoint.x % oneTile + oneTile / 2, worldPoint.y - worldPoint.y % oneTile + oneTile / 2, 2);
                                    GameObject.Find(nam).transform.localScale = new Vector3(1, 1, 1);
                                    GameObject.Find(nam).tag = "Untagged";
                                    scene.Layer2.Add(new NextScene(GameObject.Find(nam), parameter));
                                }
                            }
                            break;
                    }
                }
            }
        if (MOVING != null)
            GameObject.Find("moving").GetComponent<Text>().text = MOVING.name.Remove(MOVING.name.Length - 7) + new Vector2((worldPoint.x - worldPoint.x % oneTile + oneTile / 2) / oneTile + 0.5f, (worldPoint.y - worldPoint.y % oneTile + oneTile / 2) / oneTile + 0.5f);
        //REMOVE
        if (Input.GetMouseButton(1) && hit.collider != null && hit.collider.tag == "Untagged")
        {
            scene.Layer1.Remove(scene.Layer1.Find(p => p.Name == hit.collider.name));
            scene.Layer2.Remove(scene.Layer2.Find(p => p.Name == hit.collider.name));
            scene.Layer3.Remove(scene.Layer3.Find(p => p.Name == hit.collider.name));
            Destroy(hit.collider.gameObject);
        }
    }
    //if (before && !buildpipes && MOVING != null)
    //{
    //    //clicked
    //    //MOVING.transform.position = worldPoint;
    //    if (Input.GetMouseButton(0) && !bepress)
    //    {
    //        before = false;
    //        MOVING.gameObject.tag = "Untagged";
    //        MOVING = null;
    //        bepress = true;

    //    }
    //    else
    //        if (!Input.GetMouseButton(0) && bepress)
    //            bepress = false;
    //}
    //if (Input.GetMouseButton(0))
    //{

    //    if (hit.collider != null)
    //    {
    //        before = true;
    //        bepress = true;
    //        MOVING = Instantiate(Resources.Load(hit.collider.name.ToString().Remove(hit.collider.name.ToString().Length - 7)), new Vector3(worldPoint.x, worldPoint.y), Quaternion.identity) as GameObject;
    //        Debug.Log("new");
    //    }
    //}
    //if (MOVING != null && !buildpipes)
    //{
    //    if (worldPoint.x < 0 && worldPoint.y < 0)
    //        MOVING.transform.position = new Vector2(((float)worldPoint.x - (worldPoint.x % oneTile - (Array.Find(ItemsList, (p => p.texture == MOVING.name.Remove(MOVING.name.Length - 7))).width / 2f) * oneTile + oneTile)), ((float)worldPoint.y - (worldPoint.y % oneTile - (Array.Find(ItemsList, (p => p.texture == MOVING.name.Remove(MOVING.name.Length - 7))).height / 2f) * oneTile + oneTile)));
    //    if (worldPoint.x > 0 && worldPoint.y < 0)
    //        MOVING.transform.position = new Vector2(((float)worldPoint.x - (worldPoint.x % oneTile - (Array.Find(ItemsList, (p => p.texture == MOVING.name.Remove(MOVING.name.Length - 7))).width / 2f) * oneTile)), ((float)worldPoint.y - (worldPoint.y % oneTile - (Array.Find(ItemsList, (p => p.texture == MOVING.name.Remove(MOVING.name.Length - 7))).height / 2f) * oneTile + oneTile)));
    //    if (worldPoint.x < 0 && worldPoint.y > 0)
    //        MOVING.transform.position = new Vector2(((float)worldPoint.x - (worldPoint.x % oneTile - (Array.Find(ItemsList, (p => p.texture == MOVING.name.Remove(MOVING.name.Length - 7))).width / 2f) * oneTile + oneTile)), ((float)worldPoint.y - (worldPoint.y % oneTile - (Array.Find(ItemsList, (p => p.texture == MOVING.name.Remove(MOVING.name.Length - 7))).height / 2f) * oneTile)));
    //    if (worldPoint.x > 0 && worldPoint.y > 0)
    //        MOVING.transform.position = new Vector2(((float)worldPoint.x - (worldPoint.x % oneTile - (Array.Find(ItemsList, (p => p.texture == MOVING.name.Remove(MOVING.name.Length - 7))).width / 2f) * oneTile)), ((float)worldPoint.y - (worldPoint.y % oneTile - (Array.Find(ItemsList, (p => p.texture == MOVING.name.Remove(MOVING.name.Length - 7))).height / 2f) * oneTile)));
    //    Debug.Log((float)(Array.Find(ItemsList, (p => p.texture == MOVING.name.Remove(MOVING.name.Length - 7))).width/2f));
    //}
    void Grid()
    {
        GameObject grid = new GameObject();
        grid.name = "grid";
        grid.transform.SetParent(GameObject.Find("map_editor").GetComponent<Transform>());
        for (int i = 0; i < 26; i++)
        {
            GameObject line = new GameObject();
            line.transform.SetParent(GameObject.Find("grid").GetComponent<Transform>());
            line.transform.position = new Vector3(0, i * oneTile, 0);
            line.AddComponent<LineRenderer>();
            LineRenderer lr = line.GetComponent<LineRenderer>();
            lr.materials[0] = blankmaterial;
            lr.SetWidth(0.01f, 0.01f);
            lr.SetPosition(0, new Vector3(0, i * oneTile, 0));
            lr.SetPosition(1, new Vector3(25, i * oneTile, 0));
        }
        for (int i = 0; i < 26; i++)
        {
            GameObject line = new GameObject();
            line.transform.SetParent(GameObject.Find("grid").GetComponent<Transform>());
            line.transform.position = new Vector3(i * oneTile, 0, 0);
            line.AddComponent<LineRenderer>();
            LineRenderer lr = line.GetComponent<LineRenderer>();
            lr.materials[0] = blankmaterial;
            lr.SetWidth(0.01f, 0.01f);
            lr.SetPosition(0, new Vector3(i * oneTile, 0, 0));
            lr.SetPosition(1, new Vector3(i * oneTile, 25, 0));
        }
        for (int i = 0; i < GameObject.Find("map_editor").transform.childCount; i++)
        {
            GameObject.Find("map_editor").transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void Load()
    {
        Scene_Controller.scene.Load(path);
    }
    void Save()
    {
        if (!Directory.Exists("Assets/saves/" + path))
            Directory.CreateDirectory("Assets/saves/" + path);
        XmlManager<List<GameItem>> xm = new XmlManager<List<GameItem>>();
        xm.Save("Assets/saves/" + path + "/layer1.xml", scene.Layer1);
        xm.Save("Assets/saves/" + path + "/layer2.xml", scene.Layer2);
        xm.Save("Assets/saves/" + path + "/layer3.xml", scene.Layer3);
    }
    void Clear()
    {
        for (int i = 0; i < scene.Layer1.Count; i++)
        {
            Destroy(GameObject.Find(scene.Layer1[i].Name));
        }
        for (int i = 0; i < scene.Layer2.Count; i++)
        {
            Destroy(GameObject.Find(scene.Layer2[i].Name));
        }
        for (int i = 0; i < scene.Layer3.Count; i++)
        {
            Destroy(GameObject.Find(scene.Layer3[i].Name));
        }
        scene.Layer1 = new List<GameItem>();
        scene.Layer2 = new List<GameItem>();
        scene.Layer3 = new List<GameItem>();
    }
    void parameterChange()
    {
        parameter = parameterField.text;
    }
    void pathChange()
    {
        Debug.Log(MapPath.text);
        path = MapPath.text;
    }
    void oneTileChange()
    {
        oneTile = float.Parse(OneTile.text);
        Debug.Log(oneTile);
    }
}
