using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class map_editor : MonoBehaviour {

    public Item[] ItemsList;
    float oneTile { get { return 0.64f/*GameObject.Find("Main Camera").GetComponent<onclick>().oneTile; }*/; } set { }/*{ GameObject.Find("Main Camera").GetComponent<onclick>().oneTile = value; }*/ }
    public List<GameObject> ItemsMap;
    public bool show = false;
    public RaycastHit2D hit;
    public GameObject MOVING;
    Vector2 worldPoint;
    public Material blankmaterial;
    Scene scene;
    // Use this for initialization
	void Start() 
    {
        scene = Scene_Controller.scene;
        //DESERIALIZE
        
        //READ TILES FROM TABLE
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
            //INSTANTIATIZE
            scene.Layer1 = new List<GameItem>();
            scene.Layer2 = new List<GameItem>();
            scene.Layer3 = new List<GameItem>();
            for (int i = 0; i < ItemsList.Length; i++)
            {
                ItemsMap.Add((GameObject)GameObject.Instantiate(Resources.Load(ItemsList[i].texture), new Vector3(), Quaternion.identity));
                ItemsMap[i].transform.position = new Vector3(i * oneTile, GameObject.Find("Main Camera").GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.0F, 1.0F, GameObject.Find("Main Camera").GetComponent<Camera>().nearClipPlane)).y - oneTile / 2);
                ItemsMap[i].transform.SetParent(GameObject.Find("meCanvas").GetComponent<Transform>());
                GameObject.Find("meCanvas").transform.GetChild(i+1).gameObject.tag = "editortool";
        }

        //GRID
            GameObject grid = new GameObject();
            grid.name = "grid";
            grid.transform.SetParent(GameObject.Find("map_editor").GetComponent<Transform>());
            for (int i = 0; i < 26; i++)
            {
                GameObject line = new GameObject();
                line.transform.SetParent(GameObject.Find("grid").GetComponent<Transform>());
                line.transform.position = new Vector3(0, i*oneTile, 0);
                line.AddComponent<LineRenderer>();
                LineRenderer lr = line.GetComponent<LineRenderer>();
                lr.materials[0] = blankmaterial;
                lr.SetWidth(0.01f, 0.01f);
                lr.SetPosition(0, new Vector3(0, i*oneTile, 0));
                lr.SetPosition(1, new Vector3(25, i*oneTile, 0));
            }
            for (int i = 0; i < 26; i++)
            {
                GameObject line = new GameObject();
                line.transform.SetParent(GameObject.Find("grid").GetComponent<Transform>());
                line.transform.position = new Vector3(i*oneTile, 0, 0);
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
	
	void Update() 
    {
        //LOAD F9
        if(Input.GetKeyDown(KeyCode.F9))
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
            XmlManager<List<GameItem>> xm = new XmlManager<List<GameItem>>();
            scene.Layer1 = xm.Load("Assets/saves/layer1.xml");
            for (int i = 0; i < scene.Layer1.Count; i++)
            {
                scene.Layer1[i].Load(ItemsList);
            }
            scene.Layer2 = xm.Load("Assets/saves/layer2.xml");
            for (int i = 0; i < scene.Layer2.Count; i++)
            {
                scene.Layer2[i].Load(ItemsList);
            }
            scene.Layer3 = xm.Load("Assets/saves/layer3.xml");
            for (int i = 0; i < scene.Layer3.Count; i++)
            {
                scene.Layer3[i].Load(ItemsList);
            }
        }
        //SAVE F5
        if (Input.GetKeyDown(KeyCode.F5))
        {
            XmlManager<List<GameItem>> xm = new XmlManager<List<GameItem>>();
            xm.Save("Assets/saves/layer1.xml", scene.Layer1);
            xm.Save("Assets/saves/layer2.xml",scene.Layer2);
            xm.Save("Assets/saves/layer3.xml", scene.Layer3);

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
            for (int i = 0; i < ItemsList.Length; i++)
            {
                ItemsMap[i].transform.position = new Vector3(ItemsMap[i].transform.position.x + Input.GetAxis("Mouse ScrollWheel") * 10, ItemsMap[i].transform.position.y);
            }
        }
        move();
	}

    //UPDATE MOVING
    void move()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //MOVE CAMERA
        Vector3 cam = GameObject.Find("Main Camera").transform.position;
        if (Input.GetKey(KeyCode.W))
            GameObject.Find("Main Camera").transform.position = new Vector3(cam.x, cam.y + 0.1f, -10);
        if (Input.GetKey(KeyCode.S))
            GameObject.Find("Main Camera").transform.position = new Vector3(cam.x, cam.y - 0.1f, -10);
        if (Input.GetKey(KeyCode.A))
            GameObject.Find("Main Camera").transform.position = new Vector3(cam.x - 0.1f, cam.y, -10);
        if (Input.GetKey(KeyCode.D))
            GameObject.Find("Main Camera").transform.position = new Vector3(cam.x + 0.1f, cam.y, -10);

        //PICK TILE
        if (hit.collider != null && hit.collider.tag == "editortool" && Input.GetMouseButton(0))
        {
                MOVING = hit.collider.gameObject;
        }
        else
            if (Input.GetMouseButton(0) && MOVING != null)
            {
                string nam = MOVING.name.Remove(MOVING.name.Length - 7) + ";" + new Vector2((worldPoint.x - worldPoint.x % oneTile + oneTile / 2) / oneTile + 0.5f, (worldPoint.y - worldPoint.y % oneTile + oneTile / 2) / oneTile + 0.5f);
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
        if (MOVING != null)
            GameObject.Find("moving").GetComponent<Text>().text = MOVING.name.Remove(MOVING.name.Length - 7) + new Vector2((worldPoint.x - worldPoint.x % oneTile + oneTile / 2) / oneTile + 0.5f, (worldPoint.y - worldPoint.y % oneTile + oneTile / 2) / oneTile + 0.5f);
        //REMOVE
        if(Input.GetMouseButton(1) && hit.collider != null && hit.collider.tag != "editortool")
        {

            scene.Layer1.Remove(scene.Layer1.Find(p => p.Name == hit.collider.name));
            scene.Layer2.Remove(scene.Layer2.Find(p => p.Name == hit.collider.name));
            scene.Layer3.Remove(scene.Layer3.Find(p => p.Name == hit.collider.name));
            Destroy(hit.collider.gameObject);
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
    }
}
