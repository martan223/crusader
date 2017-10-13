using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{

    bool Colliding
    {
        get
        {
            return Father.GetComponent<person>().colliding;
        }
        set
        {
            Father.GetComponent<person>().colliding = value;
        }
    }
    public GameObject Father;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == Scene_Controller.player)
            Colliding = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == Scene_Controller.player)
            Colliding = false;
    }
}
