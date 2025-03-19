// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Boss_Skill_04"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Distortion("Distortion", Range( 0 , 1)) = 0.06
		_Distortion_Tiling("Distortion_Tiling", Vector) = (2,0.5,0,0)
		_Distortion_Speed("Distortion_Speed", Vector) = (0,-0.5,0,0)
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_Noise_Tiling("Noise_Tiling", Vector) = (1,1,0,0)
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
		_Main_Power("Main_Power", Float) = 1
		_Depth_Fade("Depth_Fade", Range( -1 , 1)) = 0
		_Noise_Panner_Speed("Noise_Panner_Speed", Vector) = (0,-0.5,0,0)
		_Desaturate("Desaturate", Float) = 0
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord4( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
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
			float4 uv_tex4coord;
			float4 vertexColor : COLOR;
			float4 uv4_tex4coord4;
			float4 uv2_tex4coord2;
			float4 screenPos;
		};

		uniform sampler2D _Main_Tex;
		uniform sampler2D _Normal_Tex;
		uniform sampler2D _Sampler601;
		uniform float2 _Distortion_Tiling;
		uniform float2 _Distortion_Speed;
		uniform float _Distortion;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _Desaturate;
		uniform float _Main_Power;
		uniform float4 _Main_Color;
		uniform sampler2D _Noise_Tex;
		uniform sampler2D _Sampler6034;
		uniform float2 _Noise_Tiling;
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
			float2 temp_output_1_0_g1 = float2( 1,1 );
			float2 appendResult10_g1 = (float2(( (temp_output_1_0_g1).x * i.uv_texcoord.x ) , ( i.uv_texcoord.y * (temp_output_1_0_g1).y )));
			float2 temp_output_11_0_g1 = float2( 0,0 );
			float2 panner18_g1 = ( ( (temp_output_11_0_g1).x * _Time.y ) * float2( 1,0 ) + i.uv_texcoord);
			float2 panner19_g1 = ( ( _Time.y * (temp_output_11_0_g1).y ) * float2( 0,1 ) + i.uv_texcoord);
			float2 appendResult24_g1 = (float2((panner18_g1).x , (panner19_g1).y));
			float2 temp_output_47_0_g1 = _Distortion_Speed;
			float2 uv_TexCoord78_g1 = i.uv_texcoord * float2( 2,2 );
			float2 temp_output_31_0_g1 = ( uv_TexCoord78_g1 - float2( 1,1 ) );
			float2 appendResult39_g1 = (float2(frac( ( atan2( (temp_output_31_0_g1).x , (temp_output_31_0_g1).y ) / 6.28318548202515 ) ) , length( temp_output_31_0_g1 )));
			float2 panner54_g1 = ( ( (temp_output_47_0_g1).x * _Time.y ) * float2( 1,0 ) + appendResult39_g1);
			float2 panner55_g1 = ( ( _Time.y * (temp_output_47_0_g1).y ) * float2( 0,1 ) + appendResult39_g1);
			float2 appendResult58_g1 = (float2((panner54_g1).x , (panner55_g1).y));
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float2 Distortion12 = ( (UnpackNormal( tex2D( _Normal_Tex, ( ( (tex2D( _Sampler601, ( appendResult10_g1 + appendResult24_g1 ) )).rg * 1.0 ) + ( _Distortion_Tiling * appendResult58_g1 ) ) ) )).xy * _Distortion * ( 1.0 - ( tex2D( _TextureSample1, uv_TextureSample1 ).r + 0.3 ) ) );
			float temp_output_4_0_g2 = 2.0;
			float temp_output_5_0_g2 = 2.0;
			float2 appendResult7_g2 = (float2(temp_output_4_0_g2 , temp_output_5_0_g2));
			float totalFrames39_g2 = ( temp_output_4_0_g2 * temp_output_5_0_g2 );
			float2 appendResult8_g2 = (float2(totalFrames39_g2 , temp_output_5_0_g2));
			float clampResult42_g2 = clamp( i.uv_tex4coord.x , 0.0001 , ( totalFrames39_g2 - 1.0 ) );
			float temp_output_35_0_g2 = frac( ( ( 1.0 + clampResult42_g2 ) / totalFrames39_g2 ) );
			float2 appendResult29_g2 = (float2(temp_output_35_0_g2 , ( 1.0 - temp_output_35_0_g2 )));
			float2 temp_output_15_0_g2 = ( ( i.uv_texcoord / appendResult7_g2 ) + ( floor( ( appendResult8_g2 * appendResult29_g2 ) ) / appendResult7_g2 ) );
			float3 desaturateInitialColor22 = tex2D( _Main_Tex, ( Distortion12 + temp_output_15_0_g2 ) ).rgb;
			float desaturateDot22 = dot( desaturateInitialColor22, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar22 = lerp( desaturateInitialColor22, desaturateDot22.xxx, _Desaturate );
			float3 temp_cast_1 = (_Main_Power).xxx;
			float4 appendResult30 = (float4(i.uv4_tex4coord4.x , i.uv4_tex4coord4.y , i.uv4_tex4coord4.z , i.uv4_tex4coord4.w));
			o.Emission = ( ( float4( pow( desaturateVar22 , temp_cast_1 ) , 0.0 ) * _Main_Color ) * i.vertexColor * appendResult30 ).rgb;
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
			float screenDepth60 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth60 = abs( ( screenDepth60 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth_Fade ) );
			o.Alpha = ( i.vertexColor.a * saturate( ( desaturateVar22 * ( ( ( 1.0 - ( length( ( i.uv_texcoord + -0.5 ) ) * 1.0 ) ) + pow( tex2D( _Noise_Tex, ( Distortion12 + ( ( (tex2D( _Sampler6034, ( appendResult10_g3 + appendResult24_g3 ) )).rg * 1.0 ) + ( _Noise_Tiling * appendResult58_g3 ) ) ) ).r , 2.4 ) ) + (-2.0 + (i.uv2_tex4coord2.y - 0.0) * (1.0 - -2.0) / (1.0 - 0.0)) ) ) ) * saturate( distanceDepth60 ) ).x;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
802;797.3334;713;562;1782.432;-348.5264;3.647973;True;False
Node;AmplifyShaderEditor.CommentaryNode;13;-1749.496,-955.5348;Float;False;1825.651;793.3546;;12;2;3;1;4;5;6;8;7;9;10;11;12;Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;3;-1699.496,-397.3713;Float;False;Property;_Distortion_Speed;Distortion_Speed;5;0;Create;True;0;0;False;0;0,-0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;2;-1695.792,-547.3235;Float;False;Property;_Distortion_Tiling;Distortion_Tiling;4;0;Create;True;0;0;False;0;2,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;1;-1436.083,-521.2697;Float;False;RadialUVDistortion;-1;;1;051d65e7699b41a4c800363fd0e822b2;0;7;60;SAMPLER2D;_Sampler601;False;1;FLOAT2;1,1;False;11;FLOAT2;0,0;False;65;FLOAT;1;False;68;FLOAT2;1,1;False;47;FLOAT2;1,1;False;29;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-954.8151,-688.4348;Float;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;False;0;0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-1054.915,-905.5348;Float;True;Property;_TextureSample1;Texture Sample 1;6;0;Create;True;0;0;False;0;7fd87b4f6e1f125408184ccfb028d141;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-737.7151,-840.5344;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-983.4153,-519.4348;Float;True;Property;_Normal_Tex;Normal_Tex;2;0;Create;True;0;0;False;0;44116b33e8fe54b4390df94f441c325f;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-697.4452,-277.1803;Float;False;Property;_Distortion;Distortion;3;0;Create;True;0;0;False;0;0.06;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;9;-519.3154,-808.0347;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;5;-692.2152,-519.4346;Float;True;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;54;-1777.717,606.5085;Float;False;2129.462;895.2944;;21;34;33;32;35;36;37;38;39;40;42;43;44;45;48;47;46;51;50;52;53;49;Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-287.8452,-513.9802;Float;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;33;-1724.013,1081.261;Float;False;Property;_Noise_Tiling;Noise_Tiling;8;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;32;-1727.717,1231.213;Float;False;Property;_Noise_Panner_Speed;Noise_Panner_Speed;12;0;Create;True;0;0;False;0;0,-0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;38;-1300.117,717.6086;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-1231.217,858.0085;Float;False;Constant;_Float3;Float 3;13;0;Create;True;0;0;False;0;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;12;-151.8452,-520.3802;Float;True;Distortion;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-1066.117,720.2086;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;36;-1106.417,1001.008;Float;False;12;Distortion;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;34;-1464.304,1107.315;Float;False;RadialUVDistortion;-1;;3;051d65e7699b41a4c800363fd0e822b2;0;7;60;SAMPLER2D;_Sampler6034;False;1;FLOAT2;1,1;False;11;FLOAT2;0,0;False;65;FLOAT;1;False;68;FLOAT2;1,1;False;47;FLOAT2;1,1;False;29;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;31;-1766.893,-126.917;Float;False;2168.113;649.3542;;14;18;17;16;14;15;20;19;21;23;25;22;26;24;27;Main;1,1,1,1;0;0
Node;AmplifyShaderEditor.LengthOpNode;42;-841.2166,731.9084;Float;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-910.1165,1140.109;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-774.9165,656.5085;Float;False;Constant;_Float4;Float 4;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;17;-1716.893,249.6512;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-1515.537,334.2545;Float;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1595.064,119.3623;Float;False;Constant;_Float1;Float 1;6;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1689.447,-22.39481;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;-704.7167,1112.809;Float;True;Property;_Noise_Tex;Noise_Tex;7;0;Create;True;0;0;False;0;4f599298d22bf8047b38f3bd26dad4c7;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-612.4164,728.0085;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;-1014.686,-76.91705;Float;False;12;Distortion;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-469.4167,963.3085;Float;False;Constant;_Float5;Float 5;13;0;Create;True;0;0;False;0;2.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;14;-1336.12,62.95491;Float;True;Flipbook;-1;;2;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;3;False;5;FLOAT;3;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.RangedFloatNode;53;-274.0188,1386.803;Float;False;Constant;_Float7;Float 7;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-283.1189,1291.904;Float;False;Constant;_Float6;Float 6;13;0;Create;True;0;0;False;0;-2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;50;-300.019,1112.504;Float;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;47;-301.7168,986.7085;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;45;-399.2169,730.6085;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-774.0269,59.21909;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;21;-640.5294,44.61343;Float;True;Property;_Main_Tex;Main_Tex;1;0;Create;True;0;0;False;0;9cea6021695b50c4a8c9dec36550cb95;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;-515.2839,261.4679;Float;False;Property;_Desaturate;Desaturate;13;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-134.0167,725.4087;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;51;-38.71888,1137.204;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;421.9163,859.6749;Float;False;Property;_Depth_Fade;Depth_Fade;11;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;49;116.7453,741.1008;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-256.2314,-39.56049;Float;False;Property;_Main_Power;Main_Power;10;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;22;-334.1878,50.38832;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;405.9097,623.4289;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;29;101.4821,-314.4352;Float;False;3;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;60;729.8353,854.5214;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;24;-87.12785,55.18556;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;27;-52.34752,315.437;Float;False;Property;_Main_Color;Main_Color;9;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;237.887,73.17525;Float;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;57;658.4144,394.0877;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;30;329.4819,-307.22;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;56;633.2552,621.8846;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;61;1014.275,836.9136;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;870.8252,150.6531;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;969.5447,556.6981;Float;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1155.264,-130.8569;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Boss_Skill_04;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;68;2;0
WireConnection;1;47;3;0
WireConnection;7;0;6;1
WireConnection;7;1;8;0
WireConnection;4;1;1;0
WireConnection;9;0;7;0
WireConnection;5;0;4;0
WireConnection;10;0;5;0
WireConnection;10;1;11;0
WireConnection;10;2;9;0
WireConnection;12;0;10;0
WireConnection;40;0;38;0
WireConnection;40;1;39;0
WireConnection;34;68;33;0
WireConnection;34;47;32;0
WireConnection;42;0;40;0
WireConnection;35;0;36;0
WireConnection;35;1;34;0
WireConnection;37;1;35;0
WireConnection;43;0;42;0
WireConnection;43;1;44;0
WireConnection;14;13;15;0
WireConnection;14;4;16;0
WireConnection;14;5;16;0
WireConnection;14;24;17;1
WireConnection;14;2;18;0
WireConnection;47;0;37;1
WireConnection;47;1;48;0
WireConnection;45;0;43;0
WireConnection;19;0;20;0
WireConnection;19;1;14;0
WireConnection;21;1;19;0
WireConnection;46;0;45;0
WireConnection;46;1;47;0
WireConnection;51;0;50;2
WireConnection;51;3;52;0
WireConnection;51;4;53;0
WireConnection;49;0;46;0
WireConnection;49;1;51;0
WireConnection;22;0;21;0
WireConnection;22;1;23;0
WireConnection;55;0;22;0
WireConnection;55;1;49;0
WireConnection;60;0;59;0
WireConnection;24;0;22;0
WireConnection;24;1;25;0
WireConnection;26;0;24;0
WireConnection;26;1;27;0
WireConnection;30;0;29;1
WireConnection;30;1;29;2
WireConnection;30;2;29;3
WireConnection;30;3;29;4
WireConnection;56;0;55;0
WireConnection;61;0;60;0
WireConnection;28;0;26;0
WireConnection;28;1;57;0
WireConnection;28;2;30;0
WireConnection;58;0;57;4
WireConnection;58;1;56;0
WireConnection;58;2;61;0
WireConnection;0;2;28;0
WireConnection;0;9;58;0
ASEEND*/
//CHKSM=632C7FFA5E53CF86BA515074D9C20C0D79556F72