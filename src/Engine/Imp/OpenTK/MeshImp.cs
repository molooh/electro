namespace Fusee.Engine
{
    /// <summary>
    /// This is the implementation of the <see cref="IMeshImp" /> interface. 
    /// It is used to check the status of the informations of a mesh and flush informations if required.
    /// </summary>
    public class MeshImp : IMeshImp
    {
        #region Internal Fields
        internal int VertexBufferObject;
        internal int NormalBufferObject;
        internal int ColorBufferObject;
        internal int UVBufferObject;
        internal int ElementBufferObject;
        internal int NElements;

        internal bool VertexBufferValid;
        internal bool NormalBufferValid;
        internal bool ColorBufferValid;
        internal bool UVBufferValid;
        internal bool ElementBufferValid;
        #endregion

        #region Public Fields & Members pairs
        /// <summary>
        /// Invalidates the vertices.
        /// </summary>
        public void InvalidateVertices()
        {
            VertexBufferValid = false;
        }
        /// <summary>
        /// Gets a value indicating whether [vertices set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [vertices set]; otherwise, <c>false</c>.
        /// </value>
        public bool VerticesSet { get { return VertexBufferValid; } }

        /// <summary>
        /// Invalidates the normals.
        /// </summary>
        public void InvalidateNormals()
        {
            NormalBufferValid = false;
        }
        /// <summary>
        /// Gets a value indicating whether [normals set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [normals set]; otherwise, <c>false</c>.
        /// </value>
        public bool NormalsSet { get { return NormalBufferValid; } }

        /// <summary>
        /// Implementation Tasks: Invalidates the colors, e.g. reset the ColorBufferObject of this instance by setting it to 0.
        /// </summary>
        public void InvalidateColors()
        {
            ColorBufferValid = false;
        }
        /// <summary>
        /// Gets a value indicating whether [colors set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [colors set]; otherwise, <c>false</c>.
        /// </value>
        public bool ColorsSet { get { return ColorBufferValid; } }

        /// <summary>
        /// Gets a value indicating whether [u vs set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [u vs set]; otherwise, <c>false</c>.
        /// </value>
        public bool UVsSet { get { return UVBufferValid; } }

        /// <summary>
        /// Invalidates the UV's.
        /// </summary>
        public void InvalidateUVs()
        {
            UVBufferValid = false;
        }

        /// <summary>
        /// Invalidates the triangles.
        /// </summary>
        public void InvalidateTriangles()
        {
            ElementBufferValid = false;
            ElementBufferObject = 0;
            NElements = 0;
        }
        /// <summary>
        /// Gets a value indicating whether [triangles set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [triangles set]; otherwise, <c>false</c>.
        /// </value>
        public bool TrianglesSet { get { return ElementBufferValid; } }
        #endregion
    }
}
