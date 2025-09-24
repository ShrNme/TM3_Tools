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
    class Decoder
    {
        //function for decoding ATP files found within the BINs
        //basically just re-implements the decompression routine found in the game
        //I broke it down into 11 parts and have implemented them here
        //It currently does not work! yay
        
        //So here's the issue, I'm loading the bytes in the ATP file as an array
        //However, in the original assembly they are loaded into RAM
        //This means the assembly does not need to worry about the bounds of the array
        //This causes my recreation to have out-of-bounds crash issues

        public static void DecodeATP(int startAddress, byte[] byteArray, String outputPath)
        {
            //Note to self: when implementing this, use integers that refer to indexes on the array as "pointers"
            //Follow the instructions in the assembly as close as possible
            //Use labels and gotos for the parts

            uint sourcePointer = (uint) startAddress + 8; //a0 in the asm, skips 8 to avoid the ATP file header


            //set up a few variables to take place of some regiseters.
            //I have no idea what these registers were being used for but they were very clearly being used
            
            uint v0 = 0x0;
            uint v1 = 0x0;
            
            uint a2 = 0x0;

            //kinda stupid way for getting around the fact that source pointer will eventually be out of bounds for byteArray
            byte[] copy = new byte[32 * 1048576]; //just casually set it to the FUCKING SIZE OF THE ENTIRE PS2 RAM
            for(int i =0; i<byteArray.Length; i++)
            {
                copy[i] = byteArray[i];
            }
            byteArray = copy;

            List<byte> destList = new List<byte>();

            //Initialization phase from the document
            byte previousByte = byteArray[sourcePointer];
            sourcePointer++;
            uint destinationPointer = 0;
            uint counter = 8;
            byte buffer;

        //The parts here will follow the parts I identified in the document
        Part1:
            //Part1
            v0 = (uint)(previousByte & 0x0001);//those goes before the if because of delay slots
            if (counter == 0)
            {

                sourcePointer++; //LOADS ALSO HAVE DELAY SLOTS BITCH!!!!!
                previousByte = byteArray[sourcePointer];
                
                counter = 8;
                v0 = (uint)(previousByte & 0x0001); //seems i glossed this over on my first scroll though the assembly
            }
            else
            {
                goto Part2;
            }

            Part2:
            //Part2
            counter--;//goes before the if because of delay slots
            if (v0 == 0x0)
            {

                sourcePointer++; //delay slot!
                buffer = byteArray[sourcePointer];
                
                previousByte = (byte)(previousByte >> 0x1);

                destinationPointer++;
                destList.Add(buffer);

                goto Part1;
            }
            else
            {
                goto Part3;
            }

            Part3:
            //Part3
            previousByte = (byte)(previousByte >> 0x1);//goes before the if because of delay slots
            if (counter == 0)
            {
                sourcePointer++; //delay slot
                previousByte = byteArray[sourcePointer];
                counter = 8;
            }
            else
            {
                goto Part4;
            }

            Part4:
            //Part4
            v0 = (uint)(previousByte & 0x0001);
            v0 = byteArray[sourcePointer]; // goes before the if because of delay slots
            if (v0 == 0x0)
            {
                goto Part7;
            }
            else
            {
                
                counter--;
                previousByte = (byte)(previousByte >> 0x1); //goes before the if because of delay slots
                if (counter != 0){
                    goto Part5;
                }

                sourcePointer++;//delay slots
                previousByte = byteArray[sourcePointer];
                counter = 8;
                goto Part5;

            }

            Part5:
            //Part5
            v0 = (uint)(previousByte & 0x0001);
            previousByte = (byte)(previousByte >> 0x1);
            counter--;
            a2 = (byte)(v0 << 0x1);// goes before the if because of delay slots
            if (counter != 0)
            {
                goto Part6;
            }
            else
            {
                sourcePointer++;
                previousByte = byteArray[sourcePointer];
                counter = 8;
                goto Part6;

            }

            Part6:
            //Part6
            v0 = (uint)(previousByte & 0x0001);
            previousByte = (byte)(previousByte >> 0x1);
            sourcePointer++;
            v1 = byteArray[sourcePointer];
            v0 = (a2 + v0);
            counter--;

            a2 = (v0 + 0x2);//before the if statement because DELAY SLOT
            if (v1 == 0)
            {
                //DELAY SLOT BABY
                v1 = 0x100;
                
            }
            goto Part9;

        Part7:
            //Part7
            sourcePointer++;
            sourcePointer++;
            v1 = byteArray[sourcePointer];
            v0 = (v0 << 8);
            previousByte = (byte)(previousByte >> 0x1);
            v1 = (v0 | v1);
            counter--;//before the if statement because of delay slots
            if (v1 == 0x0)
            {
                goto Part11; //yay it returns!
            }
            else
            {
                
                a2 = (byte)(v1 & 0x000f);
                a2 = a2 + 0x2;
                if(a2 == 0x0)
                {
                    goto Part8;
                }
                else
                {
                    v1  = (v1 >> 0x4); //delay slot bby
                    goto Part9;
                }
                
            }


            Part8:
            //Part8
            sourcePointer++;
            v0 = byteArray[sourcePointer];
            v1 = (v1 >> 4);
            a2 = (v0 + 1);
            goto Part9;
            
            Part9:
            //Part9
            v1 = (destinationPointer - v1);//i think here's our culprit for where v1 is getting set to a gigantic number
            if (a2 == 0)
            {
                goto Part1;
            }
            else
            {
                
                goto Part10;
            }

        Part10:
            //Part10
            Console.WriteLine("byteArray length: " + byteArray.Length + " | v1: " + v1);
            //so for some reason v1 is getting set to some ungodly large number
            v1++;
            v0 = byteArray[v1]; //it causes right here because of that
            a2--;
            //destination[destinationPointer] = (byte)v0;
            destList.Add((byte)v0);
            
            destinationPointer++;//before the if statement cuz delay slot
            if (a2 != 0x0)
            {
                goto Part10;
            }
            else
            {
                
                goto Part1;
            }



        Part11:

            // there's a delay slot in part 11 but it shouldn't affect output
            File.WriteAllBytes(outputPath, destList.ToArray());
            return;

        }

        public static byte[] SubArray(int startAddress, int size, byte[] byteArray)
        {
            //there are better ways to slice an array, but getting it working matters more right now
            //TODO: more efficient way to get array slices
            byte[] temp = new byte[size];

            for (int i = 0; i < size; i++)
            {
                temp[i] = byteArray[startAddress + i];
            }

            return temp;
        }

        public static void DecodeATPRedux(byte[] byteArray, String outputPath)
        {
            uint v0 = 0x0;
            uint v1 = 0x0;
            uint a2 = 0x0;

            uint destinationPointer = 0;
            uint counter = 8;
            List<byte> destList = new List<byte>();

            //This is an attempt at re-doing the prior implementation using iteration
            for(uint i = 8; i<byteArray.Length; i++) //i starts at 8 in order to skip the file header
            {
                byte x = byteArray[i];
                v0 = (uint)x & 0x1;//will be set to 0 if the last bit in x is a 0, with otherwise by set to 1
                if (counter == 0)
                {
                    counter = 8;
                    continue;
                }

                counter--;
                if(v0 == 0)
                {
                    

                }

                //stuff about counter
                //stuff about checking if the last bit in a byte is 0
            }


        }


    }
}
