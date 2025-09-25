using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BilingualImageApp.Wpf.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BilingualImageApp.Tests.Integration
{
    [TestClass]
    public class QuickstartIntegrationTests
    {
        [TestMethod]
        public void CanSaveAndLoadTextPairsDocument()
        {
            // Arrange
            var filePath = Path.GetTempFileName() + ".json";
            var doc = new Document
            {
                Id = Path.GetFileName(filePath),
                TextPairs = new List<TextPair>
                {
                    new TextPair { Id = "1", Language1 = "Hallo", Language2 = "Hello", SourceImage = "img1.png" },
                    new TextPair { Id = "2", Language1 = "Dag", Language2 = "Day", SourceImage = "img1.png" }
                },
                Images = new List<Image> { new Image { Id = "img1.png" } }
            };
            var json = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);

            // Act
            var loadedJson = File.ReadAllText(filePath);
            var loadedDoc = JsonSerializer.Deserialize<Document>(loadedJson);

            // Assert
            Assert.IsNotNull(loadedDoc);
            Assert.AreEqual(2, loadedDoc.TextPairs.Count);
            Assert.AreEqual("Hallo", loadedDoc.TextPairs[0].Language1);
            Assert.AreEqual("Hello", loadedDoc.TextPairs[0].Language2);
            Assert.AreEqual("img1.png", loadedDoc.Images[0].Id);
        }
    }
}
