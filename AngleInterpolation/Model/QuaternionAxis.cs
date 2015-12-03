using System;

namespace AngleInterpolation.Model
{
    /// <summary>
    /// http://www.opengl-tutorial.org/intermediate-tutorials/tutorial-17-quaternions/
    /// http://web.mit.edu/2.998/www/QuaternionReport1.pdf
    /// http://blog.pixelingene.com/2006/09/smooth-3d-rotations-with-quaternions/
    /// </summary>
    public class QuaternionAxis : InterpolationAxis
    {
        #region Private Members

        const double DotThreshold = 0.9995;

        #endregion Private Members

        #region Constructors

        public QuaternionAxis(Vector3 startPosition, Vector3 startRotation, Vector3 endPosition, Vector3 endRotation)
            : base(startPosition, startRotation, endPosition, endRotation)
        {
        }

        #endregion Constructors

        #region Public Methods

        // http://number-none.com/product/Understanding%20Slerp,%20Then%20Not%20Using%20It/
        public override void UpdatePosition(TimeSpan t)
        {
            var delta = EndPosition - StartPosition;
            if (delta.Length == 0) return;

            var dot = StartPosition.Dot(EndPosition);

            if (dot > DotThreshold)
            {
                var interpolation = StartPosition + t.Milliseconds * delta / 100.0;
                interpolation = interpolation.Normalized;
                Position = interpolation;
            }

            dot = Clamp(dot, -1, 1);
            var theta0 = Math.Acos(dot);
            var theta = theta0 * t.Milliseconds;

            var result = EndPosition - (StartPosition * dot);
            if (result.Length != 0)
                result = result.Normalized;
            Position = (StartPosition * Math.Cos(theta)) + (result * Math.Sin(theta));
        }

        private double Clamp(double x, double min, double max)
        {
            if (x < min) x = min;
            if (x > max) x = max;
            return x;
        }

        #endregion Public Methods
    }
}