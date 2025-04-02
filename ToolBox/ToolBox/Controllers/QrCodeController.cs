using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ToolBoxQrcode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QrCodeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Generate(string text = "Hello", string format = "png")
        {
            using var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

            if (format.ToLower() == "svg")
            {
                var svgQrCode = new SvgQRCode(data);
                string svg = svgQrCode.GetGraphic(5);
                return Content(svg, "image/svg+xml");
            }
            else
            {
                var qrCode = new QRCode(data);
                using var bitmap = qrCode.GetGraphic(20);
                using var ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Png);
                return File(ms.ToArray(), "image/png");
            }
        }
    }
}
