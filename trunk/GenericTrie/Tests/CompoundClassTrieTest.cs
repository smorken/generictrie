using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTrie.Tests
{
    class CompoundClassTrieTest : Test
    {
        public CompoundClassTrieTest(NodeContainerType ContainerType, int Count, int MinLength, int MaxLength, int DistinctValues)
            : base(ContainerType, Count, MinLength, MaxLength, DistinctValues)
        {
        }
        private Trie<CompoundClassWord, CompoundClassLetter, CompoundClassWord> trie = new Trie<CompoundClassWord, CompoundClassLetter, CompoundClassWord>();

        public override void AllocateTrie()
        {
            throw new NotImplementedException();
        }

        public override void RunTest()
        {
            throw new NotImplementedException();
        }
    }
}
