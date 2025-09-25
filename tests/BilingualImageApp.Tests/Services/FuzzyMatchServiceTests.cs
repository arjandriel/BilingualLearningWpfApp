using BilingualImageApp.Wpf.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BilingualImageApp.Tests.Services
{
    [TestClass]
    public class FuzzyMatchServiceTests
    {
        [TestMethod]
        public void IsDuplicate_ReturnsTrue_ForSimilarStrings()
        {
            var service = new FuzzyMatchService();
            Assert.IsTrue(service.IsDuplicate("Hallo", "Hallo!", 80));
        }

        [TestMethod]
        public void IsDuplicate_ReturnsFalse_ForDifferentStrings()
        {
            var service = new FuzzyMatchService();
            Assert.IsFalse(service.IsDuplicate("Hallo", "World", 90));
        }

        [TestMethod]
        public void IsDuplicate_RespectsThreshold()
        {
            var service = new FuzzyMatchService();
            Assert.IsFalse(service.IsDuplicate("Hallo", "Hallo!", 100));
        }
    }
}
