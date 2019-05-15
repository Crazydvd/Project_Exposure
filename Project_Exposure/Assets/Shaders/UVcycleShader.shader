Shader "Handout/UVCycle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TimeScaleY("TimeScaleY", float) = 10
		_TimeScaleX("TimeScaleX", float) = 10

		[MaterialToggle] _Xaxis ("xDirection", float) = 0
		[MaterialToggle] _Yaxis ("yDirection", float) = 1
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#define UNITY_SHADER_NO_UPGRADE 1

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float _TimeScaleY;
			float _TimeScaleX;
			float _Xaxis;
			float _Yaxis;

			v2f vert (appdata v)
			{
				v2f o;
				// Transform the point to clip space:
				o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
				// Cycle the UVs:
				o.uv = v.uv + float2(_Time.x * _TimeScaleX * _Xaxis, _Time.x * _TimeScaleY * _Yaxis);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
