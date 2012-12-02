using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.EntityInitialization
{
    /// <summary>
    /// Modifies the existing entity so that it inherits a copy of the parent entity's instance of a component of a particular type.
    /// </summary>
    public class InheritParentComponentEntityFactory : IEntityFactory
    {
        private Type inheritedType;

        public InheritParentComponentEntityFactory(Type toInherit)
        {
            inheritedType = toInherit;
        }

        public Entity CreateEntity(Entity parentEntity, Entity existing)
        {
            if (existing == null)
            {
                //Or create a new entity object here?
                throw new ArgumentNullException();
            }

            if (!parentEntity.components.ContainsKey(inheritedType))
            {
                throw new ArgumentException("The parent entity does not have a component of the required type.");
            }

            existing.components[inheritedType] = parentEntity.components[inheritedType].Clone();

            return existing;
        }

        public IEntityFactory Clone()
        {
            return new InheritParentComponentEntityFactory(inheritedType);
        }
    }
}
