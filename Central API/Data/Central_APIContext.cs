using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Central_API.Models;

namespace Central_API.Data
{
	public class Central_APIContext : DbContext
	{
		public Central_APIContext(DbContextOptions<Central_APIContext> options)
				: base(options)
		{
		}

		public DbSet<Central_API.Models.Team> Team { get; set; } = default!;

		public DbSet<Central_API.Models.KartLocationData> Kart { get; set; } = default!;

		public DbSet<Central_API.Models.LocationData> LocationData { get; set; }

		public DbSet<Central_API.Models.KartLocationData> KartLocationData { get; set; }
	}
}
