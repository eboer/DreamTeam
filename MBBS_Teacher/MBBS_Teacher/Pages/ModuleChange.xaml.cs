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
    /// Interaction logic for ModuleChange.xaml
    /// </summary>
    public partial class ModuleChange : UserControl, ISwitchable
    {
        Data data;
        public ModuleChange()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            data = (Data)state;
            this.header.Content = data.ModuleName;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Modulelist(), data);
        }
    }
}
