using System;
using System.Collections.Generic;
using SharpGL;

namespace AngleInterpolation.Model
{
    public abstract class InterpolationAxis : AxisDetails
    {
        #region Private Members

        private List<Tuple<Vector3, Vector3>> _frames;

        #endregion Private Members

        #region Protected Members

        protected const double PositionDelta = 0.01;

        protected const double AngleDelta = 2.0;

        #endregion Protected Members

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

        protected abstract Vector3 UpdatePosition(Vector3 start, Vector3 destination, Vector3 position, double t, int animationTime, double epsilon);

        protected Vector3 Lerp(Vector3 start, Vector3 destination, Vector3 position, double t, int animationTime, double epsilon)
        {
            if ((destination - position).Length < epsilon || t >= animationTime) return position;
            return (start + t * (destination - start) / animationTime);
        }

        #endregion Protected Methods

        #region Public Methods

        public abstract void UpdatePosition(double t, int animationTime);

        public override void Render(OpenGL gl)
        {
            base.Render(gl);
            Render(gl, EndPosition, EndRotation);
            if (_frames != null)
                foreach (var frame in _frames)
                    Render(gl, frame.Item1, frame.Item2);
        }

        public void ShowAllFrames(int animationTime, int frameCount)
        {
            double timeDelta = animationTime / (frameCount + 1.0);
            _frames = new List<Tuple<Vector3, Vector3>>();

            for (int i = 0; i < frameCount; i++)
            {
                var position = UpdatePosition(StartPosition, EndPosition, Position, i * timeDelta, animationTime, PositionDelta);
                var rotation = UpdatePosition(StartRotation, EndRotation, Rotation, i * timeDelta, animationTime, AngleDelta);
                _frames.Add(new Tuple<Vector3, Vector3>(position, rotation));
            }
        }

        #endregion Public Methods

    }
}
