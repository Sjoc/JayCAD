using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace ImageMeasuringWPF
{
    public class SerializeElements
    {
        public double ScaleFactor {  get; set; }
        public string? ImageSource { get; set; }
        public IList<SaveLine>? SaveLines { get; set; }
        public IList<SaveCircle>? SaveCircles { get; set; }

    }
    public class SaveLine
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public string? Color { get; set; }
    }
    public class SaveCircle
    {
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double Diameter { get; set; }
        public string? Color { get; set; }
    }
    public class AppCommands : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
    public static class DrawingUtils
    {
        public static SaveLine Line_GetPoints(Line line)
        {
            SaveLine saveLine = new SaveLine();
            saveLine.X1 = line.X1;
            saveLine.Y1 = line.Y1;
            saveLine.X2 = line.X2;
            saveLine.Y2 = line.Y2;
            saveLine.Color = line.Stroke.ToString();
            return saveLine;
        }
        public static SaveCircle Circle_GetPoints(Ellipse circle)
        {
            SaveCircle saveCircle = new SaveCircle();
            saveCircle.Color = ((Ellipse)circle).Stroke.ToString();
            saveCircle.Diameter = ((Ellipse)circle).Height;
            saveCircle.CenterX = Canvas.GetLeft(((Ellipse)circle)) + saveCircle.Diameter / 2;
            saveCircle.CenterY = Canvas.GetTop(((Ellipse)circle)) + saveCircle.Diameter / 2;
            return saveCircle;
        }
        public static void Circle_SetPoints(Ellipse circle, double centerX, double centerY, double radius)
        {
            Canvas.SetLeft(circle, centerX - (radius));
            Canvas.SetTop(circle, centerY - (radius));
            circle.Width = radius * 2;
            circle.Height = radius * 2;
        }
        public static void Arc_SetPoints(Path arc, double X1, double Y1, double X2, double Y2, double radius, double centerX, double centerY)
        {
            
        }

    }

}
