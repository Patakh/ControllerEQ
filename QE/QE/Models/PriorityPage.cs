using Microsoft.EntityFrameworkCore;
using QE.Models.DTO;
using QE.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.Models
{
    public partial class Main
    {
        public async Task PriorityPages(Grid panel)
        {
            panel.Children.Clear();

            var prioritiButtons = await _context.SPriorities
                .AsNoTracking()
                .OrderByDescending(o => o.PriorityPosition)
                .Select(s => new PrioritesDto
                {
                    Id = s.Id,
                    SortId = s.PriorityPosition,
                    Name = s.PriorityName,
                    Comment = s.Commentt,
                }).ToListAsync();

            if (prioritiButtons == null || prioritiButtons.Count == 0)
            {
                panel.Children.Add(new PanelEmpty());
            }
            else
            {
                await GraficaPriority(panel, prioritiButtons);
            }
        }

        private async Task GraficaPriority(Grid panel, List<PrioritesDto> prioritesBtn)
        {
            var grid = new Grid();

            var settingButtons = await GetTerminalButtonSettings();
            var idxRow = 0;
            var idxCol = 0;

            if (prioritesBtn.Count > 0)
            {
                for (int i = 0; i < settingButtons.RowCount; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition { MaxHeight = 300 });
                }

                for (int i = 0; i < settingButtons.ColCount; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { MaxWidth = 600 });
                }
                prioritesBtn.ForEach(f =>
                {
                    if (idxCol == settingButtons.ColCount)
                    {
                        idxCol = 0;
                        idxRow++;
                    }

                    ButtonPriority btnPriority = new ButtonPriority(_colorDto, f.Name);
                    btnPriority.Click += async (s, e) =>
                    {
                        await InitButtons(panel, headers: new List<string> { f.Name }, priorityId: f.Id);
                    };
                    Grid.SetRow(btnPriority, idxRow);
                    Grid.SetColumn(btnPriority, idxCol);
                    grid.Children.Add(btnPriority);

                    idxCol++;
                });
            }
            else
            {
                grid.RowDefinitions.Add(new RowDefinition { });
                var childButtonsEmpty = new TextBlock();
                childButtonsEmpty.FontSize = 40;
                childButtonsEmpty.Foreground = new SolidColorBrush(Colors.LightGray);
                childButtonsEmpty.Text = "Нет данных";
                childButtonsEmpty.HorizontalAlignment = HorizontalAlignment.Center;
                childButtonsEmpty.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(childButtonsEmpty, idxRow);
                Grid.SetColumn(childButtonsEmpty, idxCol);
                Grid.SetColumnSpan(childButtonsEmpty, (int)settingButtons.ColCount);
                grid.Children.Add(childButtonsEmpty);

            }

            Grid.SetRow(grid, 0);
            Grid.SetColumn(grid, 0);
            panel.Children.Add(grid);
        }

        //public async Task PriorityButtons(Grid panel)
        //{
        //    WrapPanel wrapPanelPreferentialСategoryСitizens = new WrapPanel();
        //    wrapPanelPreferentialСategoryСitizens.Orientation = Orientation.Vertical;
        //    wrapPanelPreferentialСategoryСitizens.VerticalAlignment = VerticalAlignment.Top;
        //    wrapPanelPreferentialСategoryСitizens.Visibility = Visibility.Collapsed;
        //    wrapPanelPreferentialСategoryСitizens.HorizontalAlignment = HorizontalAlignment.Center;
        //    wrapPanelPreferentialСategoryСitizens.Name = "PreferentialСategoryСitizens";


        //    this.Button_Click_PreferentialСategoryСitizens.Click += (s, e) =>
        //    {
        //        PreferentialСategoryСitizens();
        //        void PreferentialСategoryСitizens()
        //        {

        //            //кнопка приоритет
        //            //WrapPanel wrapPanelPriooritetButons = new WrapPanel();
        //            //wrapPanelPriooritetButons.Orientation = Orientation.Horizontal;
        //            //wrapPanelPriooritetButons.VerticalAlignment = VerticalAlignment.Center;
        //            //wrapPanelPriooritetButons.HorizontalAlignment = HorizontalAlignment.Center;

        //            ////меню
        //            //WrapPanel wrapPanelPriooritetMenu = new WrapPanel();
        //            //wrapPanelPriooritetMenu.Orientation = Orientation.Horizontal;
        //            //wrapPanelPriooritetMenu.VerticalAlignment = VerticalAlignment.Center;
        //            //wrapPanelPriooritetMenu.HorizontalAlignment = HorizontalAlignment.Center;

        //            ////кнопки меню
        //            //WrapPanel wrapPanelPriooritetMenuButons = new WrapPanel();
        //            //wrapPanelPriooritetMenuButons.Orientation = Orientation.Horizontal;
        //            //wrapPanelPriooritetMenuButons.VerticalAlignment = VerticalAlignment.Center;
        //            //wrapPanelPriooritetMenuButons.HorizontalAlignment = HorizontalAlignment.Center;

        //            //if (wrapPanelPreferentialСategoryСitizens.Children.Count > 0) wrapPanelPreferentialСategoryСitizens.Children.Clear();
        //            //TextBlock textBlockPreferentialСategoryСitizens = new TextBlock();
        //            //textBlockPreferentialСategoryСitizens.FontFamily = new FontFamily("Area");
        //            //textBlockPreferentialСategoryСitizens.FontSize = 40;
        //            //textBlockPreferentialСategoryСitizens.HorizontalAlignment = HorizontalAlignment.Center;
        //            //textBlockPreferentialСategoryСitizens.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
        //            //textBlockPreferentialСategoryСitizens.TextWrapping = TextWrapping.Wrap;
        //            //textBlockPreferentialСategoryСitizens.Text = "Льготная категория граждан";
        //            //WrapPanel wrapPanelHeadPreferentialСategoryСitizens = new WrapPanel();
        //            //wrapPanelHeadPreferentialСategoryСitizens.Orientation = Orientation.Vertical;
        //            //wrapPanelHeadPreferentialСategoryСitizens.VerticalAlignment = VerticalAlignment.Top;
        //            //wrapPanelHeadPreferentialСategoryСitizens.Children.Add(textBlockPreferentialСategoryСitizens);
        //            //wrapPanelPreferentialСategoryСitizens.Children.Add(wrapPanelHeadPreferentialСategoryСitizens);

        //            #region Кнопка далее и назад
        //            Button btnBack = new Button();
        //            DropShadowEffect shadowEffectBack = new DropShadowEffect();
        //            shadowEffectBack.Color = Colors.White;
        //            shadowEffectBack.ShadowDepth = 3;
        //            btnBack.Effect = shadowEffectBack;
        //            btnBack.Name = "Back";
        //            btnBack.Content = "Назад";
        //            btnBack.HorizontalAlignment = HorizontalAlignment.Left;
        //            btnBack.VerticalAlignment = VerticalAlignment.Bottom;
        //            btnBack.Height = 75;
        //            btnBack.Width = 200;
        //            btnBack.Background = new SolidColorBrush(Colors.DimGray);
        //            btnBack.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
        //            btnBack.FontFamily = new FontFamily("Area");
        //            btnBack.FontSize = 25;
        //            btnBack.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        //            btnBack.TabIndex = 999;
        //            btnBack.Visibility = Visibility.Hidden;
        //            ControlTemplate myControlTemplateBack = new ControlTemplate(typeof(Button));
        //            FrameworkElementFactory borderBack = new FrameworkElementFactory(typeof(Border));
        //            borderBack.Name = "border";
        //            borderBack.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
        //            borderBack.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
        //            borderBack.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
        //            borderBack.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
        //            FrameworkElementFactory contentPresenterBack = new FrameworkElementFactory(typeof(ContentPresenter));
        //            contentPresenterBack.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        //            contentPresenterBack.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
        //            borderBack.AppendChild(contentPresenterBack);
        //            myControlTemplateBack.VisualTree = borderBack;
        //            btnBack.Template = myControlTemplateBack;

        //            Button btnNextStage = new Button();
        //            DropShadowEffect shadowEffectNextStage = new DropShadowEffect();
        //            shadowEffectNextStage.Color = Colors.White;
        //            shadowEffectNextStage.ShadowDepth = 3;
        //            btnNextStage.Effect = shadowEffectNextStage;
        //            btnNextStage.Name = "Next";
        //            btnNextStage.Content = "Далее";
        //            btnNextStage.HorizontalAlignment = HorizontalAlignment.Right;
        //            btnNextStage.VerticalAlignment = VerticalAlignment.Bottom;
        //            btnNextStage.Height = 75;
        //            btnNextStage.Width = 200;
        //            btnNextStage.Background = new SolidColorBrush(Colors.DarkGreen);
        //            btnNextStage.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
        //            btnNextStage.FontFamily = new FontFamily("Area");
        //            btnNextStage.FontSize = 25;
        //            btnNextStage.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        //            btnNextStage.TabIndex = 999;
        //            btnNextStage.Visibility = Visibility.Hidden;
        //            ControlTemplate myControlTemplateNextStage = new ControlTemplate(typeof(Button));
        //            FrameworkElementFactory borderNextStage = new FrameworkElementFactory(typeof(Border));
        //            borderNextStage.Name = "border";
        //            borderNextStage.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
        //            borderNextStage.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
        //            borderNextStage.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
        //            borderNextStage.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
        //            FrameworkElementFactory contentPresenterNextStage = new FrameworkElementFactory(typeof(ContentPresenter));
        //            contentPresenterNextStage.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        //            contentPresenterNextStage.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
        //            borderNextStage.AppendChild(contentPresenterNextStage);
        //            myControlTemplateNextStage.VisualTree = borderNextStage;
        //            btnNextStage.Template = myControlTemplateNextStage;
        //            #endregion

        //            //кнопки категории
        //            if (eqContext.SPriorities.Any())
        //            {
        //                eqContext.SPriorities.ToList().ForEach(priooritet =>
        //                {
        //                    TextBlock textBtnPriooritet = new TextBlock();
        //                    textBtnPriooritet.FontFamily = new FontFamily("Area");
        //                    textBtnPriooritet.FontSize = 25;
        //                    textBtnPriooritet.HorizontalAlignment = HorizontalAlignment.Center;
        //                    textBtnPriooritet.Foreground = new SolidColorBrush(Colors.White);
        //                    textBtnPriooritet.TextWrapping = TextWrapping.Wrap;
        //                    textBtnPriooritet.Padding = new Thickness(15);
        //                    textBtnPriooritet.Text = priooritet.PriorityName + "\n" + priooritet.Commentt;

        //                    var btnPriooritet = new ButtonPriority();
        //                    btnPriooritet.Click += (s, e) =>
        //                    {
        //                        btnBack.Visibility = Visibility.Visible;
        //                        btnBack.Click += (s, e) =>
        //                        {
        //                            PreferentialСategoryСitizens();
        //                            textBlockPreferentialСategoryСitizens.Text = "Льготная категория граждан";
        //                        };

        //                        textBlockPreferentialСategoryСitizens.Text = "Льготная категория граждан: " + priooritet.PriorityName;

        //                        wrapPanelPriooritetButons.Visibility = Visibility.Collapsed;

        //                        eqContext.SOfficeTerminalButtons.Where(s => s.SOfficeTerminal.IpAddress == Ip).OrderBy(o => o.ButtonType).ToList().ForEach(b =>
        //                        {
        //                            if (b.ParentId == null) // null - Меню. 2 - Кнопка
        //                            {
        //                                //создаем кнопку перехода на меню
        //                                Button btnMenu = new Button();
        //                                DropShadowEffect shadowEffect = new DropShadowEffect();
        //                                shadowEffect.Color = Colors.White;
        //                                shadowEffect.ShadowDepth = 3;
        //                                btnMenu.Effect = shadowEffect;
        //                                btnMenu.Name = "button";
        //                                btnMenu.Content = b.ButtonName;
        //                                btnMenu.HorizontalAlignment = HorizontalAlignment.Center;
        //                                btnMenu.VerticalAlignment = VerticalAlignment.Top;
        //                                btnMenu.Height = 75;
        //                                btnMenu.Width = 200;
        //                                btnMenu.Margin = new Thickness(0, 18, 32, 0);
        //                                btnMenu.Background = new SolidColorBrush(Colors.DarkRed);
        //                                btnMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
        //                                btnMenu.FontFamily = new FontFamily("Area");
        //                                btnMenu.FontSize = 25;
        //                                btnMenu.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        //                                btnMenu.TabIndex = 999;
        //                                ControlTemplate myControlTemplate = new ControlTemplate(typeof(Button));
        //                                FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
        //                                border.Name = "border";
        //                                border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
        //                                border.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
        //                                border.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
        //                                border.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
        //                                FrameworkElementFactory contentPresenterMenu = new FrameworkElementFactory(typeof(ContentPresenter));
        //                                contentPresenterMenu.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        //                                contentPresenterMenu.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
        //                                border.AppendChild(contentPresenterMenu);
        //                                myControlTemplate.VisualTree = border;
        //                                btnMenu.Template = myControlTemplate;

        //                                //все кнопки этого меню
        //                                var SOfficeTerminalButton = eqContext.SOfficeTerminalButtons.Where(q => q.SOfficeTerminalId == b.SOfficeTerminalId && q.ParentId == b.ParentId);

        //                                //Заголовок меню
        //                                TextBlock textBlockMenu = new TextBlock();
        //                                textBlockMenu.FontFamily = new FontFamily("Area");
        //                                textBlockMenu.FontSize = 60;
        //                                textBlockMenu.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
        //                                textBlockMenu.TextWrapping = TextWrapping.Wrap;
        //                                textBlockMenu.Text = b.ButtonName;

        //                                WrapPanel warpPanelHeadMenu = new WrapPanel();
        //                                warpPanelHeadMenu.Orientation = Orientation.Horizontal;
        //                                warpPanelHeadMenu.VerticalAlignment = VerticalAlignment.Center;
        //                                warpPanelHeadMenu.Visibility = Visibility.Collapsed;
        //                                warpPanelHeadMenu.Margin = new Thickness(25, 0, 0, 0);
        //                                warpPanelHeadMenu.Children.Add(textBlockMenu);
        //                                wrapPanelPriooritetMenu.Children.Add(warpPanelHeadMenu);

        //                                //создаем кнопки меню
        //                                List<SService> sServices = new List<SService>();
        //                                WrapPanel wrapPanel = new WrapPanel();
        //                                wrapPanel.Orientation = Orientation.Horizontal;
        //                                wrapPanel.Visibility = Visibility.Collapsed;
        //                                wrapPanel.MaxWidth = 800;
        //                                SOfficeTerminalButton.ToList().ForEach(button =>
        //                                {
        //                                    int Btn_idx = 1;
        //                                    SService sServices = eqContext.SServices.First(f => f.Id == button.SServiceId);
        //                                    Button btn = new Button();
        //                                    btn.Name = "button" + Btn_idx;
        //                                    btn.Content = button.ButtonName;
        //                                    btn.HorizontalAlignment = HorizontalAlignment.Center;
        //                                    btn.VerticalAlignment = VerticalAlignment.Center;
        //                                    btn.Height = 75;
        //                                    btn.Width = 200;
        //                                    btn.Margin = new Thickness(32, 18, 0, 0);
        //                                    btn.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
        //                                    btn.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
        //                                    btn.FontFamily = new FontFamily("Area");
        //                                    btn.FontSize = 20;
        //                                    btn.Foreground = new SolidColorBrush(Color.FromRgb(135, 98, 27));
        //                                    DropShadowEffect btnShadowEffect = new DropShadowEffect();
        //                                    btnShadowEffect.Color = Color.FromRgb(22, 22, 22);
        //                                    btnShadowEffect.Direction = 50;
        //                                    btnShadowEffect.ShadowDepth = 2;
        //                                    btn.Effect = btnShadowEffect;

        //                                    ControlTemplate myControlTemplate = new ControlTemplate(typeof(Button));
        //                                    FrameworkElementFactory btnBorder = new FrameworkElementFactory(typeof(Border));
        //                                    btnBorder.Name = "border";
        //                                    btnBorder.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
        //                                    btnBorder.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
        //                                    btnBorder.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
        //                                    btnBorder.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
        //                                    FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
        //                                    contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        //                                    contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
        //                                    btnBorder.AppendChild(contentPresenter);
        //                                    myControlTemplate.VisualTree = btnBorder;
        //                                    btn.Template = myControlTemplate;
        //                                    btn.Click += (s, e) =>
        //                                    {
        //                                        SService sServices = eqContext.SServices.First(f => f.Id == button.SServiceId);
        //                                        //Talon.PrintTalon(s, e, sServices, Ip, priorityId: priooritet.Id);
        //                                    };
        //                                    wrapPanel.Children.Add(btn);
        //                                });
        //                                wrapPanelPriooritetMenuButons.Children.Add(wrapPanel);

        //                                btnMenu.Click += (s, e) =>
        //                                {
        //                                    wrapPanel.Visibility = Visibility.Visible;
        //                                    warpPanelHeadMenu.Visibility = Visibility.Visible;
        //                                    wrapPanelPriooritetMenuButons.Visibility = Visibility.Visible;
        //                                    wrapPanelPriooritetMenu.Visibility = Visibility.Collapsed;
        //                                    btnBack.Visibility = Visibility.Visible;

        //                                    btnBack.Click += (s, e) =>
        //                                    {
        //                                        btnBack.Visibility = Visibility.Collapsed;
        //                                        wrapPanel.Visibility = Visibility.Collapsed;
        //                                        warpPanelHeadMenu.Visibility = Visibility.Collapsed;
        //                                        wrapPanelPriooritetMenuButons.Visibility = Visibility.Collapsed;
        //                                        wrapPanelPriooritetMenu.Visibility = Visibility.Visible;
        //                                    };
        //                                };
        //                                wrapPanelPriooritetMenu.Children.Add(btnMenu);
        //                            }
        //                            else
        //                            if (b.ParentId == 1)
        //                            {
        //                                SService sServices = eqContext.SServices.First(f => f.Id == b.SServiceId);
        //                                Button btn = new Button();
        //                                btn.Name = "button" + Btn_idx;
        //                                btn.Content = b.ButtonName;
        //                                btn.HorizontalAlignment = HorizontalAlignment.Center;
        //                                btn.VerticalAlignment = VerticalAlignment.Top;
        //                                btn.Height = 75;
        //                                btn.Width = 200;
        //                                btn.Margin = new Thickness(0, 18, 32, 0);
        //                                btn.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
        //                                btn.BorderBrush = new SolidColorBrush(Color.FromRgb(55, 55, 55));
        //                                btn.FontFamily = new FontFamily("Area");
        //                                btn.FontSize = 25;
        //                                btn.Foreground = new SolidColorBrush(Color.FromRgb(135, 98, 27));
        //                                DropShadowEffect shadowEffect = new DropShadowEffect();
        //                                shadowEffect.Color = Color.FromRgb(22, 22, 22);
        //                                shadowEffect.Direction = 315;
        //                                shadowEffect.ShadowDepth = 3;
        //                                btn.Effect = shadowEffect;

        //                                btn.Click += async (s, e) =>
        //                                {
        //                                    try
        //                                    {
        //                                        EqContext eqContext = new EqContext();
        //                                        FastReport.Report report = new FastReport.Report();
        //                                        var path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))) + "\\FastReport\\Operator.frx";
        //                                        report.Load(path);
        //                                        var LastTicketNumber = eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == Ip && s.SServiceId == sServices.Id && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).OrderByDescending(o => o.TicketNumber).Select(s => s.TicketNumber).FirstOrDefault();

        //                                        DTicket dTicket_New = new DTicket();
        //                                        dTicket_New.SOfficeId = eqContext.SOfficeTerminals.First(s => s.IpAddress == Ip).SOfficeId;
        //                                        dTicket_New.SOfficeTerminalId = eqContext.SOfficeTerminals.First(s => s.IpAddress == Ip).Id;
        //                                        dTicket_New.SServiceId = sServices.Id;
        //                                        dTicket_New.ServicePrefix = sServices.ServicePrefix;
        //                                        dTicket_New.SPriorityId = priooritet.Id;
        //                                        dTicket_New.TicketNumber = LastTicketNumber + 1;
        //                                        dTicket_New.TicketNumberFull = sServices.ServicePrefix + (LastTicketNumber + 1);
        //                                        dTicket_New.SStatusId = 1;
        //                                        dTicket_New.DateRegistration = DateOnly.FromDateTime(DateTime.Now);
        //                                        dTicket_New.TimeRegistration = TimeOnly.FromDateTime(DateTime.Now);

        //                                        DTicketStatus dTicketStatus = new DTicketStatus
        //                                        {
        //                                            // DTicketId = eqContext.DTickets.First(s => s.SOfficeTerminal.IpAddress == Ip && s.DateRegistration == dTicket_New.DateRegistration && s.TimeRegistration == dTicket_New.TimeRegistration).Id,
        //                                            SStatusId = 1
        //                                        };

        //                                        dTicket_New.DTicketStatuses.Add(dTicketStatus);

        //                                        eqContext.DTickets.Add(dTicket_New);
        //                                        eqContext.SaveChanges();

        //                                        report.SetParameterValue("Operation", sServices.ServiceName);
        //                                        report.SetParameterValue("Number", dTicket_New.TicketNumberFull);
        //                                        report.SetParameterValue("Time", dTicket_New.TimeRegistration);
        //                                        report.SetParameterValue("TotalQueue", eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == Ip && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).Count());
        //                                        report.SetParameterValue("BeforeCount", LastTicketNumber);
        //                                        report.SetParameterValue("MFC", eqContext.SOffices.First(l => l.Id == eqContext.SOfficeTerminals.First(g => g.IpAddress == Ip).SOfficeId).OfficeName);
        //                                        report.Prepare();
        //                                        report.PrintSettings.ShowDialog = false;
        //                                        report.PrintSettings.PrintOnSheetRawPaperSize = 0;
        //                                        try
        //                                        {
        //                                            report.Print();
        //                                        }
        //                                        catch (Exception ex) { }
        //                                        await Client.SendMessageAsync("new Ticket", Ip);
        //                                    }
        //                                    catch (Exception ex)
        //                                    {

        //                                    }
        //                                };

        //                                ControlTemplate myControlTemplate = new ControlTemplate(typeof(Button));
        //                                FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
        //                                border.Name = "border";
        //                                border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
        //                                border.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
        //                                border.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
        //                                border.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
        //                                FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
        //                                contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        //                                contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
        //                                border.AppendChild(contentPresenter);
        //                                myControlTemplate.VisualTree = border;
        //                                btn.Template = myControlTemplate;

        //                                StackPanel stackPanelBtn = new StackPanel();
        //                                stackPanelBtn.Orientation = Orientation.Vertical;
        //                                stackPanelBtn.HorizontalAlignment = HorizontalAlignment.Center;
        //                                stackPanelBtn.Children.Add(btn);
        //                                wrapPanelPriooritetMenu.Children.Add(stackPanelBtn);
        //                            }
        //                        });
        //                    };

        //                    wrapPanelPreferentialСategoryСitizens.Children.Remove(wrapPanelPriooritetMenu);
        //                    wrapPanelPreferentialСategoryСitizens.Children.Remove(wrapPanelPriooritetMenuButons);
        //                    wrapPanelPreferentialСategoryСitizens.Children.Remove(btnPriooritet);

        //                    wrapPanelPreferentialСategoryСitizens.Children.Add(wrapPanelPriooritetMenu);
        //                    wrapPanelPreferentialСategoryСitizens.Children.Add(wrapPanelPriooritetMenuButons);
        //                    wrapPanelPriooritetButons.Children.Add(btnPriooritet);
        //                });
        //            }

        //            wrapPanelPreferentialСategoryСitizens.Children.Add(wrapPanelPriooritetButons);

        //            #region подвал PreRegistration 
        //            WrapPanel wrapPanelPriooritetFooter = new WrapPanel();
        //            wrapPanelPriooritetFooter.Name = "PreRegistrationFooter";
        //            wrapPanelPriooritetFooter.Orientation = Orientation.Horizontal;

        //            wrapPanelPriooritetFooter.VerticalAlignment = VerticalAlignment.Bottom;
        //            wrapPanelPriooritetFooter.Margin = new Thickness(0, 50, 0, 0);

        //            StackPanel stackPanelbtnBack = new StackPanel();
        //            stackPanelbtnBack.HorizontalAlignment = HorizontalAlignment.Left;
        //            stackPanelbtnBack.Width = 500;
        //            stackPanelbtnBack.Children.Add(btnBack);
        //            wrapPanelPriooritetFooter.Children.Add(stackPanelbtnBack);

        //            StackPanel stackPanelbtnNextStage = new StackPanel();
        //            stackPanelbtnNextStage.HorizontalAlignment = HorizontalAlignment.Right;
        //            stackPanelbtnNextStage.Width = 500;
        //            stackPanelbtnNextStage.Children.Add(btnNextStage);
        //            wrapPanelPriooritetFooter.Children.Add(stackPanelbtnNextStage);

        //            #endregion
        //            //foreach (WrapPanel obj in BodyWindow.Children)
        //            //{
        //            //    obj.Visibility = Visibility.Collapsed;
        //            //    if (obj.Name == "PreferentialСategoryСitizens")
        //            //    {
        //            //        obj.Visibility = Visibility.Visible;
        //            //    }
        //            //}
        //            //StackClose.Visibility = Visibility.Visible;

        //            foreach (StackPanel stackPanel in Buttons_Service.Children)
        //            {
        //                Button button = (Button)stackPanel.Children[0];
        //                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        //            }

        //            Button_Click_PreferentialСategoryСitizens.Background = new SolidColorBrush(Color.FromRgb(240, 250, 220));


        //            wrapPanelPreferentialСategoryСitizens.Children.Add(wrapPanelPriooritetFooter);
        //        };
        //    };

        //    //BodyWindow.Children.Add(wrapPanelPreferentialСategoryСitizens);
        //}
    }
}
