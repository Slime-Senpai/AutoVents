using RimWorld;
using System.Text;
using Verse;

namespace AutoVents
{
    class Comp_AirFlow : CompTempControl
    {
        public bool isAirFlowing = false;
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("TargetTemperature".Translate() + ": ");
			stringBuilder.Append(targetTemperature.ToStringTemperature("F0"));
			if (!isAirFlowing)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("VentClosed".Translate());
			}
			return stringBuilder.ToString();
		}
	}
}
