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
using BinaryAnalysis.UnidecodeSharp;

namespace HanziToPinyin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Inits vars
            this.reording = false;
            this.lines = new List<Line> { };

            // Add a demo line
            this.AddLine("北  京   交    通  大 学", "Bei jing jiao tong da xue");
            this.AddLine("北  京   交    通  大 学", "Bei jing jiao tong da xue");
            this.AddLine("北  京   交    通  大 学", "Bei jing jiao tong da xue");


            MessageBox.Show("北京".Unidecode());
        }

        public void AddLine(string txt = null, string trans = null)
        {
            int     idx;
            Line    line;

            idx = this.lines.Count + 1;
            line = new Line(idx, txt, txt.Unidecode());

            this.StackedLines.Children.Add(line.text);
            this.StackedLines.Children.Add(line.translation);
            line.text.Style = (Style)(this.Resources["LineTextBox"]); ;
            line.translation.Style = (Style)(this.Resources["LineLabel"]); ;

            line.text.TextChanged += textbox_TextChanged;

            this.lines.Add(line);
        }

        public void FetchWholeText()
        {
            this.whole_text = "";
            this.lines.ForEach(l => this.whole_text += l.text.Text);
        }

        public void DistributeText()
        {
            double app_w = App.Current.MainWindow.Width;
            double app_h = App.Current.MainWindow.Height;
            int max_lenght = (int)(app_w - (4 * 8)) / 20;

            MessageBox.Show(this.whole_text, max_lenght.ToString());

            string remaning = "";
            this.lines.ForEach(delegate(Line line)
            {
                if (remaning != "")
                {
                    line.text.Text = remaning + line.text.Text;
                    remaning = "";
                }
                if (line.text.Text.Length > max_lenght)
                {
                    int line_lenght = line.text.Text.Length;
                    remaning = line.text.Text.Substring(max_lenght, line_lenght);
                    line.text.Text = line.text.Text.Substring(max_lenght);
                }
                

                //string newl = this.whole_text.Substring(max_lenght);
                //this.whole_text.Remove(0, max_lenght);
                //line.text.Text = "PASS";
            });
            this.reording = false;
        }

        void textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!reording)
            {
                reording = true;
                this.FetchWholeText();
                this.DistributeText();
            }
        }

        public bool                         reording;
        public string                       whole_text { get; set; }
        private List<HanziToPinyin.Line>    lines { get; set; }
    }

    public class Line
    {
        public Line(int idx, string txt = null, string trans = null)
        {
            this.text = new TextBox();            
            this.translation = new Label();

            this.line_number = idx;
            this.text.Text = txt;
            this.translation.Content = trans;
        }

        public int      line_number { get; set; }
        public Label    translation { get; set; }
        public TextBox  text { get; set; }
    }
}
