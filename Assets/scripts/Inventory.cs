﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public List<InvIt> Items = new List<InvIt>();
    public bool[] ocuped = new bool[4];
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Items == null)
            Items = new List<InvIt>();
        
	}

    public void LoadItems()
    {
        string[] q = System.IO.File.ReadAllLines(@"Assets/saves/ItemsSheet.csv");
        for (int i = 1; i < q.Length; i++)
        {
            string s = q[i];
            InvIt a = new InvIt();
            a.ID = int.Parse(s.Split(';')[0]);
            a.Resource = Resources.Load(s.Split(';')[1]) as GameObject;

            a.weight = float.Parse(s.Split(';')[2]);
            a.Stackable = bool.Parse(s.Split(';')[3]);
            a.atributes = new string[s.Split(';')[4].Split(',').Length];
            a.atributes = s.Split(';')[4].Split(',');
            Items.Add(a);
        }
    }
    public void AddItem(int ID, int amount = 1)
    {
        if (Items.Exists(e => e.ID == ID) && Items.Find(e => e.ID == ID).Stackable)
        {
            InvIt a = Items.Find(e => e.ID == ID);
            int o = Items.IndexOf(a);
            a.amount++;
            Items.Remove(Items.Find(e => e.ID == ID));
            Items.Insert(o,a);
        }
        else
        {
            InvIt a = Scene_Controller.ItemSheet.Items[ID];
            a.amount = amount;
            Items.Add(a);
        }
    }
    public void DrawInv()
    {
        for (int i = 0; i < 5; i++)
        {
            if (GameObject.Find("box" + (i + 1).ToString()).transform.childCount > 2)
                DestroyImmediate(GameObject.Find("box" + (i + 1).ToString()).transform.GetChild(2).gameObject);
            if (GameObject.Find("box" + (i + 1).ToString()).transform.childCount > 1)
                DestroyImmediate(GameObject.Find("box" + (i + 1).ToString()).transform.GetChild(1).gameObject);
            GameObject.Find("box" + (i + 1).ToString()).transform.GetChild(0).GetComponent<Text>().text = "";
        }
        int pos = 0;

        for (int i = 1; i < Items.Count+1; i++)
        {
            InvIt a = Items[i-1];
            int o = Items.IndexOf(a);
            a.posininv = i;
            Items.RemoveAt(o);
            Items.Insert(o,a);
        }

        for (int i = 0; i < Items.Count; i++)
        {
            if (pos < 5)
            {
                Instantiate(Scene_Controller.ItemSheet.Items.Find(e => e.ID == Items[i].ID).Resource).transform.SetParent(GameObject.Find("box" + (Items[i].posininv).ToString()).transform);
                GameObject.Find("box" + (Items[i].posininv).ToString()).transform.GetChild(1).localPosition = new Vector3(0, 0, -1);
                GameObject.Find("box" + (Items[i].posininv).ToString()).transform.GetChild(1).GetComponent<SpriteRenderer>().sortingLayerName = "Inv";
                GameObject.Find("box" + (Items[i].posininv).ToString()).transform.GetChild(0).GetComponent<Text>().enabled = true;
                GameObject.Find("box" + (Items[i].posininv).ToString()).transform.GetChild(0).GetComponent<Text>().text = Items[i].amount.ToString();
                Debug.Log(GameObject.Find("box" + (Items[i].posininv).ToString()).transform.GetChild(1).localPosition);
            }
            if (Array.Exists<string>(Items[i].atributes, s => s == "RMVWeapon"))
            {
                Instantiate(Resources.Load("weapon"));
                GameObject.Find("weapon(Clone)").transform.SetParent(GameObject.Find("box" + (Items[i].posininv)).transform);
                GameObject.Find("weapon(Clone)").transform.GetComponent<SpriteRenderer>().sortingLayerName = "Inv";
                GameObject.Find("weapon(Clone)").transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
                GameObject.Find("weapon(Clone)").transform.localPosition = new Vector3();
            }
            if (Array.Exists<string>(Items[i].atributes, s => s == "deactive"))
            {
                GameObject.Find("Inventory").GetComponent<InvAmenu>().Background.transform.position = GameObject.Find("box" + (Items[i].posininv)).transform.position;
            }
            pos++;
        }
    }
    public void TransferItems(Inventory old, int i)
    {
        if (old.Items.Find(e => e.ID == i).amount > 1)
        {
            InvIt a = old.Items.Find(e => e.ID == i);
            int o = old.Items.IndexOf(a);
            old.Items.Remove(a);
            a.amount--;
            old.Items.Insert(o, a);
        }
        else
            old.RemoveItem(i);
        //old.DrawAllInv();
        this.AddItem(i);
        old.DrawAllInv();
    }
    public void RemoveItem(int i)
    {
        if (Items.Contains(Items.Find(e => e.ID == i)))
        {
            if (Items.IndexOf(Items.Find(e => e.ID == i)) < GameObject.Find("Inventory").GetComponent<InvAmenu>().weapon.posininv)
                GameObject.Find("Inventory").GetComponent<InvAmenu>().weapon.posininv--;
            if (Items.IndexOf(Items.Find(e => e.ID == i)) == GameObject.Find("Inventory").GetComponent<InvAmenu>().weapon.posininv)
            {
                GameObject.Find("Inventory").GetComponent<InvAmenu>().weapon = new InvIt();
                DestroyImmediate(GameObject.Find("weapon(Clone)"));
            }
            ocuped[Items.Find(e => e.ID == i).posininv] = false;
            Items.Remove(Items.Find(e => e.ID == i));
        }
    }

    public void RemoveItemAt(int i)
    {
        if (Items.Contains(Items.Find(e => e.posininv-1 == i)))
        {
            if (i < GameObject.Find("Inventory").GetComponent<InvAmenu>().weapon.posininv)
                GameObject.Find("Inventory").GetComponent<InvAmenu>().weapon.posininv--;
            if (GameObject.Find("Inventory").GetComponent<InvAmenu>().weapon.Resource != null && i == GameObject.Find("Inventory").GetComponent<InvAmenu>().weapon.posininv)
            {
                GameObject.Find("Inventory").GetComponent<InvAmenu>().weapon = new InvIt();
                DestroyImmediate(GameObject.Find("weapon(Clone)"));
            }
            ocuped[i] = false;
            Items.RemoveAt(i);
        }
    }
    public void DrawAllInv()
    {
        for (int i = 0; i < Items.Count; i++)
        {
                Debug.Log(Items[i].ID + " : " + Items[i].amount);
        }
    }
    public string[] saveString()
    {
        string[] s = new String[Items.Count];
        for (int i = 0; i < Items.Count; i++)       
        {
            s[i] = Items[i].ID + ";" + Items[i].amount + ";" + Items[i].posininv;
        }
        return s;
    }

    public void loadString(string[] s)
    {
        for (int i = 0; i < s.Length; i++)
        {
            AddItem(int.Parse(s[i].Split(';')[0]), int.Parse(s[i].Split(';')[1]));
            
        }
    }
}
