using System.Text;

namespace APICallSystem.Outdated.API
{
    internal static class UrlFactory
    {
        public static string? Last { get; set; }
        private static readonly StringBuilder current = new();

        public static string Create(bool isHttps, string mainUrl, ushort port = 0, string version = "")
        {
            current.Clear();
            Http(isHttps);
            MainUrl(mainUrl);
            Port(port);
            Path(version);
            current.Append('/');
            Last = current.ToString();
            return Last;
        }

        private static void Http(bool isHttps)
        {
            if (isHttps)
            {
                current.Append("https");
            }
            else
            {
                current.Append("http");
            }

            current.Append("://");
        }

        private static void MainUrl(string mainUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(mainUrl);
            current.Append(mainUrl);
        }

        private static void Port(ushort port)
        {
            if (port == 0) return;
            current.Append(':');
            current.Append(port);
        }

        private static void Path(string version)
        {
            current.Append('/');
            current.Append(version);
        }

        public static bool Validate()
        {
            // Validation
            return true;
        }
    }
}
