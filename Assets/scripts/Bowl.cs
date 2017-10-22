using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour {
    
    Collider2D col;
	// Use this for initialization
	void Start () {
        col = this.GetComponent<Collider2D>();
	}
    // Update is called once per frame
    void Update() {
        
        if (col.IsTouching(GameObject.Find("player").GetComponent<Collider2D>()) && Input.GetKeyDown(KeyCode.E)) 
        {
            string nam = this.name.Replace("bowl", "bowlFood");
            GameObject.Instantiate(Resources.Load(Array.Find(Scene_Controller.ItemsList, (p => p.texture == "bowlFood")).texture), this.transform.position, Quaternion.identity).name = nam;
            GameObject.Find(nam).GetComponent<Collider2D>().isTrigger = true;
            Scene_Controller.scene.Layer2.Remove(Scene_Controller.scene.Layer2.Find(p => p.position == this.transform.position));
            Destroy(this.gameObject);
            Scene_Controller.scene.Layer2.Add(new GameItem(GameObject.Find(nam)));
        }
    }
}
