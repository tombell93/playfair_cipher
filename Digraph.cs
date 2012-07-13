using System;

namespace Playfair_Cipher
{
    class Digraph
    {

        int firstLetter, secondLetter;
        static int rowPrint;
        static int colPrint;

        public double[,] PairCount(string text) //counts the number of times specific pairs of letters are seen next to each other
        {
            double[,] pairCount = new double[26, 26];
            for (int letter = 0; letter < text.Length - 1; letter++)
            {
                firstLetter = Convert.ToInt16(text[letter]) - 65; //the letter A in ASCII is 65, so -65 puts the letters of the alphabet in order, i.e. A = 0, B = 1 etc.
                secondLetter = Convert.ToInt16(text[letter + 1]) - 65;
                if (firstLetter > 9)
                {
                    firstLetter--;     //this removes the effect of J from the alphabet. i.e. since J is the 9th letter in the alphabet
                }
                if (secondLetter > 9)
                {
                    secondLetter--;
                }
                pairCount[firstLetter, secondLetter]++;
            }
            return pairCount;
        }

        public double[,] NormalisedPairCount(string text) //counts the number of times specific pairs of letters are seen next to each other
        {
            double[,] pairCount = new double[26, 26];
            for (int letter = 0; letter < text.Length - 1; letter++)
            {
                firstLetter = Convert.ToInt16(text[letter]) - 65; //the letter A in ASCII is 65, so -65 puts the letters of the alphabet in order, i.e. A = 0, B = 1 etc.
                secondLetter = Convert.ToInt16(text[letter + 1]) - 65;
                if (firstLetter > 9)
                {
                    firstLetter--;     //this removes the effect of J from the alphabet. i.e. since J is the 9th letter in the alphabet
                }
                if (secondLetter > 9)
                {
                    secondLetter--;
                }
                pairCount[firstLetter, secondLetter]++;
            }

            double length = text.Length;

            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    pairCount[i, j] = pairCount[i, j] / length;             //Normalises digraph by dividing by the length of the text
                }
            }
            return pairCount;
        }

        public static double[,] Print(double[,] pairCount) //prints pairCount
        {
            Console.WriteLine("   A  B  C  D  E  F  G  H  I  K  L  M  N  O  P  Q  R  S  T  U  V  W  X  Y  Z");          //prints column headers

            for (rowPrint = 0; rowPrint < 25; rowPrint++)
            {
                char colLetter = Convert.ToChar(rowPrint + 65);             //converts row header letters from integer representation to ASCII char. 
                if (rowPrint > 8)
                {
                    colLetter++;
                }
                Console.Write(colLetter);                               //prints row headers
                for (colPrint = 0; colPrint < 25; colPrint++)
                {
                    int numb = Convert.ToInt32(pairCount[rowPrint, colPrint]);      //prints digraph element values at each point
                    if(numb < 10)
                    {
                        Console.Write("  " + numb);                 //prints values
                    }
                    else
                    {
                        Console.Write(" " + numb);
                    }
                }
                Console.WriteLine();
            }
            return pairCount;
        }
    }
}
