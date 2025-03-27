namespace PudelkoLibrary
{
    public enum UnitOfMeasure
    {
        milimeter = 1000,
        centimeter = 100,
        meter = 1
    }
    public class Pudelko
    {
        //private UnitOfMeasure _unitOfMeasure{get;set;}
        //public UnitOfMeasure unitOfMeasure { get => _unitOfMeasure; set => _unitOfMeasure = value; }
        private double x { get; set; }
        public double A { 
            get => Math.Round(x, 3);
            set{
                if (value > 10 || value < 0)
                    throw new ArgumentOutOfRangeException("Invalid data!");
                x = value;
            }
        }
        private double y { get; set; }
        public double B
        {
            get => y;
            set
            {
                if (value > 10 || value < 0)
                    throw new ArgumentOutOfRangeException("Invalid data!");
                y = value;
            }
        }
        private double z { get; set; }
        public double C
        {
            get => z;
            set
            {
                if (value > 10 || value < 0)
                    throw new ArgumentOutOfRangeException("Invalid data!");
                z = value;
            }
        }

        public Pudelko(double? a, double? b, double? c, UnitOfMeasure unitOfMeasure = UnitOfMeasure.meter) 
        {
            int converter = (int)unitOfMeasure;
            //this.unitOfMeasure = unitOfMeasure;

            if (a is null)
                A = 0.1f;
            else
                A = (double)a / converter;

            if (b is null)
                B = 0.1f;
            else
                B = (double)b / converter;

            if (c is null)
                C = 0.1f;
            else
                C = (double)c / converter;

        }

        public override string ToString()
        {
            return $"{A} m × {B} m × {C} m";
        }
        
        public string ToString(string format)
        {
            int converter = (int)UnitOfMeasure.meter;
            string shortFormat = "m";
            if (format == "centimeter")
            {
                converter = (int)UnitOfMeasure.centimeter;
                shortFormat = "cm";
            }
            else if (format == "milimeter")
            {
                converter = (int)UnitOfMeasure.milimeter;
                shortFormat = "mm";
            }
                

            double[] values = { A * converter, B * converter, C * converter };
            return $"{values[0]} {shortFormat} × {values[1]} {shortFormat} × {values[2]} {shortFormat}";
        }
    }
}
