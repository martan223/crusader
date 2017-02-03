using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    class FadeIn : ScreenTransition
    {
        string scenePath;
        public FadeIn(float Duration, float Speed, string SceneInPath)
        {
            this.Speed = Speed;
            this.Duration = Remaining = Duration;
            Remaining = Duration / 2;
            scenePath = SceneInPath;
        }
        public override void Update()
        {
            Remaining -= Speed;
            Debug.Log(Remaining/Duration);
            if (Remaining > 0.5f)
                GameObject.Find("Main Camera").GetComponent<GUITexture>().color = new Color(0, 0, 0, Remaining / Duration);
            else
            {
                GameObject.Find("Main Camera").GetComponent<GUITexture>().color = new Color(0, 0, 0, 0);
                //Scene_Controller.scene.Load(scenePath);
                Scene_Controller.transition = false;
            }
        }
    }
}
