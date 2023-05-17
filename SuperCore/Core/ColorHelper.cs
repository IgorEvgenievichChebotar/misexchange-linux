using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ru.novolabs.SuperCore
{
    public static class ColorHelper
    {
        public static int ToRgb(this Color color)
        {
            string hexchars = BitConverter.ToString(new byte[] {  color.R, color.G, color.B }).Replace("-", String.Empty);
            return Int32.Parse(hexchars, System.Globalization.NumberStyles.HexNumber);       
        }

        public static int ToARgb(this Color color)
        {
            string hexchars = BitConverter.ToString(new byte[] { color.A, color.R, color.G, color.B }).Replace("-", String.Empty);
            return Int32.Parse(hexchars, System.Globalization.NumberStyles.HexNumber);
        }

        //public static long ToARgb(this Color color)
        //{
        //    string hexchars = BitConverter.ToString(new byte[] { color.A, color.R, color.G, color.B }).Replace("-", String.Empty);
        //    return long.Parse(hexchars, System.Globalization.NumberStyles.HexNumber);
        //}

        public static Color ToRgbColor(this int rgb)
        {
            string str = rgb.ToString("X6");
            str = str.Length == 8 ? str.Substring(2) : str;
            byte[] rgbBytes = new byte[3];
            for (int i = 0; i < str.Length; i += 2)
            {
                string hexChars = String.Concat(str[i], str[i + 1]);
                rgbBytes[i / 2] = Byte.Parse(hexChars, System.Globalization.NumberStyles.HexNumber);
            }            
            
            return Color.FromArgb(rgbBytes[0], rgbBytes[1], rgbBytes[2]);
        }

        public static Color ToARgbColor(this int rgb)
        {
            string str = rgb.ToString("X6");
            //str = str.Length == 8 ? str.Substring(2) : str;
            byte[] rgbBytes = new byte[4];
            for (int i = 0; i < str.Length; i += 2)
            {
                string hexChars = String.Concat(str[i], str[i + 1]);
                rgbBytes[i / 2] = Byte.Parse(hexChars, System.Globalization.NumberStyles.HexNumber);
            }

            return Color.FromArgb(rgbBytes[0], rgbBytes[1], rgbBytes[2], rgbBytes[3]);
        }

        public static Color ToRgbFromBgr(this int bgr)
        {
            string str = bgr.ToString("X6");
            str = str.Length == 8 ? str.Substring(2) : str;
            byte[] rgbBytes = new byte[3];
            for (int i = 0; i < str.Length; i += 2)
            {
                string hexChars = String.Concat(str[i], str[i + 1]);
                rgbBytes[i / 2] = Byte.Parse(hexChars, System.Globalization.NumberStyles.HexNumber);
            }
            
            return Color.FromArgb(rgbBytes[2], rgbBytes[1], rgbBytes[0]);
        }

        public static String ToRgbStringFromBgr(this int bgr)
        {
            string str = bgr.ToString("X6");
            str = str.Length == 8 ? str.Substring(2) : str;
            byte[] rgbBytes = new byte[3];
            String Color = "";
            for (int i = str.Length - 1; i > 0; i -= 2)
            {
                string hexChars = String.Concat(str[i - 1], str[i]);
                rgbBytes[i / 2] = Byte.Parse(hexChars, System.Globalization.NumberStyles.HexNumber);
                Int32 dec = Convert.ToInt32(rgbBytes[i / 2]);


                Color += Convert.ToInt32(rgbBytes[i / 2]).ToString("X2");
            }

            return Color;
        }
    }
}
