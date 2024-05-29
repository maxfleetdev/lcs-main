public static class SaveCache
{
    private static string saveLocation;

    public static void CacheSaveLocation(string location)
    {
        saveLocation = location;
    }

    public static string GetSaveLocation()
    {
        return saveLocation;
    }
}