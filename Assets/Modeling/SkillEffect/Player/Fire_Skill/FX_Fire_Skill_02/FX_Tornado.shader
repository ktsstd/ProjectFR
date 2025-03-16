// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Fire_Tornado"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Main_Rot("Main_Rot", Float) = -1.01
		_Main_UPanner("Main_UPanner", Float) = 0
		_Main_VPanner("Main_VPanner", Float) = -1
		_Smooth("Smooth", Float) = 0
		_Base_Range("Base_Range", Range( -1.5 , 1.5)) = 0.6779342
		_High_Range("High_Range", Range( -1.5 , 1.5)) = 0.9272076
		[HDR]_Base_Color("Base_Color", Color) = (0,0.2441731,0.5188679,0)
		[HDR]_Color_B("Color_B", Color) = (0,0.1349197,0.4433962,0)
		[HDR]_High_Color("High_Color", Color) = (1,1,1,0)
		_OutSide_Fresnel_Scale("OutSide_Fresnel_Scale", Range( 0 , 1)) = 1
		_OutSide_Fresnel_Power("OutSide_Fresnel_Power", Range( 1 , 30)) = 2.364706
		_Fresnel_Scale("Fresnel_Scale", Range( 0 , 1)) = 1
		_Fresnel_Power("Fresnel_Power", Range( 1 , 30)) = 2.364706
		_OutSide_Frsnel_Range("OutSide_Frsnel_Range", Range( 0 , 1.5)) = 0.3529411
		_Fresnel_Range("Fresnel_Range", Range( 0 , 1.5)) = 0.03159646
		[HDR]_Fresnel_Color("Fresnel_Color", Color) = (0,0.08493042,1,0)
		_Vertex_Normal_Tex("Vertex_Normal_Tex", 2D) = "white" {}
		_Vertex_Normal_Str("Vertex_Normal_Str", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform sampler2D _Vertex_Normal_Tex;
		uniform float _Main_UPanner;
		uniform float _Main_VPanner;
		uniform float4 _Vertex_Normal_Tex_ST;
		uniform float _Main_Rot;
		uniform float _Vertex_Normal_Str;
		uniform float4 _Fresnel_Color;
		uniform float _Smooth;
		uniform float _Fresnel_Range;
		uniform float _Fresnel_Scale;
		uniform float _Fresnel_Power;
		uniform sampler2D _Main_Tex;
		uniform float4 _Main_Tex_ST;
		uniform float4 _High_Color;
		uniform float4 _Base_Color;
		uniform float4 _Color_B;
		uniform float _Base_Range;
		uniform float _High_Range;
		uniform float _OutSide_Frsnel_Range;
		uniform float _OutSide_Fresnel_Scale;
		uniform float _OutSide_Fresnel_Power;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 appendResult67 = (float2(_Main_UPanner , _Main_VPanner));
			float2 uv0_Vertex_Normal_Tex = v.texcoord.xy * _Vertex_Normal_Tex_ST.xy + _Vertex_Normal_Tex_ST.zw;
			float temp_output_62_0 = ( uv0_Vertex_Normal_Tex.y * _Main_Rot );
			float2 appendResult66 = (float2(( uv0_Vertex_Normal_Tex.x + temp_output_62_0 ) , temp_output_62_0));
			float2 panner68 = ( 1.0 * _Time.y * appendResult67 + appendResult66);
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( tex2Dlod( _Vertex_Normal_Tex, float4( panner68, 0, 0.0) ).r * ase_vertexNormal ) * _Vertex_Normal_Str );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 temp_cast_0 = (0.9).xxxx;
			float Smooth25 = _Smooth;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV41 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode41 = ( 0.0 + _Fresnel_Scale * pow( 1.0 - fresnelNdotV41, _Fresnel_Power ) );
			float temp_output_44_0 = saturate( fresnelNode41 );
			float2 appendResult10 = (float2(_Main_UPanner , _Main_VPanner));
			float2 uv0_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float2 appendResult6 = (float2(( uv0_Main_Tex.x + ( uv0_Main_Tex.y * _Main_Rot ) ) , uv0_Main_Tex.y));
			float2 panner7 = ( 1.0 * _Time.y * appendResult10 + appendResult6);
			float4 tex2DNode1 = tex2D( _Main_Tex, panner7 );
			float smoothstepResult48 = smoothstep( ( Smooth25 + ( 1.0 - _Fresnel_Range ) ) , 1.0 , ( 1.0 - ( temp_output_44_0 * ( temp_output_44_0 + tex2DNode1.r ) ) ));
			float4 lerpResult53 = lerp( temp_cast_0 , _Fresnel_Color , smoothstepResult48);
			float temp_output_15_0 = ( 1.0 - _Base_Range );
			float smoothstepResult11 = smoothstep( ( Smooth25 + temp_output_15_0 ) , temp_output_15_0 , tex2DNode1.r);
			float4 lerpResult21 = lerp( _Base_Color , _Color_B , smoothstepResult11);
			float temp_output_18_0 = ( 1.0 - _High_Range );
			float smoothstepResult22 = smoothstep( ( Smooth25 + temp_output_18_0 ) , temp_output_18_0 , tex2DNode1.r);
			float4 lerpResult23 = lerp( _High_Color , lerpResult21 , smoothstepResult22);
			o.Emission = ( lerpResult53 * lerpResult23 ).rgb;
			float temp_output_39_0 = ( 1.0 - _OutSide_Frsnel_Range );
			float fresnelNdotV28 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode28 = ( 0.0 + _OutSide_Fresnel_Scale * pow( 1.0 - fresnelNdotV28, _OutSide_Fresnel_Power ) );
			float temp_output_31_0 = saturate( fresnelNode28 );
			float smoothstepResult37 = smoothstep( ( Smooth25 + temp_output_39_0 ) , temp_output_39_0 , ( 1.0 - ( temp_output_31_0 * ( temp_output_31_0 + tex2DNode1.r ) ) ));
			o.Alpha = saturate( smoothstepResult37 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit keepalpha fullforwardshadows noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
0;714;1548;646;2513.812;594.144;2.757569;True;False
Node;AmplifyShaderEditor.RangedFloatNode;5;-1560.638,140.8035;Float;False;Property;_Main_Rot;Main_Rot;1;0;Create;True;0;0;False;0;-1.01;-1.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1826.185,-85.16529;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-1369.412,84.64874;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1155.161,198.9245;Float;False;Property;_Main_VPanner;Main_VPanner;3;0;Create;True;0;0;False;0;-1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1146.426,111.5726;Float;False;Property;_Main_UPanner;Main_UPanner;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;57;-1253.93,-1657.166;Float;False;2073.232;626.948;Comment;15;42;43;41;44;45;52;46;51;49;47;50;55;54;48;53;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-1307.019,-256.0229;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;26;-1855.491,-543.5121;Float;False;456.7274;167.0906;Comment;2;13;25;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1202.93,-1391.72;Float;False;Property;_Fresnel_Power;Fresnel_Power;13;0;Create;True;0;0;False;0;2.364706;2.364706;1;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-982.9524,145.2654;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1203.93,-1476.72;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;12;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;-1099.87,-51.37043;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1169.739,1484.271;Float;False;Property;_OutSide_Fresnel_Scale;OutSide_Fresnel_Scale;10;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1168.739,1569.271;Float;False;Property;_OutSide_Fresnel_Power;OutSide_Fresnel_Power;11;0;Create;True;0;0;False;0;2.364706;2.364706;1;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;59;-1823.382,552.9571;Float;True;0;58;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-1805.491,-491.4215;Float;False;Property;_Smooth;Smooth;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;41;-910.3007,-1510.8;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;7;-910.1904,-50.12243;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-673.6407,-92.54059;Float;True;Property;_Main_Tex;Main_Tex;0;0;Create;True;0;0;False;0;8d21b35fab1359d4aa689ddf302e1b01;8d21b35fab1359d4aa689ddf302e1b01;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-958.285,300.1668;Float;False;Property;_Base_Range;Base_Range;5;0;Create;True;0;0;False;0;0.6779342;0.6779342;-1.5;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-1626.764,-493.5121;Float;False;Smooth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;44;-602.896,-1503.297;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;28;-876.1089,1450.191;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-1319.089,738.8829;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;63;-1223.539,599.5975;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-452.1555,-1147.218;Float;False;Property;_Fresnel_Range;Fresnel_Range;15;0;Create;True;0;0;False;0;0.03159646;0.03159646;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-438.2515,339.9349;Float;False;Property;_High_Range;High_Range;6;0;Create;True;0;0;False;0;0.9272076;0.9272076;-1.5;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;31;-551.7383,1452.271;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-436.7126,-1373.975;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-795.3987,186.6099;Float;False;25;Smooth;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;15;-685.956,288.7717;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-181.2645,1850.905;Float;False;Property;_OutSide_Frsnel_Range;OutSide_Frsnel_Range;14;0;Create;True;0;0;False;0;0.3529411;0.3529411;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-381.9588,1587.604;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;51;-138.1554,-1140.218;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-188.6006,-1493.775;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;67;-952.2682,441.8866;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;66;-1029.064,631.8804;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-566.1592,135.2824;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;18;-158.7256,280.0364;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;49;-194.225,-1253.691;Float;False;25;Smooth;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;73;-429.6083,505.089;Float;False;887.7608;445.6733;;5;58;70;72;71;69;Vertex_Normal;0.9518021,1,0,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-81.77815,141.9875;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;11;-274.1549,-71.86628;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;47;39.74064,-1492.566;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;68;-839.3842,633.1284;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;20;-381.4722,-382.5893;Float;False;Property;_Color_B;Color_B;8;1;[HDR];Create;True;0;0;False;0;0,0.1349197,0.4433962,0;0.1981132,0.03177288,0.06129611,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-84.23374,1476.834;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;49.31372,-1216.609;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;39;268.4849,1869.651;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;35;-24.26451,1700.905;Float;False;25;Smooth;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;19;-365.2495,-622.1827;Float;False;Property;_Base_Color;Base_Color;7;1;[HDR];Create;True;0;0;False;0;0,0.2441731,0.5188679,0;6.964404,0.6827847,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;27;28.00522,-733.848;Float;False;Property;_High_Color;High_Color;9;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;48;251.1671,-1329.465;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;21;-28.32133,-347.6486;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;148.0375,1703.159;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;417.635,-1597.166;Float;False;Constant;_Float0;Float 0;17;0;Create;True;0;0;False;0;0.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;58;-379.6083,555.089;Float;True;Property;_Vertex_Normal_Tex;Vertex_Normal_Tex;17;0;Create;True;0;0;False;0;7dafa5b81fffb8a41aaed27d6d1e7b58;7dafa5b81fffb8a41aaed27d6d1e7b58;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;54;213.635,-1607.166;Float;False;Property;_Fresnel_Color;Fresnel_Color;16;1;[HDR];Create;True;0;0;False;0;0,0.08493042,1,0;1.054538,0.2539724,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;34;162.7663,1465.834;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;69;-236.6213,771.7623;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;22;99.55463,64.53324;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;23;262.5547,-504.0667;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-43.33458,556.3459;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-3.347497,808.4373;Float;False;Property;_Vertex_Normal_Str;Vertex_Normal_Str;18;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;53;553.635,-1423.166;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;37;390.7355,1460.905;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;40;624.2532,1206.397;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;645.3167,-561.9452;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;223.1525,564.8372;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;79;961.5042,-581.6788;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Fire_Tornado;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;2;2
WireConnection;4;1;5;0
WireConnection;3;0;2;1
WireConnection;3;1;4;0
WireConnection;10;0;8;0
WireConnection;10;1;9;0
WireConnection;6;0;3;0
WireConnection;6;1;2;2
WireConnection;41;2;42;0
WireConnection;41;3;43;0
WireConnection;7;0;6;0
WireConnection;7;2;10;0
WireConnection;1;1;7;0
WireConnection;25;0;13;0
WireConnection;44;0;41;0
WireConnection;28;2;29;0
WireConnection;28;3;30;0
WireConnection;62;0;59;2
WireConnection;62;1;5;0
WireConnection;63;0;59;1
WireConnection;63;1;62;0
WireConnection;31;0;28;0
WireConnection;45;0;44;0
WireConnection;45;1;1;1
WireConnection;15;0;14;0
WireConnection;32;0;31;0
WireConnection;32;1;1;1
WireConnection;51;0;52;0
WireConnection;46;0;44;0
WireConnection;46;1;45;0
WireConnection;67;0;8;0
WireConnection;67;1;9;0
WireConnection;66;0;63;0
WireConnection;66;1;62;0
WireConnection;12;0;24;0
WireConnection;12;1;15;0
WireConnection;18;0;17;0
WireConnection;16;0;24;0
WireConnection;16;1;18;0
WireConnection;11;0;1;1
WireConnection;11;1;12;0
WireConnection;11;2;15;0
WireConnection;47;0;46;0
WireConnection;68;0;66;0
WireConnection;68;2;67;0
WireConnection;33;0;31;0
WireConnection;33;1;32;0
WireConnection;50;0;49;0
WireConnection;50;1;51;0
WireConnection;39;0;38;0
WireConnection;48;0;47;0
WireConnection;48;1;50;0
WireConnection;21;0;19;0
WireConnection;21;1;20;0
WireConnection;21;2;11;0
WireConnection;36;0;35;0
WireConnection;36;1;39;0
WireConnection;58;1;68;0
WireConnection;34;0;33;0
WireConnection;22;0;1;1
WireConnection;22;1;16;0
WireConnection;22;2;18;0
WireConnection;23;0;27;0
WireConnection;23;1;21;0
WireConnection;23;2;22;0
WireConnection;70;0;58;1
WireConnection;70;1;69;0
WireConnection;53;0;55;0
WireConnection;53;1;54;0
WireConnection;53;2;48;0
WireConnection;37;0;34;0
WireConnection;37;1;36;0
WireConnection;37;2;39;0
WireConnection;40;0;37;0
WireConnection;56;0;53;0
WireConnection;56;1;23;0
WireConnection;71;0;70;0
WireConnection;71;1;72;0
WireConnection;79;2;56;0
WireConnection;79;9;40;0
WireConnection;79;11;71;0
ASEEND*/
//CHKSM=7E84B09E04BE2AD3ED0837B39200F15E7965C564