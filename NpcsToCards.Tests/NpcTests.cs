using MetX.Library;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace NpcsToCards
{
    public class NpcTests
    {
        [Fact]
        public void ReadAllTest()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.File.WriteAllText(@"testfile.txt", NpcTexts.FantacyAllies);
            using (var streamReader = mockFileSystem.File.OpenText("testfile.txt"))
            {
                var worker = new NpcReader(streamReader);

                // Act
                worker.ReadAll();

                // Assert
                Assert.Equal(334 - 252 + 1, worker.Npcs.Count);
                Assert.True(worker.Npcs.TrueForAll(x => x.QuoteFromNpc.IsNotEmpty()));
                Assert.True(worker.Npcs.TrueForAll(x => x.Appearance.IsNotEmpty()));
                Assert.True(worker.Npcs.TrueForAll(x => x.Roleplaying.IsNotEmpty()));
                Assert.True(worker.Npcs.TrueForAll(x => x.NpcId >= 252));
                Assert.True(worker.Npcs.TrueForAll(x => x.Traits.Count > 2));
            }
        }

        [Fact]
        public void ReadNextTest()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.File.WriteAllText(@"testfile.txt", @"

50
NPC Name
Title
""Quote from npc!""
Appearance: Appearance
Roleplaying: Roleplaying
Personality: Personality
Motivation: Motivation
Background: Background
Traits: (PB) Ancient, merchant

");
            var worker = new NpcReader(mockFileSystem.File.OpenText("testfile.txt"));
            var expected = new Npc(50, "NPC Name", "Title", "Quote from npc!", "Appearance: Appearance",
                "Roleplaying: Roleplaying",
                "Personality: Personality", "Motivation: Motivation", "Background: Background",
                "Traits: (PB) Ancient, merchant");

            // Act
            var actual = worker.ReadNext();

            // Assert
            Assert.Equal(expected.ToXml(), actual.ToXml());
            Assert.Null(worker.ReadNext());
        }
    }
}