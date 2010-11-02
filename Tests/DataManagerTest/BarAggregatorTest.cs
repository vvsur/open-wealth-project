using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTest
{
    /// <summary>
    ///This is a test class for BarAggregatorTest and is intended
    ///to contain all BarAggregatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BarAggregatorTest
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
        ///A test for TimeAlignment
        ///</summary>
        [TestMethod()]
        public void TimeAlignmentTest()
        {
            IDataManager data = new Data();
            ISymbol symbol = new Symbol(data.GetMarket("testMarket"),"test");

            AggregateBars target1 = new AggregateBars(data, symbol,
                                                 new Scale(ScaleEnum.sec, 10));

            Assert.AreEqual(target1.TimeAlignment(new DateTime(2010,09,16,12,39,35)) ,
                                                 new DateTime(2010,09,16,12,39,30));

            Assert.AreEqual(target1.TimeAlignment(new DateTime(2010, 09, 16, 12, 39, 20)),
                                                 new DateTime(2010, 09, 16, 12, 39, 20));


            AggregateBars target2 = new AggregateBars(data, symbol,
                                                 new Scale(ScaleEnum.sec, 1));

            Assert.AreEqual(target2.TimeAlignment(new DateTime(2010, 09, 16, 12, 39, 35)),
                                                 new DateTime(2010, 09, 16, 12, 39, 35));

            Assert.AreEqual(target2.TimeAlignment(new DateTime(2010, 09, 16, 12, 39, 25)),
                                                 new DateTime(2010, 09, 16, 12, 39, 25));

            AggregateBars target3 = new AggregateBars(data, symbol,
                                                 new Scale(ScaleEnum.sec, 60));

            Assert.AreEqual(target3.TimeAlignment(new DateTime(2010, 09, 16, 12, 39, 35)),
                                                 new DateTime(2010, 09, 16, 12, 39, 0));

            Assert.AreEqual(target3.TimeAlignment(new DateTime(2010, 09, 16, 12, 39, 0)),
                                                 new DateTime(2010, 09, 16, 12, 39, 0));
        }
    }
}
