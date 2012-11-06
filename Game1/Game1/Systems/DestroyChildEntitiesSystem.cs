using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using Shooter.Systems.MarkForDestructionSystems;
using Shooter.Systems.OnDestructionSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Systems
{
    /// <summary>
    /// This class is strange because it both marks entities as to be destroyed and responds to that marking.
    /// NOTE: This class assumes that the graph of entity dependency induced by DestroyOnParentDestructionComponent links
    /// is a ***TREE***.  It will run forever if not
    /// </summary>
    public class DestroyChildEntitiesSystem : ASystem
    {
        private ListEntityManager entities;

        public DestroyChildEntitiesSystem()
        {
            SetReqTypes(new List<Type>() { typeof(DestroyedOnParentDestroyedComponent) });
            SetChildren(new List<Type>() { typeof(PreOnDestructionSystem) });
            SetParents(new List<Type>() { typeof(PostMarkForDestructionSystem) });
        }

        public override void Initialize(EntityManagerManager entityStorage)
        {
            entities = entityStorage.DefaultManager;
        }

        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            foreach (Entity ent in entities.entities)
            {
                if (IsApplicableTo(ent))
                {
                    process(ent);
                }
            }
        }

        private void process(Entity child)
        {
            DestroyedOnParentDestroyedComponent comp = (DestroyedOnParentDestroyedComponent)child.components[typeof(DestroyedOnParentDestroyedComponent)];
            Entity parent = comp.parent;

            //first, recurse all the way up the tree to make sure that the parent is marked if possible
            if (IsApplicableTo(parent))
            {
                process(parent);
            }

            //If the parent has been marked for destruction
            if (parent.components.ContainsKey(typeof(MarkedForDeathComponent)))
            {
                //Ensure the child is marked also
                if (!child.components.ContainsKey(typeof(MarkedForDeathComponent)))
                {
                    child.AddComponent(new MarkedForDeathComponent());
                }
            }
        }
    }
}
