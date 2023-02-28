Shader "Mobile/BlurURP"
{
	Properties
	{
		[HideInInspector] _MainTex("Texture", 2D) = "" {}
		[HideInInspector] _MaskTex("Mask", 2D) = "white" {}
	}
	HLSLINCLUDE
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	TEXTURE2D_X(_MainTex);
	SAMPLER(sampler_MainTex);
	TEXTURE2D_X(_BlurTex);
	SAMPLER(sampler_BlurTex);
	TEXTURE2D_X(_MaskTex);
	SAMPLER(sampler_MaskTex);
	half _Intensity;
	half4 _MainTex_TexelSize;
	struct texdata
	{
		half4 vertex : POSITION;
		half2 uv : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};
	struct v2f {
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		UNITY_VERTEX_OUTPUT_STEREO
	};
	struct v2fb
	{
		half4 pos : SV_POSITION;
		half4 uv : TEXCOORD0;
#if defined(KERNELSIZE)
		half4 uv1 : TEXCOORD1;
#endif
		UNITY_VERTEX_OUTPUT_STEREO
	};
	v2f vert(texdata v)
	{
		v2f o = (v2f)0;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); 
		o.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, half4(v.vertex.xyz, 1.0h)));
		o.uv = v.uv;
		return o;
	}
	v2fb vertb(texdata v)
	{
		v2fb o = (v2fb)0;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, half4(v.vertex.xyz, 1.0h)));
		half2 offset = _MainTex_TexelSize.xy * _Intensity;
		o.uv = half4(v.uv - offset, v.uv + offset);
#if defined(KERNELSIZE)
		offset *= 2.0h;
		o.uv1 = half4(v.uv - offset, v.uv + offset);
#endif
		return o;
	}
	half4 fragb(v2fb i) : COLOR
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		half4 result = SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv.xy));
		result += SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv.xw));
		result += SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv.zy));
		result += SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv.zw));
#if defined(KERNELSIZE)
		result += SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv1.xy));
		result += SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv1.xw));
		result += SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv1.zy));
		result += SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv1.zw));
		return result * 0.125h;
#endif
		return result * 0.25h;
	}
		half4 frag(v2f i) : COLOR
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		half4 c = SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(i.uv));
		half4 m = SAMPLE_TEXTURE2D_X(_MaskTex, sampler_MaskTex, i.uv);
		half4 b = SAMPLE_TEXTURE2D_X(_BlurTex, sampler_BlurTex, UnityStereoTransformScreenSpaceTex(i.uv));
		return lerp(c, b, m.r);
	}
	ENDHLSL
	Subshader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			HLSLPROGRAM
			#pragma multi_compile __ KERNELSIZE
			#pragma vertex vertb
			#pragma fragment fragb
			#pragma fragmentoption ARB_precision_hint_fastest
			ENDHLSL
		}
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			ENDHLSL
		}
	}
}