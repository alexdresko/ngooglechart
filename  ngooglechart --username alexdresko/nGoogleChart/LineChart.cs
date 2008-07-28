using System;

namespace nGoogleChart
{
    public class LineChart : ChartBase
    {
        public LineChart(int width, int height) : base(width, height)
        {
        }

        public override bool Validate()
        {
            throw new System.NotImplementedException();
        }

        public override string GetChartType()
        {
            return "lc";

        }
    }
}