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
        private DelegateCommand _resetViewportCommand;

        private DispatcherTimer _timer;
        private DateTime _timerStartTime;
        private double _viewportHeight;
        private double _viewportWidth;

        double[] _modelview = new double[16];
        double[] _projection = new double[16];
        int[] _viewport = new int[4];
        private int _animationLength;

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
        /// Gets or sets the Animation Length.
        /// </summary>
        public int AnimationLength
        {
            get { return _animationLength; }
            set
            {
                if (_animationLength == value) return;
                _animationLength = value;
                OnPropertyChanged("AnimationLength");
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

        /// <summary>
        /// Gets the ResetViewport Command.
        /// </summary>
        public DelegateCommand ResetViewportCommand
        {
            get { return _resetViewportCommand ?? (_resetViewportCommand = new DelegateCommand(ResetViewport)); }
        }

        #endregion Public Properties

        #region Constructors

        public MainViewModel()
        {
            _scale = 1.0;
            _rotation = new Vector(0, 0);

            StartAxis = new AxisDetails(new Vector3(-20, 0, 0), new Vector3(0, 0, 0));
            EndAxis = new AxisDetails(new Vector3(20, 0, -10), new Vector3(0, 0, 0));

            _quaternionAxis = new QuaternionAxis(StartAxis.Position, StartAxis.QuaternionRotation, EndAxis.Position, EndAxis.QuaternionRotation);
            _eulerAxis = new EulerAxis(StartAxis.Position, StartAxis.Rotation, EndAxis.Position, EndAxis.Rotation);

            _timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 10) };
            _timer.Tick += _timer_Tick;
            AnimationLength = 7;

            FrameCount = 13;
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
            gl.Rotate(_rotation.Y, 0.0f, 0.0f, 1.0f);

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

            _quaternionAxis = new QuaternionAxis(StartAxis.Position, StartAxis.QuaternionRotation, EndAxis.Position, EndAxis.QuaternionRotation);
            _eulerAxis = new EulerAxis(StartAxis.Position, StartAxis.Rotation, EndAxis.Position, EndAxis.Rotation);
        }

        private void ResetViewport(object obj)
        {
            _scale = 1.0;
            _rotation = new Vector(0, 0);
        }

        private void ShowAllFrames(object obj)
        {
            ResetAnimation(obj);
            _quaternionAxis.ShowAllFrames(AnimationLength * 1000, FrameCount);
            _eulerAxis.ShowAllFrames(AnimationLength * 1000, FrameCount);
        }

        private Point FindPointPositionInViewport()
        {
            double x = 0, y = 0, z = 0;
            var gl = new OpenGL();
            gl.UnProject(_previousMousePosition.X, _previousMousePosition.Y, 0, _modelview, _projection, _viewport, ref x, ref y, ref z);

            return new Point(x, y);
        }

        private void DrawAxis(OpenGL gl)
        {
            gl.PushMatrix();
            gl.LoadIdentity();

            gl.Translate(-0.85, -0.7, -2.0);
            gl.Rotate(_rotation.X, 1.0, 0.0, 0.0);
            gl.Rotate(_rotation.Y, 0.0, 1.0, 0.0);

            gl.Begin(BeginMode.Lines);

            // draw line for x axis
            gl.Color(1.0, 0.0, 0.0);
            gl.Vertex(0.0, 0.0, 0.0);
            gl.Vertex(0.1, 0.0, 0.0);

            gl.Vertex(0.11, 0.0, 0.0);
            gl.Vertex(0.12, -0.01, 0.0);
            gl.Vertex(0.11, -0.01, 0.0);
            gl.Vertex(0.12, 0.0, 0.0);

            // draw line for z axis
            gl.Color(0.0, 1.0, 0.0);
            gl.Vertex(0.0, 0.0, 0.0);
            gl.Vertex(0.0, 0.1, 0.0);

            gl.Vertex(0.02, 0.1, 0.0);
            gl.Vertex(0.01, 0.1, 0.0);
            gl.Vertex(0.01, 0.1, 0.0);
            gl.Vertex(0.02, 0.11, 0.0);
            gl.Vertex(0.02, 0.11, 0.0);
            gl.Vertex(0.01, 0.11, 0.0);

            // draw line for y axis
            gl.Color(0.0, 0.0, 1.0);
            gl.Vertex(0.0, 0.0, 0.0);
            gl.Vertex(0.0, 0.0, 0.1);

            gl.Vertex(0.0, 0.01, 0.1);
            gl.Vertex(0.0, 0.02, 0.1);
            gl.Vertex(0.01, 0.03, 0.1);
            gl.Vertex(0.0, 0.02, 0.1);
            gl.Vertex(-0.01, 0.03, 0.1);
            gl.Vertex(0.0, 0.02, 0.1);

            gl.End();
            gl.PopMatrix();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            var timeDelta = (DateTime.Now - _timerStartTime).TotalMilliseconds;

            _quaternionAxis.UpdatePosition(timeDelta, AnimationLength * 1000);
            _eulerAxis.UpdatePosition(timeDelta, AnimationLength * 1000);
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
            DrawAxis(gl);

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
            DrawAxis(gl);

            if (_quaternionAxis != null)
                _quaternionAxis.Render(gl);
        }

        public void MouseWheelMove(MouseWheelEventArgs eventArgs)
        {
            _scale += eventArgs.Delta / 500.0;
        }

        public void MouseMove(IInputElement sender, MouseEventArgs eventArgs)
        {
            if (eventArgs.RightButton == MouseButtonState.Pressed)
            {
                if (Keyboard.IsKeyDown(Key.S))
                {
                    StartAxis.Rotation.X += (eventArgs.GetPosition(sender).X - _previousMousePosition.X) / 2.0;
                    StartAxis.Rotation.Y += (eventArgs.GetPosition(sender).Y - _previousMousePosition.Y) / 2.0;
                }
                else if (Keyboard.IsKeyDown(Key.E))
                {
                    EndAxis.Rotation.X += (eventArgs.GetPosition(sender).X - _previousMousePosition.X) / 2.0;
                    EndAxis.Rotation.Y += (eventArgs.GetPosition(sender).Y - _previousMousePosition.Y) / 2.0;
                }
                else
                {
                    _rotation.X += (eventArgs.GetPosition(sender).X - _previousMousePosition.X) / 2.0;
                    _rotation.Y += (eventArgs.GetPosition(sender).Y - _previousMousePosition.Y) / 2.0;
                }

                _previousMousePosition = eventArgs.GetPosition(sender);
            }
        }

        public void MouseDown(IInputElement sender, MouseButtonEventArgs eventArgs)
        {
            _previousMousePosition = eventArgs.GetPosition(sender);

            if (eventArgs.LeftButton != MouseButtonState.Pressed) return;

            if (Keyboard.IsKeyDown(Key.S))
            {
                var point = FindPointPositionInViewport();

                StartAxis.Position.X = point.X * ViewportWidth / 1.6;
                StartAxis.Position.Y = 0;
                StartAxis.Position.Z = -point.Y * ViewportHeight / 1.4;
            }
            else if (Keyboard.IsKeyDown(Key.E))
            {
                var point = FindPointPositionInViewport();

                EndAxis.Position.X = point.X * ViewportWidth / 1.6;
                EndAxis.Position.Y = 0;
                EndAxis.Position.Z = -point.Y * ViewportHeight / 1.4;
            }
        }

        #endregion Public Methods
    }
}