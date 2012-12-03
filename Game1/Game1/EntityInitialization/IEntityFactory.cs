using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.EntityInitialization
{
    public interface IEntityFactory
    {
        Entity CreateEntity(Entity parentEntity, Entity existing);

        IEntityFactory Clone();
    }
}
