namespace Cirrious.MvvmCross.Interfaces.Services.Location
{
#warning TODO - need to expose some platform specific math libraries on coordinate calculations!
    public class MvxCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Accuracy { get; set; }
        public double AltitudeAccuracy { get; set; }
        public double Heading { get; set; }
        public double Speed { get; set; }
    }
}