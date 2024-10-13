using System;

namespace CrossCutting.Extensions.UseCases;

public static class ImageExtension
{
    public static bool IsPngOrBmp(
        string base64String)
    {
        base64String = base64String.Trim();

        try
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);

            if (imageBytes.Length >= 8 &&
                imageBytes[0] == 0x89 &&
                imageBytes[1] == 0x50 &&
                imageBytes[2] == 0x4E &&
                imageBytes[3] == 0x47 &&
                imageBytes[4] == 0x0D &&
                imageBytes[5] == 0x0A &&
                imageBytes[6] == 0x1A &&
                imageBytes[7] == 0x0A)
                return true;


            if (imageBytes.Length >= 2 &&
                imageBytes[0] == 0x42 &&
                imageBytes[1] == 0x4D)
                return true;
        }
        catch
        {
        }

        return false;
    }
}