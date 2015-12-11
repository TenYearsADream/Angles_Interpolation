using System;
using AngleInterpolation.ViewModel;

namespace AngleInterpolation.Model
{
    public class Vector3 : ViewModelBase
    {
        private double _x, _y, _z;

        public Vector3(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public double X
        {
            get { return _x; }
            set
            {
                if (_x == value) return;
                _x = value;
                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                if (_y == value) return;
                _y = value;
                OnPropertyChanged("Y");
            }
        }

        public double Z
        {
            get { return _z; }
            set
            {
                if (_z == value) return;
                _z = value;
                OnPropertyChanged("Z");
            }
        }

        public double Length { get { return Math.Sqrt(_x * _x + _y * _y + _z * _z); } }

        public double LengthSquared { get { return _x * _x + _y * _y + _z * _z; } }

        public Vector3 Normalized { get { return this / Math.Sqrt(_x * _x + _y * _y + _z * _z); } }

        public double Dot(Vector3 vector)
        {
            return _x * vector.X + _y * vector.Y + _z * vector.Z;
        }

        public Vector3 Cross(Vector3 v)
        {
            return new Vector3(Y * v.Z - (Z * v.Y), Z * v.X - (X * v.Z), X * v.Y - (Y * v.X));
        }

        public static Vector3 operator *(Vector3 vector, double value)
        {
            return new Vector3(vector.X * value, vector.Y * value, vector.Z * value);
        }

        public static Vector3 operator *(double value, Vector3 vector)
        {
            return new Vector3(vector.X * value, vector.Y * value, vector.Z * value);
        }

        public static Vector3 operator *(Vector3 vector, int value)
        {
            return new Vector3(vector.X * value, vector.Y * value, vector.Z * value);
        }

        public static Vector3 operator *(int value, Vector3 vector)
        {
            return new Vector3(vector.X * value, vector.Y * value, vector.Z * value);
        }

        public static double operator *(Vector3 vector, Vector3 value)
        {
            return vector.X * value.X + vector.Y * value.Y + vector.Z * value.Z;
        }

        public static Vector3 operator /(Vector3 vector, double value)
        {
            return new Vector3(vector.X / value, vector.Y / value, vector.Z / value);
        }

        public static Vector3 operator +(Vector3 vector, Vector3 vector1)
        {
            return new Vector3(vector.X + vector1.X, vector.Y + vector1.Y, vector.Z + vector1.Z);
        }

        public static Vector3 operator -(Vector3 vector, Vector3 vector1)
        {
            return new Vector3(vector.X - vector1.X, vector.Y - vector1.Y, vector.Z - vector1.Z);
        }

        public static Vector3 operator *(Vector3 vector, double[,] matrix)
        {
            if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 3)
                throw new Exception("Matrix dimensions should be 3x3");

            return new Vector3(vector.X * matrix[0, 0] + vector.Y * matrix[0, 1] + vector.Z * matrix[0, 2]
                               , vector.X * matrix[1, 0] + vector.Y * matrix[1, 1] + vector.Z * matrix[1, 2]
                               , vector.X * matrix[2, 0] + vector.Y * matrix[2, 1] + vector.Z * matrix[2, 2]);
        }

        public void ClampValues()
        {
            _x = ((_x % 360) + 360) % 360;
            _y = ((_y % 360) + 360) % 360;
            _z = ((_z % 360) + 360) % 360;
        }

        public void SetValues(Vector3 v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
        }

        public void RaisePropertyChanged()
        {
            OnPropertyChanged("X");
            OnPropertyChanged("Y");
            OnPropertyChanged("Z");
        }
    }
}

