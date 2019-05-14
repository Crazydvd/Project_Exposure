Shader "Handout/Wave" 
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		
		[MaterialToggle] _Xaxis("X-Axis", float) = 1
		[MaterialToggle] _Yaxis("Y-Axis", float) = 0
		[MaterialToggle] _Zaxis("Z-Axis", float) = 1
		_Amplitude("Amplitude", float) = 1
		_Frequency("Frequency", float) = 1
		_Unit("TimeUnit", float) = 1
	}
	SubShader 
	{
		Cull Off

		Pass 
		{
			CGPROGRAM
			#pragma vertex myVertexShader
			#pragma fragment myFragmentShader
			#define UNITY_SHADER_NO_UPGRADE 1

			struct vertexInput 
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vertexToFragment 
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
			};

			float _Xaxis;
			float _Yaxis;
			float _Zaxis;
			float _Amplitude;
			float _Frequency;
			float _Unit;

			vertexToFragment myVertexShader (vertexInput v) 
			{
				vertexToFragment o;
				o.uv = v.uv;
				// Transform the point to clip space:
				o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
				o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
				float y = 0;
				float PI = 3.141592653589;

				float frequency = _Frequency * 2 * PI * (1 / _Unit);

				if (_Xaxis)
				{
					y += _Amplitude * sin(frequency * o.worldPos.x);
				}
				if (_Yaxis)
				{
					o.vertex.x += _Amplitude * sin(frequency * o.worldPos.y);
				}
				if (_Zaxis)
				{
					y += _Amplitude * sin(frequency * o.worldPos.z);
				}

				o.vertex.y += y;
				return o;
			}
			
			sampler2D _MainTex;
			fixed4 myFragmentShader(vertexToFragment i) : SV_Target
			{
				return tex2D(_MainTex,i.uv);
			}
			ENDCG
		}
	}
}

