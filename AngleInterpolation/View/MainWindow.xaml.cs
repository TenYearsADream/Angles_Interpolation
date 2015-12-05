using System.Windows;
using System.Windows.Input;
using AngleInterpolation.ViewModel;
using SharpGL;
using SharpGL.SceneGraph;

namespace AngleInterpolation.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainViewModel _viewModel;
        private bool _isMouseDown;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void QuaternionOpenGLControl_OnOpenGLDraw(object sender, OpenGLEventArgs args)
        {
            _viewModel.RenderQuaternion(args.OpenGL);
        }

        private void EulerOpenGLControl_OnOpenGLDraw(object sender, OpenGLEventArgs args)
        {
            _viewModel.RenderEuler(args.OpenGL);
        }

        private void OpenGLControl_OnOpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            OpenGL gl = args.OpenGL;

            gl.Enable(OpenGL.GL_DEPTH_TEST);

            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, new[] { 0.2f, 0.2f, 0.2f, 1.0f });

            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, new[] { 0.5f, 0.5f, 0.5f, 1.0f });
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, new[] { 0.0f, 5.0f, 10.0f, 1.0f });
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, new[] { 0.2f, 0.2f, 0.2f, 1.0f });
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, new[] { 0.3f, 0.3f, 0.3f, 1.0f });
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, new[] { 0.8f, 0.8f, 0.8f, 1.0f });
            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);

            gl.ShadeModel(OpenGL.GL_SMOOTH);
        }

        private void OpenGLControl_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            _viewModel.MouseDown((IInputElement)sender, e);
        }

        private void OpenGLControl_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
                _viewModel.MouseMove(e);
        }

        private void OpenGLControl_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

        private void OpenGLControl_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            _viewModel.MouseWheelMove(e);
        }
    }
}
