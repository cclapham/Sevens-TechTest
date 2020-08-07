using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sevens.Game.Engine.Concrete;
using Sevens.Game.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sevens.Game.Tests.Engine
{
    [TestFixture]
    public class DeckTests
    {
        private MockRepository _mockRepository;
        private IFixture _fixture;

        private Mock<IPlayer> _human;
        private Mock<IPlayer> _skilled;
        private Mock<IPlayer> _unSkilled;

        private IList<IPlayer> _players;

        private IDeck _deck;

        [SetUp]
        public void SetUp()
        {
            // Boilerplate
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _fixture = new Fixture();

            //Prevent fixture from generating from entity circular references
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            // Mock setup
            _human = _mockRepository.Create<IPlayer>();
            _skilled = _mockRepository.Create<IPlayer>();
            _unSkilled = _mockRepository.Create<IPlayer>();

            SetUpMockDefaults();

            //Sut Instantiation
            _deck = new Deck();
        }

        private void SetUpMockDefaults()
        {
            _players = new List<IPlayer>
            {
                _human.Object,
                _skilled.Object,
                _unSkilled.Object
            };
        }

        [Test]
        public void Deal_Success()
        {
            //Arrange

            //Act

            //Assert
            Assert.DoesNotThrow(() => _deck.Deal(ref _players));
        }

        [Test]
        public void Deal_ThrowsArgumentNullException()
        {
            //Arrange
            IList<IPlayer> players = null;

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => _deck.Deal(ref players));

            //Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType(typeof(ArgumentNullException));
            exception.ParamName.Should().Be("players");
        }
    }
}
