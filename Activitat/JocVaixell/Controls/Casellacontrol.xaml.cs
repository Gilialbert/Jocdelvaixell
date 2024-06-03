using System;
using System.Collections.Generic;
using System.Drawing;
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
using Color = System.Windows.Media.Color;

namespace JocVaixell
{
    /// <summary>
    /// Lógica de interacción para Casellacontrol.xaml
    /// </summary>
    public partial class Casellacontrol : UserControl
    {
        public bool iswatah;
        public bool ishit = false;

        public Casellacontrol()
        {
            InitializeComponent();
        }
        private void Casella_Click(object sender, MouseButtonEventArgs e)
        {
            if (iswatah && !ishit)
            {
                casella.Fill = new SolidColorBrush(Color.FromRgb(66, 105, 140));
                iswatah = false;
            }
            else if (!iswatah && !ishit)
            {
                casella.Fill = new SolidColorBrush(Color.FromRgb(0, 102, 255));
                iswatah = true;
                
            }
        }
    }
}
