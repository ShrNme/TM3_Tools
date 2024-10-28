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
using System.IO;

namespace TM3_Tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Data1Filepath;
        public static string Data2Filepath;
        public static string Data3Filepath;
        public static string Data4Filepath;
        public static string Data5Filepath;

        string pathFail = "Invalid filepath";

        
        public MainWindow()
        {
            InitializeComponent();
        }

        private string FileDialogFunction(string Filter)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter = Filter;
            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                return openFileDlg.FileName;

            }
            return pathFail;
        }
        private string SaveFileDialog(string Filter)
        {
            Microsoft.Win32.SaveFileDialog saveFileDlg = new Microsoft.Win32.SaveFileDialog();
            saveFileDlg.Filter = Filter;
            Nullable<bool> result = saveFileDlg.ShowDialog();
            if(result == true)
            {
                return saveFileDlg.FileName;
            }
            return null;
        }


        private void Data1Button_Click(object sender, RoutedEventArgs e)
        {
            Data1Filepath = FileDialogFunction(".bin files|*bin");
            Data1Box.Text = Data1Filepath;
        }

        private void Data2Button_Click(object sender, RoutedEventArgs e)
        {
            Data2Filepath = FileDialogFunction(".bin files|*bin");
            Data2Box.Text = Data2Filepath;
        }

        private void Data3Button_Click(object sender, RoutedEventArgs e)
        {
            Data3Filepath = FileDialogFunction(".bin files|*bin");
            Data3Box.Text = Data3Filepath;
        }

        private void Data4Button_Click(object sender, RoutedEventArgs e)
        {
            Data4Filepath = FileDialogFunction(".bin files|*bin");
            Data4Box.Text = Data4Filepath;
        }

        private void Data5Button_Click(object sender, RoutedEventArgs e)
        {
            Data5Filepath = FileDialogFunction(".bin files|*bin");
            Data5Box.Text = Data5Filepath;
        }

        //Extracts text from Data5.BIN
        private void ExtractScript_Btn_Click(object sender, RoutedEventArgs e)
        {
           

            if(Data5Filepath != null && Data5Filepath != pathFail)
            {
                ProgressLabel.Content = "Extracting script";
                try
                {

                    StreamWriter filewrite = new StreamWriter(SaveFileDialog("Text file (.txt) |*.txt"), true);


                    byte [] bytearray = File.ReadAllBytes(Data5Filepath);
                   
                    
                    filewrite.WriteLine("Beggining lines extracted from Data5.bin");

                    Helper.extractText(0x40FB6, 0x42954, bytearray, filewrite);
                    Helper.extractText(0x4822E, 0x487D4, bytearray, filewrite);
                    Helper.extractText(0xCD0F7, 0xCFFFE, bytearray, filewrite);
                    Helper.extractText(0xD034B, 0xD08CA, bytearray, filewrite);
                    Helper.extractText(0xD0B35, 0xD120D, bytearray, filewrite);
                    Helper.extractText(0xD1509, 0xD1A58, bytearray, filewrite);
                    Helper.extractText(0xD1c48, 0xD211F, bytearray, filewrite);
                    Helper.extractText(0xD232b, 0xD27CE, bytearray, filewrite);
                    Helper.extractText(0xD2BEF, 0xD376c, bytearray, filewrite);
                    Helper.extractText(0xD398D, 0xD3F22, bytearray, filewrite);
                    Helper.extractText(0xD41b5, 0xD4C48, bytearray, filewrite);
                    Helper.extractText(0xD41b5, 0xD4C48, bytearray, filewrite);

                    if ((bool) IncludeCredits.IsChecked) {
                         Helper.extractText(0xD4E80, 0xDD927, bytearray, filewrite); //This is the game's credits.
                    }
                    
                    Helper.extractText(0xDD930, 0xDE202, bytearray, filewrite);
                    Helper.extractText(0x106D17, 0x1088BA, bytearray, filewrite);
                    Helper.extractText(0x11A570, 0x11B34E, bytearray, filewrite);
                    Helper.extractText(0x11B490, 0x11B59C, bytearray, filewrite);  
                    Helper.extractText(0x11B6F0, 0x11B7F6, bytearray, filewrite);
                    Helper.extractText(0x11B8C6, 0x11FABA, bytearray, filewrite); //completion text for hobby tasks
                    Helper.extractText(0x1204C6, 0x1213F3, bytearray, filewrite);
                    Helper.extractText(0x121825, 0x122350, bytearray, filewrite);
                    Helper.extractText(0x17D0D7, 0x17EBCF, bytearray, filewrite);//MOTHERLOADE of date dialogue, location names, holidays, other important text
                    Helper.extractText(0x17EF80, 0x18C4A7, bytearray, filewrite);//LOTS of dialogue, one proble: it's not quite clear who says what
                    Helper.extractText(0x1A9366, 0x1BA257, bytearray, filewrite);
                    Helper.extractText(0x1BA748, 0x1D4A77, bytearray, filewrite);
                    Helper.extractText(0x1D4DD2, 0x1E18AF, bytearray, filewrite);
                    Helper.extractText(0x1E1BC5, 0x1EBEEF, bytearray, filewrite);
                    Helper.extractText(0x1EC3D0, 0x1FFFFB, bytearray, filewrite);
                    Helper.extractText(0x341762, 0x3417D7, bytearray, filewrite);//mem card stuff
                    Helper.extractText(0x341816, 0x341B82, bytearray, filewrite);//mem card stuff
                    Helper.extractText(0x372F82, 0x373137, bytearray, filewrite);// more mem card stuff
                    Helper.extractText(0x373672, 0x373735, bytearray, filewrite);
                    Helper.extractText(0x373830, 0x373F16, bytearray, filewrite);//options the player is presented with at the start of the game, and some more memory card stuff
                    Helper.extractText(0x37403B, 0x37408A, bytearray, filewrite);
                    Helper.extractText(0x3742A0, 0x374336, bytearray, filewrite);//more memcard stuff
                    Helper.extractText(0x3743C7, 0x37466B, bytearray, filewrite);//options menu stuff
                    Helper.extractText(0x3B9A20, 0x3B9B5A, bytearray, filewrite);//name/birthday/evs stuff from the start of the game. preceded by the list of selectable characters in your name
                    Helper.extractText(0x3B9BB0, 0x3B9BD0, bytearray, filewrite);
                    if ((bool) IncludeNames.IsChecked)
                    {
                        Helper.extractText(0x3b9C20, 0x3DF2B0, bytearray, filewrite); //seems to be a massive list of names? i think this is a list of pre-created names you can choose from
                    }
                    Helper.extractText(0x3DF310, 0x3DF573, bytearray, filewrite);//EVS stuff
                    Helper.extractText(0x5A4090, 0x5A4338, bytearray, filewrite);//This appears to be prizes for the summer festival minigames
                    Helper.extractText(0x618DC8, 0x619070, bytearray, filewrite); //more minigame prizes?
                    Helper.extractText(0x6199F0, 0x619C4E, bytearray, filewrite);//instructions for one of the festival minigames (the from one)
                    Helper.extractText(0x69EEA0, 0x69F134, bytearray, filewrite);
                    Helper.extractText(0x6D8000, 0x6D83f7, bytearray, filewrite);
                    Helper.extractText(0x7901C0, 0x79029C, bytearray, filewrite);//i think this has something to do with a minigame
                    Helper.extractText(0x790D70, 0x7910FA, bytearray, filewrite); //instructions for another minigame
                    Helper.extractText(0x7AF600, 0x7AFFBB, bytearray, filewrite);//fashion reactions maybe? seems to be a lot of those bits dedicated to moving the faces
                    Helper.extractText(0x9FBE60, 0x9FBEF4, bytearray, filewrite);
                    Helper.extractText(0x9FBFE0, 0x9FC995, bytearray, filewrite);
                    Helper.extractText(0x9FCA60, 0x9FCAC3, bytearray, filewrite);
                    Helper.extractText(0x9FCC00, 0x9FCC35, bytearray, filewrite);
                    Helper.extractText(0x9FCC44, 0x9FCC8C, bytearray, filewrite);
                    Helper.extractText(0x9FCD40, 0x9FD0E0, bytearray, filewrite);
                    Helper.extractText(0x9FD2A8, 0x9FD2C3, bytearray, filewrite);//...Konami man?
                    Helper.extractText(0x9FD370, 0x9FD505, bytearray, filewrite);
                    Helper.extractText(0xA13D90, 0xA14356, bytearray, filewrite);
                    Helper.extractText(0xA14A30, 0xA159ED, bytearray, filewrite);
                    Helper.extractText(0xA1DE67, 0xA1DF98, bytearray, filewrite);//list of schools you play against when ur on the tennis team
                    
                    filewrite.WriteLine("Ending lines extracted from Data5.bin");

                    

                   


                    filewrite.Close();
                    ProgressLabel.Content = "Text Extraction Complete!";
                    
                }
                catch(Exception error)
                {
                    MessageBox.Show("Failed to extract Text. Error: " + error.Message);
                    ProgressLabel.Content = "Text Extraction Failed";
                }
            }
            else
            {
                MessageBox.Show("We can't extract the script without filepaths to the data files!");
            }


        }

        private void Extract_ATP_files_Click(object sender, RoutedEventArgs e)
        {
 
            Window extract = new Window1(); //i tried changing the class name but then something else broke and i don't feel like dealing with that right now
            extract.ShowDialog();

            
        }

        //TODO: add a function for mass-decoding ATP files, since there are hundreds in each .BIN
        private void DecodeATPFile_Click(object sender, RoutedEventArgs e)
        {
            string InPath = FileDialogFunction("ATP files (.atp) | *.atp");
            string OutPath = SaveFileDialog("decompressed ATP file (.atp.decompress) |*.apt.decompress");
            ProgressLabel.Content = "Decoding ATP file";
            byte[] byteArray = File.ReadAllBytes(InPath);
            Decoder.DecodeATP(0x0, byteArray, OutPath);
            ProgressLabel.Content = "Decoded ATP file";
        }
    }
        
    }


