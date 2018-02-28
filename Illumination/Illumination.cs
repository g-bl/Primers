using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primers
{
    class Illumination
    {
        private static string inputFileName = "input_6.txt"; // http://primers.xyz/3
        private static char delimiter = ',';

        public class Interrupteur
        {
            public int id;
            public List<int> ampoules;
        }

        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), inputFileName);

            if (File.Exists(inputFilePath))
            {
                // INPUT LOADING
                int nbAmpoules = -1;
                int nbInterrupteurs = -1;
                List<Interrupteur> interrupteurs = new List<Interrupteur>();
                bool[] ampoules;

                string[] lines = File.ReadAllLines(inputFilePath);

                string[] firstLine = lines[0].Split(delimiter);
                int.TryParse(firstLine[0], out nbAmpoules);
                int.TryParse(firstLine[1], out nbInterrupteurs);

                ampoules = new bool[nbAmpoules];

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] splittedLine = lines[i].Split(delimiter);

                    if (splittedLine.Length > 1)
                    {
                        List<int> amps = new List<int>();
                        for (int j = 1; j < splittedLine.Length; j++)
                        {
                            int ampID = -1;
                            if (int.TryParse(splittedLine[j], out ampID))
                                amps.Add(ampID);
                        }

                        Interrupteur interrupteur = new Interrupteur()
                        {
                            id = (i - 1),
                            ampoules = amps
                        };
                        interrupteurs.Add(interrupteur);
                    }

                    //ampoules[i - 1] = false; // Les ampoules sont toutes off
                }

                // SOLVING
                List<Interrupteur> finalInterrupteurs = new List<Interrupteur>();

                while (interrupteurs.Count > 0)
                {
                    Interrupteur nextInterrupteur = interrupteurs.Aggregate((i1, i2) => (FutureIllumination(i1, ampoules) > FutureIllumination(i2, ampoules)) ? i1 : i2);
                    finalInterrupteurs.Add(nextInterrupteur);

                    interrupteurs.Remove(nextInterrupteur);
                    foreach (int amp in nextInterrupteur.ampoules)
                    {
                        ampoules[amp] = true;
                    }
                }

                // OUTPUT
                foreach (Interrupteur interrupteur in finalInterrupteurs)
                {
                    System.Diagnostics.Debug.WriteLine(interrupteur.id);
                }
                System.Diagnostics.Debug.WriteLine("#interrupteur: " + finalInterrupteurs.Count);
            }
            else
                System.Diagnostics.Debug.WriteLine("Error: missing file.");
        }

        static int FutureIllumination(Interrupteur interrupteur, bool[] ampoules)
        {
            bool[] potentialAmpoules = ampoules;
            foreach (int amp in interrupteur.ampoules)
            {
                potentialAmpoules[amp] = true;
            }
            int total = 0;
            foreach (bool on in potentialAmpoules)
            {
                if (on) total++;
            }
            return total;
        }
    }
}