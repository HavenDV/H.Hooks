using System;

namespace H.NET.Utilities
{
    /// <summary>
    /// Windows.Forms MouseEventArgs code
    /// </summary>
    public class MouseEventArgs : EventArgs
    {
        /// <summary>Gets which mouse button was pressed.</summary>
        /// <returns>One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values.</returns>
        public MouseButtons Button { get; }

        /// <summary>Gets the number of times the mouse button was pressed and released.</summary>
        /// <returns>An <see cref="T:System.Int32" /> that contains the number of times the mouse button was pressed and released.</returns>
        public int Clicks { get; }

        /// <summary>Gets the x-coordinate of the mouse during the generating mouse event.</summary>
        /// <returns>The x-coordinate of the mouse, in pixels.</returns>
        public int X { get; }

        /// <summary>Gets the y-coordinate of the mouse during the generating mouse event.</summary>
        /// <returns>The y-coordinate of the mouse, in pixels.</returns>
        public int Y { get; }

        /// <summary>Gets a signed count of the number of detents the mouse wheel has rotated, multiplied by the WHEEL_DELTA constant. A detent is one notch of the mouse wheel.</summary>
        /// <returns>A signed count of the number of detents the mouse wheel has rotated, multiplied by the WHEEL_DELTA constant.</returns>
        public int Delta { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MouseEventArgs" /> class.</summary>
        /// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicate which mouse button was pressed. </param>
        /// <param name="clicks">The number of times a mouse button was pressed. </param>
        /// <param name="x">The x-coordinate of a mouse click, in pixels. </param>
        /// <param name="y">The y-coordinate of a mouse click, in pixels. </param>
        /// <param name="delta">A signed count of the number of detents the wheel has rotated. </param>
        public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta)
        {
            Button = button;
            Clicks = clicks;
            X = x;
            Y = y;
            Delta = delta;
        }

        // <summary>Gets the location of the mouse during the generating mouse event.</summary>
        // <returns>A <see cref="T:System.Drawing.Point" /> that contains the x- and y- mouse coordinates, in pixels, relative to the upper-left corner of the form.</returns>
        //public Point Location {
        //    get {
        //        return new Point(this.x, this.y);
        //    }
        //}
    }
}