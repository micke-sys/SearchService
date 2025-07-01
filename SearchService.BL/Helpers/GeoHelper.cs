namespace SearchService.BL.Helpers;

public static class GeoHelper
{
    /// <summary>
    /// Calculates the Haversine distance between two geographic coordinates.
    /// </summary>
    /// <param name="lat1">Latitude of point 1 in decimal degrees</param>
    /// <param name="lon1">Longitude of point 1 in decimal degrees</param>
    /// <param name="lat2">Latitude of point 2 in decimal degrees</param>
    /// <param name="lon2">Longitude of point 2 in decimal degrees</param>
    /// <returns>Distance in kilometers as a double</returns>
    public static double CalculateDistanceKm(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371.0; // Radius of the Earth in km
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }
}