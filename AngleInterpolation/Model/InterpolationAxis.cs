using System;
using System.Collections.Generic;
using SharpGL;

namespace AngleInterpolation.Model
{
    public abstract class InterpolationAxis<T> : AxisDetails
    {
        #region Private Members

        private List<Tuple<Vector3, Vector3>> _frames;

        #endregion Private Members

        #region Protected Members

        #endregion Protected Members

        #region Protected Properties

        protected Vector3 StartPosition { get; private set; }

        protected T StartRotation { get; private set; }

        protected Vector3 EndPosition { get; private set; }

        protected T EndRotation { get; private set; }

        #endregion Protected Properties

        #region Constructors

        public InterpolationAxis(Vector3 startPosition, T startRotation, Vector3 startRotationVector, Vector3 endPosition, T endRotation)
            : base(startPosition, startRotationVector)
        {
            StartPosition = startPosition;
            StartRotation = startRotation;
            EndPosition = endPosition;
            EndRotation = endRotation;
        }

        #endregion Constructors

        #region Protected Methods

        protected abstract T UpdateRotation(T start, T destination, double t, int animationTime);

        protected abstract Vector3 GetRotation(T rotation);

        protected Vector3 Lerp(Vector3 start, Vector3 destination, double t, int animationTime)
        {
            if (t >= animationTime) return destination;
            return (start + t * (destination - start) / animationTime);
        }

        #endregion Protected Methods

        #region Public Methods

        public virtual void UpdatePosition(double t, int animationTime)
        {
            Position = Lerp(StartPosition, EndPosition, t, animationTime);
        }

        public override void Render(OpenGL gl)
        {
            base.Render(gl);
            Render(gl, EndPosition, GetRotation(EndRotation));
            if (_frames != null)
                foreach (var frame in _frames)
                    Render(gl, frame.Item1, frame.Item2);
        }

        public void ShowAllFrames(int animationTime, int frameCount)
        {
            if (frameCount < 2) return;

            double timeDelta = animationTime / (frameCount - 1.0);
            _frames = new List<Tuple<Vector3, Vector3>>();

            for (int i = 1; i < frameCount - 1; i++)
            {
                var position = Lerp(StartPosition, EndPosition, i * timeDelta, animationTime);
                var rotation = UpdateRotation(StartRotation, EndRotation, i * timeDelta, animationTime);
                _frames.Add(new Tuple<Vector3, Vector3>(position, GetRotation(rotation)));
            }
        }

        #endregion Public Methods

    }
}
