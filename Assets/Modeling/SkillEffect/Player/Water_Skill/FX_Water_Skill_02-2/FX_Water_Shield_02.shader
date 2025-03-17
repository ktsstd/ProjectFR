// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Water_Shield_02"
{
	Properties
	{
		_HighLight_Tex("HighLight_Tex", 2D) = "white" {}
		_Highlight_UPanner("Highlight_UPanner", Float) = 0
		_Highlight_VPanner("Highlight_VPanner", Float) = 0
		_Fresnel_Scale("Fresnel_Scale", Range( 0 , 1)) = 1
		_Fresnel_Power("Fresnel_Power", Range( 0 , 10)) = 5.325229
		[HDR]_Fresnel_InColor("Fresnel_InColor", Color) = (0,1,1,0)
		[HDR]_Fresnel_OutColor("Fresnel_OutColor", Color) = (1,1,1,0)
		_Fade_Distance("Fade_Distance", Range( 1 , 10)) = 1
		_Opacity("Opacity", Float) = 1
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_Noise_UPanner("Noise_UPanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Normal_UPanner("Normal_UPanner", Float) = 0
		_Normal_VPanner("Normal_VPanner", Float) = 0
		_Distortion("Distortion", Range( 0 , 1)) = 0
		_Vertex_Tex("Vertex_Tex", 2D) = "white" {}
		_Vertex_UPanner("Vertex_UPanner", Float) = 0
		_Vertex_VPanner("Vertex_VPanner", Float) = 0
		_Vertex_Normal_Str("Vertex_Normal_Str", Float) = 1
		[HDR]_Edge_color("Edge_color", Color) = (1,1,1,0)
		_Edge_Range("Edge_Range", Range( 0.1 , 0.8)) = 0.149925
		_Dissovle("Dissovle", Range( 0 , 0.8)) = 0.2643328
		[Toggle(_USE_CUSTOM_ON)] _USE_CUSTOM("USE_CUSTOM", Float) = 0
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature _USE_CUSTOM_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			float4 screenPos;
			float4 uv_tex4coord;
		};

		uniform sampler2D _Vertex_Tex;
		uniform float _Vertex_UPanner;
		uniform float _Vertex_VPanner;
		uniform float4 _Vertex_Tex_ST;
		uniform float _Vertex_Normal_Str;
		uniform float4 _Fresnel_InColor;
		uniform float4 _Fresnel_OutColor;
		uniform sampler2D _HighLight_Tex;
		uniform float _Highlight_UPanner;
		uniform float _Highlight_VPanner;
		uniform float4 _HighLight_Tex_ST;
		uniform sampler2D _Normal_Tex;
		uniform float _Normal_UPanner;
		uniform float _Normal_VPanner;
		uniform float4 _Normal_Tex_ST;
		uniform float _Distortion;
		uniform float _Fresnel_Scale;
		uniform float _Fresnel_Power;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Fade_Distance;
		uniform float4 _Edge_color;
		uniform sampler2D _Noise_Tex;
		uniform float _Noise_UPanner;
		uniform float _Noise_VPanner;
		uniform float _Dissovle;
		uniform float _Edge_Range;
		uniform float _Opacity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float2 appendResult72 = (float2(_Vertex_UPanner , _Vertex_VPanner));
			float2 uv0_Vertex_Tex = v.texcoord.xy * _Vertex_Tex_ST.xy + _Vertex_Tex_ST.zw;
			float2 panner73 = ( 1.0 * _Time.y * appendResult72 + uv0_Vertex_Tex);
			v.vertex.xyz += ( ase_vertexNormal * ( tex2Dlod( _Vertex_Tex, float4( panner73, 0, 0.0) ).r * _Vertex_Normal_Str ) );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult25 = (float2(_Highlight_UPanner , _Highlight_VPanner));
			float2 uv0_HighLight_Tex = i.uv_texcoord * _HighLight_Tex_ST.xy + _HighLight_Tex_ST.zw;
			float2 panner21 = ( 1.0 * _Time.y * appendResult25 + uv0_HighLight_Tex);
			float2 appendResult31 = (float2(_Normal_UPanner , _Normal_VPanner));
			float2 uv0_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 panner33 = ( 1.0 * _Time.y * appendResult31 + uv0_Normal_Tex);
			float2 temp_output_34_0 = ( (UnpackNormal( tex2D( _Normal_Tex, panner33 ) )).xy * _Distortion );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV3 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode3 = ( 0.0 + _Fresnel_Scale * pow( 1.0 - fresnelNdotV3, _Fresnel_Power ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth11 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth11 = abs( ( screenDepth11 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Fade_Distance ) );
			float4 lerpResult7 = lerp( _Fresnel_InColor , _Fresnel_OutColor , max( ( tex2D( _HighLight_Tex, ( panner21 + temp_output_34_0 ) ).r * saturate( fresnelNode3 ) ) , saturate( ( 1.0 - distanceDepth11 ) ) ));
			float2 appendResult41 = (float2(_Noise_UPanner , _Noise_VPanner));
			float2 panner43 = ( 1.0 * _Time.y * appendResult41 + uv0_Normal_Tex);
			#ifdef _USE_CUSTOM_ON
				float staticSwitch52 = i.uv_tex4coord.z;
			#else
				float staticSwitch52 = _Dissovle;
			#endif
			float temp_output_55_0 = (0.0 + (( tex2D( _Noise_Tex, ( temp_output_34_0 + panner43 ) ).r + ( 1.0 - i.uv_texcoord.y ) + (-2.0 + (staticSwitch52 - 0.0) * (2.0 - -2.0) / (1.0 - 0.0)) ) - 0.0) * (1.0 - 0.0) / (1.0 - 0.0));
			float temp_output_59_0 = step( 0.1 , temp_output_55_0 );
			o.Emission = ( lerpResult7 + ( _Edge_color * ( temp_output_59_0 - step( _Edge_Range , temp_output_55_0 ) ) ) ).rgb;
			o.Alpha = ( _Opacity * temp_output_59_0 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
0;794;905;566;1777.513;-1077.483;1.325491;True;False
Node;AmplifyShaderEditor.CommentaryNode;38;-2177.795,391.8101;Float;False;1339.033;420.8978;Comment;9;30;29;32;31;33;27;35;28;34;;0.04924154,1,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-2127.795,569.8107;Float;False;Property;_Normal_UPanner;Normal_UPanner;13;0;Create;True;0;0;False;0;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2132.784,675.3449;Float;False;Property;_Normal_VPanner;Normal_VPanner;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;31;-1900.484,651.4663;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-2024.072,441.8101;Float;False;0;27;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;33;-1792.346,520.1552;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-1337.685,975.3193;Float;False;Property;_Noise_UPanner;Noise_UPanner;10;0;Create;True;0;0;False;0;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1342.674,1080.854;Float;False;Property;_Noise_VPanner;Noise_VPanner;11;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;27;-1565.958,516.3773;Float;True;Property;_Normal_Tex;Normal_Tex;12;0;Create;True;0;0;False;0;51fe2c9d5b236124d9f9e7ea528b0bea;51fe2c9d5b236124d9f9e7ea528b0bea;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;-1233.962,847.3187;Float;False;0;27;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;37;-706,-523.6427;Float;False;1337.261;1119.643;Comment;20;4;12;5;11;3;16;6;13;15;8;9;19;7;26;25;22;20;21;23;18;;1,0,0,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;41;-1110.374,1056.975;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;28;-1264.563,539.6039;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1300.95,641.9404;Float;False;Property;_Distortion;Distortion;15;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1253.163,1256.593;Float;False;Property;_Dissovle;Dissovle;22;0;Create;True;0;0;False;0;0.2643328;0;0;0.8;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1000.762,532.7814;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;43;-1002.236,925.6638;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;54;-1191.391,1405.421;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;-682.1874,-163.1065;Float;False;Property;_Highlight_VPanner;Highlight_VPanner;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-693.222,-290.0037;Float;False;Property;_Highlight_UPanner;Highlight_UPanner;1;0;Create;True;0;0;False;0;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;46;-715.4719,1129.606;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;25;-465.9104,-208.3481;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;52;-941.1626,1282.593;Float;False;Property;_USE_CUSTOM;USE_CUSTOM;23;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-796.459,844.5577;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-589.4978,-418.0046;Float;False;0;19;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;51;-795.0302,1475.231;Float;False;Constant;_Float3;Float 3;16;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-796.8164,1389.169;Float;False;Constant;_Float2;Float 2;16;0;Create;True;0;0;False;0;-2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;39;-625.8175,822.4091;Float;True;Property;_Noise_Tex;Noise_Tex;9;0;Create;True;0;0;False;0;9d5a62bfcd403a84a89e299be1360dc3;9d5a62bfcd403a84a89e299be1360dc3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;49;-553.0892,1316.269;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-640,480;Float;False;Property;_Fade_Distance;Fade_Distance;7;0;Create;True;0;0;False;0;1;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-624,336;Float;False;Property;_Fresnel_Power;Fresnel_Power;4;0;Create;True;0;0;False;0;5.325229;1.32;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-656,240;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;3;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;47;-493.2457,1141.091;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;21;-357.7721,-339.6593;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FresnelNode;3;-336,208;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-85.52521,1402.027;Float;False;Constant;_Float5;Float 5;18;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-272.3679,1158.287;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;11;-352,432;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-848.2559,1902.569;Float;False;Property;_Vertex_VPanner;Vertex_VPanner;18;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-80.38678,1244.004;Float;False;Constant;_Float4;Float 4;18;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-843.2669,1797.034;Float;False;Property;_Vertex_UPanner;Vertex_UPanner;17;0;Create;True;0;0;False;0;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-539.9235,8.958817;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;71;-739.544,1669.034;Float;False;0;68;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;72;-615.956,1878.69;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;6;-48,208;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;98.11932,1359.188;Float;False;Property;_Edge_Range;Edge_Range;21;0;Create;True;0;0;False;0;0.149925;0;0.1;0.8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;16;-112,448;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;55;82.7598,1159.549;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-290.9758,-99.67233;Float;True;Property;_HighLight_Tex;HighLight_Tex;0;0;Create;True;0;0;False;0;8770ffa4a97cb4e458fe0316a8cc0731;8770ffa4a97cb4e458fe0316a8cc0731;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;60;172.4748,986.0266;Float;False;Constant;_Float6;Float 6;18;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;36.03017,-89.04855;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;73;-507.8178,1747.379;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;59;385.4748,1009.027;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;61;380.7571,1331.449;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;13;48,416;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;45.03864,1887.958;Float;False;Property;_Vertex_Normal_Str;Vertex_Normal_Str;19;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;15;176,96;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;63;601.2533,1114.312;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;52.4819,-473.6427;Float;False;Property;_Fresnel_InColor;Fresnel_InColor;5;1;[HDR];Create;True;0;0;False;0;0,1,1,0;0,0.8900523,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;66;671.317,787.2235;Float;False;Property;_Edge_color;Edge_color;20;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;68;-226.6193,1712.867;Float;True;Property;_Vertex_Tex;Vertex_Tex;16;0;Create;True;0;0;False;0;6e5343f0266cf36489aa21b41e5bc1f7;6e5343f0266cf36489aa21b41e5bc1f7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;54.4819,-275.6427;Float;False;Property;_Fresnel_OutColor;Fresnel_OutColor;6;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1.844303,1.844303,1.844303,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;980.3912,661.0383;Float;False;Property;_Opacity;Opacity;8;0;Create;True;0;0;False;0;1;0.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;201.0386,1756.958;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;7;366.2613,-193.3255;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;74;9.891604,1560.294;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;890.1143,917.2877;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;1121.222,712.1208;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;990.9186,443.5216;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;391.0386,1649.958;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;1289.298,457.9139;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Water_Shield_02;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;31;0;29;0
WireConnection;31;1;30;0
WireConnection;33;0;32;0
WireConnection;33;2;31;0
WireConnection;27;1;33;0
WireConnection;41;0;40;0
WireConnection;41;1;44;0
WireConnection;28;0;27;0
WireConnection;34;0;28;0
WireConnection;34;1;35;0
WireConnection;43;0;42;0
WireConnection;43;2;41;0
WireConnection;25;0;22;0
WireConnection;25;1;23;0
WireConnection;52;1;53;0
WireConnection;52;0;54;3
WireConnection;45;0;34;0
WireConnection;45;1;43;0
WireConnection;39;1;45;0
WireConnection;49;0;52;0
WireConnection;49;3;50;0
WireConnection;49;4;51;0
WireConnection;47;0;46;2
WireConnection;21;0;20;0
WireConnection;21;2;25;0
WireConnection;3;2;4;0
WireConnection;3;3;5;0
WireConnection;48;0;39;1
WireConnection;48;1;47;0
WireConnection;48;2;49;0
WireConnection;11;0;12;0
WireConnection;26;0;21;0
WireConnection;26;1;34;0
WireConnection;72;0;70;0
WireConnection;72;1;69;0
WireConnection;6;0;3;0
WireConnection;16;0;11;0
WireConnection;55;0;48;0
WireConnection;55;3;56;0
WireConnection;55;4;57;0
WireConnection;19;1;26;0
WireConnection;18;0;19;1
WireConnection;18;1;6;0
WireConnection;73;0;71;0
WireConnection;73;2;72;0
WireConnection;59;0;60;0
WireConnection;59;1;55;0
WireConnection;61;0;62;0
WireConnection;61;1;55;0
WireConnection;13;0;16;0
WireConnection;15;0;18;0
WireConnection;15;1;13;0
WireConnection;63;0;59;0
WireConnection;63;1;61;0
WireConnection;68;1;73;0
WireConnection;75;0;68;1
WireConnection;75;1;76;0
WireConnection;7;0;8;0
WireConnection;7;1;9;0
WireConnection;7;2;15;0
WireConnection;65;0;66;0
WireConnection;65;1;63;0
WireConnection;67;0;10;0
WireConnection;67;1;59;0
WireConnection;36;0;7;0
WireConnection;36;1;65;0
WireConnection;77;0;74;0
WireConnection;77;1;75;0
WireConnection;2;2;36;0
WireConnection;2;9;67;0
WireConnection;2;11;77;0
ASEEND*/
//CHKSM=74B8C4AC509C235FEE5BC016A7396A5D22350E54