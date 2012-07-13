using System;
using System.IO;
using System.Threading;
using System.Security.Permissions;

namespace Playfair_Cipher
{
    class Program
    {
        private static bool end;
        private static bool addressUser;

        static void Main(string[] args) // <operation> <key> <plaintext address> <destination address>      <= for encrypt
                                        // <operation> <plaintext address> <destination address>            <= for digraph
                                        // <operation> <book digraph address> <cipher address>              <= for crack
        {
            try                         //try catch to manage exceptions. If any exceptions are thrown, they are picked up in catch by exception handler 'e'. 
            {
                if (args[0][0] == 'E' || args[0][0] == 'e')             //checks if first arg begins with e. If so, runs encrypt
                {
                    string key = args[1];                               //key passed in second argument
                    char[][] keyArray = Key.RandKey(key);               //converts key as string into array
                    string plain_text = File.ReadAllText(args[2]);      //reads in plain text
                    Console.WriteLine("You typed in the following key:" + key);
                    string paddedText = Preprocessor.Pad(Preprocessor.Prepare(plain_text)); //prepares text for digraph analysis
                    string encrypted_plain_text = Encryption.Result(paddedText, keyArray);  //encrypts user defined text with user defined key (keyArray)
                    Console.WriteLine("Your key:");
                    PrintKey(keyArray);
                    Console.WriteLine("Encrypted text: " + encrypted_plain_text);
                    File.WriteAllText(args[3], encrypted_plain_text);                       //Writes encrypted text to a user-defined directory
                }
                else if (args[0][0] == 'D' || args[0][0] == 'd')                //checks if first arg begins with d. If so, runs digraph
                {
                    Digraph newDigraph = new Digraph();                                                 //creates a new digraph
                    string plain_text = File.ReadAllText(args[1]);                                      //reads in plain text from user defined directory
                    string padded_plain_text = Preprocessor.Pad(Preprocessor.Prepare(plain_text));      //prepares plain text for digraph analysis
                    double[,] digraph = newDigraph.PairCount(padded_plain_text);              //creates digraph for plain text
                    try
                    {
                        Console.WriteLine("This is your digraph: ");
                        Console.WriteLine(Digraph.Print(digraph));
                        WriteDigraph(args[2], digraph);             //writes digraph to user defined directory as .dig file
                        Console.WriteLine("Digraph written to " + args[2]);
                    }
                    catch (Exception eDigraph)                      //handles writing exceptions, i.e. cannot write to directory
                    {
                        Console.WriteLine("Cannot write digraph!");
                        Console.WriteLine("Error:" + eDigraph.Message);
                    }
                }
                else if (args[0][0] == 'C' || args[0][0] == 'c')    //checks if first arg begins with c. If so, runs crack
                {
                    string cipher = File.ReadAllText(args[2]);      //reads cipher text from user defined directory
                    Console.WriteLine("Length of cipher: " + cipher.Length);

                    double[,] bookDigraph = ReadDigraph(args[1]);   //Loads digraph from second argument
                    Console.WriteLine("Your loaded digraph: ");
                    Console.WriteLine(Digraph.Print(bookDigraph));  //Prints digraph from second argumen

                    var parent = Key.RandKey(Key.randAlphabet());   //Initialises parent
                    var child = Key.RandKey(Key.randAlphabet());    //Initialises child

                    Digraph digraphCipher = new Digraph();
                    Console.WriteLine(Convert.ToChar(7));           //Sound to indicate end of loading sequence

                    double parentScore;                             //definitions
                    int cycle = 0;
                    end = false;
                    addressUser = true;

                    while (!end)
                    {
                        string decryptedText = Decryption.Result(cipher, parent);                                   //decrypts cipher according to parent key
                        double[,] decryptedTextDigraph = digraphCipher.NormalisedPairCount(decryptedText);          //creates digraph of decrypted cipher
                        parentScore = Result.FinalScoreMult(decryptedTextDigraph, bookDigraph);                     //creates score of user-defined cipher digraph
                        child = Key.RandKey(Key.randAlphabet());                                        //makes new random key
                        parent = Crack.ReturnSoln(child, cipher, bookDigraph);                          //IMPORTANT: Should return solution - NOT WORKING PROPERLY, but still some functionality
                        decryptedText = Decryption.Result(cipher, parent);                              //decrypts according to parent key
                        decryptedTextDigraph = digraphCipher.NormalisedPairCount(decryptedText);        //creates digraph according to parent key
                        parentScore = Result.FinalScoreMult(decryptedTextDigraph, bookDigraph);         //calculates score from this digraph
                        
                        while (addressUser)                     //loop for checking if finished
                        {
                            
                            if (parentScore > 100000000)        //number should be value of parentScore which indicates a solution has been found
                            {
                                end = true;                     //ends crack state
                            }
                            else
                            {
                                CheckDoneCrack(cipher, parent); //asks user whether solution is correct
                            }
                        }
                        
                        cycle++;        //increments cycle

                        File.WriteAllText("crack.sol", decryptedText);
                        Console.WriteLine("The decrypted text has been saved as: crack.sol.");
                    }
                    
                    
                }
            }
            catch(Exception e)                                  //exteption handler e handles exceptions
            {
                Console.WriteLine("There has been an error!");
                Console.WriteLine("Error:" + e.Message);        //prints exception onto console
                
            }

            Console.WriteLine("Program finished");
            Console.WriteLine(Convert.ToChar(7)); Console.WriteLine(Convert.ToChar(7)); //Sound to signify end of program
            Console.ReadKey();
        }

        public static char[][] Equate(char[][] key1, char[][] key2) //sets key 1 equal to key 2
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    key1[i][j] = key2[i][j];                //sets individual elements of key 1 equal to key 2
                }
            }
            return key1;
        }

        public static char[][] PrintKey(char[][] key)       //prints key in console as a 5x5 array
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(" " + key[i][j]);
                }
                Console.WriteLine();                        //inserts a new line at the end of each row
            }
            return key;
        }

        private static void WriteDigraph(string digraphFile, double[,] digraph)     //uses StreamWriter to write digraph to file
        {
            var digraphWriter = new StreamWriter(digraphFile);      //creates new StreamWriter

            digraphWriter.WriteLine();                              //inserts new line into file
            for (int i = 0; i <= 25; i++)
            {
                for (int j = 0; j <= 25; j++)
                {
                    digraphWriter.Write(digraph[i, j] + "\t");      //writes each element into the file followed by a tab
                }
                digraphWriter.WriteLine();              //inserts a new line at the end of each row
            }
            digraphWriter.WriteLine();      //inserst blank line at end
            digraphWriter.Close();          //closes instance of StreamWriter
        }

        private static double [,] ReadDigraph(string file)      //Reads a digraph from a file, by splitting into elements, numbers either side of a tab
            { 
                double[,] bookDigraph = new double[26,26];        //creates new uninitialised digraph
                string read = File.ReadAllText(file);             //reads data from file into string
                string[] number = read.Split('\t');               //puts each element ino new element of a one dimensional string array
                var k = 0;
                for(var i =0; i<=25; i++)
                {
                    for(var j = 0; j <= 25; j++)
                    {
                        bookDigraph[i, j] = Convert.ToDouble(number[k]);        //allocates the value of each string element to the array element
                        k++;
                    }
                }
            return bookDigraph;
            }

        private static void CheckDoneCrack(string cipher, char[][] parent)      //prompts user to check if decrypted text is correct
        {
            Console.WriteLine("Is this the correct plain text? (y/n)");
            Console.WriteLine(Decryption.Result(cipher, parent));       //writes decrypted text
            string userInput = Console.ReadLine();
            if (userInput[0] == 'y' || userInput[0] == 'Y')             //check to see if user answered yes/y
            {
                end = true;                                             //program terminated after returning to Main() 
                addressUser = false;                                    //stops prompting user
                Console.WriteLine("The key for your cipher is: ");
                PrintKey(parent);                                       //prints key for cipher
            }
            else if (userInput[0] == 'n' || userInput[0] == 'N')        //check to see if user answered no/n
            {
                end = false;                                            //continues improving key
                addressUser = false;                                    //stops prompting user
            }
            else
            {
                Console.WriteLine("Please enter 'y' of 'n' (without inverted commas)");
                end = false;                                            //continues improving key
                addressUser = true;                                     //asks user again
            }
        }
    }
}
