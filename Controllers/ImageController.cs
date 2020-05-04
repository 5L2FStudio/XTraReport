using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XGGrid_Docker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [HttpGet("{filetype}")]
        public ActionResult<Stream> Get(string filetype)
        {
            Stream stream;

            switch (filetype)
            {
                case "Black": stream = Test("微軟正黑體"); break;
                case "Light": stream = Test("華康細黑體"); break;
                case "Medium": stream = Test("華康中黑體"); break;
                default: stream = null; break;
            }
            stream.Position = 0;
            return File(stream, "image/jpeg");

        }

        private Stream Test(string fontName)
        {
            Stream stream = new System.IO.MemoryStream();

            Bitmap bitmap = new Bitmap(300, 100);

            using ( Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawString("東南西北金木水火土", new Font(fontName, 12), Brushes.White, new PointF(10, 10));
                g.DrawString("ABCDEFGHIJKLMNOPQ", new Font(fontName, 12), Brushes.White, new PointF(10, 50));
            }

            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

            return stream;
        }
    }
}
