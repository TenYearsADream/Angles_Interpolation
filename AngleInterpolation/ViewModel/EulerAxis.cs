using System.Windows.Media.Media3D;

namespace AngleInterpolation.ViewModel
{
    public class EulerAxis : AxisDetails
    {
        #region Constructors

        public EulerAxis(Point3D position, Point3D rotation)
            : base(position, rotation)
        {
        }

        #endregion Constructors
    }
}