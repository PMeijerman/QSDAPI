namespace Central_API.Models
{
	public class TrackDistance
	{
		public int ClosestIndex { get; set; }

		public double MetersToNextPoint { get; set; }

		public double[] PointTo { get; set; }

		public double[] PointFrom { get; set; }

		public double PercentageBetweenPoints { get; set; }
	}
}