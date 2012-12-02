using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.EntityInitialization
{
    public interface IEntityFactory
    {
        public Entity CreateEntity(Entity parentEntity, Entity existing);
    }
}
