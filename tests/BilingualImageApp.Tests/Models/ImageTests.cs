using System;
using BilingualImageApp.Wpf.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BilingualImageApp.Tests.Models
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void CanCreateImageWithProperties()
        {
            var image = new Image
            {
                Id = "test.png",
                DateAdded = DateTime.Now,
                RecognitionStatus = RecognitionStatus.Recognized
            };
            Assert.AreEqual("test.png", image.Id);
            Assert.AreEqual(RecognitionStatus.Recognized, image.RecognitionStatus);
        }
    }
}
