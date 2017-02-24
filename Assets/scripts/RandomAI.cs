using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAI : MonoBehaviour {
    System.Random random;
    bool slp, moving;
    public float sleep;
    public float speed;
    public Vector2 pos;
    public Vector2 dest;
	// Use this for initialization
	void Start () {
        random = new System.Random();
        speed = 0.1f;
        moving = slp = false;
        dest = new Vector2(random.Next(0, 10), random.Next(0, 10));
        sleep = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(sleep > 0)
        {
            sleep -= 1f;
        }
        else
        {
            if(moving)
            {
                if(pos.x < dest.x)
                    pos = new Vector2(pos.x + speed, pos.y);
                else if(pos.x>dest.x)
                    pos = new Vector2(pos.x - speed, pos.y);
                else pos = new Vector2(pos.x, pos.y);
                if (pos.y < dest.y)
                    pos = new Vector2(pos.x, pos.y + speed);
                else if (pos.y > dest.y)
                    pos = new Vector2(pos.x, pos.y - speed);
                else pos = new Vector2(pos.x, pos.y);
                if(pos == dest)
                {
                    moving = false;
                }
            }
            else
            {
                dest = new Vector2(random.Next(0, 10), random.Next(0, 10));
                sleep = random.Next(0, 0);
                moving = slp = true;
            }
        }
        this.transform.position = pos;
	}
}
