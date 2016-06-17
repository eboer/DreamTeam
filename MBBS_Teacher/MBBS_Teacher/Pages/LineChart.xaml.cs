using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Documents;
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
        public async void fillChart()
        {
            //try to do the webrequest, if it fails wait 10 seconds and try again
            try
            {
                string text = null;
                //do the webrequest in a task so that the UI doesn't freeze
                Task t = Task.Run(() =>
                {
                    text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Survey/AverageRatingPerYear?moduleID=" + data.ModuleName, data.Token);
                });
                await Task.WhenAll(t);
                //add the new data to the chart in reverse order
                Dictionary<string, double> values = JsonConvert.DeserializeObject<Dictionary<string, double>>(text);
                ((LineSeries)GradeChart.Series[0]).ItemsSource =
                values.Reverse();
            }
            catch
            {
                // wait 10  sec
                Task t = Task.Run(async () =>
                {
                    await Task.Delay(10000);
                });
                await Task.WhenAll(t);
                fillChart();
            }


        }


        //load the score
        public async void LoadScoreChart()
        {
            //try to do the webrequest, if it fails wait 10 seconds and try again
            try
            {
                string text = null;
                //make the webrequest run in a task so the UI doesn't freeze
                Task t = Task.Run(() =>
                {
                    text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Survey/AverageRatingSubsections?moduleID=" + data.ModuleName, data.Token);
                });
                await Task.WhenAll(t);
                Dictionary<string, double> results = JsonConvert.DeserializeObject<Dictionary<string, double>>(text);

                ((BarSeries)ScoreChart.Series[0]).ItemsSource = results;
            }
            //retry after 10 sec
            catch
            {
                Task t = Task.Run(async () =>
                {
                    await Task.Delay(10000);
                });
                await Task.WhenAll(t);
                LoadScoreChart();
            }
        }
        //utilize state, this method is used for recieving data from another page
        public void UtilizeState(object state)
        {
            //data class containing the prev page info
            data = (Data)state;
            this.header.Content = data.ModuleName;

            //fill the charts
            fillChart();
            LoadScoreChart();
            LoadComments();
        }

        private void LoadComments()
        {
            string text = "";
            try
            {
                text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Survey/GetComments?moduleID=" + data.ModuleName + "&languageID=" + data.Lang, data.Token);
            }
            catch
            {
                Task t = Task.Run(async () =>
                {
                    await Task.Delay(10000);
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
            TextRange txt = new TextRange(questionText.Document.ContentStart, questionText.Document.ContentEnd);
            txt.Text = "";
            var item = sender as ListViewItem;
            Comments content = (Comments)item.Content;            
            questionText.AppendText(content.Comment);
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
