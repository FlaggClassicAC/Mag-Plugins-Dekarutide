using Decal.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagTools.ItemInfo
{
	public struct SpellStub
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public bool ShowInfo { get; set; }
		public int Family { get; set; }
		public bool IsDebuff { get; set; }
		public bool IsOffensive { get; set; }
		public int Difficulty { get; set; }

		public SpellStub(Spell spell)
		{
			ID = spell.Id;
			Name = spell.Name;
			Family = spell.Family;
			IsDebuff = spell.IsDebuff;
			IsOffensive = spell.IsOffensive;
			Difficulty = spell.Difficulty;

			ShowInfo = true;
		}
	}
}
