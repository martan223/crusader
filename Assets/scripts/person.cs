using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class person : MonoBehaviour {
    public action_scripts acs;
    public int action_number;

    //straight vars
    public float speed;
    public Vector2 step;
    public int zielNumber;
    Vector2[] ziel = new Vector2[0];
    public bool initialize;
	// Use this for initialization
	void Start () {
        acs = new action_scripts();
        acs.load("Assets/saves/action_scripts/path.csv");
        speed = 0.32f;
        initialize = true;
	}
	
	// Update is called once per frame
	void Update () {
        act();
	}


    public void MoveOnPath(string path)
    {
        FileManager fm = new FileManager();
        Vector2[] v = fm.LoadPositions(path);
    }

    //switch between actions
    public void act()
    {
        if(action_number < acs.actions.Length)
        {
        switch(acs.actions[action_number])
        {
            case "#straight":
                walkStraight(acs.parameters[action_number][0]);
                    break;
            case "tick":
                    {
                        this.GetComponent<AudioSource>().Play();
                        action_number++;
                    }
                    break;
            case "again":
                    action_number = 0;
                    initialize = true;
                    break;
            }
        }
    }


    public void walkStraight(string path)
    {
        if(initialize)
        {
            string[] files = File.ReadAllLines("Assets/saves/action_scripts/"+path+".csv");
            ziel = new Vector2[files.Length];
            for (int i = 0; i < files.Length; i++)
			{
                string[] s = files[i].Split(';');
                Vector2 v = new UnityEngine.Vector2(float.Parse(s[0]),float.Parse(s[1]));
                ziel[i] = v;
                //Debug.Log("#"+v.x+v.y);
            }
            
            zielNumber = 0;
            float totalsteps = Mathf.CeilToInt(Mathf.Sqrt((this.transform.position.x - ziel[zielNumber].x) * (this.transform.position.x - ziel[zielNumber].x) + (this.transform.position.y - ziel[zielNumber].y) * (this.transform.position.y - ziel[zielNumber].y)) / speed);
            step = new Vector2(((this.transform.position.x + ziel[zielNumber].x) / speed) / totalsteps * speed, ((this.transform.position.y + ziel[zielNumber].y) / speed) / totalsteps * speed);
            //Debug.Log(totalsteps);
            //Debug.Log(Mathf.CeilToInt((this.transform.position.x + ziel[zielNumber].x) / speed));
            //Debug.Log(new Vector2(Mathf.CeilToInt((this.transform.position.x + ziel[zielNumber].x) / speed) / totalsteps * speed, Mathf.CeilToInt((this.transform.position.y + ziel[zielNumber].y) / speed) / totalsteps * speed));
            initialize = false;
        }
        if(ziel[zielNumber] == (Vector2)this.transform.position && zielNumber+1 < ziel.Length)
        {
            this.transform.position = ziel[zielNumber];
            zielNumber++;
            float totalsteps = Mathf.CeilToInt(Mathf.Sqrt((this.transform.position.x - ziel[zielNumber].x) * (this.transform.position.x - ziel[zielNumber].x) + (this.transform.position.y - ziel[zielNumber].y) * (this.transform.position.y - ziel[zielNumber].y)) / speed);
            step = new Vector2(((ziel[zielNumber].x-this.transform.position.x)) / totalsteps, ((ziel[zielNumber].y-this.transform.position.y)) / totalsteps);
        }
        if (ziel[zielNumber] == (Vector2)this.transform.position && zielNumber+1 == ziel.Length)
        {
            step = new Vector2(0,0);
            action_number++;
        }
        //if (this.transform.position.x < ziel[zielNumber].x)
            this.transform.position = new Vector2(this.transform.position.x + step.x, this.transform.position.y + step.y);
        //else if (this.transform.position.x > ziel[zielNumber].x)
        //    this.transform.position = new Vector2(this.transform.position.x - step.x, this.transform.position.y);
        //else this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
        //if (this.transform.position.y < ziel[zielNumber].y)
        //    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + step.y);
        //else if (this.transform.position.y > ziel[zielNumber].y)
        //    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - step.y);
        //else this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
    }
}
