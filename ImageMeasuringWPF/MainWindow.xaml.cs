using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Image m_Image { get; set; }
        private string ImagePath { get; set; }
        private Point mousePoint;
        private Point startPoint;
        private Point endPoint;
        private List<Shape> SelectedShapes = null;
        private double scaleFactor = 1.0;
        private int drawStep = 0;
        private enum DrawCommands
        {
            None,
            Line,
            Arc,
            Rectangle,
            Circle,
            Erase
        }
        private DrawCommands m_DrawCommands = DrawCommands.None;

        public MainWindow()
        {
            InitializeComponent();
            CommandBinding open_image_binding = new CommandBinding(ApplicationCommands.New);
            CommandBinding save_workspace_binding = new CommandBinding(ApplicationCommands.Save);
            CommandBinding Open_workspace_binding = new CommandBinding (ApplicationCommands.Open);
            open_image_binding.Executed += Open_Image;
            save_workspace_binding.Executed += SaveWorkSpace;
            Open_workspace_binding.Executed += Open_File;
            CommandBindings.Add(open_image_binding);
            CommandBindings.Add(save_workspace_binding);
            CommandBindings.Add(Open_workspace_binding);
            SelectedShapes = new List<Shape>();
            TextBox_ScaleFactor.Text = scaleFactor.ToString();
        }
        public void Open_File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "JSON(*.json)|*.json";
            open_dialog.Title = "Open File";
            Stream? stream = null;
            bool? result = open_dialog.ShowDialog();
            if (result == true)
            {
                stream = open_dialog.OpenFile();
                SerializeElements elements = JsonSerializer.Deserialize<SerializeElements>(stream);
                scaleFactor = elements.ScaleFactor;
                TextBox_ScaleFactor.Text = scaleFactor.ToString();
                ImagePath = elements.ImageSource;
                AddImageToCanvas();
                if (elements.SaveLines != null)
                {
                    foreach (SaveLine line in elements.SaveLines)
                    {
                        startPoint.X = line.X1;
                        startPoint.Y = line.Y1;
                        endPoint.X = line.X2;
                        endPoint.Y = line.Y2;
                        DrawLine();
                    }
                }
                if(elements.SaveCircles != null)
                {
                    foreach (SaveCircle circle in elements.SaveCircles)
                    {
                        startPoint.X = circle.CenterX;
                        startPoint.Y = circle.CenterY;
                        endPoint.X = circle.CenterX + (circle.Diameter / 2);
                        endPoint.Y = startPoint.Y;
                        DrawCircle();
                    }
                }
                stream.Close();
            }
        }
        public void Open_Image(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            openFile.Title = "Choose Image";
            bool? result = openFile.ShowDialog();
            if (result == true)
            {
                //Get the path of specified file
                ImagePath = openFile.FileName;
                AddImageToCanvas();
            }
        }
        public void AddImageToCanvas()
        {
            Graphic.Source = new BitmapImage(new Uri(ImagePath));
            canvas.Width = Graphic.Source.Width;
            canvas.Height = Graphic.Source.Height;
            DefaultBackground.Width = Graphic.Source.Width;
            DefaultBackground.Height = Graphic.Source.Height;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

            mousePoint = e.GetPosition(this.canvas);
            Mouse_Pos.Text = string.Format("X: {0} Y: {1}", mousePoint.X, mousePoint.Y);
            switch(m_DrawCommands)
            {
                case DrawCommands.None:
                    break;
                case DrawCommands.Line:
                    if (drawStep == 2)
                    {
                        ((Line)canvas.Children[canvas.Children.Count - 1]).X2 = mousePoint.X;
                        ((Line)canvas.Children[canvas.Children.Count - 1]).Y2 = mousePoint.Y;
                    }
                    break;
                case DrawCommands.Circle:
                    if (drawStep == 2)
                    {
                        double radius = 0;
                        radius = Math.Sqrt(Math.Pow(Math.Abs(mousePoint.X - startPoint.X), 2) + Math.Pow(Math.Abs(mousePoint.Y - startPoint.Y), 2));
                        DrawingUtils.Circle_SetPoints(((Ellipse)canvas.Children[canvas.Children.Count - 1]), startPoint.X, startPoint.Y, radius);
                    }
                    break;
                case DrawCommands.Arc:
                    if(drawStep == 1)
                    {

                    }
                    else if (drawStep == 2)
                    {
                        double slope = 0;
                        double perpendicular = 0;
                        double py_intercept = 0;
                        double mid_intercept = 0;
                        double center_Intercept = 0;
                        double x_cen = 0;
                        double y_cen = 0;
                        double radius = 0;
                        double x_dist = 0; //x distance from x_cen to startPoint.X
                        double y_dist = 0; //y distacne from y_cen to StartPoint.Y
                        Horizontality horizontality = DrawingUtils.CheckHorizontal(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                        Point line_mid = DrawingUtils.Line_GetMidPoint(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                        switch(horizontality)
                        {
                            case Horizontality.Horizontal:
                                slope = 0;
                                break;
                            case Horizontality.Slope:
                                slope = DrawingUtils.Line_GetSlope(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                                perpendicular = - Math.Pow(slope, -1);
                                py_intercept = perpendicular*line_mid.X-line_mid.Y;
                                mid_intercept = line_mid.Y+perpendicular*line_mid.X;
                                center_Intercept = mousePoint.X-slope*mousePoint.X;
                                x_cen = (center_Intercept - mid_intercept)/(perpendicular-slope);
                                y_cen = perpendicular * x_cen + center_Intercept;
                                x_dist = Math.Abs(x_cen - startPoint.X);
                                y_dist = Math.Abs(y_cen - startPoint.Y);
                                radius = Math.Sqrt(Math.Pow(x_dist, 2)+Math.Pow(y_dist, 2));
                                break;
                        }                        
                        DrawingUtils.Arc_SetPoints(((System.Windows.Shapes.Path)canvas.Children[canvas.Children.Count-1]), startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, radius, x_cen, y_cen);
                    }
                    break;
            }

        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch(drawStep)
            {
                case 0:
                    startPoint = e.GetPosition(this.canvas);
                    endPoint.X = startPoint.X + 1;
                    endPoint.Y = startPoint.Y + 1;
                    switch (m_DrawCommands)
                    {
                        case DrawCommands.None:
                            drawStep = 0;
                            startPoint = e.GetPosition(this.canvas);
                            break;
                        case DrawCommands.Line:
                            DrawLine();
                            drawStep=2;
                            break;
                        case DrawCommands.Circle:
                            DrawCircle();
                            drawStep=2;
                            break;
                        case DrawCommands.Arc:
                            DrawArc();
                            break;
                    }
                    break;
                case 1:
                    break;
                case 2:
                    drawStep = 0;
                    break;
            }

        }
        private void DrawLine()
        {
            Line line = new Line();
            line.X1 = startPoint.X;
            line.Y1 = startPoint.Y;
            line.X2 = endPoint.X;
            line.Y2 = endPoint.Y;
            line.Stroke = Brushes.Black;
            line.MouseEnter += OnMouseEnterElement;
            line.MouseDown += OnElementMouseDown;
            canvas.Children.Add(line);
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShapeProperties.ClearList();
            foreach(object sp_object in Buttons.Children)
            {
                if(sp_object is Button)
                {
                    ((Button)sp_object).IsEnabled = true;
                }
            }
            if (drawStep > 0)
            {
                drawStep = 0;
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
            }
            m_DrawCommands = DrawCommands.None;
            if (SelectedShapes.Count > 0)
            {
                foreach (Shape shape in SelectedShapes)
                {
                    shape.StrokeThickness -= 2;
                }
                SelectedShapes.Clear();
            }
        }

        private void Button_DrawLine_Click(object sender, RoutedEventArgs e)
        {
            drawStep = 0;
            if(m_DrawCommands != DrawCommands.Line)
            {
                m_DrawCommands = DrawCommands.Line;
                Button_DrawLine.IsEnabled = false;
            }
        }
        public SerializeElements ElementSerializer()
        {
            SerializeElements serializeElements = new SerializeElements();
            serializeElements.ScaleFactor = scaleFactor;
            foreach (object child in canvas.Children)
            {
                string child_type = child.GetType().ToString();
                switch (child_type)
                {
                    case "System.Windows.Controls.Image":
                        serializeElements.ImageSource = ((Image)child).Source.ToString();
                        break;
                    case "System.Windows.Shapes.Line":
                        if (serializeElements.SaveLines == null)
                        {
                            serializeElements.SaveLines = new List<SaveLine>();
                        }
                        SaveLine saveLine = DrawingUtils.Line_GetPoints((Line)child);
                        serializeElements.SaveLines.Add(saveLine);
                        break;
                    case "System.Windows.Shapes.Ellipse":
                        if (serializeElements.SaveCircles == null)
                        {
                            serializeElements.SaveCircles = new List<SaveCircle>();
                        }
                        SaveCircle saveCircle = DrawingUtils.Circle_GetPoints((Ellipse)child);
                        serializeElements.SaveCircles.Add(saveCircle);
                        break;
                }
            }
            return serializeElements;
        }
        private void SaveWorkSpace(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JSON (*.json) | *.json";
            dialog.Title = "Save File";
            bool? result = dialog.ShowDialog();
            SerializeElements serializeElements = ElementSerializer();
            string output = JsonSerializer.Serialize(serializeElements);
            if (result == true)
            {
                File.WriteAllText(dialog.FileName, output);
            }

        }
        private void SaveDXF(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "DXF (*.dxf) | *.dxf";
            dialog.Title = "Save DXF";
            bool? result = dialog.ShowDialog();
            SerializeElements serializeElements = ElementSerializer();
            string output = DXF_Out.Build_DXF(serializeElements);
            if (result == true)
            {
                File.WriteAllText(dialog.FileName, output);
            }

        }
        private void OnMouseEnterElement(object sender, MouseEventArgs e)
        {
            canvas.Cursor = Cursors.Pen;
        }
        private void OnElementMouseDown(object sender, MouseEventArgs e)
        {
            ((Shape)sender).StrokeThickness += 2;
            SelectedShapes.Add((Shape)sender);
            if (SelectedShapes.Count == 1)
            {
                ShapeProperties.ListView_AddShape((Shape)sender);
            }
            else
            {
                ShapeProperties.ClearList();
            }

        }
        private void Button_Erase_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedShapes.Count > 0)
            {
                foreach (Shape shape in SelectedShapes)
                {
                    canvas.Children.Remove(shape);
                }
                SelectedShapes.Clear();
            }
        }

        private void TextBox_ScaleFactor_TextChanged(object sender, EventArgs e)
        {
            double.TryParse(((TextBox)sender).Text, out scaleFactor);
        }

        private void Button_DrawCircle_Click(object sender, RoutedEventArgs e)
        {
            drawStep = 0;
            if (m_DrawCommands != DrawCommands.Circle)
            {
                m_DrawCommands = DrawCommands.Circle;
                Button_DrawCircle.IsEnabled = false;
            }
        }
        private void DrawCircle()
        {
            double radius = 0;
            radius = Math.Sqrt(Math.Pow(Math.Abs(endPoint.X-startPoint.X),2) + Math.Pow(Math.Abs(endPoint.Y-startPoint.Y),2));
            Ellipse ellipse = new Ellipse();
            Canvas.SetLeft(ellipse, startPoint.X-(radius));
            Canvas.SetTop(ellipse, startPoint.Y-(radius));
            ellipse.Height = radius*2;
            ellipse.Width = radius*2;
            ellipse.Stroke = Brushes.Black;
            ellipse.MouseEnter += OnMouseEnterElement;
            ellipse.MouseDown += OnElementMouseDown;
            canvas.Children.Add(ellipse);

        }
        private void Button_DrawArc_Click(object sender, RoutedEventArgs e)
        {
            drawStep = 0;
            if(m_DrawCommands != DrawCommands.Arc)
            {
                m_DrawCommands= DrawCommands.Line;
                Button_DrawArc.IsEnabled = false;
            }
        }
        private void DrawArc()
        {
            System.Windows.Shapes.Path arc = new System.Windows.Shapes.Path();
            DrawingUtils.Arc_SetPoints(arc, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, 1, 0, 0);
            arc.MouseEnter += OnMouseEnterElement;
            arc.MouseDown += OnElementMouseDown;
            canvas.Children.Add(arc);
        }
    }
}
