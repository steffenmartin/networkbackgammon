using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkBackgammonLib;
using NetworkBackgammonGameLogic;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace NetworkBackgammonGameLogicUnitTester
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class NetworkBackgammonGameEngineUnitTest
    {
        
        #region Members

        /// <summary>
        /// Test context
        /// </summary>
        private TestContext testContextInstance;

        /// <summary>
        /// Backgammon test player 1
        /// </summary>
        private NetworkBackgammonPlayer player1 = new NetworkBackgammonPlayer("Player1");

        /// <summary>
        /// Backgammon test player 2
        /// </summary>
        private NetworkBackgammonPlayer player2 = new NetworkBackgammonPlayer("Player2");

        /// <summary>
        /// The dice for this game logic test
        /// </summary>
        private NetworkBackgammonDice[] dice = null;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkBackgammonGameEngineUnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Test Methods

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void Initialize()
        {
            // Initialize both players checkers to their intial configuration
            // (start positions)
            player1.InitCheckers();
            player2.InitCheckers();

            dice = new NetworkBackgammonDice[] { new NetworkBackgammonDice(), new NetworkBackgammonDice() };
        }

        /// <summary>
        /// Verifies whether the active player's initial possible moves are valid
        /// </summary>
        [TestMethod]
        public void TestMethod_VerifyInitialPossibleMoves()
        {
            // Check whether players have actually been created
            Assert.IsNotNull(player1, "Player 1 object hasn't been created");
            Assert.IsNotNull(player1, "Player 2 object hasn't been created");

            // Check whether dice have actually been created
            Assert.IsNotNull(dice, "Dice objects haven't been created");

            // Set (not random) dice values
            dice[0].CurrentValue = NetworkBackgammonDice.DiceValue.ONE;
            dice[1].CurrentValue = NetworkBackgammonDice.DiceValue.ONE;

            // Configure players
            player1.Active = true;
            player2.Active = false;

            NetworkBackgammonPlayer activePlayer = player1.Active ? player1 : player2;

            bool activePlayerHasPossibleMoves =
                NetworkBackgammonGameEngine.CalculatePossibleMoves(
                    ref player1,
                    ref player2,
                    dice);

            Assert.IsTrue(activePlayerHasPossibleMoves, "Active player should always have possible moves after the winning the initial dice roll");

            Stream stream = GetManifestResourceStreamByName("InitialCheckers_" +
                                                            dice[0].CurrentValueUInt32.ToString() +
                                                            "_" +
                                                            dice[1].CurrentValueUInt32.ToString() +
                                                            ".xml");

            Assert.IsNotNull(stream, "Data (XML) for verification comparison purposes is missing");
            
            List<NetworkBackgammonChecker> checkersVerified = LoadCheckersFromXMLFile(stream);

            Assert.IsTrue(VerifyCheckersAndPossibleMoves(activePlayer.Checkers, checkersVerified), "Possible moves of active player for initial dice roll of " + dice[0] + ", " + dice[1] + " incorrect");
        }

        #endregion

        #region Utility Methods

        private Stream GetManifestResourceStreamByName(string resourceName)
        {
            Stream retVal = null;

            foreach (string s in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (s.EndsWith(resourceName))
                {
                    retVal = Assembly.GetExecutingAssembly().GetManifestResourceStream(s);
                    break;
                }
            }

            return retVal;
        }

        private List<NetworkBackgammonChecker> LoadCheckersFromXMLFile(Stream stream)
        {
            List<NetworkBackgammonChecker> retVal = null;

            TextReader textReader = null;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NetworkBackgammonChecker>));

            textReader = new StreamReader(stream);

            retVal = (List<NetworkBackgammonChecker>)xmlSerializer.Deserialize(textReader);

            textReader.Close();

            return retVal;
        }

        private bool VerifyCheckersAndPossibleMoves(List<NetworkBackgammonChecker> checkersToVerify, List<NetworkBackgammonChecker> checkersVerified)
        {
            bool bRetVal = false;

            // Does the number of checkers match?
            bRetVal = checkersVerified.Count == checkersToVerify.Count;

            if (bRetVal)
            {
                List<NetworkBackgammonChecker> checkersCheckOffList = new List<NetworkBackgammonChecker>();

                // Build a list of checkers that need to be verified (and checked off one verified)
                foreach (NetworkBackgammonChecker checker in checkersVerified)
                {
                    checkersCheckOffList.Add(checker);
                }

                // Loop through the list of checkers to be verified
                foreach (NetworkBackgammonChecker checkerToVerify in checkersToVerify)
                {
                    NetworkBackgammonChecker checkerToCheckoff = null; 

                    // Loop through the list of verified checkers to find the matching one
                    foreach (NetworkBackgammonChecker checkerVerified in checkersCheckOffList)
                    {
                        // Found matching checker?
                        if (checkerVerified.CurrentPosition == checkerToVerify.CurrentPosition)
                        {
                            // Number of possible moves matching?
                            if (checkerVerified.PossibleMoves.Count == checkerToVerify.PossibleMoves.Count)
                            {
                                // We can potentially check this one off, unless we find the actual possible moves list not matching
                                checkerToCheckoff = checkerVerified;

                                // Verify whether the list of actual possible moves matches
                                for (int i = 0; i < checkerVerified.PossibleMoves.Count; i++)
                                {
                                    // Found one non-matching item
                                    if (checkerVerified.PossibleMoves[i].CurrentValue != checkerToVerify.PossibleMoves[i].CurrentValue)
                                    {
                                        // So, we cannot check it off
                                        checkerToCheckoff = null;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (checkerToCheckoff != null)
                    {
                        checkersCheckOffList.Remove(checkerToCheckoff);
                    }
                    else
                    {
                        bRetVal = false;
                        break;
                    }
                }
            }

            // return bRetVal;
            return false;
        }

        #endregion

        #endregion

        #region Properties

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

        #endregion

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
    }
}
