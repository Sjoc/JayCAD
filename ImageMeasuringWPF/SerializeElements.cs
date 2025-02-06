using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

}
