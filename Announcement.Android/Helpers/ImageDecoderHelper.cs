using System;
using System.Threading.Tasks;
using System.IO;
using Android.Graphics;
using Android.Media;

namespace Announcement.Android
{
	public class ImageDecoderHelper
	{
		public ImageDecoderHelper ()
		{
		}

		public static async Task<byte[]> GetByteDataFromFile(string absolutePath)
		{
			FileStream fs = global::System.IO.File.OpenRead (absolutePath);
			var bitmapData = new byte[fs.Length];
			await fs.ReadAsync (bitmapData, 0, bitmapData.Length);
			fs.Close ();

			return bitmapData;
		}

		public static async Task<Bitmap> ScaleBitmap(string absolutePath, int width, int height)
		{
			try{
				var bm = await DecodeImage(absolutePath, width, height);

				Matrix m = new Matrix ();
				m.SetRectToRect(new RectF(0,0,bm.Width, bm.Height), new RectF(0,0,width,height), Matrix.ScaleToFit.Center);
				m.PostRotate(CameraPhotoOrientation (absolutePath));
				return Bitmap.CreateBitmap(bm, 0,0,bm.Width, bm.Height, m, true);}
			catch (Exception) 
			{
				return null;
			}
		}

		private static int CameraPhotoOrientation(String absolutePath)
		{
			int rotate = 0;
			try
			{
				ExifInterface exif = new ExifInterface(absolutePath);
				var orientation = exif.GetAttributeInt(ExifInterface.TagOrientation,(int) global::Android.Media.Orientation.Normal);
				switch (orientation)
				{
				case (int) global::Android.Media.Orientation.Rotate270:
					rotate = 270;
					break;
				case (int) global::Android.Media.Orientation.Rotate180:
					rotate = 180;
					break;
				case (int) global::Android.Media.Orientation.Rotate90:
					rotate = 90;
					break;
				}
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Exception during Camera orientations" + e.StackTrace + e.ToString());
			}
			return rotate;
		}

		private static async Task<Bitmap> DecodeImage(string absolutePath, int width, int height)
		{
			try 
			{
				var options = new BitmapFactory.Options ();
				options.InJustDecodeBounds = true;
				BitmapFactory.DecodeFile (absolutePath, options);

				options.InSampleSize = CalculateInSampleSize(options, width, height);

				options.InJustDecodeBounds = false;

				return await BitmapFactory.DecodeFileAsync(absolutePath, options);
			} 
			catch (Exception) 
			{
				return null;
			}
		}

		private static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
		{
			float height = options.OutHeight;
			float width = options.OutWidth;
			double inSampleSize = 2D; //was 1

			if (height > reqHeight || width > reqWidth)
			{
				int halfHeight = (int)(height / 2);
				int halfWidth = (int)(width / 2);

				while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
				{
					inSampleSize *= 2;
				}
			}

			return (int)inSampleSize;
		}
	}
}

