using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DoubleQuotesChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            string openingPlaceholder = "--{DEPLOY PLACEHOLDER}EXEC sp_executesql N'";
            string closingPlaceholder = "--{DEPLOY PLACEHOLDER}'";
            string find = "'";
            string replace = "''";
            int difLenght = replace.Length - find.Length;
            Console.WriteLine("Reading file...");
            string text = File.ReadAllText("Update.sql");
            //var aStringBuilder = new StringBuilder(text);
            bool cont = true;
            int start = 0;
            int openingIndexFound = 0;
            int closingIndexFound = 0;
            int quoteindexFound = 0;
            int quoteCounter = 0;
            bool opening = false;
            Console.WriteLine("Starting replacement... Total length: " + text.Length);
            Console.WriteLine();
            while (cont)
            {
                if (!opening)
                {
                    openingIndexFound = text.IndexOf(openingPlaceholder, start, StringComparison.Ordinal);
                    if (openingIndexFound == -1)
                    {
                        cont = false;
                    }
                    if (openingIndexFound < text.Length && cont)
                    {
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine("Opening found Line: " + openingIndexFound);
                        opening = true;
                        start = openingIndexFound + openingPlaceholder.Length;
                        Console.ReadKey();
                    }
                }
                else
                {
                    closingIndexFound = text.IndexOf(closingPlaceholder, start, StringComparison.Ordinal);
                    
                    Console.WriteLine("Line: " + start + " - closing in: " + closingIndexFound);
                    Console.ReadKey();
                    if (closingIndexFound == -1)
                    {
                        Console.WriteLine("Error: Was not able to find closing placeholder !!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        cont = false;
                    }
                    else
                    {
                        Console.WriteLine("Found end in:" + closingIndexFound);
                        Console.WriteLine("Looking for quotes");
                        while (opening && start < closingIndexFound)
                        {
                            Console.WriteLine("start: {0} closingIndexFound: {1}", start, closingIndexFound);
                            quoteindexFound = text.IndexOf(find, start, StringComparison.Ordinal);
                            if (quoteindexFound == -1)
                            {
                                // Not able to find quotes at all - finish the whole process
                                opening = false;
                                cont = false;
                            }
                            else
                            {
                                start = quoteindexFound;
                                if (quoteindexFound < closingIndexFound)
                                {
                                    text =text.Remove(quoteindexFound, find.Length);
                                    text = text.Insert(quoteindexFound, replace);
                                    closingIndexFound = closingIndexFound + difLenght;
                                    start = start + difLenght+1;
                                    Console.WriteLine("Line: " + start + " - closing in: " + closingIndexFound);
                                    quoteCounter++;
                                }
                                else
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Closing");
                                    opening = false;
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Total replacements: "+ quoteCounter);
            }
            Console.WriteLine("Saving");
            File.WriteAllText("UpdateMod.sql", text);
            Console.WriteLine("Press a key");
            Console.ReadKey();
        }
    }
}
