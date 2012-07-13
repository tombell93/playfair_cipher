using System;

namespace Playfair_Cipher
{
    class Encryption
    {
        private int length, i, j, indexRow, indexCol, rowA, rowB, colA, colB;
        private string encodedPair = "", encodedText = "";

                                                                    //methods are private where possible

        private int Length(string padded)                           //This function finds the length of the text
        {
            length = padded.Length;
            return length;
        }

        private string PairEncode(string padded, char[][] key)      //swaps a single pair with their corresponding playfair letters
        {
            for (j = 0; j < 2; j++)
            {

                if (i % 2 == 0)
                {
                    rowA = FindRow(padded, key);                    //finds index of letters in key
                    colA = FindCol(padded, key);
                }else{
                    rowB = FindRow(padded, key);
                    colB = FindCol(padded, key);
                }
                i++;
            }

            if (rowA == rowB)                        //rules for playfair pair swapping
            {
                colA++;
                colB++;
                if (colA == 5)
                {
                    colA = 0;
                }
                if (colB == 5)
                {
                    colB = 0;
                }
            }
            else if (colA == colB)
            {
                rowA++;
                rowB++;
                if (rowA == 5)
                {
                    rowA = 0;
                }
                if (rowB == 5)
                {
                    rowB = 0;
                }
            }
            else
            {
                int temp = colA;                      //swaps columns
                colA = colB;
                colB = temp;
            }
            encodedPair = Convert.ToString(key[rowA][colA]) + Convert.ToString(key[rowB][colB]);        //combines two encrypted letters int a string
            return encodedPair;
        }
        
        private int FindRow(string padded, char[][] key)                //uses indexing to find the inxed of indexRow
        {
            for (indexRow = 0; indexRow < 5; indexRow++)
            {
                indexCol = Array.IndexOf(key[indexRow], padded[i]);
                if (indexCol != -1)                         //returns -1 if element not round in array
                {
                    break;
                }
            }
            return indexRow;
        }

        private int FindCol(string padded, char[][] key)
        {
            indexCol = Array.IndexOf(key[indexRow], padded[i]);             //uses indexing to find the inxed of indexCol
            return indexCol;
        }

        private string EncodeString(string padded, char[][] key)            //encrypts a message according to the provided key
        {
            length = Length(padded);
            for (i = 0; i < length; )
            {
                encodedPair = PairEncode(padded, key);                      //encrypts a single pair
                encodedText += encodedPair;
            }
            return encodedText;
        }

        public static string Result(string padded, char[][] key)            //returns the entire encrypted string
        {
            var encryptionA = new Encryption();                             //creates new encryption object
            var encryptedText = encryptionA.EncodeString(padded, key);
            return encryptedText;
        }
    }
}
