using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Playfair_Cipher
{
    class Key
    {
        char[][] randKey = new char[][]   {
                                           new char []   {},
                                           new char []   {},
                                           new char []   {},
                                           new char []   {},
                                           new char []   {}
                                       };
        char[][] alphabet = new char[][]   {
                                           new char []   {'A', 'B', 'C', 'D', 'E'},
                                           new char []   {'F', 'G', 'H', 'I', 'K'},
                                           new char []   {'L', 'M', 'N', 'O', 'P'},
                                           new char []   {'Q', 'R', 'S', 'T', 'U'},
                                           new char []   {'V', 'W', 'X', 'Y', 'Z'}
                                       };
        public static char[] randAlphabet()     //Generates a random alphabet
        {
            var alphabet = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            var randAlph = alphabet;
            char temp;
            int randInt1 = 0;
            int randInt2 = 0;
            Random  r = new Random();

            for(int i = 0; i < 100; i++)
            {
                randInt1 = r.Next(25);
                randInt2 = r.Next(25);
                if (randInt1 == randInt2)
                {
                    if (randInt1 < 13)
                    {
                        randInt1++;
                    }
                    else
                    {
                        randInt1--;
                    }
                }
                temp = randAlph[randInt1];
                randAlph[randInt1] = randAlph[randInt2];
                randAlph[randInt2] = temp;
            }
            return randAlph;
        }

        public static char[][] RandKey(char[] randAlph)     //Converts the random alphabet into a 5x5 array (accepts char array)
        {
            char[][] randKey = new char[5][] {
                                           new char []   {'A', 'A', 'A', 'A', 'A'},
                                           new char []   {'A', 'A', 'A', 'A', 'A'},
                                           new char []   {'A', 'A', 'A', 'A', 'A'},
                                           new char []   {'A', 'A', 'A', 'A', 'A'},
                                           new char []   {'A', 'A', 'A', 'A', 'A'}
                                       };
            int index;
            index = 0;

            for(int row = 0; row < 5; row++)
            {
                for(int col = 0; col < 5; col++)
                {
                    randKey[row][col] = randAlph[index];
                    index++;
                }
            }
            return randKey;
        }

        public static char[][] RandKey(string randAlph)//Converts the random alphabet into a 5x5 array (accepts string)
        {
            int i;
            int j;
            int k = 0;
            char[][] keyArray = new char[5][] {
                                           new char []   {'A', 'A', 'A', 'A', 'A'},
                                           new char []   {'A', 'A', 'A', 'A', 'A'},
                                           new char []   {'A', 'A', 'A', 'A', 'A'},
                                           new char []   {'A', 'A', 'A', 'A', 'A'},
                                           new char []   {'A', 'A', 'A', 'A', 'A'}
                                       };
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    keyArray[i][j] = randAlph[k];
                    k++;
                }
            }
            return keyArray;
        }

    }
}
