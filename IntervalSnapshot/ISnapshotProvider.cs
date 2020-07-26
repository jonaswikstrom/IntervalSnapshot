using System.IO;
using System.Threading.Tasks;

namespace IntervalSnapshot
{
    public interface ISnapshotProvider
    {
        Task<Stream> GetSnapshot();
    }
}