using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Point = DotNet.Highcharts.Options.Point;
using DotNet.Highcharts;
using System.Drawing;
using Report = App.Service.Report.DeliveryTrackingStatus;
using App.Data.Domain;

namespace App.Web.Helper.Extensions
{
    public static class HighchartsExtensions
    {
        public static Highcharts GetHighcharts(int Moda, string From, string To, int Status, int UnitType, string ETD, string ATD, string ETA, string ATA, string NODA)
        {
            var data = Report.GetList(Moda, From, To, Status, UnitType, ETD, ATD, ETA, ATA, NODA);
            string[] categories = data.GroupBy(p => p.MonthName).Select(p => p.Key).ToArray();
            Series[] series = GetSeries(data.ToList(), categories);

            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title { Text = "Shipment Volume Report" })
                .SetXAxis(new XAxis { Categories = categories })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle { Text = "Persentase (%)" },
                    Labels = new YAxisLabels { Format = "{value} %" },
                    StackLabels = new YAxisStackLabels
                    {
                        Enabled = true,
                        Style = "fontWeight: 'bold', color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'",
                        Formatter = "function() { return this.total; }"
                    }
                })
                .SetLegend(new Legend
                {
                    Layout = Layouts.Horizontal,
                    Align = HorizontalAligns.Center,
                    VerticalAlign = VerticalAligns.Top,
                    X = 100,
                    Y = 60,
                    Floating = true,
                    BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                    Shadow = true
                })
                .SetTooltip(new Tooltip { Formatter = @"function() { return ''+ this.key +'<br><b>'+ this.series.name +'</b>: '+ this.y +' %'; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        PointPadding = 0.2,
                        BorderWidth = 0
                    }
                })
                .SetSeries(series);

            return chart;
        }

        public static Series[] GetSeries(List<RptDeliveryTrackingStatus> model, string[] categories)
        {
            List<Series> series = new List<Series>();

            var data = model.GroupBy(p => p.UnitType).Select(p => p.Key).ToList();
            foreach (var unitType in data)
            {
                object[] Data = GetData(model.Where(p => p.UnitType == unitType).ToList(), categories);
                series.Add(new Series { Name = unitType, Data = new DotNet.Highcharts.Helpers.Data(Data) });
            }
            return series.ToArray();
        }

        public static object[] GetData(List<RptDeliveryTrackingStatus> model, string[] categories)
        {
            List<object> Data = new List<object>();
            foreach (var cat in categories)
            {
                var data = model.Where(p => p.MonthName == cat);
                foreach (var m in data)
                {
                    Data.Add(new object[] { "Total : " + m.COUNT_NODA_ALL.ToString() + ", POD : " + m.COUNT_NODA_POD.ToString(), m.PERCENTAGE });
                }
            }

            return Data.ToArray();
        }
    }
}