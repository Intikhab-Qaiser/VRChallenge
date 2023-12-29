// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using VRChallenge;
using VRChallenge.Service;
using Microsoft.Extensions.DependencyInjection;


IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

var appSettings = new AppSettings();
configuration.GetSection("AppSettings").Bind(appSettings);

string folderPath = appSettings.FolderPath;
string tableName = appSettings.TableName;

var serviceProvider = new ServiceCollection()
            .AddScoped<IFileReaderService, FileReaderService>()
            .AddScoped<IFileParserService, FileParserService>()
            .AddScoped<IDbService>(provider => 
            { 
                return new DbService(tableName); 
            })
            .BuildServiceProvider();

using (FileSystemWatcher watcher = new(folderPath))
{
    watcher.Created += OnFileCreated;

    watcher.EnableRaisingEvents = true;

    Console.WriteLine($"Monitoring folder: {folderPath}");
    Console.ReadLine(); // Keep the application running
}

void OnFileCreated(object sender, FileSystemEventArgs e)
{
    Console.WriteLine($"New file created: {e.FullPath}");

    using (var scope = serviceProvider.CreateScope())
    {
        var fileReaderService = scope.ServiceProvider.GetRequiredService<IFileReaderService>();
        var fileParserService = scope.ServiceProvider.GetRequiredService<IFileParserService>();
        var dbService = scope.ServiceProvider.GetRequiredService<IDbService>();

        var parsedData = fileParserService.ParseFile(e.FullPath);

        if (parsedData?.Count > 0)
        {
            dbService.Save(parsedData).ConfigureAwait(false);
        }
    }
}

