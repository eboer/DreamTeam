using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MBBS_Teacher.Pages
{
    /// <summary>
    /// Interaction logic for PdfCreater.xaml
    /// </summary>
    public partial class PdfCreater : UserControl, ISwitchable
    {
        Data data;

        public PdfCreater()
        {    
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            data = (Data)state;
            string text = null;

            List<Module> modules = new List<Module>();
            Task t = Task.Factory.StartNew(() =>
            {
                text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/DocentModules", this.data.token);
                modules = JsonConvert.DeserializeObject<List<Module>>(text);
            });
            Task.WaitAll(t);

            PdfDocument pdf = new PdfDocument();
            pdf.Info.Title = "The first PDF document";
            PdfPage pdfPage = pdf.AddPage();
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);
            XFont font2 = new XFont("Verdana", 10);

            foreach (Module module in modules)
            {   
                graph.DrawString(module.module_name, font, XBrushes.Black, new XRect(0, 0, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                graph.DrawString("Dit is starcraft 2 voor beginners. Jullie zijn allemaal noobs en daarom hebben jullie dit nodig", font2, XBrushes.Black, new XRect(0, 0, pdfPage.Width.Point, pdfPage.Height.Point));
            }

            string pdfFilename = "firstpage.pdf";
            pdf.Save(pdfFilename);
            Process.Start(pdfFilename);
        }
    }
}
