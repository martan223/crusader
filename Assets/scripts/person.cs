using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class person : MonoBehaviour
{
    public string acsSource;
    public action_scripts acs;
    public int Action_number;
    public bool UpdateAct;
    public string SceneName;
    public string resource;
    //straight vars
    public float speed;
    public Vector2 step;
    public int zielNumber;
    public Vector2[] ziel = new Vector2[0];
    public bool initialize;
    //delegate
    public delegate void EveryUpdate(person p);
    public event EveryUpdate eu = new EveryUpdate(none);
    // Use this for initialization

    //Text_intfc
    Text_intfc txt;
    //pop up vars
    public static float popOpocity;
    public GameObject PopUp;
    //sleep var
    public int timeleft = -1;
    void Start()
    {
        txt = GameObject.Find("text_bckgrnd").GetComponent<Text_intfc>();
        txt.off();
        acs = new action_scripts();
        acs.load(acsSource);
        speed = 0.32f;
        initialize = true;
        PopUp = this.transform.FindChild("pop-up").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        eu.Invoke(this);
        if (UpdateAct)
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
        if (Action_number < acs.actions.Length && !Scene_Controller.pause)
        {
            switch (acs.actions[Action_number])
            {
                case "#straight":
                    walkStraight(acs.parameters[Action_number][0]);
                    break;
                case "pop-up":
                    {
                        this.GetComponent<AudioSource>().Play();
                        Action_number++;
                    }
                    break;
                case "again":
                    Action_number = 0;
                    initialize = true;
                    break;
                case "popupHIDE":
                    popOpocity = 1;
                    Debug.Log("added   " + eu.GetInvocationList().Length);
                    eu += new EveryUpdate(popUpHide);
                    Action_number++;
                    break;
                case "popupSHOW":
                    popOpocity = 0;
                    eu += new EveryUpdate(popUpShow);
                    Action_number++;
                    break;
                case "#sleep":
                    if (timeleft == -1)
                        timeleft = int.Parse(acs.parameters[Action_number][0]);
                    timeleft--;
                    if (timeleft == 0)
                    {
                        Action_number++;
                        timeleft = -1;
                    }
                    break;
                case "#dialog":
                    {
                        txt.Dialog(acs.parameters[Action_number][0]);
                        Action_number++;
                    }
                    break;
                case "end":
                    break;
            }
        }
    }


    public void walkStraight(string path)
    {
        if (initialize)
        {
            string[] files = File.ReadAllLines("Assets/saves/action_scripts/" + path + ".csv");
            ziel = new Vector2[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                string[] s = files[i].Split(';');
                Vector2 v = new UnityEngine.Vector2(float.Parse(s[0]), float.Parse(s[1]));
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
        if (ziel[zielNumber] == (Vector2)this.transform.position && zielNumber + 1 < ziel.Length)
        {
            this.transform.position = ziel[zielNumber];
            zielNumber++;
            float totalsteps = Mathf.CeilToInt(Mathf.Sqrt((this.transform.position.x - ziel[zielNumber].x) * (this.transform.position.x - ziel[zielNumber].x) + (this.transform.position.y - ziel[zielNumber].y) * (this.transform.position.y - ziel[zielNumber].y)) / speed);
            step = new Vector2(((ziel[zielNumber].x - this.transform.position.x)) / totalsteps, ((ziel[zielNumber].y - this.transform.position.y)) / totalsteps);
        }
        if (ziel[zielNumber] == (Vector2)this.transform.position && zielNumber + 1 == ziel.Length)
        {
            step = new Vector2(0, 0);
            Action_number++;
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

    public static void popUpHide(person p)
    {
        //Debug.Log(popOpocity);
        popOpocity -= 0.1f;
        Debug.Log(popOpocity);
        p.PopUp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, popOpocity);
        if (popOpocity < 0)
        {
            p.eu -= new EveryUpdate(popUpHide);
        }
    }

    public static void popUpShow(person p)
    {
        popOpocity += 0.1f;
        Debug.Log(popOpocity);
        p.PopUp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, popOpocity);
        if (popOpocity > 1)
        {
            p.eu -= new EveryUpdate(popUpShow);
        }
    }
    public static void none(person p)
    {

    }
}
