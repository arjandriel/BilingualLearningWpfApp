using BilingualImageApp.Wpf.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BilingualImageApp.Tests.Models
{
    [TestClass]
    public class TextPairTests
    {
        [TestMethod]
        public void CanCreateTextPairWithProperties()
        {
            var pair = new TextPair
            {
                Id = "1",
                Language1 = "Hallo",
                Language2 = "Hello",
                SourceImage = "img1.png",
                FuzzyScore = 98.5
            };
            Assert.AreEqual("1", pair.Id);
            Assert.AreEqual("Hallo", pair.Language1);
            Assert.AreEqual("Hello", pair.Language2);
            Assert.AreEqual("img1.png", pair.SourceImage);
            Assert.AreEqual(98.5, pair.FuzzyScore);
        }
    }
}
