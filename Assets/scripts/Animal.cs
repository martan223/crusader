using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

    public float speed;
    Fence fence;
    public bool infenc;
	// Use this for initialization
	void Start () {
        ifFence();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual string Save()
    {
        return "";
    }

    public void ifFence()
    {
        for (int i = 0; i < GameObject.Find("PathEditor").GetComponent<fence_controller>().fences.Count; i++)
        {
            for (int q = 0; q < GameObject.Find("PathEditor").GetComponent<fence_controller>().fences[i].InnerFields.Count; q++)
            {
                if (GameObject.Find("PathEditor").GetComponent<fence_controller>().fences[i].InnerFields[q] == (Vector2)this.transform.position)
                {
                    infenc = true;
                    fence = GameObject.Find("PathEditor").GetComponent<fence_controller>().fences[i];
                    this.GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
        }
        infenc = false;
    }
}
