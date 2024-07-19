using System;
using System.Collections.Generic;

using Mag.Shared;
using Mag.Shared.Constants;

using Decal.Adapter;
using Decal.Adapter.Wrappers;
using Decal.Filters;

using DoubleValueKey = Decal.Adapter.Wrappers.DoubleValueKey;

namespace MagTools.ItemInfo
{
	/// <summary>
	/// Instantiate this object with the item you want info for.
	/// ToString() this object for the info.
	/// </summary>
	public class ItemInfo
	{
		private readonly WorldObject wo;
		private readonly MyWorldObject mwo;

		public ItemInfo(WorldObject worldObject)
		{
			wo = worldObject;
			mwo = MyWorldObjectCreator.Create(worldObject);
		}

		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			if (wo.Values(LongValueKey.Material) > 0)
			{
				if (Dictionaries.MaterialInfo.ContainsKey(wo.Values(LongValueKey.Material)))
					sb.Append(Dictionaries.MaterialInfo[wo.Values(LongValueKey.Material)] + " ");
				else
					sb.Append("unknown material " + wo.Values(LongValueKey.Material) + " ");
			}

			sb.Append(wo.Name);

			if (wo.Values((LongValueKey)353) > 0)
			{
				if (Dictionaries.MasteryInfo.ContainsKey(wo.Values((LongValueKey)353)))
					sb.Append(" (" + Dictionaries.MasteryInfo[wo.Values((LongValueKey)353)] + ")");
				else
					sb.Append(" (Unknown mastery " + wo.Values((LongValueKey)353) + ")");
			}

			int set = wo.Values((LongValueKey)265, 0);
			if (set != 0)
			{
				sb.Append(", ");
				if (Dictionaries.AttributeSetInfo.ContainsKey(set))
					sb.Append(Dictionaries.AttributeSetInfo[set]);
				else
					sb.Append("Unknown set " + set);
			}

			if (wo.Values(LongValueKey.ArmorLevel) > 0)
				sb.Append(", AL " + wo.Values(LongValueKey.ArmorLevel));

			if (wo.Values(LongValueKey.Imbued) > 0)
			{
				sb.Append(",");
				if ((wo.Values(LongValueKey.Imbued) & 1) == 1) sb.Append(" CS");
				if ((wo.Values(LongValueKey.Imbued) & 2) == 2) sb.Append(" CB");
				if ((wo.Values(LongValueKey.Imbued) & 4) == 4) sb.Append(" AR");
				if ((wo.Values(LongValueKey.Imbued) & 8) == 8) sb.Append(" SlashRend");
				if ((wo.Values(LongValueKey.Imbued) & 16) == 16) sb.Append(" PierceRend");
				if ((wo.Values(LongValueKey.Imbued) & 32) == 32) sb.Append(" BludgeRend");
				if ((wo.Values(LongValueKey.Imbued) & 64) == 64) sb.Append(" AcidRend");
				if ((wo.Values(LongValueKey.Imbued) & 128) == 128) sb.Append(" FrostRend");
				if ((wo.Values(LongValueKey.Imbued) & 256) == 256) sb.Append(" LightRend");
				if ((wo.Values(LongValueKey.Imbued) & 512) == 512) sb.Append(" FireRend");
				if ((wo.Values(LongValueKey.Imbued) & 1024) == 1024) sb.Append(" MeleeImbue");
				if ((wo.Values(LongValueKey.Imbued) & 4096) == 4096) sb.Append(" MagicImbue");
				if ((wo.Values(LongValueKey.Imbued) & 8192) == 8192) sb.Append(" Hematited");
				if ((wo.Values(LongValueKey.Imbued) & 536870912) == 536870912) sb.Append(" MagicAbsorb");
			}

			if (wo.Values(LongValueKey.NumberTimesTinkered) > 0)
				sb.Append(", Tinks " + wo.Values(LongValueKey.NumberTimesTinkered));

			if (wo.Values(LongValueKey.MaxDamage) != 0 && wo.Values(DoubleValueKey.Variance) != 0)
				sb.Append(", " + (wo.Values(LongValueKey.MaxDamage) - (wo.Values(LongValueKey.MaxDamage) * wo.Values(DoubleValueKey.Variance))).ToString("N2") + "-" + wo.Values(LongValueKey.MaxDamage));
			else if (wo.Values(LongValueKey.MaxDamage) != 0 && wo.Values(DoubleValueKey.Variance) == 0)
				sb.Append(", " + wo.Values(LongValueKey.MaxDamage));

			if (wo.Values(LongValueKey.ElementalDmgBonus, 0) != 0)
				sb.Append(", +" + wo.Values(LongValueKey.ElementalDmgBonus));

			if (wo.Values(DoubleValueKey.DamageBonus, 1) != 1)
				sb.Append(", +" + Math.Round(((wo.Values(DoubleValueKey.DamageBonus) - 1) * 100)) + "%");

			if (wo.Values(DoubleValueKey.ElementalDamageVersusMonsters, 1) != 1)
				sb.Append(", +" + Math.Round(((wo.Values(DoubleValueKey.ElementalDamageVersusMonsters) - 1) * 100)) + "%vs. Monsters");

			if (wo.Values(DoubleValueKey.AttackBonus, 1) != 1)
				sb.Append(", +" + Math.Round(((wo.Values(DoubleValueKey.AttackBonus) - 1) * 100)) + "%a");

			if (wo.Values(DoubleValueKey.MeleeDefenseBonus, 1) != 1)
				sb.Append(", " + Math.Round(((wo.Values(DoubleValueKey.MeleeDefenseBonus) - 1) * 100)) + "%md");

			if (wo.Values(DoubleValueKey.MagicDBonus, 1) != 1)
				sb.Append(", " + Math.Round(((wo.Values(DoubleValueKey.MagicDBonus) - 1) * 100), 1) + "%mgc.d");

			if (wo.Values(DoubleValueKey.MissileDBonus, 1) != 1)
				sb.Append(", " + Math.Round(((wo.Values(DoubleValueKey.MissileDBonus) - 1) * 100), 1) + "%msl.d");

			if (wo.Values(DoubleValueKey.ManaCBonus) != 0)
				sb.Append(", " + Math.Round((wo.Values(DoubleValueKey.ManaCBonus) * 100)) + "%mc");

			if (Settings.SettingsManager.ItemInfoOnIdent.ShowBuffedValues.Value && (wo.ObjectClass == ObjectClass.MeleeWeapon || wo.ObjectClass == ObjectClass.MissileWeapon || wo.ObjectClass == ObjectClass.WandStaffOrb))
			{
				sb.Append(", (");

				// (Damage)
				if (wo.ObjectClass == ObjectClass.MeleeWeapon)
					sb.Append(mwo.CalcedBuffedTinkedDoT.ToString("N1") + "/" + mwo.GetBuffedIntValueKey((int)LongValueKey.MaxDamage));

				if (wo.ObjectClass == ObjectClass.MissileWeapon)
					sb.Append(mwo.CalcedBuffedMissileDamage.ToString("N1"));

				if (wo.ObjectClass == ObjectClass.WandStaffOrb)
					sb.Append(((mwo.GetBuffedDoubleValueKey((int)DoubleValueKey.ElementalDamageVersusMonsters) - 1) * 100).ToString("N1"));

				// (AttackBonus/MeleeDefenseBonus/ManaCBonus)
				sb.Append(" ");

				if (wo.Values(DoubleValueKey.AttackBonus, 1) != 1)
					sb.Append(Math.Round(((mwo.GetBuffedDoubleValueKey((int)DoubleValueKey.AttackBonus) - 1) * 100)).ToString("N1") + "/");

				if (wo.Values(DoubleValueKey.MeleeDefenseBonus, 1) != 1)
					sb.Append(Math.Round(((mwo.GetBuffedDoubleValueKey((int)DoubleValueKey.MeleeDefenseBonus) - 1) * 100)).ToString("N1"));

				if (wo.Values(DoubleValueKey.ManaCBonus) != 0)
					sb.Append("/" + Math.Round(mwo.GetBuffedDoubleValueKey((int)DoubleValueKey.ManaCBonus) * 100));

				sb.Append(")");
			}

			if (wo.SpellCount > 0)
			{
				FileService service = CoreManager.Current.Filter<FileService>();

				List<int> itemActiveSpells = new List<int>();

				for (int i = 0 ; i < wo.SpellCount ; i++)
					itemActiveSpells.Add(wo.Spell(i));

				itemActiveSpells.Sort();
				itemActiveSpells.Reverse();

				foreach (int spell in itemActiveSpells)
				{
					var spellStub = ((PluginCore)PluginCore.Current).CustomSpellTable.GetById(spell);
					if (!spellStub.HasValue)
						continue;

					if (!spellStub.Value.ShowInfo)
						continue;

					ShowSpell:

					sb.Append(", " + spellStub.Value.Name);
				}
			}

			// Wield Lvl 180
			if (wo.Values(LongValueKey.WieldReqValue) > 0)
			{
				// I don't quite understand this.
				if (wo.Values(LongValueKey.WieldReqType) == 7 && wo.Values(LongValueKey.WieldReqAttribute) == 1)
					sb.Append(", Wield Lvl " + wo.Values(LongValueKey.WieldReqValue));
				else
				{
					if (Dictionaries.SkillInfo.ContainsKey(wo.Values(LongValueKey.WieldReqAttribute)))
						sb.Append(", " + Dictionaries.SkillInfo[wo.Values(LongValueKey.WieldReqAttribute)] + " " + wo.Values(LongValueKey.WieldReqValue));
					else
						sb.Append(", Unknown skill: " +wo.Values(LongValueKey.WieldReqAttribute) + " " + wo.Values(LongValueKey.WieldReqValue));
				}
			}

			// Summoning Gem
			if (wo.Values((LongValueKey)369) > 0)
				sb.Append(", Lvl " + wo.Values((LongValueKey)369));

			// Melee Defense 300 to Activate
			// If the activation is lower than the wield requirement, don't show it.
			if (wo.Values(LongValueKey.SkillLevelReq) > 0 && (wo.Values(LongValueKey.WieldReqAttribute) != wo.Values(LongValueKey.ActivationReqSkillId) || wo.Values(LongValueKey.WieldReqValue) < wo.Values(LongValueKey.SkillLevelReq)))
			{
				if (Dictionaries.SkillInfo.ContainsKey(wo.Values(LongValueKey.ActivationReqSkillId)))
					sb.Append(", " + Dictionaries.SkillInfo[wo.Values(LongValueKey.ActivationReqSkillId)] + " " + wo.Values(LongValueKey.SkillLevelReq) + " to Activate");
				else
					sb.Append(", Unknown skill: " + wo.Values(LongValueKey.ActivationReqSkillId) + " " + wo.Values(LongValueKey.SkillLevelReq) + " to Activate");
			}

			// Summoning Gem
			if (wo.Values((LongValueKey)366) > 0 && wo.Values((LongValueKey)367) > 0)
			{
				if (Dictionaries.SkillInfo.ContainsKey(wo.Values((LongValueKey)366)))
					sb.Append(", " + Dictionaries.SkillInfo[wo.Values((LongValueKey)366)] + " " + wo.Values((LongValueKey)367));
				else
					sb.Append(", Unknown skill: " + wo.Values((LongValueKey)366) + " " + wo.Values((LongValueKey)367));
			}

			// Summoning Gem
			if (wo.Values((LongValueKey)368) > 0 && wo.Values((LongValueKey)367) > 0)
			{
				if (Dictionaries.SkillInfo.ContainsKey(wo.Values((LongValueKey)368)))
					sb.Append(", Spec " + Dictionaries.SkillInfo[wo.Values((LongValueKey)368)] + " " + wo.Values((LongValueKey)367));
				else
					sb.Append(", Unknown skill spec: " + wo.Values((LongValueKey)368) + " " + wo.Values((LongValueKey)367));
			}

			if (wo.Values(LongValueKey.LoreRequirement) > 0)
				sb.Append(", Diff " + wo.Values(LongValueKey.LoreRequirement));

			if (wo.ObjectClass == ObjectClass.Salvage)
			{
				if (wo.Values(DoubleValueKey.SalvageWorkmanship) > 0)
					sb.Append(", Work " + wo.Values(DoubleValueKey.SalvageWorkmanship).ToString("N2"));
			}
			else
			{
				if (wo.Values(LongValueKey.Workmanship) > 0 && wo.Values(LongValueKey.NumberTimesTinkered) != 10) // Don't show the work if its already 10 tinked.
					sb.Append(", Craft " + wo.Values(LongValueKey.Workmanship));
			}

			if (wo.ObjectClass == ObjectClass.Armor && wo.Values(LongValueKey.Unenchantable, 0) != 0)
			{
				sb.Append(", [" +
					wo.Values(DoubleValueKey.SlashProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.PierceProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.BludgeonProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.ColdProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.FireProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.AcidProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.LightningProt).ToString("N1") + "]");
			}

			if (Settings.SettingsManager.ItemInfoOnIdent.ShowValueAndBurden.Value)
			{
				if (wo.Values(LongValueKey.Value) > 0)
					sb.Append(", Value " + String.Format("{0:n0}", wo.Values(LongValueKey.Value)));

				if (wo.Values(LongValueKey.Burden) > 0)
					sb.Append(", BU " + wo.Values(LongValueKey.Burden));
			}

			if (mwo.TotalRating > 0)
			{
				sb.Append(", [");
				bool first = true;
				if (mwo.DamRating > 0) { sb.Append("D " + mwo.DamRating); first = false; }
				if (mwo.DamResistRating > 0) { if (!first) sb.Append(", "); sb.Append("DR " + mwo.DamResistRating); first = false; }
				if (mwo.CritRating > 0) { if (!first) sb.Append(", "); sb.Append("C " + mwo.CritRating); first = false; }
				if (mwo.CritDamRating > 0) { if (!first) sb.Append(", "); sb.Append("CD " + mwo.CritDamRating); first = false; }
				if (mwo.CritResistRating > 0) { if (!first) sb.Append(", "); sb.Append("CR " + mwo.CritResistRating); first = false; }
				if (mwo.CritDamResistRating > 0) { if (!first) sb.Append(", "); sb.Append("CDR " + mwo.CritDamResistRating); first = false; }
				if (mwo.HealBoostRating > 0) { if (!first) sb.Append(", "); sb.Append("HB " + mwo.HealBoostRating); first = false; }
				if (mwo.VitalityRating > 0) { if (!first) sb.Append(", "); sb.Append("V " + mwo.VitalityRating); first = false; }
				sb.Append("]");
			}

			if (wo.ObjectClass == ObjectClass.Misc && wo.Name.Contains("Keyring"))
				sb.Append(", Keys: " + wo.Values(LongValueKey.KeysHeld) + ", Uses: " + wo.Values(LongValueKey.UsesRemaining));

			return sb.ToString();
		}
	}
}
