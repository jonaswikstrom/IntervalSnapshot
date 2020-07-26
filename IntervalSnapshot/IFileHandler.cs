using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IntervalSnapshot
{
    public interface IFileHandler
    {
        void SaveStream(Stream stream);
    }

    public class FileHandler : IFileHandler
    {
        private readonly ILogger<FileHandler> logger;
        private readonly IOptions<Settings> settings;

        public FileHandler(ILogger<FileHandler> logger, IOptions<Settings> settings)
        {
            this.logger = logger;
            this.settings = settings;
        }

        public void SaveStream(Stream stream)
        {
            var directory = DateTime.Now.ToString(Path.GetDirectoryName(settings.Value.ImageFileName));
            var fileNameWithoutExtension = DateTime.Now.ToString(Path.GetFileNameWithoutExtension(settings.Value.ImageFileName));
            var extension = Path.GetExtension(settings.Value.ImageFileName);
            var fileName = string.Concat(fileNameWithoutExtension, extension);

            Directory.CreateDirectory(Path.Combine(settings.Value.RootDirectory, directory));

            var fileWithPath = Path.Combine(settings.Value.RootDirectory, directory, fileName);
            var fileStream = new FileStream(fileWithPath, FileMode.Create, FileAccess.Write);

            stream.CopyTo(fileStream);
            fileStream.Dispose();

            logger.LogInformation($"File {fileWithPath} created");
        }
    }
}