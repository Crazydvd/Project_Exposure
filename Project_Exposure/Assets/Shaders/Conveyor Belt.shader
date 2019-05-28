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

		_MaxHeight("MaxHeight", float) = 0.01
		_OcclusionStrength("Occlusion Strength", float) = 1

		[Header(The amount of seconds it takes to complete 1 cycle)]
		_SpeedX("SpeedX", float) = 0
		_SpeedY("SpeedY", float) = 1

		[Header(How much ingame time has elapsed)]
		_TimeElapsed("TimeOffset", float) = 0

		[Header(the length of the object)]
		_Length("Length", float) = 2
	}
		SubShader
		{
			Cull Off

			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows

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
				float2 uv_HeightMap;
				float2 uv_OcclusionMap;
				float3 viewDir;
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

			float _SpeedX;
			float _SpeedY;
			float _TimeElapsed;
			float _Length;

			float _MaxHeight;

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// _deltaTime / _Time == Seconds per Cycle
				// _deltaTime * Speed == Cycles per second
				// (Time = 1 / Speed) && (Speed = 1 / Time)
				float timeX = (_SpeedX == 0) ? 0 : _TimeElapsed * (_SpeedX / _Length);
				float timeY = (_SpeedY == 0) ? 0 : _TimeElapsed * (_SpeedY / _Length);
				float2 timeOffset = float2(timeX, timeY);

				//Height map gives an offset to the uvs
				float value = tex2D(_HeightMap, IN.uv_HeightMap + timeOffset).rgb;
				float2 heightOffset = ParallaxOffset(value, _MaxHeight, IN.viewDir);


				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex + timeOffset) * _Color;
				o.Albedo = c.rgb;

				// Normal comes from a NormalMap/BumpMap
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap + timeOffset + heightOffset));
				
				// Metallic and smoothness come from the MetallicMap
				float4 metallicData = tex2D(_MetallicMap, IN.uv_MetallicMap + timeOffset + heightOffset);
				o.Metallic = metallicData.rgb;
				o.Smoothness = metallicData.a;

				//Ambient Occulusion comes from the occulusionMap
				o.Occlusion = tex2D(_OcclusionMap, IN.uv_OcclusionMap + timeOffset + heightOffset) * _OcclusionStrength;

				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
