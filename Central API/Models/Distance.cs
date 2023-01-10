namespace Central_API.Models;

public static class Distance
{
	public static double GetDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
	{
		// This is used to convert degrees to radians, which are needed for the trigonometric functions used in the calculation.
		var p = 0.017453292519943295; // Math.PI / 180

		// Used to calculate the cosine of an angle.
		var c = Math.Cos;

		// The diameter of the Earth in kilometers
		double diameterEarthKm = 12742;

		double a = 0.5 - c((lat2 - lat1) * p) / 2 +
		c(lat1 * p) * c(lat2 * p) *
		(1 - c((lon2 - lon1) * p)) / 2;

		return diameterEarthKm * Math.Asin(Math.Sqrt(a));
	}
}