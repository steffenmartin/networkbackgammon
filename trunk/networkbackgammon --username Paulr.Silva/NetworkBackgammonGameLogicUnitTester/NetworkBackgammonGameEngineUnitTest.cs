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
    /// Various tests to verify functionality and validity of the game engine
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
            // Check whether players have actually been created
            Assert.IsNotNull(player1, "Player 1 object hasn't been created");
            Assert.IsNotNull(player1, "Player 2 object hasn't been created");

            // Initialize both players checkers to their intial configuration
            // (start positions)
            player1.InitCheckers();
            player2.InitCheckers();

            dice = new NetworkBackgammonDice[] { new NetworkBackgammonDice(), new NetworkBackgammonDice() };

            // Check whether dice have actually been created
            Assert.IsNotNull(dice, "Dice objects haven't been created");
        }

        /// <summary>
        /// Verifies whether the active player's initial possible moves are valid for all possible initial dice combinations
        /// </summary>
        [TestMethod]
        public void TestMethod_VerifyInitialPossibleMoves()
        {
            // Configure players
            player1.Active = true;
            player2.Active = false;

            // Set reference to active player
            NetworkBackgammonPlayer activePlayer = player1.Active ? player1 : player2;

            // Loop through all possible dice combinations
            foreach (NetworkBackgammonDice.DiceValue diceValueOne in Enum.GetValues(typeof(NetworkBackgammonDice.DiceValue)))
            {
                if (diceValueOne >= NetworkBackgammonDice.DiceValue.MIN &&
                    diceValueOne <= NetworkBackgammonDice.DiceValue.MAX)
                {
                    foreach (NetworkBackgammonDice.DiceValue diceValueTwo in Enum.GetValues(typeof(NetworkBackgammonDice.DiceValue)))
                    {
                        if (diceValueTwo >= NetworkBackgammonDice.DiceValue.MIN &&
                            diceValueTwo <= NetworkBackgammonDice.DiceValue.MAX)
                        {
                            // Set (not random) dice values
                            dice[0].CurrentValue = diceValueOne;
                            dice[1].CurrentValue = diceValueTwo;

                            bool activePlayerHasPossibleMoves =
                                NetworkBackgammonGameEngine.CalculatePossibleMoves(
                                ref player1,
                                ref player2,
                                dice);

                            Assert.IsTrue(activePlayerHasPossibleMoves, "Active player should always have possible moves after the winning the initial dice roll");

                            Stream stream = GetManifestResourceStreamByName("InitialCheckers_" +
                                                                             (dice[0].CurrentValueUInt32 <= dice[1].CurrentValueUInt32 ?
                                                                                dice[0].CurrentValueUInt32.ToString() :
                                                                                dice[1].CurrentValueUInt32.ToString()) +
                                                                            "_" +
                                                                             (dice[1].CurrentValueUInt32 >= dice[0].CurrentValueUInt32 ?
                                                                                dice[1].CurrentValueUInt32.ToString() :
                                                                                dice[0].CurrentValueUInt32.ToString()) +
                                                                            ".xml");

                            Assert.IsNotNull(stream, "Data (XML) for verification comparison purposes is missing");

                            List<NetworkBackgammonChecker> checkersVerified = LoadCheckersFromXMLFile(stream);

                            string checkerVerificationMessage = "";
                            bool checkerVerificationResult = VerifyCheckersAndPossibleMoves(activePlayer.Checkers, checkersVerified, ref checkerVerificationMessage);

                            Assert.IsTrue(checkerVerificationResult, "Possible moves of active player for initial dice roll of " + dice[0] + ", " + dice[1] + " incorrect. Detail: " + checkerVerificationMessage);
                        }
                    }
                }
            }
        }

        #region Tests for calculation of possible moves

        // Abbreviations:
        //
        // VBC: Verification of Boundary Conditions

        /// <summary>
        /// Verifies whether the game engine handles the (errorneous) case of 2 active players properly
        /// </summary>
        /// <remarks>
        /// Expected behavior: Throws a NetworkBackgammonGameEngineException
        /// </remarks>
        [TestMethod]
        public void TestMethod_VBC_PossibleMoveCalculations_BothPlayersActive()
        {
            try
            {
                // Set both player to active
                player1.Active = true;
                player2.Active = true;

                dice[0].CurrentValue = NetworkBackgammonDice.DiceValue.MIN;
                dice[1].CurrentValue = NetworkBackgammonDice.DiceValue.MIN;

                // Should throw a NetworkBackgammonGameEngineException
                bool retVal = NetworkBackgammonGameEngine.CalculatePossibleMoves(ref player1, ref player2, dice);

                Assert.Fail("Calculation of possible moves returned ( " + retVal + " but should have thrown a NetworkBackgammonGameEngineException instead");
            }
            catch (NetworkBackgammonGameEngineException)
            {
                // TODO: Check the details of the exception
            }
        }

        /// <summary>
        /// Verifies whether the game engine handles the (errorneous) case of more than 2 dice being passed in
        /// </summary>
        /// <remarks>
        /// Expected behavior: Throws a NetworkBackgammonGameEngineException
        /// </remarks>
        [TestMethod]
        public void TestMethod_VBC_PossibleMovesCalculations_MoreThanTwoDice()
        {
            try
            {
                player1.Active = true;
                player2.Active = false;

                // Create dice array with 3 elements
                NetworkBackgammonDice[] diceForTest = new NetworkBackgammonDice[] { new NetworkBackgammonDice(), new NetworkBackgammonDice(), new NetworkBackgammonDice() };

                // Should throw a NetworkBackgammonGameEngineException
                bool retVal = NetworkBackgammonGameEngine.CalculatePossibleMoves(ref player1, ref player2, diceForTest);

                Assert.Fail("Calculation of possible moves returned ( " + retVal + " but should have thrown a NetworkBackgammonGameEngineException instead");
            }
            catch (NetworkBackgammonGameEngineException)
            {
                // TODO: Check the details of the exception
            }
        }

        /// <summary>
        /// Verifies whether the game engine handles the (errorneous) case of a 0-element dice array being passed in
        /// </summary>
        /// <remarks>
        /// Expected behavior: Throws a NetworkBackgammonGameEngineException
        /// </remarks>
        [TestMethod]
        public void TestMethod_VBC_PossibleMovesCalculations_ZeroDice()
        {
            try
            {
                player1.Active = true;
                player2.Active = false;

                // Create dice array with 0 elements
                NetworkBackgammonDice[] diceForTest = new NetworkBackgammonDice[] { };

                // Should throw a NetworkBackgammonGameEngineException
                bool retVal = NetworkBackgammonGameEngine.CalculatePossibleMoves(ref player1, ref player2, diceForTest);

                Assert.Fail("Calculation of possible moves returned ( " + retVal + " but should have thrown a NetworkBackgammonGameEngineException instead");
            }
            catch (NetworkBackgammonGameEngineException)
            {
                // TODO: Check the details of the exception
            }
        }

        /// <summary>
        /// Verifies whether the game engine handles the (errorneous) case of a null reference being passed in for dice
        /// </summary>
        /// <remarks>
        /// Expected behavior: Throws a NetworkBackgammonGameEngineException
        /// </remarks>
        [TestMethod]
        public void TestMethod_VBC_PossibleMovesCalculations_NullDice()
        {
            try
            {
                player1.Active = true;
                player2.Active = false;

                // Should throw a NetworkBackgammonGameEngineException
                bool retVal = NetworkBackgammonGameEngine.CalculatePossibleMoves(ref player1, ref player2, null);

                Assert.Fail("Calculation of possible moves returned ( " + retVal + " but should have thrown a NetworkBackgammonGameEngineException instead");
            }
            catch (NetworkBackgammonGameEngineException)
            {
                // TODO: Check the details of the exception
            }
        }

        /// <summary>
        /// Verifies whether the game engine handles the (errorneous) case of a invalid dice value being passed in
        /// </summary>
        /// <remarks>
        /// Expected behavior: Throws a NetworkBackgammonGameEngineException
        /// </remarks>
        [TestMethod]
        public void TestMethod_VBC_PossibleMovesCalculations_InvalidDice()
        {
            try
            {
                player1.Active = true;
                player2.Active = false;

                dice[0].CurrentValue = NetworkBackgammonDice.DiceValue.MIN;
                dice[1].CurrentValue = NetworkBackgammonDice.DiceValue.INVALID;

                // Should throw a NetworkBackgammonGameEngineException
                bool retVal = NetworkBackgammonGameEngine.CalculatePossibleMoves(ref player1, ref player2, dice);

                Assert.Fail("Calculation of possible moves returned ( " + retVal + " but should have thrown a NetworkBackgammonGameEngineException instead");
            }
            catch (NetworkBackgammonGameEngineException)
            {
                // TODO: Check the details of the exception
            }
        }

        #endregion

        #endregion

        #region Utility Methods

        /// <summary>
        /// Queries the assembly for a resource file stream
        /// </summary>
        /// <param name="resourceName">Name of the resource (e.g. file or image)</param>
        /// <returns>File stream for the requested resource (null if resource couldn't be found)</returns>
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

        /// <summary>
        /// Loads (de-serializes) a list of checkers from an XML stream (e.g. file stream)
        /// </summary>
        /// <param name="stream">Stream with XML data to read</param>
        /// <returns>(De-serialized) list of checkers from the XML stream</returns>
        private List<NetworkBackgammonChecker> LoadCheckersFromXMLFile(Stream stream)
        {
            List<NetworkBackgammonChecker> retVal = null;

            if (stream != null)
            {
                TextReader textReader = null;

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NetworkBackgammonChecker>));

                textReader = new StreamReader(stream);

                retVal = (List<NetworkBackgammonChecker>)xmlSerializer.Deserialize(textReader);

                textReader.Close();
            }

            return retVal;
        }

        /// <summary>
        /// Verifies a list of checkers and possible moves against a verified list of checkers and possible moves
        /// </summary>
        /// <param name="checkersToVerify">List of checkers and possible moves to be verified</param>
        /// <param name="checkersVerified">List of verified checkers and possible moves</param>
        /// <param name="returnMessage">Additional detail information if verification fails</param>
        /// <returns>"True" if both lists (checkers and possible moves) match, otherwise "false"</returns>
        private bool VerifyCheckersAndPossibleMoves(List<NetworkBackgammonChecker> checkersToVerify,
                                                    List<NetworkBackgammonChecker> checkersVerified,
                                                    ref string returnMessage)
        {
            bool bRetVal = false;

            // Does the number of checkers match?
            bRetVal = checkersVerified.Count == checkersToVerify.Count;

            if (bRetVal)
            {
                List<NetworkBackgammonChecker> checkersCheckOffList = new List<NetworkBackgammonChecker>();

                // Build a list of checkers that need to be verified (and checked off once verified)
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

                                // Sort possible moves
                                checkerVerified.PossibleMoves.Sort();
                                checkerToVerify.PossibleMoves.Sort();

                                // Verify whether the list of actual possible moves matches
                                for (int i = 0; i < checkerVerified.PossibleMoves.Count; i++)
                                {
                                    // Found one non-matching item
                                    if (checkerVerified.PossibleMoves[i].CurrentValue != checkerToVerify.PossibleMoves[i].CurrentValue)
                                    {
                                        // So, we cannot check it off
                                        checkerToCheckoff = null;

                                        returnMessage += "Not all possible moves of verified checkers and checkers to be verified match for position " +
                                                         checkerVerified.CurrentPosition.ToString() + ". Verified checker has possible move " +
                                                         checkerVerified.PossibleMoves[i].CurrentValue.ToString() +
                                                         " at this position. However, checker to be verified has possible move " +
                                                         checkerToVerify.PossibleMoves[i].CurrentValue.ToString() + ".";

                                        bRetVal = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                returnMessage += "Number of possible moves (" +
                                                  checkerToVerify.PossibleMoves.Count() +
                                                  ") for checker to be verified doesn't match number of possible moves (" +
                                                  checkerVerified.PossibleMoves.Count() +
                                                  ") for verified checker at position " + checkerVerified.CurrentPosition.ToString() + ".";
                                
                                bRetVal = false;
                            }

                            break;
                        }
                    }

                    if (checkerToCheckoff != null)
                    {
                        checkersCheckOffList.Remove(checkerToCheckoff);
                        checkerToCheckoff = null;
                    }
                    else
                    {
                        bRetVal = false;
                        break;
                    }
                }
            }
            else
            {
                returnMessage += "Number of checkers to be verified (" + checkersToVerify.Count + ") doesn't match number of verified checkers (" + checkersVerified.Count + ")";
            }

            return bRetVal;
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
