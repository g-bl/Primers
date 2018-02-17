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
                bool[][] alreadyFilledPixels;

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
                    alreadyFilledPixels = new bool[height][];

                    for (int i = 0; i < height; i++)
                    {
                        painting[i] = new bool[width];
                        alreadyFilledPixels[i] = new bool[width];

                        char[] charsOnTheLine = lines[i + 1].ToCharArray();

                        for (int j = 0; j < charsOnTheLine.Length; j++)
                        {
                            painting[i][j] = (charsOnTheLine[j].Equals(filledPixel));
                            alreadyFilledPixels[i][j] = false;
                        }
                    }

                    // SOLVING
                    List<Operation> operations = new List<Operation>();

                    // Searching from big squares to little ones
                    for (int squareSize = Math.Min(width, height); squareSize > 0; squareSize--)
                    {
                        System.Diagnostics.Debug.WriteLine("squareSize: " + squareSize);

                        for (int squareOriginY = 0; squareOriginY <= (height - squareSize); squareOriginY++)
                        {
                            for (int squareOriginX = 0; squareOriginX <= (width - squareSize); squareOriginX++)
                            {
                                // Validating the square
                                bool squareIsValid = true;

                                for (int y = squareOriginY; y < (squareOriginY + squareSize); y++)
                                {
                                    for (int x = squareOriginX; x < (squareOriginX + squareSize); x++)
                                    {
                                        // The square must exclude any blank or already filled pixels
                                        if (!painting[y][x] || alreadyFilledPixels[y][x])
                                        {
                                            squareIsValid = false;
                                            break;
                                        }
                                    }
                                    if (!squareIsValid)
                                        break;
                                }

                                // If valid, removes the square and updates the operations list
                                if (squareIsValid)
                                {
                                    for (int y = squareOriginY; y < (squareOriginY + squareSize); y++)
                                    {
                                        for (int x = squareOriginX; x < (squareOriginX + squareSize); x++)
                                        {
                                            alreadyFilledPixels[y][x] = true;
                                        }
                                    }

                                    operations.Add(new Fill()
                                    {
                                        x = squareOriginX,
                                        y = squareOriginY,
                                        size = squareSize
                                    });
                                }
                            }
                        }
                    }

                    // OUTPUT
                    System.Diagnostics.Debug.WriteLine("Writing output operations to file...");

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
