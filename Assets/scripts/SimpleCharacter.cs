﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacter : MonoBehaviour {
    public Inventory Inv;
	// Use this for initialization
	void Start () {
        Inv = new Inventory();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.W))
            transform.position = new Vector3(transform.position.x,transform.position.y + 0.1f);
        if (Input.GetKey(KeyCode.S))
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f);
        if (Input.GetKey(KeyCode.A))
            transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y);
        if (Input.GetKey(KeyCode.D))
            transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y);

	}
}
