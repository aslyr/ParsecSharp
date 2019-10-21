#if !DEBUG
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ParsecSharp.Parser;

namespace UnitTest.ParsecSharp
{
    [TestClass]
    [TestCategory("SkipWhenLiveUnitTesting")]
    public class StackOverflowTest
    {
        [TestMethod]
        public void SimpleRecursionStackOverflowTest()
        {
            // 単純な再帰ループ
            var parser = SkipMany(Any<int>());
            var source = new int[1000000];

            parser.Parse(source);
        }

        [TestMethod]
        public void ValueTypesStackOverflowTest()
        {
            // トークンが値型の場合
            var parser = Many(Any<(int, int, int)>());
            var source = Enumerable.Range(0, 1000000).Select(x => (x, x, x));

            parser.Parse(source);
        }

        [TestMethod]
        public void ReferenceTypesStackOverflowTest()
        {
            // トークンが参照型の場合
            var parser = Many(Any<Tuple<int, int, int>>());
            var source = Enumerable.Range(0, 1000000).Select(x => Tuple.Create(x, x, x));

            parser.Parse(source);
        }
    }
}
#endif
