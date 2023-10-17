using System;
using System.Collections.Generic;
using System.Linq;
using Robot.Common;

namespace Babilia.Oleksandr.RobotChallange
{
    public class Functions
    {
        private const int NearbyRadius = 1;
        private const int EnergyRadius = 2;

        private static bool _isStationWithinRadius(EnergyStation station, int length, int robotsLowestY, int robotsLowestX)
        {
            return station.Position.X >= robotsLowestX - EnergyRadius &&
                   station.Position.X < robotsLowestX + length + EnergyRadius &&
                   station.Position.Y >= robotsLowestY - EnergyRadius &&
                   station.Position.Y < robotsLowestY + length + EnergyRadius;
        }
        private static bool _isCollectRnageWithinRadius(int length, int stationsLowestX, int stationsLowestY, int i, int j)
        {
            return stationsLowestX + j >= 0 &&
                   stationsLowestX + j < length &&
                   stationsLowestY + i >= 0 &&
                   stationsLowestY + i < length;
        }

        public static int DistanceCost(Position center, Position distant) =>
            (center.X - distant.X) * (center.X - distant.X) + (center.Y - distant.Y) * (center.Y - distant.Y);

        public static int GetAuthorRobotCount(IList<Robot.Common.Robot> robots, string author) =>
            ((IEnumerable<Robot.Common.Robot>)robots)
                .Where<Robot.Common.Robot>((Func<Robot.Common.Robot, bool>)(robot => robot.OwnerName == author))
                .Count<Robot.Common.Robot>();

        public static int GetNearbyRobotCount(IList<Robot.Common.Robot> robots, Position position) =>
            ((IEnumerable<Robot.Common.Robot>)robots)
                .Where<Robot.Common.Robot>((Func<Robot.Common.Robot, bool>)(robot => Functions.IsNearBy(position, robot.Position, NearbyRadius)))
                .Count<Robot.Common.Robot>();

        public static bool IsNearBy(Position center, Position point, int radius) =>
            Math.Abs(center.X - point.X) <= radius && Math.Abs(center.Y - point.Y) <= radius;
       
        public static bool IsAvailablePosition(Map map, IList<Robot.Common.Robot> robots, Position position, string author)
        {
            if (position.X > 100 || position.X < 0 || position.Y > 100 || position.Y < 0)
                return false;

            foreach (Robot.Common.Robot robot in (IEnumerable<Robot.Common.Robot>)robots)
                if (Position.Equals(robot.Position, position) && robot.OwnerName == author)
                    return false;

            return true;
        }

        public static List<KeyValuePair<int, Position>> GetBestPositions(Map map, Position robotsPosition, int radius)
        {
            int length = 2 * radius + 1;
            int[,] moveRangeMap = new int[length, length];
            int robotsLowestY = robotsPosition.Y - radius;
            int robotsLowestX = robotsPosition.X - radius;

            foreach (EnergyStation station in (IEnumerable<EnergyStation>)map.Stations)
            {
                if (_isStationWithinRadius(station, length, robotsLowestY, robotsLowestX))
                {
                    int stationsLowestX = station.Position.X - robotsLowestX;
                    int stationsLowestY = station.Position.Y - robotsLowestY;

                    for (int i = -EnergyRadius; i <= EnergyRadius; ++i)
                        for (int j = -EnergyRadius; j <= EnergyRadius; ++j)
                            if (_isCollectRnageWithinRadius(length, stationsLowestX, stationsLowestY, i, j))
                                moveRangeMap[stationsLowestY + i, stationsLowestX + j] += station.Energy;
                }
            }

            List<KeyValuePair<int, Position>> positionToStationMap = new List<KeyValuePair<int, Position>>();

            for (int i = 0; i < length; ++i)
            {
                for (int j = 0; j < length; ++j)
                {
                    moveRangeMap[i, j] -= Functions.DistanceCost(robotsPosition, new Position(robotsLowestX + j, robotsLowestY + i));
                    positionToStationMap.Add(new KeyValuePair<int, Position>(moveRangeMap[i, j], new Position(robotsLowestX + j, robotsLowestY + i)));
                }
            }

            positionToStationMap.Sort((Comparison<KeyValuePair<int, Position>>)((pair1, pair2) => -pair1.Key.CompareTo(pair2.Key)));
            
            return positionToStationMap;
        }
    }
}