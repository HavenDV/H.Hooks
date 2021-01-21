using System;
using System.Drawing;

// ReSharper disable CheckNamespace

namespace H.Hooks
{
    /// <summary>
    /// Provides data for the MouseClickExt and MouseMoveExt events.
    /// It also provides a property Handled.
    /// Set this property to <b>true</b> to prevent further processing
    /// of the event in other applications.
    /// </summary>
    public class MouseEventArgs : KeyboardEventArgs
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool IsDoubleClick { get; }

        /// <summary>
        /// Gets a signed count of the number of detents the mouse wheel has rotated,
        /// multiplied by the WHEEL_DELTA constant.
        /// A detent is one notch of the mouse wheel.
        /// </summary>
        /// <returns>
        /// A signed count of the number of detents the mouse wheel
        /// has rotated, multiplied by the WHEEL_DELTA constant.
        /// </returns>
        public int Delta { get; }

        /// <summary>
        /// Gets the location of the mouse during the generating mouse event.
        /// </summary>
        /// <returns>
        /// A <see cref="Point" /> that contains
        /// the x- and y- mouse coordinates, in pixels, relative to
        /// the upper-left corner of the form.
        /// </returns>
        public Point Position { get; }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="MouseEventArgs" /> class.</summary>
        /// <param name="keys">
        /// One of the <see cref="MouseButtons" /> values that
        /// indicate which mouse button was pressed.
        /// </param>
        /// <param name="isDoubleClick"></param>
        /// <param name="x">The x-coordinate of a mouse click, in pixels.</param>
        /// <param name="y">The y-coordinate of a mouse click, in pixels.</param>
        /// <param name="delta">A signed count of the number of detents the wheel has rotated.</param>
        public MouseEventArgs(int x, int y, int delta, bool isDoubleClick, params Key[] keys) : 
            base(keys) 
        {
            Position = new Point(x, y);
            Delta = delta;
            IsDoubleClick = isDoubleClick;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Keys}: {Position}";
        }

        #endregion
    }
}