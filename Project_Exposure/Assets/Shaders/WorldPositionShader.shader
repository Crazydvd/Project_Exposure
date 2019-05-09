Shader "Handout/WorldPosition"
{
	Properties
	{
		_Scale("Scale", float) = 5
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#define UNITY_SHADER_NO_UPGRADE 1

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				//float4 worldPosition : TEXCOORD0; // it's not a texture coordinate, but we must pick something for the semantic...
				float4 cameraSpacePosition : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				// Transform the point to clip space:
				o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
				// Also pass the world position to the fragment shader:
				//o.worldPosition = mul(UNITY_MATRIX_M,v.vertex);

				o.cameraSpacePosition = mul(UNITY_MATRIX_MV,v.vertex);
				return o;
			}

			float _Scale;
			
			fixed4 frag (v2f i) : SV_Target
			{
				//// rounds scaled coordinates down to integer:
				//float4 roundedPosition = floor(i.worldPosition * _Scale);
				//// modulo 2, so returns -1, 0 or 1 in a 3D checkerboard pattern:
				//int cell = fmod(roundedPosition.x + roundedPosition.y + roundedPosition.z,2);	
				//// Fix negative numbers! Go from -1,0,1 to 0,1:
				//cell = fmod(cell+2,2);	
				//// return color red or green depending on world position:
				//return float4(cell,1-cell,0,1);

				//Fog:
				float distance = i.cameraSpacePosition.z;
				float normalized = saturate(-distance / 20);
				return float4(0, 1, 1, 1) * normalized + float4(1, 0, 0, 1) * (1 - normalized);
			}
			ENDCG
		}
	}
}
