using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    class FadeIn_flash : ScreenTransition
    {
        string scenePath;
        public FadeIn_flash(float Duration, float Speed)
        {
            this.Speed = Speed;
            this.Duration = Remaining = Duration;
            Remaining = Duration / 2;
        }
        public override void Update()
        {
            
            Remaining -= Speed;
            //Debug.Log(Remaining/Duration);
            if (Remaining > 0.5f)
                GameObject.Find("Main Camera").GetComponent<GUITexture>().color = new Color(255, 255, 255, Remaining / Duration);
            else
            {
                GameObject.Find("Main Camera").GetComponent<GUITexture>().color = new Color(255, 255, 255, 0);
                //Scene_Controller.scene.Load(scenePath);
                Scene_Controller.transition = false;
            }
        }
    }
}
