using System.ComponentModel.DataAnnotations;

public class PassedPoint
{
	[Key]
	public int Id { get; set; }
	public int Point { get; set; }
}