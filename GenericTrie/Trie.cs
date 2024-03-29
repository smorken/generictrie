﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace GenericTrie
{

    /// <summary>
    /// A Generic Trie implementation (both key and value are generic).  A Wilcard token may be set to select many nodes from the Trie.
    /// </summary>
    /// <typeparam name="TKey">The type of the key. Eg. "string".  The key must implement the IEnumerable(TToken) interface.</typeparam>
    /// <typeparam name="TToken">The type of the tokens that make up the key. For a string this is "char", ie a string is made up of characters.
    /// must implement IComparable(TToken)</typeparam>
    /// <typeparam name="TValue">The type of the values associated with the key.</typeparam>
    public class Trie<TKey, TToken, TValue>
        where TKey : IEnumerable<TToken>
        where TToken : IComparable<TToken>
    {
        public NodeContainerType ContainerType { get; private set; }
        /// <summary>
        /// The number of nodes in the trie
        /// </summary>
        public int TotalNodes { get; private set; }
        public bool WildCardIsSet { get; private set; }
        private TToken wildcard;
        public TToken WildCard
        {
            get
            {
                return wildcard;
            }
            set
            {
                wildcard = value;
                WildCardIsSet = true;
            }
        }

        private TrieNode Root;

        public Trie(NodeContainerType ContainerType)
        {

            Root = new TrieNode(this);// (ContainerType);
            this.ContainerType = ContainerType;
        }
        public Trie()
        {

            Root = new TrieNode(this);
            this.ContainerType = NodeContainerType.SortedDictionary;
        }

        public void Add(TKey Key, TValue Value)
        {

            Root.Add(Value, Key.ToArray());
        }
        public bool ContainsKey(TKey Key)
        {
            return Root.Contains(Key.ToArray());
        }
        /// <summary>
        /// Return the keys 
        /// </summary>
        /// <param name="Key">The collection of keys</param>
        /// <returns></returns>
        public List<TToken[]> GetMatchingKeys(TKey Key)
        {
            TToken[] keyArray = Key.ToArray();

            if (Root.Contains(keyArray))
            {
                return Root.GetMatchingKeys(keyArray, false);
            }
            else
            {
                return null;
            }
        }
        public List<TValue> this[TKey Key]
        {
            get
            {
                TToken[] keyArray = Key.ToArray();
                if (Root.Contains(keyArray))
                {
                    return Root.GetMatchingValues(keyArray, false);
                }
                else
                {
                    throw new ArgumentException("That key does not exist in the Trie");
                }
            }
        }
        public List<TValue> GetMatchingValues(TKey Key)
        {
            TToken[] keyArray = Key.ToArray();
            if (Root.Contains(keyArray))
            {
                return Root.GetMatchingValues(keyArray, false);
            }
            return null;
        }
        public TValue GetValue(TKey key)
        {
            if (WildCardIsSet)
            {
                if (key.Contains(WildCard))
                {
                    throw new ArgumentException("Key may not contain wildcard");
                }
            }

            return Root.GetMatchingValues(key.ToArray(), false).First();

        }
        public List<TToken[]> PrefixKeySearch(TKey Prefix)
        {
            return Root.GetMatchingKeys(Prefix.ToArray(), true);
        }
        public List<TValue> PrefixValueSearch(TKey Prefix)
        {
            return Root.GetMatchingValues(Prefix.ToArray(), true);
        }
        public List<TToken[]> SuffixSearch()
        {
            throw new NotImplementedException();
        }
        private class TrieNode
        {
            private Trie<TKey, TToken, TValue> container;
            private TValue Value;
            private TrieNode Parent;
            private TToken Key;
            public IDictionary<TToken, TrieNode> Children;
            public bool Terminal
            {
                get;
                private set;
            }
            public List<TToken> Prefix
            {
                get
                {
                    return GetPrefix();
                }
            }
            public List<TToken> Word
            {
                get
                {
                    List<TToken> word = GetPrefix();
                    word.Add(this.Key);
                    return word;
                }
            }
            public TrieNode(Trie<TKey, TToken, TValue> container)
            {
                this.container = container;


                this.container.TotalNodes++;//update the static node count 
            }
            private IDictionary<TToken, TrieNode> CreateChildContainer()
            {
                IDictionary<TToken, TrieNode> children = null;
                switch (container.ContainerType)
                {
                    case NodeContainerType.Dictionary:
                        children = new Dictionary<TToken, TrieNode>();
                        break;
                    case NodeContainerType.LinearAssociativeArray:
                        children = new LinearAssociativeArray<TToken, TrieNode>();
                        break;
                    case NodeContainerType.SortedDictionary:
                        children = new SortedDictionary<TToken, TrieNode>();
                        break;
                    case NodeContainerType.SortedList:
                        children = new SortedList<TToken, TrieNode>();
                        break;
                }
                return children;
            }
            public override string ToString()
            {
                return Key.ToString();
            }

            public List<TToken> GetPrefix()
            {

                if (Parent != null)
                {
                    Stack<TToken> prefix = new Stack<TToken>();
                    TrieNode parent = this.Parent;
                    while (parent.Parent != null)
                    {
                        prefix.Push(parent.Key);
                        parent = parent.Parent;
                    }
                    return prefix.ToList();
                }
                return null;
            }
            #region Node functions
            /// <summary>
            /// Test if a node contains a particular child
            /// </summary>
            /// <param name="Value">The value to test</param>
            /// <returns>True if the node contains the chil, otherwise false</returns>
            private bool ContainsChild(TToken Value)
            {
                if (Children == null)
                    return false;

                else if (Children.ContainsKey(Value))
                {
                    return true;
                }
                return false;
            }
            /// <summary>
            /// Get a child node of this node using its value to find it
            /// </summary>
            /// <param name="Value">The value of the node to find</param>
            /// <returns>The node</returns>
            private TrieNode GetChild(TToken Value)
            {
                if (Children != null && Children.ContainsKey(Value))
                {
                    return Children[Value];
                }
                return null;
            }


            #endregion
            #region Insertion
            public void Add(TValue Value, params TToken[] Key)
            {
                if (container.WildCardIsSet)
                {
                    foreach (TToken token in Key)
                    {
                        if (token.CompareTo(container.WildCard) == 0)
                        {
                            throw new ArgumentException("Wildcards may not be inserted to the Trie");
                        }
                    }
                }
                Add(Value, Key, 0);

            }
            private void Add(TValue Value, TToken[] newKey, int Index)
            {

                if (this.Children == null || !this.ContainsChild(newKey[Index]))
                {
                    TrieNode newNode = new TrieNode(this.container)//(this.Container)
                    {
                        Parent = this,
                        Key = newKey[Index],
                    };
                    if (this.Children == null)
                    {
                        this.Children = CreateChildContainer();
                    }

                    Children.Add(newNode.Key, newNode);
                    if (Index < newKey.Length - 1)
                    {
                        newNode.Add(Value, newKey, Index + 1);
                    }
                    else
                    {
                        newNode.Value = Value;
                        newNode.Terminal = true;
                    }
                }
                else
                {
                    if (Index < newKey.Length - 1)
                    {

                        TrieNode MatchingChild = GetChild(newKey[Index]);
                        MatchingChild.Add(Value, newKey, Index + 1);
                    }
                    else
                    {
                        TrieNode matchingChild = GetChild(newKey[Index]);
                        matchingChild.Value = Value;
                        matchingChild.Terminal = true;
                    }
                }
            }
            #endregion
            #region Retrieval
            public List<TValue> GetMatchingValues(TToken[] Keys, bool PrefixSearch)
            {
                List<TValue> Values = new List<TValue>();
                return GetMatchingValues(Keys, 0, Values, PrefixSearch);
            }
            private List<TValue> GetMatchingValues(TToken[] Keys, int Index, List<TValue> Values, bool PrefixSearch)
            {
                if (Index == Keys.Length)
                {
                    if (this.Terminal)
                    {
                        //if (!PrefixSearch)
                        //{
                            Values.Add(this.Value);
                        //}
                        //else
                        //{
                        //    Values.Add(this.Value);
                        //    return Values;
                        //}
                    }
                    if (PrefixSearch && Children != null)
                    {
                        foreach (KeyValuePair<TToken, TrieNode> value in Children)
                        {
                            value.Value.GetMatchingValues(Keys, Index, Values, PrefixSearch);
                        }
                    }
                    return Values;
                }
                if (Children != null && container.WildCardIsSet && Keys[Index].CompareTo(container.WildCard) == 0)
                {
                    foreach (KeyValuePair<TToken, TrieNode> value in Children)
                    {
                        TToken[] TestValues = new TToken[Keys.Length];
                        Keys.CopyTo(TestValues, 0);
                        TestValues[Index] = value.Key;
                        GetMatchingValues(TestValues, Index, Values, PrefixSearch);
                    }
                }
                else
                {
                    if (this.ContainsChild(Keys[Index]))
                    {
                        return GetChild(Keys[Index]).GetMatchingValues(Keys, Index + 1, Values, PrefixSearch);
                    }
                }

                return Values;
            }
            public List<TToken[]> GetMatchingKeys(TToken[] Keys, bool PrefixSearch)
            {
                List<TToken[]> matches = new List<TToken[]>();
                return GetMatchingKeys(Keys, 0, matches, PrefixSearch);
            }
            private List<TToken[]> GetMatchingKeys(TToken[] Keys, int Index, List<TToken[]> matches, bool PrefixSearch)
            {
                if (Index == Keys.Length)
                {
                    if (this.Terminal)
                    {                       
                        matches.Add(this.Word.ToArray());                       
                    }
                    if (PrefixSearch && Children != null)
                    {
                        foreach (KeyValuePair<TToken, TrieNode> value in Children)
                        {
                            value.Value.GetMatchingKeys(Keys, Index, matches, PrefixSearch);
                        }
                    }
                    return matches;
                }
                if (Children != null && container.WildCardIsSet && Keys[Index].CompareTo(container.WildCard) == 0)
                {
                    foreach (KeyValuePair<TToken, TrieNode> value in Children)
                    {
                        TToken[] TestValues = new TToken[Keys.Length];
                        Keys.CopyTo(TestValues, 0);
                        TestValues[Index] = value.Key;
                        GetMatchingKeys(TestValues, Index, matches, PrefixSearch);
                    }
                }
                else
                {
                    if (this.ContainsChild(Keys[Index]))
                    {
                        return GetChild(Keys[Index]).GetMatchingKeys(Keys, Index + 1, matches, PrefixSearch);
                    }
                }

                return matches;
            }

            #endregion
            #region Content Checking
            /// <summary>
            /// Check if the trie contains a particular word
            /// </summary>
            /// <param name="Values">The array of T that compose the word.  A Wildcard value is allowed</param>
            /// <returns>True if the trie contains the word, otherwise false</returns>
            public bool Contains(params TToken[] Values)
            {
                return Contains(Values, 0);
            }
            /// <summary>
            /// Recursive sub function for the Contains method group
            /// </summary>
            /// <param name="Values">The value composing the word to check</param>
            /// <param name="Index">The index into the word being evaluated</param>
            /// <returns>True if the word is found, otherwise false</returns>
            private bool Contains(TToken[] Values, int Index)
            {
                if (Index == Values.Length)
                {
                    if (Terminal)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (Children != null && container.WildCardIsSet && Values[Index].CompareTo(container.WildCard) == 0)
                {

                    foreach (KeyValuePair<TToken, TrieNode> value in Children)
                    {
                        TToken[] TestValues = new TToken[Values.Length];
                        Values.CopyTo(TestValues, 0);
                        TestValues[Index] = value.Key;
                        bool Contained = Contains(TestValues, Index);
                        if (Contained == true)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (this.ContainsChild(Values[Index]))
                    {
                        return GetChild(Values[Index]).Contains(Values, Index + 1);
                    }
                }

                return false;
            }
            #endregion
        }
    }
}