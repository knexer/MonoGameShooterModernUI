using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Shooter.Components;
using Shooter.EntityManagers;
using Shooter.Systems.OnDestructionSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Systems
{
    public class PlayMusicSystem : ASystem
    {
        private ListEntityManager entities;

        public PlayMusicSystem()
        {
            SetReqTypes(new List<Type>() { typeof(MusicComponent) });
            SetChildren(new List<Type>() { typeof(SpawnEntitySystem), typeof(SpawnEntityAtPositionSystem) });
            SetParents(new List<Type>() { typeof(PostOnDestructionSystem) });
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
                    MusicComponent comp = (MusicComponent)ent.components[typeof(MusicComponent)];

                    try
                    {
                        MediaPlayer.Play(comp.music);

                        MediaPlayer.IsRepeating = comp.repeat;
                    }
                    catch { } //catch all the things!

                    //Only play the music once!
                    ent.RemoveComponent(comp);
                }
            }
        }
    }
}
