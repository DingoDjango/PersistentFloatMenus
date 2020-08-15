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

			Harmony harmony = new Harmony("dingo.persistentfloatmenus");

			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}

		[HarmonyPatch(typeof(FloatMenu))]
		[HarmonyPatch("UpdateBaseColor")]
		public static class Patch_Transpiler_FloatMenu
		{
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				foreach (CodeInstruction code in instructions)
				{
					// ldc.r4 - Pushes a supplied value of type float32 onto the evaluation stack as type float
					if (code.opcode == OpCodes.Ldc_R4 && (float)code.operand == Settings.DefaultClosingMouseDist)
					{
						// ldsfld - Pushes the value of a static field onto the evaluation stack
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
}
