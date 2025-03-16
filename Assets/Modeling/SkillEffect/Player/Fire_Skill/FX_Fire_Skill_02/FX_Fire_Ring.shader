// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Fire_Tornado_Ring"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Main_Upanner("Main_Upanner", Float) = 0
		_Main_Vpanner("Main_Vpanner", Float) = 0
		[HDR]_Color_A("Color_A", Color) = (1,0,0,0)
		[HDR]_Color_B("Color_B", Color) = (1,0.9714144,0,0)
		_Rot("Rot", Float) = -1.01
		_Mask_TEx("Mask_TEx", 2D) = "white" {}
		_Mask_UPanner("Mask_UPanner", Float) = 0
		_Mask_VPanner("Mask_VPanner", Float) = 0
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Normal_Upanner("Normal_Upanner", Float) = 0
		_Normal_Vpanner("Normal_Vpanner", Float) = 0
		_Color_Offset("Color_Offset", Float) = 1
		_Color_Range("Color_Range", Float) = 1
		_Distortion("Distortion", Range( 0 , 1)) = 0
		_Dissovle("Dissovle", Range( -1 , 1)) = 1
		_Opacity("Opacity", Float) = 1
		_Normal_VerTEX("Normal_VerTEX", 2D) = "white" {}
		_Ver_Normal_Str("Ver_Normal_Str", Range( 0 , 1)) = 0
		_Depth_Fade("Depth_Fade", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform sampler2D _Normal_VerTEX;
		uniform float _Main_Upanner;
		uniform float _Main_Vpanner;
		uniform float4 _Normal_VerTEX_ST;
		uniform float _Rot;
		uniform float _Ver_Normal_Str;
		uniform float4 _Color_A;
		uniform float4 _Color_B;
		uniform sampler2D _Main_Tex;
		uniform sampler2D _Normal_Tex;
		uniform float _Normal_Upanner;
		uniform float _Normal_Vpanner;
		uniform float4 _Normal_Tex_ST;
		uniform float _Distortion;
		uniform float4 _Main_Tex_ST;
		uniform float _Color_Offset;
		uniform float _Color_Range;
		uniform float _Dissovle;
		uniform sampler2D _Mask_TEx;
		uniform float _Mask_UPanner;
		uniform float _Mask_VPanner;
		uniform float4 _Mask_TEx_ST;
		uniform float _Opacity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth_Fade;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 appendResult60 = (float2(_Main_Upanner , _Main_Vpanner));
			float2 uv0_Normal_VerTEX = v.texcoord.xy * _Normal_VerTEX_ST.xy + _Normal_VerTEX_ST.zw;
			float temp_output_55_0 = ( uv0_Normal_VerTEX.y * _Rot );
			float2 appendResult61 = (float2(( uv0_Normal_VerTEX.x + temp_output_55_0 ) , temp_output_55_0));
			float2 panner66 = ( 1.0 * _Time.y * appendResult60 + appendResult61);
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( tex2Dlod( _Normal_VerTEX, float4( panner66, 0, 0.0) ).r * ase_vertexNormal ) * _Ver_Normal_Str );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult9 = (float2(_Normal_Upanner , _Normal_Vpanner));
			float2 uv0_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 panner8 = ( 1.0 * _Time.y * appendResult9 + uv0_Normal_Tex);
			float2 temp_output_5_0 = ( (UnpackNormal( tex2D( _Normal_Tex, panner8 ) )).xy * _Distortion );
			float2 appendResult50 = (float2(_Main_Upanner , _Main_Vpanner));
			float2 uv0_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float2 appendResult22 = (float2(( uv0_Main_Tex.x + ( uv0_Main_Tex.y * _Rot ) ) , uv0_Main_Tex.y));
			float2 panner21 = ( 1.0 * _Time.y * appendResult50 + appendResult22);
			float4 tex2DNode1 = tex2D( _Main_Tex, ( temp_output_5_0 + panner21 ) );
			float4 lerpResult32 = lerp( _Color_A , _Color_B , saturate( ( saturate( pow( tex2DNode1.r , _Color_Offset ) ) * _Color_Range ) ));
			o.Emission = ( lerpResult32 * i.vertexColor ).rgb;
			float2 appendResult77 = (float2(_Mask_UPanner , _Mask_VPanner));
			float2 uv0_Mask_TEx = i.uv_texcoord * _Mask_TEx_ST.xy + _Mask_TEx_ST.zw;
			float2 panner79 = ( 1.0 * _Time.y * appendResult77 + uv0_Mask_TEx);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth81 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth81 = abs( ( screenDepth81 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth_Fade ) );
			o.Alpha = ( i.vertexColor.a * saturate( ( ( tex2DNode1.r + _Dissovle ) * tex2D( _Mask_TEx, ( temp_output_5_0 + panner79 ) ).r * _Opacity ) ) * saturate( distanceDepth81 ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 

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
				float4 screenPos : TEXCOORD3;
				half4 color : COLOR0;
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
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				o.color = v.color;
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
				surfIN.screenPos = IN.screenPos;
				surfIN.vertexColor = IN.color;
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
500.6667;721.3334;1136;646;1229.534;-136.3558;1.276543;True;False
Node;AmplifyShaderEditor.CommentaryNode;26;-3282.329,-554.3131;Float;False;1630.885;442;;9;10;11;9;8;6;5;3;4;7;Normal_Tex;0,0.799809,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-3206.329,-227.313;Float;False;Property;_Normal_Vpanner;Normal_Vpanner;11;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-3196.329,-359.313;Float;False;Property;_Normal_Upanner;Normal_Upanner;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-2861.145,34.39958;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;-2744.279,375.739;Float;False;Property;_Rot;Rot;5;0;Create;True;0;0;False;0;-1.01;-1.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-2990.329,-313.313;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-3232.329,-504.313;Float;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;8;-2809.329,-447.313;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-2558.428,237.8237;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2273.824,415.6172;Float;False;Property;_Main_Vpanner;Main_Vpanner;2;0;Create;True;0;0;False;0;0;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-2404.573,-83.99686;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-2263.824,283.6172;Float;False;Property;_Main_Upanner;Main_Upanner;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-2516.739,-384.5775;Float;True;Property;_Normal_Tex;Normal_Tex;9;0;Create;True;0;0;False;0;51fe2c9d5b236124d9f9e7ea528b0bea;4c8faf9427d160841865e5c67602aef9;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;22;-2159.652,92.61931;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-2199.582,-260.1866;Float;False;Property;_Distortion;Distortion;14;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;3;-2170.883,-384.757;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;50;-2045.688,343.964;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1886.445,-378.4445;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;21;-1977.418,99.25938;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-2203.907,698.1367;Float;False;Property;_Mask_UPanner;Mask_UPanner;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-2213.907,830.1367;Float;False;Property;_Mask_VPanner;Mask_VPanner;8;0;Create;True;0;0;False;0;0;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;51;-2697.92,1011.949;Float;True;0;69;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-1708.874,91.45319;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;-2168.594,535.5719;Float;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;77;-1985.771,758.4835;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-1522.363,64.74133;Float;True;Property;_Main_Tex;Main_Tex;0;0;Create;True;0;0;False;0;75051480ad010e14b9b7f3870000ab87;c2f5e06ce5d539b418dc5ebfbfeeee94;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-2193.627,1197.875;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1291.322,-30.34667;Float;False;Property;_Color_Offset;Color_Offset;12;0;Create;True;0;0;False;0;1;4.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;27;-1159.084,89.40221;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-2098.077,1058.589;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;79;-1852.993,599.9879;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;29;-1022.084,84.40221;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-979.0842,-42.59779;Float;False;Property;_Color_Range;Color_Range;13;0;Create;True;0;0;False;0;1;9.93;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;61;-1903.602,1090.872;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-1577.287,632.405;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1772.273,283.5122;Float;False;Property;_Dissovle;Dissovle;15;0;Create;True;0;0;False;0;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;60;-1826.806,900.8781;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-1169.073,252.3121;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;65;-1304.146,964.0806;Float;False;887.7608;445.6733;;5;74;72;71;70;69;Vertex_Normal;0.9518021,1,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1041.879,773.2469;Float;False;Property;_Opacity;Opacity;16;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-496.7985,732.5014;Float;False;Property;_Depth_Fade;Depth_Fade;19;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-815.0842,77.40221;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;66;-1713.922,1092.12;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-1376.031,641.7487;Float;True;Property;_Mask_TEx;Mask_TEx;6;0;Create;True;0;0;False;0;ccb8549547c6d53418c09427b34fc823;c7d564bbc661feb448e7dcb86e2aa438;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-915.5769,632.2574;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;34;-954.0842,-246.5978;Float;False;Property;_Color_B;Color_B;4;1;[HDR];Create;True;0;0;False;0;1,0.9714144,0,0;1,0.9633075,0.7264151,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;35;-671.0842,61.40222;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;33;-953.0842,-445.5978;Float;False;Property;_Color_A;Color_A;3;1;[HDR];Create;True;0;0;False;0;1,0,0,0;1.112218,0.3726803,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;69;-1254.146,1014.081;Float;True;Property;_Normal_VerTEX;Normal_VerTEX;17;0;Create;True;0;0;False;0;7dafa5b81fffb8a41aaed27d6d1e7b58;fdb7f2284c843954baf647c1c33d72fe;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;70;-1111.16,1230.754;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;81;-269.4627,699.2798;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;32;-510.0842,-209.5978;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-917.873,1015.338;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;44;-599.5247,496.58;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;83;12.54214,701.8646;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;37;-183.6727,79.41208;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;72;-877.8858,1267.429;Float;False;Property;_Ver_Normal_Str;Ver_Normal_Str;18;0;Create;True;0;0;False;0;0;0.18;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;10.4622,-146.7879;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-5.243874,260.7245;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-651.3858,1023.829;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;266.6001,-46.8;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Fire_Tornado_Ring;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;11;0
WireConnection;9;1;10;0
WireConnection;8;0;7;0
WireConnection;8;2;9;0
WireConnection;48;0;20;2
WireConnection;48;1;49;0
WireConnection;47;0;20;1
WireConnection;47;1;48;0
WireConnection;4;1;8;0
WireConnection;22;0;47;0
WireConnection;22;1;20;2
WireConnection;3;0;4;0
WireConnection;50;0;23;0
WireConnection;50;1;24;0
WireConnection;5;0;3;0
WireConnection;5;1;6;0
WireConnection;21;0;22;0
WireConnection;21;2;50;0
WireConnection;25;0;5;0
WireConnection;25;1;21;0
WireConnection;77;0;75;0
WireConnection;77;1;76;0
WireConnection;1;1;25;0
WireConnection;55;0;51;2
WireConnection;55;1;49;0
WireConnection;27;0;1;1
WireConnection;27;1;28;0
WireConnection;56;0;51;1
WireConnection;56;1;55;0
WireConnection;79;0;42;0
WireConnection;79;2;77;0
WireConnection;29;0;27;0
WireConnection;61;0;56;0
WireConnection;61;1;55;0
WireConnection;41;0;5;0
WireConnection;41;1;79;0
WireConnection;60;0;23;0
WireConnection;60;1;24;0
WireConnection;38;0;1;1
WireConnection;38;1;39;0
WireConnection;30;0;29;0
WireConnection;30;1;31;0
WireConnection;66;0;61;0
WireConnection;66;2;60;0
WireConnection;2;1;41;0
WireConnection;40;0;38;0
WireConnection;40;1;2;1
WireConnection;40;2;43;0
WireConnection;35;0;30;0
WireConnection;69;1;66;0
WireConnection;81;0;82;0
WireConnection;32;0;33;0
WireConnection;32;1;34;0
WireConnection;32;2;35;0
WireConnection;71;0;69;1
WireConnection;71;1;70;0
WireConnection;44;0;40;0
WireConnection;83;0;81;0
WireConnection;36;0;32;0
WireConnection;36;1;37;0
WireConnection;46;0;37;4
WireConnection;46;1;44;0
WireConnection;46;2;83;0
WireConnection;74;0;71;0
WireConnection;74;1;72;0
WireConnection;0;2;36;0
WireConnection;0;9;46;0
WireConnection;0;11;74;0
ASEEND*/
//CHKSM=62B334B5E37738DDA8BE99C53303247AFB2E5A2E