using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MBBS
{
    public partial class FormulierPage : ContentPage
    {
        private string moduleName;
        private string moduleID;
        public FormulierPage(string token, string moduleID, string moduleName)
        {
            InitializeComponent();
            //TestLabel.Text = moduleName;
            this.Title = moduleName;
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
               
        }
    }
}
