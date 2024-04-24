using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.Models
{
    public partial class Main
    {
        private async Task ActivePages()
        {
            if (_isActiveWorkTime)
            {
                await MainPages(_window.ContentWrapper);
            }
            else
            {
                Page.Error(_window.ContentWrapper, "Нерабочее время");
            }
            UpdateButton();
        }

        private void UpdateButton()
        {

            ControlTemplate myControlTemplateOne = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderOne = new FrameworkElementFactory(typeof(Border));
            borderOne.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
            borderOne.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
            borderOne.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
            FrameworkElementFactory contentPresenterOne = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterOne.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Bottom);
            contentPresenterOne.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            borderOne.AppendChild(contentPresenterOne);
            myControlTemplateOne.VisualTree = borderOne;
             
            ControlTemplate myControlTemplateTwo = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderTwo = new FrameworkElementFactory(typeof(Border));
            borderTwo.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
            borderTwo.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
            borderTwo.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
            FrameworkElementFactory contentPresenterTwo = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterTwo.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            contentPresenterTwo.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterTwo.SetValue(FrameworkElement.MarginProperty, new Thickness(15, 0, 15, 0));
            borderTwo.AppendChild(contentPresenterTwo);
            myControlTemplateTwo.VisualTree = borderTwo;


            #region Кнопка Домой
            _window.HomeButton.Content = new SVG.Svg(Properties.Resources.homeButton, new SolidColorBrush(_colorDto.ColorTextFooter), 100, 100);
            _window.HomeButton.Template = myControlTemplateOne;
            _window.HomeButton.Click += async (s, e) =>
            {
                if (_isActiveWorkTime)
                {
                    _window.PageNameText.Text = string.Empty;
                    _window.PageNameText.Visibility = Visibility.Hidden;
                    await MainPages(_window.ContentWrapper);
                }
            };

            #endregion

            #region Кнопка "Льготная категория граждан"

            if (_settingDto.isActiveButtonPriority)
            {
                _window.PreferentialСategoryСitizensButton.BorderBrush = new SolidColorBrush(_colorDto.ColorTextFooter);
                _window.PreferentialСategoryСitizensButton.Foreground = new SolidColorBrush(_colorDto.ColorTextFooter);
                _window.PreferentialСategoryСitizensButton.Visibility = Visibility.Visible;
                _window.PreferentialСategoryСitizensButton.Template = myControlTemplateTwo;
                _window.PreferentialСategoryСitizensButton.Click += async (s, e) =>
                {
                    if (_isActiveWorkTime)
                    {
                        _window.PageNameText.Text = "Льготная категория граждан";
                        _window.PageNameText.Visibility = Visibility.Visible;
                        await PriorityPages(_window.ContentWrapper);
                    }
                };
            }

            #endregion

            #region Кнопка "Предварительная запись"

            if (_settingDto.isActiveButtonPreRecord)
            {
                _window.PreRegistrationButton.BorderBrush = new SolidColorBrush(_colorDto.ColorTextFooter);
                _window.PreRegistrationButton.Foreground = new SolidColorBrush(_colorDto.ColorTextFooter);
                _window.PreRegistrationButton.Visibility = Visibility.Visible;
                _window.PreRegistrationButton.Template = myControlTemplateTwo;
                _window.PreRegistrationButton.Click += async (s, e) =>
                {
                    if (_isActiveWorkTime)
                    {
                        _window.PageNameText.Text = "Предварительная запись";
                        _window.PageNameText.Visibility = Visibility.Visible;
                        await PreRecordPages(_window.ContentWrapper);
                    }
                };
            }

            #endregion

            #region Кнопка "Регистрация по предварительной записи"

            if (_settingDto.isActiveButtonPreRecord)
            {
                _window.RegistrationPreRecordButton.BorderBrush = new SolidColorBrush(_colorDto.ColorTextFooter);
                _window.RegistrationPreRecordButton.Foreground = new SolidColorBrush(_colorDto.ColorTextFooter);
                _window.RegistrationPreRecordButton.Visibility = Visibility.Visible;
                _window.RegistrationPreRecordButton.Template = myControlTemplateTwo;
                _window.RegistrationPreRecordButton.Click += (s, e) =>
                {
                    if (_isActiveWorkTime)
                    {
                        _window.PageNameText.Text = "Регистрация по предварительной записи";
                        _window.PageNameText.Visibility = Visibility.Visible;
                        RegistrationPreRecordPages(_window.ContentWrapper);
                    }
                };
            }
            #endregion

            #region Справочная

            _window.ReferenceButton.Content = new SVG.Svg(Properties.Resources.referenceButton, new SolidColorBrush(_colorDto.ColorTextFooter), 100, 100);
            _window.ReferenceButton.Template = myControlTemplateOne;
            _window.ReferenceButton.Click += (s, e) =>
            {
                if (_isActiveWorkTime)
                {
                    _window.PageNameText.Text = "Справочная";
                    _window.PageNameText.Visibility = Visibility.Visible;
                    ShelduesPages(_window.ContentWrapper);
                }
            };

            #endregion
        }
    }
}
