namespace Central_API.Models;

public class Kart
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    

    void Init()
    {
        // Initialization code goes here
    }

    Dictionary<string, double> GetKartCoordinates()
    {
        // Code for making an AJAX call goes here
        // For example:
        /*
        var client = new HttpClient();
        var response = await client.GetAsync("https://selecting-thought-rim-counter.trycloudflare.com/mostRecent%22);
        var jsonString = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<Dictionary<string, double>>(jsonString);
        return data;
        */
    }

    public Dictionary<string, object> GetPassedPoint(double kartX, double kartY, List<List<double>> mapData)
    {
        int closestIndex = (int)_configMap["kartPassedPoints"].Count();
        double closestLength = 10000;

        double x, y;

        if (closestIndex != mapData.Count())
        {
            x = mapData[closestIndex][0];
            y = mapData[closestIndex][];
        }
    }
    
    public bool pointPassed(HartlineCoordinates hartlineCoordinates, MapData mapData)
    {
        var nearestPoint = getPassedPoint(hartlineCoordinates.x, hartlineCoordinates.y, mapData);

        if (nearestPoint.length < 0.001)
        {
            if (!_configMap.kartPassedPoints.Contains(nearestPoint.index))
            {
                _configMap.kartPassedPoints.Add(nearestPoint.index);
                return true;
            }
        }

        return false;
    }
    
    public TrackDistance calculateTrackDistance(NearestPoint nearestPoint, double[][] mapData)
    {
        double prevX = 0, prevY = 0, meters = 0;

        for (int index = 0; index <= nearestPoint.index; index++)
        {
            double x = mapData[index][0];
            double y = mapData[index][1];

            if (prevX != 0)
            {
                meters += Distance.getDistanceFromLatLonInKm(y, x, prevY, prevX) * 1000;
            }

            prevX = x;
            prevY = y;
        }

        double[] pointTo = mapData[nearestPoint.index];
        double[] pointFrom = mapData[nearestPoint.index - 1];

        if (nearestPoint.index == 0)
        {
            pointFrom = mapData[mapData.Length - 1];
        }

        double distanceBetween = Distance.getDistanceFromLatLonInKm(pointTo[1], pointTo[0], pointFrom[1], pointFrom[0]) * 1000;

        double percentage = distanceBetween / meters;

        if (!_configMap.kartPassedPoints.Contains(nearestPoint.index))
        {
            meters -= nearestPoint.length * 1000;
        }
        else
        {
            meters += nearestPoint.length * 1000;
        }

        return new TrackDistance
        {
            metersTraveled = meters,
            pointTo = pointTo,
            pointFrom = pointFrom,
            percentageBetweenPoints = percentage
        };
    }
}