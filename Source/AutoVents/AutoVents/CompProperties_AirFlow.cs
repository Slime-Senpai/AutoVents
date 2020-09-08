using RimWorld;

namespace AutoVents
{
    class CompProperties_AirFlow : CompProperties_TempControl
    {
        public CompProperties_AirFlow()
        {
            compClass = typeof(Comp_AirFlow);
        }
    }
}
