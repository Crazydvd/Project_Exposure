Shader "Custom/Wave" 
{
	Properties
	{
		_Color("Color", color) = (1, 1, 1, 0)
		_MainTex ("Texture", 2D) = "white" {}	

		[MaterialToggle] _Local("ModelSpace", float) = 0
		[MaterialToggle] _Xaxis("X-Axis", float) = 1
		[MaterialToggle] _Yaxis("Y-Axis", float) = 0
		[MaterialToggle] _Zaxis("Z-Axis", float) = 1
		_Amplitude("Amplitude", float) = 1
		_Frequency("Frequency", float) = 1
		_Speed("Speed", float) = 1
		_Unit("Length", float) = 1
	}
	SubShader 
	{
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull front
		LOD 100

		Pass 
		{
			CGPROGRAM
			#pragma vertex myVertexShader alpha
			#pragma fragment myFragmentShader alpha
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

			float _Local;
			float _Xaxis;
			float _Yaxis;
			float _Zaxis;
			float _Amplitude;
			float _Speed;
			float _Frequency;
			float _Unit;

			vertexToFragment myVertexShader (vertexInput v) 
			{
				vertexToFragment o;
				o.uv = v.uv;
				// Transform the point to clip space:
				o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
				if (_Local)
				{
					o.worldPos = v.vertex;
				}
				else
				{
					o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
				}
				float y = 0;
				float PI = 3.141592653589;

				float frequency = _Frequency * 2 * PI * (1 / _Unit);

				if (_Xaxis)
				{
					y += _Amplitude * sin(_Speed * _Time.y + v.vertex.z * frequency);
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
			float4 _Color;

			fixed4 myFragmentShader(vertexToFragment i) : SV_Target
			{
				return _Color * tex2D(_MainTex,i.uv);
			}
			ENDCG
		}
	}
}

