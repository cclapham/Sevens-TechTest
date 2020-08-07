using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using Sevens.Game.Engine;
using Sevens.Game.Engine.Concrete;
using Sevens.Game.Engine.Enums;
using Sevens.Game.Engine.Interfaces;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;

namespace Sevens.Game.Tests.Engine
{
    [TestFixture]
    public class SevensGameEngineTests
    {
        private MockRepository _mockRepository;
        private IFixture _fixture;

        private Mock<IMemoryCache> _memCache;

        private IGameBoard _board;

        [SetUp]
        public void SetUp()
        {
            // Boilerplate
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _fixture = new Fixture();

            //Prevent fixture from generating from entity circular references
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            // Mock setup
            _memCache = _mockRepository.Create<IMemoryCache>();

            SetUpMockDefaults();

            //Sut Instantiation
            _board = new GameBoard(_memCache.Object);
        }

        private void SetUpMockDefaults()
        {
            _memCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);
        }

        [Test]
        public void GetGameBoardState_ReturnsNewState()
        {
            //Arrange
            object dummy;
            _memCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out dummy)).Returns(false);

            IDictionary<Suits, InGameSuit> expected = new Dictionary<Suits, InGameSuit>
            {
                {Suits.Diamonds, new InGameSuit(Suits.Diamonds) },
                {Suits.Spades, new InGameSuit(Suits.Spades) },
                {Suits.Hearts, new InGameSuit(Suits.Hearts) },
                {Suits.Clubs, new InGameSuit(Suits.Clubs) }
            };

            //Act
            var result = _board.GetGameBoardState();

            //Assert
            result.Should().BeEquivalentTo(expected);
            result.Count.Should().Be(expected.Count);

            result[Suits.Diamonds].LowValue.Should().Be(expected[Suits.Diamonds].LowValue);
            result[Suits.Diamonds].HighValue.Should().Be(expected[Suits.Diamonds].HighValue);

            result[Suits.Spades].LowValue.Should().Be(expected[Suits.Spades].LowValue);
            result[Suits.Spades].HighValue.Should().Be(expected[Suits.Spades].HighValue);

            result[Suits.Hearts].LowValue.Should().Be(expected[Suits.Hearts].LowValue);
            result[Suits.Hearts].HighValue.Should().Be(expected[Suits.Hearts].HighValue);

            result[Suits.Clubs].LowValue.Should().Be(expected[Suits.Clubs].LowValue);
            result[Suits.Clubs].HighValue.Should().Be(expected[Suits.Clubs].HighValue);
        }

        [Test]
        public void GetGameBoardState_RetrievesStateFromCache()
        {
            //Arrange
            object expectedValue = new Dictionary<Suits, InGameSuit>
            {
                {Suits.Diamonds, _fixture.Create<InGameSuit>() },
                {Suits.Spades, _fixture.Create<InGameSuit>() },
                {Suits.Hearts, _fixture.Create<InGameSuit>() },
                {Suits.Clubs, _fixture.Create<InGameSuit>() }
            };
            _memCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue)).Returns(true);

            //Act
            var result = _board.GetGameBoardState();

            //Assert
            IDictionary<Suits, InGameSuit> expected = expectedValue as IDictionary<Suits, InGameSuit>;

            result.Should().BeEquivalentTo(expected);
            result.Count.Should().Be(expected.Count);

            result[Suits.Diamonds].LowValue.Should().Be(expected[Suits.Diamonds].LowValue);
            result[Suits.Diamonds].HighValue.Should().Be(expected[Suits.Diamonds].HighValue);

            result[Suits.Spades].LowValue.Should().Be(expected[Suits.Spades].LowValue);
            result[Suits.Spades].HighValue.Should().Be(expected[Suits.Spades].HighValue);

            result[Suits.Hearts].LowValue.Should().Be(expected[Suits.Hearts].LowValue);
            result[Suits.Hearts].HighValue.Should().Be(expected[Suits.Hearts].HighValue);

            result[Suits.Clubs].LowValue.Should().Be(expected[Suits.Clubs].LowValue);
            result[Suits.Clubs].HighValue.Should().Be(expected[Suits.Clubs].HighValue);
        }


        [Test]
        public void SetGameBoardState_UpdatesMemoryCacheWithNewState()
        {
            //Arrange
            var cardToPlay = new Card(Suits.Clubs, CardValue.Eight);
            IDictionary<Suits, InGameSuit> expected = new Dictionary<Suits, InGameSuit>
            {
                {Suits.Diamonds, _fixture.Create<InGameSuit>() },
                {Suits.Spades, _fixture.Create<InGameSuit>() },
                {Suits.Hearts, _fixture.Create<InGameSuit>() },
                {Suits.Clubs, new InGameSuit(Suits.Clubs) }
            };

            expected[Suits.Clubs].HighValue = CardValue.Seven;
            expected[Suits.Clubs].LowValue = CardValue.Seven;
            expected[Suits.Clubs].IsOpen = true;

            object expectedValue = expected as object;
            _memCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue)).Returns(true);

            //Act
            _board.SetGameBoardState(cardToPlay);

            //Assert
            _memCache.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Once);
        }

        [Test]
        public void SetGameBoardState_ThrowsArgumentNullException()
        {
            //Arrange
            object expectedValue = new Dictionary<Suits, InGameSuit>
            {
                {Suits.Diamonds, _fixture.Create<InGameSuit>() },
                {Suits.Spades, _fixture.Create<InGameSuit>() },
                {Suits.Hearts, _fixture.Create<InGameSuit>() },
                {Suits.Clubs, _fixture.Create<InGameSuit>() }
            };
            _memCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue)).Returns(true);

            Card card = null;

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => _board.SetGameBoardState(card));

            //Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType(typeof(ArgumentNullException));
        }

        [Test]
        public void SetGameBoardState_ThrowsUnableToPlayCardException()
        {
            //Arrange
            var cardToPlay = new Card(Suits.Clubs, CardValue.King);

            var expectedMessage = $"The card \"{cardToPlay.GetFriendlyName()}\" cannot be played at this time";

            IDictionary<Suits, InGameSuit> expected = new Dictionary<Suits, InGameSuit>
            {
                {Suits.Diamonds, _fixture.Create<InGameSuit>() },
                {Suits.Spades, _fixture.Create<InGameSuit>() },
                {Suits.Hearts, _fixture.Create<InGameSuit>() },
                {Suits.Clubs, new InGameSuit(Suits.Clubs) }
            };

            expected[Suits.Clubs].HighValue = CardValue.Seven;
            expected[Suits.Clubs].LowValue = CardValue.Seven;
            expected[Suits.Clubs].IsOpen = true;

            object expectedValue = expected as object;
            _memCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue)).Returns(true);


            //Act
            var exception = Assert.Throws<UnableToPlayCardException>(() => _board.SetGameBoardState(cardToPlay));

            //Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType(typeof(UnableToPlayCardException));

            exception.Message.Should().Be(expectedMessage);
        }

        [Test]
        public void IsEligible_returnsTrue()
        {
            //Arrange
            var cardToPlay = new Card(Suits.Clubs, CardValue.Eight);
            IDictionary<Suits, InGameSuit> expected = new Dictionary<Suits, InGameSuit>
            {
                {Suits.Diamonds, _fixture.Create<InGameSuit>() },
                {Suits.Spades, _fixture.Create<InGameSuit>() },
                {Suits.Hearts, _fixture.Create<InGameSuit>() },
                {Suits.Clubs, new InGameSuit(Suits.Clubs) }
            };

            expected[Suits.Clubs].HighValue = CardValue.Seven;
            expected[Suits.Clubs].LowValue = CardValue.Seven;
            expected[Suits.Clubs].IsOpen = true;

            object expectedValue = expected as object;
            _memCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue)).Returns(true);

            //Act
            var eligible = _board.IsEligible(cardToPlay);

            //Assert
            eligible.Should().BeTrue();
        }

        [Test]
        public void IsEligible_returnsFalse()
        {

            //Arrange
            var cardToPlay = new Card(Suits.Clubs, CardValue.King);
            IDictionary<Suits, InGameSuit> expected = new Dictionary<Suits, InGameSuit>
            {
                {Suits.Diamonds, _fixture.Create<InGameSuit>() },
                {Suits.Spades, _fixture.Create<InGameSuit>() },
                {Suits.Hearts, _fixture.Create<InGameSuit>() },
                {Suits.Clubs, new InGameSuit(Suits.Clubs) }
            };

            expected[Suits.Clubs].HighValue = CardValue.Seven;
            expected[Suits.Clubs].LowValue = CardValue.Seven;
            expected[Suits.Clubs].IsOpen = true;

            object expectedValue = expected as object;
            _memCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue)).Returns(true);

            //Act
            var eligible = _board.IsEligible(cardToPlay);

            //Assert
            eligible.Should().BeFalse();
        }

        [Test]
        public void IsEligible_ThrowsArgumentNullException()
        {
            //Arrange
            object expectedValue = new Dictionary<Suits, InGameSuit>
            {
                {Suits.Diamonds, _fixture.Create<InGameSuit>() },
                {Suits.Spades, _fixture.Create<InGameSuit>() },
                {Suits.Hearts, _fixture.Create<InGameSuit>() },
                {Suits.Clubs, _fixture.Create<InGameSuit>() }
            };
            _memCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue)).Returns(true);

            Card card = null;

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => _board.IsEligible(card));

            //Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType(typeof(ArgumentNullException));
        }

        [Test]
        public void IsFirstTurn_returnsTrue()
        {
            //Arrange
            IDictionary<Suits, InGameSuit> expected = new Dictionary<Suits, InGameSuit>
            {
                {Suits.Diamonds, _fixture.Create<InGameSuit>() },
                {Suits.Spades, _fixture.Create<InGameSuit>() },
                {Suits.Hearts, _fixture.Create<InGameSuit>() },
                {Suits.Clubs, new InGameSuit(Suits.Clubs) }
            };

            expected[Suits.Diamonds].IsOpen = false;

            object expectedValue = expected as object;
            _memCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue)).Returns(true);

            //Act
            var firstTurn = _board.IsFirstTurn();

            //Assert
            firstTurn.Should().BeTrue();
        }

        [Test]
        public void IsFirstTurn_returnsFalse()
        {
            //Arrange
            IDictionary<Suits, InGameSuit> expected = new Dictionary<Suits, InGameSuit>
            {
                {Suits.Diamonds, _fixture.Create<InGameSuit>() },
                {Suits.Spades, _fixture.Create<InGameSuit>() },
                {Suits.Hearts, _fixture.Create<InGameSuit>() },
                {Suits.Clubs, new InGameSuit(Suits.Clubs) }
            };

            expected[Suits.Diamonds].IsOpen = true;

            object expectedValue = expected as object;
            _memCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue)).Returns(true);

            //Act
            var firstTurn = _board.IsFirstTurn();

            //Assert
            firstTurn.Should().BeFalse();
        }
    }
}
