using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace JocVaixell
{
    /// <summary>
    /// Lógica de interacción para TableAlbertcontrol.xaml
    /// </summary>
    public partial class TableAlbertcontrol : UserControl
    {
        private bool isLoaded = false;

        public static readonly DependencyProperty FilasProperty =
          DependencyProperty.Register("Filas", typeof(int), typeof(TableAlbertcontrol), new FrameworkPropertyMetadata(3, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        
        public static readonly DependencyProperty ColumnasProperty =
            DependencyProperty.Register("Columnas", typeof(int), typeof(TableAlbertcontrol), new FrameworkPropertyMetadata(3, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public int Rows
        {
            get { return (int)GetValue(FilasProperty); }
            set { SetValue(FilasProperty, value); }
        }
        public int Colums
        {
            get { return (int)GetValue(ColumnasProperty); }
            set { SetValue(ColumnasProperty, value); }
        }

        public TableAlbertcontrol()
        {
            InitializeComponent();
            Loaded += TableControl_Loaded;
        }
        private void TableControl_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            DrawGrid();
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TableAlbertcontrol;
            if (control.isLoaded)
            {
                control.DrawGrid();
            }
        }
        public void DrawGrid()
        {
            this.MainGrid.RowDefinitions.Clear();
            this.MainGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < Rows; i++)
            {
                this.MainGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < Colums; i++)
            {
                this.MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            int rnd = new Random().Next(0, Colums);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Colums; j++)
                {
                    Casellacontrol cell = new Casellacontrol();
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                    if (i == rnd && j == rnd)
                    {
                        cell.iswatah = false;
                    }
                    else
                    {
                        cell.iswatah = true;
                    }
       
                    cell.texte.Text= $"{i},{j}";
                    MainGrid.Children.Add(cell);
                }
            }
            paintgrid(this.MainGrid);
        }
        public string buscar(int R, int C)
        {
            if (R > 0 && R < Rows && C > 0 && C < Colums)
            {
                UIElement element = MainGrid.Children
                    .Cast<UIElement>()
                    .First(e => Grid.GetRow(e) == R && Grid.GetColumn(e) == C);
                if (element is Casellacontrol cell)
                {
                    if (cell.iswatah)
                    {
                        return ("Agua");
                    }
                    else
                    {
                        return ("Vaixell");
                    }
                }
            }
            else
            {
                return ("Coordenadas fuera de rango.");
            }
            return ("Error");
        }
        public void shiphitted(int R, int C)
        {
            if (R > 0 && R < Rows && C > 0 && C < Colums)
            {
                UIElement element = MainGrid.Children
                    .Cast<UIElement>()
                    .First(e => Grid.GetRow(e) == R && Grid.GetColumn(e) == C);
                if (element is Casellacontrol cell)
                {
                    cell.ishit = true;
                    cell.casella.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }
            }
        }
        public void paintgrid(Grid grid)
        {
            foreach (UIElement element in grid.Children)
            {
                if (element is Casellacontrol cell)
                {
                    if (cell.iswatah)
                    {
                        cell.casella.Fill = new SolidColorBrush(Color.FromRgb(0, 102, 255)); // Pintar celdas de agua en azul
                    }
                    else
                    {
                        cell.casella.Fill = new SolidColorBrush(Color.FromRgb(66, 105, 140));// Pintar celdas de vaixell en gris
                    }
                }
            }
        }
    }
}
