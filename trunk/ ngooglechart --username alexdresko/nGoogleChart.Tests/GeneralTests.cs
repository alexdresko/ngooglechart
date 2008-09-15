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
                                                        new ValueInformation()
                                                            {
                                                                Value = 60
                                                            },
                                                        new ValueInformation()
                                                            {
                                                                Value = 40
                                                            },
                                                        new ValueInformation()
                                                            {
                                                                Value = 80
                                                            }
                                                    }
            });

            chart.DataSets.Add(new ChartDataSet
            {
                Values = new ValueCollection
                                                    {
                                                          new ValueInformation()
                                                            {
                                                                Value = 4
                                                            },
                                                        new ValueInformation()
                                                            {
                                                                Value = 46
                                                            },
                                                        new ValueInformation()
                                                            {
                                                                Value = 46
                                                            }
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
                                                        new ValueInformation()
                                                            {
                                                                Value = 60
                                                            },
                                                            new ValueInformation()
                                                            {
                                                                Value = 40
                                                            },
                                                            new ValueInformation()
                                                            {
                                                                Value = 80
                                                            }
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

        [TestMethod]
        public void GridSectionNotIncludedIfNoValuesSpecified()
        {
            var chart = GetLineChartForTesting();
            chart.GridSettings.YAxisStepSize = null;
            chart.GridSettings.XAxisStepSize = null;
            chart.GridSettings.LengthOfBlankSegment = null;
            chart.GridSettings.LengthOfLineSegment = null;

            var url = chart.GetURL();

            Assert.IsTrue(!url.Contains("&chg="));
        }

        [TestMethod]
        public void TextEncodingErrorsIfValueGreaterThan100()
        {
            var chart = GetLineChartForTesting();
            chart.DataSets.Add(new ChartDataSet()
                                   {
                                       Values = new ValueCollection()
                                                    {
                                                        new ValueInformation()
                                                            {
                                                                Value = 13
                                                            },
                                                            new ValueInformation()
                                                            {
                                                                Value = 101
                                                            },
                                                            new ValueInformation()
                                                            {
                                                                Value = 131
                                                            }
                                                    }
                                   });


            if (chart.Encoding == Encoding.TextEncoding)
            {
                var maxValue = 100;
                var asdf = from a in chart.DataSets from b in a.Values where b.Value > maxValue select b;

                if (asdf.Count() > 0)
                {
                    // 9/12/2008 9:41:37 PM by AD: throw exception? Might be overkill. Don't know what google does if you supply a value that is too large. Need to find out. 
                }
            }

            Assert.Inconclusive();
        }



        [TestMethod]
        public void SimpleEncodingDataGetsEncoded()
        {


            var chart = GetLineChartForTesting();
            chart.Encoding = Encoding.SimpleEncoding;
            var values = new ValueCollection();
            for (var i = 0; i < 63; i++)
            {
                values.Add(new ValueInformation()
                               {
                                   Value = i
                               });
            }

            chart.DataSets = new DataSetCollection()
                                 {
                                     new ChartDataSet()
                                         {
                                             Values = values
                                         }
                                 };



            var expectedValue = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";



            var url = chart.GetURL();


            Assert.IsTrue(url.Contains(GetTestableValue(expectedValue)));

        }

        [TestMethod]
        public void TextEncodingUsesPipeSeparator()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void TextEncodingRequiresSameLengthDataSets()
        {
            // 9/12/2008 9:56:43 PM by AD: I'm not entierly sure this is true, but the docuemtation
            // states that missing values should use a -1 value. Need to experiment. 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void MapChartTypeDoesNotAllowTEWDS()
        {
            // 9/12/2008 10:01:02 PM by AD: Not sure if I should throw an error here.Docuemtnation states "Note this is not available for maps."
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SimpleEncodingDoesNotAllowDataOver62()
        {
            // 9/12/2008 10:04:15 PM by AD: To error or not. That is the question
            Assert.Inconclusive();
        }



        [TestMethod]
        public void ExtendedEncodingDoesNotAllowDataOver4096()
        {
            // 9/12/2008 10:16:20 PM by AD: Experiment
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ExtendedEncodingEncodesData()
        {

            // 9/12/2008 10:17:25 PM by AD: Need to find someone else's code that does the encoding
            Assert.Inconclusive();
        }

        [TestMethod]
        public void MinimumValueIsWrittenWithTEWDS()
        {
            // 9/12/2008 10:23:30 PM by AD: Minimum appears to be required if Max is specified according to the vague documentation.
            Assert.Inconclusive();
        }

        [TestMethod]
        public void MinimumIsRequiredIfMaximumIsSpecifiedWithTEWDS()
        {
            // 9/12/2008 10:23:30 PM by AD: Minimum appears to be required if Max is specified according to the vague documentation.
            Assert.Inconclusive();

        }

        [TestMethod]
        public void TEWDSUsesPipeSeparatorForDataSets()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SimpleEncodingUsesCommaSeparatorForDataSets()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SimpleEncodingUsesUnderscoreForMissingValues()
        {
            // 9/12/2008 10:44:55 PM by AD: I'm not sure what a missing value constitutes. Try to figure out by examining the javascript encoder. 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ExtendedEncodingUsesTwoUnderscoresForMissingValues()
        {
            // 9/12/2008 10:44:55 PM by AD: I'm not sure what a missing value constitutes. Try to figure out by examining the javascript encoder. 
            Assert.Inconclusive();

        }

        [TestMethod]
        public void ExtendedEncodingUsesCommaSeparatorForDataSets()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void lxyChartTypeRequiresAndEvenNumberOfDataSets()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void lcChartTypeCanBeSet()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void lxyChartTypeCanBeSet()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void lsChartTypeCanBeSet()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void bhsChartTypeCanBeSet()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void bvsChartTypeCanBeSet()
        {
            Assert.Inconclusive();
        }

        [TestMethod] public void ColorCanBeSetPerPoint()
        {

            Assert.Inconclusive();
        }

        [TestMethod] public void bhgChartTypeCanBeSet()
        {
            Assert.Inconclusive();
        }

        [TestMethod] public void bvgChartTypeCanBeSet()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void chbhChartTypeCanBeSet()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void BarChartBarWidthCanBeSet()
        {
            Assert.Inconclusive();
        }

        [TestMethod] public void BarChartSpaceBetweenBarsInAGroupCanBeSet()
        {
            Assert.Inconclusive();
        }

        [TestMethod] public void BarChartOptionalSpaceBetweenGroups()
        {
            Assert.Inconclusive();
        }

        [TestMethod] public void BarChartOptionalSpaceBetweenGroupsIsRequiredIfSpaceBetweenBarsInAGroupIsSet()
        {
            Assert.Inconclusive();    
        }

        [TestMethod] public void pChartTypeCanBeSet()
        {
            Assert.Inconclusive();
        }

        [TestMethod] public void p3ChartTypeCanBeSet()
        {
            Assert.Inconclusive();
        }

        [TestMethod] public void vChartTypeCanBeSpecified()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void VennDiagramDoesNotAllowMoreThanOneDataSet()
        {
            // 9/13/2008 12:27:36 AM by AD: ???
            Assert.Inconclusive();
        }

        [TestMethod]
        public void VennDiagramDataSetRequiresSevenValues()
        {
            // 9/13/2008 12:46:01 AM by AD: appears to be the case
            Assert.Inconclusive();
        }

        [TestMethod]
        public void sChartTypeCanBeSet()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void ScatterPlotRequiresTwoDataSets()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void ScatterPlotDotSizeCanBeSet()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void ScatterPlotPointShapesCanBeChanged()
        {
            // 9/13/2008 12:50:43 AM by AD: as of right now, there is no Shape property on the ValueInformation object. 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void chmParameterCanBeUsedToScaleScatterPlotPoints()
        {
            // 9/13/2008 12:55:00 AM by AD: I don't really know what the chm property does off hand.
            Assert.Inconclusive();
        }

        [TestMethod]
        public void rChartTypeCanBeSet()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void rsChartTypeCanBeSet()
        {

            Assert.Inconclusive();
        }


        [TestMethod]
        public void tChartTypeCanBeSet()
        {
                        
            Assert.Inconclusive();
        }

        [TestMethod]
		public void chtmCanBeUsedToGetGeographicalArea()
		{

			Assert.Inconclusive();
		}

        [TestMethod]
        public void TestCanReturnImageObject()
        {
            // 9/13/2008 10:47:20 AM by AD: I think it's possible to create a GetImage() function that wraps HTTP request call out to the google server. 
            // The data can be read in and loaded into a standard System.Drawing.Image object to be saved anywhere. 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void chcoCanBeUsedToSetDefaultMapColor()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void chcoCanBeUsedtoSetMapGradients()
        {
            
            Assert.Inconclusive();
        }

        [TestMethod]
        public void chldSetsCodesForEachCountryToBeColored()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void chldSetsCodesForEachStateToBeColored()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void chdSetsValuesForEachCountryToBeColored()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void chdSetsValuesForEachStateToBeColored()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void SimpleEncodingEncodesAllPossibleNumericValues()
        {
            // 9/13/2008 11:01:40 AM by AD: I think I can test this by full populating a chart object with data using Simple encoding, using the number 1 in all cases. 
            // Examining the url should not have any 1's in the querystring. 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ExtendedEncodingEncodesAllPossibleNumericValues()
        {
            // 9/13/2008 11:01:40 AM by AD: I think I can test this by full populating a chart object with data using Simple encoding, using the number 1 in all cases. 
            // Examining the url should not have any 1's in the querystring. 
            
            Assert.Inconclusive();
        }

        [TestMethod]
        public void HelperMethodsPreventAccidentalValidationErrors()
        {
            // 9/13/2008 11:09:47 AM by AD: This test is more or less a reminder that I want to create these functions where necessary. 
            // Anywhere I have a property that contains more than one value in a set and some of those values are optional, I should create overloaded
            // helper functions to ensure trouble free chart population. I DO need to test these functions, but I don't know all of the functions I need
            // to create off the top of my head. The MAP might be a good place to start. At the moment, I cannot decide what tests to write for the map
            // or even how the object model should look without further investigating chco and chf which might be used differently in other chart types. 
            // Thinking through this a bit more, I think maybe the generic properties that are used differently by different chart types can be stored 
            // at the top level of the object model and the helper methods can be used to populate them properly. This prevents me from having to store
            // colors and other options in multiple places throughout the object model. This technique might also be easier for those who are used to working
            // with the google chart api and might be easier to populate in cases where several chart types are being used with a single chart object.  On the flip side, it doesn't produce a very obvious object model. It might not even be posssible to switch between some chart types that have different data requirements. Consider also that this choice 
            // may affect querystring trimming (only rendering necessary data to the querystring), and the choice is obvious...... SIKE! :)
            Assert.Inconclusive();
        }

        [TestMethod]
        public void gomChartTypeCanBeSet()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void qrChartTypeCanBeSet()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void chlSetsTextToEncodeInQRChart()
        {
            
            Assert.Inconclusive();
        }

        [TestMethod]
        public void chlIsURLEncodedUsingUTF8ForQRCharts()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void QRChartOutputEncodingCanBeSetToUTF8()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void QRChartOutputEncodingCanBeSetToShift_JIS()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void QRChartOutputEncodingCanBeSetToISO88591()
        {
            
            Assert.Inconclusive();
        }

        [TestMethod]
        public void MarginDefaultsTo4IfECLevelSetInQRChart()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void ECLevelCanBeSetToLInQRChart()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void ECLevelCanBeSetToMInQRChart()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void ECLevelCanBeSetToQInQRChart()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void ECLevelCanBeSetToHInQRChart()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void ECLevelDefaultstoLIfMarginSet()
        {
            // 9/13/2008 11:36:40 AM by AD: a helper method might avoid this scenario. At this point, I may want to decide if I want to completely
            // prevent users from misuing the object model by not using the helper methods. I think it would make understanding the object model easier. 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void QRChartMarginMustBeAtLeast4()
        {
            // 9/13/2008 11:38:48 AM by AD: this, aparently, is a requirement of all QR chart readers. 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void LineChartDataSetsAreSortedHighestFirst()
        {
            // 9/13/2008 12:10:45 PM by AD: I think this can be done with a little Linq by sorting on the aggregate total of all values in each
            // DataSet.
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ColorsMustBeSixCharacters()
        {
            // 9/13/2008 4:14:55 PM by AD: I don't know how to enforce this yet..or if I even though. I'll bet money a chart will render with
            // SOME color even if it's specified with six characters.  
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TransparencyCanBeSpecifiedWithColor()
        {
            // 9/13/2008 4:16:03 PM by AD: This is a great spot for a helper method or construtor. I would also like to be able to enter 0-255 instead
            // of hexidecimal information. Just have to figure out how to convert the two. 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void chcoSupportMultipleColors()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void MustSpecifyEitherGlobalColorsOrPerPointColors()
        {
            // 9/13/2008 7:14:13 PM by AD: one is separated with |, and the other is separated with commas. I can't see how these two methods can be used 
            // together with the chco parameter. This, I believe, is worthy of an exception.
            Assert.Inconclusive();
        }

        [TestMethod]
        public void chm_bCanBeSetWithDataSetFillColorProperty()
        {
            // 9/13/2008 7:28:57 PM by AD: Now that I understand how the fill color parameter works, it seems I have an easy solution. 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void chm_BIsUsedIfOnlyOneDataSetAndUsingFillColor()
        {
            // 9/13/2008 7:31:07 PM by AD: Frickin easy. 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CanSetBackgroundSolidFill()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void CanSetChartSolidFill()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void CanStackMultipleFillTypes()
        {
            // 9/13/2008 7:47:29 PM by AD: I don't really know how many fill types can be stacked or what types of fill types can be stacked. 
            // Should experiment. 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void LinearGradientCanBeAllpliedToBackground()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void LinearGradientCanBeAppliedToChart()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void LinearStripesCanBeAppliedToBackground()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void LinearStripesCanBeAppliedToChart()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void SpacesAreReplacedWithPlusSignInChtt()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void LineBreaksAreReplacedWithPipesInChtt()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void chttCanBeUsedToSetChartTitle()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void chtsSetsChartTitleAndFont()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void chdlSetsLegend()
        {
            // 9/13/2008 8:45:49 PM by AD: Implement as Title property on DataSet
            Assert.Inconclusive();
        }

        [TestMethod]
        public void LengendCanBePlacedAtTop()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void LengendCanBePlacedAtBottom()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void LengendCanBePlacedAtLeft()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void LengendCanBePlacedAtRight()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void chlSetsPieChartLabels()
        {
            // 9/13/2008 8:52:32 PM by AD: Implemented as Title property on DataSet. For pie charts, I think if one value is specified, they all have
            // to be specified. Hence the rule to include missing values with double pipes.
            Assert.Inconclusive();
        }

        [TestMethod]
        public void chlUsesDoublePipesForMissingValues()
        {
            
            Assert.Inconclusive();
        }

        [TestMethod]
        public void chlSetsGoogleOMeterArrowLabel()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void CanAddXAxis()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void CanAddYAxis()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void CanAddTAxis()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void CanAddRAxis()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void CanAddMultipleAxis()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void AxisLabelsCanBeSet()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void AxisLabelIndexesCanBeSorted()
        {
            // 9/13/2008 9:04:32 PM by AD: this is a requirement of the API. 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DoublePipesAreUsedForMissingAxisLabels()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void HexFunctionsWork()
        {

            var a = ExtensionMethods.IntToHex(1);

            Assert.IsTrue(a == "01");
            var b = 255.IntToHex();
            Assert.IsTrue(b == "ff");
            var c = 0.IntToHex();
            Assert.IsTrue(c == "00");
            var d = "AA".HexToInt();
            Assert.IsTrue(d == 170);
            var e = "FF".HexToInt();
            Assert.IsTrue(e == 255);

        }

        [TestMethod]
        public void AxisLabelPositionCanBeSet()
        {
            // 9/14/2008 9:06:44 PM by AD: When it comes time to add support for Axis labels, I should make sure that the
            // chart's AxisLabels collection object contains .Location, and a collection of labels containing .Position, .Text.
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AxisRangCanBeSpecified()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void MultipleAxisRangesCanbeSpecified()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void AxisRangesAndAxisLabelsAreMutuallyExclusive()
        {
            // 9/14/2008 9:15:12 PM by AD: Maybe my AxisLabels property could accept two types of AxisObjects..1) A label based object. 2) 
            // A range based object? 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AxisColorCanBeSet()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void AxisColorAndFontSizeCanBeSet()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void AxisColorFontSizeAndAlignmentCanBeSet()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void MultipleAxisCanContainStyles()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void BarChartBarWidthCanBeSet()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void chpSetBarChartZeroLine()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void chpCanSetMultipleBarChartZeroLines()
        {
            
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TEWDSCanBeUsedToSetBarChartZeroLine()
        {

            Assert.Inconclusive();
        }

        [TestMethod]
        public void chlsSetsLineStyle()
        {
            
            Assert.Inconclusive();
        }

        [TestMethod]
        public void chlsCanSetMultipleLineStyles()
        {

            Assert.Inconclusive();
        }




    }





}