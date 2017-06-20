using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public List<InvIt> Items = new List<InvIt>();
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadItems()
    {
        string[] q = System.IO.File.ReadAllLines(@"Assets/saves/ItemsSheet.csv");
        for (int i = 1; i < q.Length; i++)
        {
            string s = q[i];
            Items.Add(new InvIt());

            Items[i - 1].ID = int.Parse(s.Split(';')[0]);
            Items[i - 1].Resource = Resources.Load(s.Split(';')[1]) as GameObject;

            Items[i - 1].weight = float.Parse(s.Split(';')[2]);
            Items[i - 1].Stackable = bool.Parse(s.Split(';')[3]);
        }
    }
    public void AddItem(int ID, int amount = 1)
    {
        if (Items.Exists(e => e.ID == ID) && Items.Find(e => e.ID == ID).Stackable)
            Items.Find(e => e.ID == ID).amount++;
        else
        {
            Items.Add(Scene_Controller.ItemSheet.Items[ID]);
            Items[Items.Count - 1].amount = amount;
        }
    }
    public void DrawInv()
    {
        for (int i = 0; i < 5; i++)
        {
            if (GameObject.Find("box" + (i + 1).ToString()).transform.childCount > 1)
                DestroyImmediate(GameObject.Find("box" + (i + 1).ToString()).transform.GetChild(1).gameObject);
        }
        int pos = 0;
        for (int i = 0; i < Items.Count; i++)
        {
            if (pos < 5)
            {
                Instantiate(Scene_Controller.ItemSheet.Items.Find(e => e.ID == Items[i].ID).Resource).transform.SetParent(GameObject.Find("box" + (pos + 1).ToString()).transform);
                GameObject.Find("box" + (pos + 1).ToString()).transform.GetChild(1).localPosition = new Vector3(0, 0, -1);
                GameObject.Find("box" + (pos + 1).ToString()).transform.GetChild(1).GetComponent<SpriteRenderer>().sortingLayerName = "Inv";
                GameObject.Find("box" + (pos + 1).ToString()).transform.GetChild(0).GetComponent<Text>().enabled = true;
                GameObject.Find("box" + (pos + 1).ToString()).transform.GetChild(0).GetComponent<Text>().text = Items[i].amount.ToString();
                Debug.Log(GameObject.Find("box" + (pos + 1).ToString()).transform.GetChild(1).localPosition);
            }
            pos++;
        }
    }
    public void TransferItems(Inventory old, int i)
    {
        if (old.Items.Find(e => e.ID == i).amount > 1)
            old.Items.Find(e => e.ID == i).amount--;
        else
            old.RemoveItem(i);
        this.AddItem(i);
    }
    public void RemoveItem(int i)
    {
        if (Items.Contains(Scene_Controller.ItemSheet.Items[i]))
            this.Items.Remove(Scene_Controller.ItemSheet.Items[i]);
    }
    public void DrawAllInv()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Debug.Log(Items[i].ID + " : " + Items[i].amount);
        }
    }
}
