using System;
using System.Diagnostics;
#if CONTRACTS_FULL
using System.Diagnostics.Contracts;
#else
using PixelLab.Contracts;
#endif
using System.Windows;

namespace PixelLab.Common
{
    public  class GeoHelper
    {
      
        /// <summary>
        ///     Returns the scale factor by which an object of size <paramref name="source"/>
        ///     should be scaled to fit within an object of size <param name="target"/>.
        /// </summary>
        /// <param name="target">The target size.</param>
        /// <param name="size2">The source size.</param>
        public  double ScaleToFit(Size target, Size source)
        {
            Contract.Requires(target.IsValid());
            Contract.Requires(source.IsValid());
            Contract.Requires(target.Width > 0);
            Contract.Requires(source.Width > 0);

            double targetHWR = target.Height / target.Width;
            double sourceHWR = source.Height / source.Width;

            if (targetHWR > sourceHWR)
            {
                return target.Width / source.Width;
            }
            else
            {
                return target.Height / source.Height;
            }
        }

        public  bool Animate(
            double currentValue, double currentVelocity, double targetValue,
            double attractionFator, double dampening,
            double terminalVelocity, double minValueDelta, double minVelocityDelta,
            out double newValue, out double newVelocity)
        {
            Debug.Assert(currentValue.IsValid());
            Debug.Assert(currentVelocity.IsValid());
            Debug.Assert(targetValue.IsValid());

            Debug.Assert(dampening.IsValid());
            Debug.Assert(dampening > 0 && dampening < 1);

            Debug.Assert(attractionFator.IsValid());
            Debug.Assert(attractionFator > 0);

            Debug.Assert(terminalVelocity > 0);

            Debug.Assert(minValueDelta > 0);
            Debug.Assert(minVelocityDelta > 0);

            double diff = targetValue - currentValue;

            if (diff.Abs() > minValueDelta || currentVelocity.Abs() > minVelocityDelta)
            {
                newVelocity = currentVelocity * (1 - dampening);
                newVelocity += diff * attractionFator;
                if (currentVelocity.Abs() > terminalVelocity)
                {
                    newVelocity *= terminalVelocity / currentVelocity.Abs();
                }

                newValue = currentValue + newVelocity;

                return true;
            }
            else
            {
                newValue = targetValue;
                newVelocity = 0;
                return false;
            }
        }

        public  bool Animate(
        Point currentValue, Vector currentVelocity, Point targetValue,
        double attractionFator, double dampening,
        double terminalVelocity, double minValueDelta, double minVelocityDelta,
        out Point newValue, out Vector newVelocity)
        {
            Debug.Assert(currentValue.IsValid());
            Debug.Assert(currentVelocity.IsValid());
            Debug.Assert(targetValue.IsValid());

            Debug.Assert(dampening.IsValid());
            Debug.Assert(dampening > 0 && dampening < 1);

            Debug.Assert(attractionFator.IsValid());
            Debug.Assert(attractionFator > 0);

            Debug.Assert(terminalVelocity > 0);

            Debug.Assert(minValueDelta > 0);
            Debug.Assert(minVelocityDelta > 0);

            Vector diff = targetValue.Subtract(currentValue);

            if (diff.Length > minValueDelta || currentVelocity.Length > minVelocityDelta)
            {
                newVelocity = currentVelocity * (1 - dampening);
                newVelocity += diff * attractionFator;
                if (currentVelocity.Length > terminalVelocity)
                {
                    newVelocity *= terminalVelocity / currentVelocity.Length;
                }

                newValue = currentValue + newVelocity;

                return true;
            }
            else
            {
                newValue = targetValue;
                newVelocity = new Vector();
                return false;
            }
        }

    
        public  double AngleRad(Point point1, Point point2, Point point3)
        {
            Debug.Assert(point1.IsValid());
            Debug.Assert(point2.IsValid());
            Debug.Assert(point3.IsValid());

            double rad = AngleRad(point2.Subtract(point1), point2.Subtract(point3));

            double rad2 = AngleRad(point2.Subtract(point1), (point2.Subtract(point3)).RightAngle());

            if (rad2 < (Math.PI / 2))
            {
                return rad;
            }
            else
            {
                return (Math.PI * 2) - rad;
            }
        }

     

        public  double Dot(Vector v1, Vector v2)
        {
            Debug.Assert(v1.IsValid());
            Debug.Assert(v2.IsValid());

            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public  double AngleRad(Vector v1, Vector v2)
        {
            Debug.Assert(v1.IsValid());
            Debug.Assert(v2.IsValid());

            double dot = Dot(v1, v2);
            double dotNormalize = dot / (v1.Length * v2.Length);
            double acos = Math.Acos(dotNormalize);

            return acos;
        }

        public  Vector GetVectorFromAngle(double angleRadians, double length)
        {
            Contract.Requires(angleRadians.IsValid());
            Contract.Requires(length.IsValid());

            double x = Math.Cos(angleRadians) * length;
            double y = -Math.Sin(angleRadians) * length;

            return new Vector(x, y);
        }

        public  readonly Size SizeInfinite = new Size(double.PositiveInfinity, double.PositiveInfinity);
    }
}
