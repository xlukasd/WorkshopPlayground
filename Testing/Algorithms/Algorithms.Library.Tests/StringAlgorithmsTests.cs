namespace Algorithms.Library.Tests
{
    public class Tests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
        }

        [SetUp]
        public void Setup()
        {
        }
        
        [TearDown]
        public void TearDown()
        {
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }

        [Test]
        public void CountVowelsInWordWorksCorrectly()
        {
            //Arrange
            const string word = "conference";
            const int expectedVowelCount = 4;

            //Act
            int vowelCount = StringAlgorithms.CountVowels(word);

            //Assert
            Assert.That(vowelCount, Is.EqualTo(expectedVowelCount));
        }
    }
}
