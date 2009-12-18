using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTrie
{

    /// <summary>
    /// A Generic Trie implementation (both key and value are generic).  A Wilcard token may be set to select many nodes from the Trie.
    /// </summary>
    /// <typeparam name="TKey">The type of the key. Eg. "string".  The key must implement the IComparable(TKey) interface.</typeparam>
    /// <typeparam name="TToken">The type of the tokens that make up the key. For a string this is "char", ie a string is made up of characters.</typeparam>
    /// <typeparam name="TValue">The type of the values associated with the key.</typeparam>
    public class Trie<TKey, TToken, TValue>
        where TKey : IEnumerable<TToken>
        where TToken : IComparable<TToken>, IEquatable<TToken>
    {
        /// <summary>
        /// The number of nodes in the trie
        /// </summary>
        public static int TotalNodes { get; private set; }
        public static TToken WildCard;
        private TrieNode Root = new TrieNode();
        public void Add(TKey Key, TValue Value)
        {
            Root.Add(Value, Key.ToArray());
        }
        public bool ContainsKey(TKey Key)
        {
            return Root.Contains(Key.ToArray());
        }
        public List<TToken[]> GetMatchingKeys(TKey Key)
        {
            return Root.GetMatchingKeys(Key.ToArray());
        }
        public List<TValue> this[TKey Key]
        {
            get
            {
                return Root.GetMatchingValues(Key.ToArray());
            }
        }
        public List<TValue> GetMatchingValues(TKey Key)
        {
            return Root.GetMatchingValues(Key.ToArray());
        }
        private class TrieNode
        {
            
            public TValue Value;
            public List<TToken> Prefix;
            public TToken Key;
            public Dictionary<TToken, TrieNode> Children = new Dictionary<TToken, TrieNode>();
            public bool Terminal = false;
            public TrieNode()
            {
                TotalNodes++;//update the static node count 
            }
            public override string ToString()
            {
                return Key.ToString();
            }
            #region Node functions
            /// <summary>
            /// Test if a node contains a particular child
            /// </summary>
            /// <param name="Value">The value to test</param>
            /// <returns>True if the node contains the chil, otherwise false</returns>
            private bool ContainsChild(TToken Value)
            {
                return Children.ContainsKey(Value);
            }
            /// <summary>
            /// Get a child node of this node using its value to find it
            /// </summary>
            /// <param name="Value">The value of the node to find</param>
            /// <returns>The node</returns>
            private TrieNode GetChild(TToken Value)
            {
                if (Children.ContainsKey(Value))
                {
                    return Children[Value];
                }
                return null;
            }
            #endregion
            #region Insertion
            public void Add(TValue Value, params TToken[] Key)
            {
                if (WildCard != null)
                {
                    foreach (TToken token in Key)
                    {
                        if (token.CompareTo(WildCard) == 0)
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
                    TrieNode newNode = new TrieNode()
                    {
                        Prefix = new List<TToken>(),
                        Key = newKey[Index],
                    };
                    if (this.Prefix != null)
                    {
                        newNode.Prefix.AddRange(Prefix);
                        newNode.Prefix.Add(Key);
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
                }
            }
            #endregion
            #region Retrieval
            public List<TValue> GetMatchingValues(TToken[] Keys)
            {
                List<TValue> Values = new List<TValue>();
                return GetMatchingValues(Keys, 0, Values);
            }
            private List<TValue> GetMatchingValues(TToken[] Keys, int Index, List<TValue> Values)
            {
                if (Index == Keys.Length)
                {
                    Values.Add(this.Value);
                    return Values;
                }
                if (WildCard != null && Keys[Index].CompareTo(WildCard) == 0)
                {
                    foreach (KeyValuePair<TToken,TrieNode> value in Children)
                    {
                        TToken[] TestValues = new TToken[Keys.Length];
                        Keys.CopyTo(TestValues, 0);
                        TestValues[Index] = value.Key;
                        GetMatchingValues(TestValues, Index, Values);
                    }
                }
                else
                {
                    if (this.ContainsChild(Keys[Index]))
                    {
                        return GetChild(Keys[Index]).GetMatchingValues(Keys, Index + 1, Values);
                    }
                }
                return Values;
            }
            public List<TToken[]> GetMatchingKeys(TToken[] Keys)
            {
                List<TToken[]> matches = new List<TToken[]>();
                return GetMatchingKeys(Keys, 0, matches);
            }
            private List<TToken[]> GetMatchingKeys(TToken[] Keys, int Index, List<TToken[]> matches)
            {
                if (Index == Keys.Length)
                {
                    matches.Add(Keys);
                    return matches;
                }
                if (WildCard != null && Keys[Index].CompareTo(WildCard) == 0)
                {
                    foreach (KeyValuePair<TToken,TrieNode> value in Children)
                    {
                        TToken[] TestValues = new TToken[Keys.Length];
                        Keys.CopyTo(TestValues, 0);
                        TestValues[Index] = value.Key;
                        GetMatchingKeys(TestValues, Index, matches);
                    }
                }
                else
                {
                    if (this.ContainsChild(Keys[Index]))
                    {
                        return GetChild(Keys[Index]).GetMatchingKeys(Keys, Index + 1, matches);
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
                    return true;
                }
                if (WildCard != null && Values[Index].CompareTo(WildCard) == 0)
                {

                    foreach (KeyValuePair<TToken,TrieNode> value in Children)
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
