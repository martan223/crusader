using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chicken : Animal{

    bool triggered;
    public Vector2 oneStep;
    int ActionNumber;
    public Vector2 step;
    private bool initialize;
    public Vector2 ziel;
    public int timeleft;
    int minSleep, maxSleep;
    float randMin, RandMax;
    float totalsteps;

    // Use this for initialization
    void Start () {
        minSleep = 60;
        maxSleep = 300;
        randMin = 2.56f;
        RandMax = 12.8f;
        ActionNumber = 1;
        initialize = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(triggered)
        {
            oneStep = new Vector2((GameObject.Find("player").transform.position.x-transform.position.x)*speed/10, (GameObject.Find("player").transform.position.y - transform.position.y)*speed/10);
            transform.position -= new Vector3(oneStep.x,oneStep.y);
            if (!gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>().IsTouching(GameObject.Find("player").GetComponent<BoxCollider2D>()))
                {
                triggered = false;
            }
        }
        else
            switch(ActionNumber)
            {
                case 1:
                    randomStraight(randMin, RandMax);
                    break;
                case 2:
                    RandomSleep();
                    break;
            }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "player")
            triggered = true;
    }

    public override string Save()
    {
        return "";
    }
    public void randomStraight(float min, float max)
    {
        if (initialize)
        { 
            float distance = Random.Range(min, max);
            float rotation = Random.Range(0, 0.5f * Mathf.PI);
            ziel = new Vector2((Mathf.Cos(rotation) * distance) - ((Mathf.Cos(rotation) * distance) % 0.64f), (Mathf.Sin(rotation) * distance) - ((Mathf.Sin(rotation) * distance) % 0.64f));
            totalsteps = Mathf.CeilToInt(Mathf.Sqrt((this.transform.position.x - ziel.x) * (this.transform.position.x - ziel.x) + (this.transform.position.y - ziel.y) * (this.transform.position.y - ziel.y)) / speed);
            step = new Vector2(((ziel.x - this.transform.position.x)) / totalsteps * speed, ((ziel.y - this.transform.position.y)) / totalsteps * speed);
            initialize = false;
        }
        if (totalsteps==0)
        {
            step = new Vector2(0, 0);
            ActionNumber++;
            initialize = true;
        }
        totalsteps--;
        this.transform.position = new Vector2(this.transform.position.x + step.x, this.transform.position.y + step.y);
    }
        void RandomSleep()
    {
        if (timeleft == -1)
            timeleft = (int)Random.Range(minSleep, maxSleep);
        timeleft--;
        if (timeleft == 0)
        {
            ActionNumber = 1;
            timeleft = -1;
        }
    }
}
