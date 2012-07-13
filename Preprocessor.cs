using System;
using System.Text.RegularExpressions;

namespace Playfair_Cipher
{
    static class Preprocessor       //class given beforehand. Prepares text for digraph analysis.
    {
        public static string Prepare(string str)            //corrects case
        {
            string upStr = str.ToUpper();
            string alphaStr = Regex.Replace(upStr, "[^A-Z]+", "");
            return alphaStr.Replace("J", "I");
        }

        public static string Pad(string str)            //pads with X etc, as required
        {
            string paddedStr = "";
            if (str.Length > 0)
            {
                int i = 0;
                while (i < str.Length)
                {
                    char fc = str[i];
                    i++;
                    char sc;
                    if (i == str.Length || str[i] == fc)
                    {
                        sc = 'X';
                    }
                    else
                    {
                        sc = str[i];
                        i++;
                    }
                    paddedStr += fc;
                    paddedStr += sc;
                }
            }
            return paddedStr;
        }
    }
}