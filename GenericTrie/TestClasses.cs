using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTrie
{
    public class Classifier
    {
       public List<ClassifierValue> Values = new List<ClassifierValue>();
       public void AddValue(string Value)
       {
           Values.Add(new ClassifierValue() { Value = Value });
       }
    }
    public class ClassifierValue : IComparable<ClassifierValue>, IEquatable<ClassifierValue>
    {
        public string Value;
        public int CompareTo(ClassifierValue Rh)
        {
            return Value.CompareTo(Rh.Value);
        }
        public bool Equals(ClassifierValue Rh)
        {
            return this.Value.CompareTo(Rh.Value) == 0;
        }
        public override string ToString()
        {
            return Value;
        }
    }
    public class ClassifierSet : System.Collections.Generic.IEnumerable<ClassifierValue>
    {
        static Random rnd = new Random();

        public ClassifierValue[] Values;
        public IEnumerator<ClassifierValue> GetEnumerator()
        {
            foreach (ClassifierValue c in Values)
            {
                yield return c;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Values.GetEnumerator();
        }
        public override string ToString()
        {
            StringBuilder setStringBuilder = new StringBuilder();
            foreach(ClassifierValue cval in Values )
            {
                setStringBuilder.Append(cval + ", ");
            }
            return setStringBuilder.ToString().Remove(setStringBuilder.Length - 2, 2);
        }
        public static ClassifierSet GetRandomClassifierSet(List<Classifier> Classifiers)
        {
            
            ClassifierSet set = new ClassifierSet();
            set.Values = new ClassifierValue[Classifiers.Count];
            for (int ClassifierIndex = 0; ClassifierIndex < Classifiers.Count; ClassifierIndex++)
            {
                Classifier classifier = Classifiers[ClassifierIndex];
                set.Values[ClassifierIndex] = classifier.Values[rnd.Next(0, classifier.Values.Count)];
            }
            return set;
        }
    }
    public class Inventory
    {
        public int InventoryID = 0;
        public ClassifierSet ClassifierSet;
        public Inventory(int ID, ClassifierSet set)
        {
            this.InventoryID = ID;
            this.ClassifierSet = set;
        }
        public override string ToString()
        {
            return "InventoryID = " + InventoryID + 
                ", ClassifierSet = " + ClassifierSet;
        }
    }
}
