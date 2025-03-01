using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ImageMeasuringWPF
{
    public enum Horizontality
    {
        Horizontal,
        Vertical,
        Slope
    }
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
        public static void Arc_SetPoints(Path arc, double x1, double y1, double x2, double y2, double radius, double centerX, double centerY)
        {
            ArcSegment arcSegment = new ArcSegment();
            Point startPoint = new Point(0, 0);
            Point arcsegPoint = new Point();
            Size arcSecSize = new Size();
            arcSecSize.Height = radius;
            arcSecSize.Width = radius;
            arcsegPoint.X = Math.Abs(x1 - x2);
            arcsegPoint.Y = Math.Abs(y1 - y2);
            arcSegment.Point = arcsegPoint;
            arcSegment.Size = arcSecSize;
            Point midPoint = Line_GetMidPoint(x1,y1,x2,y2);
            if (centerY > midPoint.Y)
            {
                arcSegment.SweepDirection = SweepDirection.Counterclockwise;
            }
            else if (centerY < midPoint.Y)
            {
                arcSegment.SweepDirection = SweepDirection.Clockwise;
            }
            else
            {
                if (centerX < midPoint.X)
                {
                    arcSegment.SweepDirection = SweepDirection.Counterclockwise;
                }
                else
                {
                    arcSegment.SweepDirection= SweepDirection.Clockwise;
                }
            }            
            IEnumerable<PathSegment> segments = new PathSegment[] { arcSegment };
            PathGeometry arcGeometry = new PathGeometry();
            PathFigure arcFigure = new PathFigure(startPoint, segments, false);
            arcGeometry.Figures.Add(arcFigure);
            arc.Data = arcGeometry;
        }
        public static Point Line_GetMidPoint(double x1, double y1, double x2, double y2)
        {
            double x_dist = x1 - x2;
            double y_dist = y1 - y2;
            double midX = x1+.5*x_dist;
            double midY = y1+.5*y_dist;
            return new Point(midX, midY);

        }
        public static Horizontality CheckHorizontal(double x1, double y1, double x2, double y2)
        {
            Horizontality val = Horizontality.Slope;
            if (x1 == x2)
            {
                val = Horizontality.Vertical;
            }
            else if (y1 == y2)
            {
                val = Horizontality.Horizontal;
            }
            return val;
        }
        public static double Line_GetSlope(double x1, double y1, double x2, double y2)
        {
            double val = 0;
            val = (y2 - y1) / (x2 - x1);
            return val;
        }

    }

}
