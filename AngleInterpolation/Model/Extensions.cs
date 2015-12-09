using System;

namespace AngleInterpolation.Model
{
    public static class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Gets the euler angles from quaternion.
        /// </summary>
        /// <param name="q">The quaternion.</param>
        /// <returns>Euler angles from quaternion q.</returns>
        public static Vector3 EulerAnglesFromQuaternion(this Vector4 q)
        {
            return new Vector3(Math.Atan2(2 * ((q.X * q.Y) + (q.Z * q.W)), 1 - (2 * ((q.Y * q.Y) + (q.Z * q.Z)))),
                Math.Asin(2 * ((q.X * q.Z) - (q.W * q.Y))),
                Math.Atan2(2 * ((q.X * q.Z) + (q.Y * q.Z)), 1 - (2 * ((q.Z * q.Z) + (q.W * q.W)))));
        }

        /// <summary>
        /// Gets the quaternion from euler angles.
        /// </summary>
        /// <param name="angles">The euler angles.</param>
        /// <returns>Quaternion from euler angles.</returns>
        public static Vector4 QuaternionFromEulerAngles(this Vector3 angles)
        {
            return new Vector4(0,0,0,0);
        }

        /// <summary>
        /// Returns the dot product of two given quaternions.
        /// </summary>
        /// <param name="q1">The q1.</param>
        /// <param name="q2">The q2.</param>
        /// <returns>The dot product of two given quaternions</returns>
        public static double Dot(this Vector4 q1, Vector4 q2)
        {
            return (q1.X * q2.X) + (q1.Y * q2.Y) + (q1.Z * q2.Z) + (q1.W * q2.W);
        }

        #endregion Public Methods
    }
}

