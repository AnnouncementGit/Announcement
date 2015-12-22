using System;
using System.Threading.Tasks;
using System.IO;
using Android.Graphics;
using Android.Media;

namespace Announcement.Android
{
	public static class ImageDecoderHelper
	{
        public static byte[] FilePathToByteArray(string path, int size)
        {
            byte[] result = null;

            try
            {
                using (var bitmap = ScaleBitmap(path, size))
                {
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Jpeg, 70, stream);
                   
                        result = stream.ToArray();

                        stream.Close();
                    }

                    bitmap.Recycle();
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        private static Bitmap ScaleBitmap(string path, int size)
        {
            try
            {
                var bitmap = DecodeImage(path, size);

                var angle = Orientation(path);

                Bitmap result = null;

                if (angle == 0)
                {
                    if (bitmap.Height < size && bitmap.Width < size)
                    {
                        return bitmap;                   
                    }
                    else
                    {
                        var bitmapHeight = bitmap.Height;

                        var bitmapWidth = bitmap.Width;

                        if (bitmapHeight > bitmapWidth)
                        {
                            bitmapWidth = (int)(size / (bitmapHeight / (float)bitmapWidth));

                            bitmapHeight = size;
                        }
                        else
                        {
                            bitmapHeight = (int)(size / (bitmapWidth / (float)bitmapHeight));

                            bitmapWidth = size;
                        }

                        result = Bitmap.CreateScaledBitmap(bitmap, bitmapWidth, bitmapHeight, true);
                    }
                }
                else
                {
                    using (var matrix = new Matrix())
                    {
                        matrix.PostRotate(angle);
                        
                        if (bitmap.Height > size || bitmap.Width > size)
                        {
                            var bitmapHeight = bitmap.Height;

                            var bitmapWidth = bitmap.Width;


                            if (bitmapHeight > bitmapWidth)
                            {
                                bitmapWidth = (int)(size / (bitmapHeight / (float)bitmapWidth));

                                bitmapHeight = size;
                            }
                            else
                            {
                                bitmapHeight = (int)(size / (bitmapWidth / (float)bitmapHeight));

                                bitmapWidth = size;
                            }

                            var scaledBitmap = Bitmap.CreateScaledBitmap(bitmap, bitmapWidth, bitmapHeight, true);  

                            result = Bitmap.CreateBitmap(scaledBitmap, 0, 0, scaledBitmap.Width, scaledBitmap.Height, matrix, true);

                            scaledBitmap.Recycle();

                            scaledBitmap.Dispose();
                        }
                        else
                        {
                            result = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);
                        }
                    }
                }

                bitmap.Recycle();

                bitmap.Dispose();

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static int Orientation(String path)
        {
            var rotate = 0;

            try
            {
                var exif = new ExifInterface(path);

                var orientation = exif.GetAttributeInt(ExifInterface.TagOrientation, (int)global::Android.Media.Orientation.Normal);

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

		private static Bitmap DecodeImage(string path, int size)
        {
            try
            {
                using (var options = new BitmapFactory.Options())
                {
                    options.InJustDecodeBounds = true;

                    BitmapFactory.DecodeFile(path, options);

                    options.InSampleSize = OptimalInSampleSize(options, size);

                    options.InJustDecodeBounds = false;

                    options.InPreferredConfig = Bitmap.Config.Rgb565;

                    return BitmapFactory.DecodeFile(path, options);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static int OptimalInSampleSize(BitmapFactory.Options options, int size)
		{
			var height = options.OutHeight;

			var width = options.OutWidth;

			var inSampleSize = 1;

            while (height > size || width > size)
            {
                inSampleSize *= 2;

                height = height / inSampleSize;

                width = width / inSampleSize;
            }
                
			return inSampleSize;
		}
	}
}

