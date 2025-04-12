using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace WebApplication3.Utils
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string?> UploadImageAsync(string localImagePath)
        {
            try
            {
                if (!File.Exists(localImagePath))
                {
                    throw new FileNotFoundException("Image not found on local storage.");
                }

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(localImagePath),
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = false,
                    Folder = "Mobile"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Delete the image from local storage after successful upload
                    File.Delete(localImagePath);
                    return uploadResult.SecureUrl.ToString();
                }

                return null; // Upload failed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return null;
            }
        }
    }
}
