using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sevens.Game.Engine.Concrete;
using Sevens.Game.Engine.Interfaces;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sevens.Game.Tests.Engine
{
    [TestFixture]
    public class GameStateTests
    {
        private MockRepository _mockRepository;
        private IFixture _fixture;

        private Mock<IPlayer> _human;
        private Mock<IPlayer> _skilled;
        private Mock<IPlayer> _unSkilled;

        private IList<IPlayer> _players;

        private IGameState _state;

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
            _state = new GameState(_players);
        }

        private void SetUpMockDefaults()
        {
            _players = new List<IPlayer>
            {
                _human.Object,
                _skilled.Object,
                _unSkilled.Object
            };

            _human.Setup(x => x.HasPlayerWon()).Returns(false);
            _skilled.Setup(x => x.HasPlayerWon()).Returns(false);
            _unSkilled.Setup(x => x.HasPlayerWon()).Returns(false);
        }

        [Test]
        public async Task IsGameOver_retrunsTrue()
        {
            //Arrange
            _skilled.Setup(x => x.HasPlayerWon()).Returns(true);

            var expected = new GameOverModel
            {
                IsGameOver = false,
                Player = _skilled.Object
            };

            //Act
            var actual = await _state.IsGameOver();

            //Assert
            actual.IsGameOver.Should().BeTrue();
            actual.Player.Should().Be(expected.Player);
        }

        [Test]
        public async Task IsGameOver_retrunsFalse()
        {
            //Arrange
            var expected = new GameOverModel
            {
                IsGameOver = false,
            };

            //Act
            var actual = await _state.IsGameOver();

            //Assert
            actual.IsGameOver.Should().BeFalse();
            actual.Player.Should().BeNull();
        }

        [Test]
        public void AddMove_success()
        {
            //Arrange
            var move = new MoveHistory
            {
                move = _fixture.Create<Card>(),
                Player = _human.Object
            };

            //Act

            //Assert
            Assert.DoesNotThrow(() => _state.AddMove(move));
        }

        [Test]
        public void AddMove_ThrowsArgumentNullException()
        {
            //Arrange

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => _state.AddMove(null));

            //Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType(typeof(ArgumentNullException));
            exception.ParamName.Should().Be("move");
        }
    }
}
