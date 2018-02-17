using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primers
{
    class PizzaGoogle
    {
        private static string inputFileName = "input_7.txt"; // http://primers.xyz/7
        private static string outputFileName = "output.txt";
        private static char delimiter = ',';
        private static char ham = 'H';

        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), inputFileName);

            if (File.Exists(inputFilePath))
            {
                // INPUT LOADING
                bool[][] pizza; // true if ham

                int pizzaWidth = -1;
                int pizzaHeight = -1;
                int minHamPiecesRequired = -1;
                int maxPiecesInAPart = -1;

                string[] lines = File.ReadAllLines(inputFilePath);

                if (lines.Length > 0)
                {
                    string[] splittedLine = lines[0].Split(delimiter);

                    if (splittedLine.Length < 1
                        || !int.TryParse(splittedLine[0], out pizzaWidth)
                        || !int.TryParse(splittedLine[1], out pizzaHeight)
                        || !int.TryParse(splittedLine[2], out minHamPiecesRequired)
                        || !int.TryParse(splittedLine[3], out maxPiecesInAPart))
                    {
                        System.Diagnostics.Debug.WriteLine("Error: incorrect input file.");
                        Environment.Exit(0);
                    }

                    pizza = new bool[pizzaHeight][];

                    for (int i = 0; i < pizzaHeight; i++)
                    {
                        pizza[i] = new bool[pizzaWidth];
                        char[] charsOnTheLine = lines[i + 1].ToCharArray();

                        for (int j = 0; j < pizzaWidth; j++)
                        {
                            pizza[i][j] = (charsOnTheLine[j].Equals(ham));
                        }
                    }

                    // SOLVING
                    // Worst solver ever: first 3 ham pieces aligned will do the job

                    List<PizzaPart> pizzaParts = new List<PizzaPart>();
                    bool terminated = false;

                    for (int y = 0; y < pizzaHeight - 3; y++)
                    {
                        for (int x = 0; x < pizzaWidth - 3; x++)
                        {
                            if (pizza[y][x] && pizza[y][x + 1] && pizza[y][x + 2])
                            {
                                pizzaParts.Add(new PizzaPart()
                                {
                                    x = x,
                                    y = y,
                                    width = 3,
                                    height = 1
                                });
                                terminated = true;
                                break;
                            }
                        }
                        if (terminated)
                            break;
                    }

                    // OUTPUT
                    System.Diagnostics.Debug.WriteLine("Writing output pizza parts to file...");

                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), outputFileName)))
                    {
                        foreach (PizzaPart part in pizzaParts)
                        {
                            outputFile.WriteLine(part.ToString());
                        }
                    }
                }
            }
            else
                System.Diagnostics.Debug.WriteLine("Error: missing file.");
        }
    }

    class PizzaPart
    {
        public int x;
        public int y;
        public int width;
        public int height;

        public override string ToString()
        {
            return String.Format("{0},{1},{2},{3}", x, y, width, height);
        }
    }
}
