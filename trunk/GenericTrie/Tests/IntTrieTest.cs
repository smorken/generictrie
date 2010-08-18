using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTrie.Tests
{
    public class IntTrieTest :Test
    {
        Trie<int[], int, int> intTrie;
        List<int[]> Keys = new List<int[]>();

        int collisions = 0;
        public IntTrieTest(NodeContainerType ContainerType, int Count, int MinLength, int MaxLength, int DistinctValues)
            : base(ContainerType, Count, MinLength, MaxLength, DistinctValues)
        {

            intTrie = new Trie<int[], int, int>(ContainerType);
            intTrie.WildCard = -999;
        }
        public override void AllocateTrie()
        {
            Random rnd = new Random();

            for (int i = 0; i < Count; i++)
            {
                int Length = rnd.Next(MinLength, MaxLength);
                int[] RandomIntegers = new int[Length];
                for (int j = 0; j < Length; j++)
                {
                    RandomIntegers[j] = rnd.Next(0, DistinctValues);
                }
                if (!intTrie.ContainsKey(RandomIntegers))
                {
                    Keys.Add(RandomIntegers);
                    intTrie.Add(RandomIntegers, i);
                }
                else
                {
                    collisions++;
                }

            }

        }

        public override void RunTest()
        {

            for (int i = 0; i < Keys.Count; i++)
            {

                bool contained = intTrie.ContainsKey(Keys[i]);
                if (!contained)
                {
                    throw new Exception();
                }

                List<int> li = intTrie[Keys[i]];
            }

            int Total = 0;
            for (int i = MinLength; i < MaxLength; i++)
            {
                int[] Wildcardkey = new int[i];
                for (int j = 0; j < Wildcardkey.Length; j++)
                {
                    Wildcardkey[j] = intTrie.WildCard;
                }
                List<int> Matches = intTrie.GetMatchingValues(Wildcardkey);
                if (Matches != null)
                {
                    Total += Matches.Count;
                }
                
            }
            if (Total != Keys.Count)//check 1, the number of values is the same as the number of selected values by all wildcards
            {
                throw new Exception();
            }

        }
    }

}
