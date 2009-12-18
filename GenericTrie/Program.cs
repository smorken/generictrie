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

            List<char[]> matches = myTrie.GetMatchingKeys("?b?");
            List<int> MatchingValues = myTrie.GetMatchingValues("ab?");
            List<int> values = myTrie["???"];
        }
        static void ClassifierTrie(List<Classifier> Classifiers)
        {
            Trie<ClassifierSet, ClassifierValue, List<Inventory>> InventoryLookup = new Trie<ClassifierSet, ClassifierValue, List<Inventory>>();
            Trie<ClassifierSet, ClassifierValue, List<Inventory>>.WildCard = new ClassifierValue() { Value = "?" };
            int InventoryID = 0;
            for (int i = 0; i < 100000; i++)
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
            for (int i = 0; i < 100000; i++)
            {
                ClassifierSet RandomClassifierSet = ClassifierSet.GetRandomClassifierSet(Classifiers);
                bool contained = InventoryLookup.ContainsKey(RandomClassifierSet);
            }
        }
        static void Main(string[] args)
        {
            BasicTrie();

            List<Classifier> Classifiers = BuildClassifiers();

            ClassifierTrie(Classifiers);
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
