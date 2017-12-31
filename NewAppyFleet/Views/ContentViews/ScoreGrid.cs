using System;
using System.Collections.Generic;
using mvvmframework.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ContentViews
{
    public class ScoreGrid
    {
        public static PlotView GenerateScoreGrid(Tuple<double, double> minmax, List<ScoreData> data)
        {
            var plotView = new PlotView
            {
                Model = new OxyplotModel(minmax, data).PieModel,
                WidthRequest = App.ScreenSize.Width * 2
            };

            return plotView;
        }
    }

    public class OxyplotModel
    {
        public PlotModel PieModel { get; set; }
        Tuple<double, double> MinMax;
        List<ScoreData> ScoreData;

        public OxyplotModel(Tuple<double, double> minmax, List<ScoreData> data)
        {
            MinMax = minmax;
            ScoreData = data;
            PieModel = CreateChart();
        }

        PlotModel CreateChart()
        {
            var model = new PlotModel 
            { 
                Title = "Score", 
                LegendPlacement = LegendPlacement.Outside, 
                LegendBorderThickness = 0,
                PlotType = PlotType.XY,
            };

            var dataPoints = new List<DataPoint>();
            var x = 0;
            foreach(var data in ScoreData)
            {
                dataPoints.Add(new DataPoint(x, data.Score));
                x++;
            }

            var series = new LineSeries
            {
                ItemsSource = dataPoints
            };
            model.Series.Add(series);

            return model;
        }
    }
}

