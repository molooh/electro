﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fusee.Engine;
using Fusee.Math;
using Fusee.Serialization;

namespace Examples.SceneViewer
{
   

    public class SceneRenderer
    {
        private Dictionary<MeshContainer, Mesh> _mm;
        private SceneContainer _sc;

        private ShaderProgram _shader;
        private IShaderParam _colorParam;


        public SceneRenderer(SceneContainer sc)
        {
            _sc = sc;
            _mm = new Dictionary<MeshContainer, Mesh>();
        }

        public void Render(RenderContext rc)
        {
            if (_shader == null)
            {
                _shader = MoreShaders.GetDiffuseColorShader(rc);
                _colorParam = _shader.GetShaderParam("color");
            }

            rc.SetShader(_shader);

            foreach (var soc in _sc.Children)
            {
                VisitNode(soc, rc);
            }
        }

        protected void VisitNode(SceneObjectContainer soc, RenderContext rc)
        {
            float4x4 origMV = rc.ModelView;

            rc.ModelView = rc.ModelView*soc.Transform;
            rc.SetShaderParam(_colorParam, new float4(soc.Color.x, soc.Color.y, soc.Color.z, 1));
            if (soc.Mesh != null)
            {
                Mesh rm;
                if (!_mm.TryGetValue(soc.Mesh, out rm))
                {
                    rm = new Mesh()
                    {
                        Colors = null,
                        Normals = soc.Mesh.Normals,
                        UVs = soc.Mesh.UVs,
                        Vertices = soc.Mesh.Vertices,
                        Triangles = soc.Mesh.Triangles
                    };
                    _mm.Add(soc.Mesh, rm);
                }
                rc.Render(rm);
            }

            if (soc.Children != null)
            {
                foreach (var child in soc.Children)
                {
                    VisitNode(child, rc);
                }
            }

            rc.ModelView = origMV;
        }
    }
}