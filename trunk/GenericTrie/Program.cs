using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GenericTrie.Tests;
namespace GenericTrie
{

    class Program
    {
 
        static void Main(string[] args)
        {
            Trie<string, char, int> Trie = new Trie<string, char, int>();
            Trie.Add("ABCD", 1);
            Trie.Add("ABED", 2);
            Trie.Add("ACED", 4);
            Trie.WildCard = '?';
            List<int> value = Trie["?B??"];
            List<int> value2 = Trie["A"];
            IntTrieTest test1 = new IntTrieTest(NodeContainerType.Dictionary, 5000, 5, 100, 5200);

            string values = test1.Start();

            //IntTrieTest test2 = new IntTrieTest(NodeContainerType.Dictionary, 500000, 1, 10, 500);

            //string values2 = test2.Start();

            StringTrieTest test3 = new StringTrieTest(NodeContainerType.LinearAssociativeArray, 50000, 2, 10, 26);
            string derp = test3.Start();

        }
        
    }
}
