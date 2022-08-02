using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace Persistent_Float_Menus
{
	[StaticConstructorOnStartup]
	public static class HarmonyPatches
	{
		private static readonly FieldInfo GetModifiedDistanceToClose = AccessTools.Field(typeof(Settings), nameof(Settings.ModifiedDistanceToClose));

		static HarmonyPatches()
		{
#if DEBUG
			Harmony.DEBUG = true;
#endif

			Harmony harmony = new Harmony("dingo.rimworld.persistent_float_menus");

			// Patch: Verse.FloatMenu.UpdateBaseColor
			harmony.Patch(
				original: AccessTools.Method(typeof(FloatMenu), "UpdateBaseColor"),
				transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.Patch_UpdateBaseColor_Transpiler)));			
		}

		public static IEnumerable<CodeInstruction> Patch_UpdateBaseColor_Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			foreach (CodeInstruction code in instructions)
			{
				// ldc.r4 - Pushes a supplied value of type float32 onto the evaluation stack as type float
				// Vanilla compares to 95f a few times (FadeFinish minus FadeStart)
				if (code.opcode == OpCodes.Ldc_R4 && (float)code.operand == Settings.DefaultClosingMouseDist)
				{
					// ldsfld - Pushes the value of a static field onto the evaluation stack
					// Tell the game to defer to our distance value (from Settings)
					CodeInstruction replacement = new CodeInstruction(OpCodes.Ldsfld, GetModifiedDistanceToClose);

					yield return replacement;
				}

				else
				{
					yield return code;
				}
			}
		}
	}
}
