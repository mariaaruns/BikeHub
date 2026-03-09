namespace BikeHub.Extension
{
    public class ImageHelper
    {

        public static async Task<string> SaveImageAsync(byte[] imageBytes,
        string folderPath,
        string fileNameWithoutExtension,
        string extension)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                throw new ArgumentException("Image bytes are empty");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = $"{fileNameWithoutExtension}_{Guid.NewGuid()}{extension}";
            string fullPath = Path.Combine(folderPath, fileName);

            await File.WriteAllBytesAsync(fullPath, imageBytes);

            return fileName;
        }

        public static string GetImageExtension(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 4)
                return string.Empty;

            // JPG
            if (bytes[0] == 0xFF &&
                bytes[1] == 0xD8 &&
                bytes[2] == 0xFF)
                return ".jpg";

            // PNG
            if (bytes[0] == 0x89 &&
                bytes[1] == 0x50 &&
                bytes[2] == 0x4E &&
                bytes[3] == 0x47)
                return ".png";

            // GIF
            if (bytes[0] == 0x47 &&
                bytes[1] == 0x49 &&
                bytes[2] == 0x46)
                return ".gif";

            // BMP
            if (bytes[0] == 0x42 &&
                bytes[1] == 0x4D)
                return ".bmp";

            // WebP (RIFF....WEBP)
            if (bytes[0] == 0x52 &&
                bytes[1] == 0x49 &&
                bytes[2] == 0x46 &&
                bytes[3] == 0x46)
                return ".webp";

            return string.Empty;
        }
    }
}
