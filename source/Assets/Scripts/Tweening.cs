using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Tweening
    {
        // TRUE
        public bool Enabled { get; set; }

        // TRUE
        public bool Bounce_Y { get; set; }

        // TRUE
        public bool Rotation_Smooth { get; set; }

        // TRUE
        public bool Time_Scale { get; set; }

        // 0.0
        public float Delay { get; set; }

        // TRUE
        public bool Shake { get; set; }

        // 0.7
        public float Duration { get; set; }

        // 0.0
        public float Equation { get; set; }
    }
}
