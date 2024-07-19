using System.Collections.Generic;
using Decal.Adapter;
using Decal.Filters;
using MagTools.ItemInfo;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace MagTools.Custom
{
	public class CustomSpellTable
	{
		SpellTable defaultTable;


		Dictionary<int, SpellStub> initial = InitCustom();
		Dictionary<int, SpellStub> cache = InitCustom();

		public CustomSpellTable(SpellTable defaultTable = null)
		{
			this.defaultTable = defaultTable ?? CoreManager.Current.Filter<FileService>().SpellTable;
		}

		private static Dictionary<int, SpellStub> InitCustom()
		{
			var customcsv = @"Id,Name,Description,Difficulty,Duration,Family,Mana,School,Speed,TargetEffect,TargetMask,Unknown1,Unknown2,Unknown3
6341,Armor Mastery Self I,Increases the caster's Armor skill by 10 points.,1,1800,734,15,Creature Enchantment,0.01,18,16,0,0,1
6342,Armor Mastery Self II,Increases the caster's Armor skill by 15 points.,50,1800,734,30,Creature Enchantment,0.05,18,16,0,0,1
6343,Armor Mastery Self III,Increases the caster's Armor skill by 20 points.,100,1800,734,40,Creature Enchantment,0.1,18,16,0,0,1
6344,Armor Mastery Self IV,Increases the caster's Armor skill by 25 points.,150,1800,734,50,Creature Enchantment,0.15,18,16,0,0,1
6345,Armor Mastery Self V,Increases the caster's Armor skill by 30 points.,200,1800,734,60,Creature Enchantment,0.2,18,16,0,0,1
6346,Armor Mastery Self VI,Increases the caster's Armor skill by 35 points.,250,2700,734,70,Creature Enchantment,0.3,18,16,0,0,1
6347,Armor Mastery Self VII,Increases the caster's Armor skill by 40 points.,300,2700,734,70,Creature Enchantment,0.3,18,16,0,0,1
6348,Incantation of Armor Mastery Self,Increases the caster's Armor skill by 45 points.,400,2700,734,70,Creature Enchantment,0.3,18,16,0,0,1
6349,Armor Mastery Other I,Increases the target's Armor skill by 10 points.,1,1800,734,15,Creature Enchantment,0.01,18,16,0,0,1
6350,Armor Mastery Other II,Increases the target's Armor skill by 15 points.,50,1800,734,30,Creature Enchantment,0.05,18,16,0,0,1
6351,Armor Mastery Other III,Increases the target's Armor skill by 20 points.,100,1800,734,40,Creature Enchantment,0.1,18,16,0,0,1
6352,Armor Mastery Other IV,Increases the target's Armor skill by 25 points.,150,1800,734,50,Creature Enchantment,0.15,18,16,0,0,1
6353,Armor Mastery Other V,Increases the target's Armor skill by 30 points.,200,1800,734,60,Creature Enchantment,0.2,18,16,0,0,1
6354,Armor Mastery Other VI,Increases the target's Armor skill by 35 points.,250,2700,734,70,Creature Enchantment,0.3,18,16,0,0,1
6355,Armor Mastery Other VII,Increases the target's Armor skill by 40 points.,300,2700,734,70,Creature Enchantment,0.3,18,16,0,0,1
6356,Incantation of Armor Mastery Other,Increases the target's Armor skill by 45 points.,300,2700,734,70,Creature Enchantment,0.3,18,16,0,0,1
6357,Armor Ineptitude Other I,Decreases the target's Armor skill by 10 points.,1,60,735,10,Creature Enchantment,0.01,27,16,0,1,1
6358,Armor Ineptitude Other II,Decreases the target's Armor skill by 15 points.,75,90,735,20,Creature Enchantment,0.05,27,16,5,0.85,1
6359,Armor Ineptitude Other III,Decreases the target's Armor skill by 20 points.,125,120,735,30,Creature Enchantment,0.1,27,16,5,0.7,1
6360,Armor Ineptitude Other IV,Decreases the target's Armor skill by 25 points.,175,150,735,40,Creature Enchantment,0.15,27,16,5,0.55,1
6361,Armor Ineptitude Other V,Decreases the target's Armor skill by 30 points.,225,180,735,50,Creature Enchantment,0.2,27,16,5,0.4,1
6362,Armor Ineptitude Other VI,Decreases the target's Armor skill by 35 points.,275,210,735,60,Creature Enchantment,0.3,27,16,5,0.25,1
6363,Armor Ineptitude Other VII,Decreases the target's Armor skill by 40 points.,325,210,735,60,Creature Enchantment,0.3,27,16,5,0.25,1
6364,Incantation of Armor Ineptitude Other,Decreases the target's Armor skill by 45 points.,425,210,735,60,Creature Enchantment,0.3,27,16,5,0.25,1
6365,Minor Armor Aptitude,,275,210,52,60,Creature Enchantment,0.3,27,736,5,0.25,1
6366,Major Armor Aptitude,,275,210,52,60,Creature Enchantment,0.3,27,736,5,0.25,1
6367,Epic Armor Aptitude,,275,210,52,60,Creature Enchantment,0.3,27,736,5,0.25,1
6368,Legendary Armor Aptitude,,275,210,52,60,Creature Enchantment,0.3,27,736,5,0.25,1
6369,Awareness Mastery Self I,Increases the caster's Awareness skill by 10 points.,1,1800,734,15,Creature Enchantment,0.01,18,16,0,0,1
6370,Awareness Mastery Self II,Increases the caster's Awareness skill by 15 points.,50,1800,734,30,Creature Enchantment,0.05,18,16,0,0,1
6371,Awareness Mastery Self III,Increases the caster's Awareness skill by 20 points.,100,1800,734,40,Creature Enchantment,0.1,18,16,0,0,1
6372,Awareness Mastery Self IV,Increases the caster's Awareness skill by 25 points.,150,1800,734,50,Creature Enchantment,0.15,18,16,0,0,1
6373,Awareness Mastery Self V,Increases the caster's Awareness skill by 30 points.,200,1800,734,60,Creature Enchantment,0.2,18,16,0,0,1
6374,Awareness Mastery Self VI,Increases the caster's Awareness skill by 35 points.,250,2700,734,70,Creature Enchantment,0.3,18,16,0,0,1
6375,Awareness Mastery Self VII,Increases the caster's Awareness skill by 40 points.,300,2700,734,70,Creature Enchantment,0.3,18,16,0,0,1
6376,Incantation of Awareness Mastery Self,Increases the caster's Awareness skill by 45 points.,400,2700,734,70,Creature Enchantment,0.3,18,16,0,0,1
6377,Awareness Mastery Other I,Increases the target's Awareness skill by 10 points.,1,1800,734,15,Creature Enchantment,0.01,18,16,0,0,1
6378,Awareness Mastery Other II,Increases the target's Awareness skill by 15 points.,50,1800,734,30,Creature Enchantment,0.05,18,16,0,0,1
6379,Awareness Mastery Other III,Increases the target's Awareness skill by 20 points.,100,1800,734,40,Creature Enchantment,0.1,18,16,0,0,1
6380,Awareness Mastery Other IV,Increases the target's Awareness skill by 25 points.,150,1800,734,50,Creature Enchantment,0.15,18,16,0,0,1
6381,Awareness Mastery Other V,Increases the target's Awareness skill by 30 points.,200,1800,734,60,Creature Enchantment,0.2,18,16,0,0,1
6382,Awareness Mastery Other VI,Increases the target's Awareness skill by 35 points.,250,2700,734,70,Creature Enchantment,0.3,18,16,0,0,1
6383,Awareness Mastery Other VII,Increases the target's Awareness skill by 40 points.,300,2700,734,70,Creature Enchantment,0.3,18,16,0,0,1
6384,Incantation of Awareness Mastery Other,Increases the target's Awareness skill by 45 points.,300,2700,734,70,Creature Enchantment,0.3,18,16,0,0,1
6385,Awareness Ineptitude Other I,Decreases the target's Awareness skill by 10 points.,1,60,735,10,Creature Enchantment,0.01,27,16,0,1,1
6386,Awareness Ineptitude Other II,Decreases the target's Awareness skill by 15 points.,75,90,735,20,Creature Enchantment,0.05,27,16,5,0.85,1
6387,Awareness Ineptitude Other III,Decreases the target's Awareness skill by 20 points.,125,120,735,30,Creature Enchantment,0.1,27,16,5,0.7,1
6388,Awareness Ineptitude Other IV,Decreases the target's Awareness skill by 25 points.,175,150,735,40,Creature Enchantment,0.15,27,16,5,0.55,1
6389,Awareness Ineptitude Other V,Decreases the target's Awareness skill by 30 points.,225,180,735,50,Creature Enchantment,0.2,27,16,5,0.4,1
6390,Awareness Ineptitude Other VI,Decreases the target's Awareness skill by 35 points.,275,210,735,60,Creature Enchantment,0.3,27,16,5,0.25,1
6391,Awareness Ineptitude Other VII,Decreases the target's Awareness skill by 40 points.,325,210,735,60,Creature Enchantment,0.3,27,16,5,0.25,1
6392,Incantation of Awareness Ineptitude Other,Decreases the target's Awareness skill by 45 points.,425,210,735,60,Creature Enchantment,0.3,27,16,5,0.25,1
6393,Minor Awareness Aptitude,,275,210,52,60,Creature Enchantment,0.3,27,736,5,0.25,1
6394,Major Awareness Aptitude,,275,210,52,60,Creature Enchantment,0.3,27,736,5,0.25,1
6395,Epic Awareness Aptitude,,275,210,52,60,Creature Enchantment,0.3,27,736,5,0.25,1
6396,Legendary Awareness Aptitude,,275,210,52,60,Creature Enchantment,0.3,27,736,5,0.25,1".Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			var fields = customcsv[0].Split(',');
			var list = new Dictionary<int, SpellStub>();
			for (int i = 1; i < customcsv.Length; i++)
			{
				var row = customcsv[i].Split(',');
				var id = int.Parse(row[0]);
				list[id] = new SpellStub()
				{
					Family = int.Parse(row[10]),
					ID = id,
					Name = row[1],
					ShowInfo = true,
					IsOffensive = row[1].Contains("Ineptitude"), // bandaid, fix as needed
					IsDebuff = row[1].Contains("Ineptitude"),
					Difficulty = int.Parse(row[3])
				};
			}
			return list;
		}

		public SpellStub? GetById(int id)
		{
			if (cache.ContainsKey(id))
				return cache[id];

			return SpellStubFromTable(defaultTable.GetById(id));
		}

		private SpellStub? SpellStubFromTable(Spell spell)
		{
			if (spell == null)
				return null; // Need to add missing spell to cache above
			var ss = new SpellStub(spell);
			cache[ss.ID] = ss;
			return ss;
		}

		public int Length { get { return defaultTable.Length + initial.Count; } }

		public SpellStub? this[int id]
		{
			get
			{
				return GetById(id);
			}
		}
	}
}
