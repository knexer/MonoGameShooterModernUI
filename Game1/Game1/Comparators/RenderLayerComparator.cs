using Shooter.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Comparators
{
    public class RenderLayerComparator : IComparer<Entity>
    {
        public int Compare(Entity x, Entity y)
        {
            RenderLayerComponent xLayer = (RenderLayerComponent)x.components[typeof(RenderLayerComponent)];
            RenderLayerComponent yLayer = (RenderLayerComponent)y.components[typeof(RenderLayerComponent)];

            if (xLayer.LayerID < yLayer.LayerID)
            {
                return -1;
            }
            else if (xLayer.LayerID > yLayer.LayerID)
            {
                return 1;
            }
            return 0;
        }
    }
}
