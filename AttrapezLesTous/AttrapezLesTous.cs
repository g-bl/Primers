using System;
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

    class Search
    {
        public List<Pokemon> availablePokemons;
        public List<Pokemon> team;
        public List<int> atksPool;
    }

    class AttrapezLesTous
    {
        private static string inputFileName = "input_3.txt"; // http://primers.xyz/3
        private static char delimiter = ',';

        private static List<Pokemon> pokemonsDb;
        private static List<Search> searchs;

        private static bool secondChoice;

        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), inputFileName);

            if (File.Exists(inputFilePath))
            {
                // INPUT LOADING
                pokemonsDb = new List<Pokemon>();

                string[] lines = File.ReadAllLines(inputFilePath);

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] splittedLine = lines[i].Split(delimiter);

                    if (splittedLine.Length > 1)
                    {
                        List<int> atks = new List<int>();
                        int atk = 0;
                        for (int j = 1; j < splittedLine.Length; j++)
                        {
                            if (int.TryParse(splittedLine[j], out atk))
                                atks.Add(atk);
                        }

                        pokemonsDb.Add(new Pokemon()
                        {
                            name = splittedLine[0],
                            attacks = atks
                        });
                    }
                }

                // SOLVING: Random-Multiple-DFS-Glouton-DoubleChoice

                searchs = new List<Search>();
                secondChoice = false;

                for (int i = 0; i < 10000; i++)
                {
                    List<Pokemon> pkms = new List<Pokemon>(pokemonsDb);
                    Pokemon[] pkmsArray = pkms.ToArray();

                    Random rng = new Random();
                    int nbPokemonsToBeRemoved = rng.Next(1, 10);

                    for (int j = 0; j < nbPokemonsToBeRemoved; j++)
                    {
                        int pokemonId = rng.Next(0, pokemonsDb.Count);
                        if (pokemonId < pkmsArray.Length && pkmsArray[pokemonId] != null)
                        {
                            pkms.Remove(pkmsArray[pokemonId]);
                        }
                    }

                    ComputeSearch(pkms, new List<Pokemon>(), new List<int>());
                }

                // OUTPUT
                System.Diagnostics.Debug.WriteLine("#searchs: " + searchs.Count);

                Search bestSearch = searchs.Aggregate((s1, s2) => (s1.atksPool.Count > s2.atksPool.Count) ? s1 : s2);

                System.Diagnostics.Debug.WriteLine("Best team:");
                foreach (Pokemon pokemon in bestSearch.team)
                {
                    System.Diagnostics.Debug.WriteLine(pokemon.name);
                }
                System.Diagnostics.Debug.WriteLine("#atks: " + bestSearch.atksPool.Count);
            }
            else
                System.Diagnostics.Debug.WriteLine("Error: missing file.");
        }

        static void ComputeSearch(List<Pokemon> availablePokemons, List<Pokemon> team, List<int> atksPool)
        {
            if (team.Count < 6)
            {
                // First, removes the last pokemon chosen in team from the available pokemons list.
                if (team.Count > 1)
                {
                    availablePokemons.Remove(team.Last());
                }

                // Compares all the available pokemons based on the union of their attacks and the team's attacks.
                // First choice,
                Pokemon firstPokemon = availablePokemons.Aggregate((p1, p2) => (atksPool.Union(p1.attacks).ToList().Count > atksPool.Union(p2.attacks).ToList().Count) ? p1 : p2);

                List<Pokemon> firstTeam = new List<Pokemon>(team);
                firstTeam.Add(firstPokemon);
                List<int> firstAtksPool = atksPool.Union(firstTeam.Last().attacks).ToList();

                ComputeSearch(availablePokemons, firstTeam, firstAtksPool);

                // and second choice.
                if (secondChoice)
                {
                    availablePokemons.Remove(firstPokemon);
                    Pokemon secondPokemon = availablePokemons.Aggregate((p1, p2) => (atksPool.Union(p1.attacks).ToList().Count > atksPool.Union(p2.attacks).ToList().Count) ? p1 : p2);
                    availablePokemons.Add(firstPokemon);

                    List<Pokemon> secondTeam = new List<Pokemon>(team);
                    secondTeam.Add(secondPokemon);
                    List<int> secondAtksPool = atksPool.Union(secondTeam.Last().attacks).ToList();

                    ComputeSearch(availablePokemons, secondTeam, secondAtksPool);
                }
            }
            else
            {
                searchs.Add(new Search()
                {
                    availablePokemons = availablePokemons,
                    team = team,
                    atksPool = atksPool
                });
            }
        }
    }
}
