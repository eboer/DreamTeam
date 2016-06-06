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
using System.Net;
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
        XGraphics graph;
        PdfPage pdfPage;
        XFont font;
        XFont font2;
        PdfDocument pdf = new PdfDocument();
        XTextFormatter tf;

        public PdfCreater()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            data = (Data)state;

            drawPdf();
        }

        private List<Module> getModuleData()
        { 
            string text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/DocentModules", this.data.token);
            return JsonConvert.DeserializeObject<List<Module>>(text);
        }

        public void drawSomething(double myY, XUnit Height, double newHeight, string title, string content)
        {
            XRect rect = new XRect(myX, myY, someWidth, someHeight);
            XRect rect2 = new XRect(myX, myY + 50, someWidth, newHeight);
            tf.DrawString(title, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            double contentSpaceLength = content.Count(Char.IsWhiteSpace);
            double contentHeight = (contentSpaceLength / 12) * 11 + 50;
            if (myY + contentHeight < Height)
            {
                tf.DrawString(content, font2, XBrushes.Black, rect2, XStringFormats.TopLeft);
            }
            else
            {
                string[] splittedString = content.Split('.');
                foreach(string s in splittedString)
                {
                    double sSpaceLength = s.Count(Char.IsWhiteSpace);
                    newHeight = (sSpaceLength / 12) * 11 + 30;
                    if(myX + newHeight > Height)
                    {
                        pdfPage = pdf.AddPage();
                        graph = XGraphics.FromPdfPage(pdfPage);
                        tf = new XTextFormatter(graph);
                        myY = 0;
                    }
                    rect2 = new XRect(myX, myY + 50, someWidth, newHeight);
                    tf.DrawString(s, font2, XBrushes.Black, rect2, XStringFormats.TopLeft);
                    
                    myY += newHeight;

                }
            }
        }

        public void drawPdf()
        { 
           // string text = null;
            List<Module> modules = new List<Module>();
            Task t = Task.Run(() =>
            {
               modules = getModuleData();
            });
            Task.WhenAll(t);

            pdf.Info.Title = "The first PDF document";
            pdfPage = pdf.AddPage();
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            
            font = new XFont("Verdana", 15, XFontStyle.Bold);
            font2 = new XFont("Verdana", 10);
            XStringFormat format = new XStringFormat();
            tf = new XTextFormatter(graph);
            

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

                var stringSize = graph.Graphics.MeasureString(value, new System.Drawing.Font("verdana", 10));
                double stringSpaceLength = value.Count(Char.IsWhiteSpace);
                newHeight += (stringSpaceLength / 12) * 11 + 50;

                if (pdfPage.Height < myY + newHeight)
                {
                    myY = 50;
                    pdfPage = pdf.AddPage();
                    graph = XGraphics.FromPdfPage(pdfPage);
                    tf = new XTextFormatter(graph);
                }
                drawSomething(myY,pdfPage.Height,newHeight,k.Key,value);
                 myY += newHeight;
            }

            string pdfFilename = "firstpage.pdf";
            pdf.Save(pdfFilename);
            Process.Start(pdfFilename);
        }
    }
}
;