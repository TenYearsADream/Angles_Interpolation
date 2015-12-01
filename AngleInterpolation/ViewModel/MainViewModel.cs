using System;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace AngleInterpolation.ViewModel
{
    /// <summary>
    /// Application's main view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Private Members

        private AxisDetails _startAxis;
        private AxisDetails _endAxis;

        private QuaternionAxis _quaternionAxis;
        private EulerAxis _eulerAxis;

        private int _frameCount;
        private bool _showAllFrames;

        private DelegateCommand _startAnimationCommand;

        private DispatcherTimer _timer;
        private DateTime _timerStartTime;

        #endregion Private Members

        #region Public Properties

        /// <summary>
        /// Gets or sets the start axis details.
        /// </summary>
        public AxisDetails StartAxis
        {
            get { return _startAxis; }
            set
            {
                if (_startAxis == value) return;
                _startAxis = value;
                OnPropertyChanged("StartAxis");
            }
        }

        /// <summary>
        /// Gets or sets the end axis details.
        /// </summary>
        public AxisDetails EndAxis
        {
            get { return _endAxis; }
            set
            {
                if (_endAxis == value) return;
                _endAxis = value;
                OnPropertyChanged("EndAxis");
            }
        }

        /// <summary>
        /// Gets or sets the frame count.
        /// </summary>
        public int FrameCount
        {
            get { return _frameCount; }
            set
            {
                if (_frameCount == value) return;
                _frameCount = value;
                OnPropertyChanged("FrameCount");
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether all frames should be shown.
        /// </summary>
        public bool ShowAllFrames
        {
            get { return _showAllFrames; }
            set
            {
                if (_showAllFrames == value) return;
                _showAllFrames = value;
                OnPropertyChanged("ShowAllFrames");
            }
        }

        /// <summary>
        /// Gets the StartAnimation Command.
        /// </summary>
        public DelegateCommand StartAnimationCommand
        {
            get { return _startAnimationCommand ?? (_startAnimationCommand = new DelegateCommand(StartAnimation)); }
        }

        #endregion Public Properties

        #region Constructors

        public MainViewModel()
        {
            StartAxis = new AxisDetails(new Point3D(0, 0, 0), new Point3D(0, 0, 0));
            EndAxis = new AxisDetails(new Point3D(0, 0, 0), new Point3D(0, 0, 0));
            
            _timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 10) };
            _timer.Tick += _timer_Tick;
        }
        
        #endregion Constructors

        #region Private Methods

        private void StartAnimation(object obj)
        {
            _quaternionAxis = new QuaternionAxis(StartAxis.Position, StartAxis.Rotation);
            _eulerAxis = new EulerAxis(StartAxis.Position, StartAxis.Rotation);

            _timerStartTime = DateTime.Now;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            var timeDelta = DateTime.Now - _timerStartTime;

            _quaternionAxis.UpdatePosition(timeDelta);
            _eulerAxis.UpdatePosition(timeDelta);
        }

        #endregion Private Methods
    }
}