using System;
using System.Windows.Media.Media3D;

namespace AngleInterpolation.ViewModel
{
    public class EulerAxis : AxisDetails
    {
        #region Private Members

        private Vector3D _startPosition;
        private Vector3D _endPosition;

        #endregion Private Members

        #region Constructors

        public EulerAxis(Vector3D startPosition, Vector3D startRotation, Vector3D endPosition, Vector3D endRotation)
            : base(startPosition, startRotation)
        {
            _startPosition = startPosition;
            _endPosition = endPosition;
        }

        #endregion Constructors

        public override void UpdatePosition(TimeSpan t)
        {
        }
    }
}