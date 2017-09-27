using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    class SaveControl : MonoBehaviour
    {
        void Update()
        {
            SavePersons();
            loadPersons();
        }
        public void SavePersons()
        {
            //if (!Directory.Exists("Assets/saves/" + path))
            //    Directory.CreateDirectory("Assets/saves/" + path);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                List<SavePerson> sp = new List<SavePerson>();
                foreach (person p in GameObject.Find("AIController").GetComponent<AIController>().persons)
                {
                    sp.Add(new SavePerson(p));
                }
                XmlManager<List<SavePerson>> xm = new XmlManager<List<SavePerson>>();
                    xm.Save("Assets/saves/SaveGame/persons.xml", sp);
                    SavePlayer();
            }

        }
        public void SavePlayer()
        {
            System.IO.File.WriteAllLines("Assets/saves/SaveGame/persons/playerInv.txt", GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.saveString());
        }

        public void LoadPlayer()
        {
            GameObject.Find("player").GetComponent<SimpleCharacter>().Inv = new Inventory();
            GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.loadString(System.IO.File.ReadAllLines("Assets/saves/SaveGame/persons/playerInv.txt"));
            GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.DrawInv();
        }

        public void SaveMap()
        {

        }
        public void loadPersons()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                List<SavePerson> lp = new List<SavePerson>();
                XmlManager<List<SavePerson>> xm = new XmlManager<List<SavePerson>>();
                lp = xm.Load("Assets/saves/SaveGame/persons.xml");
                GameObject.Find("AIController").GetComponent<AIController>().Load(lp);
                LoadPlayer();
            }
        }
    }
}
