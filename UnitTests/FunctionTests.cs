using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Robot.Common;
using R = Robot.Common.Robot;

namespace Babilia.Oleksandr.RobotChallange.Tests
{
    [TestClass]
    public class TestFunctions
    {
        [TestMethod]
        public void TestDistanceCost()
        {
            var p0 = new Position(1, 1);
            var p1 = new Position(2, 4);
            var p2 = new Position(0, 20);
            var p3 = new Position(10, 50);
            Assert.AreEqual(10, Functions.DistanceCost(p0, p1));
            Assert.AreEqual(362, Functions.DistanceCost(p0, p2));
            Assert.AreEqual(2482, Functions.DistanceCost(p0, p3));
        }
       
        [TestMethod]
        public void TestGetAuthorRobotCount()
        {
            var robots = new List<R>()
            {
                new R {Position = new Position(2, 1), Energy = 500, OwnerName = "Oleksandr"},
                new R {Position = new Position(3, 1), Energy = 300, OwnerName = "Pavlo"},
                new R {Position = new Position(4, 1), Energy = 500, OwnerName = "Taras"},
                new R {Position = new Position(5, 1), Energy = 400, OwnerName = "Oleksandr"},
                new R {Position = new Position(6, 1), Energy = 500, OwnerName = "Pavlo"},
                new R {Position = new Position(0, 1), Energy = 800, OwnerName = "Oleksandr"}
            };
            Assert.AreEqual(3, Functions.GetAuthorRobotCount(robots, "Oleksandr"));
            Assert.AreEqual(2, Functions.GetAuthorRobotCount(robots, "Pavlo"));
            Assert.AreEqual(1, Functions.GetAuthorRobotCount(robots, "Taras"));
            Assert.AreEqual(0, Functions.GetAuthorRobotCount(robots, "Ivan"));
        }
        [TestMethod]
        public void TestIsWithinRadius()
        {
            var p0 = new Position(0, 0);
            var p1 = new Position(2, 2);
            var p2 = new Position(3, 3);
            var p3 = new Position(1, 1);
            Assert.AreEqual(true, Functions.IsNearBy(p0, p1, 2));
            Assert.AreEqual(true, Functions.IsNearBy(p0, p1, 3));
            Assert.AreEqual(false, Functions.IsNearBy(p0, p2, 2));
            Assert.AreEqual(false, Functions.IsNearBy(p0, p2, 1));
            Assert.AreEqual(true, Functions.IsNearBy(p0, p3, 2));
            Assert.AreEqual(true, Functions.IsNearBy(p0, p3, 3));
        }
        [TestMethod]
        public void TestGetNearbyRobotCount()
        {
            var p0 = new Position(1, 1);
            var p1 = new Position(5, 5);
            var robots = new List<R>()
            {
                new R {Position = new Position(2, 1), Energy = 500, OwnerName = "Oleksandr"},
                new R {Position = new Position(3, 1), Energy = 300, OwnerName = "Pavlo"},
                new R {Position = new Position(4, 1), Energy = 500, OwnerName = "Taras"},
                new R {Position = new Position(5, 1), Energy = 400, OwnerName = "Oleksandr"},
                new R {Position = new Position(6, 1), Energy = 500, OwnerName = "Pavlo"},
                new R {Position = new Position(0, 1), Energy = 800, OwnerName = "Oleksandr"}
            };
            Assert.AreEqual(2, Functions.GetNearbyRobotCount(robots, p0));
            Assert.AreEqual(0, Functions.GetNearbyRobotCount(robots, p1));
        }
        [TestMethod]
        public void TestAvailablePosition()
        {
            var p0 = new Position(1, 1);
            var p1 = new Position(2, 4);
            var p2 = new Position(-10, 20);
            var p3 = new Position(10, 150);
            var map = new Map();
            map.Stations.Add(new EnergyStation()
            {
                Energy = 500,
                Position = new Position(2, 2),
                RecoveryRate = 2
            });

            var robots = new List<R>()
            {
                new R {Position = new Position(1, 1), Energy = 200, OwnerName = "Oleksandr"},
                new R {Position = new Position(5, 1), Energy = 500, OwnerName = "Oleksandr"}
            };
            Assert.AreEqual(false, Functions.IsAvailablePosition(map, robots, p0, "Oleksandr"));
            Assert.AreEqual(true, Functions.IsAvailablePosition(map, robots, p1, "Oleksandr"));
            Assert.AreEqual(true, Functions.IsAvailablePosition(map, robots, p1, "Andrii"));
            Assert.AreEqual(false, Functions.IsAvailablePosition(map, robots, p2, "Andrii"));
            Assert.AreEqual(false, Functions.IsAvailablePosition(map, robots, p3, "Oleksandr"));
        }
        [TestMethod]
        public void TestGetBestPositions()
        {
            var map = new Map();
            map.Stations.Add(new EnergyStation()
            {
                Energy = 500,
                Position = new Position(2, 2),
                RecoveryRate = 2
            });

            var robotPosition = new Position(1, 1);
            int radius = 2; 
            var bestPositions = Functions.GetBestPositions(map, robotPosition, radius);

            Assert.AreEqual(new Position(1, 1), bestPositions[0].Value);
        }
        [TestMethod]
        public void TestIsAvailablePositionWithoutEnergyStation()
        {
            var map = new Map();
            map.Stations.Add(new EnergyStation()
            {
                Energy = 500,
                Position = new Position(2, 2),
                RecoveryRate = 2
            });

            var robots = new List<R>()
            {
                new R { Position = new Position(1, 1), Energy = 200, OwnerName = "Oleksandr" },
            };

            var positionWithoutStation = new Position(3, 3);

            Assert.IsTrue(Functions.IsAvailablePosition(map, robots, positionWithoutStation, "Oleksandr"));
        }


    }
}
