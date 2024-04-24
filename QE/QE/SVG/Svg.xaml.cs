using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.SVG
{
    /// <summary>
    /// Логика взаимодействия для Svg.xaml
    /// </summary>
    public partial class Svg : UserControl
    {
        private ObservableCollection<SvgData> DataList { get; set; }
        private Vector GeomSize = new Vector(0, 0);
        private Brush _Fill;
        public Svg(byte[] svg, SolidColorBrush brushes, double width, double height)
        {
            InitializeComponent();
            DataContext = this;
            DataList = new ObservableCollection<SvgData>();
            Width = width;
            Height = height;
            _Fill = brushes;
            Build(svg);

            Loaded += (o, e) =>
            {
                _resize();
            };
            ic.ItemsSource = DataList;
        }

        private void Build(byte[] data)
        {
            string data_ = System.Text.Encoding.Default.GetString(data);
            foreach (Match _svg in Regex.Matches(data_, "<svg[\\w\\W]+?<\\/svg>"))
            {
                foreach (Match item in Regex.Matches(_svg.Value, "<path[\\w\\W]+?\\/>"))
                {
                    foreach (Match _d in Regex.Matches(item.Value, "d=\\\"[\\w\\W]+?\\\""))
                    {
                        this.Add(Regex.Replace(_d.Value, "(d=\")|(\")", ""), _Fill);
                    }
                }
            }


            if (Regex.IsMatch(data_, RegexPattern.ViewBox))
            {
                string viv_ = Regex.Match(data_, RegexPattern.ViewBox).Value;

                viv_ = Regex.Match(viv_, "\"[\\s\\S]+?\"").Value.Replace("\"", "");
                if (viv_ != String.Empty)
                {
                    int[] ints = new int[4];
                    string[] array_ = viv_.Split(' ');

                    for (int i = 0; i < array_.Length; i++)
                    {
                        if (Regex.IsMatch(array_[i], "\\.[0-9]"))
                        {
                            ints[i] = int.Parse(Regex.Replace(array_[i], "\\.[0-9]+", ""));
                            continue;
                        }
                        ints[i] = int.Parse(array_[i]);
                    }
                    GeomSize.X = ints[2];
                    GeomSize.Y = ints[3];
                }
            }
        }


        public void Add(string str_, Brush brush)
        {
            this.DataList.Add(new SvgData()
            {
                Path = str_,
                Fill = brush,
            });

        }

        private void _resize()
        {
            ScaleTransform myScaleTransform = new ScaleTransform();
            myScaleTransform.ScaleX = this.ActualWidth / GeomSize.X;
            myScaleTransform.ScaleY = this.ActualHeight / GeomSize.Y;
            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(myScaleTransform);
            this.RenderTransform = myTransformGroup;
        }
    }
}
