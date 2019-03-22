using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using static System.Web.HttpContext;


namespace PleasantEnglish.Helpers
{
	public class ImageManager
	{
		public const string CatalogPath = "~/JsonData/catalog.txt";

		private static long LastTimeStamp = DateTime.UtcNow.Ticks;

		public static long UtcNowTicks
		{
			get
			{
				long original, newValue;
				do
				{
					original = LastTimeStamp;
					long now = DateTime.UtcNow.Ticks;
					newValue = Math.Max(now, original + 1);
				} while (Interlocked.CompareExchange
							 (ref LastTimeStamp, newValue, original) != original);

				return newValue;
			}
		}

		public static string GenerateId()
		{
			return Guid.NewGuid().ToString("N");
		}

		public static void DeleteImg(string path)
		{
			var serverPath = Current.Server.MapPath(path);
			if (serverPath != null && File.Exists(serverPath))
				File.Delete(serverPath);
		}

		public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
		{
			var ratioX = (double)maxWidth / image.Width;
			var ratioY = (double)maxHeight / image.Height;
			var ratio = Math.Min(ratioX, ratioY);

			var newWidth = (int)(image.Width * ratio);
			var newHeight = (int)(image.Height * ratio);

			var newImage = new Bitmap(newWidth, newHeight);

			using (var graphics = Graphics.FromImage(newImage))
				graphics.DrawImage(image, 0, 0, newWidth, newHeight);

			return newImage;
		}

		public static Image ScaleImageTo(Image source, int width, int height)
		{
			Image dest = new Bitmap(width, height);
			using (Graphics gr = Graphics.FromImage(dest))
			{
				gr.FillRectangle(Brushes.White, 0, 0, width, height);  // Очищаем экран
				gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

				float srcwidth = source.Width;
				float srcheight = source.Height;
				float dstwidth = width;
				float dstheight = height;

				if (srcwidth <= dstwidth && srcheight <= dstheight)  // Исходное изображение меньше целевого
				{
					int left = (width - source.Width) / 2;
					int top = (height - source.Height) / 2;
					gr.DrawImage(source, left, top, source.Width, source.Height);
				}
				else if (srcwidth / srcheight > dstwidth / dstheight)  // Пропорции исходного изображения более широкие
				{
					float cy = srcheight / srcwidth * dstwidth;
					float top = ((float)dstheight - cy) / 2.0f;
					if (top < 1.0f) top = 0;
					gr.DrawImage(source, 0, top, dstwidth, cy);
				}
				else  // Пропорции исходного изображения более узкие
				{
					float cx = srcwidth / srcheight * dstheight;
					float left = ((float)dstwidth - cx) / 2.0f;
					if (left < 1.0f) left = 0;
					gr.DrawImage(source, left, 0, cx, dstheight);
				}

				return dest;
			}
		}
	}
}