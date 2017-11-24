using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript;
using System.IO;

public class AIController : MonoBehaviour
{
    public List<person> SamplePersons = new List<person>();
    public List<person> persons = new List<person>();
    public person AddPerson;
    public bool Addperson;
    public Text_intfc Textbckgrnd;
    // Use this for initialization
    void Start()
    {
        List<SavePerson> lp = new List<SavePerson>();
        XmlManager<List<SavePerson>> xm = new XmlManager<List<SavePerson>>();
        Textbckgrnd = GameObject.Find("text_bckgrnd").GetComponent<Text_intfc>();
        LoadSamplePersons();
        //if (File.Exists("Assets/saves/SaveGame/persons.xml"))
        //{
        //    lp = xm.Load("Assets/saves/SaveGame/persons.xml");
        //    Load(lp);
        //}
        //else
        //{
        //    Load();
        //}
         
    }

    void Load()
    {
        string[] q = System.IO.File.ReadAllLines(@"Assets/saves/persons_default.csv");
        for (int i = 1; i < q.Length; i++)
        {
            string s = q[i];
            //Debug.Log(s.Split(';')[0]);

            //Debug.Log(s.Split(';')[2]);
            //Debug.Log(s.Split(';')[3]);
            //Debug.Log(s.Split(';')[4]);
            //Debug.Log(s.Split(';')[5]);
            Instantiate(Resources.Load(s.Split(';')[1])).name = s.Split(';')[0];
            persons.Add(GameObject.Find(s.Split(';')[0]).AddComponent<person>());
            //persons[i].name = s.Split(';')[0];
            //persons[i].resource = s.Split(';')[1];
            persons[i - 1].Action_number = int.Parse(s.Split(';')[2]);
            persons[i - 1].acsSource = s.Split(';')[3];
            persons[i - 1].resource = s.Split(';')[1];
            persons[i - 1].transform.position = new Vector3(float.Parse(s.Split(';')[4]), float.Parse(s.Split(';')[5]));
            persons[i - 1].UpdateAct = true;
            persons[i - 1].transform.SetParent(GameObject.Find("AIController").transform);
            persons[i - 1].gameObject.SetActive(true);
            persons[i - 1].GetComponent<person>().txt = Textbckgrnd;
            persons[i - 1].SceneName = s.Split(';')[6];
        }
        //GameObject.Find("text_bckgrnd").GetComponent<Text_intfc>().off();
    }
    public void Load(List<SavePerson> lst)
    {
        clear();
        foreach (SavePerson p in lst)
        {
            Instantiate(Resources.Load(p.resource)).name = p.name;
            persons.Add(GameObject.Find(p.name).AddComponent<person>());
            persons[persons.Count - 1].LoadComp(p);
            persons[persons.Count - 1].acsSource = p.acsSource;
            persons[persons.Count - 1].SceneName = p.SceneName;
            persons[persons.Count - 1].transform.position = p.pos;
            persons[persons.Count - 1].transform.SetParent(GameObject.Find("AIController").transform);
            persons[persons.Count - 1].gameObject.SetActive(true);
            persons[persons.Count - 1].GetComponent<person>().txt = Textbckgrnd;
            persons[persons.Count - 1].popupstate = p.popupstate;

            //persons[persons.Count - 1].Inv = new Inventory();
            //persons[persons.Count - 1].Inv.loadString(System.IO.File.ReadAllLines("Assets/saves/SaveGame/persons/" + persons[persons.Count - 1].name + "Inv.txt"));
        }
    }
    void LoadSamplePersons()
    {
        string[] q = System.IO.File.ReadAllLines(@"Assets/saves/persons_default.csv");
        for (int i = 1; i < q.Length; i++)
        {
            string s = q[i];
            //Debug.Log(s.Split(';')[0]);

            //Debug.Log(s.Split(';')[2]);
            //Debug.Log(s.Split(';')[3]);
            //Debug.Log(s.Split(';')[4]);
            //Debug.Log(s.Split(';')[5]);
            Instantiate(Resources.Load(s.Split(';')[1])).name = s.Split(';')[0];
            persons.Add(GameObject.Find(s.Split(';')[0]).AddComponent<person>());
            //persons[i].name = s.Split(';')[0];
            //persons[i].resource = s.Split(';')[1];
            //persons[i - 1].Action_number = int.Parse(s.Split(';')[2]);
            persons[i - 1].acsSource = s.Split(';')[3];
            persons[i - 1].resource = s.Split(';')[1];
            //persons[i - 1].transform.position = new Vector3(float.Parse(s.Split(';')[4]), float.Parse(s.Split(';')[5]));
            persons[i - 1].UpdateAct = true;
            persons[i - 1].transform.SetParent(GameObject.Find("AIController").transform);
            persons[i - 1].gameObject.SetActive(true);
            persons[i - 1].GetComponent<person>().txt = Textbckgrnd;
            persons[i - 1].SceneName = "cb_home";
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if(Addperson == true && AddPerson != null)
        //{
        //    persons.Add(AddPerson);
        //    AddPerson = null;
        //    Addperson = false;
        //}
        //persons[0].UpdateAct = true;
        //if(clear)
        //{
        // //   Updatelist = false;
        //    persons.Clear();
        //    clear = false;
        //}
    }

    public void clear()
    {
        foreach (person p in persons)
        {
            DestroyImmediate(p.gameObject);
        }
        persons = new List<person>();
    }

    public void SceneUpdate()
    {
        foreach (person p in persons)
        {
            if (Scene_Controller.SceneName != p.SceneName)
                p.gameObject.SetActive(false);
            else
                p.gameObject.SetActive(true);
        }
    }
}
