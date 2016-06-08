using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

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
            string text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Survey/AverageRatingPerYear?moduleID=" + data.ModuleName, data.token);
           
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
            string text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Survey/AverageRatingSubsections?moduleID=" + data.ModuleName, data.token);
            Dictionary<string, double> results = JsonConvert.DeserializeObject<Dictionary<string, double>>(text);
         //   var myKey = results.FirstOrDefault(x => x.Value == "one").Key;
            Console.WriteLine(text);
            ((BarSeries)ScoreChart.Series[0]).ItemsSource = results;
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
