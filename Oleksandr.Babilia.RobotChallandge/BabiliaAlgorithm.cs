using System.Collections.Generic;
using Robot.Common;

namespace Babilia.Oleksandr.RobotChallange
{
    public class BabiliaAlgorithm : IRobotAlgorithm
    {
        public string Author => "Oleksandr Babilia";

        public int Round { get; set; }

        public BabiliaAlgorithm()
        {
            Logger.OnLogRound += LogRound;
        }

        private void LogRound(object sender, LogRoundEventArgs e)
        {
            Round++;
        }

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            if (Round == 51)
                return new CollectEnergyCommand();

            var robot = robots[robotToMoveIndex];

            if (Functions.GetAuthorRobotCount(robots, Author) < 100 && robot.Energy > 200)
                return new CreateNewRobotCommand();

            IList<EnergyStation> nearbyResources = map.GetNearbyResources(robot.Position, 2);

            if (nearbyResources.Count > 0)
                foreach (EnergyStation energyStation in nearbyResources)
                    if (energyStation.Energy > 40)
                        return new CollectEnergyCommand();

            foreach (KeyValuePair<int, Position> bestPosition in Functions.GetBestPositions(map, robot.Position, 25))
                if (Functions.IsAvailablePosition(map, robots, bestPosition.Value, Author)
                && robot.Energy > Functions.DistanceCost(robot.Position, bestPosition.Value))
                    return new MoveCommand{ NewPosition = bestPosition.Value };

            return new CollectEnergyCommand();
        }
    }
}