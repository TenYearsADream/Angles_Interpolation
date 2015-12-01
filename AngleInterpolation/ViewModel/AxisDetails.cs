using System;
using System.Windows.Media.Media3D;

namespace AngleInterpolation.ViewModel
{
    public class AxisDetails : ViewModelBase
    {
        #region Private Members

        private Point3D _position;
        private Point3D _rotation;

        #endregion Private Members

        #region Public Properties

        /// <summary>
        /// Gets or sets the position of the axis.
        /// </summary>
        public Point3D Position
        {
            get { return _position; }
            set
            {
                if (_position == value) return;
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        /// <summary>
        /// Gets or sets the rotation of the axis.
        /// </summary>
        public Point3D Rotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation == value) return;
                _rotation = value;
                OnPropertyChanged("Rotation");
            }
        }

        #endregion Public Properties

        #region Constructors

        public AxisDetails(Point3D position, Point3D rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        #endregion Constructors

        public virtual void UpdatePosition(TimeSpan t)
        {
            
        }
    }
}