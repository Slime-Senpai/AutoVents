using RimWorld;
using Verse;

namespace AutoVents
{
	public class Building_AutomaticVent : Building_TempControl
	{
		private Comp_AirFlow airFlowComp;
		
		private VacuumComponent intVacuum;

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			airFlowComp = GetComp<Comp_AirFlow>();
		}
		
		private VacuumComponent Vacuum => intVacuum ?? (intVacuum = Map.GetComponent<VacuumComponent>());

		public override bool ExchangeVacuum => base.ExchangeVacuum || airFlowComp.isAirFlowing;

		public override void TickRare()
		{
			if (!compPowerTrader.PowerOn || !FlickUtility.WantsToBeOn(this))
			{
				HandleAirFlowChange(false);

				return;
			}

			var intVec = Position + IntVec3.South.RotatedBy(Rotation);
			var intVec2 = Position + IntVec3.North.RotatedBy(Rotation);

			if (intVec2.Impassable(Map) || intVec.Impassable(Map))
			{
				HandleAirFlowChange(false);

				return;
			}

			var temperatureRed = intVec2.GetTemperature(Map);
			var temperature = intVec.GetTemperature(Map);

			if (temperatureRed == airFlowComp.targetTemperature || temperatureRed == temperature ||
			    (temperatureRed < airFlowComp.targetTemperature && temperatureRed > temperature) ||
			    (temperatureRed > airFlowComp.targetTemperature && temperatureRed < temperature))
			{
				HandleAirFlowChange(false);

				return;
			}

			HandleAirFlowChange(true);

			GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f, true);
			Map.gasGrid.EqualizeGasThroughBuilding(this, true);
		}
		
		private void HandleAirFlowChange(bool newAirFlow)
		{
			if (airFlowComp.isAirFlowing == newAirFlow)
			{
				return;
			}
			
			airFlowComp.isAirFlowing = newAirFlow;
			Vacuum.Dirty();
		}
	}
}