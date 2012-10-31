using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class RenderLayerComponent : IComponent
    {
        public int LayerID;

        public IComponent Clone()
        {
            RenderLayerComponent ret = new RenderLayerComponent();
            ret.LayerID = LayerID;

            return ret;
        }
    }
}
