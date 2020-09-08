using RimWorld;
using Verse;

namespace AutoVents
{
    public class Building_AutomaticVent : Building_TempControl
    {
		private Comp_AirFlow airFlowComp;

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
			airFlowComp = GetComp<Comp_AirFlow>();
		}

		public override void TickRare()
		{
			if (!compPowerTrader.PowerOn || !FlickUtility.WantsToBeOn(this))
			{
				airFlowComp.isAirFlowing = false;

				return;
			}

			IntVec3 intVec = Position + IntVec3.South.RotatedBy(Rotation);
			IntVec3 intVec2 = Position + IntVec3.North.RotatedBy(Rotation);

			if (intVec2.Impassable(Map) || intVec.Impassable(Map))
			{
				airFlowComp.isAirFlowing = false;

				return;
			}
			float temperatureRed = intVec2.GetTemperature(Map);
			float temperature = intVec.GetTemperature(Map);

			if (temperatureRed == airFlowComp.targetTemperature || temperatureRed == temperature || (temperatureRed < airFlowComp.targetTemperature && temperatureRed > temperature) || (temperatureRed > airFlowComp.targetTemperature && temperatureRed < temperature))
            {
				airFlowComp.isAirFlowing = false;

				return;
            }

			GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f, twoWay: true);
			airFlowComp.isAirFlowing = true;
		}
	}
}
