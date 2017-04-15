using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    class FadeOut : ScreenTransition
    {
        string scenePath;
        bool nextIn;
        public FadeOut(float Duration, float Speed, string SceneInPath,bool nextIn)
        {
            this.Speed = Speed;
            this.Duration = Duration;
            Remaining = 0;
            scenePath = SceneInPath;
            this.nextIn = nextIn;
        }
        public override void Update()
        {
            Remaining += Speed;
            //Debug.Log(Remaining / Duration);
            if (Remaining < Duration / 2)
                GameObject.Find("Main Camera").GetComponent<GUITexture>().color = new Color(0, 0, 0, Remaining / Duration);
            else
            {
                GameObject.Find("Main Camera").GetComponent<GUITexture>().color = new Color(0, 0, 0, 0.5f);
                Scene_Controller.scene.Load(scenePath);
                //Scene_Controller.transition = false;
                if(nextIn)
                    Scene_Controller.scrtransition = new FadeIn(10f, 0.1F);
            }
        }
    }
}
