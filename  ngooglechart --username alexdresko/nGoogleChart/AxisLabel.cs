using System.Collections.Generic;

namespace nGoogleChart
{
    public class AxisLabel
    {
        private LabelLocation _labelLocation = LabelLocation.X;
        private List<string> _values = new List<string>();
        public List<string> Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public LabelLocation LabelLocation
        {
            get { return _labelLocation; }
            set { _labelLocation = value; }
        }


    }
}