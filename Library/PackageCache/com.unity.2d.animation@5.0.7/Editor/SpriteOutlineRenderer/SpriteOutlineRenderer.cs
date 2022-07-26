using System.Collections.Generic;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace UnityEditor.U2D.Animation
{
    internal class SpriteOutlineRenderer
    {
        class OutlineRenderTexture
        {
            public Texture outlineTexture;
            public bool dirty;
        }

        Material m_OutlineMaterial;
        Material m_BitMaskMaterial;
        Dictionary<string, OutlineRenderTexture> m_OutlineTextureCache = new Dictionary<string, OutlineRenderTexture>();
        ISpriteEditorDataProvider m_CurrentDataProvider;
        SkinningEvents m_EventSystem;
        ISpriteEditor m_SpriteEditor;

        public SpriteOutlineRenderer(ISpriteEditor spriteEditor, SkinningEvents eventSystem)
        {
            m_BitMaskMaterial = new Material(Shader.Find("com.unity3d.animation/SpriteBitmask"));
            m_BitMaskMaterial.hideFlags = HideFlags.HideAndDontSave;
            m_OutlineMaterial = new Material(Shader.Find("com.unity3d.animation/SpriteOutline"));
            m_OutlineMaterial.hideFlags = HideFlags.HideAndDontSave;
            m_SpriteEditor = spriteEditor;
            m_EventSystem = eventSystem;
            m_EventSystem.meshPreviewChanged.AddListener(OnMeshPreviewChanged);
            m_EventSystem.selectedSpriteChanged.AddListener(OnSelectionChanged);
            CheckDataProviderChanged();
        }

        public void Dispose()
        {
            if (m_BitMaskMaterial != null)
                Object.DestroyImmediate(m_BitMaskMaterial);
            if (m_OutlineMaterial != null)
                Object.DestroyImmediate(m_OutlineMaterial);
            m_EventSystem.meshPreviewChanged.RemoveListener(OnMeshPreviewChanged);
            m_EventSystem.selectedSpriteChanged.RemoveListener(OnSelectionChanged);
            DestoryTextures();
        }

        private void OnMeshPreviewChanged(MeshPreviewCache mesh)
        {
            AddOrUpdateMaskTexture(mesh.sprite, true);
        }

        private void OnSelectionChanged(SpriteCache spriteCache)
        {
            CheckDataProviderChanged();
            AddOrUpdateMaskTexture(spriteCache, false);
        }

        private void DestoryTextures()
        {
            if (m_OutlineTextureCache != null)
            {
                foreach (var value in m_OutlineTextureCache.Values)
                {
                    if (value != null && value.outlineTexture != null)
                        Texture.DestroyImmediate(value.outlineTexture);
                }
                m_OutlineTextureCache.Clear();
            }
        }

        private void AddOrUpdateMaskTexture(SpriteCache sprite, bool regenerate)
        {
            SpriteCache selectedSprite = null;

            if (sprite != null)
                selectedSprite = sprite.skinningCache.selectedSprite;

            if (m_OutlineTextureCache != null && sprite != null)
            {
                if (!m_OutlineTextureCache.ContainsKey(sprite.id))
                    m_OutlineTextureCache.Add(sprite.id, new OutlineRenderTexture() {dirty = true});

                var outlineTextureCache = m_OutlineTextureCache[sprite.id];
                outlineTextureCache.dirty |= regenerate;

                if (sprite == selectedSprite)
                {
                    if (outlineTextureCache.dirty || outlineTextureCache.outlineTexture == null)
                    {
                        outlineTextureCache.outlineTexture = GenerateOutlineTexture(m_SpriteEditor, sprite, (RenderTexture)outlineTextureCache.outlineTexture);
                        if (outlineTextureCache.outlineTexture != null)
                        {
                            outlineTextureCache.outlineTexture.hideFlags = HideFlags.HideAndDontSave;
                            outlineTextureCache.dirty = false;
                        }
                    }
                    m_OutlineMaterial.mainTexture = outlineTextureCache.outlineTexture;
                }
            }
        }

        private void CheckDataProviderChanged()
        {
            var dp = m_SpriteEditor.GetDataProvider<ISpriteEditorDataProvider>();
            if (dp != m_CurrentDataProvider)
            {
                DestoryTextures();
                m_OutlineTextureCache.Clear();
                m_CurrentDataProvider = dp;
            }
        }

        internal void RenderSpriteOutline(ISpriteEditor spriteEditor, SpriteCache sprite)
        {
            if (sprite == null)
                return;

            if (Event.current.type == EventType.Repaint)
            {
                UnityEngine.Profiling.Profiler.BeginSample("SpriteOutlineRenderer::RenderSpriteOutline");
                m_OutlineMaterial.SetColor("_OutlineColor", SelectionOutlineSettings.outlineColor);
                m_OutlineMaterial.SetFloat("_AdjustLinearForGamma", PlayerSettings.colorSpace == ColorSpace.Linear ? 1.0f : 0.0f);
                var texture = spriteEditor.GetDataProvider<ITextureDataProvider>().texture;
                float outlineSize = Mathf.Max(texture.width, texture.height) * SelectionOutlineSettings.selectedSpriteOutlineSize / 1024.0f;
                m_OutlineMaterial.SetFloat("_OutlineSize", outlineSize);
                var mesh = GetMesh(sprite);
                m_OutlineMaterial.SetPass(0);
                GL.PushMatrix();
                GL.MultMatrix(Handles.matrix * sprite.GetLocalToWorldMatrixFromMode());

                Rect r = new Rect(mesh.bounds.min.x, mesh.bounds.min.y, mesh.bounds.size.x, mesh.bounds.size.y);
                GL.Begin(GL.QUADS);
                GL.Color(Color.white);
                GL.TexCoord(new Vector3(0, 0, 0));
                GL.Vertex3(r.xMin, r.yMin, 0);

                GL.TexCoord(new Vector3(1, 0, 0));
                GL.Vertex3(r.xMax, r.yMin, 0);

                GL.TexCoord(new Vector3(1, 1, 0));
                GL.Vertex3(r.xMax, r.yMax, 0);

                GL.TexCoord(new Vector3(0, 1, 0));
                GL.Vertex3(r.xMin, r.yMax, 0);
                GL.End();
                GL.PopMatrix();
                UnityEngine.Profiling.Profiler.EndSample();
            }
        }

        private Texture GenerateOutlineTexture(ISpriteEditor spriteEditor, SpriteCache spriteCache, RenderTexture reuseRT)
        {
            if (spriteCache != null && spriteCache.textureRect.width != 0 && spriteCache.textureRect.height != 0)
            {
                UnityEngine.Profiling.Profiler.BeginSample("SpriteOutlineRenderer::GenerateOutlineTexture");
                var mesh = GetMesh(spriteCache);
                var b = mesh.bounds;

                if (reuseRT == null)
                {
                    UnityEngine.Profiling.Profiler.BeginSample("SpriteOutlineRenderer::CreateRTNew");
                    reuseRT = new RenderTexture((int)b.size.x, (int)b.size.y, 24, RenderTextureFormat.ARGBHalf);
                    UnityEngine.Profiling.Profiler.EndSample();
                }
                else if (reuseRT.width != (int)b.size.x || reuseRT.height != (int)b.size.y)
                {
                    UnityEngine.Profiling.Profiler.BeginSample("SpriteOutlineRenderer::CreateRTReuse");
                    Object.DestroyImmediate(reuseRT);
                    reuseRT = new RenderTexture((int)b.size.x, (int)b.size.y, 24, RenderTextureFormat.ARGBHalf);
                    UnityEngine.Profiling.Profiler.EndSample();
                }
                m_BitMaskMaterial.mainTexture = spriteEditor.GetDataProvider<ITextureDataProvider>().texture;

                var oldRT = RenderTexture.active;
                Graphics.SetRenderTarget(reuseRT);
                m_BitMaskMaterial.SetPass(0);
                UnityEngine.Profiling.Profiler.BeginSample("SpriteOutlineRenderer::DrawMesh");
                GL.Clear(false, true, new Color(0, 0, 0, 0));
                GL.PushMatrix();
                GL.LoadOrtho();
                var h = b.size.y * 0.5f;
                var w = h * (b.size.x / b.size.y);
                GL.LoadProjectionMatrix(Matrix4x4.Ortho(-w, w, -h, h, -1, 1));
                GL.Begin(GL.QUADS);
                GL.Color(Color.white);
                Graphics.DrawMeshNow(mesh, Matrix4x4.Translate(-b.center));
                GL.End();
                GL.PopMatrix();
                Graphics.SetRenderTarget(oldRT);
                UnityEngine.Profiling.Profiler.EndSample();

                UnityEngine.Profiling.Profiler.EndSample();
                return reuseRT;
            }

            return null;
        }

        private Mesh GetMesh(SpriteCache sprite)
        {
            var meshPreview = sprite.GetMeshPreview();
            var skeleton = sprite.skinningCache.GetEffectiveSkeleton(sprite);

            Debug.Assert(meshPreview != null);
            Debug.Assert(skeleton != null);

            if (meshPreview.canSkin && skeleton.isPosePreview)
                return meshPreview.mesh;

            return meshPreview.defaultMesh;
        }
    }
}
