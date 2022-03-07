using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using xPowerPhoneApp.Models;

namespace xPowerPhoneApp.Factorys
{
    /// <summary>
    /// A Factory class to make charts from microcharts
    /// </summary>
    internal static class ChartFactory
    {
        /// <summary>
        /// Creates a PointChart with data that was given
        /// </summary>
        /// <param name="PowerUsages">The data that will be shown</param>
        /// <param name="Weekly">If it should take the weekday name or hour</param>
        /// <returns>A LineChart</returns>
        static public Chart CreatePointedChart(List<PowerUsage> PowerUsages, bool Weekly)
        {
            var min = PowerUsages.Min(x => x.WattHour);
            if (Weekly)
            {
                var entries = ConvertPowerUsageToWeekly(PowerUsages);

                return new PointChart
                {
                    Entries = entries,
                    PointSize = 64,
                    PointAreaAlpha = 255,
                    ValueLabelOrientation = Orientation.Vertical,
                    BackgroundColor = SKColors.Transparent,
                    LabelTextSize = 64,
                    Margin = 30,
                    MinValue = (float)min / 2
                };
            }
            else
            {
                var entries = ConvertPowerUsageToDaily(PowerUsages);
                return new PointChart
                {
                    Entries = entries,
                    PointSize = 32,
                    PointAreaAlpha = 255,
                    ValueLabelOrientation = Orientation.Vertical,
                    BackgroundColor = SKColors.Transparent,
                    LabelTextSize = 32,
                    Margin = 30,
                    MinValue = (float)min / 2
                };
            }
        }

        /// <summary>
        /// Creates a LineChart with data that was given
        /// </summary>
        /// <param name="PowerUsages">The data that will be shown</param>
        /// <param name="Weekly">If it should take the weekday name or hour</param>
        /// <returns>A LineChart</returns>
        static public Chart CreateLineChart(List<PowerUsage> PowerUsages, bool Weekly)
        {
            var min = PowerUsages.Count > 0 ? PowerUsages.Min(x => x.WattHour) : 0;
            if (Weekly)
            {
                var entries = ConvertPowerUsageToWeekly(PowerUsages);

                return new LineChart
                {
                    Entries = entries,
                    LineMode = LineMode.Straight,
                    ValueLabelOrientation = Orientation.Vertical,
                    BackgroundColor = SKColors.Transparent,
                    LabelTextSize = 64,
                    PointSize = 16,
                    Margin = 30,
                    MinValue = (float)min / 2
                };
            }
            else
            {
                var entries = ConvertPowerUsageToDaily(PowerUsages);
                return new LineChart
                {
                    Entries = entries,
                    ValueLabelOrientation = Orientation.Vertical,
                    BackgroundColor = SKColors.Transparent,
                    LabelOrientation = Orientation.Horizontal,
                    LabelTextSize = 32,
                    PointSize = 16,
                };
            }
        }

        private static ChartEntry[] ConvertPowerUsageToWeekly(List<PowerUsage> PowerUsages)
        {
            var entries = new List<ChartEntry>();

            foreach (var powerUsage in PowerUsages)
            {

                entries.Add(new ChartEntry((float)powerUsage.WattHour)
                {
                    Label = powerUsage.Taken.ToString("dddd", new CultureInfo("da-DK")),
                    ValueLabel = powerUsage.ToString(),
                    Color = SKColor.Parse("#6fa8dc")
                });
            }

            return entries.ToArray();
        }
        private static ChartEntry[] ConvertPowerUsageToDaily(List<PowerUsage> PowerUsages)
        {
            var entries = new List<ChartEntry>();

            foreach (var powerUsage in PowerUsages)
            {

                entries.Add(new ChartEntry((float)powerUsage.WattHour)
                {
                    Label = powerUsage.Taken.Hour%2 == 0 ? powerUsage.Taken.ToString("HH", new CultureInfo("da-DK")) : "",
                    ValueLabel = powerUsage.ToString(),
                    Color = SKColor.Parse("#6fa8dc")
                });
            }

            return entries.ToArray();
        }
    }
}
