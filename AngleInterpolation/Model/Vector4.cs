using System;
using System.Windows.Media.Media3D;
using AngleInterpolation.ViewModel;

namespace AngleInterpolation.Model
{
    public class Vector4 : ViewModelBase
    {
        private double _x, _y, _z, _w;

        public Vector4(double x, double y, double z, double w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public double X { get { return _x; } set { if (_x == value)return; _x = value; OnPropertyChanged("X"); } }
        public double Y { get { return _y; } set { if (_y == value)return; _y = value; OnPropertyChanged("Y"); } }
        public double Z { get { return _z; } set { if (_z == value)return; _z = value; OnPropertyChanged("Z"); } }
        public double W { get { return _w; } set { if (_w == value)return; _w = value; OnPropertyChanged("W"); } }

        public double Length { get { return Math.Sqrt(LengthSquared); } }

        public double LengthSquared { get { return _x * _x + _y * _y + _z * _z + _w * _w; } }

        public Vector4 Normalized { get { return this / Length; } }

        public double Dot(Vector4 vector)
        {
            return _x * vector.X + _y * vector.Y + _z * vector.Z + _w * vector.W;
        }

        public Vector4 Cross(Vector4 v)
        {
            return new Vector4(Y * v.Z - (Z * v.Y), Z * v.X - (X * v.Z), X * v.Y - (Y * v.X), 1);
        }

        public static Vector4 operator *(Vector4 vector, double value)
        {
            return new Vector4(vector.X * value, vector.Y * value, vector.Z * value, vector.W * value);
        }

        public static double operator *(Vector4 vector, Vector4 value)
        {
            return vector.X * value.X + vector.Y * value.Y + vector.Z * value.Z + vector.W * value.W;
        }

        public static Vector4 operator /(Vector4 vector, double value)
        {
            return new Vector4(vector.X / value, vector.Y / value, vector.Z / value, vector.W / value);
        }

        public static Vector4 operator +(Vector4 vector, Vector4 vector1)
        {
            return new Vector4(vector.X + vector1.X, vector.Y + vector1.Y, vector.Z + vector1.Z, vector.W + vector1.W);
        }

        public static Vector4 operator -(Vector4 vector, Vector4 vector1)
        {
            return new Vector4(vector.X - vector1.X, vector.Y - vector1.Y, vector.Z - vector1.Z, vector.W - vector1.W);
        }

        public static Vector4 operator *(Matrix3D matrix, Vector4 vector)
        {
            return new Vector4(vector.X * matrix.M11 + vector.Y * matrix.M12 + vector.Z * matrix.M13 + vector.W * matrix.M14
                                , vector.X * matrix.M21 + vector.Y * matrix.M22 + vector.Z * matrix.M23 + vector.W * matrix.M24
                                , vector.X * matrix.M31 + vector.Y * matrix.M32 + vector.Z * matrix.M33 + vector.W * matrix.M34
                                , vector.X * matrix.OffsetX + vector.Y * matrix.OffsetY + vector.Z * matrix.OffsetZ + vector.W * matrix.M44);
        }

        public static Vector4 operator *(Vector4 vector, Matrix3D matrix)
        {
            return new Vector4(vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + vector.W * matrix.OffsetX
                                , vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + vector.W * matrix.OffsetY
                                , vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + vector.W * matrix.OffsetZ
                                , vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + vector.W * matrix.M44);
        }

        public static Vector4 operator *(Vector4 vector, double[,] matrix)
        {
            if (matrix.GetLength(0) != 4 || matrix.GetLength(1) != 4)
                throw new Exception("Matrix dimensions should be 4x4");

            return new Vector4(vector.X * matrix[0, 0] + vector.Y * matrix[0, 1] + vector.Z * matrix[0, 2] + vector.W * matrix[0, 3]
                                , vector.X * matrix[1, 0] + vector.Y * matrix[1, 1] + vector.Z * matrix[1, 2] + vector.W * matrix[1, 3]
                                , vector.X * matrix[2, 0] + vector.Y * matrix[2, 1] + vector.Z * matrix[2, 2] + vector.W * matrix[2, 3]
                                , vector.X * matrix[3, 0] + vector.Y * matrix[3, 1] + vector.Z * matrix[3, 2] + vector.W * matrix[3, 3]);
        }

        public void SetValues(Vector4 v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
            _w = v.W;
        }

        public void RaisePropertyChanged()
        {
            OnPropertyChanged("X");
            OnPropertyChanged("Y");
            OnPropertyChanged("Z");
            OnPropertyChanged("W");
        }
    }

}

