using _27DrawStartingHandCalc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test27DrawStartingHandCalc
{
    
    
    /// <summary>
    ///DeckTest のテスト クラスです。すべての
    ///DeckTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class DeckTest
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
        ///Deck コンストラクター のテスト
        ///</summary>
        [TestMethod()]
        public void DeckConstructorTest()
        {
            Deck target = new Deck();
            Assert.Inconclusive("TODO: ターゲットを確認するためのコードを実装してください");
        }

        /// <summary>
        ///DrawA のテスト
        ///</summary>
        [TestMethod()]
        public void DrawATest()
        {
            Deck target = new Deck(); // TODO: 適切な値に初期化してください
            int expected = 0; // TODO: 適切な値に初期化してください
            int actual;
            actual = target.DrawA();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("このテストメソッドの正確性を確認します。");
        }

        /// <summary>
        ///DrawB のテスト
        ///</summary>
        [TestMethod()]
        public void DrawBTest()
        {
            Deck target = new Deck(); // TODO: 適切な値に初期化してください
            int expected = 0; // TODO: 適切な値に初期化してください
            int actual;
            actual = target.DrawB();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("このテストメソッドの正確性を確認します。");
        }

        /// <summary>
        ///Reset のテスト
        ///</summary>
        [TestMethod()]
        public void ResetTest()
        {
            Deck target = new Deck(); // TODO: 適切な値に初期化してください
            target.Reset();
            Assert.Inconclusive("値を返さないメソッドは確認できません。");
        }
    }
}
