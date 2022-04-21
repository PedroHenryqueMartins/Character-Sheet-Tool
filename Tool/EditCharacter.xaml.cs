using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using pdftron;
using pdftron.Common;
using pdftron.PDF;
using pdftron.SDF;
using pdftron.PDF.Annots;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Image = pdftron.PDF.Image;

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for EditCharacter.xaml
    /// </summary>
    public partial class EditCharacter : Window
    {
        Character character;
        PDFDoc doc;
        pdftron.PDF.Page page;

        public EditCharacter(Character characterSheet, PDFDoc file)
        {
            InitializeComponent();
            character = characterSheet;
            DataContext = character;
            doc = file;
        }

        private void SavePressed(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "PDF File (*.pdf) | *.pdf";
            if (saveFile.ShowDialog() == true)
            {

                SetUp();
                try
                {
                    doc.Save(saveFile.FileName, SDFDoc.SaveOptions.e_incremental);
                    string path = System.IO.Path.GetFullPath(saveFile.FileName);
                    Process.Start("explorer.exe", path);
                }
                catch
                {
                    MessageBox.Show("Please, close the file before overwritting it.");
                }
            }

        }

        private void ExitPressed(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "CharacterSheet", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    MessageBox.Show("Good Bye");
                    Close();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Don't forget to save before exiting.");
                    break;
                default:
                    break;
            }


        }

        // LC: this funciton and all the individual functions should.
        void SetUp()
        {
            CreatePage();
            CharacterHeader();
            SetAbilitiesValues();
            SetProficiencyInspiration();
            SetSavingThrows();
            SetSkills();
            SetStats();
            SetPersonality();
            SetFeaturesTraits();
            SetEquipment();
            SetOtherProficienciesLanguages();
        }

        void CreatePage()
        {
            page = doc.PageCreate();
            ElementBuilder eb = new ElementBuilder();
            ElementWriter ew = new ElementWriter();


            Element element;

            //  CharaterSheet blueprint
            ew.Begin(page);
            // LC: hard coded path is not user friendly, so the image will go missing when I save on my machine.
            string sheetpath = "D:/VGP/VGP 232/VGP232/";
            Image sheetImg = Image.Create(doc, sheetpath + "charactersheet.png");
            element = eb.CreateImage(sheetImg, 0, 0, 600, 800);

            ew.WriteElement(element);
            ew.End();
            eb.Reset();
        }
        void CharacterHeader()
        {
            // LC: I look at this method, and think, it should be a static helper function that exist in the merge pdf class because the only 
            // i.e. this can be a method call CreateCharacterPDFHeaders(Charachter character);
            try
            {
                Field fieldCharacterName = doc.FieldCreate("CharacterName", Field.Type.e_text, doc.CreateIndirectString(character.CharacterName));
                Field fieldClass = doc.FieldCreate("CharacterClass", Field.Type.e_text, doc.CreateIndirectString(character.Race));
                Field fieldRace = doc.FieldCreate("CharacterRace", Field.Type.e_text, doc.CreateIndirectString(character.Class));
                Field fieldLevel = doc.FieldCreate("CharacterLevel", Field.Type.e_text, doc.CreateIndirectString(character.CharacterLevel.ToString()));
                Field fieldBackground = doc.FieldCreate("CharacterBackground", Field.Type.e_text, doc.CreateIndirectString(character.CharacterBackground));
                Field fieldAlignment = doc.FieldCreate("CharacterAlignment", Field.Type.e_text, doc.CreateIndirectString(character.CharacterAlignment));
                Field fieldPlayerName = doc.FieldCreate("PlayerName", Field.Type.e_text, doc.CreateIndirectString(character.PlayerName));

                // LC: a lot of magic numbers, should probaly store that in a constant somewhere.
                TextWidget characterNameText = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 715, 180, 735), fieldCharacterName);
                TextWidget characterClassText = TextWidget.Create(doc, new pdftron.PDF.Rect(265, 710, 340, 725), fieldClass);
                TextWidget characterRaceText = TextWidget.Create(doc, new pdftron.PDF.Rect(265, 735, 335, 755), fieldRace);
                TextWidget characterLevelText = TextWidget.Create(doc, new pdftron.PDF.Rect(340, 735, 355, 755), fieldLevel);
                TextWidget characterBackgroundText = TextWidget.Create(doc, new pdftron.PDF.Rect(375, 735, 435, 755), fieldBackground);
                TextWidget characterAlignmentText = TextWidget.Create(doc, new pdftron.PDF.Rect(375, 710, 465, 725), fieldAlignment);
                TextWidget playerNameText = TextWidget.Create(doc, new pdftron.PDF.Rect(470, 735, 550, 755), fieldPlayerName);


                characterNameText.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                characterClassText.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                characterRaceText.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                characterLevelText.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                characterBackgroundText.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                characterAlignmentText.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                playerNameText.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));

                characterClassText.SetFontSize(14);
                characterNameText.SetFontSize(14);
                characterRaceText.SetFontSize(14);
                characterLevelText.SetFontSize(14);
                characterBackgroundText.SetFontSize(14);
                characterAlignmentText.SetFontSize(14);
                playerNameText.SetFontSize(14);
                characterNameText.RefreshAppearance();
                characterClassText.RefreshAppearance();
                characterRaceText.RefreshAppearance();
                characterLevelText.RefreshAppearance();
                characterBackgroundText.RefreshAppearance();
                characterAlignmentText.RefreshAppearance();
                playerNameText.RefreshAppearance();

                page.AnnotPushBack(characterNameText);
                page.AnnotPushBack(characterClassText);
                page.AnnotPushBack(characterRaceText);
                page.AnnotPushBack(characterLevelText);
                page.AnnotPushBack(characterBackgroundText);
                page.AnnotPushBack(characterAlignmentText);
                page.AnnotPushBack(playerNameText);

                doc.PagePushBack(page);
            }
            catch
            {
                MessageBox.Show("Please, do not leave the header blank.");
            }


        }

        void SetAbilitiesValues()
        {
            //  Str Score
            Field fieldStrScore = doc.FieldCreate("StrScore", Field.Type.e_text, doc.CreateIndirectString(character.StrengthScore.ToString()));
            TextWidget strScore = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 625, 65, 645), fieldStrScore);
            strScore.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            strScore.SetFontSize(14);
            strScore.RefreshAppearance();
            page.AnnotPushBack(strScore);
            //  Str Modifier
            Field fieldStrMod = doc.FieldCreate("StrMod", Field.Type.e_text, doc.CreateIndirectString(character.StrengthModifier.ToString()));
            TextWidget strMod = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 597, 65, 612), fieldStrMod);
            strMod.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            strMod.SetFontSize(14);
            strMod.RefreshAppearance();
            page.AnnotPushBack(strMod);

            //  Dex Score
            Field fieldDexScore = doc.FieldCreate("DexScore", Field.Type.e_text, doc.CreateIndirectString(character.DexterityScore.ToString()));
            TextWidget dexScore = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 552, 65, 572), fieldDexScore);
            dexScore.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            dexScore.SetFontSize(14);
            dexScore.RefreshAppearance();
            page.AnnotPushBack(dexScore);
            //  Dex Modifier
            Field fieldDexMod = doc.FieldCreate("DexMod", Field.Type.e_text, doc.CreateIndirectString(character.DexterityModifier.ToString()));
            TextWidget dexMod = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 524, 65, 539), fieldDexMod);
            dexMod.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            dexMod.SetFontSize(14);
            dexMod.RefreshAppearance();
            page.AnnotPushBack(dexMod);

            //  Const Score
            Field fieldConstScore = doc.FieldCreate("ConstScore", Field.Type.e_text, doc.CreateIndirectString(character.ConstitutionScore.ToString()));
            TextWidget constScore = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 479, 65, 499), fieldConstScore);
            constScore.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            constScore.SetFontSize(14);
            constScore.RefreshAppearance();
            page.AnnotPushBack(constScore);
            //  Const  Modifier
            Field fieldConstMod = doc.FieldCreate("ConstModifier", Field.Type.e_text, doc.CreateIndirectString(character.ConstitutionModifier.ToString()));
            TextWidget constMod = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 451, 65, 466), fieldConstMod);
            constMod.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            constMod.SetFontSize(14);
            constMod.RefreshAppearance();
            page.AnnotPushBack(constMod);

            // Int Score
            Field fieldIntScore = doc.FieldCreate("IntScore", Field.Type.e_text, doc.CreateIndirectString(character.InteligenceScore.ToString()));
            TextWidget intScore = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 408, 65, 428), fieldIntScore);
            intScore.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            intScore.SetFontSize(14);
            intScore.RefreshAppearance();
            page.AnnotPushBack(intScore);
            // Int Modifier
            Field fieldIntMod = doc.FieldCreate("IntMod", Field.Type.e_text, doc.CreateIndirectString(character.InteligenceModifier.ToString()));
            TextWidget intMod = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 379, 65, 394), fieldIntMod);
            intMod.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            intMod.SetFontSize(14);
            intMod.RefreshAppearance();
            page.AnnotPushBack(intMod);

            // Wis Score
            Field fieldWisScore = doc.FieldCreate("WisScore", Field.Type.e_text, doc.CreateIndirectString(character.WisdomScore.ToString()));
            TextWidget wisScore = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 334, 65, 351), fieldWisScore);
            wisScore.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            wisScore.SetFontSize(14);
            wisScore.RefreshAppearance();
            page.AnnotPushBack(wisScore);
            // Wis Modifier
            Field fieldWisMod = doc.FieldCreate("WisMod", Field.Type.e_text, doc.CreateIndirectString(character.WisdomModifier.ToString()));
            TextWidget wisMod = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 306, 65, 321), fieldWisMod);
            wisMod.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            wisMod.SetFontSize(14);
            wisMod.RefreshAppearance();
            page.AnnotPushBack(wisMod);

            // Char Score
            Field fieldCharScore = doc.FieldCreate("CharScore", Field.Type.e_text, doc.CreateIndirectString(character.CharismaScore.ToString()));
            TextWidget charScore = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 262, 65, 282), fieldCharScore);
            charScore.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            charScore.SetFontSize(14);
            charScore.RefreshAppearance();
            page.AnnotPushBack(charScore);
            //  Char Mod
            Field fieldCharMod = doc.FieldCreate("CharMode", Field.Type.e_text, doc.CreateIndirectString(character.CharismaModifier.ToString()));
            TextWidget charMod = TextWidget.Create(doc, new pdftron.PDF.Rect(45, 234, 65, 249), fieldCharMod);
            charMod.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            charMod.SetFontSize(14);
            charMod.RefreshAppearance();
            page.AnnotPushBack(charMod);

            //  Passive Wisdom
            Field fieldPW = doc.FieldCreate("PassiveWisdom", Field.Type.e_text, doc.CreateIndirectString(character.PassiveWisdom.ToString()));
            TextWidget pwtext = TextWidget.Create(doc, new pdftron.PDF.Rect(32, 187, 52, 202), fieldPW);
            pwtext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            pwtext.SetFontSize(14);
            pwtext.RefreshAppearance();
            page.AnnotPushBack(pwtext);

        }
        void SetProficiencyInspiration()
        {
            //  Proficiency
            Field fieldProficiency = doc.FieldCreate("ProficiencyBonus", Field.Type.e_text, doc.CreateIndirectString(character.ProficiencyBonus.ToString()));
            TextWidget proficiency = TextWidget.Create(doc, new pdftron.PDF.Rect(98, 650, 113, 665), fieldProficiency);
            proficiency.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            proficiency.SetFontSize(14);
            proficiency.RefreshAppearance();
            page.AnnotPushBack(proficiency);

            //  Inspiration
            Field fieldInspiration = doc.FieldCreate("InspirationBonus", Field.Type.e_text, doc.CreateIndirectString(character.Inspiration.ToString()));
            TextWidget inspiration = TextWidget.Create(doc, new pdftron.PDF.Rect(98, 615, 113, 630), fieldInspiration);
            inspiration.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            inspiration.SetFontSize(14);
            inspiration.RefreshAppearance();
            page.AnnotPushBack(inspiration);
        }

        void SetSavingThrows()
        {
            //  Str Saving Throw
            CheckBoxWidget strCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 583, 107, 593));
            Field strvalue = doc.FieldCreate("StrCheck", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget strtext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 583, 123, 593), strvalue);
            strCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            strtext.SetFontSize(8);
            if (strSavingThrow.IsChecked == true)
            {
                strCheck.SetChecked(true);
                strtext.SetText(character.StrengthModifier.ToString());
            }
            strCheck.RefreshAppearance();
            strtext.RefreshAppearance();
            page.AnnotPushBack(strtext);
            page.AnnotPushBack(strCheck);

            //  Dex Saving Throw
            CheckBoxWidget dexCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 569, 107, 579));
            Field dexvalue = doc.FieldCreate("DexCheck", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget dextext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 569, 123, 579), dexvalue);
            dexCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            dextext.SetFontSize(8);
            if (dexSavingThrow.IsChecked == true)
            {
                dexCheck.SetChecked(true);
                dextext.SetText(character.DexterityModifier.ToString());
            }
            dexCheck.RefreshAppearance();
            dextext.RefreshAppearance();
            page.AnnotPushBack(dextext);
            page.AnnotPushBack(dexCheck);

            //  Const Saving Throw
            CheckBoxWidget constCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 555, 107, 565));
            Field constvalue = doc.FieldCreate("ConstCheck", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget consttext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 555, 123, 565), constvalue);
            constCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            consttext.SetFontSize(8);
            if (constSavingThrow.IsChecked == true)
            {
                constCheck.SetChecked(true);
                consttext.SetText(character.ConstitutionModifier.ToString());
            }
            constCheck.RefreshAppearance();
            consttext.RefreshAppearance();
            page.AnnotPushBack(consttext);
            page.AnnotPushBack(constCheck);

            //  Int Saving Throw
            CheckBoxWidget intCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 541, 107, 551));
            Field intvalue = doc.FieldCreate("IntCheck", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget inttext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 541, 123, 551), intvalue);
            intCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            inttext.SetFontSize(8);
            if (intSavingThrow.IsChecked == true)
            {
                intCheck.SetChecked(true);
                inttext.SetText(character.InteligenceModifier.ToString());
            }
            intCheck.RefreshAppearance();
            inttext.RefreshAppearance();
            page.AnnotPushBack(inttext);
            page.AnnotPushBack(intCheck);

            //  Wis Saving Throw
            CheckBoxWidget wisCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 527, 107, 537));
            Field wisvalue = doc.FieldCreate("WisCheck", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget wistext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 527, 123, 537), wisvalue);
            wisCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            wistext.SetFontSize(8);
            if (wisSavingThrow.IsChecked == true)
            {
                wisCheck.SetChecked(true);
                wistext.SetText(character.WisdomModifier.ToString());
            }
            wisCheck.RefreshAppearance();
            wistext.RefreshAppearance();
            page.AnnotPushBack(wistext);
            page.AnnotPushBack(wisCheck);

            //  Char Saving Throw
            CheckBoxWidget charCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 513, 107, 523));
            Field charvalue = doc.FieldCreate("CharCheck", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget chartext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 513, 123, 523), charvalue);
            charCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            chartext.SetFontSize(8);
            if (charSavingThrow.IsChecked == true)
            {
                charCheck.SetChecked(true);
                chartext.SetText(character.CharismaModifier.ToString());
            }
            charCheck.RefreshAppearance();
            chartext.RefreshAppearance();
            page.AnnotPushBack(chartext);
            page.AnnotPushBack(charCheck);
        }

        void SetSkills()
        {
            //  Acrobatics
            CheckBoxWidget acroCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 465, 107, 475));
            Field acrovalue = doc.FieldCreate("Acrobatics", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget acrotext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 465, 123, 475), acrovalue);
            acroCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            acrotext.SetFontSize(8);
            if (cbAcrobatics.IsChecked == true)
            {
                acroCheck.SetChecked(true);
                acrotext.SetText((character.DexterityModifier + character.ProficiencyBonus).ToString());
            }
            acroCheck.RefreshAppearance();
            acrotext.RefreshAppearance();
            page.AnnotPushBack(acrotext);
            page.AnnotPushBack(acroCheck);

            //  Animal Handling
            CheckBoxWidget ahCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 451, 107, 461));
            Field ahvalue = doc.FieldCreate("Animal Handling", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget ahtext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 451, 123, 461), ahvalue);
            ahCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            ahtext.SetFontSize(8);
            if (cbAnimalHandling.IsChecked == true)
            {
                ahCheck.SetChecked(true);
                ahtext.SetText((character.WisdomModifier + character.ProficiencyBonus).ToString());
            }
            ahCheck.RefreshAppearance();
            ahtext.RefreshAppearance();
            page.AnnotPushBack(ahtext);
            page.AnnotPushBack(ahCheck);

            //  Arcana
            CheckBoxWidget arcanaCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 437, 107, 447));
            Field arcanavalue = doc.FieldCreate("Arcana", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget arcanatext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 437, 123, 447), arcanavalue);
            arcanaCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            arcanatext.SetFontSize(8);
            if (cbArcana.IsChecked == true)
            {
                arcanaCheck.SetChecked(true);
                arcanatext.SetText((character.InteligenceModifier + character.ProficiencyBonus).ToString());
            }
            arcanaCheck.RefreshAppearance();
            arcanatext.RefreshAppearance();
            page.AnnotPushBack(arcanatext);
            page.AnnotPushBack(arcanaCheck);

            //  Athletics
            CheckBoxWidget athleticsCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 424, 107, 434));
            Field athleticsvalue = doc.FieldCreate("Athletics", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget athleticstext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 424, 123, 434), athleticsvalue);
            athleticsCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            athleticstext.SetFontSize(8);
            if (cbAthletics.IsChecked == true)
            {
                athleticsCheck.SetChecked(true);
                athleticstext.SetText((character.StrengthModifier + character.ProficiencyBonus).ToString());
            }
            athleticsCheck.RefreshAppearance();
            athleticstext.RefreshAppearance();
            page.AnnotPushBack(athleticstext);
            page.AnnotPushBack(athleticsCheck);

            //  Deception
            CheckBoxWidget deceptionCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 410, 107, 420));
            Field deceptionvalue = doc.FieldCreate("Deception", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget deceptiontext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 410, 123, 420), deceptionvalue);
            deceptionCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            deceptiontext.SetFontSize(8);
            if (cbDeception.IsChecked == true)
            {
                deceptionCheck.SetChecked(true);
                deceptiontext.SetText((character.CharismaModifier + character.ProficiencyBonus).ToString());
            }
            deceptionCheck.RefreshAppearance();
            deceptiontext.RefreshAppearance();
            page.AnnotPushBack(deceptiontext);
            page.AnnotPushBack(deceptionCheck);

            //  History
            CheckBoxWidget historyCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 396, 107, 406));
            Field historyvalue = doc.FieldCreate("History", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget historytext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 396, 123, 406), historyvalue);
            historyCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            historytext.SetFontSize(8);
            if (cbHistory.IsChecked == true)
            {
                historyCheck.SetChecked(true);
                historytext.SetText((character.InteligenceModifier + character.ProficiencyBonus).ToString());
            }
            historyCheck.RefreshAppearance();
            historytext.RefreshAppearance();
            page.AnnotPushBack(historytext);
            page.AnnotPushBack(historyCheck);

            //  Insight
            CheckBoxWidget insightCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 382, 107, 392));
            Field insightvalue = doc.FieldCreate("Insight", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget insighttext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 382, 123, 392), insightvalue);
            insightCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            insighttext.SetFontSize(8);
            if (cbInsight.IsChecked == true)
            {
                insightCheck.SetChecked(true);
                insighttext.SetText((character.WisdomModifier + character.ProficiencyBonus).ToString());
            }
            insightCheck.RefreshAppearance();
            insighttext.RefreshAppearance();
            page.AnnotPushBack(insighttext);
            page.AnnotPushBack(insightCheck);

            //  Intimidation
            CheckBoxWidget intimidationCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 368, 107, 378));
            Field intimidationvalue = doc.FieldCreate("Intimidation", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget intimidationtext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 368, 123, 378), intimidationvalue);
            intimidationCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            intimidationtext.SetFontSize(8);
            if (cbIntimidation.IsChecked == true)
            {
                intimidationCheck.SetChecked(true);
                intimidationtext.SetText((character.CharismaModifier + character.ProficiencyBonus).ToString());
            }
            intimidationCheck.RefreshAppearance();
            intimidationtext.RefreshAppearance();
            page.AnnotPushBack(intimidationtext);
            page.AnnotPushBack(intimidationCheck);

            //  Investigation
            CheckBoxWidget investigationCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 355, 107, 365));
            Field investigationvalue = doc.FieldCreate("Investigation", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget investigaiontext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 355, 123, 365), investigationvalue);
            investigationCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            investigaiontext.SetFontSize(8);
            if (cbInvestigation.IsChecked == true)
            {
                investigationCheck.SetChecked(true);
                investigaiontext.SetText((character.InteligenceModifier + character.ProficiencyBonus).ToString());
            }
            investigationCheck.RefreshAppearance();
            investigaiontext.RefreshAppearance();
            page.AnnotPushBack(investigaiontext);
            page.AnnotPushBack(investigationCheck);

            //  Medicine
            CheckBoxWidget medicineCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 341, 107, 351));
            Field medicinevalue = doc.FieldCreate("Medicine", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget medicinetext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 341, 123, 351), medicinevalue);
            medicineCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            medicinetext.SetFontSize(8);
            if (cbMedicine.IsChecked == true)
            {
                medicineCheck.SetChecked(true);
                medicinetext.SetText((character.WisdomModifier + character.ProficiencyBonus).ToString());
            }
            medicineCheck.RefreshAppearance();
            medicinetext.RefreshAppearance();
            page.AnnotPushBack(medicinetext);
            page.AnnotPushBack(medicineCheck);

            //  Nature
            CheckBoxWidget natureCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 328, 107, 338));
            Field naturevalue = doc.FieldCreate("Nature", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget naturetext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 328, 123, 338), naturevalue);
            natureCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            naturetext.SetFontSize(8);
            if (cbNature.IsChecked == true)
            {
                natureCheck.SetChecked(true);
                naturetext.SetText((character.InteligenceModifier + character.ProficiencyBonus).ToString());
            }
            natureCheck.RefreshAppearance();
            naturetext.RefreshAppearance();
            page.AnnotPushBack(naturetext);
            page.AnnotPushBack(natureCheck);

            //  Perception
            CheckBoxWidget perceptionCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 315, 107, 325));
            Field perceptionvalue = doc.FieldCreate("Perception", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget percetiontext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 315, 123, 325), perceptionvalue);
            perceptionCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            percetiontext.SetFontSize(8);
            if (cbPerception.IsChecked == true)
            {
                perceptionCheck.SetChecked(true);
                percetiontext.SetText((character.WisdomModifier + character.ProficiencyBonus).ToString());
            }
            perceptionCheck.RefreshAppearance();
            percetiontext.RefreshAppearance();
            page.AnnotPushBack(percetiontext);
            page.AnnotPushBack(perceptionCheck);

            //  Performance
            CheckBoxWidget performanceCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 302, 107, 312));
            Field performancevalue = doc.FieldCreate("Performance", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget performancetext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 302, 123, 312), performancevalue);
            performanceCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            performancetext.SetFontSize(8);
            if (cbPerformance.IsChecked == true)
            {
                performanceCheck.SetChecked(true);
                performancetext.SetText((character.CharismaModifier + character.ProficiencyBonus).ToString());
            }
            performanceCheck.RefreshAppearance();
            performancetext.RefreshAppearance();
            page.AnnotPushBack(performancetext);
            page.AnnotPushBack(performanceCheck);

            //  Persuasion
            CheckBoxWidget persuasionCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 289, 107, 299));
            Field persuasionvalue = doc.FieldCreate("Persuasion", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget persuasiontext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 289, 123, 299), persuasionvalue);
            persuasionCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            persuasiontext.SetFontSize(8);
            if (cbPersuasion.IsChecked == true)
            {
                persuasionCheck.SetChecked(true);
                persuasiontext.SetText((character.CharismaModifier + character.ProficiencyBonus).ToString());
            }
            persuasionCheck.RefreshAppearance();
            persuasiontext.RefreshAppearance();
            page.AnnotPushBack(persuasiontext);
            page.AnnotPushBack(persuasionCheck);

            //  Religion
            CheckBoxWidget religionCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 275, 107, 285));
            Field religionvalue = doc.FieldCreate("Religion", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget religiontext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 275, 123, 285), religionvalue);
            religionCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            religiontext.SetFontSize(8);
            if (cbReligion.IsChecked == true)
            {
                religionCheck.SetChecked(true);
                religiontext.SetText((character.InteligenceModifier + character.ProficiencyBonus).ToString());
            }
            religionCheck.RefreshAppearance();
            religiontext.RefreshAppearance();
            page.AnnotPushBack(religiontext);
            page.AnnotPushBack(religionCheck);

            //  Sleight of Hand
            CheckBoxWidget sohCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 261, 107, 271));
            Field sohvalue = doc.FieldCreate("Sleight of Hand", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget sohtext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 261, 123, 271), sohvalue);
            sohCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            sohtext.SetFontSize(8);
            if (cbSleightofHand.IsChecked == true)
            {
                sohCheck.SetChecked(true);
                sohtext.SetText((character.DexterityModifier + character.ProficiencyBonus).ToString());
            }
            sohCheck.RefreshAppearance();
            sohtext.RefreshAppearance();
            page.AnnotPushBack(sohtext);
            page.AnnotPushBack(sohCheck);

            //  Stealth
            CheckBoxWidget stealthCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 247, 107, 257));
            Field stealthvalue = doc.FieldCreate("Stealth", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget stealthtext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 247, 123, 257), stealthvalue);
            stealthCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            stealthtext.SetFontSize(8);
            if (cbStealth.IsChecked == true)
            {
                stealthCheck.SetChecked(true);
                stealthtext.SetText((character.DexterityModifier + character.ProficiencyBonus).ToString());
            }
            stealthCheck.RefreshAppearance();
            stealthtext.RefreshAppearance();
            page.AnnotPushBack(stealthtext);
            page.AnnotPushBack(stealthCheck);

            //  Survival
            CheckBoxWidget survivalCheck = CheckBoxWidget.Create(doc, new pdftron.PDF.Rect(97, 233, 107, 243));
            Field survivalvalue = doc.FieldCreate("Survival", Field.Type.e_text, doc.CreateIndirectString(" "));
            TextWidget survivaltext = TextWidget.Create(doc, new pdftron.PDF.Rect(113, 233, 123, 243), survivalvalue);
            survivalCheck.SetBorderColor(new ColorPt(0, 0, 0), 3);
            survivaltext.SetFontSize(8);
            if (cbSurvival.IsChecked == true)
            {
                survivalCheck.SetChecked(true);
                survivaltext.SetText((character.WisdomModifier + character.ProficiencyBonus).ToString());
            }
            survivalCheck.RefreshAppearance();
            survivaltext.RefreshAppearance();
            page.AnnotPushBack(survivalCheck);
            page.AnnotPushBack(survivaltext);

        }

        void SetStats()
        {
            try
            {
                //  Armor Class
                Field acvalue = doc.FieldCreate("ArmorClass", Field.Type.e_text, doc.CreateIndirectString(character.ArmorClass.ToString()));
                TextWidget actext = TextWidget.Create(doc, new pdftron.PDF.Rect(235, 635, 250, 655), acvalue);
                actext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                actext.SetFontSize(14);
                actext.RefreshAppearance();
                page.AnnotPushBack(actext);

                //  Initiative
                Field initvalue = doc.FieldCreate("Initiative", Field.Type.e_text, doc.CreateIndirectString(character.Initiative.ToString()));
                TextWidget inittext = TextWidget.Create(doc, new pdftron.PDF.Rect(290, 635, 305, 655), initvalue);
                inittext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                inittext.SetFontSize(14);
                inittext.RefreshAppearance();
                page.AnnotPushBack(inittext);

                //  Speed
                Field speedvalue = doc.FieldCreate("Speed", Field.Type.e_text, doc.CreateIndirectString(character.CharacterSpeed.ToString()));
                TextWidget speedtext = TextWidget.Create(doc, new pdftron.PDF.Rect(345, 635, 360, 655), speedvalue);
                speedtext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                speedtext.SetFontSize(14);
                speedtext.RefreshAppearance();
                page.AnnotPushBack(speedtext);

                //HitPointMaximum
                Field maxHPvalue = doc.FieldCreate("MaximumHitPoint", Field.Type.e_text, doc.CreateIndirectString(character.CharacterMaxHP.ToString()));
                TextWidget maxHPtext = TextWidget.Create(doc, new pdftron.PDF.Rect(290, 590, 315, 603), maxHPvalue);
                maxHPtext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                maxHPtext.SetFontSize(10);
                maxHPtext.RefreshAppearance();
                page.AnnotPushBack(maxHPtext);

                //  Current Hit Points
                Field hpvalue = doc.FieldCreate("CurrentHitPoint", Field.Type.e_text, doc.CreateIndirectString(character.CharacterHP.ToString()));
                TextWidget hptext = TextWidget.Create(doc, new pdftron.PDF.Rect(275, 565, 305, 580), hpvalue);
                hptext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                hptext.SetFontSize(14);
                hptext.RefreshAppearance();
                page.AnnotPushBack(hptext);

                //  Temporary Hit Points
                Field tempHPvalue = doc.FieldCreate("TemporaryHitPoint", Field.Type.e_text, doc.CreateIndirectString(character.CharacterMaxHP.ToString()));
                TextWidget tempHPtext = TextWidget.Create(doc, new pdftron.PDF.Rect(275, 510, 305, 525), tempHPvalue);
                tempHPtext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                tempHPtext.SetFontSize(12);
                tempHPtext.RefreshAppearance();
                page.AnnotPushBack(tempHPtext);

                // Total Hit Dice
                Field totalhitDicevalue = doc.FieldCreate("TotalHitDice", Field.Type.e_text, doc.CreateIndirectString(character.HitDice.ToString()));
                TextWidget totalhitDicetext = TextWidget.Create(doc, new pdftron.PDF.Rect(245, 468, 260, 478), totalhitDicevalue);
                totalhitDicetext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                totalhitDicetext.SetFontSize(8);
                totalhitDicetext.RefreshAppearance();
                page.AnnotPushBack(totalhitDicetext);

                //  Recovery Dice
                Field recoveryvalue = doc.FieldCreate("RecoveryDice", Field.Type.e_text, doc.CreateIndirectString(character.DiceRecovery));
                TextWidget recoverytext = TextWidget.Create(doc, new pdftron.PDF.Rect(245, 445, 265, 460), recoveryvalue);
                recoverytext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                recoverytext.SetFontSize(12);
                recoverytext.RefreshAppearance();
                page.AnnotPushBack(recoverytext);

                //  Attack#1
                Field attackvalue = doc.FieldCreate("Attack#1", Field.Type.e_text, doc.CreateIndirectString(character.Attack));
                TextWidget attacktext = TextWidget.Create(doc, new pdftron.PDF.Rect(216, 389, 380, 405), attackvalue);
                attacktext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                attacktext.SetFontSize(10);
                attacktext.RefreshAppearance();
                page.AnnotPushBack(attacktext);

                //  Attack#2
                Field attackvalue2 = doc.FieldCreate("Attack#2", Field.Type.e_text, doc.CreateIndirectString(character.Attack2));
                TextWidget attacktext2 = TextWidget.Create(doc, new pdftron.PDF.Rect(216, 367, 380, 383), attackvalue2);
                attacktext2.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                attacktext2.SetFontSize(10);
                attacktext2.RefreshAppearance();
                page.AnnotPushBack(attacktext2);

                //  Attack#3
                Field attackvalue3 = doc.FieldCreate("Attack#3", Field.Type.e_text, doc.CreateIndirectString(character.Attack3));
                TextWidget attacktext3 = TextWidget.Create(doc, new pdftron.PDF.Rect(216, 345, 380, 361), attackvalue3);
                attacktext3.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                attacktext3.SetFontSize(10);
                attacktext3.RefreshAppearance();
                page.AnnotPushBack(attacktext3);

                //  Spells 
                Field spellsvalue = doc.FieldCreate("Spells", Field.Type.e_text, doc.CreateIndirectString(character.Spells));
                spellsvalue.SetFlag(Field.Flag.e_multiline, true);
                TextWidget spellstext = TextWidget.Create(doc, new pdftron.PDF.Rect(216, 223, 381, 341), spellsvalue);
                spellstext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                spellstext.SetFontSize(10);
                spellstext.RefreshAppearance();
                page.AnnotPushBack(spellstext);
            }
            catch
            {
                MessageBox.Show("Please, check if you let something blank around attacks or spells.");
            }
        }

        void SetEquipment()
        {
            try
            {
                // Equipment#1
                Field equipsvalue = doc.FieldCreate("Equipment#1", Field.Type.e_text, doc.CreateIndirectString(character.Equipment1));
                equipsvalue.SetFlag(Field.Flag.e_multiline, true);
                TextWidget equipstext = TextWidget.Create(doc, new pdftron.PDF.Rect(259, 70, 381, 205), equipsvalue);
                equipstext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                equipstext.SetFontSize(10);
                equipstext.RefreshAppearance();
                page.AnnotPushBack(equipstext);

                //  Equipment#2
                Field equipsvalue2 = doc.FieldCreate("Equipment#2", Field.Type.e_text, doc.CreateIndirectString(character.Equipment2));
                equipsvalue2.SetFlag(Field.Flag.e_multiline, true);
                TextWidget equipstext2 = TextWidget.Create(doc, new pdftron.PDF.Rect(221, 35, 381, 67), equipsvalue2);
                equipstext2.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                equipstext2.SetFontSize(10);
                equipstext2.RefreshAppearance();
                page.AnnotPushBack(equipstext2);

                //  Copper
                Field coppervalue = doc.FieldCreate("Copper", Field.Type.e_text, doc.CreateIndirectString(character.Copper.ToString()));
                TextWidget coppertext = TextWidget.Create(doc, new pdftron.PDF.Rect(226, 179, 251, 195), coppervalue);
                coppertext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                coppertext.SetFontSize(10);
                coppertext.RefreshAppearance();
                page.AnnotPushBack(coppertext);

                //  Silver
                Field silvervalue = doc.FieldCreate("Silver", Field.Type.e_text, doc.CreateIndirectString(character.Silver.ToString()));
                TextWidget silvertext = TextWidget.Create(doc, new pdftron.PDF.Rect(226, 152, 251, 168), silvervalue);
                silvertext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                silvertext.SetFontSize(10);
                silvertext.RefreshAppearance();
                page.AnnotPushBack(silvertext);

                //  Electrum
                Field electrumvalue = doc.FieldCreate("Electrum", Field.Type.e_text, doc.CreateIndirectString(character.Electrum.ToString()));
                TextWidget electrumtext = TextWidget.Create(doc, new pdftron.PDF.Rect(226, 126, 251, 141), electrumvalue);
                electrumtext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                electrumtext.SetFontSize(10);
                electrumtext.RefreshAppearance();
                page.AnnotPushBack(electrumtext);

                //  Gold
                Field goldvalue = doc.FieldCreate("Gold", Field.Type.e_text, doc.CreateIndirectString(character.Gold.ToString()));
                TextWidget goldtext = TextWidget.Create(doc, new pdftron.PDF.Rect(226, 99, 251, 115), goldvalue);
                goldtext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                goldtext.SetFontSize(10);
                goldtext.RefreshAppearance();
                page.AnnotPushBack(goldtext);

                //  Platinum
                Field platinumvalue = doc.FieldCreate("Platinum", Field.Type.e_text, doc.CreateIndirectString(character.Platinum.ToString()));
                TextWidget platinumtext = TextWidget.Create(doc, new pdftron.PDF.Rect(226, 72, 251, 88), platinumvalue);
                platinumtext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                platinumtext.SetFontSize(10);
                platinumtext.RefreshAppearance();
                page.AnnotPushBack(platinumtext);
            }
            catch
            {
                MessageBox.Show("Check if you did not forgot to fill in the equipments and your gold.");
            }
        }

        void SetPersonality()
        {
            try
            {
                //  Personality Traits
                Field traitsvalue = doc.FieldCreate("PersonalityTraits", Field.Type.e_text, doc.CreateIndirectString(character.Traits));
                traitsvalue.SetFlag(Field.Flag.e_multiline, true);
                TextWidget traitstext = TextWidget.Create(doc, new pdftron.PDF.Rect(406, 607, 562, 655), traitsvalue);
                traitstext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                traitstext.SetFontSize(10);
                traitstext.RefreshAppearance();
                page.AnnotPushBack(traitstext);

                //  Ideals
                Field idealsvalue = doc.FieldCreate("Ideals", Field.Type.e_text, doc.CreateIndirectString(character.Ideals));
                idealsvalue.SetFlag(Field.Flag.e_multiline, true);
                TextWidget idealstext = TextWidget.Create(doc, new pdftron.PDF.Rect(406, 555, 562, 592), idealsvalue);
                idealstext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                idealstext.SetFontSize(10);
                idealstext.RefreshAppearance();
                page.AnnotPushBack(idealstext);

                //  Bonds
                Field bondsvalue = doc.FieldCreate("Bonds", Field.Type.e_text, doc.CreateIndirectString(character.Bonds));
                bondsvalue.SetFlag(Field.Flag.e_multiline, true);
                TextWidget bondstext = TextWidget.Create(doc, new pdftron.PDF.Rect(406, 495, 562, 537), bondsvalue);
                bondstext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                bondstext.SetFontSize(10);
                bondstext.RefreshAppearance();
                page.AnnotPushBack(bondstext);

                //  Flaws
                Field flawsvalue = doc.FieldCreate("Flaws", Field.Type.e_text, doc.CreateIndirectString(character.Flaws));
                flawsvalue.SetFlag(Field.Flag.e_multiline, true);
                TextWidget flawstext = TextWidget.Create(doc, new pdftron.PDF.Rect(406, 444, 562, 480), flawsvalue);
                flawstext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                flawstext.SetFontSize(10);
                flawstext.RefreshAppearance();
                page.AnnotPushBack(flawstext);
            }
            catch
            {
                MessageBox.Show("Please, check if you let something blank in Flaws, Bonds, Ideals or Personality Traits.");
            }

        }

        void SetFeaturesTraits()
        {
            try
            {
                //  Features & Traits
                Field featuresValue = doc.FieldCreate("FeaturesTraits", Field.Type.e_text, doc.CreateIndirectString(character.FeaturesTraits));
                featuresValue.SetFlag(Field.Flag.e_multiline, true);
                TextWidget featuresText = TextWidget.Create(doc, new pdftron.PDF.Rect(404, 30, 565, 416), featuresValue);
                featuresText.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
                featuresText.SetFontSize(10);
                featuresText.RefreshAppearance();
                page.AnnotPushBack(featuresText);
            }
            catch
            {
                MessageBox.Show("Please, fill in all your traits and features.");
            }
        }

        void SetOtherProficienciesLanguages()
        {
            Field plValue = doc.FieldCreate("Proficiencies&Languages", Field.Type.e_text, doc.CreateIndirectString(character.ProficienciesLanguages));
            plValue.SetFlag(Field.Flag.e_multiline, true);
            TextWidget pltext = TextWidget.Create(doc, new pdftron.PDF.Rect(32, 35, 197, 170), plValue);
            pltext.SetFont(Font.Create(doc, Font.StandardType1Font.e_times_bold));
            pltext.SetFontSize(10);
            pltext.RefreshAppearance();
            page.AnnotPushBack(pltext);
        }

        #region Saving Throws

        private void strChecked(object sender, RoutedEventArgs e)
        {
            strSavingThrow.Content = character.StrengthModifier.ToString();

            if (strSavingThrow.IsChecked == false)
            {
                strSavingThrow.Content = "";
            }
        }

        private void dexChecked(object sender, RoutedEventArgs e)
        {
            dexSavingThrow.Content = character.DexterityModifier.ToString();
            if (dexSavingThrow.IsChecked == false)
            {
                dexSavingThrow.Content = "";
            }
        }

        private void constChecked(object sender, RoutedEventArgs e)
        {
            constSavingThrow.Content = character.ConstitutionModifier.ToString();
            if (constSavingThrow.IsChecked == false)
            {
                constSavingThrow.Content = "";
            }
        }

        private void intChecked(object sender, RoutedEventArgs e)
        {
            intSavingThrow.Content = character.InteligenceModifier.ToString();
            if (intSavingThrow.IsChecked == false)
            {
                intSavingThrow.Content = "";
            }
        }

        private void wisChecked(object sender, RoutedEventArgs e)
        {
            wisSavingThrow.Content = character.WisdomModifier.ToString();
            if (wisSavingThrow.IsChecked == false)
            {
                wisSavingThrow.Content = "";
            }
        }

        private void charChecked(object sender, RoutedEventArgs e)
        {
            charSavingThrow.Content = character.CharismaModifier.ToString();
            if (charSavingThrow.IsChecked == false)
            {
                charSavingThrow.Content = "";
            }
        }



        #endregion

        #region Skills
        private void AcrobaticsChecked(object sender, RoutedEventArgs e)
        {
            cbAcrobatics.Content = (character.DexterityModifier + character.ProficiencyBonus).ToString();
            if (cbAcrobatics.IsChecked == false)
            {
                cbAcrobatics.Content = "";
            }
        }

        private void cbAnimalHandling_Checked(object sender, RoutedEventArgs e)
        {
            cbAnimalHandling.Content = (character.WisdomModifier + character.ProficiencyBonus).ToString();
            if (cbAnimalHandling.IsChecked == false)
            {
                cbAnimalHandling.Content = "";
            }
        }

        private void ArcanaChecked(object sender, RoutedEventArgs e)
        {
            cbArcana.Content = (character.InteligenceModifier + character.ProficiencyBonus).ToString();
            if (cbArcana.IsChecked == false)
            {
                cbArcana.Content = "";
            }
        }

        private void AthleticsChecked(object sender, RoutedEventArgs e)
        {
            cbAthletics.Content = (character.StrengthModifier + character.ProficiencyBonus).ToString();
            if (cbAthletics.IsChecked == false)
            {
                cbAthletics.Content = "";
            }
        }

        private void DeceptionChecked(object sender, RoutedEventArgs e)
        {
            cbDeception.Content = (character.CharismaModifier + character.ProficiencyBonus).ToString();
            if (cbDeception.IsChecked == false)
            {
                cbDeception.Content = "";
            }
        }

        private void cbHistory_Checked(object sender, RoutedEventArgs e)
        {
            cbHistory.Content = (character.InteligenceModifier + character.ProficiencyBonus).ToString();
            if (cbHistory.IsChecked == false)
            {
                cbHistory.Content = "";
            }
        }

        private void cbInsight_Checked(object sender, RoutedEventArgs e)
        {
            cbInsight.Content = (character.WisdomModifier + character.ProficiencyBonus).ToString();
            if (cbInsight.IsChecked == false)
            {
                cbInsight.Content = "";
            }
        }

        private void cbIntimidation_Checked(object sender, RoutedEventArgs e)
        {
            cbIntimidation.Content = (character.CharismaModifier + character.ProficiencyBonus).ToString();
            if (cbIntimidation.IsChecked == false)
            {
                cbIntimidation.Content = "";
            }
        }

        private void cbInvestigation_Checked(object sender, RoutedEventArgs e)
        {
            cbInvestigation.Content = (character.InteligenceModifier + character.ProficiencyBonus).ToString();
            if (cbInvestigation.IsChecked == false)
            {
                cbInvestigation.Content = "";
            }
        }

        private void cbMedicine_Checked(object sender, RoutedEventArgs e)
        {
            cbMedicine.Content = (character.WisdomModifier + character.ProficiencyBonus).ToString();
            if (cbMedicine.IsChecked == false)
            {
                cbMedicine.Content = "";
            }
        }

        private void cbNature_Checked(object sender, RoutedEventArgs e)
        {
            cbNature.Content = (character.InteligenceModifier + character.ProficiencyBonus).ToString();
            if (cbNature.IsChecked == false)
            {
                cbNature.Content = "";
            }
        }

        private void cbPerception_Checked(object sender, RoutedEventArgs e)
        {
            cbPerception.Content = (character.WisdomModifier + character.ProficiencyBonus).ToString();
            if (cbPerception.IsChecked == false)
            {
                cbPerception.Content = "";
            }
        }

        private void cbPerformance_Checked(object sender, RoutedEventArgs e)
        {
            cbPerformance.Content = (character.CharismaModifier + character.ProficiencyBonus).ToString();
            if (cbPerformance.IsChecked == false)
            {
                cbPerformance.Content = "";
            }
        }

        private void cbPersuasion_Checked(object sender, RoutedEventArgs e)
        {
            cbPersuasion.Content = (character.CharismaModifier + character.ProficiencyBonus).ToString();
            if (cbPersuasion.IsChecked == false)
            {
                cbPersuasion.Content = "";
            }
        }

        private void cbReligion_Checked(object sender, RoutedEventArgs e)
        {
            cbReligion.Content = (character.InteligenceModifier + character.ProficiencyBonus).ToString();
            if (cbReligion.IsChecked == false)
            {
                cbReligion.Content = "";
            }
        }

        private void cbSleightofHand_Checked(object sender, RoutedEventArgs e)
        {
            cbSleightofHand.Content = (character.DexterityModifier + character.ProficiencyBonus).ToString();
            if (cbSleightofHand.IsChecked == false)
            {
                cbSleightofHand.Content = "";
            }
        }

        private void cbStealth_Checked(object sender, RoutedEventArgs e)
        {
            cbStealth.Content = (character.DexterityModifier + character.ProficiencyBonus).ToString();
            if (cbStealth.IsChecked == false)
            {
                cbStealth.Content = "";
            }
        }

        private void cbSurvival_Checked(object sender, RoutedEventArgs e)
        {
            cbSurvival.Content = (character.WisdomModifier + character.ProficiencyBonus).ToString();
            if (cbSurvival.IsChecked == false)
            {
                cbSurvival.Content = "";
            }
        }








        #endregion

    }
}
