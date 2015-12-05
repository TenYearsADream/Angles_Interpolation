using System;
using System.Windows;
using System.Windows.Input;
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

        private Vector _rotation;
        private double _scale;
        private Point _previousMousePosition;

        private AxisDetails _startAxis;
        private AxisDetails _endAxis;

        private QuaternionAxis _quaternionAxis;
        private EulerAxis _eulerAxis;

        private int _frameCount;

        private DelegateCommand _startAnimationCommand;
        private DelegateCommand _resetAnimationCommand;
        private DelegateCommand _showAllFramesCommand;

        private const int AnimationTime = 10000;

        private DispatcherTimer _timer;
        private DateTime _timerStartTime;
        private double _viewportHeight;
        private double _viewportWidth;

        double[] _modelview = new double[16];
        double[] _projection = new double[16];
        int[] _viewport = new int[4];

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
            _scale = 1.0;
            _rotation = new Vector(0, 0);

            StartAxis = new AxisDetails(new Vector3(-20, 0, 0), new Vector3(0, 0, 0));
            EndAxis = new AxisDetails(new Vector3(20, 0, -10), new Vector3(0, 0, 0));

            _quaternionAxis = new QuaternionAxis(StartAxis.Position, StartAxis.Rotation, EndAxis.Position, EndAxis.Rotation);
            _eulerAxis = new EulerAxis(StartAxis.Position, StartAxis.Rotation, EndAxis.Position, EndAxis.Rotation);

            _timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 10) };
            _timer.Tick += _timer_Tick;

            FrameCount = 3;
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

            gl.Scale(_scale, _scale, _scale);
            gl.Rotate(_rotation.X, 0.0f, 1.0f, 0.0f);
            gl.Rotate(_rotation.Y, 1.0f, 0.0f, 0.0f);

            gl.GetDouble(OpenGL.GL_MODELVIEW_MATRIX, _modelview);
            gl.GetDouble(OpenGL.GL_PROJECTION_MATRIX, _projection);
            gl.GetInteger(OpenGL.GL_VIEWPORT, _viewport);
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
            _quaternionAxis.ShowAllFrames(AnimationTime, FrameCount);
            _eulerAxis.ShowAllFrames(AnimationTime, FrameCount);
        }

        private Point FindPointPositionInViewport()
        {
            double x = 0, y = 0, z = 0;
            var gl = new OpenGL();
            gl.UnProject(_previousMousePosition.X, _previousMousePosition.Y, 0, _modelview, _projection, _viewport, ref x, ref y, ref z);

            return new Point(x, y);
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            var timeDelta = (DateTime.Now - _timerStartTime).TotalMilliseconds;

            _quaternionAxis.UpdatePosition(timeDelta, AnimationTime);
            _eulerAxis.UpdatePosition(timeDelta, AnimationTime);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Renders the OpenGl control presenting euler transformations.
        /// </summary>
        /// <param name="gl">The gl.</param>
        public void RenderEuler(OpenGL gl)
        {
            ClearRenderState(gl);
            if (_eulerAxis != null)
                _eulerAxis.Render(gl);
        }

        /// <summary>
        /// Renders the OpenGl control presenting quaternion transformations.
        /// </summary>
        /// <param name="gl">The gl.</param>
        public void RenderQuaternion(OpenGL gl)
        {
            ClearRenderState(gl);
            if (_quaternionAxis != null)
                _quaternionAxis.Render(gl);
        }

        public void MouseWheelMove(MouseWheelEventArgs eventArgs)
        {
            _scale += eventArgs.Delta / 500.0;
        }

        public void MouseMove(MouseEventArgs eventArgs)
        {
            if (eventArgs.RightButton == MouseButtonState.Pressed)
            {
                _rotation.X += (eventArgs.GetPosition(null).X - _previousMousePosition.X) / 50;
                _rotation.Y += (eventArgs.GetPosition(null).Y - _previousMousePosition.Y) / 50;
            }
        }

        public void MouseDown(IInputElement sender, MouseButtonEventArgs eventArgs)
        {
            _previousMousePosition = eventArgs.GetPosition(sender);

            if (Keyboard.IsKeyDown(Key.S))
            {
                var point = FindPointPositionInViewport();

                StartAxis.Position.X = point.X * ViewportWidth / 1.6;
                StartAxis.Position.Y = -point.Y * ViewportHeight / 1.4;
                StartAxis.Position.Z = 0;
            }
            else if (Keyboard.IsKeyDown(Key.E))
            {
                var point = FindPointPositionInViewport();

                EndAxis.Position.X = point.X * ViewportWidth / 1.6;
                EndAxis.Position.Y = -point.Y * ViewportHeight / 1.4;
                EndAxis.Position.Z = 0;
            }
        }

        #endregion Public Methods
    }
}