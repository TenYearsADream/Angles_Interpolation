using System;

namespace AngleInterpolation.Model
{
    /// <summary>
    /// http://ogre3d.org/tikiwiki/Euler+Angle+Class
    /// http://run.usc.edu/cs520-s14/quaternions/quaternions-cs520.pdf
    /// </summary>
    public class EulerAxis : InterpolationAxis
    {
        #region Private Members

        #endregion Private Members

        #region Constructors

        public EulerAxis(Vector3 startPosition, Vector3 startRotation, Vector3 endPosition, Vector3 endRotation)
            : base(startPosition, startRotation, endPosition, endRotation)
        {
        }

        #endregion Constructors

        protected override Vector3 UpdatePosition(Vector3 start, Vector3 destination, Vector3 position, TimeSpan t, double epsilon)
        {
            return Position;
        }
    }
}