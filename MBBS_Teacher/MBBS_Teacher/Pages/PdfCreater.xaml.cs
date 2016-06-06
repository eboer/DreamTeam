using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
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
        double myY = 50;
        double someWidth = 500;
        double someHeight = 50;
        

        Data data;
        PdfDocument pdf;
        XGraphics graph;
        XFont font;

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
            Boolean justDoIt = true;

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
                newHeight += (stringSpaceLength / 12) * 11 + 50;

                XRect rect = new XRect(myX, myY, someWidth, someHeight);
                XRect rect2 = new XRect(myX, myY + 50, someWidth, newHeight);

                string splitString = value;
                int middlePos = 0;
                if (splitString.Count(Char.IsWhiteSpace) > 500)
                {
                    middlePos = splitString.Length / 2;
                    splitString = splitString.Substring(0, middlePos);
                    myY = 50;
                    justDoIt = false;
                    //Section section = new Section();
                    //Paragraph paragraph = section.AddParagraph();
                    //paragraph.AddFormattedText(splitString);
                    tf.DrawString(splitString, font, XBrushes.Black, rect);
                    pdfPage = pdf.AddPage();
                    graph = XGraphics.FromPdfPage(pdfPage);
                    tf = new XTextFormatter(graph);
                }

                if (pdfPage.Height < myY + newHeight && justDoIt == true)
                {
                    myY = 50;
                    pdfPage = pdf.AddPage();
                    graph = XGraphics.FromPdfPage(pdfPage);
                    tf = new XTextFormatter(graph);
                }    

                tf.DrawString(k.Key, font, XBrushes.Black, rect, XStringFormats.TopLeft);
                tf.DrawString(value, font2, XBrushes.Black, rect2, XStringFormats.TopLeft);

                tf.Alignment = XParagraphAlignment.Left;

                if (pdfPage.Height > myY + newHeight)
                {
                    myY += newHeight;
                }
                justDoIt = true;
            }
          
            string pdfFilename = "firstpage.pdf";
            pdf.Save(pdfFilename);
            Process.Start(pdfFilename);
        }

        //public void drawText(double myY, XRect rect)
        //{
            
        //}
    }
}
;