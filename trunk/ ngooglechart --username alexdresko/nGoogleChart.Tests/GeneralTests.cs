using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace nGoogleChart.Tests
{
    /// <summary>
    /// Summary description for GeneralTests
    /// </summary>
    [TestClass]
    public class GeneralTests
    {
        private TestContext testContextInstance;
        private List<string> Values;

        private const string _targetURL =
            "http://chart.apis.google.com/chart?chs=640x300&chd=t:60,40,80|4,46,46&cht=lc&chxl=0:|1|2|3|4|5|1:|0|2|3|4|5|2:|Years|3:|Experience&chxt=x,y,x,y&chg=20,20&chdl=MVC|ASP.NET&chco=ff0000,00ff00&chf=c,lg,45,ffffff,0,76A4FB,0.75|bg,s,EFEFEF&chtt=Site+visitors+by+month|January+to+July&chts=FF0000,20&chxp=3,50|2,50&chxs=3,0000dd,13|2,0000dd,13&chls=3,1,0|3,1,0";

        private static Chart GetLineChartForTesting()
        {
            var chart = new Chart(640, 300);
            chart.DataSets.Add(new ChartDataSet
            {
                Values = new ValueCollection
                                                    {
                                                        60,
                                                        40,
                                                        80
                                                    }
            });

            chart.DataSets.Add(new ChartDataSet
            {
                Values = new ValueCollection
                                                    {
                                                        4,
                                                        46,
                                                        46
                                                    }
            });

            chart.AxisLabels = new AxisLabelCollection()
                                   {
                                       new AxisLabel()
                                           {
                                               LabelLocation = LabelLocation.Right
                                           },
                                       new AxisLabel(),
                                       new AxisLabel()
                                           {
                                               LabelLocation = LabelLocation.Top,
                                               Values = new List<string>()
                                                            {
                                                                "Jan",
                                                                "Feb"
                                                            }
                                           },
                                       new AxisLabel()
                                           {
                                               LabelLocation = LabelLocation.Y,
                                               Values = new List<string>()
                                                            {
                                                                "Red",
                                                                "Blue",
                                                                "Green"
                                                            }
                                           }
                                   };

            return chart;
        }
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }



        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        [TestMethod]
        public void EncodingDefaultsToTextEncoding()
        {
            var chart = new Chart(3, 3);

            var url = chart.GetURL();

            var encoding = chart.Encoding;
            var formattedValue = URLHelper.GetStartOfChartDataSection(Encoding.TextEncoding);
            Assert.IsTrue(url.Contains(formattedValue));
        }

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod]
        public void ExceptionIfOnlyLengthOfLineSegment()
        {
            var chart = GetLineChartForTesting();
            chart.GridSettings.LengthOfLineSegment = 0;
            chart.GetURL();
        }

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod]
        public void ExceptionIfOnlyLengthOfBlankSegment()
        {
            var chart = GetLineChartForTesting();
            chart.GridSettings.LengthOfBlankSegment = 0;
            chart.GetURL();
        }

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod]
        public void ExceptionIfLengthOfLineSegmentNotSpecified()
        {
            var chart = GetLineChartForTesting();
            chart.GridSettings.LengthOfLineSegment = 0;
            chart.GridSettings.LengthOfBlankSegment = 0;
            chart.GetURL();
        }


        [ExpectedException(typeof(ApplicationException))]
        [TestMethod]
        public void ExceptionIfOnlyXAxisGridStepSize()
        {
            var chart = GetLineChartForTesting();
            chart.GridSettings.XAxisStepSize = 0;
            chart.GetURL();
        }

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod]
        public void ExceptionIfOnlyYAxisGridStepSize()
        {
            var chart = GetLineChartForTesting();
            chart.GridSettings.YAxisStepSize = 0;
            chart.GetURL();
        }

        [TestMethod]
        public void EncodingCanBeSetToExtended()
        {
            var chart = GetLineChartForTesting();
            chart.Encoding = Encoding.ExtendedEncoding;

            var url = chart.GetURL();

            var encoding = chart.Encoding;
            var formattedValue = URLHelper.GetStartOfChartDataSection(encoding);
            Assert.IsTrue(url.Contains(formattedValue));
        }

        [TestMethod]
        public void CanAddOneDataSet()
        {
            var chart = new Chart(640, 300);
            chart.DataSets.Add(new ChartDataSet
                                   {
                                       Values = new ValueCollection
                                                    {
                                                        60,
                                                        40,
                                                        80
                                                    }
                                   });

            var url = chart.GetURL();
            var data = URLHelper.GetCommaSeparatedValues(chart.DataSets[0].Values.Select(s => s.ToString()));


            Assert.IsTrue(url.Contains(data));
            Assert.IsTrue(url.Contains(GetTestableValue(URLHelper.GetFullDataSection(chart))));
        }

        [TestMethod]
        public void CanAddMultipleDataSets()
        {
            var chart = GetLineChartForTesting();

            var url = chart.GetURL();

            var combined = URLHelper.GetCombinedDataSection(chart);


            var dataSection = URLHelper.GetFullDataSection(chart);
            Assert.IsTrue(url.Contains(GetTestableValue(dataSection)));
        }

        private static string GetTestableValue(string dataSection)
        {
            return "&" + dataSection + "&";
        }


        [TestMethod]
        public void ChartSizeIsFormattedCorrectly()
        {
            var chart = GetLineChartForTesting();
            var sizeSection = URLHelper.GetFullSizeSection(chart);

            Assert.IsTrue(_targetURL.Contains(sizeSection));
        }

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod]
        public void ZeroWidthThrowsException()
        {
            var chart = GetLineChartForTesting();
            chart.Width = 0;

            chart.GetURL();
        }

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod]
        public void ZeroHeightThrowsException()
        {
            var chart = GetLineChartForTesting();
            chart.Height = 0;

            chart.GetURL();
        }

        [TestMethod]
        public void URLIsTestable()
        {
            var chart = GetLineChartForTesting();

            Assert.IsTrue(chart.BaseURL.EndsWith("?a=1&"));
            Assert.IsTrue(chart.GetURL().EndsWith("&b=1"));
        }

        [TestMethod]
        public void CanUseLineChartType()
        {
            var chart = GetLineChartForTesting();

            chart.ChartType = ChartType.LineChart;

            var fullChartSection = URLHelper.GetFullChartSection(chart);

            Assert.IsTrue(chart.GetURL().Contains(GetTestableValue(fullChartSection)));
            Assert.IsTrue(chart.GetURL().Contains("cht=lc"));
        }

        [TestMethod]
        public void CanTurnOnAxisLabels()
        {
            var chart = GetLineChartForTesting();

            var url = chart.GetURL();

            string fullAxisLabelSection = URLHelper.GetFullAxisEnableSection(chart);
            Assert.IsTrue(url.Contains(GetTestableValue(fullAxisLabelSection)));
        }

        [TestMethod]
        public void CanSetAxisLabels()
        {
            var chart = GetLineChartForTesting();

            string fullAxisLabelSection = URLHelper.GetFullAxisLabelSection(chart);

            var url = chart.GetURL();

            Assert.IsTrue(url.Contains(GetTestableValue(fullAxisLabelSection)));
            Assert.IsFalse(url.Contains("&&"));


        }

        [TestMethod]
        public void CanSetGridStepSizes()
        {
            var xAxisStepSize = 10.2;
            var yAxisStepSize = 13.2;
            var lengthOfBlankSegment = 5.1;
            var lengthOfLineSegment = 4.2;

            var chart = GetLineChartForTesting();
            chart.GridSettings.XAxisStepSize = xAxisStepSize;
            chart.GridSettings.YAxisStepSize = yAxisStepSize;

            var fullGridLineSection = URLHelper.GetFullGridLineSection(chart);

            var url = chart.GetURL();

            Assert.IsTrue(url.Contains(GetTestableValue(fullGridLineSection)));

            chart.GridSettings.LengthOfLineSegment = lengthOfLineSegment;
            chart.GridSettings.LengthOfBlankSegment = lengthOfBlankSegment;

            url = chart.GetURL();
            fullGridLineSection = URLHelper.GetFullGridLineSection(chart);


            Assert.IsTrue(url.Contains(GetTestableValue(fullGridLineSection)));
            
        }

        [TestMethod] public void GridSectionNotIncludedIfNoValuesSpecified()
        {
            var chart = GetLineChartForTesting();
            chart.GridSettings.YAxisStepSize = null;
            chart.GridSettings.XAxisStepSize = null;
            chart.GridSettings.LengthOfBlankSegment = null;
            chart.GridSettings.LengthOfLineSegment = null;

            var url = chart.GetURL();

            Assert.IsTrue(!url.Contains("&chg="));
        }
    }
}