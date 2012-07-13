using System;

namespace Playfair_Cipher
{
    class Decryption
    {
        private int length, i, j, indexRow, indexCol, rowA, rowB, colA, colB;
        private string decodedPair = "", decodedText = "";

        readonly char[][] keyArray = Key.RandKey(Key.randAlphabet()); /*I used an array of arrays here, in order to enable the use of Array.IndexOf
                                                                        * as required, and for ease of indexing in for loops*/

        public int Length(string padded)                                    //returns the length of the string 'padded'
        {
            length = padded.Length;
            return length;
        }

        public string PairDecode(string padded, char[][] key)           /*Swaps a single pair with their corresponding playfair 
                                                                         * letter to convert back to plaintext*/
        {
            for (j = 0; j < 2; j++)     //once for loop exits, rowA, colA, rowB and colB are the coordinates of the next pair
            {
                if (i % 2 == 0)
                {
                    rowA = FindRow(padded, key);        /*finds location of string element in key*/
                    colA = FindCol(padded, key);
                }
                else
                {
                    rowB = FindRow(padded, key);
                    colB = FindCol(padded, key);
                }
                i++;
            }

            if (rowA == rowB)       //implements same-row condition for swap
            {
                colA--;
                colB--;
                if (colA == -1)
                {
                    colA = 4;
                }
                if (colB == -1)
                {
                    colB = 4;
                }
            }
            else if (colA == colB)      //implements same-column condition for swap
            {
                rowA--;
                rowB--;
                if (rowA == -1)
                {
                    rowA = 4;
                }
                if (rowB == -1)
                {
                    rowB = 4;
                }
            }
            else
            {
                int temp = colA;        //swaps columns
                colA = colB;
                colB = temp;
            }
            decodedPair = Convert.ToString(keyArray[rowA][colA]) + Convert.ToString(keyArray[rowB][colB]);  //adds elemend to make pair string
            return decodedPair;
        }


        public int FindRow(string padded, char[][] key)     //uses indexing to find the inxed of indexRow
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

        public int FindCol(string padded, char[][] key)     //uses indexing to find the indxed of indexCol
        {
            indexCol = Array.IndexOf(key[indexRow], padded[i]);
            return indexCol;
        }

        public string DecodeString(string padded, char[][] key) //decrypts a message according to the provided key
        {
            length = Length(padded);
            for (i = 0; i < length; )
            {
                decodedPair = PairDecode(padded, key);      //decrypts individual pair according to key
                decodedText += decodedPair;
            }
            return decodedText;
        }
        public static string Result(string padded, char[][] key)            //returns the entire encrypted string
        {
            var decryptionA = new Decryption();                             //creates new encryption object
            var decryptedText = decryptionA.DecodeString(padded, key);
            return decryptedText;
        }
    }
}
