// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Water_Projectle"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Color_Offset("Color_Offset", Float) = 1
		_Color_Range("Color_Range", Float) = 1
		[HDR]_Color_A("Color_A", Color) = (0,0.7448976,1,0)
		[HDR]_Color_B("Color_B", Color) = (0.745283,1,0.9340957,0)
		_Main_UPanner("Main_UPanner", Float) = 0
		_Main_VPanner("Main_VPanner", Float) = 0
		_HighLight_Tex("HighLight_Tex", 2D) = "white" {}
		[HDR]_Color_High("Color_High", Color) = (1,1,1,0)
		_HighLight_Range("HighLight_Range", Range( 0.01 , 0.9)) = 0.1607939
		_HighLight_UPanner("HighLight_UPanner", Float) = 0
		_HighLight_VPanner("HighLight_VPanner", Float) = 0
		_Normal_Tex("Normal_Tex", 2D) = "white" {}
		_Normal_UPanner("Normal_UPanner", Float) = 0
		_Normal_VPanner("Normal_VPanner", Float) = 0
		_Distortion("Distortion", Range( 0 , 1)) = 0
		_Dissolve("Dissolve", Range( -1 , 1)) = 0
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		_Opacity("Opacity", Float) = 1
		[Toggle(_CUSTOM_DISSOLVE_ON)] _CUSTOM_Dissolve("CUSTOM_Dissolve", Float) = 0
		[Toggle(_CUSTOM_DISTORTION_ON)] _CUSTOM_Distortion("CUSTOM_Distortion", Float) = 0
		_Depth_Fade("Depth_Fade", Float) = 0
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature _CUSTOM_DISTORTION_ON
		#pragma shader_feature _CUSTOM_DISSOLVE_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform float4 _Color_High;
		uniform float _HighLight_Range;
		uniform sampler2D _HighLight_Tex;
		uniform sampler2D _Normal_Tex;
		uniform float _Normal_UPanner;
		uniform float _Normal_VPanner;
		uniform float4 _Normal_Tex_ST;
		uniform float _Distortion;
		uniform float _HighLight_UPanner;
		uniform float _HighLight_VPanner;
		uniform float4 _HighLight_Tex_ST;
		uniform float4 _Color_A;
		uniform float4 _Color_B;
		uniform sampler2D _Main_Tex;
		uniform float _Main_UPanner;
		uniform float _Main_VPanner;
		uniform float4 _Main_Tex_ST;
		uniform float _Color_Offset;
		uniform float _Color_Range;
		uniform float _Dissolve;
		uniform sampler2D _Mask_Tex;
		uniform float4 _Mask_Tex_ST;
		uniform float _Opacity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth_Fade;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult37 = (float2(_Normal_UPanner , _Normal_VPanner));
			float2 uv0_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 panner36 = ( 1.0 * _Time.y * appendResult37 + uv0_Normal_Tex);
			#ifdef _CUSTOM_DISTORTION_ON
				float staticSwitch54 = i.uv_tex4coord.w;
			#else
				float staticSwitch54 = _Distortion;
			#endif
			float3 temp_output_40_0 = ( (tex2D( _Normal_Tex, panner36 )).rga * staticSwitch54 );
			float2 appendResult5 = (float2(_HighLight_UPanner , _HighLight_VPanner));
			float2 uv0_HighLight_Tex = i.uv_texcoord * _HighLight_Tex_ST.xy + _HighLight_Tex_ST.zw;
			float2 panner4 = ( 1.0 * _Time.y * appendResult5 + uv0_HighLight_Tex);
			float2 appendResult29 = (float2(_Main_UPanner , _Main_VPanner));
			float2 uv0_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float2 panner28 = ( 1.0 * _Time.y * appendResult29 + uv0_Main_Tex);
			float4 tex2DNode1 = tex2D( _Main_Tex, ( temp_output_40_0 + float3( panner28 ,  0.0 ) ).xy );
			float4 lerpResult20 = lerp( _Color_A , _Color_B , saturate( ( saturate( pow( tex2DNode1.r , _Color_Offset ) ) * _Color_Range ) ));
			o.Emission = ( ( ( _Color_High * step( _HighLight_Range , tex2D( _HighLight_Tex, ( temp_output_40_0 + float3( panner4 ,  0.0 ) ).xy ).r ) ) + lerpResult20 ) * i.vertexColor ).rgb;
			#ifdef _CUSTOM_DISSOLVE_ON
				float staticSwitch52 = i.uv_tex4coord.z;
			#else
				float staticSwitch52 = _Dissolve;
			#endif
			float2 uv0_Mask_Tex = i.uv_texcoord * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth55 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth55 = abs( ( screenDepth55 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth_Fade ) );
			o.Alpha = ( i.vertexColor.a * saturate( ( ( tex2DNode1.r + staticSwitch52 ) * tex2D( _Mask_Tex, ( temp_output_40_0 + float3( uv0_Mask_Tex ,  0.0 ) ).xy ).r * _Opacity ) ) * saturate( distanceDepth55 ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
492.6667;714;1077;638;893.6867;172.8261;1.308804;True;False
Node;AmplifyShaderEditor.CommentaryNode;42;-3056.655,-427.0146;Float;False;1408.963;473.6998;Comment;10;36;37;33;35;38;39;34;40;41;54;;0,1,0.9898431,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-3000.156,-68.31478;Float;False;Property;_Normal_VPanner;Normal_VPanner;15;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-3006.655,-190.5147;Float;False;Property;_Normal_UPanner;Normal_UPanner;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;37;-2771.356,-128.1147;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;-2967.655,-347.8148;Float;False;0;33;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;36;-2619.256,-329.6148;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2292.027,379.6261;Float;False;Property;_Main_VPanner;Main_VPanner;7;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2298.526,257.4262;Float;False;Property;_Main_UPanner;Main_UPanner;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;33;-2350.136,-373.4733;Float;True;Property;_Normal_Tex;Normal_Tex;13;0;Create;True;0;0;False;0;44116b33e8fe54b4390df94f441c325f;44116b33e8fe54b4390df94f441c325f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-2350.78,-124.7229;Float;False;Property;_Distortion;Distortion;16;0;Create;True;0;0;False;0;0;0.138;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;53;-2596.941,382.2589;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-2260.526,100.1262;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;54;-2045.72,-162.4131;Float;False;Property;_CUSTOM_Distortion;CUSTOM_Distortion;21;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;34;-2045.394,-377.0146;Float;False;True;True;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;29;-2063.227,319.8262;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;13;-1584.553,-1245.723;Float;False;1664.605;734.2399;Comment;11;3;4;5;6;7;2;8;9;10;11;12;;0.1515336,1,0,1;0;0
Node;AmplifyShaderEditor.PannerNode;28;-1911.127,118.3262;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-1811.025,-361.9146;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-1700.527,128.7265;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1534.553,-748.6827;Float;False;Property;_HighLight_UPanner;HighLight_UPanner;11;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1528.054,-626.4826;Float;False;Property;_HighLight_VPanner;HighLight_VPanner;12;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1527.009,100.5579;Float;True;Property;_Main_Tex;Main_Tex;1;0;Create;True;0;0;False;0;c2f5e06ce5d539b418dc5ebfbfeeee94;7ead229491461f64fb8abd86dfcd7d4b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1495.553,-905.9825;Float;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;5;-1299.253,-686.2826;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1228.474,311.9125;Float;False;Property;_Color_Offset;Color_Offset;2;0;Create;True;0;0;False;0;1;6.43;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;14;-1089.474,136.9125;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;4;-1147.153,-887.7826;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-936.5532,-877.3824;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1912.917,349.1776;Float;False;Property;_Dissolve;Dissolve;17;0;Create;True;0;0;False;0;0;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-941.4739,256.9125;Float;False;Property;_Color_Range;Color_Range;3;0;Create;True;0;0;False;0;1;4.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;48;-2162.497,660.8546;Float;False;0;46;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;16;-921.4739,135.9125;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;52;-1542.669,412.3862;Float;False;Property;_CUSTOM_Dissolve;CUSTOM_Dissolve;20;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-777.9534,-889.0825;Float;True;Property;_HighLight_Tex;HighLight_Tex;8;0;Create;True;0;0;False;0;c10244bcb5987bd41b64e7758c3af36f;c10244bcb5987bd41b64e7758c3af36f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-657.8821,-1076.122;Float;False;Property;_HighLight_Range;HighLight_Range;10;0;Create;True;0;0;False;0;0.1607939;0.01;0.01;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-776.4739,140.9125;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-1613.303,683.4389;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;12;-399.1824,-1195.723;Float;False;Property;_Color_High;Color_High;9;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;19;-635.4739,141.9125;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;46;-1456.987,648.9416;Float;True;Property;_Mask_Tex;Mask_Tex;18;0;Create;True;0;0;False;0;7f78920f34007b34ca97216413e97960;604a52c360f2090428767147e4f5e133;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-803.4739,-57.08751;Float;False;Property;_Color_B;Color_B;5;1;[HDR];Create;True;0;0;False;0;0.745283,1,0.9340957,0;7.02519,11.47448,14.92853,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;56;-624.3029,633.6295;Float;False;Property;_Depth_Fade;Depth_Fade;22;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;9;-393.9824,-883.7223;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1080.396,765.928;Float;False;Property;_Opacity;Opacity;19;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-1162.211,467.8801;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;-812.4739,-241.0875;Float;False;Property;_Color_A;Color_A;4;1;[HDR];Create;True;0;0;False;0;0,0.7448976,1,0;0,0.8863635,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-845.3958,617.928;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;20;-472.4739,-87.08749;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-83.28245,-951.3224;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;55;-425.459,584.9242;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;51;-378.3958,413.928;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-177.5858,-284.3786;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;25;-149.4655,74.14343;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;57;-125.5735,562.8418;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;49.04803,-163.7424;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;146.2962,232.8928;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;363,-19;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Water_Projectle;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;37;0;38;0
WireConnection;37;1;39;0
WireConnection;36;0;35;0
WireConnection;36;2;37;0
WireConnection;33;1;36;0
WireConnection;54;1;41;0
WireConnection;54;0;53;4
WireConnection;34;0;33;0
WireConnection;29;0;30;0
WireConnection;29;1;31;0
WireConnection;28;0;27;0
WireConnection;28;2;29;0
WireConnection;40;0;34;0
WireConnection;40;1;54;0
WireConnection;32;0;40;0
WireConnection;32;1;28;0
WireConnection;1;1;32;0
WireConnection;5;0;6;0
WireConnection;5;1;7;0
WireConnection;14;0;1;1
WireConnection;14;1;15;0
WireConnection;4;0;3;0
WireConnection;4;2;5;0
WireConnection;8;0;40;0
WireConnection;8;1;4;0
WireConnection;16;0;14;0
WireConnection;52;1;44;0
WireConnection;52;0;53;3
WireConnection;2;1;8;0
WireConnection;17;0;16;0
WireConnection;17;1;18;0
WireConnection;47;0;40;0
WireConnection;47;1;48;0
WireConnection;19;0;17;0
WireConnection;46;1;47;0
WireConnection;9;0;10;0
WireConnection;9;1;2;1
WireConnection;43;0;1;1
WireConnection;43;1;52;0
WireConnection;49;0;43;0
WireConnection;49;1;46;1
WireConnection;49;2;50;0
WireConnection;20;0;21;0
WireConnection;20;1;22;0
WireConnection;20;2;19;0
WireConnection;11;0;12;0
WireConnection;11;1;9;0
WireConnection;55;0;56;0
WireConnection;51;0;49;0
WireConnection;23;0;11;0
WireConnection;23;1;20;0
WireConnection;57;0;55;0
WireConnection;24;0;23;0
WireConnection;24;1;25;0
WireConnection;26;0;25;4
WireConnection;26;1;51;0
WireConnection;26;2;57;0
WireConnection;0;2;24;0
WireConnection;0;9;26;0
ASEEND*/
//CHKSM=F1A4E54FA4DB825CBAE23AA70F865872198F8D19