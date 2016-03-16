using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Bergfall.Utils
{
    public static class Imaging
    {
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
