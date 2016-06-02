using MigraDoc.DocumentObjectModel;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MBBS_Teacher.Pages
{
    /// <summary>
    /// Interaction logic for PdfCreater.xaml
    /// </summary>
    public partial class PdfCreater : UserControl, ISwitchable
    {
        double myX = 50;
        double myY = 0;
        double someWidth = 500;
        double someHeight = 50;

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
            XFont font = new XFont("Verdana", 15, XFontStyle.Bold);
            XFont font2 = new XFont("Verdana", 10);
            XStringFormat format = new XStringFormat();
            XTextFormatter tf = new XTextFormatter(graph);

            pdfPage.Orientation = PdfSharp.PageOrientation.Portrait;
            pdfPage.Width = XUnit.FromInch(8.5);
            pdfPage.Height = XUnit.FromInch(11);

            Dictionary<string, string> p = Module.getModuleData(data.token, "SCII");

            foreach (KeyValuePair<string, string> k in p)
            {
                double newHeight = someHeight;
                string value = "";
                if (k.Value != null)
                {
                    value = k.Value;
                }

                double stringSpaceLength = value.Count(Char.IsWhiteSpace);
                newHeight += (stringSpaceLength / 12) * 15;

                XRect rect = new XRect(myX, myY, someWidth, newHeight);
                XRect rect2 = new XRect(myX, myY += 50, someWidth, newHeight);

                tf.DrawString(k.Key, font, XBrushes.Black, rect, XStringFormats.TopLeft);
                this.myY = myY += newHeight;
                tf.DrawString(value, font2, XBrushes.Black, rect2, XStringFormats.TopLeft);
                tf.Alignment = XParagraphAlignment.Left;
            }

            string pdfFilename = "firstpage.pdf";
            pdf.Save(pdfFilename);
            Process.Start(pdfFilename);
        }
    }
}
