using System;
using System.Collections.Generic;
using System.IO;

namespace Primers
{
    class ArtOptimal
    {
        private static string inputFileName = "input_0.txt"; // http://primers.xyz/0
        private static string outputFileName = "output.txt";
        private static char delimiter = ',';
        private static char filledPixel = '#';

        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), inputFileName);

            if (File.Exists(inputFilePath))
            {
                // INPUT LOADING

                // Better use an array-of-arrays than a 2d array
                // https://stackoverflow.com/questions/597720/what-are-the-differences-between-a-multidimensional-array-and-an-array-of-arrays
                bool[][] painting;
                
                int width = 0;
                int height = 0;

                string[] lines = File.ReadAllLines(inputFilePath);

                if (lines.Length > 0)
                {
                    string[] splittedLine = lines[0].Split(delimiter);

                    if (splittedLine.Length < 1 || !int.TryParse(splittedLine[0], out width) || !int.TryParse(splittedLine[1], out height))
                    {
                        System.Diagnostics.Debug.WriteLine("Error: incorrect input file.");
                        Environment.Exit(0);
                    }

                    painting = new bool[height][];

                    for (int i = 0; i < height; i++)
                    {
                        painting[i] = new bool[width];

                        for (int j = 0; j < width; j++)
                        {
                            char[] chars = lines[i+1].ToCharArray();
                            painting[i][j] = (chars[j].Equals(filledPixel));
                        }
                    }

                    // SOLVING
                    List<Operation> operations = new List<Operation>();

                    // Worst solver only uses 1x1 squares
                    for (int i = 0; i < painting.Length; i++)
                    {
                        for (int j = 0; j < painting[i].Length; j++)
                        {
                            if (painting[i][j])
                                operations.Add(new Fill()
                                {
                                    x = i,
                                    y = j,
                                    size = 1
                                });
                        }
                    }

                    // OUTPUT
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), outputFileName)))
                    {
                        foreach (Operation operation in operations)
                        {
                            outputFile.WriteLine(operation.ToString());
                        }
                    }

                    System.Diagnostics.Debug.WriteLine("#operations: " + operations.Count);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Error: incorrect input file.");
                    Environment.Exit(0);
                }
            }
            else
                System.Diagnostics.Debug.WriteLine("Error: missing file.");
        }
    }

    class Operation
    {
        public string name;
        public int x;
        public int y;

        public override string ToString()
        {
            return String.Format("{0},{1},{2}", name, x, y);
        }
    }

    class Fill : Operation
    {
        public int size;

        public Fill()
        {
            name = "FILL";
        }

        public override string ToString()
        {
            return String.Format("{0},{1}", base.ToString(), size);
        }
    }

    class Erase : Operation
    {
        public Erase()
        {
            name = "ERASE";
        }
    }
}
