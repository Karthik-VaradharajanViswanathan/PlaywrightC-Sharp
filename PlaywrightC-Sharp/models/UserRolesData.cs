using Newtonsoft.Json;

namespace playwrightcs.models;

public class UserRolesItem
{
    public class UserRole
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    [JsonProperty("standard")]
    public UserRole Standard { get; set; }

    [JsonProperty("lockedOut")]
    public UserRole LockedOut { get; set; }

    [JsonProperty("problemUser")]
    public UserRole ProblemUser { get; set; }

    [JsonProperty("performanceGlitchUser")]
    public UserRole PerformanceGlitchUser { get; set; }
}

public partial class UserRolesData
{
    private readonly string fileName = "UserRoles.json";

    [JsonProperty("UserRolesData")]
    public IEnumerable<UserRolesItem> Result { get; set; }

    public string FileName => fileName;
}