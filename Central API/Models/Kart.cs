using System.Linq;

namespace Central_API.Models;

public class Kart
{
	public double Longitude { get; set; }
	public double Latitude { get; set; }
	public double CenterlineLongitude { get; set; }
	public double CenterlineLatitude { get; set; }
	public List<int> PassedPoints { get; set; }
	public Track Track { get; set; }

	void Init()
	{
		// Initialization code goes here
		if(StaticMap.MapPoints == null) {
			StaticMap.MapPoints = StaticMap.ReadFromJsonFile();
		}
	}

	public TrackDistance GetPassedPoint()
	{
		int closestIndex = PassedPoints.Count();
		double closestLength = 10000;

		double x = 0, y = 0;

		if (closestIndex != StaticMap.MapPoints.Count())
		{
			x = StaticMap.MapPoints[closestIndex][0];
			y = StaticMap.MapPoints[closestIndex][1];
		}

        closestLength = Track.getDistanceFromLatLonInKm(y, x, Latitude, Longitude);

        return new TrackDistance() { ClosestIndex = closestIndex, MetersToNextPoint = closestLength };
	}

	public bool pointPassed()
	{
		TrackDistance nearestPoint = GetPassedPoint();

		if (nearestPoint.MetersToNextPoint < 0.003)
		{
			if (!PassedPoints.Contains(nearestPoint.ClosestIndex))
			{
				PassedPoints.Add(nearestPoint.ClosestIndex);
				return true;
			}
		}

		return false;
	}

	public TrackDistance calculateTrackDistance(TrackDistance nearestPoint)
	{
		double prevX = 0, prevY = 0, meters = 0;

		for (int index = 0; index <= nearestPoint.ClosestIndex; index++)
		{
			double x = StaticMap.MapPoints[index][0];
			double y = StaticMap.MapPoints[index][1];

			if (prevX != 0)
			{
				meters += Track.getDistanceFromLatLonInKm(y, x, prevY, prevX) * 1000;
			}

			prevX = x;
			prevY = y;
		}

		double[] pointTo = StaticMap.MapPoints[nearestPoint.ClosestIndex];
		double[] pointFrom = StaticMap.MapPoints[nearestPoint.ClosestIndex - 1];

		if (nearestPoint.ClosestIndex == 0)
		{
			pointFrom = StaticMap.MapPoints[StaticMap.MapPoints.Length - 1];
		}

		double distanceBetween = Track.getDistanceFromLatLonInKm(pointTo[1], pointTo[0], pointFrom[1], pointFrom[0]) * 1000;

		double percentage = distanceBetween / meters;

		if (!PassedPoints.Contains(nearestPoint.ClosestIndex))
		{
			meters -= nearestPoint.MetersToNextPoint * 1000;
		}
		else
		{
			meters += nearestPoint.MetersToNextPoint * 1000;
		}

		return new TrackDistance
		{
			MetersToNextPoint = meters,
			PointTo = pointTo,
			PointFrom = pointFrom,
			PercentageBetweenPoints = percentage
		};
	}
}