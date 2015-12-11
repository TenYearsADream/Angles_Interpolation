using System;

namespace AngleInterpolation.Model
{
    /// <summary>
    /// http://www.opengl-tutorial.org/intermediate-tutorials/tutorial-17-quaternions/
    /// http://web.mit.edu/2.998/www/QuaternionReport1.pdf
    /// http://blog.pixelingene.com/2006/09/smooth-3d-rotations-with-quaternions/
    /// </summary>
    public class QuaternionAxis : InterpolationAxis<Vector4>
    {
        #region Private Members

        private InterpolationType _interpolationType;
        private const double Epsilon = 0.001;

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

        public QuaternionAxis(Vector3 startPosition, Vector4 startRotation, Vector3 endPosition, Vector4 endRotation)
            : base(startPosition, startRotation, startRotation.EulerAnglesFromQuaternion(), endPosition, endRotation)
        {
            _interpolationType = InterpolationType.Slerp;
        }

        #endregion Constructors

        #region Private Methods

        private Vector4 Slerp(Vector4 start, Vector4 destination, double t, int animationTime)
        {
            if (t >= animationTime) return destination;

            var cosTheta = start.Dot(destination);

            if (Math.Abs(Math.Abs(cosTheta) - 1.0) < Epsilon)
                return start;

            if (cosTheta < 0)
            {
                cosTheta *= -1;
                destination *= -1;
            }

            cosTheta = cosTheta.Clamp(-1, 1);
            var theta = Math.Acos(cosTheta);
            if (Math.Abs(theta) < Epsilon)
                return start;

            start *= Math.Sin((1 - (t / animationTime)) * theta) / Math.Sin(theta);
            destination *= Math.Sin(theta * (t / animationTime)) / Math.Sin(theta);
            start += destination;
            return start;
        }

        #endregion Private Methods

        #region Protected Methods

        // http://number-none.com/product/Understanding%20Slerp,%20Then%20Not%20Using%20It/
        protected override Vector4 UpdateRotation(Vector4 start, Vector4 destination, double t, int animationTime)
        {
            if (InterpolationType == InterpolationType.Lerp)
                return Lerp(start.EulerAnglesFromQuaternion(), destination.EulerAnglesFromQuaternion(), t, animationTime).QuaternionFromEulerAngles();
            return Slerp(start, destination, t, animationTime);
        }

        protected override Vector3 GetRotation(Vector4 rotation)
        {
            return rotation.EulerAnglesFromQuaternion();
        }

        #endregion Protected Methods

        #region Public Methods

        public override void UpdatePosition(double t, int animationTime)
        {
            base.UpdatePosition(t, animationTime);
            Rotation = UpdateRotation(StartRotation, EndRotation, t, animationTime).EulerAnglesFromQuaternion();
        }

        #endregion Public Methods
    }
}