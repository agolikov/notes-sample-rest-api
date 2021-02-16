using notes.data.Settings;

namespace notes.api.Models.Settings
{
    public class AppSettings
    {
        public string SecretKey { get; set; }
        public DbConnectionOptions DbConnectionOptions { get; set; }
    }
}
