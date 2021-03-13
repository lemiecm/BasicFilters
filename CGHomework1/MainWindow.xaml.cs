using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
 


namespace CGHomework1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int num = 1;
        private bool resetFlag = false;
        private Dictionary<string, List<Vertex>> customFilters;
        int[] functionFig;
        public ViewModel myView;
        public MainWindow()
        {
            InitializeComponent();
            myView = new ViewModel();
            customFilters = new Dictionary<string, List<Vertex>>();
            this.DataContext = myView;
            var tmpList = new List<Vertex>();
            var vertex1 = new Vertex();
            var vertex4 = new Vertex();
            vertex1.Point = new Point(22, 277);
            vertex4.Point = new Point(277, 22);
            tmpList.Add(vertex1);
            tmpList.Add(vertex4);
            customFilters.Add("default", tmpList);
        }
        
        private void AddVertexOnCanvas(object sender, MouseButtonEventArgs e)

        {
            if (myView.Vertices.Count!=0)
            {
                if (Mouse.GetPosition(myCanvas).X > 22 && Mouse.GetPosition(myCanvas).X < 277 && Mouse.GetPosition(myCanvas).Y >= 22 && Mouse.GetPosition(myCanvas).Y <= 277)
                {

                    var newVertex = new Vertex();
                    List<Vertex> vertices = new List<Vertex>();
                    var q = myView.Vertices.IndexOf(myView.Vertices.Where(X => X.Point.X > Mouse.GetPosition(myCanvas).X).FirstOrDefault());
                    newVertex.Point = new Point(Mouse.GetPosition(myCanvas).X, Mouse.GetPosition(myCanvas).Y);
                    // how much vertices to redraw before adding new
                    int verticesToRedraw = myView.Vertices.Count - q;

                    for (int i = 0; i < verticesToRedraw; i++)
                    {
                        vertices.Add(myView.Vertices[myView.Vertices.Count - 1]);
                        myView.Vertices.RemoveAt(myView.Vertices.Count - 1);
                    }
                    myView.Vertices.Add(newVertex);
                    for (int i = verticesToRedraw - 1; i > -1; i--)
                    {
                        myView.Vertices.Add(vertices[i]);


                    }
                }



            }
        }
        private void RemoveVertexFromCanvas(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Ellipse)
            {
                Ellipse activeEllipse = (Ellipse)e.OriginalSource;
                var activeVertex = new Vertex();
                activeVertex.Point = new Point(Mouse.GetPosition(myCanvas).X, Mouse.GetPosition(myCanvas).Y);

                myCanvas.Children.Remove(activeEllipse);
                var q = myView.Vertices.IndexOf(myView.Vertices.Where(X => X.Point.X >= Mouse.GetPosition(myCanvas).X - 4 && X.Point.X <= Mouse.GetPosition(myCanvas).X + 4 && X.Point.Y >= Mouse.GetPosition(myCanvas).Y - 4 && X.Point.Y <= Mouse.GetPosition(myCanvas).Y + 4).FirstOrDefault());
                if(q!=0 && q!= myView.Vertices.Count - 1)
                    myView.Vertices.Remove(myView.Vertices[q]);

            }
            
           
               



        }
        
        private void btnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            int[] tmpPixelMap = new int[256];
            var functionStack = StackFunctionFilters.Children.OfType<RadioButton>()
                 .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            string mystring;
            bool emptyString = false;


            if (functionStack != null)
                mystring = functionStack.Content.ToString();
            else
                mystring = null;
            var myNewList = new List<Vertex>();
            if (functionStack==null)
            {
                emptyString = true;
                var tmpList = new List<Vertex>();
                foreach (var vertex in myView.Vertices.ToList())
                {
                    var tmp = new Vertex();
                    tmp.Point = new Point(vertex.Point.X, vertex.Point.Y);
                    tmpList.Add(vertex);
                }
                mystring = "tmp";
                customFilters.Add(mystring, tmpList);
            }
            else if (!customFilters.TryGetValue(mystring,out myNewList))
            {
                if (mystring == "Inversion")
                {
                    functionStack.Click -= new RoutedEventHandler(InversionCheck);
                }
                else if (mystring == "Brigthness Correction")
                {
                    functionStack.Click -= new RoutedEventHandler(BrightCheck);
                }
                else if (mystring == "Contrast Enhancement")
                {
                    functionStack.Click -= new RoutedEventHandler(ContrastCheck);

                }
                functionStack.Click += new RoutedEventHandler(btnCustomFilter_Click);
                var tmpList = new List<Vertex>();
                foreach (var vertex in myView.Vertices.ToList())
                {
                    var tmp = new Vertex();
                    tmp.Point = new Point(vertex.Point.X, vertex.Point.Y);
                    tmpList.Add(vertex);
                }
                customFilters.Add(mystring, tmpList);
                
            }
            else
            {
                var tmpList = new List<Vertex>();
                foreach (var vertex in myView.Vertices.ToList())
                {
                    var tmp = new Vertex();
                    tmp.Point = new Point(vertex.Point.X, vertex.Point.Y);
                    tmpList.Add(vertex);
                }
                customFilters.Remove(mystring);
                customFilters.Add(mystring, tmpList);

            }
            for (int i = 0; i < customFilters[mystring].Count; i++)
            {
                if (i == 0)
                {
                    tmpPixelMap[i] = 0;
                }
                else
                {
                    int denominator = (int)(customFilters[mystring][i].Point.X - customFilters[mystring][i - 1].Point.X);
                    double leftVertex = (int)(277 - customFilters[mystring][i - 1].Point.Y);
                    double rigthVertex = (int)(277 - customFilters[mystring][i].Point.Y);

                    double coeff = (rigthVertex - leftVertex) / denominator;
                    for (int j = 1; j <= denominator; j++)
                    {

                        tmpPixelMap[(int)customFilters[mystring][i - 1].Point.X - 22 + j] = (int)(leftVertex + coeff * j);
                    }
                }

            }
            var help = new Helper();
            imgFiltered.Source = help.MyLittleHelp((BitmapSource)imgFiltered.Source, tmpPixelMap);
            if(emptyString)
                customFilters.Remove("tmp");
        }
        private void btnSaveFilter_Click(object sender, RoutedEventArgs e)
        {
            if(num <= 6)
            {
                int tmpNum = num++;
                customFilters.Add($"Custom Filter {tmpNum}", myView.Vertices.ToList());
                RadioButton btn = new RadioButton();
                btn.Content = $"Custom Filter {tmpNum}";
                btn.Click += new RoutedEventHandler(btnCustomFilter_Click);
                StackFunctionFilters.Children.Add(btn);
            }
            else
            {
                MessageBox.Show("You have reached final number of custom filters.");
            }
            
        }

        private void btnCustomFilter_Click(object sender, RoutedEventArgs e)
        {
            if (NewPolyline.Points.Count != 0)
            {
                NewPolyline.Points.Clear();
            }
            string content = (sender as RadioButton).Content.ToString();
            List<Vertex> vertices = new List<Vertex>();
            customFilters.TryGetValue(content, out vertices);

            btnFunctionApply.IsEnabled = true;
            btnFunctionSave.IsEnabled = true;
            int[] tmpPixelMap = new int[256];

            for (int i = 0; i < vertices.Count; i++)
            {
                if (i == 0)
                {
                    tmpPixelMap[i] = 0;
                }
                else
                {
                    int denominator = (int)(vertices[i].Point.X - vertices[i - 1].Point.X);
                    double leftVertex = (int)(277 - vertices[i - 1].Point.Y);
                    double rigthVertex = (int)(277 - vertices[i].Point.Y);

                    double coeff = (rigthVertex - leftVertex) / denominator;
                    for (int j = 1; j <= denominator; j++)
                    {

                        tmpPixelMap[(int)vertices[i - 1].Point.X - 22 + j] = (int)(leftVertex + coeff * j);
                    }
                }


            }
            myView.Vertices.Clear();
            foreach (var vertex in customFilters[content])
            {
                var tmp = new Vertex();
                tmp.Point = new Point(vertex.Point.X, vertex.Point.Y);
                myView.Vertices.Add(tmp);
            }
            
            var help = new Helper();
            imgFiltered.Source = help.MyLittleHelp((BitmapSource)imgFiltered.Source, tmpPixelMap);
        }
        private void uploadClick(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                imgStart.Source = new BitmapImage(fileUri);
                imgFiltered.Source = new BitmapImage(fileUri);

                StackConvolutionalFilters.IsEnabled = true;
                StackFunctionFilters.IsEnabled = true;
                btnReset.IsEnabled = true;
                btnSave.IsEnabled = true;
                btnFunctionApply.IsEnabled = true;
                btnFunctionSave.IsEnabled= true;


                functionFig = new int[256];
                for(int i =0; i < 256; i++)
                {
                    functionFig[i] = i;
                }
                if(myView.Vertices.Count ==0)
                    CreateAPolyline();
            }
        }
        private void CreateAPolyline()
        {
            var vertex1 = new Vertex();
            var vertex4 = new Vertex();
            vertex1.Point = new Point(22, 277);
            vertex4.Point = new Point(277, 22);
            myView.Vertices.Add(vertex1);
            myView.Vertices.Add(vertex4);
        }
        // VieModel and Vertex Source: https://stackoverflow.com/questions/52678534/c-sharp-wpf-how-to-draw-rectangle-on-polyline-points
        private void ThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            var vertex = (Vertex)((Thumb)sender).DataContext;
            var q = myView.Vertices.IndexOf(myView.Vertices.Where(X => X== vertex).FirstOrDefault());
            if ((vertex==myView.Vertices[0] || vertex == myView.Vertices[myView.Vertices.Count-1])&& (vertex.Point.Y + e.VerticalChange >= 22 && vertex.Point.Y + e.VerticalChange <= 277))
                vertex.Point = new Point(
                    vertex.Point.X ,
                    vertex.Point.Y + e.VerticalChange);
     
            else if ((vertex.Point.Y + e.VerticalChange >= 22 && vertex.Point.Y + e.VerticalChange <= 277) && vertex.Point.X + e.HorizontalChange > myView.Vertices[q-1].Point.X && vertex.Point.X + e.HorizontalChange< myView.Vertices[q +1 ].Point.X)
                vertex.Point = new Point(
                    vertex.Point.X + e.HorizontalChange,
                    vertex.Point.Y + e.VerticalChange);
           
        }
       
       
        public class ViewModelBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class Vertex : ViewModelBase
        {
            private Point point;

            public Point Point
            {
                get { return point; }
                set { point = value; OnPropertyChanged(); }
            }
        }

        public class ViewModel : ViewModelBase
        {
            public ViewModel()
            {
                Vertices.CollectionChanged += VerticesCollectionChanged;
            }

            public ObservableCollection<Vertex> Vertices { get; set; }
                = new ObservableCollection<Vertex>();

            private void VerticesCollectionChanged(
                object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in e.NewItems.OfType<INotifyPropertyChanged>())
                    {
                        item.PropertyChanged += VertexPropertyChanged;
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (var item in e.OldItems.OfType<INotifyPropertyChanged>())
                    {
                        item.PropertyChanged -= VertexPropertyChanged;
                    }
                }

                OnPropertyChanged(nameof(Vertices));
            }

            private void VertexPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                OnPropertyChanged(nameof(Vertices));
            }
        }
        // Function filters
        
        private void InversionCheck(object sender, RoutedEventArgs e)
        {
            if (NewPolyline.Points.Count != 0)
            {
                NewPolyline.Points.Clear();
            }
                var convolutionalStack = StackConvolutionalFilters.Children.OfType<RadioButton>()
                     .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);


                if (convolutionalStack != null)
                {
                    convolutionalStack.IsChecked = false;

                }
                var help = new Helper();
                myView.Vertices.Clear();
            btnFunctionApply.IsEnabled = true;
            btnFunctionSave.IsEnabled = true;
            foreach (var vertex in customFilters["default"])
                {
                    var tmp = new Vertex();
                    tmp.Point = new Point(vertex.Point.X, vertex.Point.Y);
                    myView.Vertices.Add(tmp);
                }
                imgFiltered.Source = help.Invert((BitmapSource)imgFiltered.Source, functionFig, myView);
            

        }
        private void BrightCheck(object sender, RoutedEventArgs e)
        {
            if (NewPolyline.Points.Count != 0)
            {
                NewPolyline.Points.Clear();
            }
            var convolutionalStack = StackConvolutionalFilters.Children.OfType<RadioButton>()
                 .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);

           
            if (convolutionalStack != null)
            {
                convolutionalStack.IsChecked = false;

            }
            var help = new Helper();
            int change_value = 60;
            myView.Vertices.Clear();
            btnFunctionApply.IsEnabled = true;
            btnFunctionSave.IsEnabled = true;
            foreach (var vertex in customFilters["default"])
            {
                var tmp = new Vertex();
                tmp.Point = new Point(vertex.Point.X, vertex.Point.Y);
                myView.Vertices.Add(tmp);
            }
            imgFiltered.Source = help.ChangeBrightness((BitmapSource)imgFiltered.Source, change_value, functionFig,myView);
        }
        private void ContrastCheck(object sender, RoutedEventArgs e)
        {

            if (NewPolyline.Points.Count != 0)
            {
                NewPolyline.Points.Clear();
            }
            var convolutionalStack = StackConvolutionalFilters.Children.OfType<RadioButton>()
                 .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);

           
            if (convolutionalStack != null)
            {
                convolutionalStack.IsChecked = false;

            }
            var help = new Helper();
            myView.Vertices.Clear();
            btnFunctionApply.IsEnabled = true;
            btnFunctionSave.IsEnabled = true;
            foreach (var vertex in customFilters["default"])
            {
                var tmp = new Vertex();
                tmp.Point = new Point(vertex.Point.X, vertex.Point.Y);
                myView.Vertices.Add(tmp);
            }
            imgFiltered.Source = help.ChangeContrast((BitmapSource)imgFiltered.Source, functionFig, myView);
        }

        private void GammaCheck(object sender, RoutedEventArgs e)
        {

            if (NewPolyline.Points.Count != 0)
            {
                NewPolyline.Points.Clear();
            }
            var convolutionalStack = StackConvolutionalFilters.Children.OfType<RadioButton>()
                 .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);

         
            if (convolutionalStack != null)
            {
                convolutionalStack.IsChecked = false;

            }
            int[] tmpMatrix = new int[256];
            for(int i =0; i< 256;i++)
            {
                functionFig[i] = i;
            }
            var help = new Helper();
            imgFiltered.Source = help.ChangeGamma((BitmapSource)imgFiltered.Source, functionFig, myView);
            myView.Vertices.Clear();
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;
            NewPolyline.Stroke = blackBrush;
            NewPolyline.StrokeThickness = 3;
            btnFunctionApply.IsEnabled = false;
            btnFunctionSave.IsEnabled = false;
            for (int i =0;i < 256;i++)
            {
                var tmp = new Point(i+22, functionFig[i]+22);
                NewPolyline.Points.Add(tmp);
            }
           
        }

        // Convolutional filters
        private void EmbossCheck(object sender, RoutedEventArgs e)
        {
            if (NewPolyline.Points.Count != 0)
            {
                NewPolyline.Points.Clear();
            }
            var functionStack = StackFunctionFilters.Children.OfType<RadioButton>()
                 .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);

           

            if (functionStack != null)
            {
                functionStack.IsChecked = false;

            }
           
            var help = new Helper();

                int[,] filterMatrix =
            new int[,] {{ 2,  0,  0, },
                        { 0, -1,  0, },
                        { 0,  0, -1, }, };
            myView.Vertices.Clear();
            btnFunctionApply.IsEnabled = false;
            btnFunctionSave.IsEnabled = false;
            
            imgFiltered.Source = help.ChangeBlur((BitmapSource)imgFiltered.Source, filterMatrix, 128);
               
            
           
        }

        private void EdgeCheck(object sender, RoutedEventArgs e)
        {
            if (NewPolyline.Points.Count != 0)
            {
                NewPolyline.Points.Clear();
            }
            var functionStack = StackFunctionFilters.Children.OfType<RadioButton>()
                 .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);



            if (functionStack != null)
            {
                functionStack.IsChecked = false;

            }
            var help = new Helper();

            int[,] filterMatrix =
        new int[,] {{ -1,-1, -1, },
                        { -1,  8,-1, },
                        { -1, -1, -1, }, };
            myView.Vertices.Clear();
            btnFunctionApply.IsEnabled = false;
            btnFunctionSave.IsEnabled = false;
           
            imgFiltered.Source = help.ChangeBlur((BitmapSource)imgFiltered.Source, filterMatrix,0);
           
        }

        private void SharpenCheck(object sender, RoutedEventArgs e)
        {
            if (NewPolyline.Points.Count != 0)
            {
                NewPolyline.Points.Clear();
            }
            var functionStack = StackFunctionFilters.Children.OfType<RadioButton>()
                 .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);



            if (functionStack != null)
            {
                functionStack.IsChecked = false;

            }
            var help = new Helper();

            int[,] filterMatrix =
        new int[,] {{ -1, -1,  -1, },
                        { -1,  9, -1, },
                        {  -1, -1,  -1, }, };
            myView.Vertices.Clear();
            btnFunctionApply.IsEnabled = false;
            btnFunctionSave.IsEnabled = false;
            
            imgFiltered.Source = help.ChangeBlur((BitmapSource)imgFiltered.Source, filterMatrix,0);
           
        
        }

        private void GaussianCheck(object sender, RoutedEventArgs e)
        {
            if (NewPolyline.Points.Count != 0)
            {
                NewPolyline.Points.Clear();
            }
            var functionStack = StackFunctionFilters.Children.OfType<RadioButton>()
                 .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);



            if (functionStack != null)
            {
                functionStack.IsChecked = false;

            }
            var help = new Helper();
           
            int[,] filterMatrix =
        new int[,] { { 1, 2, 1, },
                        { 2, 4, 2, },
                        { 1, 2, 1, }, };
            myView.Vertices.Clear();
            btnFunctionApply.IsEnabled = false;
            btnFunctionSave.IsEnabled = false;
            
            imgFiltered.Source = help.ChangeBlur((BitmapSource)imgFiltered.Source, filterMatrix,0);
           
            
        }

        private void BlurCheck(object sender, RoutedEventArgs e)
        {
            if (NewPolyline.Points.Count != 0)
            {
                NewPolyline.Points.Clear();
            }
            var functionStack = StackFunctionFilters.Children.OfType<RadioButton>()
                 .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);



            if (functionStack != null)
            {
                functionStack.IsChecked = false;

            }
            var help = new Helper();
            
            int[,] filterMatrix =
        new int[,] { { 1, 1, 1, },
                        { 1, 1, 1, },
                        { 1, 1, 1, }, };
            myView.Vertices.Clear();
            btnFunctionApply.IsEnabled = false;
            btnFunctionSave.IsEnabled = false;
            
            imgFiltered.Source = help.ChangeBlur((BitmapSource)imgFiltered.Source,filterMatrix,0);
            
        }
        
        

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki JPG | *.jpg | Pliki BMP | *.bmp | Pliki PNG | *.png";
            if (saveFileDialog.ShowDialog() == true)
            {
                var encoder = new JpegBitmapEncoder(); // Or PngBitmapEncoder, or whichever encoder you want
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imgFiltered.Source));
                using (var stream = saveFileDialog.OpenFile())
                {
                    encoder.Save(stream);
                }

            }
          



        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (NewPolyline.Points.Count != 0)
            {
                NewPolyline.Points.Clear();
            }
            resetFlag = true;
            var functionStack = StackFunctionFilters.Children.OfType<RadioButton>()
                 .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);

            var convolutionalStack = StackConvolutionalFilters.Children.OfType<RadioButton>()
                 .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            btnFunctionApply.IsEnabled = true;
            btnFunctionSave.IsEnabled = true;
            if (functionStack!=null)
            {
                functionStack.IsChecked = false;

            }
            if(convolutionalStack !=null)
            {
                convolutionalStack.IsChecked = false;

            }

            myView.Vertices.Clear();
            imgFiltered.Source = imgStart.Source;
            foreach(var vertex in  customFilters["default"])
            {
                var tmp = new Vertex();
                tmp.Point = new Point(vertex.Point.X, vertex.Point.Y);
                myView.Vertices.Add(vertex);
            }
           
            //CreateAPolyline();
            resetFlag = false;




        }
    }
}
