// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Fire_Skill_03_Fire"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Distortion("Distortion", Range( 0 , 1)) = 0.147385
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Desaturate("Desaturate", Float) = 0
		_Main_Power("Main_Power", Float) = 1
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_Noise_Tilling("Noise_Tilling", Vector) = (1,1,0,0)
		_Noise_Panner_Speed("Noise_Panner_Speed", Vector) = (0,-0.5,0,0)
		_Depth_Fade("Depth_Fade", Range( -1 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
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
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 uv_tex4coord;
			float4 screenPos;
		};

		uniform sampler2D _Main_Tex;
		uniform float4 _Main_Tex_ST;
		uniform float _Desaturate;
		uniform float _Main_Power;
		uniform float4 _Main_Color;
		uniform sampler2D _Noise_Tex;
		uniform sampler2D _Normal_Tex;
		uniform float4 _Normal_Tex_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Distortion;
		uniform sampler2D _Sampler6084;
		uniform float2 _Noise_Tilling;
		uniform float2 _Noise_Panner_Speed;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth_Fade;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv0_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float3 desaturateInitialColor59 = tex2D( _Main_Tex, uv0_Main_Tex ).rgb;
			float desaturateDot59 = dot( desaturateInitialColor59, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar59 = lerp( desaturateInitialColor59, desaturateDot59.xxx, _Desaturate );
			float3 temp_cast_1 = (_Main_Power).xxx;
			float4 appendResult66 = (float4(i.uv_tex4coord.x , i.uv_tex4coord.y , i.uv_tex4coord.z , i.uv_tex4coord.w));
			o.Emission = ( ( float4( pow( desaturateVar59 , temp_cast_1 ) , 0.0 ) * _Main_Color ) * i.vertexColor * appendResult66 ).rgb;
			float2 uv_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float2 Distortion50 = ( (UnpackNormal( tex2D( _Normal_Tex, uv_Normal_Tex ) )).xy * ( 1.0 - ( tex2D( _TextureSample0, uv_TextureSample0 ).r + 0.3 ) ) * _Distortion );
			float2 temp_output_1_0_g3 = float2( 1,1 );
			float2 appendResult10_g3 = (float2(( (temp_output_1_0_g3).x * i.uv_texcoord.x ) , ( i.uv_texcoord.y * (temp_output_1_0_g3).y )));
			float2 temp_output_11_0_g3 = float2( 0,0 );
			float2 panner18_g3 = ( ( (temp_output_11_0_g3).x * _Time.y ) * float2( 1,0 ) + i.uv_texcoord);
			float2 panner19_g3 = ( ( _Time.y * (temp_output_11_0_g3).y ) * float2( 0,1 ) + i.uv_texcoord);
			float2 appendResult24_g3 = (float2((panner18_g3).x , (panner19_g3).y));
			float2 temp_output_47_0_g3 = _Noise_Panner_Speed;
			float2 uv_TexCoord78_g3 = i.uv_texcoord * float2( 2,2 );
			float2 temp_output_31_0_g3 = ( uv_TexCoord78_g3 - float2( 1,1 ) );
			float2 appendResult39_g3 = (float2(frac( ( atan2( (temp_output_31_0_g3).x , (temp_output_31_0_g3).y ) / 6.28318548202515 ) ) , length( temp_output_31_0_g3 )));
			float2 panner54_g3 = ( ( (temp_output_47_0_g3).x * _Time.y ) * float2( 1,0 ) + appendResult39_g3);
			float2 panner55_g3 = ( ( _Time.y * (temp_output_47_0_g3).y ) * float2( 0,1 ) + appendResult39_g3);
			float2 appendResult58_g3 = (float2((panner54_g3).x , (panner55_g3).y));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth97 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth97 = abs( ( screenDepth97 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth_Fade ) );
			o.Alpha = ( i.vertexColor.a * saturate( ( desaturateVar59 * ( ( ( 1.0 - ( length( ( i.uv_texcoord + -0.5 ) ) * 1.0 ) ) + pow( tex2D( _Noise_Tex, ( Distortion50 + ( ( (tex2D( _Sampler6084, ( appendResult10_g3 + appendResult24_g3 ) )).rg * 1.0 ) + ( _Noise_Tilling * appendResult58_g3 ) ) ) ).r , 2.4 ) ) + (-2.0 + (i.uv_tex4coord.y - 0.0) * (1.0 - -2.0) / (1.0 - 0.0)) ) ) ) * saturate( distanceDepth97 ) ).x;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
0;733;1295;618;1552.153;-1438.081;2.062078;True;False
Node;AmplifyShaderEditor.CommentaryNode;95;-1730.753,264.9988;Float;False;1206.799;724.8478;Comment;9;43;41;47;42;44;45;48;50;49;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;41;-1680.753,314.9988;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;e433b9860a5b28a46beb453662ba4e4b;6ec6609a20135d9498251e41c996402d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-1515.914,555.835;Float;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-1315.393,433.4654;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;47;-1646.409,756.9524;Float;True;Property;_Normal_Tex;Normal_Tex;2;0;Create;True;0;0;False;0;51fe2c9d5b236124d9f9e7ea528b0bea;6f2f1be8652ee4e44b1e28c533f88c22;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;45;-1354.18,758.2808;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;44;-1109.73,426.2671;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1290.299,870.6299;Float;False;Property;_Distortion;Distortion;3;0;Create;True;0;0;False;0;0.147385;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-972.9542,731.7147;Float;True;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;94;-2371.468,1706.734;Float;False;2051.032;1010.783;Comment;21;87;88;78;89;90;80;91;77;79;74;81;73;82;75;84;71;83;85;70;72;86;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;85;-2277.268,2288.297;Float;False;Property;_Noise_Tilling;Noise_Tilling;9;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-2049.983,1848.685;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;72;-1989.243,1982.734;Float;False;Constant;_Float3;Float 3;8;0;Create;True;0;0;False;0;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;86;-2321.468,2471.597;Float;False;Property;_Noise_Panner_Speed;Noise_Panner_Speed;10;0;Create;True;0;0;False;0;0,-0.5;0,-0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;50;-747.954,730.8469;Float;True;Distortion;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;-1744.097,2113.864;Float;False;50;Distortion;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-1800.243,1847.734;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;84;-2042.097,2290.864;Float;True;RadialUVDistortion;-1;;3;051d65e7699b41a4c800363fd0e822b2;0;7;60;SAMPLER2D;_Sampler6084;False;1;FLOAT2;1,1;False;11;FLOAT2;0,0;False;65;FLOAT;1;False;68;FLOAT2;1,1;False;47;FLOAT2;1,1;False;29;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-1492.243,1756.734;Float;False;Constant;_Float4;Float 4;8;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-1572.097,2199.864;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LengthOpNode;73;-1559.243,1848.734;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-1195.243,2064.734;Float;False;Constant;_Float5;Float 5;9;0;Create;True;0;0;False;0;2.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-1368.243,1857.734;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;79;-1414.243,2157.734;Float;True;Property;_Noise_Tex;Noise_Tex;8;0;Create;True;0;0;False;0;6e5343f0266cf36489aa21b41e5bc1f7;49bfacb9ca221074b9b8d450d7444651;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;89;-1057.433,2528.827;Float;False;Constant;_Float6;Float 6;11;0;Create;True;0;0;False;0;-2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;55;-2195.583,1217.513;Float;False;0;51;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;90;-1057.866,2605.568;Float;False;Constant;_Float7;Float 7;11;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;77;-1160.243,1849.734;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;80;-1005.243,2078.734;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;91;-1077.952,2227.337;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;78;-814.2435,1944.734;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;88;-840.9989,2198.373;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-1057.054,1525.852;Float;False;Property;_Desaturate;Desaturate;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;51;-1165.291,1300.885;Float;True;Property;_Main_Tex;Main_Tex;4;0;Create;True;0;0;False;0;08f28e42a647e2a4cbd32e793359aa3c;01443f889e6f7d643a35ba5220dca7c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;87;-569.299,1939.672;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-672.4239,1228.037;Float;False;Property;_Main_Power;Main_Power;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;59;-855.1541,1307.552;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-287.5302,2177.169;Float;False;Property;_Depth_Fade;Depth_Fade;11;0;Create;True;0;0;False;0;0;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;61;-478.7238,1291.737;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;64;-456.6237,1437.337;Float;False;Property;_Main_Color;Main_Color;7;1;[HDR];Create;True;0;0;False;0;1,1,1,0;7.046587,3.316847,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-197.63,1842.751;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;65;-170.6236,1016.137;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;97;5.391235,2151.915;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-209.6238,1311.237;Float;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;93;109.5157,1744.1;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VertexColorNode;68;36.21248,1444.177;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;66;81.57641,1034.336;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;98;229.3912,2134.915;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-2118.38,1574.743;Float;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-1427.238,1288.169;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;58;-2409.58,1466.843;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;52;-1660.001,1214.466;Float;False;50;Distortion;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-2184.783,1356.513;Float;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;54;-1933.946,1318;Float;False;Flipbook;-1;;4;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;3;False;5;FLOAT;3;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;296.0312,1299.124;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;326.2155,1619.987;Float;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;607.5537,1309.553;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Fire_Skill_03_Fire;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;42;0;41;1
WireConnection;42;1;43;0
WireConnection;45;0;47;0
WireConnection;44;0;42;0
WireConnection;48;0;45;0
WireConnection;48;1;44;0
WireConnection;48;2;49;0
WireConnection;50;0;48;0
WireConnection;71;0;70;0
WireConnection;71;1;72;0
WireConnection;84;68;85;0
WireConnection;84;47;86;0
WireConnection;82;0;83;0
WireConnection;82;1;84;0
WireConnection;73;0;71;0
WireConnection;74;0;73;0
WireConnection;74;1;75;0
WireConnection;79;1;82;0
WireConnection;77;0;74;0
WireConnection;80;0;79;1
WireConnection;80;1;81;0
WireConnection;78;0;77;0
WireConnection;78;1;80;0
WireConnection;88;0;91;2
WireConnection;88;3;89;0
WireConnection;88;4;90;0
WireConnection;51;1;55;0
WireConnection;87;0;78;0
WireConnection;87;1;88;0
WireConnection;59;0;51;0
WireConnection;59;1;60;0
WireConnection;61;0;59;0
WireConnection;61;1;62;0
WireConnection;92;0;59;0
WireConnection;92;1;87;0
WireConnection;97;0;96;0
WireConnection;63;0;61;0
WireConnection;63;1;64;0
WireConnection;93;0;92;0
WireConnection;66;0;65;1
WireConnection;66;1;65;2
WireConnection;66;2;65;3
WireConnection;66;3;65;4
WireConnection;98;0;97;0
WireConnection;53;0;52;0
WireConnection;53;1;54;0
WireConnection;54;4;56;0
WireConnection;54;5;56;0
WireConnection;54;24;58;1
WireConnection;54;2;57;0
WireConnection;67;0;63;0
WireConnection;67;1;68;0
WireConnection;67;2;66;0
WireConnection;69;0;68;4
WireConnection;69;1;93;0
WireConnection;69;2;98;0
WireConnection;0;2;67;0
WireConnection;0;9;69;0
ASEEND*/
//CHKSM=C519FF74AAC85816AFD887A9436A715F681FF044