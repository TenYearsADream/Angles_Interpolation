using System;
using System.Windows.Media.Media3D;

namespace AngleInterpolation.ViewModel
{
    public class QuaternionAxis : AxisDetails
    {
        #region Private Members

        const double DotThreshold = 0.9995;

        private Vector3D _startPosition;
        private Vector3D _endPosition;

        #endregion Private Members

        #region Constructors

        public QuaternionAxis(Vector3D startPosition, Vector3D startRotation, Vector3D endPosition, Vector3D endRotation)
            : base(startPosition, startRotation)
        {
            _startPosition = startPosition;
            _endPosition = endPosition;
        }

        #endregion Constructors

        #region Public Methods

        public override void UpdatePosition(TimeSpan t)
        {
            var dot = Vector3D.DotProduct(_startPosition, _endPosition);

            if (dot > DotThreshold)
            {
                var interpolation = _startPosition + t.Milliseconds * (_endPosition - _startPosition);
                interpolation.Normalize();
                Position = interpolation;
            }

            // TODO: Clamp(dot, -1, 1);
            //        clamp(x, min, max):
            //if (x < min) then
            //    x = min;
            //else if (x > max) then
            //    x = max;
            //return x;

            var theta0 = Math.Acos(dot);
            var theta = theta0 * t.Milliseconds;

            var result = _endPosition - _startPosition * dot;
            result.Normalize();
            Position = _startPosition * Math.Cos(theta) + result * Math.Sin(theta);
        }

        #endregion Public Methods
    }
}