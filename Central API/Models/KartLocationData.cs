using System.Linq;

namespace Central_API.Models;

public class KartLocationData
{
	public int Id { get; set; }
	public double KartLongitude { get; set; }
	public double KartLatitude { get; set; }
	public double CenterlineLongitude { get; set; }
	public double CenterlineLatitude { get; set; }
	public double DistanceTraveled { get; set; }
	public int TeamId { get; set; }
	public Team Team { get; set; }

	public KartLocationData()
	{
		if (StaticMap.MapPoints == null)
		{
			StaticMap.MapPoints = StaticMap.ReadFromJsonFile();
		}
	}

	public KartDistanceTrack GetNearestPoint(double referenceY, double referenceX)
	{
		int closestIndex = Team.PassedPoints.Count;
		double closestLength = 10000;

		for (int index = 0; index < StaticMap.MapPoints.Length; index++)
		{
			double longitude = StaticMap.MapPoints[index][0];
			double latitude = StaticMap.MapPoints[index][1];
			double length = Distance.GetDistanceFromLatLonInKm(latitude, longitude, referenceY, referenceX);

			if (length < closestLength)
			{
				closestLength = length;
				closestIndex = index;
			}
		}

		return new KartDistanceTrack { ClosestIndex = closestIndex, MetersToNextPoint = closestLength };
	}
	public KartDistanceTrack GetPassedPoint()
	{
		int closestIndex = Team.PassedPoints.Count();
		double closestLength = 10000;

		double longitude = 0, latitude = 0;

		if (closestIndex != StaticMap.MapPoints.Count())
		{
			longitude = StaticMap.MapPoints[closestIndex][0];
			latitude = StaticMap.MapPoints[closestIndex][1];
		}
		else
		{
			Team.PassedPoints = new List<PassedPoint>();
			closestIndex = 0;

			longitude = StaticMap.MapPoints[0][0];
			latitude = StaticMap.MapPoints[0][1];
		}

		closestLength = Track.getDistanceFromLatLonInKm(latitude, longitude, KartLatitude, KartLongitude);

		return new KartDistanceTrack() { ClosestIndex = closestIndex, MetersToNextPoint = closestLength };
	}

	public bool PointPassed()
	{
		KartDistanceTrack nearestPoint = GetPassedPoint();

		if (nearestPoint.MetersToNextPoint < 0.003)
		{
			if (!Team.PassedPoints.Contains(new PassedPoint() { Point = nearestPoint.ClosestIndex }))
			{
				Team.PassedPoints.Add(new PassedPoint() { Point = nearestPoint.ClosestIndex });
				return true;
			}
		}

		return false;
	}

	public KartDistanceTrack CalculateTrackDistance(KartDistanceTrack nearestPoint, MapPoint centerlineFakePoint)
	{
		double prevX = 0, prevY = 0, meters = 0;

		for (int index = 0; index <= nearestPoint.ClosestIndex; index++)
		{
			double longitude = StaticMap.MapPoints[index][0];
			double latitude = StaticMap.MapPoints[index][1];

			if (prevX != 0)
			{
				meters += Track.getDistanceFromLatLonInKm(latitude, longitude, prevY, prevX) * 1000;
			}

			prevX = longitude;
			prevY = latitude;
		}

		double[] pointTo = StaticMap.MapPoints[nearestPoint.ClosestIndex];
		double[] pointFrom = StaticMap.MapPoints[nearestPoint.ClosestIndex - 1];

		if (nearestPoint.ClosestIndex == 0)
		{
			pointFrom = StaticMap.MapPoints[StaticMap.MapPoints.Length - 1];
		}

		double metersTraveledBetweenFakePoints = Distance.GetDistanceFromLatLonInKm(centerlineFakePoint.Latitude, centerlineFakePoint.Longitude, KartLatitude, KartLongitude) * 1000;

		double distanceBetween = Distance.GetDistanceFromLatLonInKm(pointTo[1], pointTo[0], pointFrom[1], pointFrom[0]) * 1000;
		double distanceBetweenToFake = Distance.GetDistanceFromLatLonInKm(centerlineFakePoint.Latitude, centerlineFakePoint.Longitude, pointFrom[1], pointFrom[0]) * 1000;

		double metersTraveledBetweenPoints = distanceBetweenToFake - metersTraveledBetweenFakePoints;

		double percentage = metersTraveledBetweenPoints / distanceBetween;

		if (percentage > 1)
		{
			percentage = 1;
			DistanceTraveled = distanceBetween;
		}

		if (percentage < 0)
		{
			percentage = 0;
		}

		if (!Team.PassedPoints.Contains(new PassedPoint() { Point = nearestPoint.ClosestIndex }))
		{
			meters -= nearestPoint.MetersToNextPoint * 1000;
		}
		else
		{
			meters += nearestPoint.MetersToNextPoint * 1000;
		}

		return new KartDistanceTrack
		{
			MetersToNextPoint = meters,
			PointTo = new MapPoint() { Longitude = pointTo[0], Latitude = pointTo[1] },
			PointFrom = new MapPoint() { Longitude = pointFrom[0], Latitude = pointFrom[1] },
			PercentageBetweenPoints = percentage
		};
	}
}