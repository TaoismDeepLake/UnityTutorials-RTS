Shader "Lily/CharacterReflection" {
	Properties {
		[HideInInspector] _AlphaCutoff ("Alpha Cutoff ", Range(0, 1)) = 0.5
		_ReflectionTex ("ReflectionTex", 2D) = "white" {}
		[Toggle(_HSVOFFSET_ON)] _HSVEnable ("HSVオン", Float) = 0
		_HSVHueTune ("Hue offset", Range(0, 1)) = 0
		_HSVSatTune ("Saturation multiplicator", Float) = 0.1
		_HSVValTune ("Value multiplicator", Float) = 0.35
		[HDR] _Color ("ベースカラー", Vector) = (1,1,1,1)
		[HDR] _ColorAdd ("カラー加算値", Vector) = (1,1,1,1)
		[Toggle(_OVERLAYTEX_ON)] _OverlayTexOn ("オーバーレイテクスチャ有効", Float) = 0
		_OverlayTex ("OverlayTex", 2D) = "white" {}
		[Toggle(_EMISSIONENABLE_ON)] _EmissionEnable ("自己照明オン", Float) = 0
		[HDR] _EmissionColor ("自己照明カラー", Vector) = (0,0,0,0)
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("裏表カリング", Float) = 0
		[Toggle] _EnableOutline ("Enable Outline", Float) = 1
		_OutlineWidth ("OutlineWidth", Range(0, 0.5)) = 0.06
		_OutlineColor ("OutlineColor", Vector) = (0.2075472,0.1275391,0.05384479,1)
		[HDR] _OutlineAddColor ("OutlineAddColor", Vector) = (0,0,0,0)
		[MaterialToggle] _ZWrite ("デプス書き込み", Float) = 0
		_MaxWidth ("MaxWidth", Range(0, 100)) = 0.4
		_FOV ("FOV", Range(0, 1)) = 0.05
		_Distance ("Distance", Range(0, 1)) = 0.04
		[HDR] _ColorScale ("カラースケール（システム用）", Vector) = (1,1,1,1)
		[HDR] _ColorOffset ("カラーオフセット（システム用）", Vector) = (0,0,0,0)
		[HideInInspector] _BaseColorScale ("BaseColorScale", Vector) = (1,1,1,1)
		[HideInInspector] _AnimationParams1 ("AnimationParams1", Vector) = (1,1,1,0)
		[HideInInspector] _BlendMode ("BlendMode", Float) = 0
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("BlendOp", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Factor", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Factor", Float) = 1
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

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "LilyShaderCommonGUI"
}