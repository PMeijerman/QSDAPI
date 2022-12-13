using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Central_API.Models
{
    public class Lap
    {
        [Key]
        public int Id { get; set; }
        public long Time { get; set; }

        [ForeignKey("TeamId")]
        public int TeamId { get; set; }
        public Team? Team { get; set; }

        public Lap()
        {

        }

        public Lap(int id, long time, int teamId, Team team)
        {
            Id = id;
            Time = time;
            TeamId = teamId;
            Team = team;
        }
    }
}