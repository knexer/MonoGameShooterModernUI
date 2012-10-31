using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class ScreenWrappingComponent : IComponent
    {
        public IComponent Clone()
        {
            return new ScreenWrappingComponent();
        }
    }
}
