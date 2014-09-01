﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomRectangleManipulator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a manipulator for rectangle zooming functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides a manipulator for rectangle zooming functionality.
    /// </summary>
    public class ZoomRectangleManipulator : MouseManipulator
    {
        /// <summary>
        /// The zoom rectangle.
        /// </summary>
        private OxyRect zoomRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomRectangleManipulator" /> class.
        /// </summary>
        /// <param name="plotView">The plot view.</param>
        public ZoomRectangleManipulator(IPlotView plotView)
            : base(plotView)
        {
        }

        /// <summary>
        /// Occurs when a manipulation is complete.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Completed(OxyMouseEventArgs e)
        {
            base.Completed(e);

            this.PlotView.HideZoomRectangle();

            if (this.zoomRectangle.Width > 10 && this.zoomRectangle.Height > 10)
            {
                var p0 = this.InverseTransform(this.zoomRectangle.Left, this.zoomRectangle.Top);
                var p1 = this.InverseTransform(this.zoomRectangle.Right, this.zoomRectangle.Bottom);

                if (this.XAxis != null)
                {
                    this.XAxis.Zoom(p0.X, p1.X);
                }

                if (this.YAxis != null)
                {
                    this.YAxis.Zoom(p0.Y, p1.Y);
                }

                this.PlotView.InvalidatePlot();
            }
        }

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Delta(OxyMouseEventArgs e)
        {
            base.Delta(e);

            var plotArea = this.PlotView.ActualModel.PlotArea;

            double x = Math.Min(this.StartPosition.X, e.Position.X);
            double w = Math.Abs(this.StartPosition.X - e.Position.X);
            double y = Math.Min(this.StartPosition.Y, e.Position.Y);
            double h = Math.Abs(this.StartPosition.Y - e.Position.Y);

            if (this.XAxis == null || !this.XAxis.IsZoomEnabled)
            {
                x = plotArea.Left;
                w = plotArea.Width;
            }

            if (this.YAxis == null || !this.YAxis.IsZoomEnabled)
            {
                y = plotArea.Top;
                h = plotArea.Height;
            }

            this.zoomRectangle = new OxyRect(x, y, w, h);
            this.PlotView.ShowZoomRectangle(this.zoomRectangle);
        }

        /// <summary>
        /// Gets the cursor for the manipulation.
        /// </summary>
        /// <returns>The cursor.</returns>
        public override CursorType GetCursorType()
        {
            if (this.XAxis == null)
            {
                return CursorType.ZoomVertical;
            }

            if (this.YAxis == null)
            {
                return CursorType.ZoomHorizontal;
            }

            return CursorType.ZoomRectangle;
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);
            this.zoomRectangle = new OxyRect(this.StartPosition.X, this.StartPosition.Y, 0, 0);
            this.PlotView.ShowZoomRectangle(this.zoomRectangle);
        }
    }
}