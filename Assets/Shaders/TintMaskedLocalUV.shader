Shader "Unlit/TintMaskedLocalUV"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Accent ("Accent", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = float2(o.vertex.x, o.vertex.y) * 2.0f;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			float4 _Color;
			float4 _Accent;
			
			float4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float4 tintMap = tex2D(_MainTex, i.uv);

				return (tintMap.x * _Color) + (tintMap.y * _Accent);
			}
			ENDCG
		}
	}
}
