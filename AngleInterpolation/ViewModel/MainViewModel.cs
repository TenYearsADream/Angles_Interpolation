using System;
using System.Windows.Threading;
using SharpGL;
using SharpGL.Enumerations;
using AngleInterpolation.Model;

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

        private DelegateCommand _startAnimationCommand;
        private DelegateCommand _resetAnimationCommand;
        private DelegateCommand _showAllFramesCommand;

        private DispatcherTimer _timer;
        private DateTime _timerStartTime;
        private double _viewportHeight;
        private double _viewportWidth;

        #endregion Private Members

        #region Public Properties

        /// <summary>
        /// Gets or sets the width of the viewport.
        /// </summary>
        public double ViewportWidth
        {
            get { return _viewportWidth; }
            set
            {
                if (_viewportWidth == value) return;
                _viewportWidth = value;
                OnPropertyChanged("ViewportWidth");
            }
        }

        /// <summary>
        /// Gets or sets the height of the viewport.
        /// </summary>
        public double ViewportHeight
        {
            get { return _viewportHeight; }
            set
            {
                if (_viewportHeight == value) return;
                _viewportHeight = value;
                OnPropertyChanged("ViewportHeight");
            }
        }

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
        /// Gets or sets the quaternion axis details.
        /// </summary>
        public QuaternionAxis QuaternionAxis
        {
            get { return _quaternionAxis; }
            set
            {
                if (_quaternionAxis == value) return;
                _quaternionAxis = value;
                OnPropertyChanged("QuaternionAxis");
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
        /// Gets or sets the ShowAllFrames Command.
        /// </summary>
        public DelegateCommand ShowAllFramesCommand
        {
            get { return _showAllFramesCommand ?? (_showAllFramesCommand = new DelegateCommand(ShowAllFrames)); }
        }

        /// <summary>
        /// Gets the StartAnimation Command.
        /// </summary>
        public DelegateCommand StartAnimationCommand
        {
            get { return _startAnimationCommand ?? (_startAnimationCommand = new DelegateCommand(StartAnimation)); }
        }

        /// <summary>
        /// Gets the ResetAnimation Command.
        /// </summary>
        public DelegateCommand ResetAnimationCommand
        {
            get { return _resetAnimationCommand ?? (_resetAnimationCommand = new DelegateCommand(ResetAnimation)); }
        }

        #endregion Public Properties

        #region Constructors

        public MainViewModel()
        {
            StartAxis = new AxisDetails(new Vector3(-20, 0, 10), new Vector3(0, 0, 0));
            EndAxis = new AxisDetails(new Vector3(20, 0, -10), new Vector3(0, 0, 0));

            _quaternionAxis = new QuaternionAxis(StartAxis.Position, StartAxis.Rotation, EndAxis.Position, EndAxis.Rotation);
            _eulerAxis = new EulerAxis(StartAxis.Position, StartAxis.Rotation, EndAxis.Position, EndAxis.Rotation);

            _timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 10) };
            _timer.Tick += _timer_Tick;
        }

        #endregion Constructors

        #region Private Methods

        private void ClearRenderState(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            InitializeMatrices(gl);
        }

        private void InitializeMatrices(OpenGL gl)
        {
            gl.MatrixMode(MatrixMode.Modelview);
            gl.LoadIdentity();
            gl.Translate(0.0f, 0.0f, -50.0f);
        }

        private void StartAnimation(object obj)
        {
            _timerStartTime = DateTime.Now;
            _timer.Start();
        }

        private void ResetAnimation(object obj)
        {
            _timer.Stop();

            _quaternionAxis = new QuaternionAxis(StartAxis.Position, StartAxis.Rotation, EndAxis.Position, EndAxis.Rotation);
            _eulerAxis = new EulerAxis(StartAxis.Position, StartAxis.Rotation, EndAxis.Position, EndAxis.Rotation);
        }

        private void ShowAllFrames(object obj)
        {
            ResetAnimation(obj);
            _quaternionAxis.ShowAllFrames();
            _eulerAxis.ShowAllFrames();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            var timeDelta = DateTime.Now - _timerStartTime;

            _quaternionAxis.UpdatePosition(timeDelta);
            _eulerAxis.UpdatePosition(timeDelta);
        }

        #endregion Private Methods

        #region Public Properties

        public void RenderEuler(OpenGL gl)
        {
            ClearRenderState(gl);
            if (_eulerAxis != null)
                _eulerAxis.Render(gl);
        }

        public void RenderQuaternion(OpenGL gl)
        {
            ClearRenderState(gl);
            if (_quaternionAxis != null)
                _quaternionAxis.Render(gl);
        }

        #endregion Public Properties
    }
}