using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GraphalyzerPro.Views
{
    public class SessionTabControlDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ItemTemplate { get; set; }

        public DataTemplate NewButtonTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate result;
            if (item == CollectionView.NewItemPlaceholder)
            {
                result = NewButtonTemplate;
            }
            else
            {
                result = ItemTemplate;
            }
            return result;
        }
    }
}
