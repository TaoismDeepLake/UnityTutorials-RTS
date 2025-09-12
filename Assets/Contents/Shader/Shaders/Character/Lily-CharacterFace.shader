Shader "Lily/Character Face" {
	Properties {
		[HideInInspector] _EmissionColor ("Emission Color", Vector) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff ("Alpha Cutoff ", Range(0, 1)) = 0.5
		_MainTex ("MainTex", 2D) = "white" {}
		[HDR] _Color ("ベースカラー", Vector) = (1,1,1,1)
		[Toggle(_ALPHATEST_ON)] _AlphaClip ("カットオフ有効（抜き）", Float) = 0
		_Cutoff ("カットオフアルファ値", Range(0, 1)) = 0.004
		_Zoffset ("押し出し距離", Range(-3, 3)) = 0
		_RimRamp ("RimRamp", 2D) = "black" {}
		[HDR] _RimColor ("RimColor", Vector) = (0.5,0.5,0.5,1)
		_RimTrans ("RimTrans", Vector) = (0,0,0,0)
		_RimScale ("RimScale", Range(0, 2)) = 1
		[MaterialToggle] _ZWrite ("デプス書き込み", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("裏表カリング", Float) = 0
		[HDR] _ColorScale ("カラースケール（システム用）", Vector) = (1,1,1,1)
		[HDR] _ColorOffset ("カラーオフセット（システム用）", Vector) = (0,0,0,0)
		[HideInInspector] _BaseColorScale ("BaseColorScale", Vector) = (1,1,1,1)
		[HideInInspector] _RimColorScale ("RimColorScale", Vector) = (1,1,1,1)
		[HideInInspector] _AnimationParams1 ("AnimationParams1", Vector) = (1,1,1,0)
		[HideInInspector] _AnimationParams2 ("AnimationParams2", Vector) = (1,1,1,1)
		[Toggle(_HSVOFFSET_ON)] _HSVEnable ("HSVオン", Float) = 0
		_HSVHueTune ("Hue offset", Range(0, 1)) = 0
		_HSVSatTune ("Saturation multiplicator", Float) = 0.1
		_HSVValTune ("Value multiplicator", Float) = 0.35
		[Toggle(_DITHER_ENABLE)] _DitherEnable ("ディザ有効化", Float) = 0
		_Dither ("ディザ透明度", Range(0, 1)) = 0.5
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Factor", Float) = 0
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