using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Systems.OnDestructionSystems
{
    public class AddComponentOnDestructionSystem : ASystem
    {
        private ListEntityManager entities;

        public AddComponentOnDestructionSystem()
        {
            SetReqTypes(new List<Type>() { typeof(AddComponentOnDestructionComponent), typeof(MarkedForDeathComponent) });
            SetParents(new List<Type>() { typeof(PreOnDestructionSystem) });
            SetChildren(new List<Type>() { typeof(PostOnDestructionSystem) });
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
                    AddComponentOnDestructionComponent wrapper = (AddComponentOnDestructionComponent)ent.components[typeof(AddComponentOnDestructionComponent)];
                    IComponent compTemplate = wrapper.ToAdd;

                    IComponent comp = compTemplate.Clone();

                    ent.AddComponent(comp);
                }
            }
        }
    }
}
