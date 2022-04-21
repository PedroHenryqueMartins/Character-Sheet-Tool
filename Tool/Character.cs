using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FinalProject
{
    public class Character : INotifyPropertyChanged
    {
        #region Races&Classes

        public enum RaceType
        {
            Dragonborn,
            Dwarf,
            Elf,
            Gnome,
            HalfElf,
            Halfling,
            HalfOrc,
            Human,
            Tiefling
        }


        public enum Classes
        {
            Barbarian,
            Bard,
            Cleric,
            Druid,
            Fighter,
            Monk,
            Paladin,
            Ranger,
            Rogue,
            Sorcerer,
            Warlock,
            Wizard
        }

       

        #endregion

        #region CharacterHeader
        private string _CharacterName;

        public string CharacterName
        {
            get { return _CharacterName; }
            set
            {
                _CharacterName = value;
                NotifyPropertyChanged("CharacterName");
            }
        }

        private string _CharacterBackground;

        public string CharacterBackground
        {
            get { return _CharacterBackground; }
            set { _CharacterBackground = value;
                NotifyPropertyChanged("CharacterBackground");
            }
        }

        private string _characterAlignment;

        public string CharacterAlignment
        {
            get { return _characterAlignment; }
            set { _characterAlignment = value;
                NotifyPropertyChanged("CharacterAlignment");
            }
        }

        private string _playerName;

        public string PlayerName
        {
            get { return _playerName; }
            set { _playerName = value;
                NotifyPropertyChanged("PlayerName");
            }
        }


        private int _characterLevel;

        public int CharacterLevel
        {
            get { return _characterLevel; }
            set { _characterLevel = value;
                _hitDice = _characterLevel;
                if (_characterLevel >= 1 && _characterLevel <= 4)
                {
                    _proficiencyBonus = 2;
                    NotifyPropertyChanged("HitDice");
                    NotifyPropertyChanged("CharacterLevel");
                    NotifyPropertyChanged("ProficiencyBonus");
                }
                if (_characterLevel >= 5 && _characterLevel <= 8)
                {
                    _proficiencyBonus = 3;
                    NotifyPropertyChanged("HitDice");
                    NotifyPropertyChanged("CharacterLevel");
                    NotifyPropertyChanged("ProficiencyBonus");
                }
                if (_characterLevel >= 9 && _characterLevel <= 12)
                {
                    _proficiencyBonus = 4;
                    NotifyPropertyChanged("HitDice");
                    NotifyPropertyChanged("CharacterLevel");
                    NotifyPropertyChanged("ProficiencyBonus");
                }
                if (_characterLevel >= 13 && _characterLevel <= 16)
                {
                    _proficiencyBonus = 5;
                    NotifyPropertyChanged("HitDice");
                    NotifyPropertyChanged("CharacterLevel");
                    NotifyPropertyChanged("ProficiencyBonus");
                }
                if (_characterLevel >= 17 && _characterLevel < 20)
                {
                    _proficiencyBonus = 6;
                    NotifyPropertyChanged("HitDice");
                    NotifyPropertyChanged("CharacterLevel");
                    NotifyPropertyChanged("ProficiencyBonus");
                }
            }
        }
        #endregion

        #region Proficiency&Inspiration

        private int _proficiencyBonus;

        public int ProficiencyBonus
        {
            get { return _proficiencyBonus; }
            set 
            { 
                _proficiencyBonus = value;
                _passiveWisdom = 10 + _proficiencyBonus + WisdomModifier;
                NotifyPropertyChanged("PassiveWisdom");
                NotifyPropertyChanged("ProficiencyBonus");
            }
        }

        private int _inspiration;

        public int Inspiration
        {
            get { return _inspiration; }
            set 
            { 
                _inspiration = value;
                NotifyPropertyChanged("Inspiration");
            }
        }
        #endregion

        #region AbilitiesValues

        private int _strengthScore;

        public int StrengthScore
        {
            get { return _strengthScore; }
            set 
            { 
                _strengthScore = value;
                double x = (_strengthScore - 10.1) / 2;
                double y = Math.Round(x);
                _strengthMod = (int)y;

                NotifyPropertyChanged("StrengthModifier");
                NotifyPropertyChanged("StrengthScore");
            }
        }

        private int _strengthMod;

        public int StrengthModifier
        {
            get { return _strengthMod; }
            set 
            { 
                _strengthMod = value;
                NotifyPropertyChanged("StrengthModifier");
            }
        }


        private int _dexterityScore;

        public int DexterityScore
        {
            get { return _dexterityScore; }
            set 
            { 
                _dexterityScore = value;
                double x = (_dexterityScore - 10.1) / 2;
                double y = Math.Round(x);
                _dexterityMod = (int)y;
                _armorClass = 10 + _dexterityMod;
                _initiativeValue = _dexterityMod;
                NotifyPropertyChanged("Initiative");
                NotifyPropertyChanged("ArmorClass");
                NotifyPropertyChanged("DexterityModifier");
                NotifyPropertyChanged("DexterityScore");
            }
        }

        private int _dexterityMod;

        public int DexterityModifier
        {
            get { return _dexterityMod; }
            set { _dexterityMod = value;
                NotifyPropertyChanged("DexterityModifier");
            }
        }


        private int _constitutionScore;
        public int ConstitutionScore
        {
            get { return _constitutionScore; }
            set { _constitutionScore = value;
                double x = (_constitutionScore - 10.1) / 2;
                double y = Math.Round(x);
                _constitutionMod = (int)y;

                NotifyPropertyChanged("ConstitutionModifier");
                NotifyPropertyChanged("ConstitutionScore");
            }
        }

        private int _constitutionMod;

        public int ConstitutionModifier
        {
            get { return _constitutionMod; }
            set { _constitutionMod = value;
                NotifyPropertyChanged("ConstitutionModifier");
            }
        }

        private int _inteligenceScore;

        public int InteligenceScore
        {
            get { return _inteligenceScore; }
            set { _inteligenceScore = value;
                double x = (_inteligenceScore - 10.1) / 2;
                double y = Math.Round(x);
                _inteligenceMod = (int)y;

                NotifyPropertyChanged("InteligenceModifier");
                NotifyPropertyChanged("InteligenceScore");
            }
        }

        private int _inteligenceMod;

        public int InteligenceModifier
        {
            get { return _inteligenceMod; }
            set { _inteligenceMod = value;
                NotifyPropertyChanged("InteligenceModifier");
            }
        }

        private int _wisdomScore;

        public int WisdomScore
        {
            get { return _wisdomScore; }
            set { _wisdomScore = value;
                double x = (_wisdomScore - 10.1) / 2;
                double y = Math.Round(x);
                _wisdomMod = (int)y;
                _passiveWisdom = 10 + _proficiencyBonus + WisdomModifier;
                NotifyPropertyChanged("PassiveWisdom");
                NotifyPropertyChanged("WisdomModifier");
                NotifyPropertyChanged("WisdomScore");
            }
        }

        private int _wisdomMod;

        public int WisdomModifier
        {
            get { return _wisdomMod; }
            set { _wisdomMod = value;
                NotifyPropertyChanged("WisdomModifier");
            }
        }

        private int _charismaScore;

        public int CharismaScore
        {
            get { return _charismaScore; }
            set { _charismaScore = value;
                double x = (_charismaScore - 10.1) / 2;
                double y = Math.Round(x);
                _charismaMod = (int)y;

                NotifyPropertyChanged("CharismaModifier");
                NotifyPropertyChanged("CharismaScore");
            }
        }

        private int _charismaMod;

        public int CharismaModifier
        {
            get { return _charismaMod; }
            set { _charismaMod = value;
                NotifyPropertyChanged("CharismaModifier");

            }
        }

        #endregion

        #region CharacterSkills

        private string _proficiencieslanguages;

        public string ProficienciesLanguages
        {
            get { return _proficiencieslanguages; }
            set { _proficiencieslanguages = value;
                NotifyPropertyChanged("ProficienciesLanguages");
            }
        }


        private string _featuresTraits;

        public string FeaturesTraits
        {
            get { return _featuresTraits; }
            set
            {
                _featuresTraits = value;
                NotifyPropertyChanged("FeaturesTraits");
            }
        }

        private int _passiveWisdom;

        public int PassiveWisdom
        {
            get { return _passiveWisdom; }
            set { _passiveWisdom = value;
                NotifyPropertyChanged("PassiveWisdom");
            }
        }

        private int _armorClass;

        public int ArmorClass
        {
            get { return _armorClass; }
            set { _armorClass = value;
                NotifyPropertyChanged("ArmorClass");
            }
        }

        private int _initiativeValue;

        public int Initiative
        {
            get { return _initiativeValue; }
            set { _initiativeValue = value;
                NotifyPropertyChanged("Initiative");
            }
        }

        private string race;

        public string Race
        {
            get { return race; }
            set
            {
                race = value;
                if (race.ToLower() == "dragonborn" || race.ToLower() == "elf" || race.ToLower() == "halfelf" || race.ToLower() == "halforc" || race.ToLower() == "human" || race.ToLower() == "tiefling")
                {
                    _speed = 30;
                    NotifyPropertyChanged("Race");
                    NotifyPropertyChanged("CharacterSpeed");
                }
                if (race.ToLower() == "dwarf" || race.ToLower() == "gnome"|| race.ToLower() == "halfling")
                {
                    _speed = 25;
                    NotifyPropertyChanged("Race");
                    NotifyPropertyChanged("CharacterSpeed");
                }

            }
        }

        private string classes;

        public string Class
        {
            get { return classes; }
            set { classes = value;
                if (classes.ToLower() == "barbarian")
                {
                    _diceRecovery = "d12";
                    NotifyPropertyChanged("DiceRecovery");
                    NotifyPropertyChanged("Class");
                }
                if (classes.ToLower() == "fighter" || classes.ToLower() == "paladin")
                {
                    _diceRecovery = "d10";
                    NotifyPropertyChanged("DiceRecovery");
                    NotifyPropertyChanged("Class");
                }
                if (classes.ToLower() == "bard"|| classes.ToLower() == "cleric" || classes.ToLower() == "druid" || classes.ToLower() == "monk" || classes.ToLower() == "rogue" || classes.ToLower() == "warlock")
                {
                    _diceRecovery = "d8";
                    NotifyPropertyChanged("DiceRecovery");
                    NotifyPropertyChanged("Class");
                }
                if (classes.ToLower() == "sorcerer" || classes.ToLower() == "wizard")
                {
                    _diceRecovery = "d6";
                    NotifyPropertyChanged("DiceRecovery");
                    NotifyPropertyChanged("Class");
                }
            }
        }


        private int _speed;

        public int CharacterSpeed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                NotifyPropertyChanged("CharacterSpeed");
            }
        }

        private int _characterHP;

        public int CharacterHP
        {
            get { return _characterHP; }
            set
            {
                _characterHP = value;
                NotifyPropertyChanged("CharacterHP");
            }
        }

        private int _characterMaxHP;

        public int CharacterMaxHP
        {
            get { return _characterMaxHP; }
            set
            {
                _characterMaxHP = value;
                _characterHP = _characterMaxHP;
                NotifyPropertyChanged("CharacterHP");
                NotifyPropertyChanged("CharacterMaxHP");
            }
        }

        private int _hitDice;

        public int HitDice
        {
            get { return _hitDice; }
            set { _hitDice = value;
                NotifyPropertyChanged("HitDice");
            }
        }

        private string _diceRecovery;

        public string DiceRecovery
        {
            get { return _diceRecovery; }
            set { _diceRecovery = value;
                NotifyPropertyChanged("DiceRecovery");
            }
        }


        #endregion

        #region Traits, bonds, ideals & flaws

        private string _traits;

        public string Traits
        {
            get { return _traits; }
            set { _traits = value;
                NotifyPropertyChanged("Traits");
            }
        }

        private string _bonds;

        public string Bonds
        {
            get { return _bonds; }
            set { _bonds = value;
                NotifyPropertyChanged("Bonds");
            }
        }

        private string _ideals;

        public string Ideals
        {
            get { return _ideals; }
            set { _ideals = value;
                NotifyPropertyChanged("Ideals");
            }
        }

        private string _flaws;

        public string Flaws
        {
            get { return _flaws; }
            set { _flaws = value;
                NotifyPropertyChanged("Flaws");
            }
        }


        #endregion

        #region Attacks & Spellcasting

        private string _Attack;

        public string Attack
        {
            get { return _Attack; }
            set { _Attack = value;
                NotifyPropertyChanged("Attack");
            }
        }

        private string _attack2;

        public string Attack2
        {
            get { return _attack2; }
            set { _attack2 = value;
                NotifyPropertyChanged("Attack2");
            }
        }

        private string _attack3;

        public string Attack3
        {
            get { return _attack3; }
            set { _attack3 = value;
                NotifyPropertyChanged("Attack3");
            }
        }

        private string spells;

        public string Spells
        {
            get { return spells; }
            set { spells = value;
                NotifyPropertyChanged("Spells");
            }
        }


        #endregion

        #region Gold&Equipment

        private int _copper;

        public int Copper
        {
            get { return _copper; }
            set { _copper = value;
                NotifyPropertyChanged("Copper");
            }
        }

        private int _silver;

        public int Silver
        {
            get { return _silver; }
            set { _silver = value;
                NotifyPropertyChanged("Silver");
            }
        }

        private int _electrum;

        public int Electrum
        {
            get { return _electrum; }
            set { _electrum = value;
                NotifyPropertyChanged("Electrum");
            }
        }

        private int _gold;

        public int Gold
        {
            get { return _gold; }
            set { _gold = value;
                NotifyPropertyChanged("Gold");
            }
        }

        private int _platinum;

        public int Platinum
        {
            get { return _platinum; }
            set { _platinum = value;
                NotifyPropertyChanged("Platinum");
            }
        }

        private string _equipment1;

        public string Equipment1
        {
            get { return _equipment1; }
            set { _equipment1 = value;
                NotifyPropertyChanged("Equipment1");
            }
        }

        private string _equipment2;

        public string Equipment2
        {
            get { return _equipment2; }
            set { _equipment2 = value;
                NotifyPropertyChanged("Equipment2");
            }
        }


        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
