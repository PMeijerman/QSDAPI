namespace Central_API.Models
{
	public class TrackDistance
	{
		public int ClosestIndex { get; set; }

		public double MetersToNextPoint { get; set; }

		public double[] pointTo { get; set; }

		public double[] pointFrom { get; set; }

		public double percentageBetweenPoints { get; set; }
	}
}