using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.scripts
{
    class DoorsEvent : MonoBehaviour
    {
        string nextScene;
        void OnTriggerEnter2D(Collider2D col)
        {
            NextScene ns = Scene_Controller.scene.Layer2.Find(p => p.Name == gameObject.name)as NextScene;
            nextScene = ns.nextScene;
            Scene_Controller.scrtransition = new FadeOut(10f, 0.1F, nextScene, true);
            Scene_Controller.transition = true;
        }
    }
}
