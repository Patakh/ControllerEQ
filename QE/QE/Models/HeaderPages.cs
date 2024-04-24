using QE.ViewModel;
using System;
using System.Windows.Controls;

namespace QE.Models
{
    public partial class Main
    {
        public void HeaderOfficesPages(StackPanel panel)
        {
            panel.Children.Clear();
            panel.Children.Add(new HeaderOfficeText(_colorDto, _terminalDto.Office.Name));
        }
        public void HeaderDatePages(StackPanel panel, DateTime? date=null)
        {
            panel.Children.Clear();
            panel.Children.Add(new HeaderDateText(_colorDto, date ?? DateTime.Now));
        }
    }
}
