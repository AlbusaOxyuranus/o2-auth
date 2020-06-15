using System;
using SkiaSharp;
using SkiaSharp.QrCode;

namespace O2.Auth.Web.Utilities
{
    public interface IBase64QrCodeGenerator 
    { 
        string Generate(Uri target); 
    } 

    public class Base64QrCodeGenerator : IBase64QrCodeGenerator 
    { 
        public string Generate(Uri target) 
        { 
            // https://github.com/guitarrapc/SkiaSharp.QrCode 
             
            using (var generator = new QRCodeGenerator()) 
            { 
                var code = generator.CreateQrCode(target.OriginalString, ECCLevel.Q); 
                 
                var info = new SKImageInfo(256, 256); 
                using (var surface = SKSurface.Create(info)) 
                { 
                    var canvas = surface.Canvas; 
                    canvas.Render(code, info.Width, info.Height); 
 
                    using (var image = surface.Snapshot()) 
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100)) 
                    { 
                        return Convert.ToBase64String(data.ToArray()); 
                    } 
                } 
            }             
        } 
    } 
}