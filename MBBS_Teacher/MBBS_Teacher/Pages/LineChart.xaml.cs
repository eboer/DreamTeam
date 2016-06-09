﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Input;

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

       

        public void LoadScoreChart()
        {
            string text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Survey/AverageRatingSubsections?moduleID=" + data.ModuleName, data.token);
            Dictionary<string, double> results = JsonConvert.DeserializeObject<Dictionary<string, double>>(text);

            ((BarSeries)ScoreChart.Series[0]).ItemsSource = results;
        }

        public void UtilizeState(object state)
        {
            data = (Data)state;
            this.header.Content = data.ModuleName;
            fillChart();
            
            LoadScoreChart();

            LoadComments();
        }

        private void LoadComments()
        {
            string text = "";
            try
            {
                text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Survey/GetComments?moduleID=" + data.ModuleName + "&languageID=en", data.token);
            }
            catch
            {
                Task t = Task.Run(async () =>
                {
                    await Task.Delay(20);
                });
                Task.WaitAll(t);
                LoadComments();
            }
            List<Comments> commentsList = JsonConvert.DeserializeObject<List<Comments>>(text);
            commentList.ItemsSource = commentsList;
            Console.WriteLine(text);
        }
            
        private void back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ModuleDetail(), data);
        }
        private void commentList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            Comments content = (Comments)item.Content;
            Console.WriteLine("pressed");
            
            questionText.AppendText(content.Comment);
            Console.WriteLine("appended");

        }
    }

    internal class Comments
    {
        [JsonProperty("QuestionID")]
        public string QuestionID { get; set; }

        [JsonProperty("QuestionText")]
        public string QuestionText { get; set; }

        [JsonProperty("SubsectionID")]
        public string SubsectionID { get; set; }

        [JsonProperty("SubsectionName")]
        public string SubsectionName { get; set; }

        [JsonProperty("DateCompleted")]
        public string DateCompleted { get; set; }

        [JsonProperty("Comment")]
        public string Comment { get; set; }

    }
}
