using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Robot.Common;
using R = Robot.Common.Robot;

namespace Babilia.Oleksandr.RobotChallange.Tests
{
    [TestClass]
    public class TestAlgorithm
    {
        [TestMethod]
        public void TestCollectCommand()
        {
            var algo = new BabiliaAlgorithm();
            var map = new Map();
            map.Stations.Add(new EnergyStation()
            {
                Energy = 500,
                Position = new Position(2, 2),
                RecoveryRate = 2
            });
            var robots = new List<R>()
            {
                new R {Position = new Position(1, 1), Energy = 200, OwnerName = "Babilia Oleksandr"},
                new R {Position = new Position(5, 1), Energy = 500, OwnerName = "Babilia Oleksandr"},
                new R {Position = new Position(6, 1), Energy = 200, OwnerName = "Babilia Oleksandr"},
            };
            var command1 = algo.DoStep(robots, 0, map);
            Assert.IsTrue(command1 is CollectEnergyCommand);
        }

        [TestMethod]
        public void TestMoveCommand()
        {
            var algo = new BabiliaAlgorithm();
            var map = new Map();
            map.Stations.Add(new EnergyStation()
            {

                Energy = 500,
                Position = new Position(2, 2),
                RecoveryRate = 2
            });
            var robots = new List<R>()
            {
                new R {Position = new Position(1, 1), Energy = 200, OwnerName = "Babilia Oleksandr"},
                new R {Position = new Position(5, 1), Energy = 500, OwnerName = "Babilia Oleksandr"},
                new R {Position = new Position(6, 1), Energy = 200, OwnerName = "Babilia Oleksandr"},
            };
            var command3 = algo.DoStep(robots, 2, map);
            Assert.IsTrue(command3 is MoveCommand);
            Assert.AreEqual(((MoveCommand)command3).NewPosition, new Position(4, 1));
        }

        [TestMethod]
        public void TestCreateCommand()
        {
            var algo = new BabiliaAlgorithm();
            var map = new Map();
            map.Stations.Add(new EnergyStation()
            {
                Energy = 500,
                Position = new Position(2, 2),
                RecoveryRate = 2
            });
            var robots = new List<R>()
            {
                new R {Position = new Position(1, 1), Energy = 200, OwnerName = "Babilia Oleksandr"},
                new R {Position = new Position(5, 1), Energy = 500, OwnerName = "Babilia Oleksandr"},
                new R {Position = new Position(6, 1), Energy = 200, OwnerName = "Babilia Oleksandr"},
            };
            var command2 = algo.DoStep(robots, 1, map);
            Assert.IsTrue(command2 is CreateNewRobotCommand);
        }
    }
}