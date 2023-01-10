using System.IO;
using Newtonsoft.Json;

public static class StaticMap
{
    public static double[][] MapPoints { get; set; }

    public static double[][] ReadFromJsonFile()
    {
        double[][] result;
        using (StreamReader reader = new StreamReader("./Content/trackpoints.json"))
        {
            string json = reader.ReadToEnd();
            result = JsonConvert.DeserializeObject<double[][]>(json);
        }

        return result;
    }
}