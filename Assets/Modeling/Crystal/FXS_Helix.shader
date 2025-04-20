// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify Shader/Skill/FX_Helix"
{
	Properties
	{
		_Main_TEx("Main_TEx", 2D) = "white" {}
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_NOise_UPanner("NOise_UPanner", Float) = 0
		_NOise_VPanner("NOise_VPanner", Float) = 0
		_Main_Power("Main_Power", Float) = 1
		[HDR]_Main_color("Main_color", Color) = (1,1,1,0)
		_Opacity("Opacity", Float) = 1
		_normal_Tex("normal_Tex", 2D) = "bump" {}
		_distotion("distotion", Range( 0 , 1)) = 0.4058736
		_normal_UPanner("normal_UPanner", Float) = 0
		_normal_VPanner("normal_VPanner", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Main_TEx;
		uniform sampler2D _normal_Tex;
		uniform float _normal_UPanner;
		uniform float _normal_VPanner;
		uniform float4 _normal_Tex_ST;
		uniform float _distotion;
		uniform float4 _Main_TEx_ST;
		uniform sampler2D _Noise_Tex;
		uniform float _NOise_UPanner;
		uniform float _NOise_VPanner;
		uniform float4 _Noise_Tex_ST;
		uniform float _Main_Power;
		uniform float4 _Main_color;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult37 = (float2(_normal_UPanner , _normal_VPanner));
			float2 uv0_normal_Tex = i.uv_texcoord * _normal_Tex_ST.xy + _normal_Tex_ST.zw;
			float2 panner34 = ( 1.0 * _Time.y * appendResult37 + uv0_normal_Tex);
			float3 temp_output_31_0 = ( (UnpackNormal( tex2D( _normal_Tex, panner34 ) )).xyz * _distotion );
			float2 uv0_Main_TEx = i.uv_texcoord * _Main_TEx_ST.xy + _Main_TEx_ST.zw;
			float2 appendResult4 = (float2(( i.uv_tex4coord.z + uv0_Main_TEx.x ) , uv0_Main_TEx.y));
			float4 tex2DNode1 = tex2D( _Main_TEx, ( temp_output_31_0 + float3( appendResult4 ,  0.0 ) ).xy );
			float2 appendResult14 = (float2(_NOise_UPanner , _NOise_VPanner));
			float2 uv0_Noise_Tex = i.uv_texcoord * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
			float2 panner10 = ( 1.0 * _Time.y * appendResult14 + uv0_Noise_Tex);
			float temp_output_17_0 = saturate( ( tex2DNode1.r * ( tex2DNode1.r + tex2D( _Noise_Tex, ( temp_output_31_0 + float3( panner10 ,  0.0 ) ).xy ).r ) * saturate( ( ( i.uv_texcoord.x + 0.0 ) * ( 1.0 - i.uv_texcoord.x ) * 6.92 ) ) ) );
			o.Emission = ( pow( temp_output_17_0 , _Main_Power ) * _Main_color * i.vertexColor ).rgb;
			o.Alpha = ( i.vertexColor.a * saturate( ( temp_output_17_0 * _Opacity ) ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
1101;807;1028;592;318.9607;452.1459;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;39;-2848.101,-262.7501;Float;False;Property;_normal_VPanner;normal_VPanner;11;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-2863.905,-383.9138;Float;False;Property;_normal_UPanner;normal_UPanner;10;0;Create;True;0;0;False;0;0;-0.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-2715.083,-597.2672;Float;False;0;29;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;37;-2638.7,-379.9624;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;34;-2442.469,-606.4863;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;5;-2087.24,-414.2277;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-2106.24,-205.2283;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-2108.531,209.2514;Float;False;Property;_NOise_UPanner;NOise_UPanner;3;0;Create;True;0;0;False;0;0;-0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2103.531,340.252;Float;False;Property;_NOise_VPanner;NOise_VPanner;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;29;-2224.317,-703.2677;Float;True;Property;_normal_Tex;normal_Tex;8;0;Create;True;0;0;False;0;645b0a2fda25d114599a2fba6417fe81;645b0a2fda25d114599a2fba6417fe81;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;30;-1868.738,-703.3228;Float;True;True;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;14;-1901.531,263.252;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-1811.239,-303.2277;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1947.531,53.25163;Float;False;0;7;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;32;-1749.725,-461.6164;Float;False;Property;_distotion;distotion;9;0;Create;True;0;0;False;0;0.4058736;0.424;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;10;-1663.531,107.2516;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1411.256,-526.1492;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;41;-1355.553,346.0899;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;4;-1606.239,-198.2283;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-886.2534,572.2896;Float;False;Constant;_Float0;Float 0;12;0;Create;True;0;0;False;0;6.92;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;43;-1092.953,513.79;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-1200.083,16.95197;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-1142.883,-287.6854;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-1113.753,261.5896;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-719.8535,349.9893;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-822.524,52.91858;Float;True;Property;_Noise_Tex;Noise_Tex;2;0;Create;True;0;0;False;0;d010bcc6d54f28a41aa8be3558cfd9f3;c7d564bbc661feb448e7dcb86e2aa438;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-799.2324,-207.5611;Float;True;Property;_Main_TEx;Main_TEx;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-496.524,-84.08142;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;46;-437.7533,339.5894;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-270.524,-211.0814;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;48.18341,164.1339;Float;False;Property;_Opacity;Opacity;7;0;Create;True;0;0;False;0;1;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;17;6.183411,-209.8661;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;247.1834,49.13385;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;47.18341,-326.8661;Float;False;Property;_Main_Power;Main_Power;5;0;Create;True;0;0;False;0;1;2.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;25;466.1834,30.13385;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;18;255.1834,-271.8661;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;226.1834,-474.8661;Float;False;Property;_Main_color;Main_color;6;1;[HDR];Create;True;0;0;False;0;1,1,1,0;2,2,2,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;22;253.1834,-140.8661;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;461.1834,-328.8661;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;666.1834,-37.86615;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;935,-249;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Amplify Shader/Skill/FX_Helix;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;37;0;38;0
WireConnection;37;1;39;0
WireConnection;34;0;33;0
WireConnection;34;2;37;0
WireConnection;29;1;34;0
WireConnection;30;0;29;0
WireConnection;14;0;15;0
WireConnection;14;1;16;0
WireConnection;3;0;5;3
WireConnection;3;1;2;1
WireConnection;10;0;9;0
WireConnection;10;2;14;0
WireConnection;31;0;30;0
WireConnection;31;1;32;0
WireConnection;4;0;3;0
WireConnection;4;1;2;2
WireConnection;43;0;41;1
WireConnection;28;0;31;0
WireConnection;28;1;10;0
WireConnection;27;0;31;0
WireConnection;27;1;4;0
WireConnection;42;0;41;1
WireConnection;44;0;42;0
WireConnection;44;1;43;0
WireConnection;44;2;45;0
WireConnection;7;1;28;0
WireConnection;1;1;27;0
WireConnection;8;0;1;1
WireConnection;8;1;7;1
WireConnection;46;0;44;0
WireConnection;6;0;1;1
WireConnection;6;1;8;0
WireConnection;6;2;46;0
WireConnection;17;0;6;0
WireConnection;23;0;17;0
WireConnection;23;1;24;0
WireConnection;25;0;23;0
WireConnection;18;0;17;0
WireConnection;18;1;19;0
WireConnection;20;0;18;0
WireConnection;20;1;21;0
WireConnection;20;2;22;0
WireConnection;26;0;22;4
WireConnection;26;1;25;0
WireConnection;0;2;20;0
WireConnection;0;9;26;0
ASEEND*/
//CHKSM=5F0C1F4D374466800BBA28F36F16B3BB92A09CCF