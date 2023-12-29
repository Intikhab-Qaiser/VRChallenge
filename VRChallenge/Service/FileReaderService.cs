using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChallenge.Service
{
    public class FileReaderService : IFileReaderService
    {
        public StreamReader GetStreamReader(string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return new StreamReader(fileStream);
        }

        public void Dispose()
        {
            // Cleanup resources if needed
        }
    }
}
