using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace VidaForaneaCliente.Models
{
    public class Utils
    {
        public static byte[] ConvertImageToBytes(string imageSource)
        {
            MemoryStream ms = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            BitmapImage imagenFinal = new BitmapImage();
            imagenFinal.BeginInit();
            imagenFinal.UriSource = new Uri(imageSource);
            imagenFinal.DecodePixelHeight = 200;
            imagenFinal.DecodePixelWidth = 200;
            imagenFinal.EndInit();
            encoder.Frames.Add(BitmapFrame.Create(imagenFinal));
            encoder.Save(ms);
            return ms.ToArray();
        }

        public static BitmapImage ConvertBytesToImage(byte[] arrayDeImagen)
        {
            BitmapImage imagen = new BitmapImage();
            using (MemoryStream memStream = new MemoryStream(arrayDeImagen))
            {
                imagen.BeginInit();
                imagen.CacheOption = BitmapCacheOption.OnLoad;
                imagen.StreamSource = memStream;
                imagen.EndInit();
                imagen.Freeze();
            }
            return imagen;
        }
    }
}
