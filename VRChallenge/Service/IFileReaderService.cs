using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChallenge.Service
{
    public interface IFileReaderService : IDisposable
    {
        StreamReader GetStreamReader(string filePath);
    }
}
