using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.DataAccess.Json;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace XGGrid_Docker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        //const string FontName = "Noto Sans T Chinese"; //"微軟正黑體" "Noto Sans T Chinese";
        private object GetDataSource()
        {
            var jsonDataSource = new JsonDataSource();
            jsonDataSource.JsonSource = new UriJsonSource(new Uri("https://tiptopapi.azurewebsites.net/Test/100"));
            jsonDataSource.Fill();
            return jsonDataSource;
        }

        private Stream Test(string fontName = "微軟正黑體")
        {
            XtraReport report = new XtraReport();
            report.DataSource = GetDataSource();
            ArrayList columnAL = new ArrayList()
            {
                "col1","col2","col3","col4","col5"
            };

            XRTable table1 = new XRTable();
            XRTable table2 = new XRTable();
            XRTableRow row1 = new XRTableRow();
            XRTableRow row2 = new XRTableRow();

            foreach (string column in columnAL)
            {
                XRTableCell cell = new XRTableCell();
                cell.Width = 100;
                cell.Text = column.Replace("col", "欄位");
                cell.Font = new Font(fontName, 10);

                row1.Cells.Add(cell);

                XRTableCell cell2 = new XRTableCell();
                cell2.Width = 100;
                cell2.DataBindings.Add("Text", null, column );
                cell2.Font = new System.Drawing.Font(fontName, 10);
                row2.Cells.Add(cell2);
            }
            table1.Rows.Add(row1);
            table1.Width = 100 * columnAL.Count;
            table1.Borders = BorderSide.Bottom;

            table2.Rows.Add(row2);
            table2.Width = 100 * columnAL.Count;

            GroupHeaderBand gh = new GroupHeaderBand();
            gh.GroupFields.Add(new GroupField("col5", XRColumnSortOrder.Ascending));

            XRLabel l = new XRLabel();
            l.Font = new System.Drawing.Font(fontName, 10);
            l.DataBindings.Add("Text", report.DataSource, "col5");
            gh.Controls.Add(l);
            gh.HeightF = 12;
            gh.BackColor = System.Drawing.Color.LightBlue;
            report.Bands.Add(gh);


            GroupFooterBand gf = new GroupFooterBand();
            XRLabel l2 = new XRLabel();
            l2.Font = new Font(fontName,10);
            gf.Controls.Add(l2);
            gf.HeightF = 12;
            gf.BackColor = System.Drawing.Color.LightGreen;
            report.Bands.Add(gf);

            report.Bands.Add(new ReportHeaderBand());
            report.Bands.Add(new PageHeaderBand());
            report.Bands.Add(new DetailBand());
            report.Bands.Add(new ReportFooterBand());

            XRLabel title = new XRLabel();

            title.Font = new System.Drawing.Font(fontName, 12, FontStyle.Regular, GraphicsUnit.Point, 136 );

            title.LocationFloat = new DevExpress.Utils.PointFloat(148.6667F, 25.50001F);
            title.Multiline = true;
            title.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            title.SizeF = new System.Drawing.SizeF(501.3333F, 23F);
            title.TextAlignment = TextAlignment.MiddleCenter;
//            title.Text = "測試公司 Title";

            title.Text = $"測試公司 Title + FontName: {title.Font.Name} + OriginalFontName: {title.Font.OriginalFontName}";

            report.Bands[BandKind.ReportHeader].Controls.Add(title);
            report.Bands[BandKind.ReportHeader].HeightF = 20;

            report.Bands[BandKind.PageHeader].Controls.Add(table1);
            report.Bands[BandKind.PageHeader].HeightF = 12;
            report.Bands[BandKind.Detail].Controls.Add(table2);
            report.Bands[BandKind.Detail].HeightF = 12;

            //report.Bands[BandKind.TopMargin].HeightF = 10;
            report.Margins = new System.Drawing.Printing.Margins(20, 20, 20, 20);

            Stream stream = new System.IO.MemoryStream();
            report.ExportToPdf(stream);
            stream.Position = 0;
            return stream;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            Test();
            return new string[] { "value1", "value2" };
        }

        // GET api/Report/PDF
        [HttpGet("{filetype}")]
        public ActionResult<Stream> Get(string filetype)
        {
            switch (filetype)
            {
                case "Black": return File(Test(), "application/pdf");
                case "Light": return File(Test("華康細黑體"), "application/pdf");
                case "Medium": return File(Test("華康中黑體"), "application/pdf");
                case "Noto": return File(Test("Noto Sans CJK TC"), "application/pdf");
                default: return null;
            }
        }
    }


}