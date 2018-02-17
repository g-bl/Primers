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

                string[] lines = File.ReadAllLines(inputFilePath);

                if (lines.Length > 1)
                {
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
                    // Worst solver: eat some cells by id ASC

                    List<Cell> eatenCells = new List<Cell>();
                    
                    eatenCells.AddRange(cells.Take(10));

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
    }
    
    class Cell
    {
        public int id;
        public int x;
        public int y;
    }
}
