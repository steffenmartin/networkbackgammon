using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkBackgammonRemotingLib;

namespace TestPlayerList
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTestPlayerList
    {
        public static NetworkBackgammonPlayerList myList;

        public UnitTestPlayerList()
        {
            myList = new NetworkBackgammonPlayerList();
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            myList.AddPlayer("Player1", "Password1");
            myList.AddPlayer("Player2", "Password2");
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestVerifyPlayer()
        {
            // Test if Player1 is in list
            Assert.IsFalse(myList.AddPlayer("Player1", "Password1"));
            Assert.IsTrue(myList.AddPlayer("Player3", "Password3"));
        }

        [TestMethod]
        public void TestPassword()
        {
            // Test if password works
            Assert.IsTrue(myList.VerifyLogin("Player1","Password1"));
            Assert.IsFalse(myList.VerifyLogin("Player1","BadPassword1"));
        }

        [TestMethod]
        public void TestEmptyArgs()
        {
            // Test if password works
            Assert.IsFalse(myList.VerifyLogin("", "Password1"));
            Assert.IsFalse(myList.VerifyLogin("Player1", ""));
        }
    }
}
