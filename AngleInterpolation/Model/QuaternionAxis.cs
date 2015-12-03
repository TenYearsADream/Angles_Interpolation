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
            if ((EndPosition - Position).Length <0.001) return;

            var cosTheta = StartPosition.Dot(EndPosition);

            if (cosTheta > DotThreshold)
            {
                var interpolation = StartPosition + (EndPosition - StartPosition) * t.TotalMilliseconds / 100.0;
                interpolation = interpolation.Normalized;
                Position = interpolation;
            }

            cosTheta = Clamp(cosTheta, -1, 1);
            var theta0 = Math.Acos(cosTheta);
            var theta = theta0 * t.TotalMilliseconds / 5000;

            var result = EndPosition - (StartPosition * cosTheta);
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