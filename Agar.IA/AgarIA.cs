using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primers
{
    class AgarIA
    {
        private static string inputFileName = "input_2.txt"; // http://primers.xyz/2
        private static string outputFileName = "output.txt";
        private static char delimiter = ',';

        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), inputFileName);

            if (File.Exists(inputFilePath))
            {
                // INPUT LOADING
                List<Cell> cells = new List<Cell>();
                double timeLeft = -1;

                string[] lines = File.ReadAllLines(inputFilePath);

                if (lines.Length > 1)
                {
                    Double.TryParse(lines[0], out timeLeft);

                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] splittedLine = lines[i].Split(delimiter);
                        Cell cell = new Cell();

                        if (splittedLine.Length < 3
                            || !int.TryParse(splittedLine[0], out cell.id)
                            || !int.TryParse(splittedLine[1], out cell.x)
                            || !int.TryParse(splittedLine[2], out cell.y))
                        {
                            System.Diagnostics.Debug.WriteLine("Error: incorrect input file.");
                            Environment.Exit(0);
                        }

                        cells.Add(cell);
                    }

                    // SOLVING
                    // From the nearest to the nearest

                    List<Cell> eatenCells = new List<Cell>();

                    Cell me = new Cell()
                    {
                        x = 0,
                        y = 0
                    };

                    while (timeLeft > 0)
                    {
                        Cell nextVictim = cells.Aggregate((c1, c2) => (Distance(me, c1) < Distance(me, c2)) ? c1 : c2);

                        eatenCells.Add(nextVictim);
                        cells.Remove(nextVictim);

                        timeLeft -= Distance(me, nextVictim);

                        me = nextVictim;
                    }
                    eatenCells.Remove(eatenCells.Last());

                    // OUTPUT
                    System.Diagnostics.Debug.WriteLine("GET READY!");

                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), outputFileName)))
                    {
                        foreach (Cell cell in eatenCells)
                        {
                            outputFile.WriteLine(cell.id.ToString());
                        }
                    }
                }
            }
            else
                System.Diagnostics.Debug.WriteLine("Error: missing file.");
        }

        public static double Distance(Cell origin, Cell cell)
        {
            return Math.Sqrt(Math.Pow(cell.x - origin.x, 2) + Math.Pow(cell.y - origin.y, 2));
        }
    }
    
    class Cell
    {
        public int id;
        public int x;
        public int y;
    }
}
