using System;
using System.Collections;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bergfall.Utils
{
    public static class ExtensionMethods
    {
        
        // TEXT EXTENSTIONS
        public static bool IsRegexMatch(this string s, string regExpression)
        {
            Regex regexMatch = new Regex(regExpression, RegexOptions.IgnoreCase);

            return regexMatch.IsMatch(s.Trim());
        }
        public static string FirstMatch(this string s, string regex)
        {
            Regex firstMatch = new Regex(regex, RegexOptions.Multiline);
            Match match = firstMatch.Match(s.Trim(), 0);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }
        public static int ToInt(this string s)
        {
         
            return Int32.Parse(s);
        }
        public static bool IsNullOrEmpty(this string s)
        {
            if (s == null || s.Length < 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string IsNullThenGiveValue(this string s, string value)
        {
            if (s == null || s.Length < 1 && !value.IsNullOrEmpty())
            {
                s = value;
            }
            else
            {
            }
            return s;
        }
        public static string RemoveHTMLTags(this string s)
        {
            Regex htmlRegex = new Regex(@"<\/?[^>]+?>");
            return htmlRegex.Replace(s, String.Empty);
        }
        public static string[] SplitCSV(this string s)
        {
            char[] c = new char[] { ',' };
            return s.Split(c);
        }
        public static HtmlAgilityPack.HtmlNodeCollection GetNodesContainingAttributeValue(this HtmlAgilityPack.HtmlNode n, string element, string attribute, string value)
        {
            var nodes = n.SelectNodes(".//" + element + "[contains(concat(' ', normalize-space(@" + attribute + "), ' '), ' " + value + " ')]");
            return nodes;
        }
        public static HtmlAgilityPack.HtmlNode GetNodeContainingAttributeValue(this HtmlAgilityPack.HtmlNode n, string element, string attribute, string value)
        {
            var nodes = n.GetNodesContainingAttributeValue(element, attribute, value);
            if(nodes.Count > 0)
            {
                return nodes[0];
            }            
            else
            {
                return null;
            }
        }
        public static bool ContainedInArray(this string s, string[] values)
        {
            string str = s.ToLower().Trim();

            for (int i=0 ; i < values.Length ; i++)
            {
                if (values[i].Contains(str))
                {
                    return true;
                }
            }
            return false;
        }

        // DATETIME EXTENSIONS
        public static DateTime DotDate(this DateTime current, string ddDottMMdottYYYY)
        {
            Match match = Regex.Match(ddDottMMdottYYYY, @"(\d\d?)\.(\d\d?)\.(\d{2,4})");
            if (match.Success)
                current = new DateTime(Int32.Parse(match.Groups[3].Value), Int32.Parse(match.Groups[2].Value), Int32.Parse(match.Groups[1].Value));
            else
                current = new DateTime(1978, 11, 24);

            return current;
        }
        public static DateTime FromInt(this DateTime current, int date)
        {
            current = DateTime.ParseExact(date.ToString(), "yyyyMMdd", CultureInfo.CurrentCulture);
            return current;
        }

        // IMAGE EXTENSIONS
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }
        public static void SaveAsJpeg(this Image Img, string FileName, Int64 Quality)
        {
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;

            using (EncoderParameters EP = new EncoderParameters(1))
            {
                using (EncoderParameter QualityEncoderParameter = new EncoderParameter(QualityEncoder, Quality))
                {
                    EP.Param[0] = QualityEncoderParameter;
                    Img.Save(FileName, jgpEncoder, EP);
                }
            }
        }
        public static void SaveAsGif(this Image Img, string FileName, Int64 Quality)
        {
            ImageCodecInfo gifEncoder = GetEncoder(ImageFormat.Gif);
            System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;

            using (EncoderParameters EP = new EncoderParameters(1))
            {
                using (EncoderParameter QualityEncoderParameter = new EncoderParameter(QualityEncoder, Quality))
                {
                    EP.Param[0] = QualityEncoderParameter;
                    Img.Save(FileName, gifEncoder, EP);
                }
            }
        }
        public static Image Resize(this Image Img, int Width, int Height, InterpolationMode InterpolationMode)
        {
            Image CropedImage = new Bitmap(Width, Height);
            using (Graphics G = Graphics.FromImage(CropedImage))
            {
                G.SmoothingMode = SmoothingMode.HighQuality;
                G.InterpolationMode = InterpolationMode;
                G.PixelOffsetMode = PixelOffsetMode.HighQuality;
                G.DrawImage(Img, 0, 0, Width, Height);
            }

            return CropedImage;
        }
        public static Image Resize(this Image Img, int Width, int Height)
        {
            return Img.Resize(Width, Height, InterpolationMode.HighQualityBicubic);
        }
        private static Rectangle EnsureAspectRatio(this Image Image, int Width, int Height)
        {
            float AspectRatio = Width / (float)Height;
            float CalculatedWidth = Image.Width, CalculatedHeight = Image.Height;

            if (Image.Width >= Image.Height)
            {
                if (Width > Height)
                {
                    CalculatedHeight = Image.Width / AspectRatio;
                    if (CalculatedHeight > Image.Height)
                    {
                        CalculatedHeight = Image.Height;
                        CalculatedWidth = CalculatedHeight * AspectRatio;
                    }
                }
                else
                {
                    CalculatedWidth = Image.Height * AspectRatio;
                    if (CalculatedWidth > Image.Width)
                    {
                        CalculatedWidth = Image.Width;
                        CalculatedHeight = CalculatedWidth / AspectRatio;
                    }
                }
            }
            else
            {
                if (Width < Height)
                {
                    CalculatedHeight = Image.Width / AspectRatio;
                    if (CalculatedHeight > Image.Height)
                    {
                        CalculatedHeight = Image.Height;
                        CalculatedWidth = CalculatedHeight * AspectRatio;
                    }
                }
                else
                {
                    CalculatedWidth = Image.Height * AspectRatio;
                    if (CalculatedWidth > Image.Width)
                    {
                        CalculatedWidth = Image.Width;
                        CalculatedHeight = CalculatedWidth / AspectRatio;
                    }
                }
            }
            return Rectangle.Ceiling(new RectangleF((Image.Width - CalculatedWidth) / 2, 0, CalculatedWidth, CalculatedHeight));
        }
        public static Image ResizeToCanvas(this Image Img, int Width, int Height, out Rectangle CropRectangle)
        {
            return Img.ResizeToCanvas(Width, Height, InterpolationMode.HighQualityBicubic, out CropRectangle);
        }
        public static Image ResizeToCanvas(this Image Img, int Width, int Height, InterpolationMode InterpolationMode, out Rectangle CropRectangle)
        {
            CropRectangle = EnsureAspectRatio(Img, Width, Height);
            Image CropedImage = new Bitmap(Width, Height);

            using (Graphics G = Graphics.FromImage(CropedImage))
            {
                G.SmoothingMode = SmoothingMode.HighQuality;
                G.InterpolationMode = InterpolationMode;
                G.PixelOffsetMode = PixelOffsetMode.HighQuality;
                G.DrawImage(Img, new Rectangle(0, 0, Width, Height), CropRectangle, GraphicsUnit.Pixel);
            }

            return CropedImage;
        }
        public static Image ResizeToCanvas(this Image Img, int Width, int Height, RectangleF CR)
        {
            return Img.ResizeToCanvas(Width, Height, InterpolationMode.HighQualityBicubic, CR);
        }
        public static Image ResizeToCanvas(this Image Img, int Width, int Height, InterpolationMode InterpolationMode, RectangleF CR)
        {
            Image CropedImage = new Bitmap(Width, Height);
            using (Graphics G = Graphics.FromImage(CropedImage))
            {
                G.SmoothingMode = SmoothingMode.HighQuality;
                G.InterpolationMode = InterpolationMode;
                G.PixelOffsetMode = PixelOffsetMode.HighQuality;
                G.DrawImage(Img, new Rectangle(0, 0, Width, Height), CR, GraphicsUnit.Pixel);
            }

            return CropedImage;
        }

        //FILESYSTEMINFO
        public static FileType GetFileType(this FileInfo f)
        {
            switch (f.Extension)
            {
                case ".pdf":
                case ".chm":
                    return FileType.Info;

                case ".avi":
                case ".mpg":
                case ".flv":
                case ".mpeg":
                case ".mkv":
                case ".wmv":
                    return FileType.Video;

                case ".mp3":
                case ".flac":
                    return FileType.Music;
                case ".rar":
                case ".zip":
                case ".7z":
                    return FileType.Package;
                case ".iso":
                    return FileType.DiscImage;
                default:
                    return FileType.Other;
            }
        }
    }
}