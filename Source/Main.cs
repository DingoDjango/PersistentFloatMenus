using UnityEngine;
using Verse;

namespace PersistentFloatMenus
{
	public class Main : Mod
	{
		public Main(ModContentPack content) : base(content)
		{
			this.GetSettings<Settings>();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			base.DoSettingsWindowContents(inRect);

			Settings.DoSettingsWindowContents(inRect);
		}

		public override string SettingsCategory()
		{
			return "Persistent Float Menus";
		}
	}
}
