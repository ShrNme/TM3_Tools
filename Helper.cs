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
    static class Helper
    {
        //byte sequences that appear in small breaks between sections of valid game text
        //not exactly sure what they mark
        //there's a good chance they're being used for things like animation control, inserting names, sound control, etc
        //regardless, they (for now) need to be filtered out when extracting text
        //TODO: figure out how these scripting commands work and find ways to include them in the extraction script so the translators don't mess something up
        private static byte[] BadArray = {0x80, 0x7E};
        private static byte[] BadArray2 = {0x02, 0x22,0xF3,0xff,0xff,0xff};
        private static byte[] BadArray3 = { 0x1a};
        private static byte[] BadArray4 = { 0x74, 0x04 };
        private static byte[] BadArray5 = { 0x21,0x47};
        //private static byte[] BadArray6 = { 0x1b };
        private static byte[] BadArray7 = { 0x81, 0x75, 0x70};
        private static byte[] BadArray8 = { 0x20, 0x3D };
        private static byte[] BadArray9 = { 0x03, 0x22 };
        //private static byte[] BadArray10 = { 0x09, 0x1B, 0x21};
        private static byte[] BadArray11 = {0xff, 0xff, 0xff };
        //private static byte[] BadArray12 = { 0x1B, 0x20 };
        private static byte[] BadArray13 = { 0x42, 0x81, 0x7E };
        private static byte[] BadArray14 = { 0x06, 0x23};

        public static string byteArrayToString(byte[] array)
        {
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("shift-jis");
            System.Text.Encoding enc2 = System.Text.Encoding.GetEncoding("unicode");
            byte[] temp = System.Text.Encoding.Convert(enc, enc2, array);
            string temp2 = enc2.GetString(temp);
            return temp2;


        }

        private static bool byteArrayContainsSubarray(byte[] array, byte[] subarray)
        {
            if(subarray.Length > array.Length)
            {
                return false;
            }
            if(subarray.Length == 0)
            {
                return true;
            }

            int matches = 0;
            for(int i = 0; i<array.Length; i++)
            {
                if (array[i] == subarray[matches])
                {
                    matches++;
                    if(matches == subarray.Length)
                    {
                        return true;
                    }

                }
                else
                {
                    matches = 0;
                }


                if (i + subarray.Length - matches > array.Length)
                {
                    return false;
                }
            }
            return false;

        }

        //the code in this method looks like a mess
        //I'm sorry but I'm only doing it this way to make extractText look easier to read
        private static bool isByteArrayClean(byte[] array)
        {
            //see if it contains banned subarrays 
            if(byteArrayContainsSubarray(array, BadArray) || byteArrayContainsSubarray(array, BadArray2) || byteArrayContainsSubarray(array,BadArray3) || byteArrayContainsSubarray(array, BadArray4))
            {
                return false;
            }
            if(byteArrayContainsSubarray(array, BadArray5) || byteArrayContainsSubarray(array, BadArray7) || byteArrayContainsSubarray(array, BadArray8))
            {
                return false;
            }
            if(byteArrayContainsSubarray(array, BadArray9) || byteArrayContainsSubarray(array, BadArray11)){// || byteArrayContainsSubarray(array, BadArray12)){
                return false;
            }
            if(byteArrayContainsSubarray(array, BadArray13)|| byteArrayContainsSubarray(array, BadArray14))
            {
                return false;
            }

            //check if the array is a two-byte array starting with a random ascii character
            //it's possible, though unlikely, that this could accidentally block the extraction of some valid text
            //i don't think it should be a problem however, given how unlikely a block of text meeting these requirements would show up in the actual script
            if(array.Length == 2 && array[0] <0x7f)
            {
                return false;
            }

            

            return true;
        }


        //extract text from the byte array, starting and ending at the specified addresses
        //it also writes the extraced text to a file (as defined by streamWrite)
        public static void extractText(int startAddress, int endAddress, byte[] byteArray, StreamWriter streamWriter)
        {
            streamWriter.WriteLine("Block begins at address: " + (startAddress).ToString("X"));
            if (endAddress > byteArray.Length) //catch a possible error
            {
                MessageBox.Show("Text extraction error: end address greater than byte array length");
                return;
            }
            //when end Address is set to -1, read to the end of the file
            if (endAddress == -1)
            {
                endAddress = byteArray.Length;
            }


            List<byte> temp = new List<byte>();
            for (int i = startAddress; i < endAddress; i++)
            {
                
                byte x = byteArray[i];
                
                
                if ((x > 0x1f || x==0x9) && x!=0x7f)
                {
                    temp.Add(x);
                }
                else
                {
                    if (temp.Count > 0)
                    {

                        
                        AppendToFile(temp, streamWriter);

                    }
                }
            }
            //Catch any remaining characters in temp after the for loop completes
            AppendToFile(temp, streamWriter);

            streamWriter.WriteLine("Block ends at address: " + (endAddress).ToString("X"));

        }
        private static void AppendToFile(List<byte> temp, StreamWriter streamWriter)
        {
            string tempString;
            byte[] testArray = temp.ToArray();


            if (isByteArrayClean(testArray))
            {
                
                tempString = byteArrayToString(testArray);
            }
            else
            {
                tempString = null;
            }
            temp.Clear();

            //i'm hesitant to filter out 1-character strings (risk of filtering valid text) but I think I have to
            //also, when the first character in the string is " or ! it's going to be nonsense and not game text
            if (tempString != null && tempString.Length > 1 && tempString[0] != '"' && tempString[0] != '!')
            {
                tempString = tempString + "\n";
                streamWriter.Write(tempString);
            }
        }


        

    }

   
}
