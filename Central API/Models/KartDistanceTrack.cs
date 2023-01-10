namespace Central_API.Models
{
	public class KartDistanceTrack
	{
		public int ClosestIndex { get; set; }

		public double MetersToNextPoint { get; set; }

		public MapPoint PointTo { get; set; }

		public MapPoint PointFrom { get; set; }

		public double PercentageBetweenPoints { get; set; }

	}
}