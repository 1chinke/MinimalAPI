using NUlid;

namespace MinimalAPI.Utils;

public class Generate 
{    
    public static string Id()
    {
        Ulid.NewUlid();
        return Ulid.NewUlid().ToString();
    }
    
}
