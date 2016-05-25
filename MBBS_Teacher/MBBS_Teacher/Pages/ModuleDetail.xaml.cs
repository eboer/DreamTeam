using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for ModuleDetail.xaml
    /// </summary>
    public partial class ModuleDetail : UserControl, ISwitchable
    {
        Data data;
        public ModuleDetail()
        {
            InitializeComponent();
            this.SettingsList.Items.Add(new ListViewItem { Content = "Download module" });
            this.SettingsList.Items.Add(new ListViewItem { Content = "Check review" });

            this.EditView.Items.Add(new ListViewItem { Content = "Voorwoord" });
            this.EditView.Items.Add(new ListViewItem { Content = "Introductie" });
            this.EditView.Items.Add(new ListViewItem { Content = "Toetsing" });
            this.EditView.Items.Add(new ListViewItem { Content = "Programma" });
            this.EditView.Items.Add(new ListViewItem { Content = "Structuur & Organisatie" });
            this.EditView.Items.Add(new ListViewItem { Content = "Literatuur/Programmatuur" });
            this.EditView.Items.Add(new ListViewItem { Content = "Module Evaluatie" });
            this.EditView.Items.Add(new ListViewItem { Content = "Bijlagen" });
        }

        public void UtilizeState(object state)
        {
            data = (Data)state;
            this.header.Content = data.ModuleName;
        }
        private void moduleList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                data.ModuleChange = item.Content.ToString();
                Switcher.Switch(new ModuleChange(), data);
            }
        }

        private void moduleListDetail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                data.ModuleChange = item.Content.ToString();
                Switcher.Switch(new LineChart(), data);
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Modulelist(),data);
        }
    }
}
