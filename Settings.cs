using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Persistent_Float_Menus
{
	public class Settings : ModSettings
	{
		private static string modifiedDistanceTextBuffer = "300"; // Unsaved

		public static readonly float DefaultClosingMouseDist = (float)AccessTools.Field(typeof(FloatMenu), "FadeFinishMouseDist").GetValue(null) - (float)AccessTools.Field(typeof(FloatMenu), "FadeStartMouseDist").GetValue(null);

		public static float ModifiedDistanceToClose = 300f;

		public static void DoSettingsWindowContents(Rect rect)
		{
			Listing_Standard modOptions = new Listing_Standard();

			modOptions.Begin(rect);
			modOptions.Gap(20f);

			Rect modifyDistanceRect = modOptions.GetRect(Text.LineHeight);
			Rect labelRect = modifyDistanceRect.LeftPart(0.75f);
			Rect inputRect = modifyDistanceRect.RightPart(0.20f);

			Widgets.Label(labelRect, "PFM.ModifiedDistanceToClose".Translate());
			Widgets.DrawHighlightIfMouseover(labelRect);
			TooltipHandler.TipRegion(labelRect, "PFM.ModifiedDistanceToClose.Tooltip".Translate());
			Widgets.TextFieldNumeric(inputRect, ref ModifiedDistanceToClose, ref modifiedDistanceTextBuffer);

			modOptions.End();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref ModifiedDistanceToClose, "PFM_ModifiedDistanceToClose", 300f);

			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				// Set unsaved values
				modifiedDistanceTextBuffer = ModifiedDistanceToClose.ToString();
			}
		}
	}
}
