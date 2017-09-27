using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.scripts
{
    class DoorsEvent : MonoBehaviour
    {
        public string nextScene;
        void OnTriggerEnter2D(Collider2D col)
        {
            NextScene ns = Scene_Controller.scene.Layer2.Find(p => p.Name == gameObject.name)as NextScene;
            Debug.Log(ns.nextScene);
            nextScene = ns.nextScene.Split(';')[0];
            Scene_Controller.scrtransition = new FadeOut_flash(0.5f, 0.1F, nextScene, true, ns.pos);
            Scene_Controller.transition = true;
        }
    }
}
