using System;

namespace AngleInterpolation.Model
{
    public static class Extensions
    {
        #region Public Methods

        public static double Clamp(this double x, double min, double max)
        {
            if (x < min) x = min;
            if (x > max) x = max;
            return x;
        }

        /// <summary>
        /// Gets the euler angles from quaternion.
        /// </summary>
        /// <param name="q">The quaternion.</param>
        /// <returns>Euler angles from quaternion q.</returns>
        public static Vector3 EulerAnglesFromNormalizedQuaternion(this Vector4 q)
        {
            return new Vector3(Math.Atan2(2 * ((q.X * q.Y) + (q.Z * q.W)), 1 - (2 * ((q.Y * q.Y) + (q.Z * q.Z)))),
                Math.Asin(2 * ((q.X * q.Z) - (q.W * q.Y))),
                Math.Atan2(2 * ((q.X * q.Z) + (q.Y * q.Z)), 1 - (2 * ((q.Z * q.Z) + (q.W * q.W)))));
        }

        public static Vector3 EulerAnglesFromQuaternion(this Vector4 q) 
        {
            double sqw = q.W * q.W;
            double sqx = q.X * q.X;
            double sqy = q.Y * q.Y;
            double sqz = q.Z * q.Z;
	        double unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
	        double test = q.X * q.Y + q.Z * q.W;

	        if (test > 0.499*unit) // singularity at north pole
	        	return new Vector3(2 * Math.Atan2(q.X, q.W), Math.PI / 2, 0);

	        if (test < -0.499 * unit) // singularity at south pole
                return new Vector3(-2 * Math.Atan2(q.X, q.W), -Math.PI / 2, 0);

            return new Vector3(Math.Atan2(2 * q.Y * q.W - 2 * q.X * q.Z, sqx - sqy - sqz + sqw) * 180 / Math.PI,
            Math.Asin(2 * test / unit) * 180 / Math.PI,
            Math.Atan2(2 * q.X * q.W - 2 * q.Y * q.Z, -sqx + sqy - sqz + sqw) * 180 / Math.PI);

            //return new Vector3(Math.Atan2(-2 * q.Y * q.X - 2 * q.W * q.Z,sqw  + sqx - sqy - sqz) * 180 / Math.PI,
            //Math.Asin(2 * (q.X * q.Z + q.W * q.Y)) * 180 / Math.PI,
            //Math.Atan2(-2 * q.Y * q.Z - q.W * q.X, -sqx - sqy + sqz + sqw) * 180 / Math.PI);
        }

        /// <summary>
        /// Gets the quaternion from euler angles.
        /// </summary>
        /// <param name="angles">The euler angles.</param>
        /// <returns>Quaternion from euler angles.</returns>
        /// <remarks>Assuming the angles are in radians.</remarks>
        public static Vector4 QuaternionFromEulerAngles(this Vector3 angles)
        {
            var xAngle = angles.X * Math.PI / 180;
            var yAngle = angles.Y * Math.PI / 180;
            var zAngle = angles.Z * Math.PI / 180;
            double c1 = Math.Cos(xAngle);
            double s1 = Math.Sin(xAngle);
            double c2 = Math.Cos(yAngle);
            double s2 = Math.Sin(yAngle);
            double c3 = Math.Cos(zAngle);
            double s3 = Math.Sin(zAngle);
            var w = Math.Sqrt(1.0 + c1 * c2 + c1 * c3 - s1 * s2 * s3 + c2 * c3) / 2.0;
            double w4 = (4.0 * w);
            var x = (c2 * s3 + c1 * s3 + s1 * s2 * c3) / w4 ;
            var y = (s1 * c2 + s1 * c3 + c1 * s2 * s3) / w4 ;
            var z = (-s1 * s3 + c1 * s2 * c3 +s2) / w4 ;

            return new Vector4(x, y, z, w);
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

