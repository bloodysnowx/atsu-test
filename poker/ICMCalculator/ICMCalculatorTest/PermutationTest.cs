using ICMCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ICMCalculatorTest
{
    
    
    /// <summary>
    ///PermutationTest のテスト クラスです。すべての
    ///PermutationTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class PermutationTest
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
        ///Permutation コンストラクター のテスト
        ///</summary>
        [TestMethod()]
        public void PermutationConstructorTest()
        {
            Permutation target = new Permutation();
            Assert.Inconclusive("TODO: ターゲットを確認するためのコードを実装してください");
        }

        /// <summary>
        ///Enumerate のテスト
        ///</summary>
        public void EnumerateTestHelper<T>()
        {
            Permutation target = new Permutation(); // TODO: 適切な値に初期化してください
            IEnumerable<T> nums = null; // TODO: 適切な値に初期化してください
            IEnumerable<T[]> expected = null; // TODO: 適切な値に初期化してください
            IEnumerable<T[]> actual;
            actual = target.Enumerate<T>(nums, 1);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("このテストメソッドの正確性を確認します。");
        }

        [TestMethod()]
        public void EnumerateTest()
        {
            // EnumerateTestHelper<GenericParameterHelper>();
            Permutation target = new Permutation();
            IEnumerable<int> nums = new int[] { 1, 2, 3 };
            // IEnumerable<int[]> expected = new List<int[]>{ new int[] { 1 } };
            IEnumerable<int[]> actual;
            actual = target.Enumerate<int>(nums, 2);
            foreach (int[] n in actual)
            {
                foreach (int x in n)
                {
                    System.Diagnostics.Debug.Write(x);
                    System.Diagnostics.Debug.Write(",");
                }
                System.Diagnostics.Debug.WriteLine("");
            }
            // Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///_GetPermutations のテスト
        ///</summary>
        public void _GetPermutationsTestHelper<T>()
        {
            Permutation_Accessor target = new Permutation_Accessor(); // TODO: 適切な値に初期化してください
            IEnumerable<T> perm = null; // TODO: 適切な値に初期化してください
            IEnumerable<T> nums = null; // TODO: 適切な値に初期化してください
            IEnumerable<T[]> expected = null; // TODO: 適切な値に初期化してください
            IEnumerable<T[]> actual;
            actual = target._GetPermutations<T>(perm, nums, 1);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("このテストメソッドの正確性を確認します。");
        }

        [TestMethod()]
        [DeploymentItem("ICMCalculator.dll")]
        public void _GetPermutationsTest()
        {
            _GetPermutationsTestHelper<GenericParameterHelper>();
        }
    }
}
