using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MBBS_Teacher.Pages
{
    /// <summary>
    /// Interaction logic for LineChart.xaml
    /// </summary>
    public partial class LineChart : UserControl, ISwitchable
    {
        Data data;
        public LineChart()
        {
            InitializeComponent();

        }

        //fills the chart with data
        public void fillChart()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://mbbsweb.azurewebsites.net/api/Survey/AverageRatingPerYear?moduleID=" + data.ModuleName);
            request.Headers.Add("Authorization", data.token);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            string text = reader.ReadToEnd();
            Console.WriteLine(text);
            Dictionary<string, double> values = JsonConvert.DeserializeObject<Dictionary<string, double>>(text);
            ((LineSeries)GradeChart.Series[0]).ItemsSource =
            values;

        }

        public void LoadPieChartData()
        {
            ((PieSeries)PieChart.Series [0]).ItemsSource =
            new KeyValuePair<String, int>[]{
            new KeyValuePair<String, int>("First Exam", 7),
            new KeyValuePair<String, int>("Re-exam", 6),
            new KeyValuePair<String, int>("Failed", 4) };
        }

        public void LoadScoreChart()
        {
            ((BarSeries)ScoreChart.Series[0]).ItemsSource =
            new KeyValuePair<String, int>[]{
            new KeyValuePair<String, int>("Introductie", 7),
            new KeyValuePair<String, int>("Toetsing", 6),
            new KeyValuePair<String, int>("Programma", 4),
            new KeyValuePair<String, int>("Structuur & Organisatie", 3),
            new KeyValuePair<String, int>("Literatuur/Programmatuur", 8),
            new KeyValuePair<String, int>("Module Evaluatie", 9),
            new KeyValuePair<String, int>("Bijlagen", 4)};
        }

        public void UtilizeState(object state)
        {
            data = (Data)state;
            this.header.Content = data.ModuleName;
            fillChart();
            LoadPieChartData();
            LoadScoreChart();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ModuleDetail(), data);
        }
    }
}
