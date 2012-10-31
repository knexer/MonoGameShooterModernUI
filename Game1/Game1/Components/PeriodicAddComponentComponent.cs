using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class PeriodicAddComponentComponent : IComponent
    {
        public IComponent ComponentToAdd;

        public float Period { get; set; }

        public float TimeSinceLastFiring { get; set; }

        public IComponent Clone()
        {
            PeriodicAddComponentComponent ret = new PeriodicAddComponentComponent();
            ret.ComponentToAdd = ComponentToAdd.Clone();
            ret.Period = Period;
            ret.TimeSinceLastFiring = TimeSinceLastFiring;

            return ret;
        }
    }
}
