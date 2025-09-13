Shader "Lily/Background" {
	Properties {
		[HideInInspector] _AlphaCutoff ("Alpha Cutoff ", Range(0, 1)) = 0.5
		[Toggle] _ReceiveShadowmask ("Receive Shadowmask", Float) = 1
		_Color ("Color", Vector) = (0,0,0,0)
		_MainTex ("MainTex", 2D) = "white" {}
		[Toggle(_BLEND_TEXTURE_ENABLED)] _BlendTextureEnabled ("ブレンドテクスチャ有効", Float) = 0
		_BlendTex ("テクスチャ（頂点アルファでブレンド）", 2D) = "white" {}
		[Toggle(_ALPHATEST_ON)] _AlphaClip ("カットオフ有効（抜き）", Float) = 0
		_Cutoff ("カットオフアルファ値", Range(0, 1)) = 0.1
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("裏表カリング", Float) = 2
		[MaterialToggle] _ZWrite ("デプス書き込み", Float) = 1
		[Toggle(_EMISSION_ENABLED)] _EmissionEnable ("自己照明オン", Float) = 0
		[HDR] _EmissionColor ("自己照明カラー", Vector) = (1,1,1,1)
		_EmissionMap ("自己照明テクスチャ", 2D) = "white" {}
		[Toggle(_REFLECTION_PROBE)] _ReflectionProbe ("リフレクションプローブ", Float) = 0
		_Smoothness ("Smoothness（リフレクションプローブ用）", Range(0, 1)) = 0
		_Specular ("スペキュラ（リフレクションプローブ用）", Vector) = (0,0,0,0)
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Factor", Float) = 1
		[HideInInspector] _BlendMode ("BlendMode", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Factor", Float) = 0
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("BlendOp", Float) = 0
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
		[HideInInspector] _QueueControl ("_QueueControl", Float) = -1
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
		[ToggleOff] [HideInInspector] _ReceiveShadows ("Receive Shadows", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "LilyShaderCommonGUI"
}