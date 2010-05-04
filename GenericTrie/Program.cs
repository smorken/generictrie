using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTrie
{

    class Program
    {
        static void BasicTrie()
        {
            Trie<string, char, int> myTrie = new Trie<string, char, int>();
            Trie<string, char, int>.WildCard = '?';
            myTrie.Add("abc", 1);
            myTrie.Add("abd", 2);
            myTrie.Add("abe", 3);
            myTrie.Add("dbf", -999);
            myTrie.Add("abcd", -9999);
            myTrie.Add("ab", -999999);
            myTrie.Add("asdfab;kna;slkdnga;lskgna;slgkdn", -1);

            List<char[]> matches = myTrie.GetMatchingKeys("?b?");
            List<int> MatchingValues = myTrie.GetMatchingValues("ab?");
            List<int> values = myTrie["ab"];
            List<int> values2 = myTrie["????"];
        }
        static void ClassifierTrie(List<Classifier> Classifiers)
        {
            Trie<ClassifierSet, ClassifierValue, List<Inventory>> InventoryLookup = new Trie<ClassifierSet, ClassifierValue, List<Inventory>>();
            Trie<ClassifierSet, ClassifierValue, List<Inventory>>.WildCard = new ClassifierValue() { Value = "?" };
            int InventoryID = 0;
            for (int i = 0; i < 1000000; i++)
            {
                ClassifierSet RandomClassifierSet = ClassifierSet.GetRandomClassifierSet(Classifiers);
                if (!InventoryLookup.ContainsKey(RandomClassifierSet))
                {
                    InventoryLookup.Add(RandomClassifierSet, new List<Inventory>() { new Inventory(InventoryID++, RandomClassifierSet) });
                }
                else
                {
                    InventoryLookup[RandomClassifierSet][0].Add(new Inventory(InventoryID++, RandomClassifierSet));
                }
            }
            for (int i = 0; i < 1000000; i++)
            {
                ClassifierSet RandomClassifierSet = ClassifierSet.GetRandomClassifierSet(Classifiers);
                bool contained = InventoryLookup.ContainsKey(RandomClassifierSet);
                List<List<Inventory>> li = InventoryLookup[RandomClassifierSet];
            }
        }
        static void IntTrie()
        {
            Random rnd = new Random();
            Trie<int[], int, List<Inventory>> intTrie = new Trie<int[], int, List<Inventory>>();
            Trie<int[], int, List<Inventory>>.WildCard = -1;
            int InventoryID = 0;
            for (int i = 0; i < 1000000; i++)
            {
                int[] RandomIntegers = new int[4];
                RandomIntegers[0] = rnd.Next(0, 10);
                RandomIntegers[1] = rnd.Next(0, 20);
                RandomIntegers[1] = rnd.Next(0, 3);
                RandomIntegers[3] = rnd.Next(0, 1000000);
                if (!intTrie.ContainsKey(RandomIntegers))
                {
                    intTrie.Add(RandomIntegers, new List<Inventory>() { new Inventory(InventoryID++, RandomIntegers) });
                }
                else
                {
                    intTrie[RandomIntegers][0].Add(new Inventory(InventoryID++, RandomIntegers));
                }

            } 
            for (int i = 0; i < 1000000; i++)
            {
                int[] RandomIntegers = new int[4];
                RandomIntegers[0] = rnd.Next(0, 10);
                RandomIntegers[1] = rnd.Next(0, 20);
                RandomIntegers[1] = rnd.Next(0, 3);
                RandomIntegers[3] = rnd.Next(0, 1000000);
                bool contained = intTrie.ContainsKey(RandomIntegers);
                List<List<Inventory>> li = intTrie[RandomIntegers];
            }
        }
        static void Main(string[] args)
        {
            BasicTrie();

           // List<Classifier> Classifiers = BuildClassifiers();

            DateTime t1, t2;

            //t1 = DateTime.Now;
            //ClassifierTrie(Classifiers);
            //t2 = DateTime.Now;
            //TimeSpan classifierTime = t2 - t1;
            //double classifierTimeSeconds = classifierTime.TotalMilliseconds;

            //t1 = DateTime.Now;
            //IntTrie();
            //t2 = DateTime.Now;
            //TimeSpan intTime = t2 - t1;
            //double intTimeSeconds = intTime.TotalMilliseconds;

        }
        static List<Classifier> BuildClassifiers()
        {
            List<Classifier> Classifiers = new List<Classifier>();

            Classifier Location = new Classifier();
            Location.AddValue("AB");
            Location.AddValue("BC");
            Location.AddValue("NWT");
            Location.AddValue("SASK");
            Location.AddValue("MB");
            Location.AddValue("ONT");
            Location.AddValue("QC");
            Location.AddValue("NB");
            Location.AddValue("PEI");
            Location.AddValue("NS");
            Classifiers.Add(Location);

            Classifier Species = new Classifier();
            Species.AddValue("Spruce");
            Species.AddValue("Blackspruce");
            Species.AddValue("Redspruce");
            Species.AddValue("Norwayspruce");
            Species.AddValue("Engelmannspruce");
            Species.AddValue("Whitespruce");
            Species.AddValue("Sitkaspruce");
            Species.AddValue("Blackandredspruce");
            Species.AddValue("Redandwhitespruce");
            Species.AddValue("Otherspruce");
            Species.AddValue("Pine");
            Species.AddValue("Westernwhitepine");
            Species.AddValue("Easternwhitepine");
            Species.AddValue("Jackpine");
            Species.AddValue("Lodgepolepine");
            Species.AddValue("Shorepine");
            Species.AddValue("Whitebarkpine");
            Species.AddValue("Austrianpine");
            Species.AddValue("Ponderosapine");
            Species.AddValue("Redpine");
            Species.AddValue("Pitchpine");
            Species.AddValue("Scotspine");
            Species.AddValue("Mughopine");
            Species.AddValue("Limberpine");
            Classifiers.Add(Species);

            Classifier SiteQuality = new Classifier();
            SiteQuality.AddValue("Poor");
            SiteQuality.AddValue("Average");
            SiteQuality.AddValue("Good");
            Classifiers.Add(SiteQuality);

            Classifier ID = new Classifier();
            for (int i = 0; i < 1000000; i++)
            {
                ID.AddValue(i.ToString());
            }
            Classifiers.Add(ID);

            return Classifiers;
        }
    }
}
