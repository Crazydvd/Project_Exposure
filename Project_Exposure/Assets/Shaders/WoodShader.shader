Shader "Handout/WoodShader" {
	Properties
	{
		_Color1("Color1", Color) = (1,0.5,0,1)
		_Color2("Color2", Color) = (0.5,0.25,0,1)
		Size("RingSize", float) = 4.38
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex myVertexShader
			#pragma fragment myFragmentShader
			#define UNITY_SHADER_NO_UPGRADE 1

			struct vertexInput {
				float4 vertex : POSITION;
			};

			struct vertexToFragment {
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORDS0;
			};

			float PingPong(float value, float length)
			{
				if (value > length) //check if we are over our limit
				{
					float a = value % length; //grab the decimal number
					uint b = value / length; //check how many times we are over our limit
					uint c = b % 2; //see if it's an even number
					if (c == 0) //even
					{
						return a;
					}
					else //not even
					{
						return length - a;
					}
				}
				else
				{
					return value;
				}
			}

			float4 _Color1;
			float4 _Color2;
			float Size;

			vertexToFragment myVertexShader (vertexInput v) {
				vertexToFragment o;
				// Transform the point to clip space:
				o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);

				o.worldPos = v.vertex; /// mul(UNITY_MATRIX_M, v.vertex);
				return o;
			}
			
			fixed4 myFragmentShader (vertexToFragment i) : SV_Target {
				// return color red (1,0,0), with alpha=1:
				//return float4(1,0,0,1);

				//Wood:
				float distanceFromCenter = length(i.worldPos.xz) * Size;
				float value = PingPong(distanceFromCenter, 1);
				return lerp(_Color1, _Color2, value);
			}
			ENDCG
		}
		// TODO: change name + variable names!
		// It's good that you don't use UVs. But the texture shouldn't change when the camera changes (orth)
		// make sure you have rings around an *axis* (if z coordinate changes, the wood color doesn't change. But it does change for x and y)
	}
}

