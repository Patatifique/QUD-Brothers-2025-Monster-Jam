using System;
using XRL.UI;
using UnityEngine;
using Occult.Engine.CodeGeneration;
using System.CodeDom.Compiler;
using XRL.World.Capabilities;

using ConsoleLib.Console;
using System;
using XRL.Wish;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;



#nullable disable
namespace XRL.World.Parts
{
    [Serializable]
    public class Brothers_TurnIntoZombieOnDeath : IPart
    {

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == EquippedEvent.ID
                   || ID == UnequippedEvent.ID;
        }

        public override bool HandleEvent(EquippedEvent E)
        {
            E.Actor.RegisterEvent((IEventHandler)this, BeforeDieEvent.ID, 0, false);
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(UnequippedEvent E)
        {
            E.Actor.UnregisterEvent((IEventHandler)this, BeforeDieEvent.ID);
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(BeforeDieEvent E)
        {
            var dying = E.Dying;

            if (!dying.IsValid() || dying != this.ParentObject.Equipped || !this.ParentObject.IsWorn())
                return true;

            // print message
            IComponent<XRL.World.GameObject>.AddPlayerMessage(
                $"{dying.the}{dying.DisplayNameOnly} succumbs... and turns into an annoying dog :^)"
            );

            // replace corpse (with dog for now lmao)
            Corpse Part;
            if (dying.TryGetPart<Corpse>(out Part))
            {
                Part.CorpseChance = 100;
                Part.BurntCorpseChance = 0;
                Part.VaporizedCorpseChance = 0;
                Part.CorpseBlueprint = "Dog";

            }


            return base.HandleEvent(E);
        }

    }
}
