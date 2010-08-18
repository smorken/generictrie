using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTrie.Tests
{
    public abstract class Test
    {
        protected NodeContainerType ContainerType;
        public Test(NodeContainerType ContainerType, int Count, int MinLength, int MaxLength, int DistinctValues)
        {
            this.ContainerType = ContainerType;
            this.Count = Count;
            this.MinLength = MinLength;
            this.MaxLength = MaxLength;
            this.DistinctValues = DistinctValues;
        }
        public long MemoryUsed { get; private set; }
        public TimeSpan TestTime { get; private set; }
        public TimeSpan AllocationTime { get; private set; }
        public int Count { get; private set; }
        public int MinLength { get; private set; }
        public int MaxLength { get; private set; }
        public int DistinctValues { get; private set; }
        public abstract void RunTest();
        public abstract void AllocateTrie();

        public string Start()
        {
            StringBuilder resultBuilder = new StringBuilder();
            resultBuilder.Append(Count + "," + MinLength + "," + MaxLength + ",");

            DateTime allocationStartTime = DateTime.Now;
            long mem = GC.GetTotalMemory(true);
            AllocateTrie();
            MemoryUsed = GC.GetTotalMemory(true) - mem;
            AllocationTime = DateTime.Now - allocationStartTime;
            resultBuilder.Append(MemoryUsed + "," + AllocationTime + ",");

            DateTime TestStartTime = DateTime.Now;
            RunTest();
            TestTime = DateTime.Now - TestStartTime;
            resultBuilder.Append(TestTime);

            return resultBuilder.ToString();
        }

    }
}
