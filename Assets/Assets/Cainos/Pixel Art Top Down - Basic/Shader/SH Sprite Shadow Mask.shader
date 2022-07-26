Shader "Cainos/Sprite Shadow Mask"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[HDR] _Color ("Tint", Color) = (1,1,1,1)
		_AlphaClip ("Alpha Clip" , Float) = 0.05

		[IntRange] _StencilRef("Stencil Ref Value", Range(0,255)) = 0

		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			//Work together with shader "SH Sprite Shadow"
			//to make sure shadow only get drawn within objects with this shader
			Stencil
			{
				Ref[_StencilRef]
				Comp Always
				Pass Replace
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex SpriteVert
				#pragma fragment Frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
				#include "UnitySprites.cginc"

				float _AlphaClip;

			fixed4 Frag(v2f IN) : SV_Target
			{


				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

				clip(c.a - _AlphaClip);

				c.rgb *= c.a;
				return c;
			}

		ENDCG
		}
	}
}
