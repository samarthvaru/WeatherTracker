using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;


namespace WeatherTracker.Controllers{
    public class BlobStorageService
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly string containerName = "user-city-data"; // Replace with your container name

        public BlobStorageService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureBlobStorageConnection");
            blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task StoreUserCity(string userEmail, string city)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient($"{userEmail}.json");

            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                // Construct JSON data
                string json = $"{{ \"{userEmail}\": [\"{city}\"] }}";
                writer.Write(json);
                writer.Flush();
                stream.Position = 0;

                // Upload JSON data to Blob Storage
                await blobClient.UploadAsync(stream, true);
            }
        }
    }
}