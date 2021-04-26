using System.IO;
using UnityEngine;

namespace LD48.Utils
{
	public static class TextureUtils
	{
		public static Texture2D ScaleTexture(this Texture2D source, int targetWidth, int targetHeight)
		{
			var result = new Texture2D(targetWidth, targetHeight, source.format, true);
			var resultPixels = result.GetPixels(0);
			var incX = 1.0f / targetWidth;
			var incY = 1.0f / targetHeight;
			for (var pixel = 0; pixel < resultPixels.Length; pixel++)
			{
				resultPixels[pixel] = source.GetPixelBilinear(incX * ((float)pixel % targetWidth),
					incY * Mathf.Floor(pixel / targetWidth));
			}

			result.SetPixels(resultPixels, 0);
			result.Apply();
			return result;
		}

		public static Texture2D LoadTexture(string FilePath)
		{
			if (File.Exists(FilePath))
			{
				var FileData = File.ReadAllBytes(FilePath);
				var Tex2D = new Texture2D(2, 2);
				if (Tex2D.LoadImage(FileData))
				{
					return Tex2D;
				}
			}
			return null;
		}
	}
}