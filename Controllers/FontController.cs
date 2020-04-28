using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace XGGrid_Docker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FontController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return GetAllFontsName()
;
        }


        private string[] GetAllFontsName()
        {
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();

            // Get the array of FontFamily objects.
            FontFamily[] fontFamilies = installedFontCollection.Families;

            // The loop below creates a large string that is a comma-separated
            // list of all font family names.
            string[] names = new string[fontFamilies.Length];
            int ptr = 0;
            
            foreach (FontFamily fontFamily in fontFamilies)
            {
                var name = fontFamily.Name;
                name += fontFamily.IsStyleAvailable(FontStyle.Regular) ? " Regular" : "";
                name += fontFamily.IsStyleAvailable(FontStyle.Bold) ? " Bold" : "";
                name += fontFamily.IsStyleAvailable(FontStyle.Italic) ? " Italic" : "";
                names[ptr++] = name;
            }
            return names;
        }

    }
}