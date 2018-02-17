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
                bool[][] alreadyTakenPizzaPieces;

                int pizzaWidth = -1;
                int pizzaHeight = -1;
                int minHamPiecesRequired = -1;
                int maxPiecesInAPart = -1;

                string[] lines = File.ReadAllLines(inputFilePath);

                if (lines.Length > 0)
                {
                    string[] splittedLine = lines[0].Split(delimiter);

                    if (splittedLine.Length < 4
                        || !int.TryParse(splittedLine[0], out pizzaWidth)
                        || !int.TryParse(splittedLine[1], out pizzaHeight)
                        || !int.TryParse(splittedLine[2], out minHamPiecesRequired)
                        || !int.TryParse(splittedLine[3], out maxPiecesInAPart))
                    {
                        System.Diagnostics.Debug.WriteLine("Error: incorrect input file.");
                        Environment.Exit(0);
                    }

                    pizza = new bool[pizzaHeight][];
                    alreadyTakenPizzaPieces = new bool[pizzaHeight][];

                    for (int i = 0; i < pizzaHeight; i++)
                    {
                        pizza[i] = new bool[pizzaWidth];
                        alreadyTakenPizzaPieces[i] = new bool[pizzaWidth];
                        char[] charsOnTheLine = lines[i + 1].ToCharArray();

                        for (int j = 0; j < pizzaWidth; j++)
                        {
                            pizza[i][j] = (charsOnTheLine[j].Equals(ham));
                            alreadyTakenPizzaPieces[i][j] = false;
                        }
                    }

                    // SOLVING
                    // Searching for slices with (dynamic) size (biggest possible) and at least the required number of ham pieces

                    List<PizzaPart> pizzaParts = new List<PizzaPart>();

                    // List all possible shapes,
                    List<ValidShape> shapes = new List<ValidShape>();

                    for (int w = 1; w <= maxPiecesInAPart; w++)
                    {
                        for (int h = 1; h <= maxPiecesInAPart; h++)
                        {
                            if ((w * h) <= maxPiecesInAPart)
                                shapes.Add(new ValidShape()
                                {
                                    width = w,
                                    height = h
                                });
                        }
                    }

                    IEnumerable<ValidShape> validShapes = shapes.OrderBy(shape => shape.Size()).Reverse(); // and sort them.

                    // Searching for slices
                    foreach(ValidShape shape in validShapes)
                    {
                        for (int shapeOriginY = 0; shapeOriginY <= (pizzaHeight - shape.height); shapeOriginY++)
                        {
                            for (int shapeOriginX = 0; shapeOriginX <= (pizzaWidth - shape.width); shapeOriginX++)
                            {
                                // Take a look at this pizza part
                                bool squareIsValid = true;
                                int totalHam = 0;

                                for (int y = shapeOriginY; y < (shapeOriginY + shape.height); y++)
                                {
                                    for (int x = shapeOriginX; x < (shapeOriginX + shape.width); x++)
                                    {
                                        // The square must not include a pizza piece already taken.
                                        if (alreadyTakenPizzaPieces[y][x])
                                        {
                                            squareIsValid = false;
                                            break;
                                        }
                                        else if (pizza[y][x])
                                            totalHam++;
                                    }
                                    if (!squareIsValid)
                                        break;
                                }

                                // If valid, removes the square and updates the operations list
                                if (squareIsValid && totalHam >= minHamPiecesRequired)
                                {
                                    for (int y = shapeOriginY; y < (shapeOriginY + shape.height); y++)
                                    {
                                        for (int x = shapeOriginX; x < (shapeOriginX + shape.width); x++)
                                        {
                                            alreadyTakenPizzaPieces[y][x] = true;
                                        }
                                    }

                                    pizzaParts.Add(new PizzaPart()
                                    {
                                        x = shapeOriginX,
                                        y = shapeOriginY,
                                        width = shape.width,
                                        height = shape.height
                                    });
                                }
                            }
                        }
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

                    System.Diagnostics.Debug.WriteLine("#parts: " + pizzaParts.Count);
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

    class ValidShape
    {
        public int width;
        public int height;

        public int Size() // number of pieces
        {
            return (width * height);
        }
    }
}
