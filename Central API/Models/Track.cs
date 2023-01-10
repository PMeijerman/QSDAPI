namespace Central_API.Models;

public static class Track
{
	public static double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
	{
		// Optimized version
		const double p = 0.017453292519943295;    // Math.PI / 180
		var c = Math.Cos;
		var diameterEarthKm = 12742;
		var a = 0.5 - c((lat2 - lat1) * p) / 2 +
						c(lat1 * p) * c(lat2 * p) *
						(1 - c((lon2 - lon1) * p)) / 2;

		return diameterEarthKm * Math.Asin(Math.Sqrt(a));

		// Not optimized
		// var diameterEarthKm = 12742;
		// var a = lat2 - lat1;
		// var b = lon2 - lon1;
		// var l = (a * a) + (b * b);

		// return diameterEarthKm * Math.Sqrt(l);
	}

	public static double deg2rad(double deg)
	{
		return deg * (Math.PI / 180);
	}
}