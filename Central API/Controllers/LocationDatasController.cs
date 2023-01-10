using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Central_API.Data;
using Central_API.Models;

namespace Central_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class LocationDatasController : ControllerBase
	{
		private readonly Central_APIContext _context;

		public LocationDatasController(Central_APIContext context)
		{
			_context = context;
		}

		// GET: api/LocationDatas
		[HttpGet]
		public async Task<ActionResult<IEnumerable<LocationData>>> GetLocationData()
		{
			return await _context.LocationData/*.Include(l => l.Team)*/.ToListAsync();
		}

		// GET: /MostRecent
		[HttpGet("/mostRecent")]
		public async Task<ActionResult<LocationData>> GetMostRecent()
		{
			var list = await _context.LocationData.ToListAsync();
			return list.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
		}


		// GET: api/LocationDatas/5
		[HttpGet("{id}")]
		public async Task<ActionResult<LocationData>> GetLocationData(int id)
		{
			var locationData = await _context.LocationData.FindAsync(id);

			if (locationData == null)
			{
				return NotFound();
			}

			return locationData;
		}

		// PUT: api/LocationDatas/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutLocationData(int id, LocationData locationData)
		{
			if (id != locationData.Id)
			{
				return BadRequest();
			}

			_context.Entry(locationData).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!LocationDataExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/LocationDatas
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost()]
		public async Task<ActionResult<LocationData>> PostLocationData([FromBody] LocationData locationData)
		{
			//TODO calculate distance
			locationData.CreatedAt = DateTime.Now;
			_context.Add(locationData);

			KartLocationData kartLocationData = new KartLocationData()
			{
				KartLatitude = locationData.Latitude,
				KartLongitude = locationData.Longitude,
				Team = locationData.Team,
			};

			while (true)
			{
				KartDistanceTrack updatedHartlinePosition = UpdateKartHartlinePosition(kartLocationData);

				if (updatedHartlinePosition.PercentageBetweenPoints != 1) break;
			}

			await _context.KartLocationData.AddAsync(kartLocationData);
			await _context.SaveChangesAsync();

			return locationData;
		}

		public KartDistanceTrack UpdateKartHartlinePosition(KartLocationData kartLocationData)
		{
			KartDistanceTrack nearestPoint = kartLocationData.GetPassedPoint();

			MapPoint nextPoint = (nearestPoint.ClosestIndex <= 0) ? nextPoint = new MapPoint() { Longitude = StaticMap.MapPoints[StaticMap.MapPoints.Length - 1][0], Latitude = StaticMap.MapPoints[StaticMap.MapPoints.Length - 1][1] } :
						 nextPoint = new MapPoint() { Longitude = StaticMap.MapPoints[nearestPoint.ClosestIndex - 1][0], Latitude = StaticMap.MapPoints[nearestPoint.ClosestIndex - 1][1] };

			//calculates distance from target point to 'extension point', subtract that from distance between gps point to extension point
			//that is tha distance from gps point to traget point, which is used to snap it to the hartline.
			MapPoint centerlineExtensionPoint = Centerline.CalculateCoordinates(new MapPoint() { Longitude = StaticMap.MapPoints[nearestPoint.ClosestIndex][0], Latitude = StaticMap.MapPoints[nearestPoint.ClosestIndex][1] }, nextPoint, 10);

			KartDistanceTrack trackDistance = kartLocationData.CalculateTrackDistance(nearestPoint, centerlineExtensionPoint);

			MapPoint centerlineCoordinates = Centerline.CalculateCoordinates(trackDistance.PointTo, trackDistance.PointFrom, trackDistance.PercentageBetweenPoints);

			kartLocationData.PointPassed();

			kartLocationData.CenterlineLongitude = centerlineCoordinates.Longitude;
			kartLocationData.CenterlineLatitude = centerlineCoordinates.Latitude;

			return trackDistance;
		}


		// DELETE: api/LocationDatas/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteLocationData(int id)
		{
			var locationData = await _context.LocationData.FindAsync(id);
			if (locationData == null)
			{
				return NotFound();
			}

			_context.LocationData.Remove(locationData);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool LocationDataExists(int id)
		{
			return _context.LocationData.Any(e => e.Id == id);
		}
	}
}
