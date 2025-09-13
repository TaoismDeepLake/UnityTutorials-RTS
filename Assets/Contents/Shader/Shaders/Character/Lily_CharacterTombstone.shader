Shader "Lily/CharacterTombstone" {
	Properties {
		[HideInInspector] _EmissionColor ("Emission Color", Vector) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff ("Alpha Cutoff ", Range(0, 1)) = 0.5
		_MainTex ("MainTex", 2D) = "white" {}
		_Color ("ベースカラー", Vector) = (1,1,1,1)
		_AddBrightness ("AddBrightness", Range(0, 1)) = 0.4
		_NormalTexture ("NormalTexture", 2D) = "bump" {}
		_Fogmask ("Fogmask", 2D) = "white" {}
		_matcaptex ("matcaptex", 2D) = "white" {}
		[HDR] _EmissiveColor ("EmissiveColor", Vector) = (1,1,1,0)
		_EmissiveMaskTile ("EmissiveMaskTile", Vector) = (3,4,0,0)
		_EmissiveTile ("EmissiveTile", Range(1, 20)) = 2
		_EmmisiveScroll ("EmmisiveScroll", Vector) = (0,0,0,0)
		_WhiteOffset ("WhiteOffset", Range(-0.6, 0.5)) = 0
		[MaterialToggle] _ZWrite ("デプス書き込み", Float) = 0
		[Toggle(_FAKEFOG_ON)] _FakeFog ("FakeFog", Float) = 0
		_FogSpeed ("FogSpeed", Vector) = (0,0,0,0)
		_FogTile ("FogTile", Vector) = (1,1,0,0)
		_FogStartPosition ("FogStartPosition", Range(0, 300)) = 2
		_FogPower ("FogPower", Range(0, 1)) = 0.1
		_FogColor ("FogColor", Vector) = (0.5896226,0.9106476,1,0)
		_FogTransparency ("FogTransparency", Range(0, 1)) = 1
		_bottom ("bottom", Range(0, 10)) = 2
		_hight ("hight", Range(0, 500)) = 2
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("裏表カリング", Float) = 0
		_ColorScale ("カラースケール（システム用）", Vector) = (1,1,1,1)
		_ColorOffset ("カラーオフセット（システム用）", Vector) = (0,0,0,0)
		[HideInInspector] _BlendMode ("_BlendMode", Float) = 0
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("Blend Op", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Factor", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Factor", Float) = 10
		_ChangeColor ("ChangeColor", Vector) = (0,0,0,0)
		[HDR] _FresnelColor ("FresnelColor", Vector) = (0,0,0,0)
		_FresnelPower ("FresnelPower", Range(0, 10)) = 5
		_FresnelScale ("FresnelScale", Range(0, 2)) = 1
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