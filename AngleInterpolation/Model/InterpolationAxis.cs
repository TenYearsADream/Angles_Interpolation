using System;
using SharpGL;

namespace AngleInterpolation.Model
{
    public abstract class InterpolationAxis : AxisDetails
    {
        #region Private Members

        #endregion Private Members

        #region Protected Properties

        protected Vector3 StartPosition { get; private set; }

        protected Vector3 StartRotation { get; private set; }

        protected Vector3 EndPosition { get; private set; }

        protected Vector3 EndRotation { get; private set; }

        #endregion Protected Properties

        #region Constructors

        public InterpolationAxis(Vector3 startPosition, Vector3 startRotation, Vector3 endPosition, Vector3 endRotation)
            : base(startPosition, startRotation)
        {
            StartPosition = startPosition;
            StartRotation = startRotation;
            EndPosition = endPosition;
            EndRotation = endRotation;
        }

        #endregion Constructors

        #region Public Methods

        public abstract void UpdatePosition(TimeSpan t);

        public override void Render(OpenGL gl)
        {
            base.Render(gl);
            Render(gl, EndPosition, EndRotation);
        }

        #endregion Public Methods

    }
}
