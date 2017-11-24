using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class person : MonoBehaviour
{
    public Inventory Inv;
    public string acsSource;
    public action_scripts acs;
    public int Action_number;
    public bool UpdateAct;
    public string SceneName;
    public string resource;
    public bool colliding;
    public static bool collidedLF;
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
    public Text_intfc txt;
    //pop up vars
    public static float popOpocity;
    public static float popscale;
    public GameObject PopUp, Pop;
    //sleep var
    public int timeleft = -1;
    //only for saving
    public Vector3 pos;
    public string name;
    //animals values
    public bool runaway;

    public int popupstate
    {
        get
        {
            float before = popOpocity;
            eu.Invoke(this);
            if (before == popOpocity && popOpocity > 0.9f)
                return 1;
            else
                if (before == popOpocity && popOpocity < 0.1f)
                    return 0;
                else
                    if (before < popOpocity)
                        return 1;
                    else
                        return 0;
        }
        set 
        {
            if (value == 1)
                eu += new EveryUpdate(popUpShow);  
        }
    }
    void Start()
    {
        Inv = new Inventory();
        //Inv.AddItem(0, 4);
        //Inv.AddItem(1, 2);
        acs = new action_scripts();
        acs.load(acsSource);
        speed = 0.25f;
        initialize = true;
        PopUp = this.transform.Find("pop-up").gameObject;
        Pop = this.transform.Find("pop").gameObject;
        PopUp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255*popupstate);
        name = this.gameObject.name;
        loadInv();
    }

    // Update is called once per frame
    void Update()
    {
        eu.Invoke(this);
        if (UpdateAct)
        {
            act();
        }
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
                case "popHIDE":
                    popscale = 1;
                    Debug.Log("added   " + eu.GetInvocationList().Length);
                    eu += new EveryUpdate(popHide);
                    Action_number++;
                    break;
                case "#changePOP":
                    Debug.Log("Assets/sprites/" + acs.parameters[Action_number][0] + ".png");
                    Pop.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(acs.parameters[Action_number][0]);
                    Action_number++;
                    break;
                case "popSHOW":
                    popscale= 0;
                    eu += new EveryUpdate(popShow);
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
                case "#randomSleep":
                    if (timeleft == -1)
                        timeleft = (int)Random.Range(float.Parse(acs.parameters[Action_number][0]), float.Parse(acs.parameters[Action_number][1]));
                    timeleft--;
                    if (timeleft == 0)
                    {
                        Action_number++;
                        timeleft = -1;
                    }
                    break;
                case "#randomStraight":
                    randomStraight(float.Parse(acs.parameters[Action_number][0]), float.Parse(acs.parameters[Action_number][1]));
                    break;
                case "#dialog":
                    {
                        if (colliding && Input.GetKeyDown(KeyCode.E))
                        {
                            Scene_Controller.Freze = true;
                            txt.Dialog(acs.parameters[Action_number][0], gameObject as GameObject);
                            eu += new EveryUpdate(popUpHide);
                            Action_number++;
                        }
                    }
                    break;
                case "#forceDialog":
                    {
                        txt.Dialog(acs.parameters[Action_number][0], gameObject as GameObject);
                        Action_number++;
                    }
                    break;
                case "#changeScene":
                    {
                        SceneName = acs.parameters[Action_number][0];
                        transform.position = new Vector2(float.Parse(acs.parameters[Action_number][1]), float.Parse(acs.parameters[Action_number][2]))*0.64f;
                        Action_number++;
                    }
                    break;
                case "end":
                    break;
                case "#GoTo":
                    Action_number = int.Parse(acs.parameters[Action_number][0]);
                    break;
                default:
                    break;
            }
        }
    }

    // multiple stops
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
            step = new Vector2(((ziel[zielNumber].x - this.transform.position.x)) / totalsteps * speed, ((ziel[zielNumber].y - this.transform.position.y)) / totalsteps * speed);
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
            step = new Vector2(((ziel[zielNumber].x - this.transform.position.x)) / totalsteps * speed, ((ziel[zielNumber].y - this.transform.position.y)) / totalsteps * speed);
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
    // only one stop
    public void walkStraight(Vector2 pos)
    {
        if (initialize)
        {
            float totalsteps = Mathf.CeilToInt(Mathf.Sqrt((this.transform.position.x - ziel[zielNumber].x) * (this.transform.position.x - ziel[zielNumber].x) + (this.transform.position.y - ziel[zielNumber].y) * (this.transform.position.y - ziel[zielNumber].y)) / speed);
            step = new Vector2(((ziel[zielNumber].x - this.transform.position.x)) / totalsteps * speed, ((ziel[zielNumber].y - this.transform.position.y)) / totalsteps * speed);
            initialize = false;
        }
        if (pos == (Vector2)this.transform.position)
        {
            step = new Vector2(0, 0);
            Action_number++;
        }
        this.transform.position = new Vector2(this.transform.position.x + step.x, this.transform.position.y + step.y);
    }

    // one random walk
    public void randomStraight(float min, float max)
    {
        if (initialize)
        {
            ziel = new Vector2[1];
            float distance = Random.Range(min, max);
            float rotation = Random.Range(0, 0.5f*Mathf.PI);
            ziel[0] = new Vector2((Mathf.Cos(rotation) * distance)-((Mathf.Cos(rotation) * distance)%0.64f), (Mathf.Sin(rotation) * distance)-((Mathf.Sin(rotation) * distance) % 0.64f));
            float totalsteps = Mathf.CeilToInt(Mathf.Sqrt((this.transform.position.x - ziel[0].x) * (this.transform.position.x - ziel[0].x) + (this.transform.position.y - ziel[0].y) * (this.transform.position.y - ziel[0].y)) / speed);
            step = new Vector2(((ziel[0].x - this.transform.position.x)) / totalsteps * speed, ((ziel[0].y - this.transform.position.y)) / totalsteps * speed);
            initialize = false;
        }
        if (ziel[0]== (Vector2)this.transform.position)
        {
            step = new Vector2(0, 0);
            Action_number++;
        }
        this.transform.position = new Vector2(this.transform.position.x + step.x, this.transform.position.y + step.y);
    }
    public static void popUpHide(person p)
    {
        //Debug.Log(popOpocity);
        popOpocity -= 0.1f;
        p.PopUp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, popOpocity);
        if (popOpocity < 0)
        {
            p.eu -= new EveryUpdate(popUpHide);
        }
    }

    public static void popUpShow(person p)
    {
        popOpocity += 0.1f;
        p.PopUp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, popOpocity);
        if (popOpocity > 1)
        {
            p.eu -= new EveryUpdate(popUpShow);
        }
    }

    private void Person_eu(person p)
    {
        throw new System.NotImplementedException();
    }

    public static void popHide(person p)
    {
        //Debug.Log(popOpocity);
        popscale -= 0.1f;
        p.Pop.transform.localScale = new Vector3(popscale,popscale);
        p.Pop.transform.position = new Vector3(popscale*0.1f, popscale*0.64f);
        if (popscale < 0)
        {
            p.eu -= new EveryUpdate(popHide);
        }
    }

    public static void popShow(person p)
    {
        popscale += 0.1f;
        p.Pop.GetComponent<Transform>().localScale = new Vector3(popscale, popscale);
        p.Pop.GetComponent<Transform>().position = new Vector3(popscale * 0.1f, popscale * 0.64f);
        if (popscale > 1)
        {
            p.eu -= new EveryUpdate(popShow);
        }
    }
    public static void none(person p)
    {

    }

    public void LoadComp(SavePerson s)
    {
        this.Action_number = s.Action_number;
        this.UpdateAct = s.UpdateAct;
        this.SceneName = s.SceneName;
        this.colliding = s.colliding;
        this.speed = s.speed;
        this.step = s.step;
        this.ziel = s.ziel;
        this.zielNumber = s.zielNumber;
        this.initialize = s.initialize;
        this.pos = s.pos;
        this.popupstate = s.popupstate;
        this.timeleft = s.timeleft;
        this.resource = s.resource;
        this.name = s.name;
        
    }

    public void loadInv()
    {
        Inv.loadString(System.IO.File.ReadAllLines("Assets/saves/SaveGame/persons/" + name + "Inv.txt"));
    }
}

public class SavePerson
{
    public int Action_number;
    public bool UpdateAct;
    public string SceneName;
    public string resource;
    public bool colliding;
    //straight vars
    public float speed;
    public Vector2 step;
    public int zielNumber;
    public Vector2[] ziel = new Vector2[0];
    public bool initialize;
    public Vector3 pos;
    public string name;
    public string acsSource;
    //Text_intfc
    
    //pop up vars
    public int popupstate;
    //sleep var
    public int timeleft = -1;
    public SavePerson()
    {

    }
    public SavePerson(person p)
    {
        if(p.acs.actions[Action_number] == "#dialog")
            Action_number = p.Action_number-1;
        else
            Action_number = p.Action_number;
        UpdateAct = p.UpdateAct;
        SceneName = p.SceneName;
        colliding = p.colliding;
        speed = p.speed;
        step = p.step;
        zielNumber = p.zielNumber;
        ziel = p.ziel;
        initialize = p.initialize;
        pos = p.transform.position;
        popupstate = p.popupstate;
        timeleft = p.timeleft;
        resource = p.resource;
        name = p.transform.name;
        acsSource = p.acsSource;
        System.IO.File.WriteAllLines("Assets/saves/SaveGame/persons/" + name + "Inv.txt", p.Inv.saveString());
    }

     public person LoadPerson()
    {
        person p = new person();
        p.Action_number = Action_number;
        p.UpdateAct = UpdateAct;
        p.SceneName = SceneName;
        p.colliding = colliding;
        p.speed = speed;
        p.step = step;
        p.ziel = ziel;
        p.zielNumber = zielNumber;
        p.initialize = initialize;
        p.pos = pos;
        p.popupstate = popupstate;
        p.timeleft = timeleft;
        p.resource = resource;
        p.name = name;
        p.acsSource = acsSource;
        p.Inv.loadString(System.IO.File.ReadAllLines("Assets/saves/SaveGame/persons/" + name + "Inv.txt"));
        return p;
    }

     public void LoadPerson(ref person p)
     {
         p.Action_number = Action_number;
         p.UpdateAct = UpdateAct;
         p.SceneName = SceneName;
         p.colliding = colliding;
         p.speed = speed;
         p.step = step;
         p.ziel = ziel;
         p.zielNumber = zielNumber;
         p.initialize = initialize;
         p.pos = pos;
         p.popupstate = popupstate;
         p.timeleft = timeleft;
         p.resource = resource;
         p.name = name;
        p.acsSource = acsSource;
        p.Inv.loadString(System.IO.File.ReadAllLines("Assets/saves/SaveGame/persons/" + name + "Inv.txt"));
     }
}
