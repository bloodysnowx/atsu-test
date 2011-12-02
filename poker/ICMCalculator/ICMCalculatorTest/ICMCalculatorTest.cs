using ICMCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ICMCalculatorTest
{
    
    
    /// <summary>
    ///ICMCalculatorTest のテスト クラスです。すべての
    ///ICMCalculatorTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class ICMCalculatorTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 追加のテスト属性
        // 
        //テストを作成するときに、次の追加属性を使用することができます:
        //
        //クラスの最初のテストを実行する前にコードを実行するには、ClassInitialize を使用
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //クラスのすべてのテストを実行した後にコードを実行するには、ClassCleanup を使用
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //各テストを実行する前にコードを実行するには、TestInitialize を使用
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //各テストを実行した後にコードを実行するには、TestCleanup を使用
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///CalcPermutationProbability のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ICMCalculator.dll")]
        public void CalcPermutationProbabilityTest()
        {
            ICMCalculator_Accessor target = new ICMCalculator_Accessor();
            int[] permutations = { 0, 1 };
            int[] chips = { 100, 100 };
            double expected = 0.5F;
            double actual;
            actual = target.CalcPermutationProbability(permutations, chips);
            Assert.AreEqual(expected, actual);

            permutations = new int[]{ 0, 1, 2 };
            chips = new int[]{ 100, 100, 100 };
            expected = 1.0 / 6.0;
            actual = target.CalcPermutationProbability(permutations, chips);
            Assert.AreEqual(expected, actual);

            permutations = new int[] { 0, 1 };
            chips = new int[] { 200, 100 };
            expected = 2.0 / 3.0;
            actual = target.CalcPermutationProbability(permutations, chips);
            Assert.AreEqual(expected, actual);

            permutations = new int[] { 0, 1, 2 };
            chips = new int[] { 200, 100, 100 };
            expected = 1.0 / 4.0;
            actual = target.CalcPermutationProbability(permutations, chips);
            Assert.AreEqual(expected, actual);

            permutations = new int[] { 0, 1, 2 };
            chips = new int[] { 200, 400, 100 };
            expected = 2.0 / 7.0 * 4.0 / 5.0;
            actual = target.CalcPermutationProbability(permutations, chips);
            Assert.AreEqual(expected, actual);

            permutations = new int[] { 1, 3, 0, 2 };
            chips = new int[] { 500, 800, 1000, 300 };
            expected = 8.0 / 26.0 * 3.0 / 18.0 * 5.0 / 15.0;
            actual = target.CalcPermutationProbability(permutations, chips);
            Assert.IsTrue(expected * 0.999 < actual);
            Assert.IsTrue(expected * 1.001 > actual);

            permutations = new int[] { 1, 3, 2, 0 };
            chips = new int[] { 500, 800, 1000, 300, 700 };
            expected = 8.0 / 33.0 * 3.0 / 25.0 * 10.0 / 22.0 * 5.0 / 12.0;
            actual = target.CalcPermutationProbability(permutations, chips);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///CalcEV のテスト
        ///</summary>
        [TestMethod()]
        public void CalcEVTest()
        {
            ICMCalculator.ICMCalculator target = new ICMCalculator.ICMCalculator();
            int[] structure = { 100 };
            int[] chips = { 1000, 1000 };
            double[] expected = { 50.0, 50.0 };
            double[] actual;
            actual = target.CalcEV(structure, chips);
            for(int i = 0; i < expected.Length; ++i)
                Assert.AreEqual(expected[i], actual[i]);

            structure = new int[]{ 100 };
            chips = new int[] { 1000, 1000, 500 };
            expected = new double[]{ 40.0, 40.0, 20.0 };
            actual = target.CalcEV(structure, chips);
            for (int i = 0; i < expected.Length; ++i)
                Assert.AreEqual(expected[i], actual[i]);

            structure = new int[] { 60, 40 };
            chips = new int[] { 1000, 1000, 500 };
            expected = new double[] { 10.0/25.0 * 60 + (10.0/25.0 * 10.0/15.0 + 5.0/25.0 * 10.0/20.0) * 40,
                10.0/25.0 * 60 + (10.0/25.0 * 10.0/15.0 + 5.0/25.0 * 10.0/20.0) * 40,
                5.0/25.0 * 60 + (10.0/25.0 * 5.0/15.0 + 10.0/25.0 * 5.0/15.0) * 40
            };
            actual = target.CalcEV(structure, chips);
            for (int i = 0; i < expected.Length; ++i)
                Assert.AreEqual(expected[i], actual[i]);
        }
    }
}
