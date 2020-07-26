using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IntervalSnapshot
{
    public class SnapshotProvider : ISnapshotProvider
    {
        private readonly IOptions<Settings> settings;
        private readonly HttpClient httpClient;

        public SnapshotProvider(ILogger<SnapshotProvider> logger, IOptions<Settings> settings)
        {
            this.settings = settings;
            httpClient = new HttpClient();
        }

        public async Task<Stream> GetSnapshot()
        {
            var response = await httpClient.GetAsync(settings.Value.SnapshotUrl);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }
    }
}