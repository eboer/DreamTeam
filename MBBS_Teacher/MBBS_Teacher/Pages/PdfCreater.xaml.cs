using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        Boolean heightIsUpdated = false;
        int entryNumber = 1;
        

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
            popUp.IsOpen = true;
        }

        public async void UtilizeState(object state)
        {
            data = (Data)state;
            popUp.IsOpen = true;
            Task t = Task.Run(() =>
            {
                drawPdf();
            });
            await Task.WhenAll(t);
            Switcher.Switch(new ModuleDetail(), data);
        }

        private List<Module> getModuleData()
        { 
            string text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/DocentModules", this.data.Token);
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
                string totalString = "";
                string removedPart = "";
                string[] splittedString = content.Split('.');
                for(int i = splittedString.Length - 1; i > 0; i--)
                {
                    totalString = "";
                    for(int x = 0; x < i; x++)
                    {
                        totalString += splittedString[x];
                    }
                    removedPart = splittedString[i] + removedPart;
                    if(myX + ((totalString.Count(char.IsWhiteSpace)/12) * 11 + 50) < Height)
                    {

                        rect2 = new XRect(myX, myY + 50, someWidth, ((totalString.Count(char.IsWhiteSpace) / 12) * 11 + 50));
                        tf.DrawString(totalString, font2, XBrushes.Black, rect2, XStringFormats.TopLeft);

                        pdfPage = pdf.AddPage();
                        graph = XGraphics.FromPdfPage(pdfPage);
                        tf = new XTextFormatter(graph);
                        myY = 50;
          
                        rect2 = new XRect(myX, myY, someWidth, ((removedPart.Count(char.IsWhiteSpace) / 12) * 11 + 50));
                        tf.DrawString(removedPart, font2, XBrushes.Black, rect2, XStringFormats.TopLeft);
                        Console.WriteLine(removedPart);
                        this.myY += ((removedPart.Count(char.IsWhiteSpace) / 12) * 11 + 50);
                        Console.WriteLine(myY.ToString());
                        totalString = "";
                        removedPart = "";
                        heightIsUpdated = true;
                        i = -1;
                    }

                }
                
            }
        }

        public async void drawPdf()
        {
            // string text = null;
            List<Module> modules = new List<Module>();
            Task t = Task.Run(() =>
            {
                modules = getModuleData();
            });
            await Task.WhenAll(t);

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

            Dictionary<string, string> p = Module.getModuleData(data.Token, data.Lang, data.ModuleName);

            foreach (KeyValuePair<string, string> k in p)
            {
                double newHeight = someHeight;
                string value = "";
                if (k.Value != null)
                {
                    value = k.Value;



                    double stringSpaceLength = value.Count(Char.IsWhiteSpace);
                    newHeight += (stringSpaceLength / 12) * 11 + 50;

                    if (pdfPage.Height < myY + newHeight && entryNumber != 1)
                    {
                        myY = 50;
                        pdfPage = pdf.AddPage();
                        graph = XGraphics.FromPdfPage(pdfPage);
                        tf = new XTextFormatter(graph);
                    }

                    drawSomething(myY, pdfPage.Height, newHeight, k.Key, value);
                    if (!heightIsUpdated)
                    {
                        myY += newHeight;
                    }
                    else
                    {
                        heightIsUpdated = false;
                    }
                    entryNumber++;
                }
            }

            string pdfFilename = data.ModuleName + ".pdf";
            pdf.Save(pdfFilename);
            Process.Start(pdfFilename);
        }
    }
}
;