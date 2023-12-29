using VRChallenge.Domain;

namespace VRChallenge.Service
{
    public interface IFileParserService
    {
        List<Box> ParseFile(string filePath);
    }
}
