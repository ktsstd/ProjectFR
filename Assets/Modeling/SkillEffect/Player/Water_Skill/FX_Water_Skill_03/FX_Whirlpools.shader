// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Water_Whirlpools"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Mask_Opacity("Mask_Opacity", Float) = 1
		_Main_Power("Main_Power", Float) = 1
		[HDR]_Color_A("Color_A", Color) = (0,0.4912767,1,0)
		[HDR]_Color_B("Color_B", Color) = (0,0.2810159,1,0)
		_Main_Opacity("Main_Opacity", Float) = 1
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		_Main_Upanner("Main_Upanner", Float) = 0
		_Main_Vpanner("Main_Vpanner", Float) = 0
		_Tum_UV("Tum_UV", Float) = -1
		_Mask_UPanner("Mask_UPanner", Float) = 0
		_Mask_VPanner("Mask_VPanner", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Front
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Color_A;
		uniform float4 _Color_B;
		uniform sampler2D _Main_Tex;
		uniform float _Main_Upanner;
		uniform float _Main_Vpanner;
		uniform float4 _Main_Tex_ST;
		uniform float _Tum_UV;
		uniform sampler2D _Mask_Tex;
		uniform float _Mask_UPanner;
		uniform float _Mask_VPanner;
		uniform float4 _Mask_Tex_ST;
		uniform float _Mask_Opacity;
		uniform float _Main_Power;
		uniform float _Main_Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult75 = (float2(_Main_Upanner , _Main_Vpanner));
			float2 uv0_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float2 appendResult86 = (float2(uv0_Main_Tex.x , ( uv0_Main_Tex.y + ( uv0_Main_Tex.x * _Tum_UV ) )));
			float2 panner77 = ( 1.0 * _Time.y * appendResult75 + appendResult86);
			float2 appendResult91 = (float2(_Mask_UPanner , _Mask_VPanner));
			float2 uv0_Mask_Tex = i.uv_texcoord * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
			float2 panner90 = ( 1.0 * _Time.y * appendResult91 + uv0_Mask_Tex);
			float4 temp_output_43_0 = saturate( ( tex2D( _Main_Tex, panner77 ) * ( tex2D( _Mask_Tex, panner90 ) * _Mask_Opacity ) ) );
			float4 lerpResult36 = lerp( _Color_A , _Color_B , temp_output_43_0);
			o.Emission = ( lerpResult36 * i.vertexColor ).rgb;
			float4 temp_cast_1 = (_Main_Power).xxxx;
			o.Alpha = saturate( ( saturate( pow( temp_output_43_0 , temp_cast_1 ) ) * i.vertexColor.a * _Main_Opacity ) ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
492.6667;714;1077;638;2233.913;820.17;1.172205;True;False
Node;AmplifyShaderEditor.RangedFloatNode;88;-1782.956,72.42191;Float;False;Property;_Tum_UV;Tum_UV;15;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;76;-1827.704,-193.8613;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;92;-1767.376,-525.9466;Float;False;Property;_Mask_UPanner;Mask_UPanner;16;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-1759.17,-378.2488;Float;False;Property;_Mask_VPanner;Mask_VPanner;17;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-1632.953,-10.42947;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-1402.718,181.22;Float;False;Property;_Main_Upanner;Main_Upanner;13;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;91;-1564.584,-473.1973;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;74;-1755.022,-672.788;Float;False;0;66;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;89;-1385.956,-99.57809;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-1366.49,317.6818;Float;False;Property;_Main_Vpanner;Main_Vpanner;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;75;-1232.207,195.8441;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;90;-1464.947,-597.4508;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;86;-1198.053,-213.9295;Float;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;77;-1010.309,15.8018;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;66;-1245.725,-673.2825;Float;True;Property;_Mask_Tex;Mask_Tex;12;0;Create;True;0;0;False;0;f38c1841d7ffa7240ba7ae5c416b4ade;f38c1841d7ffa7240ba7ae5c416b4ade;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-1100.164,-422.2211;Float;False;Property;_Mask_Opacity;Mask_Opacity;6;0;Create;True;0;0;False;0;1;13.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-810.26,-113.7678;Float;True;Property;_Main_Tex;Main_Tex;1;0;Create;True;0;0;False;0;c2f5e06ce5d539b418dc5ebfbfeeee94;c2f5e06ce5d539b418dc5ebfbfeeee94;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-882.2561,-654.7253;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-309.7802,-385.4064;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;43;-103.9467,-464.4682;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-165.1345,-46.01937;Float;False;Property;_Main_Power;Main_Power;7;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;44;14.86552,-80.01936;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;50;245.5161,185.5095;Float;False;Property;_Main_Opacity;Main_Opacity;10;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;41;-388.3456,-715.9915;Float;False;Property;_Color_B;Color_B;9;1;[HDR];Create;True;0;0;False;0;0,0.2810159,1,0;0,0.2810159,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;46;254.0917,-75.70144;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;49;238.3307,-351.3368;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;40;-414.5455,-937.0916;Float;False;Property;_Color_A;Color_A;8;1;[HDR];Create;True;0;0;False;0;0,0.4912767,1,0;0,0.4912767,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;448.5845,-70.23304;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;65;-3276.121,-973.0212;Float;False;1462.702;691.3134;Comment;10;20;22;21;27;23;26;34;33;28;35;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;36;-48.84554,-750.0914;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;84;-2550.627,-62.37299;Float;False;660.4165;565.5528;Comment;5;14;6;13;62;61;;1,1,1,1;0;0
Node;AmplifyShaderEditor.FunctionNode;61;-2315.211,248.1798;Float;False;RadialUVDistortion;-1;;4;051d65e7699b41a4c800363fd0e822b2;0;7;60;SAMPLER2D;_Sampler6061;False;1;FLOAT2;1,1;False;11;FLOAT2;0,0;False;65;FLOAT;1;False;68;FLOAT2;1,1;False;47;FLOAT2;1,1;False;29;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2224.619,-560.2076;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-3138.479,-534.2394;Float;False;Property;_Float0;Float 0;5;0;Create;True;0;0;False;0;-0.5;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;449.7845,-488.8844;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;35;-2011.419,-544.6077;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-2957.648,-657.3819;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;6;-2281.223,26.78487;Float;True;PolarCoordinates;-1;;2;d4cffec22897b1749b81b0aafe578606;0;0;0
Node;AmplifyShaderEditor.Vector2Node;62;-2500.627,218.1004;Float;False;Property;_Vector2;Vector 2;11;0;Create;True;0;0;False;0;0.5,0.5;1,-1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-3226.121,-674.0229;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-2189.22,-817.1251;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;26;-2489.486,-657.3819;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;51;686.9002,-62.89737;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-2638.144,-760.5556;Float;False;Property;_Mask_Power;Mask_Power;4;0;Create;True;0;0;False;0;4.72;5.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;23;-2702.489,-648.5069;Float;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-2387.119,-397.7079;Float;False;Constant;_Float4;Float 4;6;0;Create;True;0;0;False;0;51.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-2428.734,-12.373;Float;False;Property;_Length;Length;2;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-2462.109,95.74631;Float;False;Property;_Rotation;Rotation;3;0;Create;True;0;0;False;0;0.8;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;843.925,-535.3772;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Water_Whirlpools;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Front;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;87;0;76;1
WireConnection;87;1;88;0
WireConnection;91;0;92;0
WireConnection;91;1;93;0
WireConnection;89;0;76;2
WireConnection;89;1;87;0
WireConnection;75;0;78;0
WireConnection;75;1;79;0
WireConnection;90;0;74;0
WireConnection;90;2;91;0
WireConnection;86;0;76;1
WireConnection;86;1;89;0
WireConnection;77;0;86;0
WireConnection;77;2;75;0
WireConnection;66;1;90;0
WireConnection;1;1;77;0
WireConnection;85;0;66;0
WireConnection;85;1;29;0
WireConnection;30;0;1;0
WireConnection;30;1;85;0
WireConnection;43;0;30;0
WireConnection;44;0;43;0
WireConnection;44;1;45;0
WireConnection;46;0;44;0
WireConnection;47;0;46;0
WireConnection;47;1;49;4
WireConnection;47;2;50;0
WireConnection;36;0;40;0
WireConnection;36;1;41;0
WireConnection;36;2;43;0
WireConnection;61;68;62;0
WireConnection;33;0;26;0
WireConnection;33;1;34;0
WireConnection;48;0;36;0
WireConnection;48;1;49;0
WireConnection;35;0;33;0
WireConnection;21;0;20;0
WireConnection;21;1;22;0
WireConnection;28;0;26;0
WireConnection;26;0;23;0
WireConnection;26;1;27;0
WireConnection;51;0;47;0
WireConnection;23;0;21;0
WireConnection;0;2;48;0
WireConnection;0;9;51;0
ASEEND*/
//CHKSM=99C875630910CA4536688C87F2F8084A6E86B1DD