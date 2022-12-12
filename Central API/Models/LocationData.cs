using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Central_API.Models
{
    public class LocationData
    {
        [Key]
        public int Id { get; set; }
        public float? Distance { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("TeamId")]
        public int TeamId { get; set; }
        public Team? Team { get; set; }

        public LocationData(int teamId, float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            TeamId = teamId;
        }

        public LocationData()
        {

        }
    }
}
