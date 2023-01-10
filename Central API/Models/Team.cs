using System.ComponentModel.DataAnnotations;

namespace Central_API.Models
{
	public class Team
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public Color Color { get; set; }
		public List<KartLocationData>? KartLocationDatas { get; set; }
		public long TotalTime { get; set; }
		public float TotalDistance { get; set; }
		public float Score { get; set; }
		public List<PassedPoint> PassedPoints { get; set; }
        public ICollection<Lap>? Laps { get; set; }

		public Team()
		{
			PassedPoints = new List<PassedPoint>();
			PassedPoints.Add(new PassedPoint() { Point = 1 });
        }
	}

	public enum Color
	{
		RED,
		BLUE,
		GREEN,
		YELLOW,
		PURPLE,
		ORANGE
	}
}
