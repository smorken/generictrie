using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTrie.Tests
{
    public class StringTrieTest : Test
    {
        Trie<string, char, int> myTrie;
        List<string> Keys;

        public StringTrieTest(NodeContainerType ContainerType, int Count, int MinLength, int MaxLength, int DistinctValues)
            : base(ContainerType, Count, MinLength, MaxLength, DistinctValues)
        {

            myTrie = new Trie<string, char, int>(ContainerType);
            myTrie.WildCard = '?';
        }
        public override void AllocateTrie()
        {
            Random rnd = new Random();
            Keys = new List<string>();
            for (int stringIndex = 0; stringIndex < Count; stringIndex++)
            {
                int length = rnd.Next(MinLength, MaxLength);
                StringBuilder builder = new StringBuilder();
                for (int charIndex = 0; charIndex < length; charIndex++)
                {
                    char c = (char)rnd.Next(0, DistinctValues);
                    while (c == myTrie.WildCard)
                    {
                        c = (char)rnd.Next(0, DistinctValues);
                    }
                    builder.Append(c);
                }
                string key = builder.ToString();
                if (!myTrie.ContainsKey(key))
                {
                    Keys.Add(key);
                    myTrie.Add(key, stringIndex);
                }
            }

        }
        public override void RunTest()
        {
            int totalValues = 0;
            int totalKeys = 0;
            for (int i = MinLength; i < MaxLength; i++)
            {
                StringBuilder query = new StringBuilder();
                query.Append('?',i);
                List<int> values = myTrie.GetMatchingValues(query.ToString());
                totalValues += values.Count;
                
                List<char[]> keys = myTrie.GetMatchingKeys(query.ToString());
                totalKeys += keys.Count;
                
            }
            if (totalValues != Keys.Count && totalKeys != Keys.Count)
            {
                throw new Exception();
            }
        
        }

    }
}
