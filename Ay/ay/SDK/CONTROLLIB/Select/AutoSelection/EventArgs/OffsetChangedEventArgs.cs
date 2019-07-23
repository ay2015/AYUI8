using System;

namespace ay.Controls.Args
{
    public sealed class OffsetChangedEventArgs : EventArgs
    {
        private readonly double horizontal;
        private readonly double vertical;

        /// <summary>
        /// Initializes a new instance of the OffsetChangedEventArgs class.
        /// </summary>
        /// <param name="horizontal">The change in horizontal scroll.</param>
        /// <param name="vertical">The change in vertical scroll.</param>
        public OffsetChangedEventArgs(double horizontal, double vertical)
        {
            this.horizontal = horizontal;
            this.vertical = vertical;
        }

        /// <summary>Gets the change in horizontal scroll position.</summary>
        public double HorizontalChange
        {
            get { return this.horizontal; }
        }

        /// <summary>Gets the change in vertical scroll position.</summary>
        public double VerticalChange
        {
            get { return this.vertical; }
        }
    }
}
