using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using SharpGL;
using SharpGL.Enumerations;
using AngleInterpolation.ViewModel;

namespace AngleInterpolation.Model
{
    public class AxisDetails : ViewModelBase
    {
        #region Private Members

        private Vector3 _position;
        private Vector3 _rotation;

        private uint _millDrawListId;
        private const int CylinderDivisions = 20;
        private const double Radius = 0.2;
        private const double Height = 2;

        #endregion Private Members

        #region Public Properties

        /// <summary>
        /// Gets or sets the position of the axis.
        /// </summary>
        public Vector3 Position
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
        public Vector3 Rotation
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

        public AxisDetails(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
            _millDrawListId = uint.MinValue;
        }

        #endregion Constructors

        #region Private Methods

        private void CreateDrawList(OpenGL gl)
        {
            // Prepare a draw list for drawing a cyllinder.
            _millDrawListId = gl.GenLists(1);
            gl.NewList(_millDrawListId, OpenGL.GL_COMPILE);

            List<int> indices;
            var vertices = CreateCylinder(out indices);

            gl.Begin(BeginMode.Triangles);
            foreach (int index in indices)
            {
                var vertex = vertices[index];
                gl.Normal(vertex.X / Radius, 0, vertex.Z / Radius);
                gl.Vertex(vertex.X, vertex.Y, vertex.Z);
            }
            gl.End();

            gl.Begin(BeginMode.TriangleFan);
            for (int i = CylinderDivisions; i >= 0; i--)
            {
                var degree = i * Math.PI * 2 / CylinderDivisions;
                gl.Normal(0, 1, 0);
                gl.Vertex(Math.Cos(degree) * Radius, Height, Math.Sin(degree) * Radius);
            }
            gl.End();

            gl.EndList();
        }

        private List<Vector3D> CreateCylinder(out List<int> indices)
        {
            var vertices = new List<Vector3D>();
            for (double y = 0; y < 2; y++)
                for (double x = 0; x < CylinderDivisions; x++)
                {
                    double theta = (x / (CylinderDivisions - 1)) * 2 * Math.PI;

                    vertices.Add(new Vector3D
                    {
                        X = Radius * Math.Cos(theta),
                        Y = Height * y,
                        Z = Radius * Math.Sin(theta),
                    });
                }

            indices = new List<int>();
            for (int x = 0; x < CylinderDivisions - 1; x++)
            {
                indices.Add(x);
                indices.Add(x + CylinderDivisions);
                indices.Add(x + CylinderDivisions + 1);

                indices.Add(x + CylinderDivisions + 1);
                indices.Add(x + 1);
                indices.Add(x);
            }
            return vertices;
        }

        #endregion

        public void Render(OpenGL gl, Vector3 position, Vector3 rotation)
        {
            if (_millDrawListId == uint.MinValue) CreateDrawList(gl);

            gl.Color(255, 255, 255);
            gl.MatrixMode(MatrixMode.Modelview);
            gl.PushMatrix();
            gl.Translate(position.X, position.Z, position.Y);
            gl.Rotate(rotation.X, 1, 0, 0);
            gl.Rotate(rotation.Y, 0, 1, 0);
            gl.Rotate(rotation.Z, 0, 0, 1);
            gl.CallList(_millDrawListId);
            gl.Rotate(90, 1, 0, 0);
            gl.CallList(_millDrawListId);
            gl.Rotate(-90, 0, 0, 1);
            gl.CallList(_millDrawListId);
            gl.PopMatrix();
        }

        public virtual void Render(OpenGL gl)
        {
            Render(gl, Position, Rotation);
        }
    }
}