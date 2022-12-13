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
	}

	public TrackDistance GetPassedPoint()
	{
		int closestIndex = PassedPoints.Count();
		int closestLength = 10000;

		double x, y;

		if (closestIndex != mapData.Count())
		{
			x = mapData[closestIndex][0];
			y = mapData[closestIndex][1];
		}
		return new TrackDistance() { ClosestIndex = closestIndex, MetersToNextPoint = closestLength };
	}

	public bool pointPassed()
	{
		TrackDistance nearestPoint = GetPassedPoint(CenterlineLatitude, CenterlineLongitude, mapData);

		if (nearestPoint.MetersToNextPoint < 0.001)
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
			double x = mapData[index][0];
			double y = mapData[index][1];

			if (prevX != 0)
			{
				meters += Track.getDistanceFromLatLonInKm(y, x, prevY, prevX) * 1000;
			}

			prevX = x;
			prevY = y;
		}

		double[] pointTo = mapData[nearestPoint.ClosestIndex];
		double[] pointFrom = mapData[nearestPoint.ClosestIndex - 1];

		if (nearestPoint.ClosestIndex == 0)
		{
			pointFrom = mapData[mapData.Length - 1];
		}

		double distanceBetween = Track.getDistanceFromLatLonInKm(pointTo[1], pointTo[0], pointFrom[1], pointFrom[0]) * 1000;

		double percentage = distanceBetween / meters;

		if (!_configMap.kartPassedPoints.Contains(nearestPoint))
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
			pointTo = pointTo,
			pointFrom = pointFrom,
			percentageBetweenPoints = percentage
		};
	}
}