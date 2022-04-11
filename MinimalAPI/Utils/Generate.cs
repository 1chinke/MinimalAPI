using NUlid;

namespace MinimalAPI.Utils;

public static class Generate
{
    public static string Id()
    {
        return Ulid.NewUlid().ToString();
    }
}
