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
    }
}

