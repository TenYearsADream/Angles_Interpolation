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

        private const double DotThreshold = 0.9995;

        private InterpolationType _interpolationType;

        #endregion Private Members

        #region Public Properties

        /// <summary>
        /// Gets or sets the type of the quaternion interpolation.
        /// </summary>
        public InterpolationType InterpolationType
        {
            get { return _interpolationType; }
            set
            {
                if (_interpolationType == value) return;
                _interpolationType = value;
                OnPropertyChanged("InterpolationType");
            }
        }

        #endregion Public Properties

        #region Constructors

        public QuaternionAxis(Vector3 startPosition, Vector3 startRotation, Vector3 endPosition, Vector3 endRotation)
            : base(startPosition, startRotation, endPosition, endRotation)
        {
            _interpolationType = InterpolationType.Slerp;
        }

        #endregion Constructors

        #region Private Methods

        private double Clamp(double x, double min, double max)
        {
            if (x < min) x = min;
            if (x > max) x = max;
            return x;
        }

        private Vector3 Slerp(Vector3 start, Vector3 destination, Vector3 position, double t, int animationTime)
        {
            if (t >= animationTime) return position;

            var cosTheta = start.Dot(destination);

            if (cosTheta > DotThreshold)
            {
                var interpolation = start + (destination - start) * t / animationTime;
                interpolation = interpolation.Normalized;
                return interpolation;
            }

            cosTheta = Clamp(cosTheta, -1, 1);
            var theta0 = Math.Acos(cosTheta);
            var theta = theta0 * t / animationTime;

            var result = destination - (start * cosTheta);
            if (result.Length != 0)
                result = result.Normalized;
            return (start * Math.Cos(theta)) + (result * Math.Sin(theta));
        }

        #endregion Private Methods

        #region Protected Methods

        // http://number-none.com/product/Understanding%20Slerp,%20Then%20Not%20Using%20It/
        protected override Vector3 UpdatePosition(Vector3 start, Vector3 destination, Vector3 position, double t, int animationTime)
        {
            if (InterpolationType == InterpolationType.Lerp)
                return Lerp(start, destination, position, t, animationTime);
            return Slerp(start, destination, position, t, animationTime);
        }

        #endregion Protected Methods

        #region Public Methods

        public override void UpdatePosition(double t, int animationTime)
        {
            Position = Lerp(StartPosition, EndPosition, Position, t, animationTime);
            Rotation = UpdatePosition(StartRotation, EndRotation, Rotation, t, animationTime);
        }

        #endregion Public Methods
    }
}