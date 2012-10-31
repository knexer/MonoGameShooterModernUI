using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class AnimationComponent : IComponent
    {
        public int CurrentFrameIndex;
        public int TimeSinceFrameChange;
        public int FrameDuration;
        public int NumFrames;
        public bool Looping;

        public IComponent Clone()
        {
            AnimationComponent ret = new AnimationComponent();

            ret.CurrentFrameIndex = CurrentFrameIndex;
            ret.TimeSinceFrameChange = TimeSinceFrameChange;
            ret.FrameDuration = FrameDuration;
            ret.NumFrames = NumFrames;
            ret.Looping = Looping;

            return ret;
        }
    }
}
