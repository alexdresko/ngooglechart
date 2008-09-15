using System;
using System.Collections.Generic;
using System.Linq;

namespace nGoogleChart
{
    public class URLHelper
    {
        public static string GetCombinedParameters(List<string> parameters)
        {
            return string.Join("&", parameters.Where(p => p.Trim().Length > 0).ToArray());
        }

        //public static string GetDataSection(IEnumerable<string> values, Encoding encoding)
        //{
        //    string commaValues = GetCommaSeparatedValues(values);
        //    return GetStartOfChartDataSection(encoding) + commaValues;
        //}

        public static string GetStartOfChartDataSection(Encoding encoding)
        {
            return string.Format("chd={0}:", encoding.GetStringValue());
        }

        public static string GetCommaSeparatedValues(IEnumerable<string> values)
        {
            return String.Join(",", values.ToArray());
        }

        public static string GetCombinedDataSection(Chart chart)
        {
            var datasets = new List<string>();
            foreach (var ds in chart.DataSets)
            {
                datasets.Add(GetCommaSeparatedValues(ds.Values.Select(p => p.ToString())));
            }

            string separator = GetSeparator(chart);

            return string.Join(separator, datasets.ToArray());
        }

        private static string GetSeparator(Chart chart)
        {
            var separator = "";

            if (chart.Encoding == Encoding.TextEncoding)
                separator = ",";
            return separator;
        }

        public static string GetFullDataSection(Chart chart)
        {
            return GetStartOfChartDataSection(chart.Encoding) + GetCombinedDataSection(chart);
        }

        public static string GetFullSizeSection(Chart chart)
        {
            return string.Format("chs={0}x{1}", chart.Width, chart.Height);
        }

        public static string GetChartURL(Chart chart)
        {
            Validate(chart);

            var parameters = new List<string>
                                 {
                                     GetFullSizeSection(chart),
                                     GetFullDataSection(chart),
                                     (GetFullChartSection(chart)),
                                     GetFullChartSection(chart),
                                     GetFullAxisEnableSection(chart),
                                     GetFullAxisLabelSection(chart),
                                     GetFullGridLineSection(chart),
                                     "b=1"
                                 };
            var combinedParameters = GetCombinedParameters(parameters);
            var fullURL = chart.BaseURL + combinedParameters;
            return fullURL;
        }

        private static void Validate(Chart chart)
        {
            XAndYAxisStepSizeMustBothBeSpecifiedIfEitherIsSpecified(chart);
            LengthOfLineSegmentAndLengthOfBlankSegmentMustBothBeSpecifiedIfEitherIsSpecified(chart);
            XAndYAxisStepSizeMustBeSpecifiedIfLengthIsSpecified(chart);
            WidthAndHeightAreValid(chart);
        }

        private static void XAndYAxisStepSizeMustBothBeSpecifiedIfEitherIsSpecified(Chart chart)
        {
            if ((!chart.GridSettings.XAxisStepSize.HasValue && chart.GridSettings.YAxisStepSize.HasValue)
                || (!chart.GridSettings.YAxisStepSize.HasValue && chart.GridSettings.XAxisStepSize.HasValue))
                throw new ApplicationException("X and Y axis step sizes must both be specified.");
        }

        private static void LengthOfLineSegmentAndLengthOfBlankSegmentMustBothBeSpecifiedIfEitherIsSpecified(Chart chart)
        {
            if ((!chart.GridSettings.LengthOfBlankSegment.HasValue && chart.GridSettings.LengthOfLineSegment.HasValue)
                ||
                (!chart.GridSettings.LengthOfLineSegment.HasValue && chart.GridSettings.LengthOfBlankSegment.HasValue))
                throw new ApplicationException(
                    "LengthOfLineSegment and LengthOfBlank must both be specified if either is specified.");
        }

        private static void XAndYAxisStepSizeMustBeSpecifiedIfLengthIsSpecified(Chart chart)
        {
            if ((chart.GridSettings.LengthOfBlankSegment.HasValue || chart.GridSettings.LengthOfLineSegment.HasValue) &&
                (!chart.GridSettings.XAxisStepSize.HasValue || !chart.GridSettings.YAxisStepSize.HasValue))
            {
                throw new ApplicationException(
                    "X and Y axis step sizes must be specified if line segment length is specified.");
            }
        }

        private static void WidthAndHeightAreValid(Chart chart)
        {
            if (chart.Width == 0 || chart.Height == 0)
            {
                throw new ApplicationException("Width and Height properties must be greater than zero.");
            }
        }

        public static string GetFullChartSection(Chart chart)
        {
            return string.Format("cht={0}", chart.ChartType.GetStringValue());
        }

        public static string GetFullAxisEnableSection(Chart chart)
        {
            var combinedAxisEnableSection = String.Join(",",
                                                        chart.AxisLabels.Select(p => p.LabelLocation.GetStringValue()).
                                                            ToArray());

            return string.Format("chxt={0}", combinedAxisEnableSection);
        }

        public static string GetFullAxisLabelSection(Chart chart)
        {
            var axisValueList = GetAxisLabelValueList(chart);

            var fullAxisLabelSection = string.Empty;

            
            if (axisValueList.Count > 0)
                fullAxisLabelSection = "chxl=" + string.Join("|", axisValueList.ToArray());

            return fullAxisLabelSection;

        }

        private static List<string> GetAxisLabelValueList(Chart chart)
        {
            var axisValueList = new List<string>();

            for (var axisLabelIndex = 0; axisLabelIndex < chart.AxisLabels.Count; axisLabelIndex++)
            {
                var axisLabel = chart.AxisLabels[axisLabelIndex];
                if (axisLabel.Values.Count > 0)
                {
                    var combinedValues = string.Join("|", axisLabel.Values.ToArray());
                    axisValueList.Add(string.Format("{0}:|{1}", axisLabelIndex, combinedValues));
                }
            }
            return axisValueList;
        }


        public static string GetFullGridLineSection(Chart chart)
        {
            var stepSize = string.Empty;
            if (chart.GridSettings.XAxisStepSize.HasValue && chart.GridSettings.YAxisStepSize.HasValue)
                stepSize = chart.GridSettings.XAxisStepSize + "," + chart.GridSettings.YAxisStepSize;

            if (chart.GridSettings.LengthOfLineSegment.HasValue && chart.GridSettings.LengthOfBlankSegment.HasValue)
                stepSize += "," + chart.GridSettings.LengthOfLineSegment + "," + chart.GridSettings.LengthOfBlankSegment;

            if (stepSize.Length > 0)
                stepSize = "chg=" + stepSize;

            return stepSize;
        }
    }
}