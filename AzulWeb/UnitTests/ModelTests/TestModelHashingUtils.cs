using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Client;
using Models.Hashing;

namespace UnitTests
{
    [TestClass]
    public class TestModelHashingUtils
    {
        [TestMethod]
        public void HashOfSetWithDifferentElementsDifferent(){
            SortedSet<int> ascendingSet=new SortedSet<int>(Comparer<int>.Create((x,y)=>x-y));
            SortedSet<int> decendingSet=new SortedSet<int>(Comparer<int>.Create((x,y)=>y-x));
            foreach(var i in new[]{1,2,3,5,6,8,9}){
                ascendingSet.Add(i);
                decendingSet.Add(i);
            }
            ascendingSet.Remove(1);
            Assert.AreNotEqual(
                ModelHashUtils.HashOfSet(ascendingSet),
                ModelHashUtils.HashOfSet(decendingSet),
                "expected hash of set with more elements to be differrent that other hash"
            );
        }
        [TestMethod]
        public void HashOfSetOrderIndependent(){
            SortedSet<int> ascendingSet=new SortedSet<int>(Comparer<int>.Create((x,y)=>x-y));
            SortedSet<int> decendingSet=new SortedSet<int>(Comparer<int>.Create((x,y)=>y-x));
            foreach(var i in new[]{1,2,3,5,6,8,9}){
                ascendingSet.Add(i);
                decendingSet.Add(i);
            }
            Assert.AreEqual(
                ModelHashUtils.HashOfSet(ascendingSet),
                ModelHashUtils.HashOfSet(decendingSet),
                "expected hash of ascending sorted set to match that of descending sorted set"
            );
        }
        [TestMethod]
        public void CombineHashHandlesNull()
        {
            int hashA=17;
            ModelHashUtils.CombineHash(ref hashA,null);
            int hashB=17;
            ModelHashUtils.CombineHash(ref hashB,null);
            Assert.AreEqual(hashA,hashB);
        }
        [TestMethod]
        public void HashListHandlesNullList()
        {
            int[] listA=null;
            int[] listB=null;
            Assert.AreEqual(ModelHashUtils.HashOfEnumerable(listA),ModelHashUtils.HashOfEnumerable(listB));
        }
        [TestMethod]
        public void HashListHandlesNullElement()
        {
            int?[] listA={1,2,3,null,5};
            int?[] listB={1,2,3,null,5};
            Assert.AreEqual(ModelHashUtils.HashOfEnumerable(listA),ModelHashUtils.HashOfEnumerable(listB));
        }
        [TestMethod]
        public void HashListDifferentForDifferentContent()
        {
            int[] listA={1,2,3,3};
            int[] listB={1,2,3,5};
            Assert.AreNotEqual(ModelHashUtils.HashOfEnumerable(listA),ModelHashUtils.HashOfEnumerable(listB));
        }
        [TestMethod]
        public void HashListDifferentForDifferentOrder()
        {
            int[] listA={1,2,3,4};
            int[] listB={1,4,3,2};
            Assert.AreNotEqual(ModelHashUtils.HashOfEnumerable(listA),ModelHashUtils.HashOfEnumerable(listB));
        }
        [TestMethod]
        public void HashListOfListHandlesNull(){
            int[][] list=null;
            ModelHashUtils.HashOfEnumerableOfEnumerable(list);
        }
        [TestMethod]
        public void HashListOfListHandlesNullRowAndHashesDifferentToNull(){
            int[][] listA=null; 
            int[][] listB=new int[][]{
                new int[]{1,32,4},
                null,
                new int[]{1,32,4}
            };
            Assert.AreNotEqual(ModelHashUtils.HashOfEnumerable(listA),ModelHashUtils.HashOfEnumerable(listB));
        }
        [TestMethod]
        public void HashListOfListHandlesNullElementAndHashesDifferentToNullRow(){
            int?[][] listA=new int?[][]{
                new int?[]{1,32,4},
                new int?[]{1,null,4},
                new int?[]{1,32,4}
            };
            int?[][] listB=new int?[][]{
                new int?[]{1,32,4},
                null,
                new int?[]{1,32,4}
            };
            Assert.AreNotEqual(ModelHashUtils.HashOfEnumerable(listA),ModelHashUtils.HashOfEnumerable(listB));
        }
        [TestMethod]
        public void HashListSameForSameList()
        {
            int[] listA={1,2,3,4};
            int[] listB={1,2,3,4};
            Assert.AreEqual(ModelHashUtils.HashOfEnumerable(listA),ModelHashUtils.HashOfEnumerable(listB));
        }
        [TestMethod]
        public void HashListOfListSameForSameMat()
        {
            var listA=new int[][]{
                new int[]{1,2,4,3,5},
                new int[]{2,4,3,5,1},
                new int[]{4,3,5,1,2},
                new int[]{3,5,1,2,4},
                new int[]{5,1,2,4,3}
            };
            var listB=new int[][]{
                new int[]{1,2,4,3,5},
                new int[]{2,4,3,5,1},
                new int[]{4,3,5,1,2},
                new int[]{3,5,1,2,4},
                new int[]{5,1,2,4,3}
            };
            Assert.AreEqual(ModelHashUtils.HashOfEnumerableOfEnumerable(listA),ModelHashUtils.HashOfEnumerableOfEnumerable(listB));
        }
        [TestMethod]
        public void HashListOfListDifferentByCol()
        {
            var listA=new int[][]{
                new int[]{1,2,4,3,5},
                new int[]{2,4,3,5,1},
                new int[]{4,3,5,1,2},
                new int[]{3,5,1,2,4},
                new int[]{5,1,2,4,3}
            };
            var listB=new int[][]{
                new int[]{4,1,2,3,5},
                new int[]{2,4,3,5,1},
                new int[]{4,3,5,1,2},
                new int[]{3,5,1,2,4},
                new int[]{5,1,2,4,3}
            };
            Assert.AreNotEqual(ModelHashUtils.HashOfEnumerableOfEnumerable(listA),ModelHashUtils.HashOfEnumerableOfEnumerable(listB));
        }
        [TestMethod]
        public void HashListOfListDifferentByRow()
        {
            var listA=new int[][]{
                new int[]{1,2,4,3,5},
                new int[]{2,4,3,5,1},
                new int[]{4,3,5,1,2},
                new int[]{3,5,1,2,4},
                new int[]{5,1,2,4,3}
            };
            var listB=new int[][]{
                new int[]{1,2,4,3,5},
                new int[]{3,5,1,2,4},
                new int[]{4,3,5,1,2},
                new int[]{5,1,2,4,3},
                new int[]{2,4,3,5,1}
            };
            Assert.AreNotEqual(ModelHashUtils.HashOfEnumerableOfEnumerable(listA),ModelHashUtils.HashOfEnumerableOfEnumerable(listB));
        }
        [TestMethod]
        public void HashBoolListSameForSameList()
        {
            bool x=true;
            bool o=false;
            bool[] listA={x,x,o,x,o};
            bool[] listB={x,x,o,x,o};
            Assert.AreEqual(ModelHashUtils.HashOfEnumerable(listA),ModelHashUtils.HashOfEnumerable(listB));
        }
        [TestMethod]
        public void HashBoolListDifferentForDifferentList()
        {
            bool x=true;
            bool o=false;
            bool[] listA={x,x,o,x,o};
            bool[] listB={x,o,o,x,o};
            Assert.AreNotEqual(ModelHashUtils.HashOfEnumerable(listA),ModelHashUtils.HashOfEnumerable(listB));
        }
    }
}