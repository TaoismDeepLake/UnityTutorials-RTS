Shader "Lily/Character2DMatcapscroll_Amplify" {
	Properties {
		[HideInInspector] _EmissionColor ("Emission Color", Vector) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff ("Alpha Cutoff ", Range(0, 1)) = 0.5
		[Toggle] _EnableOutline ("Enable Outline", Float) = 1
		_OutlineColor ("Outline Color", Vector) = (0.2075472,0.1275391,0.05384479,0)
		_OutlineWidth ("Outline Width", Range(0, 0.5)) = 0.03
		_FOV ("FOV", Range(0, 1)) = 0.05
		_Distance ("Distance", Range(0, 1)) = 0.5301833
		_MaxWidth ("MaxWidth", Range(0, 100)) = 1
		_Float0 ("Float 0", Range(0, 50)) = 5
		_MainTex ("テクスチャ", 2D) = "white" {}
		_Color ("ベースカラー", Vector) = (1,1,1,1)
		_Cutoff ("カットオフアルファ値", Range(0, 1)) = 0.01
		_Rimcolor ("Rimcolor", Vector) = (0.5660378,0.5660378,0.5660378,0)
		[MaterialToggle] _ZWrite ("デプス書き込み", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("裏表カリング", Float) = 0
		_matcaptex ("matcaptex", 2D) = "black" {}
		[Toggle(_EMISSIONENABLE_ON)] _EmissionEnable ("自己照明オン", Float) = 0
		_EmissionStorengs ("自己照明強度", Range(0, 1)) = 1
		_RimTrans ("RimTrans", Vector) = (0,0,0,0)
		_MaskTexture ("MaskTexture", 2D) = "white" {}
		_RimScale ("RimScale", Range(0, 2)) = 1
		[HDR] _CrackColor ("CrackColor", Vector) = (0.2509804,0.972549,7.937255,0)
		_CrackPoint1 ("CrackPoint1", Range(0, 1)) = 0
		_CrackAll ("CrackAll", Range(0, 1)) = 0
		_ColorScale ("カラースケール（システム用）", Vector) = (1,1,1,1)
		[HideInInspector] _BaseColorScale ("ベースカラースケール（システム用）", Vector) = (1,1,1,1)
		_effectmask ("effectmask", 2D) = "white" {}
		_ColorOffset ("カラーオフセット（システム用）", Vector) = (0,0,0,0)
		_CrackTile ("CrackTile", Range(1, 10)) = 1
		[HideInInspector] _BlendMode ("_BlendMode", Float) = 0
		_BlackTile ("BlackTile", Range(1, 10)) = 1
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("Blend Op", Float) = 0
		[Toggle(_EMISSIONENABLE_ON)] _EmissionEnable ("AttackEmission", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Factor", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Factor", Float) = 10
		[HDR] _AttackColor ("AttackColor", Vector) = (0,0,0,0)
		_Attack ("Attack", Range(0, 1)) = 0
		_DissolveTile ("DissolveTile", Range(0, 50)) = 5
		[Toggle(_METAL_ON)] _Metal ("Metal", Float) = 0
		_metalmatcap ("metalmatcap", 2D) = "black" {}
		_metaric ("metaric", Range(0, 1)) = 1
		[Toggle(_METALNORMAL_ON)] _MetalNormal ("MetalNormal", Float) = 0
		_NormalTexture ("NormalTexture", 2D) = "bump" {}
		[Toggle(_PRIZM_ON)] _Prizm ("Prizm", Float) = 0
		_PrizmYscale ("PrizmYscale", Float) = 0
		_PrizmUVSpeed ("PrizmUVSpeed", Vector) = (0,0,0,0)
		_Prizmmatcap ("Prizmmatcap", 2D) = "black" {}
		_PrizmPower ("PrizmPower", Range(0, 10)) = 5
		_PrizmScale ("PrizmScale", Range(0, 10)) = 5
		_RimColorScale ("RimColorScale", Vector) = (1,1,1,0)
		_Prizmscroll ("Prizmscroll", 2D) = "white" {}
		[Toggle(_COLORILLUMINATION_ON)] _ColorIllumination ("ColorIllumination", Float) = 0
		_Illumination ("Illumination", Range(0, 1)) = 0
		_IlluminationTex ("IlluminationTex", 2D) = "white" {}
		_IlluminationSpeed ("IlluminationSpeed", Range(0, 10)) = 0
		_IlluminationTile ("IlluminationTile", Range(1, 10)) = 1
		[Toggle(_FORCECUTOFFALWAYS_ON)] _ForceCutoffAlways ("ForceCutoffAlways", Float) = 0
		[HideInInspector] _texcoord2 ("", 2D) = "white" {}
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