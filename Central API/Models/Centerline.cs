public class Centerline
{
	public static MapPoint CalculateCoordinates(MapPoint pointTo, MapPoint pointFrom, double percentageTraveled)
	{
		double directionX = pointTo.Longitude - pointFrom.Longitude;
		double directionY = pointTo.Latitude - pointFrom.Latitude;

		double newPositionLongitude = pointFrom.Longitude + percentageTraveled * directionX;
		double newPositionLatitude = pointFrom.Latitude + percentageTraveled * directionY;

		return new MapPoint() { Longitude = newPositionLongitude, Latitude = newPositionLatitude };
	}
}