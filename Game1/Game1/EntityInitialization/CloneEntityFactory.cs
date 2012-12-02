using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.EntityInitialization
{
    public class CloneEntityFactory : IEntityFactory
    {
        private Entity template;

        public CloneEntityFactory(Entity template)
        {
            this.template = template;
        }

        public Entity CreateEntity(Entity parentEntity, Entity existing)
        {
            if(existing.components.Count != 0)
            {
                throw new ArgumentException("Nonempty Entity provided and discarded.");
            }

            return template.Clone();
        }
    }
}
