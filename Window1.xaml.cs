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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        string targetPath;

        //class for pairing the start address and end address of an ATP file
        private class StartEndPair
        {
            uint Start;
            uint End;

            public StartEndPair(uint startInt, uint endInt)
            {
                Start = startInt;
                End = endInt;
            }
            public override string ToString()
            {
                return "Start: " + Start.ToString("X") + " | End: " + End.ToString("X");
            }
            public uint getStart()
            {
                return Start;
            }
            public uint getEnd()
            {
                return End;
            }
        }
        
        public Window1()
        {
            InitializeComponent();
            
        }

        private StartEndPair[] FindATP(string filepath)
        {
            //look through the file and find the start/end locations of ATP files
            try
            {
                byte[] byteArray = File.ReadAllBytes(filepath);
                uint startAddress = 0;
                bool started = false;
                uint endAddress;
                byte previousPreviousByte = byteArray[0];
                byte previousByte = byteArray[1];

                List<StartEndPair> ATPs = new List<StartEndPair>();


                for (int i = 2; i < byteArray.Length; i++)
                {
                    byte current = byteArray[i];
                    if (!started)
                    {
                        if (previousPreviousByte == 0x41 && previousByte == 0x54 && current == 0x50)
                        {
                            startAddress = (uint)(i - 2);
                            started = true;
                        }
                    }
                    else
                    {
                        if (current == 0x0 && previousByte == 0x0 && previousPreviousByte == 0x0)
                        {
                            endAddress = (uint)(i - 2);
                            ATPs.Add(new StartEndPair(startAddress, endAddress));
                            started = false;
                        }
                    }
                    previousPreviousByte = previousByte;
                    previousByte = current;

                }

                return ATPs.ToArray();
            }
            catch(Exception error)
            {
                MessageBox.Show("Failed to locate ATP files. Error: " + error.Message);
                return null;
            }
        }

        //The following 4 functions handle clicks on the buttons that change which Data file we are extracting ATPs from.
        private void Data3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainWindow.Data3Filepath != null)
                {
                    targetPath = MainWindow.Data3Filepath;
                    OutputPath.Text = MainWindow.Data3Filepath.Substring(0, MainWindow.Data3Filepath.Length - 4) + "\\";
                    ATPList.Items.Clear();
                    StartEndPair[] toAdd = FindATP(MainWindow.Data3Filepath);
                    foreach (StartEndPair x in toAdd)
                    {
                        ATPList.Items.Add(x);
                    }
                    ATPList.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("We can't look for ATP files in Data3.BIN without a filepath for Data3.BIN!");
                }
            }
            catch(Exception error)
            {
                MessageBox.Show("Failed to list ATP files. Error: " + error.Message);
            }
        }
        private void Data4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainWindow.Data4Filepath != null)
                {
                    targetPath = MainWindow.Data4Filepath;
                    OutputPath.Text = MainWindow.Data4Filepath.Substring(0, MainWindow.Data4Filepath.Length - 4) + "\\";
                    ATPList.Items.Clear();
                    StartEndPair[] toAdd = FindATP(MainWindow.Data4Filepath);
                    foreach (StartEndPair x in toAdd)
                    {
                        ATPList.Items.Add(x);
                    }
                    ATPList.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("We can't look for ATP files in Data4.BIN without a filepath for Data4.BIN!");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Failed to list ATP files. Error: " + error.Message);
            }
        }
        private void Data1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainWindow.Data1Filepath != null)
                {
                    targetPath = MainWindow.Data1Filepath;
                    OutputPath.Text = MainWindow.Data1Filepath.Substring(0, MainWindow.Data1Filepath.Length - 4) + "\\";
                    ATPList.Items.Clear();
                    StartEndPair[] toAdd = FindATP(MainWindow.Data1Filepath);
                    foreach (StartEndPair x in toAdd)
                    {
                        ATPList.Items.Add(x);
                    }
                    ATPList.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("We can't look for ATP files in Data1.BIN without a filepath for Data1.BIN!");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Failed to list ATP files. Error: " + error.Message);
            }
        }
        private void Data2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainWindow.Data2Filepath != null)
                {
                    targetPath = MainWindow.Data2Filepath;
                    OutputPath.Text = MainWindow.Data2Filepath.Substring(0, MainWindow.Data2Filepath.Length - 4) + "\\";
                    ATPList.Items.Clear();
                    StartEndPair[] toAdd = FindATP(MainWindow.Data2Filepath);
                    foreach (StartEndPair x in toAdd)
                    {
                        ATPList.Items.Add(x);
                    }
                    ATPList.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("We can't look for ATP files in Data2.BIN without a filepath for Data2.BIN!");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Failed to list ATP files. Error: " + error.Message);
            }
        }

        //Extracts all the ATPs from a data bin file
        private void Extract_Click(object sender, RoutedEventArgs e)
        {
            try {
                int fileNumber = 0;
                byte[] byteArray = File.ReadAllBytes(targetPath);
                Directory.CreateDirectory(OutputPath.Text);
                ProgressLabel.Content = ".ATP extraction\nin progress";
                foreach (StartEndPair x in ATPList.Items) //there should only be these pairs in ATPList
                {

                    byte[] temp = Decoder.SubArray((int)x.getStart(), (int)(x.getEnd() - x.getStart()), byteArray);

                    File.WriteAllBytes(OutputPath.Text + fileNumber + ".atp", temp);
                    fileNumber++;
                }
                ProgressLabel.Content = ".ATP extraction\ncomplete";
            }
            catch(Exception error)
            {
                MessageBox.Show("Failed to extract ATP files. Error: " + error.Message);
            }
        }
    }


}
