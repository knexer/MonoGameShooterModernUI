using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.EntityInitialization
{
    /// <summary>
    /// Applies an ordered list of entity factories to create an entity.
    /// </summary>
    public class ComposedEntityFactory : IEntityFactory
    {
        private List<IEntityFactory> entityFactories;

        public ComposedEntityFactory(List<IEntityFactory> entityFactories)
        {
            this.entityFactories = new List<IEntityFactory>(entityFactories);
        }

        public Entity CreateEntity(Entity parentEntity, Entity existing)
        {
            Entity ret = new Entity();
            foreach(IEntityFactory factory in entityFactories)
            {
                ret = factory.CreateEntity(parentEntity, ret);
            }

            return ret;
        }
    }
}
