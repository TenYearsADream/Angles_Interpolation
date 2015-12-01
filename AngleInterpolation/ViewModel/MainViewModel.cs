using System;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;

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

        #region Public Properties

        public void RenderEuler(OpenGL gl)
        {
            ClearRenderState(gl);
        }

        public void RenderQuaternion(OpenGL gl)
        {
            ClearRenderState(gl);
        }

        #endregion Public Properties
    }
}