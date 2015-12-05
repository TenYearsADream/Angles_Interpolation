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

        protected override Vector3 UpdatePosition(Vector3 start, Vector3 destination, Vector3 position, double t, int animationTime)
        {
            if (t >= animationTime) return position;

            var alpha = (destination - start) * t / animationTime;

            return position * new[,]{{Math.Cos(alpha.X),Math.Sin(alpha.X),0},
                                      {-Math.Sin(alpha.X),Math.Cos(alpha.X), 0},
                                      {0,0,1}};

            //return position * new[,]{{((Math.Cos(alpha.X) * Math.Cos(alpha.Z)) - (Math.Sin(alpha.X) * Math.Sin(alpha.Z) * Math.Cos(alpha.Y))), ((Math.Sin(alpha.X) * Math.Cos(alpha.Z)) + (Math.Cos(alpha.X) * Math.Sin(alpha.Z) * Math.Cos(alpha.Y))), (Math.Sin(alpha.Z * Math.Sin(alpha.Y)))},
            //                             {(-(Math.Cos(alpha.X) * Math.Sin(alpha.Z)) - (Math.Sin(alpha.X) * Math.Cos(alpha.Z) * Math.Cos(alpha.Y))), (-(Math.Sin(alpha.X) * Math.Sin(alpha.Z)) + (Math.Cos(alpha.X) * Math.Cos(alpha.Z) * Math.Cos(alpha.Y))), (Math.Cos(alpha.Z) * Math.Sin(alpha.Y))},
            //                             {(Math.Sin(alpha.X) * Math.Sin(alpha.Y)), (-Math.Cos(alpha.X) * Math.Sin(alpha.Y)), Math.Cos(alpha.Y)}};
        }

        public override void UpdatePosition(double t, int animationTime)
        {
            Position = Lerp(StartPosition, EndPosition, Position, t, animationTime);
            Rotation = UpdatePosition(StartRotation, EndRotation, Rotation, t, animationTime);
        }
    }
}