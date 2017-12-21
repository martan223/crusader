using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvAmenu : MonoBehaviour {
    bool active;
    RaycastHit2D hit;
    public InvIt head, body, legs, weapon, activ;
    Inventory playersinv;
    int posininv;
    public GameObject Background;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        
        hit = Physics2D.Raycast(new Vector2((float)(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), (float)(Camera.main.ScreenToWorldPoint(Input.mousePosition).y)), Vector2.zero);
        //place chicken
        if (Input.GetMouseButtonDown(0))
        {
            placeIt();
        }
        if (hit.collider != null && hit.collider.tag == "InvBox" && Input.GetMouseButtonDown(1) && !active)
        {
            if (hit.collider.transform.childCount > 1)
            {
            Instantiate(Resources.Load("itemMenu/Casle"));
            GameObject.Find("Casle(Clone)").transform.SetParent(GameObject.Find("interface").transform);
            GameObject.Find("Casle(Clone)").transform.localScale = new Vector3(1, 1, 1);
            
            playersinv = GameObject.Find("player").GetComponent<SimpleCharacter>().Inv;
            GameObject.Find("Casle(Clone)").transform.position = hit.collider.transform.position;
            Debug.Log(playersinv.Items[(int.Parse(hit.collider.name[hit.collider.name.Length - 1].ToString()) - 1)].atributes.Length);
            for (int i = 0; i < playersinv.Items[(int.Parse(hit.collider.name[hit.collider.name.Length - 1].ToString()) - 1)].atributes.Length; i++)
            {
                posininv = (int.Parse(hit.collider.name[hit.collider.name.Length - 1].ToString()) - 1);
                //Debug.Log(Scene_Controller.ItemSheet.Items.Find(e => e.posininv == (int.Parse(hit.collider.name[hit.collider.name.Length - 1].ToString()) - 1)).atributes[i]);
                Instantiate(Resources.Load("itemMenu/" + playersinv.Items.Find(e => e.posininv == (int.Parse(hit.collider.name[hit.collider.name.Length - 1].ToString()))).atributes[i]));
                //Debug.Log(GameObject.Find(Scene_Controller.ItemSheet.Items.Find(e => e.posininv == (int.Parse(hit.collider.name[hit.collider.name.Length - 1].ToString()) - 1)).atributes[i] + "(Clone)").transform.position);
                GameObject.Find(playersinv.Items.Find(e => e.posininv == (int.Parse(hit.collider.name[hit.collider.name.Length - 1].ToString()))).atributes[i] + "(Clone)").transform.SetParent(GameObject.Find("Casle(Clone)").transform);
                GameObject.Find(playersinv.Items.Find(e => e.posininv == (int.Parse(hit.collider.name[hit.collider.name.Length - 1].ToString()))).atributes[i] + "(Clone)").transform.localPosition = new Vector3(0, 0 - i * 50, 0);
                GameObject.Find(playersinv.Items.Find(e => e.posininv == (int.Parse(hit.collider.name[hit.collider.name.Length - 1].ToString()))).atributes[i] + "(Clone)").transform.localScale = new Vector3(1, 1, 1);
            }
            active = true;
            }
        }
        if (hit.collider != null && hit.collider.tag == "itemMenu" && active && Input.GetMouseButtonDown(0))
        {
            Debug.Log(hit.collider.tag);
            switch(hit.collider.name.Split('(')[0])
            {
                case "delete":
                    GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.RemoveItemAt(posininv);
                    GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.DrawInv();
                    Destroy(GameObject.Find("Casle(Clone)"));
                    active = false;
                    break;
                case "SAWeapon":
                    Inventory player;
                    InvIt i;
                    int o;
                    string[] s;
                    if(!weapon.Equals(new InvIt())) 
                    {
                        player = GameObject.Find("player").GetComponent<SimpleCharacter>().Inv;
                        i = weapon;
                        o = player.Items.IndexOf(i);
                        s = i.atributes.Clone() as string[];
                        s[Array.FindIndex<string>(s, p => p == "RMVWeapon")] = "SAWeapon";
                        InvIt atr = player.Items[o];
                        atr.atributes = s;
                        player.Items[o] = atr;
                        //player.Items[posininv] = i;
                        //player.Items.RemoveAt(o);
                        //player.Items.Insert(o, i);
                        DestroyImmediate(GameObject.Find("weapon(Clone)"));
                    }
                    
                    active = false;
                    Destroy(GameObject.Find("Casle(Clone)"));
                    player = GameObject.Find("player").GetComponent<SimpleCharacter>().Inv;
                    player.DrawInv();
                    i = player.Items[posininv];
                    o = player.Items.IndexOf(i);
                    s = i.atributes.Clone() as string[];
                    s[Array.FindIndex<string>(s, p => p == "SAWeapon")] = "RMVWeapon";
                    i.atributes = s;
                    player.Items[posininv] = i;
                    player.Items.RemoveAt(o);
                    player.Items.Insert(o,i);
                    player.DrawInv();
                    weapon = GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.Items[posininv];
                    for (int q = 0; q < player.Items.Count; q++)
                    {
                        for (int e = 0; e < player.Items[q].atributes.Length; e++)
                        {
                            Debug.Log(player.Items[q].atributes[e]);
                        }
                    }
                    GameObject.Find("player").GetComponent<SimpleCharacter>().Inv = player;
                    break;
                case "RMVWeapon":
                    player = GameObject.Find("player").GetComponent<SimpleCharacter>().Inv;
                    i = player.Items[posininv];
                    o = player.Items.IndexOf(i);
                    s = i.atributes.Clone() as string[];
                    s[Array.FindIndex<string>(s, p => p == "RMVWeapon")] = "SAWeapon";
                    i.atributes = s;
                    player.Items[posininv] = i;
                    DestroyImmediate(GameObject.Find("weapon(Clone)"));
                    weapon = GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.Items[posininv];
                    active = false;
                    GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.DrawInv();
                    Destroy(GameObject.Find("Casle(Clone)"));
                    weapon = new InvIt();
                    break;
                case "active":
                    Background.SetActive(true);
                    Background.transform.position = GameObject.Find("box" + (posininv + 1).ToString()).transform.position;
                    player = GameObject.Find("player").GetComponent<SimpleCharacter>().Inv;
                    i = player.Items[posininv];
                    activ = i;
                    o = player.Items.IndexOf(i);
                    s = i.atributes.Clone() as string[];
                    s[Array.FindIndex<string>(s, p => p == "active")] = "deactive";
                    i.atributes = s;
                    player.Items[posininv] = i;
                    player.Items.RemoveAt(o);
                    player.Items.Insert(o, i);
                    player.DrawInv();
                    active = false;
                    Destroy(GameObject.Find("Casle(Clone)"));
                    break;
                case "deactive":
                    activ = new InvIt();
                    Background.SetActive(false);
                    player = GameObject.Find("player").GetComponent<SimpleCharacter>().Inv;
                    i = player.Items[posininv];
                    o = player.Items.IndexOf(i);
                    s = i.atributes.Clone() as string[];
                    s[Array.FindIndex<string>(s, p => p == "deactive")] = "active";
                    i.atributes = s;
                    player.Items[posininv] = i;
                    player.Items.RemoveAt(o);
                    player.Items.Insert(o, i);
                    player.DrawInv();
                    active = false;
                    Destroy(GameObject.Find("Casle(Clone)"));
                    break;
            }
        }
	}
    void placeIt()
    {
        Instantiate(Resources.Load("chicken"));
        Vector2 pos  = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x % 0.64f + 0.64f / 2, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y % 0.64f + 0.64f / 2);
        GameObject.Find("chicken(Clone)").transform.position = pos;
        GameObject.Find("chicken(Clone)").name = "chicken";

    }
}
