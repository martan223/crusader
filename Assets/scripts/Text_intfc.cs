using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_intfc : MonoBehaviour {

	// Use this for initialization
    GameObject parent;
    Dialog_controller DC;
    bool haveDialog;
	void Start () {
        DC = new Dialog_controller();
        haveDialog = false;
        this.off();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(haveDialog && Input.GetKeyDown(KeyCode.Space))
        {
            DC.phrase++;
            if (DC.phrase == DC.dialogue.Length)
            {
                haveDialog = false;
                Scene_Controller.pause = false;
                Scene_Controller.Freze = false;
                gameObject.SetActive(false);
            }
            else
            {
                if(DC.dialogue[DC.phrase].Contains("@"))
                {
                    //Debug.Log(int.Parse(DC.dialogue[DC.phrase].Split(':')[1]));
                    switch(DC.dialogue[DC.phrase].Remove(0,3).Split(':')[0])
                    {
                        case "give":
                            {
                                Debug.Log(Scene_Controller.ItemSheet.Items[int.Parse(DC.dialogue[DC.phrase].Split(':')[1])].Resource.name);
                                if(parent.GetComponent<person>().Inv.Items.Exists(e => e.ID == Scene_Controller.ItemSheet.Items[int.Parse(DC.dialogue[DC.phrase].Split(':')[1])].ID))
                                {
                                    GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.TransferItems(parent.GetComponent<person>().Inv, int.Parse(DC.dialogue[DC.phrase].Split(':')[1]));
                                    write(DC.dialogue[DC.phrase].Split(':')[2]);
                                }
                                else
                                    write("I dont have any more.");
                                GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.DrawInv();
                                //GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.DrawAllInv();
                            }
                            break;
                        case "take":
                            {
                                if (GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.Items.Exists((e => e.ID == Scene_Controller.ItemSheet.Items[int.Parse(DC.dialogue[DC.phrase].Split(':')[1])].ID)))
                                {
                                    parent.GetComponent<person>().Inv.TransferItems(GameObject.Find("player").GetComponent<SimpleCharacter>().Inv, int.Parse(DC.dialogue[DC.phrase].Split(':')[1]));
                                    write(DC.dialogue[DC.phrase].Split(':')[2]);
                                }
                                else
                                    write("I dont have any more.");
                                GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.DrawInv();
                                GameObject.Find("player").GetComponent<SimpleCharacter>().Inv.DrawAllInv();
                            }
                            break;
                    }
                }
                else
                    write(DC.dialogue[DC.phrase].Remove(0,2));
            }
            
        }
	}

    public void write(string text)
    {
        gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = text;
    }
    public void off()
    {
        gameObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
    }
    public void Dialog(string path, GameObject father)
    {
        parent = father;
        Scene_Controller.pause = true;
        Scene_Controller.Freze = true;
        haveDialog = true;
        DC.Load(path);
        write(DC.dialogue[0]);
    }
}
