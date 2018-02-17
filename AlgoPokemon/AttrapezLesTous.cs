using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Primers
{
    class Pokemon
    {
        public string name;
        public List<int> attacks;
    }

    class AttrapezLesTous
    {
        private static string inputFileName = "input_3.txt"; // http://primers.xyz/3
        private static char delimiter = ',';

        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), inputFileName);

            if (File.Exists(inputFilePath))
            {
                // INPUT LOADING
                List<Pokemon> pokemons = new List<Pokemon>();

                string[] lines = File.ReadAllLines(inputFilePath);

                foreach (string line in lines)
                {
                    string[] splittedLine = line.Split(delimiter);

                    if (splittedLine.Length > 1)
                    {
                        List<int> atks = new List<int>();
                        int atk = 0;
                        for (int j = 1; j < splittedLine.Length; j++)
                        {
                            if (int.TryParse(splittedLine[j], out atk))
                                atks.Add(atk);
                        }

                        pokemons.Add(new Pokemon()
                        {
                            name = splittedLine[0],
                            attacks = atks
                        });
                    }
                }

                // SOLVING
                List<Pokemon> team = new List<Pokemon>();
                List<int> atksPool = new List<int>();

                for (int i = 0; i < 6; i++)
                {
                    // First, removes the last pokemon chosen in team from the available pokemons list.
                    if (team.Count > 1)
                        pokemons.Remove(team.Last<Pokemon>());

                    // Compares all the available pokemons based on the union of their attacks and the team's attacks.
                    team.Add(pokemons.Aggregate((p1, p2) => (atksPool.Union(p1.attacks).ToList().Count > atksPool.Union(p2.attacks).ToList().Count) ? p1 : p2));

                    // Finally, updates the union of chosen Pokemons' atks
                    atksPool = atksPool.Union(team.Last<Pokemon>().attacks).ToList();
                }

                // OUTPUT
                System.Diagnostics.Debug.WriteLine("Best team:");
                foreach (Pokemon pokemon in team)
                {
                    System.Diagnostics.Debug.WriteLine(pokemon.name);
                }
                System.Diagnostics.Debug.WriteLine("#atks: " + atksPool.Count);
            }
            else
                System.Diagnostics.Debug.WriteLine("Error: missing file.");
        }
    }
}
