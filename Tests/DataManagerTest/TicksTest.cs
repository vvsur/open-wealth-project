using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTest
{    
    /// <summary>
    ///This is a test class for TicksTest and is intended
    ///to contain all TicksTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TicksTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for BarExists
        ///</summary>
        [TestMethod()]
        public void BarExistsTest()
        {
            IDataManager data = new OpenWealth.Data.Data();
            //data.Init(); сейчас всё делается в конструкторе, но в будущем может понадобится

            ISymbol symbol = data.GetSymbol("BarExistsTest");
            IScale scale = data.GetScale(ScaleEnum.tick, 1);
            Ticks ticks = new Ticks(symbol, scale);

            int index;
            Assert.AreEqual(ticks.BarExists(777, out index), false);
            Assert.AreEqual(0, index);

            ticks.Add(null, new Bar(DateTime.Now, 10, 1, 1, 1, 1, 1));

            Assert.AreEqual(ticks.BarExists(5, out index), false);
            Assert.AreEqual(0, index);
            Assert.AreEqual(ticks.BarExists(10, out index), true);
            Assert.AreEqual(0, index);
            Assert.AreEqual(ticks.BarExists(20, out index), false);
            Assert.AreEqual(1, index);

            ticks.Add(null, new Bar(DateTime.Now, 20, 1, 1, 1, 1, 1));

            Assert.AreEqual(ticks.BarExists(5, out index), false);
            Assert.AreEqual(0, index);
            Assert.AreEqual(ticks.BarExists(10, out index), true);
            Assert.AreEqual(0, index);
            Assert.AreEqual(ticks.BarExists(15, out index), false);
            Assert.AreEqual(1, index);
            Assert.AreEqual(ticks.BarExists(20, out index), true);
            Assert.AreEqual(1, index);
            Assert.AreEqual(ticks.BarExists(25, out index), false);
            Assert.AreEqual(2, index);

        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            IDataManager data = new OpenWealth.Data.Data();
            //data.Init(); сейчас всё делается в конструкторе, но в будущем может понадобится

            ISymbol symbol = data.GetSymbol("AddTest");
            IScale scale = data.GetScale(ScaleEnum.tick, 1);
            Ticks ticks = new Ticks(symbol, scale);

            Assert.AreEqual(ticks.Count,0);
            IBar bar0 = new Bar(DateTime.Now, 10, 1, 1, 1, 1, 1);
            ticks.Add(null, bar0);
            Assert.AreEqual(ticks.Count, 1);
            Assert.AreEqual(ticks[0], bar0);

            IBar bar1 = new Bar(DateTime.Now, 20, 1, 1, 1, 1, 1);
            ticks.Add(null, bar1);
            Assert.AreEqual(ticks.Count, 2);
            Assert.AreEqual(ticks[1], bar1);

            IBar bar2 = new Bar(DateTime.Now, 15, 1, 1, 1, 1, 1);
            ticks.Add(null, bar2);
            Assert.AreEqual(ticks.Count, 3);
            Assert.AreEqual(ticks[0], bar0);
            Assert.AreEqual(ticks[1], bar2);
            Assert.AreEqual(ticks[2], bar1);

            IBar bar3 = new Bar(DateTime.Now, 15, 1, 1, 1, 1, 2);
            ticks.Add(null, bar2);
            Assert.AreEqual(ticks.Count, 3);
            Assert.AreEqual(ticks[0], bar0);
            Assert.AreEqual(ticks[1], bar2);
            Assert.AreEqual(ticks[2], bar1);
        }
    }
}
