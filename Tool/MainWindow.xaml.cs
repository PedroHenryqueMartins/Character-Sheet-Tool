using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using pdftron;
using pdftron.Common;
using pdftron.PDF;
using pdftron.SDF;

// Final Project
// NAME: Pedro Henryque Martins
// STUDENT NUMBER: 1944889
//
// CODE:
// MARKS: 93/100 Excellent work. Definitely can use some code clean up. I think the EditCharacter and CharacterSheetWindow can be a single window with 2 constructors because majority of the functionality is identical. The ManagePDF should handle both save and load, and I think checkboxes can be implemented when loaded and saved.
// Does it compile? Yes
// Does it produce the correct results? Yes
// Is using databinding? Yes
// Does the UI look like the mock up? Yes
//
// PRESENTATION:
// MARKS: 19/20 Excellent work. Nice demo, and code walk through. I really like the fact that you used a third party library for the PDF with a form like file. Improvements: code clean up, move logic of PDF generation outside of the code 

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // LC: not sure why EditCharacter and CharacterSheetWindow can be a single window.
        CharacterSheetWindow characterSheetWindow = new CharacterSheetWindow();

        Character character = new Character();
        ManagePdf managePdf = new ManagePdf();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewPressed(object sender, RoutedEventArgs e)
        {
            this.Close();
            characterSheetWindow.Show();
        }

        private void EditPressed(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "PDF File (*.pdf) | *.pdf";
            if (openFile.ShowDialog() == true)
            {
                PDFDoc file = new PDFDoc(openFile.FileName);
                managePdf.ReadDocument(file, character);
                this.Close();
                EditCharacter editCharacterWindow = new EditCharacter(character, file);
                editCharacterWindow.Show();
            }
        }
    }
}
