using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Playfair_Cipher
{
    class Crack     /*This crack class  models the entire solution of a playfair cipher, without the requirement of being passed the correct key.
                     It swaps every letter in a key with every other, and keeps the one which gives the highest score. It also performs column 
                     and row swaps and finds the one which gives the best solution*/
    {
        static Random s = new Random();
        int rowPrint, colPrint;

        public static char[][] ReturnSoln(char[][] child, string cipher, double[,] bookDigraph)
        {
            bool end = false;
            Digraph childDigraph = new Digraph();
            string newDecMsg = Decryption.Result(cipher, child);
            double[,] newDecMsgDigraph = childDigraph.NormalisedPairCount(newDecMsg);
            
            int cycle_ReturnSoln = 0;
            char[][] temp = Key.RandKey(Key.randAlphabet());
            double childScore = Result.FinalScoreMult(newDecMsgDigraph, bookDigraph);

            while (!end)            /*In this while loop, I chose to call methods to perform each function, as opposed to having all the 
                                     operations being internal. This meant that the method was easiers to test, maintain and understand.*/
            {
                Console.WriteLine("childScore in ReturnSoln: " + childScore + "cycle_ReturnSoln" + cycle_ReturnSoln);
                Console.WriteLine("temp key: ");
                Program.PrintKey(temp);         //Prints temp key
                Console.WriteLine("child key: ");
                Program.PrintKey(child);        //Prints child key

                temp = SwapAllPairs(child, cipher, bookDigraph);                //Swaps all possible pairs
                child = temp;
                
                newDecMsg = Decryption.Result(cipher, child);                   //generates decoded message to be used to create digraph
                newDecMsgDigraph = childDigraph.NormalisedPairCount(newDecMsg); //creates digraph to be used to find score
                childScore = Result.FinalScoreMult(newDecMsgDigraph, bookDigraph);  //calculates the score of child to be used to analyse the quality of the child key

                temp = FindBestRow(child, cipher, bookDigraph); //swap all rows and returns best
                child = temp;

                temp = SwapAllPairs(child, cipher, bookDigraph);                //Swaps all possible pairs
                child = temp;

                newDecMsg = Decryption.Result(cipher, child);                   //generates decoded message to be used to create digraph
                newDecMsgDigraph = childDigraph.NormalisedPairCount(newDecMsg); //creates digraph to be used to find score
                childScore = Result.FinalScoreMult(newDecMsgDigraph, bookDigraph);  //calculates the score of child to be used to analyse the quality of the child key

                temp = FindBestCol(child, cipher, bookDigraph); //swap all columns and returns best
                child = temp;

                temp = SwapAllPairs(child, cipher, bookDigraph);                //Swaps all possible pairs
                child = temp;

                newDecMsg = Decryption.Result(cipher, child);                   //generates decoded message to be used to create digraph
                newDecMsgDigraph = childDigraph.NormalisedPairCount(newDecMsg); //creates digraph to be used to find score
                childScore = Result.FinalScoreMult(newDecMsgDigraph, bookDigraph);  //calculates the score of child to be used to analyse the quality of the child key

                cycle_ReturnSoln++;

                if (childScore > 100000000)     //checks to see if score is good enough to assume correct key
                {
                    end = true;
                }

                if (cycle_ReturnSoln > 10)
                {
                    temp = Key.RandKey(Key.randAlphabet());     //sets temp equal to new random key
                    cycle_ReturnSoln = 0;
                }
            }
            
            return child;
        }

        public static char[][] SwapAllPairs(char[][] child, string cipher, double[,] bookDigraph) //Always returns the best pair swap
        {
            Digraph digraphCipher = new Digraph();
            int col_a = 1, col_b = 0, row_a = 0, row_b = 0;
            char[][] temp = child;
            double tempScore = Result.FinalScoreMult(digraphCipher.NormalisedPairCount(Decryption.Result(cipher, temp)), bookDigraph);

            string decryptedText;
            double[,] decryptedTextDigraph;

            decryptedText = Decryption.Result(cipher, child);                   //generates decoded message to be used to create digraph
            decryptedTextDigraph = digraphCipher.NormalisedPairCount(decryptedText); //creates digraph to be used to find score
            double childScore = Result.FinalScoreMult(decryptedTextDigraph, bookDigraph);  //calculates the score of child to be used to analyse the quality of the child key

            /*the for loop below swaps every possible combination of letter swaps and keeps the combination which gives the best score*/
            for (col_a = 0; col_a < 5; col_a++)
            {
                for (row_a = 0; row_a < 5; row_a++)
                {
                    for (col_b = 0; col_b < 5; col_b++)
                    {
                        for (row_b = 0; row_b < 5; row_b++)
                        {
                            temp = SwapPair(row_a, col_a, row_b, col_b, child);     //makes swap on specific paire

                            decryptedText = Decryption.Result(cipher, temp);        //decrypts cipher
                            decryptedTextDigraph = digraphCipher.NormalisedPairCount(decryptedText);        //finds digraph for decrypted txt
                            tempScore = Result.FinalScoreMult(decryptedTextDigraph, bookDigraph);           //finds the new score of the temp key

                            if ((tempScore > childScore))
                            {
                                childScore = tempScore;         //childScore is always the highest score
                            }
                            else
                            {
                                temp = SwapPair(row_a, col_a, row_b, col_b, child); //Swap back
                            }
                            child = temp;     //Child always set to the best score

                        }
                    }
                }
            }

            return child;
        }

        public static char[][] FindBestRow(char[][] child, string cipher, double[,] bookDigraph)        /*this method takes in a key and the cipher text, 
                                                                                                         * and performs every possible row swap, and returns 
                                                                                                         * the one which corresponds to the highest score*/
        {
            Digraph digraphCipher = new Digraph();
            char[][] temp = Key.RandKey(Key.randAlphabet());
            string decryptedText = Decryption.Result(cipher, temp);                             //generates decoded message to be used to create digraph
            double[,] decryptedTextDigraph = digraphCipher.NormalisedPairCount(decryptedText);  //creates digraph to be used to find score
            double tempScore = Result.FinalScoreMult(decryptedTextDigraph, bookDigraph);        //calculates the score of child to be used to analyse the quality of the temp key
            
            decryptedText = Decryption.Result(cipher, child);                                   //generates decoded message to be used to create digraph
            decryptedTextDigraph = digraphCipher.NormalisedPairCount(decryptedText);            //creates digraph to be used to find score
            double childScore = Result.FinalScoreMult(decryptedTextDigraph, bookDigraph);       //calculates the score of child to be used to analyse the quality of the child key
            

            for (int row_a = 0; row_a < 5; row_a++)
            {
                for (int row_b = 0; row_b < 5; row_b++)
                {
                    temp = SwapSpecificRow(row_a, row_b, child);                                    //swap rows
                    decryptedText = Decryption.Result(cipher, temp);                                //decrypts cipher
                    decryptedTextDigraph = digraphCipher.NormalisedPairCount(decryptedText);        //finds digraph for decrypted txt
                    tempScore = Result.FinalScoreMult(decryptedTextDigraph, bookDigraph);           //finds the new score of the temp key
                    

                    if (tempScore > childScore)
                    {
                        childScore = tempScore;                         //childScore is always the highest score
                    }
                    else
                    {
                        temp = SwapSpecificRow(row_a, row_b, child);    //Swap rows back
                    }

                    child = temp;                                       //child always set to the best score
                }
            }
            return child;
        }

        public static char[][] FindBestCol(char[][] child, string cipher, double[,] bookDigraph)        /*this method takes in a key and the cipher text, 
                                                                                                         * and performs every possible column swap, and returns 
                                                                                                         * the one which corresponds to the highest score*/
        {
            Digraph digraphCipher = new Digraph();
            double childScore = Result.FinalScoreMult(digraphCipher.NormalisedPairCount(Decryption.Result(cipher, child)), bookDigraph);
            char[][] temp = Key.RandKey(Key.randAlphabet());        //Initialises random new key
            double tempScore = 0;

            for (int col_a = 0; col_a < 5; col_a++)
            {
                for (int col_b = 0; col_b < 5; col_b++)
                {
                    temp = SwapSpecificCol(col_a, col_b, child);    //swap columns
                    tempScore = Result.FinalScoreMult(digraphCipher.NormalisedPairCount(Decryption.Result(cipher, temp)), bookDigraph);

                    if (tempScore > childScore) 
                    {
                        childScore = tempScore;                     //childScore is always the highest score in this method
                    }
                    else
                    {
                        temp = SwapSpecificCol(col_a, col_b, child); //Swap columns back
                    }

                    child = temp;       //child always set to the best score
                }
            }
            return child;
        }

        public static char[][] SwapSpecificRow(int row_a, int row_b, char[][] updatedKey) //Makes a specified row swap
        {
            char temp;
            int i;

            for (i = 0; i < 5; i++)
            {
                temp = updatedKey[row_a][i];
                updatedKey[row_a][i] = updatedKey[row_b][i];
                updatedKey[row_b][i] = temp;
            }

            return updatedKey;
        }

        public static char[][] SwapSpecificCol(int col_a, int col_b, char[][] updatedKey) //Makes a specified column swap
        {
            char temp;
            int i;

            for (i = 0; i < 5; i++)
            {
                temp = updatedKey[i][col_a];
                updatedKey[i][col_a] = updatedKey[i][col_b];
                updatedKey[i][col_b] = temp;
            }

            return updatedKey;
        }

        private static char[][] SwapPair(int row1, int col1, int col2, int row2, char[][] key) //swaps two specific letters
        {
            char temp_value = 'a';
            temp_value = key[row1][col1];
            key[row1][col1] = key[row2][col2];
            key[row2][col2] = temp_value;

            return key;     //returns the new key
        }

        public char[][] PrintKey(char[][] key) //prints any 5x5 key
        {
            Console.WriteLine("Key is:");
            
            for(rowPrint = 0; rowPrint < 5; rowPrint++)
            {
                for (colPrint = 0; colPrint < 5; colPrint++ )
                {
                    Console.Write(key[rowPrint][colPrint] + " ");       //Prints element followed by space
                }
                Console.WriteLine();                                    //Prints new line at the end of each row
            }
            return key;
        }
    }
}
