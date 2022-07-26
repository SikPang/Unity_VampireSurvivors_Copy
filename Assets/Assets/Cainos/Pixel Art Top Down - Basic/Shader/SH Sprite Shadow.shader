Shader "Cainos/Sprite Shadow"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)

		[IntRange] _StencilRef("Stencil Ref Value", Range(0,255)) = 0

		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		//Use stencil to make sure shadow get only drawn once
		//And only within object with shader "SH Sprite Shadow Mask"
		Stencil
		{
			Ref [_StencilRef]
			Comp Equal
			Pass Zero
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

			fixed4 Frag(v2f IN) : SV_Target
			{
				fixed4 c = IN.color * _Color;
				c.a *= SampleSpriteTexture(IN.texcoord).a;

				clip(c.a - 0.01f);

				c.rgb *= c.a;
				return c;
			}


		ENDCG
		}
	}
}
