using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript;

public class AIController : MonoBehaviour {
    public List<person> persons = new List<person>();
    public person AddPerson;
    public bool Addperson;
    public bool Updatelist;
	// Use this for initialization
	void Start () {
        Load();
	}
	
    void Load()
    {
        string[] q = System.IO.File.ReadAllLines(@"Assets/saves/persons_default.csv");
        for (int i = 1; i < q.Length; i++)
        {
            string s = q[i];
            Debug.Log(s.Split(';')[0]);
            Debug.Log(s.Split(';')[1]);
            Debug.Log(s.Split(';')[2]);
            Debug.Log(s.Split(';')[3]);
            Debug.Log(s.Split(';')[4]);
            Debug.Log(s.Split(';')[5]);
            Instantiate(Resources.Load(s.Split(';')[1])).name = s.Split(';')[0];
            persons.Add(GameObject.Find(s.Split(';')[0]).AddComponent<person>());
            //persons[i].name = s.Split(';')[0];
            //persons[i].resource = s.Split(';')[1];
            persons[i-1].Action_number = int.Parse(s.Split(';')[2]);
            persons[i-1].acsSource = s.Split(';')[3];
            
            persons[i-1].transform.position = new Vector3(float.Parse(s.Split(';')[4]), float.Parse(s.Split(';')[5]));
            persons[i - 1].UpdateAct = true;
            persons[i - 1].transform.SetParent(GameObject.Find("AIController").transform);
            persons[i - 1].gameObject.SetActive(true);
            persons[i - 1].GetComponent<person>().txt = GameObject.Find("text_bckgrnd").GetComponent<Text_intfc>();
        }
        //GameObject.Find("text_bckgrnd").GetComponent<Text_intfc>().off();
    }
	// Update is called once per frame
	void Update () {
		if(Addperson == true && AddPerson != null)
        {
            persons.Add(AddPerson);
            AddPerson = null;
            Addperson = false;
        }
        persons[0].UpdateAct = true;
        if(Updatelist)
        {
         //   Updatelist = false;
            persons[0].UpdateAct = false;
        }
	}
    public void SceneUpdate()
    {
        foreach(person p in persons)
        {
            if (Scene_Controller.SceneName != p.SceneName)
                p.gameObject.SetActive(false);
            else
                p.gameObject.SetActive(true);
        }
    }
}
