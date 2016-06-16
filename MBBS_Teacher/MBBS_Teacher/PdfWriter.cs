using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MBBS_Teacher
{

    class PdfWriter
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
        
        public PdfWriter(Data data)
        {
            this.data = data;
            
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
                for (int i = splittedString.Length - 1; i > 0; i--)
                {
                    totalString = "";
                    for (int x = 0; x < i; x++)
                    {
                        totalString += splittedString[x];
                    }
                    removedPart = splittedString[i] + removedPart;
                    if (myX + ((totalString.Count(char.IsWhiteSpace) / 12) * 11 + 50) < Height)
                    {

                        rect2 = new XRect(myX, myY + 50, someWidth, ((totalString.Count(char.IsWhiteSpace) / 12) * 11 + 50));
                        tf.DrawString(totalString, font2, XBrushes.Black, rect2, XStringFormats.TopLeft);

                        pdfPage = pdf.AddPage();
                        graph = XGraphics.FromPdfPage(pdfPage);
                        tf = new XTextFormatter(graph);
                        myY = 50;

                        rect2 = new XRect(myX, myY, someWidth, ((removedPart.Count(char.IsWhiteSpace) / 12) * 11 + 50));
                        tf.DrawString(removedPart, font2, XBrushes.Black, rect2, XStringFormats.TopLeft);
                        this.myY += ((removedPart.Count(char.IsWhiteSpace) / 12) * 11 + 50);
                        totalString = "";
                        removedPart = "";
                        heightIsUpdated = true;
                        i = -1;
                    }

                }

            }
        }

        public async Task drawPdf()
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
            getTitlePage();
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
                    newHeight += (Regex.Matches(value, @"\r\n").Count * 5);

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
            try
            {
                pdf.Save(pdfFilename);
            }
            catch
            {
                Console.WriteLine("already in use");
            }
            Process.Start(pdfFilename);
        }

        private void getTitlePage()
        {
            JsonReader reader = new JsonTextReader(new StringReader(WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/GetModuleInfo?moduleID=" + data.ModuleName,data.Token)));
            string text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/GetModuleInfo?moduleID=" + data.ModuleName,data.Token);
            while (reader.Read())
            {
                Console.WriteLine("Token {0} , Value {1}", reader.TokenType, reader.Value);
            }
            Console.WriteLine(text);
        }
    }
}
