using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace DIYHumans
{
    public class CompCraftedPawn : ThingComp
    {
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
        }

        public CompProperties_CraftedPawn Props
        {
            get
            {
                return (CompProperties_CraftedPawn)this.props;
            }
        }

        /// <summary>
        /// spawns pawn and applies mental state if required
        /// </summary>
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            Faction pawnFaction = this.Props.faction;
            if (pawnFaction == null)
            {
                pawnFaction = Faction.OfPlayer;
            }
            bool pawnIsNewborn = this.Props.pawnKindDef.maxGenerationAge == 0;
            Pawn pawnToSpawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.Props.pawnKindDef, pawnFaction, PawnGenerationContext.NonPlayer, -1, false, pawnIsNewborn, false, false, false, false, 0f, false, true, false, false, false, false, false, false, 0f, null, 0f, null, null, null));
            pawnToSpawn.apparel.DestroyAll();
            IntVec3 spawnPosition = this.parent.Position;
            Map spawnMap = this.parent.Map;
            this.parent.Destroy(DestroyMode.Vanish);
            GenSpawn.Spawn(pawnToSpawn, spawnPosition, spawnMap, WipeMode.Vanish);
            if (!this.Props.possibleMentalStates.NullOrEmpty() && Rand.Value < this.Props.mentalStateChance)
            {
                MentalStateDef mentalState = this.Props.possibleMentalStates.RandomElement();
                pawnToSpawn.mindState.mentalStateHandler.TryStartMentalState(mentalState, null, false, false, null, false);
            }
        }
    }
}
