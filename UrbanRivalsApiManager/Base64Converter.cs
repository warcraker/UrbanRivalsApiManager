using System;
using System.Drawing;
using System.IO;

namespace UrbanRivalsApiManager.Utils
{
    public static class Base64Converter
    {
        /// <summary>
        /// Used for the "players.setPicture" ApiCall. Converts an image from a file to a Base64 string.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">filepath is null/empty/whitespace</exception>
        /// <exception cref="System.IO.FileNotFoundException">The specified file does not exist.</exception>
        public static string ConvertImageFileToBase64(string filepath)
        {
            if (String.IsNullOrWhiteSpace(filepath))
                throw new ArgumentNullException("filepath");

            using (var image = Image.FromFile(filepath))
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, image.RawFormat);
                byte[] imageBytes = memoryStream.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}
