//using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.Crypto
{
    public static class Compresser
    {
        public static byte[] Decompress(byte[] zippedData)
        {
            byte[] decompressedData = null;
            //zippedData = Convert.FromBase64String("Rc3NCsIwEATgu08R9240/REPaXrQHkWhBem1zSKBZiPdVH1824N6G5hhPl2+/SCeOLILVICSOxBIfbCO7gXcHNnw4o1KcgWlWen16XJs2mslRuRHIEZRt3VTnQUMzrO00cK8+pXd5AZLk+9wLCCR+1SmC8DIC+fsDGaZyg9gdB8oIkWjt//0/TEfAAAAAAAA");
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (MemoryStream inputStream = new MemoryStream(zippedData))
                {
                    using (DeflateStream zip = new DeflateStream(inputStream, CompressionMode.Decompress))
                    //using (DeflateStream zip = new DeflateStream(inputStream, CompressionLevel.Fastest))
                    {
                        zip.CopyTo(outputStream);
                        zip.Flush();
                    }
                    /*inputStream.Seek(0, SeekOrigin.Begin);
                    using (GZipInputStream decompressionStream = new GZipInputStream(inputStream))
                    {
                        decompressionStream.CopyTo(outputStream);
                        decompressionStream.Flush();
                    }*/
                }
                decompressedData = outputStream.ToArray();
                outputStream.Close();
            }

            return decompressedData;
        }

        public static String Decompress(byte[] zippedData, Encoding encoding)
        {
            byte[] decompressedData = Decompress(zippedData);

            return encoding.GetString(decompressedData);
        }

        public static byte[] Compress(byte[] plainData)
        {
            byte[] compressesData = null;
            using (MemoryStream outputStream = new MemoryStream())
            {
                // Тарасов: Очень жаль, но в .Net Framework 4 у класса System.IO.Compression.DeflateStream нет конструктора, принимающего в кач-ве аргумента степень сжатия
                // Такой конструктор появился в .Net 4.5, если библиотека nlscorlib.dll не должна быть совместима с Windows Server 2003 и Windows XP,
                // то можно нацелить проект на .Net 4.5 и использовать следующий конструктор:                
                //using (DeflateStream zip = new DeflateStream(outputStream, CompressionLevel.Fastest))
                // Однако поверхностное тестирование показало, что существенно время на накладные расходы уменьшить не удалось
                using (DeflateStream zip = new DeflateStream(outputStream, CompressionMode.Compress))
                {                    
                    zip.Write(plainData, 0, plainData.Length);
                    zip.Flush();
                }
                compressesData = outputStream.ToArray();
                outputStream.Close();
            }

            return compressesData;
        }

        public static string Zip(string value)
        {
            //Transform string into byte[]  
            byte[] byteArray = new byte[value.Length];
            int indexBA = 0;
            foreach (char item in value.ToCharArray())
            {
                byteArray[indexBA++] = (byte)item;
            }

            //Prepare for compress
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms,
                System.IO.Compression.CompressionMode.Compress);

            //Compress
            sw.Write(byteArray, 0, byteArray.Length);
            //Close, DO NOT FLUSH cause bytes will go missing...
            sw.Close();

            //Transform byte[] zip data to string
            byteArray = ms.ToArray();
            System.Text.StringBuilder sB = new System.Text.StringBuilder(byteArray.Length);
            foreach (byte item in byteArray)
            {
                sB.Append((char)item);
            }
            ms.Close();
            sw.Dispose();
            ms.Dispose();
            return sB.ToString();
        }

        public static byte[] Zip(string value, Encoding encoding)
        {
            //Transform string into byte[]  
            byte[] byteArray = encoding.GetBytes(value);

            //Prepare for compress
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms,
                System.IO.Compression.CompressionMode.Compress);

            //Compress
            sw.Write(byteArray, 0, byteArray.Length);
            //Close, DO NOT FLUSH cause bytes will go missing...
            sw.Close();

            //Transform byte[] zip data to string
            byteArray = ms.ToArray();
            return byteArray;
        }

        public static string UnZip(string value)
        {
            //Transform string into byte[]
            byte[] byteArray = new byte[value.Length];
            int indexBA = 0;
            foreach (char item in value.ToCharArray())
            {
                byteArray[indexBA++] = (byte)item;
            }

            //Prepare for decompress
            System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArray);
            System.IO.Compression.GZipStream sr = new System.IO.Compression.GZipStream(ms,
                System.IO.Compression.CompressionMode.Decompress);

            //Reset variable to collect uncompressed result
            byteArray = new byte[byteArray.Length];

            //Decompress
            int rByte = sr.Read(byteArray, 0, byteArray.Length);

            //Transform byte[] unzip data to string
            System.Text.StringBuilder sB = new System.Text.StringBuilder(rByte);
            //Read the number of bytes GZipStream red and do not a for each bytes in
            //resultByteArray;
            for (int i = 0; i < rByte; i++)
            {
                sB.Append((char)byteArray[i]);
            }
            sr.Close();
            ms.Close();
            sr.Dispose();
            ms.Dispose();
            return sB.ToString();
        }

        public static string UnZip(byte[] value, Encoding encoding)
        {
            //Transform string into byte[]
            byte[] byteArray = value;

            //Prepare for decompress
            System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArray);
            System.IO.Compression.GZipStream sr = new System.IO.Compression.GZipStream(ms,
                System.IO.Compression.CompressionMode.Decompress);

            //Reset variable to collect uncompressed result
            byteArray = new byte[byteArray.Length];

            //Decompress
            int rByte = sr.Read(byteArray, 0, byteArray.Length);

            //Transform byte[] unzip data to string
            String str = "";
            //Read the number of bytes GZipStream red and do not a for each bytes in
            //resultByteArray;
            str = encoding.GetString(byteArray);
            sr.Close();
            ms.Close();
            sr.Dispose();
            ms.Dispose();
            return str;
        }


        public static byte[] Compress(String plainData, Encoding encoding)
        {
            byte[] byteData = encoding.GetBytes(plainData);
            return Compress(byteData);
        }
    }
}
