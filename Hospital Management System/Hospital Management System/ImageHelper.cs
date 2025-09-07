namespace Praticse.Helpers;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;

// Updated ImageHelper method for fixed filename with folder cleanup and JPG extension
public static class ImageHelper
{
    public static string SaveImageWithFixedName(IFormFile imageFile, string dir, string fileName)
    {
        string finalDirPath = $"wwwroot/{dir}";

        if (imageFile == null || imageFile.Length == 0)
        {
            throw new Exception("No image file provided");
        }

        // Create directory if it doesn't exist
        if (!Directory.Exists(finalDirPath))
        {
            Directory.CreateDirectory(finalDirPath);
        }

        // Delete all existing files from the folder first
        string[] existingFiles = Directory.GetFiles(finalDirPath);
        foreach (string existingFile in existingFiles)
        {
            try
            {
                File.Delete(existingFile);
            }
            catch (Exception ex)
            {
                // Log the exception if needed, but continue with other files
                Console.WriteLine($"Could not delete file {existingFile}: {ex.Message}");
            }
        }

        // Force JPG extension regardless of original file extension
        string fixedFileName = $"{fileName}.jpg";

        // Get full path to store the file
        string fullPathToWrite = Path.Combine(finalDirPath, fixedFileName);

        // Save the new image
        using (FileStream stream = new FileStream(fullPathToWrite, FileMode.Create))
        {
            imageFile.CopyTo(stream);
        }

        // Return path for reference (relative to wwwroot)
        return $"{dir}/{fixedFileName}";
    }
}