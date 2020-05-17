using RimWorld;
using Verse;

namespace Plague
{
    public class ThingDef_PlagueBullet : ThingDef
    {
        public float AddHediffChance = 0.05f;
        public HediffDef HediffToAdd = HediffDefOf.Plague;
    }

    public class Projectile_PlagueBullet : Bullet
    {
        #region Properties
        public ThingDef_PlagueBullet Def
        {
            get
            {
                return def as ThingDef_PlagueBullet;
            }
        }
        #endregion Properties
        #region Overrides
        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);
            
            if (Def != null && hitThing != null && hitThing is Pawn hitPawn)
            {
                var rand = Rand.Value;
                if (rand <= Def.AddHediffChance)
                {                   
                    Messages.Message("TST_PlagueBullet_SuccessMessage".Translate(new object[] {
                        this.launcher.Label, hitPawn.Label
                    }), MessageTypeDefOf.NeutralEvent);

                    var plagueOnPawn = hitPawn?.health?.hediffSet?.GetFirstHediffOfDef(Def.HediffToAdd);
                    var randomSeverity = Rand.Range(0.15f, 0.30f);
                    if (plagueOnPawn != null)
                    {
                        plagueOnPawn.Severity += randomSeverity;
                    }
                    else
                    {
                        Hediff hediff = HediffMaker.MakeHediff(Def.HediffToAdd, hitPawn, null);
                        hediff.Severity = randomSeverity;
                        hitPawn.health.AddHediff(hediff, null, null);
                    }
                }
                else 
                {
                    //MoteMaker.ThrowText(hitThing.PositionHeld.ToVector3(), hitThing.MapHeld, "TST_PlagueBullet_FailureMote".Translate(Def.AddHediffChance), 12f);
                }
            }
        }
        #endregion Overrides
    }
}