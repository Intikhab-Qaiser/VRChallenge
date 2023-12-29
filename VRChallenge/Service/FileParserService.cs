using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Text;
using VRChallenge.DB;
using VRChallenge.Domain;

namespace VRChallenge.Service
{
    public class FileParserService : IFileParserService
    {
        private const string BOXSTART = "HDR";
        private const string ITEMSTART = "LINE";
        private readonly IFileReaderService fileService;

        
        public FileParserService(IFileReaderService fileService)
        {
            this.fileService = fileService;
        }

        public List<Box> ParseFile(string filePath)
        {
            try
            {
                var dataItems = new List<Box>();

                using (var reader = fileService.GetStreamReader(filePath))
                {
                    StringBuilder blockData = new();
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith(BOXSTART) && blockData.Length > 0)
                        {
                            var data = ProcessData(blockData);
                            dataItems.Add(data);
                        }

                        if (line.StartsWith(BOXSTART))
                        {
                            SetupSupplierJson(line, blockData);
                        }

                        if (line.StartsWith(ITEMSTART))
                        {
                            SetupItemJson(line, blockData);
                        }
                    }

                    // Process the last block
                    if (blockData.Length > 0)
                    {
                        var data = ProcessData(blockData);
                        dataItems.Add(data);
                    }
                }

                Console.WriteLine("File parsing completed successfully.");

                return dataItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                return null;
            }
        }

        private Box ProcessData(StringBuilder blockData)
        {
            blockData.AppendLine("   ]");
            blockData.AppendLine("}");

            var box = ConvertJsonToDomainModel(blockData.ToString());

            blockData.Clear();

            return box;
        }

        private Box ConvertJsonToDomainModel(string jsonData)
        {
            Box? box = null;
            try
            {
                box = JsonConvert.DeserializeObject<Box>(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return box;
        }

        private void SetupSupplierJson(string data, StringBuilder blockData)
        {
            blockData.AppendLine("{");
            blockData.AppendLine($"  \"supplierIdentifier\": \"{data.Substring(5, 7).Trim()}\",");
            blockData.AppendLine($"  \"identifier\": \"{data.Substring(97, 8).Trim()}\",");
        }

        private void SetupItemJson(string data, StringBuilder blockData)
        {
            if (!blockData.ToString().Contains("contents"))
            {
                blockData.AppendLine("  \"contents\":[");
            }

            blockData.AppendLine("  {");
            blockData.AppendLine($"    \"poNumber\": \"{data.Substring(5, 10).Trim()}\",");
            blockData.AppendLine($"    \"isbn\": \"{data.Substring(42, 13).Trim()}\",");
            blockData.AppendLine($"    \"quantity\": \"{data.Substring(76, 2).Trim()}\"");
            blockData.AppendLine("  },");
        }
    }
}
