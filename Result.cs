using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Playfair_Cipher
{
    class Result
    {
        public static double[,] Abs(double[,] digraphScore)     //returns the abolute value of the digraph
        {
            int i, j;
            for (i = 0; i < 25; i++)
            {
                for (j = 0; j < 25; j++)
                {
                    digraphScore[i, j] = Math.Abs(digraphScore[i, j]);      //returns the absolute value of each digraphDcore element
                }
            }
            return digraphScore;
        }
        
        public static double FinalScoreMult(double[,] cipherDigraph, double[,] bookDigraph)     //Sums the product of each element of the two digraphs. 
        {
            double finalScore2 = 0;
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    finalScore2 += (cipherDigraph[i, j] * bookDigraph[i, j]);
                    //Console.WriteLine("finalScore2: " + finalScore2);
                }
            }
            return finalScore2;
        }
    }
}
