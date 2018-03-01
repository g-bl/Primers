using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleHashCode
{
    class SelfDriving
    {
        private static string inputFileName = "a_example.in"; // a_example b_should_be_easy c_no_hurry d_metropolis e_high_bonus
        private static string outputFileName = "a_example.out";
        private static char delimiter = ' ';

        static void Main(string[] args)
        {
            /**************************************************************************************
             *  Input loading
             **************************************************************************************/

            Debug.WriteLine("Input loading...");

            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), inputFileName);
            string[] lines = File.ReadAllLines(inputFilePath);

            string[] firstLine = lines[0].Split(delimiter);

            int R = int.Parse(firstLine[0]); // number of rows
            int C = int.Parse(firstLine[1]); // number of columns
            int F = int.Parse(firstLine[2]); // number of vehicles
            int N = int.Parse(firstLine[3]); // number of rides
            int B = int.Parse(firstLine[4]); // per-ride bonus
            int T = int.Parse(firstLine[5]); // number of steps

            List<Ride> rides = new List<Ride>();

            for (int i = 1; i < lines.Length; i++)
            {
                string[] splittedLine = lines[i].Split(delimiter);

                Ride ride = new Ride()
                {
                    id = (i - 1),
                    Start = new Intersection()
                    {
                        r = int.Parse(splittedLine[0]),
                        c = int.Parse(splittedLine[1])
                    },
                    Finish = new Intersection()
                    {
                        r = int.Parse(splittedLine[2]),
                        c = int.Parse(splittedLine[3])
                    },
                    earliestStart = int.Parse(splittedLine[4]),
                    latestFinish = int.Parse(splittedLine[5])
                };
                rides.Add(ride);
            }

            List<Vehicle> fleet = new List<Vehicle>();

            for (int i = 0; i < F; i++)
            {
                fleet.Add(new Vehicle()
                {
                    available = true,
                    position = new Intersection()
                    {
                        r = 0,
                        c = 0
                    },
                    assignedRides = new List<Ride>()
                });
            }

            /**************************************************************************************
             *  Solver
             **************************************************************************************/

            Debug.WriteLine("Solving the problem...");

            for (int t = 0; t < T; t++)
            {
                
            }

            /**************************************************************************************
             *  Output
             **************************************************************************************/

            Debug.WriteLine("Output to file...");

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), outputFileName)))
            {
                foreach (Vehicle vehicle in fleet)
                {
                    outputFile.WriteLine(vehicle.ToString());
                }
            }

            Debug.WriteLine("Done.");
        }

        static int Distance(Intersection a, Intersection b)
        {
            return Math.Abs(b.r - a.r) + Math.Abs(b.c - a.c);
        }
    }

    class Intersection
    {
        public int r;
        public int c;
    }

    class Ride
    {
        public int id;
        public Intersection Start;
        public Intersection Finish;
        public int earliestStart;
        public int latestFinish;
        public int LastestStart()
        {
            return latestFinish - (Math.Abs(Finish.r - Start.r) + Math.Abs(Finish.c - Start.c));
        }
    }

    class Vehicle
    {
        public bool available;
        public Intersection position;
        public List<Ride> assignedRides;

        public override string ToString()
        {
            string assignedRidesOutput = "";
            foreach (Ride ride in assignedRides)
            {
                assignedRidesOutput = String.Join(" ", assignedRidesOutput, ride.id.ToString());
            }

            return assignedRides.Count.ToString() + " " + assignedRidesOutput;
        }
    }
}
