using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace GenericTrie.Tests
{
    public class CompoundClassLetter : IComparable<CompoundClassLetter>
    {
        public string Value;
        public int CompareTo(CompoundClassLetter c)
        {
            return Value.CompareTo(c.Value);
        }

    }

    public class CompoundClassWord : IEnumerable<CompoundClassLetter>
    {
        public CompoundClassLetter[] Letters;
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<CompoundClassLetter> GetEnumerator()
        {
            if (Letters == null)
            {
                yield break;
            }
            else
            {
                foreach (CompoundClassLetter letter in Letters)
                {
                    yield return letter;
                }
            }
        }
    }
}
