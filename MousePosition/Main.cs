using Fusee.Engine;
using Fusee.Math;

namespace Examples.MousePosition
{
    public class MousePosition : RenderCanvas
    {

        private Point _mousePos;
        private float4x4 camMatrix;
        private Mesh _meshCube;

        // variables for shader
        private ShaderProgram _spColor;
        private ShaderProgram _spTexture;
        private IShaderParam _colorParam;
        private IShaderParam _textureParam;
        private ITexture _iTex;
        private float4x4 mtxCam = float4x4.CreateTranslation(0, 0, -10);

        public override void Init()
        {
            // is called on startup
            RC.ClearColor = new float4(1, 1, 1, 1);
            _meshCube = MeshReader.LoadMesh(@"Assets/cube.obj.model");
            _spColor = MoreShaders.GetDiffuseColorShader(RC);
            _colorParam = _spColor.GetShaderParam("color");
        }

        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // is called once a frame
            if (Input.Instance.IsButton(MouseButtons.Left))
            {
                // calculation of projection-coordinates from screen-coordinates
                _mousePos = Input.Instance.GetMousePos();

                float xProj = ((2*_mousePos.x)/(float) Width) - 1;
                float yProj = -1*(((2*_mousePos.y)/(float) Height) - 1);

                RC.ModelView = mtxCam;
                float4 worldCorClick  = RC.InvModelViewProjection * new float4(xProj, yProj, 0.2f, 1);

                // calculation of the collision between xz-plain and ray
                float4 camPos = new float4(0, 0, -10, 1);
                float4 rayDirection = (worldCorClick - camPos);
                
                /*
                float t = -camPos.y/rayDirection.y;
                float xGrab = camPos.x + t*rayDirection.x;
                float yGrab = 0;
                float zGrab = camPos.z + t*rayDirection.z;
                */

                RC.ModelView = float4x4.CreateTranslation((worldCorClick + rayDirection).xyz) * mtxCam;
                RC.SetShader(_spColor);
                RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));

                RC.Render(_meshCube);
            }
            // swap buffers
            Present();
        }

        public override void Resize()
        {
            // is called when the window is resized
            RC.Viewport(0, 0, Width, Height);

            var aspectRatio = Width / (float)Height;
            RC.Projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 10000);
        }

        public static void Main()
        {
            var app = new MousePosition();
            app.Run();
        }

    }
}
