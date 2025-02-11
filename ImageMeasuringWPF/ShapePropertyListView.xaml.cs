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

namespace ImageMeasuringWPF
{
    /// <summary>
    /// Interaction logic for ShapePropertyListView.xaml
    /// </summary>
    public partial class ShapePropertyListView : UserControl
    {
        private Shape selectedShape = null;
        public ShapePropertyListView()
        {
            InitializeComponent();
        }
        public void ListView_AddShape(Shape shape)
        {
            switch (shape.GetType().ToString())
            {
                case "System.Windows.Shapes.Line":
                    selectedShape = shape;
                    SaveLine saveLine = DrawingUtils.Line_GetPoints((Line)shape);
                    Shape_Data X1 = new Shape_Data();
                    X1.Property = "X1";
                    X1.Value = saveLine.X1.ToString();
                    ListViewProperties.Items.Add(X1);
                    Shape_Data Y1 = new Shape_Data();
                    Y1.Property = "Y1";
                    Y1.Value = saveLine.Y1.ToString();
                    ListViewProperties.Items.Add(Y1);
                    Shape_Data X2 = new Shape_Data();
                    X2.Property = "X2";
                    X2.Value = saveLine.X2.ToString();
                    ListViewProperties.Items.Add(X2);
                    Shape_Data Y2 = new Shape_Data();
                    Y2.Property = "Y2";
                    Y2.Value = saveLine.Y2.ToString();
                    ListViewProperties.Items.Add(Y2);
                    break;
                case "System.Windows.Shapes.Ellipse":
                    SaveCircle saveCircle = DrawingUtils.Circle_GetPoints((Ellipse)shape);
                    break;
            }


        }
        public void ClearList()
        {
            ListViewProperties.Items.Clear();
        }

        public void testAdd()
        {
            Shape_Data shapeData = new Shape_Data();
            shapeData.Property = "Test1";
            shapeData.Value = "1245.25";
            ListViewProperties.Items.Add(shapeData);
        }
        public void OnTextBox_PropertyChanged(object sender, RoutedEventArgs e)
        {
            Shape_Data data = (Shape_Data)(((TextBox)sender).DataContext);
            double newValue = 0.0;
            if (double.TryParse(((TextBox)sender).Text, out newValue))
            {
                switch (selectedShape.GetType().ToString())
                {
                    case "System.Windows.Shapes.Line":
                        switch (data.Property)
                        {
                            case "X1":
                                ((Line)selectedShape).X1 = newValue;
                                break;
                            case "Y1":
                                ((Line)selectedShape).Y1 = newValue;
                                break;
                            case "X2":
                                ((Line)selectedShape).X2 = newValue;
                                break;
                            case "Y2":
                                ((Line)selectedShape).Y2 = newValue;
                                break;
                        }
                        break;
                    case "System.Windows.Shapes.Ellipse":
                        break;
                }
            }
        }
    }
    public class Shape_Data
    {
        public string? Property { get; set; }
        public string? Value { get; set; }
    }
}
