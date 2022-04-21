using System;
using System.Collections.Generic;
using System.Text;
using pdftron;
using pdftron.Common;
using pdftron.PDF;
using pdftron.SDF;
using pdftron.PDF.Annots;

namespace FinalProject
{
    public class ManagePdf
    {
        // The Write functionality can go into this class too.

        public void ReadDocument(PDFDoc file, Character character)
        {
            FieldIterator itr;
            for (itr = file.GetFieldIterator(); itr.HasNext(); itr.Next())
            {
                
                Field field = itr.Current();
                GetHeader(file, character, field);
                GetAbilities(file, character, field);
                GetPersonalityAndTraits(file, character, field);
                GetAttacksEquipmentsGoldSpells(file, character, field);
                GetHealthArmorSpeedInitiativeDice(file, character, field);
                break;
            }
        }

        void GetHeader(PDFDoc file, Character character, Field field)
        {
            field = file.GetField("CharacterName");
            character.CharacterName = field.GetValueAsString();
            field = file.GetField("CharacterLevel");
            character.CharacterLevel = int.Parse(field.GetValueAsString());
            field = file.GetField("CharacterAlignment");
            character.CharacterAlignment = field.GetValueAsString();
            field = file.GetField("CharacterClass");
            character.Class = field.GetValueAsString();
            field = file.GetField("CharacterRace");
            character.Race = field.GetValueAsString();
            field = file.GetField("CharacterBackground");
            character.CharacterBackground = field.GetValueAsString();
            field = file.GetField("PlayerName");
            character.PlayerName = field.GetValueAsString();
        }

        void GetAbilities(PDFDoc file, Character character, Field field)
        {
            field = file.GetField("StrScore");
            character.StrengthScore = int.Parse(field.GetValueAsString());
            field = file.GetField("StrMod");
            character.StrengthModifier = int.Parse(field.GetValueAsString());
            field = file.GetField("DexScore");
            character.DexterityScore = int.Parse(field.GetValueAsString());
            field = file.GetField("DexMod");
            character.DexterityModifier = int.Parse(field.GetValueAsString());
            field = file.GetField("ConstScore");
            character.ConstitutionScore = int.Parse(field.GetValueAsString());
            field = file.GetField("ConstModifier");
            character.ConstitutionModifier = int.Parse(field.GetValueAsString());
            field = file.GetField("IntScore");
            character.InteligenceScore= int.Parse(field.GetValueAsString());
            field = file.GetField("IntMod");
            character.InteligenceModifier = int.Parse(field.GetValueAsString());
            field = file.GetField("WisScore");
            character.WisdomScore = int.Parse(field.GetValueAsString());
            field = file.GetField("WisMod");
            character.WisdomModifier = int.Parse(field.GetValueAsString());
            field = file.GetField("CharScore");
            character.CharismaScore = int.Parse(field.GetValueAsString());
            field = file.GetField("CharMode");
            character.CharismaModifier = int.Parse(field.GetValueAsString());
            field = file.GetField("PassiveWisdom");
            character.PassiveWisdom = int.Parse(field.GetValueAsString());
            field = file.GetField("ProficiencyBonus");
            character.ProficiencyBonus = int.Parse(field.GetValueAsString());
            field = file.GetField("InspirationBonus");
            character.Inspiration = int.Parse(field.GetValueAsString());
        }

        void GetPersonalityAndTraits(PDFDoc file, Character character, Field field)
        {
            field = file.GetField("PersonalityTraits");
            character.Traits = field.GetValueAsString();
            field = file.GetField("Bonds");
            character.Bonds = field.GetValueAsString();
            field = file.GetField("Flaws");
            character.Flaws= field.GetValueAsString();
            field = file.GetField("Ideals");
            character.Ideals = field.GetValueAsString();
            field = file.GetField("Proficiencies&Languages");
            character.ProficienciesLanguages = field.GetValueAsString();
            field = file.GetField("FeaturesTraits");
            character.FeaturesTraits = field.GetValueAsString();
            
        }

        void GetAttacksEquipmentsGoldSpells(PDFDoc file, Character character, Field field)
        {
            field = file.GetField("Attack#1");
            character.Attack = field.GetValueAsString();
            field = file.GetField("Attack#2");
            character.Attack2 = field.GetValueAsString();
            field = file.GetField("Attack#3");
            character.Attack3 = field.GetValueAsString();
            field = file.GetField("Equipment#1");
            character.Equipment1 = field.GetValueAsString();
            field = file.GetField("Equipment#2");
            character.Equipment2 = field.GetValueAsString();
            field = file.GetField("Spells");
            character.Spells = field.GetValueAsString();
            field = file.GetField("Copper");
            character.Copper = int.Parse(field.GetValueAsString());
            field = file.GetField("Silver");
            character.Silver = int.Parse(field.GetValueAsString());
            field = file.GetField("Electrum");
            character.Electrum = int.Parse(field.GetValueAsString());
            field = file.GetField("Gold");
            character.Gold = int.Parse(field.GetValueAsString());
            field = file.GetField("Platinum");
            character.Platinum = int.Parse(field.GetValueAsString());
        }

        void GetHealthArmorSpeedInitiativeDice(PDFDoc file, Character character, Field field)
        {
            field = file.GetField("ArmorClass");
            character.ArmorClass = int.Parse(field.GetValueAsString());
            field = file.GetField("Initiative");
            character.Initiative = int.Parse(field.GetValueAsString());
            field = file.GetField("Speed");
            character.CharacterSpeed = int.Parse(field.GetValueAsString());
            field = file.GetField("MaximumHitPoint");
            character.CharacterMaxHP = int.Parse(field.GetValueAsString());
            field = file.GetField("CurrentHitPoint");
            character.CharacterHP = int.Parse(field.GetValueAsString());
            field = file.GetField("TotalHitDice");
            character.HitDice = int.Parse(field.GetValueAsString());
            field = file.GetField("RecoveryDice");
            character.DiceRecovery = field.GetValueAsString();
        }
    }
}
