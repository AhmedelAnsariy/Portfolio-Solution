﻿namespace Portfolio.API.Helper
{
    public class FileHelper
    {
        public static string SaveFile(IFormFile file, string rootPath, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required.");

            string folderPath = Path.Combine(rootPath, folderName);


            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Path.Combine(folderName, fileName); // Relative path to the file
        }




        public bool DeleteDocument(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false; // File not found
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Exception occurred while deleting file: {ex.Message}");
                return false;
            }
        }







    }
}
