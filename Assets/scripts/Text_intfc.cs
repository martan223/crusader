using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_intfc : MonoBehaviour {

	// Use this for initialization
    Dialog_controller DC;
    bool haveDialog;
	void Start () {
        DC = new Dialog_controller();
        haveDialog = false;
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
                gameObject.SetActive(false);
            }
            else
            {
                
                write(DC.dialogue[DC.phrase].Remove(1,1));
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
    public void Dialog(string path)
    {
        Scene_Controller.pause = true;
        haveDialog = true;
        DC.Load(path);
        write(DC.dialogue[0]);
    }
}
