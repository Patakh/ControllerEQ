using QE.Models.DTO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.Models
{
    public partial class Main
    {
        public void ShelduesPages(Grid panel)
        {
            GraficaSheldues(panel, _schedulesDto);
        }

        private void GraficaSheldues(Grid panel, List<SchedulesDto> schedules)
        {
            panel.Children.Clear();

            WrapPanel stackPanelForm = new WrapPanel();
            stackPanelForm.Orientation = Orientation.Vertical;
            stackPanelForm.HorizontalAlignment = HorizontalAlignment.Center;

            TextBlock textSchedulesHead = new TextBlock();
            textSchedulesHead.FontFamily = new FontFamily("Area");
            textSchedulesHead.FontSize = 30;
            textSchedulesHead.HorizontalAlignment = HorizontalAlignment.Center;
            textSchedulesHead.Margin = new Thickness(0, 0, 0, 20);
            textSchedulesHead.Foreground = new SolidColorBrush(_colorDto.ColorTextSheldue);
            textSchedulesHead.FontWeight = FontWeights.Bold;
            textSchedulesHead.Text = "Режим работы";

            stackPanelForm.Children.Add(textSchedulesHead);

            schedules.ForEach(schedule =>
            {

                TextBlock textBlockDayWeek = new TextBlock();
                textBlockDayWeek.FontFamily = new FontFamily("Area");
                textBlockDayWeek.FontSize = 25;
                textBlockDayWeek.HorizontalAlignment = HorizontalAlignment.Left;
                textBlockDayWeek.Foreground = new SolidColorBrush(_colorDto.ColorTextSheldue);
                textBlockDayWeek.Text = schedule.SDayWeekName;
                textBlockDayWeek.Width = 250;

                TextBlock textBlockTime = new TextBlock();
                textBlockTime.FontFamily = new FontFamily("Area");
                textBlockTime.FontSize = 25;
                textBlockTime.HorizontalAlignment = HorizontalAlignment.Right;
                textBlockTime.Foreground = new SolidColorBrush(_colorDto.ColorTextSheldue);
                textBlockTime.Text = schedule.StartTime + " - " + schedule.StopTime;

                StackPanel stackPanelSchedules = new StackPanel();
                stackPanelSchedules.Orientation = Orientation.Horizontal;
                stackPanelSchedules.HorizontalAlignment = HorizontalAlignment.Center;
                stackPanelSchedules.Children.Add(textBlockDayWeek);
                stackPanelSchedules.Children.Add(textBlockTime);

                stackPanelForm.Children.Add(stackPanelSchedules);

            });

            panel.Children.Add(stackPanelForm);
        }
    }
}
