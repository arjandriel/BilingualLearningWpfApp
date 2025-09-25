using System.Collections.Generic;
using BilingualImageApp.Wpf.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BilingualImageApp.Tests.Models
{
    [TestClass]
    public class DocumentTests
    {
        [TestMethod]
        public void CanCreateDocumentWithTextPairsAndImages()
        {
            var doc = new Document
            {
                Id = "doc1.json",
                TextPairs = new List<TextPair> { new TextPair { Id = "1" } },
                Images = new List<Image> { new Image { Id = "img1.png" } }
            };
            Assert.AreEqual("doc1.json", doc.Id);
            Assert.AreEqual(1, doc.TextPairs.Count);
            Assert.AreEqual(1, doc.Images.Count);
        }
    }
}
