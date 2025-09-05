namespace Praticse.Helpers;

using System;
using System.IO;
// Updated ImageHelper method for fixed filename
public static class ImageHelper
{
    public static string SaveImageWithFixedName(IFormFile imageFile, string dir, string fileName)
    {
        string finalDirPath = $"wwwroot/{dir}";
        if (imageFile == null || imageFile.Length == 0)
        {
            throw new Exception("No image file provided");
        }
        if (!Directory.Exists(finalDirPath))
        {
            Directory.CreateDirectory(finalDirPath);
        }

        // Extract extension from uploaded file
        string fileExtension = Path.GetExtension(imageFile.FileName);

        // Use fixed filename with original extension
        string fixedFileName = $"{fileName}{fileExtension}";

        // Get full path to store the file
        string fullPathToWrite = Path.Combine(finalDirPath, fixedFileName);

        // Delete existing file if it exists (to override)
        if (File.Exists(fullPathToWrite))
        {
            File.Delete(fullPathToWrite);
        }

        // Save the new image
        using (FileStream stream = new FileStream(fullPathToWrite, FileMode.Create))
        {
            imageFile.CopyTo(stream);
        }

        // Return path for reference (relative to wwwroot)
        return $"{dir}/{fixedFileName}";
    }
}
