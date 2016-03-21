using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;


namespace Bergfall.Utils
{
    public static class Imaging
    {
        /* Screenshots */
        public class ScreenCapture
        {
            /// <summary>
            /// Creates an Image object containing a screen shot of the entire desktop
            /// </summary>
            /// <returns></returns>
            public Image CaptureScreen()
            {
                return CaptureWindow(User32.GetDesktopWindow());
            }
            /// <summary>
            /// Creates an Image object containing a screen shot of a specific window
            /// </summary>
            /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
            /// <returns></returns>
            public Image CaptureWindow(IntPtr handle)
            {
                // get te hDC of the target window
                IntPtr hdcSrc = User32.GetWindowDC(handle);
                // get the size
                User32.RECT windowRect = new User32.RECT();
                User32.GetWindowRect(handle, ref windowRect);
                int width = windowRect.right - windowRect.left;
                int height = windowRect.bottom - windowRect.top;
                // create a device context we can copy to
                IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
                // create a bitmap we can copy it to,
                // using GetDeviceCaps to get the width/height
                IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
                // select the bitmap object
                IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
                // bitblt over
                GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
                // restore selection
                GDI32.SelectObject(hdcDest, hOld);
                // clean up 
                GDI32.DeleteDC(hdcDest);
                User32.ReleaseDC(handle, hdcSrc);
                // get a .NET image object for it
                Image img = Image.FromHbitmap(hBitmap);
                // free up the Bitmap object
                GDI32.DeleteObject(hBitmap);
                return img;
            }
            /// <summary>
            /// Captures a screen shot of a specific window, and saves it to a file
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="filename"></param>
            /// <param name="format"></param>
            public void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
            {
                Image img = CaptureWindow(handle);
                img.Save(filename, format);
            }
            /// <summary>
            /// Captures a screen shot of the entire desktop, and saves it to a file
            /// </summary>
            /// <param name="filename"></param>
            /// <param name="format"></param>
            public void CaptureScreenToFile(string filename, ImageFormat format)
            {
                Image img = CaptureScreen();
                img.Save(filename, format);
            }

            /// <summary>
            /// Helper class containing Gdi32 API functions
            /// </summary>
            private class GDI32
            {

                public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
                [DllImport("gdi32.dll")]
                public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                    int nWidth, int nHeight, IntPtr hObjectSource,
                    int nXSrc, int nYSrc, int dwRop);
                [DllImport("gdi32.dll")]
                public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                    int nHeight);
                [DllImport("gdi32.dll")]
                public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
                [DllImport("gdi32.dll")]
                public static extern bool DeleteDC(IntPtr hDC);
                [DllImport("gdi32.dll")]
                public static extern bool DeleteObject(IntPtr hObject);
                [DllImport("gdi32.dll")]
                public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
            }

            /// <summary>
            /// Helper class containing User32 API functions
            /// </summary>
            private class User32
            {
                [StructLayout(LayoutKind.Sequential)]
                public struct RECT
                {
                    public int left;
                    public int top;
                    public int right;
                    public int bottom;
                }
                [DllImport("user32.dll")]
                public static extern IntPtr GetDesktopWindow();
                [DllImport("user32.dll")]
                public static extern IntPtr GetWindowDC(IntPtr hWnd);
                [DllImport("user32.dll")]
                public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
                [DllImport("user32.dll")]
                public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
            }
        }
    
    public static Image DrawRef(string filename, Color toBG)
        {
            Image ime;

            string[] allowedImages = { "jpg", "png", "gif", "jpeg" };

            bool isImage = false;

            while (!isImage)
            {

                foreach (string allowImage in allowedImages)
                {
                    if (filename.EndsWith(allowImage, true, null))
                    {
                        isImage = true;
                    }
                }
            }
            if (isImage)
            {
                FileStream strm = new FileStream(filename, FileMode.Open);
                //pictureBox1.Image = Image.FromStream(strm);
                ime = Image.FromStream(strm);
                strm.Close();
                //menuItem8.Enabled = true;
                return DrawReflection(ime, toBG);
            }
            else
            {
                ime = null;
                return ime;
            }
        }
        public static Bitmap BergfallImgFixer(Image img, int width, int height, int gradientHeight)
        {
            Bitmap b = new Bitmap(width, height + gradientHeight);
            Graphics gfx = Graphics.FromImage(b);
            gfx.DrawImage(img, 0, 0, width, height);
            Rectangle rt = new Rectangle(0, 0, width, height + gradientHeight);
            gfx.DrawImageUnscaledAndClipped(img, rt);
            return b;
            
            
        }
        public static Image DrawReflection(Image img, Color toBG) // img is the original image.
        {
            //This is the static function that generates the reflection...
            int height = img.Height + 100; //Added height from the original height of the image.
            Bitmap bmp = new Bitmap(img.Width, height, PixelFormat.Format64bppPArgb); //A new 
            //bitmap.
            //The Brush that generates the fading effect to a specific color of your background.
            Brush brsh = new LinearGradientBrush(new Rectangle(0, 0, img.Width + 10,
              height), Color.Transparent, toBG, LinearGradientMode.Vertical);
            bmp.SetResolution(img.HorizontalResolution, img.VerticalResolution); //Sets the new 
            //bitmap's resolution.
            using (Graphics grfx = Graphics.FromImage(bmp)) //A graphics to be generated 
            //from an image (here, the new Bitmap we've created (BMP)).
            {
                Bitmap bm = (Bitmap)img; //Generates a bitmap from the original image (img).
                grfx.DrawImage(bm, 0, 0, img.Width, img.Height); //Draws the generated 
                //bitmap (bm) to the new bitmap (bmp).
                Bitmap bm1 = (Bitmap)img; 	//Generates a bitmap again 
                //from the original image (img).
                bm1.RotateFlip(RotateFlipType.Rotate180FlipX); //Flips and rotates the 
                //image (bm1).
                grfx.DrawImage(bm1, 0, img.Height); 	//Draws (bm1) below (bm) so it serves 
                //as the reflection image.
                Rectangle rt = new Rectangle(0, img.Height, img.Width, 100); //A new rectangle 
                //to paint our gradient effect.
                grfx.FillRectangle(brsh, rt); //Brushes the gradient on (rt).
            }
            return bmp; //Returns the (bmp) with the generated image.
        }
        //public void CreateThumbs(string originPath, string outputPath)
        //{
        //    string[] pattern = { ".jpg", ".jpeg", ".png" };
        //    string[] files = Directory.GetFiles(originPath, "*", SearchOption.AllDirectories);
        //    int nr = 1;

        //    foreach (string file in files)
        //    {
        //        if (file.EndsWith(pattern[0]) || file.EndsWith(pattern[1]) || file.EndsWith(pattern[2]))
        //        {
        //            try
        //            {
        //                Bitmap bitmap = new Bitmap(file);

        //                int orgHeight = bitmap.Height;
        //                int orgWidth = bitmap.Width;
        //                double ratio = orgWidth / Convert.ToDouble(orgHeight);
        //                int newHeight = Convert.ToInt32(150 / ratio);
        //                Bitmap thumb = new Bitmap(150, newHeight);
        //                Graphics g = Graphics.FromImage(thumb);
        //                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //                g.FillRectangle(Brushes.White, 0, 0, 150, newHeight);
        //                g.DrawImage(bitmap, 0, 0, 150, newHeight);
        //                bitmap.Dispose();

        //                string filename = outputPath + "thumb" + nr + ".jpg";
        //                thumb.Save(filename, ImageFormat.Jpeg);
        //                nr++;

        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }
        //}
    }
}
