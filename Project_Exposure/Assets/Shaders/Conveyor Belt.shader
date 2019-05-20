Shader "Custom/Conveyor Belt"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)

		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("NormalMap", 2D) = "bump" {}
		_MetallicMap("MetallicMap (RGB), Smoothness (A)", 2D) = "white" {}
		_HeightMap("HeightMap", 2D) = "black" {}
		_OcclusionMap("OcclusionMap", 2D) = "white" {}

		_MaxHeight("MaxHeight", Float) = 1
		_OcclusionStrength("Occlusion Strength", float) = 1

		_TimeScaleX("TimeScaleX", float) = 0
		_TimeScaleY("TimeScaleY", float) = 1
	}
		SubShader
		{
			Cull Off

			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows vertex:vert

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _BumpMap;
			sampler2D _MetallicMap;
			sampler2D _HeightMap;
			sampler2D _OcclusionMap;

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_BumpMap;
				float2 uv_MetallicMap;
				float2 uv_OcclusionMap;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			float _OcclusionStrength;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
			
			UNITY_INSTANCING_BUFFER_END(Props)

			float _TimeScaleX;
			float _TimeScaleY;

			float _MaxHeight;

			void vert(inout appdata_full v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input, o);
				float4 heightMap = tex2Dlod(_HeightMap, float4(v.texcoord.xy + float2(_Time.x * _TimeScaleX, _Time.x * _TimeScaleY), 0, 0));

				float value = saturate(heightMap.rgb);
				value -= 0.5;

				v.vertex.y += _MaxHeight * 2 * value, 1;
			}

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex + float2(_Time.x * _TimeScaleX, _Time.x * _TimeScaleY)) * _Color;
				o.Albedo = c.rgb;

				// Normal comes from a NormalMap/BumpMap
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap + float2(_Time.x * _TimeScaleX, _Time.x * _TimeScaleY)));
				
				// Metallic and smoothness come from the MetallicMap
				float4 metallicData = tex2D(_MetallicMap, IN.uv_MetallicMap + float2(_Time.x * _TimeScaleX, _Time.x * _TimeScaleY));
				o.Metallic = metallicData.rgb;
				o.Smoothness = metallicData.a;

				//Ambient Occulusion comes from the occulusionMap
				o.Occlusion = tex2D(_OcclusionMap, IN.uv_OcclusionMap + float2(_Time.x * _TimeScaleX, _Time.x * _TimeScaleY)) * _OcclusionStrength;

				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
