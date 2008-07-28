using System;

namespace nGoogleChart
{
    public class GridSettings
    {
        public double? YAxisStepSize { get; set; }
        public double? LengthOfLineSegment { get; set; }
        public double? LengthOfBlankSegment { get; set; }
        public double? XAxisStepSize { get; set; }



        public void ConfigureGrid(double? xAxisStepSize, double? yAxisStepSize, double? lengthOfLineSegment, double? lengthOfBlankSegment)
        {


            YAxisStepSize = yAxisStepSize;
            LengthOfLineSegment = lengthOfLineSegment;
            LengthOfBlankSegment = lengthOfBlankSegment;
            XAxisStepSize = xAxisStepSize;
        }
        
        public void ConfigureGrid(double? xAxisStepSize, double? yAxisStepSize)
        {

            ConfigureGrid(xAxisStepSize, yAxisStepSize, null, null);
        }
    }
}