using System;
using System.Windows.Documents;
using SharpGL;

namespace AngleInterpolation.Model
{
    public abstract class InterpolationAxis : AxisDetails
    {
        #region Private Members

        private const double PositionDelta = 0.01;
        private const double AngleDelta = 2.0;

        //private List

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

        #region Protected Methods

        protected abstract Vector3 UpdatePosition(Vector3 start, Vector3 destination, Vector3 position, TimeSpan t, double epsilon);

        #endregion Protected Methods

        #region Public Methods

        public void UpdatePosition(TimeSpan t)
        {
            Position = UpdatePosition(StartPosition, EndPosition, Position, t, PositionDelta);
            Rotation = UpdatePosition(StartRotation, EndRotation, Rotation, t, AngleDelta);
        }

        public override void Render(OpenGL gl)
        {
            base.Render(gl);
            Render(gl, EndPosition, EndRotation);
        }

        public void ShowAllFrames()
        {}

        #endregion Public Methods

    }
}
