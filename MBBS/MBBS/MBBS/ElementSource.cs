using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MBBS
{
    [ContentProperty("ElementName")]
    class ElementSource : IMarkupExtension
    {
        public string ElementName { get; set; }
        //provide elementsource
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            //check if element exists
            var rootProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            if (rootProvider == null)
                return null;
            var root = rootProvider.RootObject as Element;
            if (root == null)
                return null;
            //return element object if found
            return root.FindByName<Element>(ElementName);
        }
    }
}
