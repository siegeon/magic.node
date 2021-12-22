﻿/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.IO;
using System.Threading.Tasks;
using magic.node.contracts;
using magic.node.extensions;

namespace magic.node.services
{
    /// <inheritdoc/>
    public class StreamService : IStreamService
    {
        readonly IFileService _fileService;

        public StreamService(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <inheritdoc/>
        public Stream OpenFile(string path)
        {
            return File.OpenRead(path);
        }

        /// <inheritdoc/>
        public Task<Stream> OpenFileAsync(string path)
        {
            return Task.FromResult<Stream>(File.OpenRead(path));
        }

        /// <inheritdoc/>
        public void SaveFile(Stream stream, string path, bool overwrite)
        {
            if (_fileService.Exists(path))
            {
                if (overwrite)
                    _fileService.Delete(path);
                else
                    throw new HyperlambdaException("File already exists, and overwrite was false");
            }
            using (var fileStream = File.Create(path))
            {
                stream.CopyTo(fileStream);
            }
        }

        /// <inheritdoc/>
        public async Task SaveFileAsync(Stream stream, string path, bool overwrite)
        {
            if (await _fileService.ExistsAsync(path))
            {
                if (overwrite)
                    await _fileService.DeleteAsync(path);
                else
                    throw new HyperlambdaException("File already exists, and overwrite was false");
            }
            using (var fileStream = File.Create(path))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
    }
}
