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
        /// <param name="powerUsages">The data that will be shown</param>
        /// <param name="Weekly">If it should take the weekday name or hour</param>
        /// <returns>A LineChart</returns>
        static public Chart CreatePointedChart(List<PowerUsage> powerUsages, bool Weekly, bool showText = true)
        {
            var min = powerUsages.Min(x => x.WattHour);
            if (Weekly)
            {
                var entries = ConvertPowerUsageToWeekly(powerUsages, showText);

                return new PointChart
                {
                    Entries = entries,
                    PointSize = 64,
                    ValueLabelOrientation = Orientation.Vertical,
                    BackgroundColor = SKColors.Transparent,
                    LabelTextSize = 64,
                    Margin = 30,
                    MinValue = (float)min / 2
                };
            }
            else
            {
                var entries = ConvertPowerUsageToDaily(powerUsages, showText);
                return new PointChart
                {
                    Entries = entries,
                    PointSize = 32,
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
        /// <param name="powerUsages">The data that will be shown</param>
        /// <param name="Weekly">If it should take the weekday name or hour</param>
        /// <returns>A LineChart</returns>
        static public Chart CreateLineChart(List<PowerUsage> powerUsages, bool Weekly, bool showText = true)
        {
            var min = powerUsages.Count > 0 ? powerUsages.Min(x => x.WattHour) : 0;
            if (Weekly)
            {
                var entries = ConvertPowerUsageToWeekly(powerUsages, showText);

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
                var entries = ConvertPowerUsageToDaily(powerUsages, showText);
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

        /// <summary>
        /// Creates a LineChart with data that was given
        /// </summary>
        /// <param name="powerPrices">The data that will be shown</param>
        /// <returns>A LineChart</returns>
        static public Chart CreateLineChart(List<PowerPrice> powerPrices)
        {
            var min = powerPrices.Count > 0 ? powerPrices.Min(x => x.MWhPrice) : 0;
            var entries = ConvertPowerUsageToDaily(powerPrices);
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

        private static ChartEntry[] ConvertPowerUsageToWeekly(List<PowerUsage> powerUsages, bool showText)
        {
            var entries = new List<ChartEntry>();

            foreach (var powerUsage in powerUsages)
            {
                if (showText)
                    entries.Add(new ChartEntry((float)powerUsage.WattHour)
                    {
                        Label = powerUsage.Taken.ToString("dddd", new CultureInfo("da-DK")),
                        ValueLabel = powerUsage.ToString(),
                        Color = SKColor.Parse("#6fa8dc")
                    });
                else
                    entries.Add(new ChartEntry((float)powerUsage.WattHour)
                    {
                        Color = SKColor.Parse("#6fa8dc")
                    });
            }

            return entries.ToArray();
        }
        private static ChartEntry[] ConvertPowerUsageToDaily(List<PowerUsage> powerUsages, bool showText)
        {
            var entries = new List<ChartEntry>();

            foreach (var powerUsage in powerUsages)
            {
                if (showText)
                    entries.Add(new ChartEntry((float)powerUsage.WattHour)
                    {
                        Label = powerUsage.Taken.Hour % 2 == 0 ? powerUsage.Taken.ToString("HH", new CultureInfo("da-DK")) : "",
                        ValueLabel = powerUsage.ToString(),
                        Color = SKColor.Parse("#6fa8dc")
                    });
                else
                    entries.Add(new ChartEntry((float)powerUsage.WattHour)
                    {
                        Color = SKColor.Parse("#6fa8dc")
                    });
            }

            return entries.ToArray();
        }
        private static ChartEntry[] ConvertPowerUsageToDaily(List<PowerPrice> powerPrices)
        {
            var entries = new List<ChartEntry>();

            foreach (var powerPrice in powerPrices)
            {

                entries.Add(new ChartEntry((float)powerPrice.MWhPrice)
                {
                    Label = powerPrice.Hour.Hour%2 == 0 ? powerPrice.Hour.ToString("HH", new CultureInfo("da-DK")) : "",
                    ValueLabel = powerPrice.ToString(),
                    Color = SKColor.Parse("#a5a5a5")
                });
            }

            return entries.ToArray();
        }
    }
}
