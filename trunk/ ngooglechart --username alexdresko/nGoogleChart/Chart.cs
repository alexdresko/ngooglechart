using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nGoogleChart
{
    public class Chart
    {
        private const string _baseURL = "http://chart.apis.google.com/chart?a=1&";
        public string BaseURL
        {
            get { return _baseURL; }

        }

        public Chart(int width, int height)
        {
            _width = width;
            _height = height;
        }

        private int _width;
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private int _height;
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }


        public string GetURL()
        {
            return URLHelper.GetChartURL(this);
        }


        private DataSetCollection _dataSets = new DataSetCollection();
        private ChartType chartType = ChartType.LineChart;
        private AxisLabelCollection _axisLabels = new AxisLabelCollection();
        private GridSettings _gridSettings = new GridSettings();
        public GridSettings GridSettings
        {
            get { return _gridSettings; }
            set { _gridSettings = value; }
        }

        public AxisLabelCollection AxisLabels
        {
            get { return _axisLabels; }
            set { _axisLabels = value; }
        }

        public DataSetCollection DataSets
        {
            get { return _dataSets; }
            set { _dataSets = value; }
        }

        public Encoding Encoding { get; set; }

        public ChartType ChartType
        {
            get { return chartType; }
            set { chartType = value; }
        }
    }

    public enum ChartType
    {
        [StringValue("lc")]
        LineChart,
        [StringValue("bc")]
        BarChart
    }
}
