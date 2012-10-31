using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Comparators;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public class RenderSystem : ASystem
    {
        private SortedListEntityManager renderables;

        private SpriteBatch spriteBatch;

        public RenderSystem(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            SetReqTypes(new List<Type> { typeof(TextureComponent), typeof(PositionComponent), typeof(RenderLayerComponent) });
            SetParents(new List<Type> { typeof(AnimationResolutionSystem) });
        }

        public void Process(Entity toProcess, GameTime gameTime)
        {
            if (!IsApplicableTo(toProcess))
            {
                return;
            }

            PositionComponent pos = (PositionComponent)toProcess.components[typeof(PositionComponent)];
            TextureComponent tex = (TextureComponent)toProcess.components[typeof(TextureComponent)];

            spriteBatch.Draw(tex.Texture, pos.Position, tex.SourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }


        public override void Initialize(EntityManagerManager entityStorage)
        {
            renderables = new SortedListEntityManager();
            renderables.requiredComponents.Add(typeof(RenderLayerComponent));
            renderables.comparator = new RenderLayerComparator();

            entityStorage.AddEntityManager(renderables);
        }


        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            foreach (Entity ent in renderables.entities)
            {
                Process(ent, gameTime);
            }
        }
    }
}
