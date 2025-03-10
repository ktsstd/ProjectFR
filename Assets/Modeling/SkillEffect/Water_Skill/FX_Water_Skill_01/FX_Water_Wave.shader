// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Water_Wave"
{
	Properties
	{
		_Fresnel_Scale("Fresnel_Scale", Range( 0 , 1)) = 1
		_Fresnel_Power("Fresnel_Power", Range( 0 , 10)) = 1.9163
		[HDR]_Fresnel_OutColor("Fresnel_OutColor", Color) = (1,1,1,0)
		[HDR]_Fresnel_Incolor("Fresnel_Incolor", Color) = (0,0.4782095,1,0)
		[HDR]_Base_Color("Base_Color", Color) = (0,0.4184885,1,0)
		_Noise_Texture_A("Noise_Texture_A", 2D) = "white" {}
		_NoiseA_VPanner("NoiseA_VPanner", Float) = 0.5
		_Step_A("Step_A", Range( 0 , 1)) = 0.6378281
		_A_Range("A_Range", Float) = 0.48
		_A_Opacity("A_Opacity", Range( 0 , 1)) = 1
		_Noise_Texture_B("Noise_Texture_B", 2D) = "white" {}
		_Noise_B_VPanner("Noise_B_VPanner", Float) = 0.5
		_Step_B("Step_B", Range( 0 , 1)) = 0.5133145
		_B_Range("B_Range", Float) = 0.41
		_B_Opacity("B_Opacity", Range( 0 , 1)) = 0.5
		_vertex_Tex("vertex_Tex", 2D) = "bump" {}
		_Vertex_UPanner("Vertex_UPanner", Float) = 0
		_Vertex_VPanner("Vertex_VPanner", Float) = 0
		_Vertex_Str("Vertex_Str", Range( 0 , 1)) = 0.4094087
		_shadow_Range("shadow_Range", Float) = 0.99
		_Shadow_Offset("Shadow_Offset", Float) = 0
		_Dissovle_Tex("Dissovle_Tex", 2D) = "white" {}
		_Dissolve("Dissolve", Range( -1 , 1)) = 1
		_Down_Range("Down_Range", Range( 0 , 1.5)) = 1.023529
		[Toggle(_USE_DISSOLVE_ON)] _USE_Dissolve("USE_Dissolve", Float) = 0
		_Vertex_2_Tex("Vertex_2_Tex", 2D) = "white" {}
		_Vertex2_Upanner("Vertex2_Upanner", Float) = 0
		_Vertex2_Vpanner("Vertex2_Vpanner", Float) = 0
		_Vertex_2_Str("Vertex_2_Str", Float) = 0
		[Toggle(_USE_VERTEX_ON)] _USE_Vertex("USE_Vertex", Float) = 0
		_Depth_Fade("Depth_Fade", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
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
		#pragma shader_feature _USE_VERTEX_ON
		#pragma shader_feature _USE_DISSOLVE_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float4 screenPos;
		};

		uniform sampler2D _Vertex_2_Tex;
		uniform float _Vertex2_Upanner;
		uniform float _Vertex2_Vpanner;
		uniform float _Vertex_2_Str;
		uniform float4 _Fresnel_Incolor;
		uniform float4 _Fresnel_OutColor;
		uniform float _Fresnel_Scale;
		uniform float _Fresnel_Power;
		uniform float4 _Base_Color;
		uniform sampler2D _vertex_Tex;
		uniform float _Vertex_UPanner;
		uniform float _Vertex_VPanner;
		uniform float4 _vertex_Tex_ST;
		uniform float _Vertex_Str;
		uniform float _Step_A;
		uniform float _Step_B;
		uniform float _A_Range;
		uniform sampler2D _Noise_Texture_A;
		uniform float _NoiseA_VPanner;
		uniform float4 _Noise_Texture_A_ST;
		uniform float _A_Opacity;
		uniform float _B_Range;
		uniform sampler2D _Noise_Texture_B;
		uniform float _Noise_B_VPanner;
		uniform float _B_Opacity;
		uniform float _shadow_Range;
		uniform float _Shadow_Offset;
		uniform float _Down_Range;
		uniform sampler2D _Dissovle_Tex;
		uniform float4 _Dissovle_Tex_ST;
		uniform float _Dissolve;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth_Fade;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float2 appendResult123 = (float2(_Vertex2_Upanner , _Vertex2_Vpanner));
			float2 panner122 = ( 1.0 * _Time.y * appendResult123 + v.texcoord.xy);
			#ifdef _USE_VERTEX_ON
				float staticSwitch173 = v.texcoord.w;
			#else
				float staticSwitch173 = _Vertex_2_Str;
			#endif
			v.vertex.xyz += ( ase_vertexNormal * ( saturate( (0.0 + (( tex2Dlod( _Vertex_2_Tex, float4( panner122, 0, 0.0) ).r + 0.2 ) - 0.0) * (1.0 - 0.0) / (1.0 - 0.0)) ) * staticSwitch173 ) );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV158 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode158 = ( 0.0 + _Fresnel_Scale * pow( 1.0 - fresnelNdotV158, _Fresnel_Power ) );
			float4 lerpResult170 = lerp( _Fresnel_Incolor , _Fresnel_OutColor , max( saturate( fresnelNode158 ) , 0.0 ));
			float2 appendResult65 = (float2(_Vertex_UPanner , _Vertex_VPanner));
			float2 uv0_vertex_Tex = i.uv_texcoord * _vertex_Tex_ST.xy + _vertex_Tex_ST.zw;
			float2 panner63 = ( 1.0 * _Time.y * appendResult65 + uv0_vertex_Tex);
			float temp_output_82_0 = (( ( (UnpackNormal( tex2D( _vertex_Tex, panner63 ) )).xy * _Vertex_Str ) + i.uv_texcoord )).y;
			float temp_output_19_0 = step( temp_output_82_0 , _Step_A );
			float4 lerpResult23 = lerp( float4( 0,0,0,0 ) , _Base_Color , temp_output_19_0);
			float2 appendResult100 = (float2(0.0 , _NoiseA_VPanner));
			float2 uv0_Noise_Texture_A = i.uv_texcoord * _Noise_Texture_A_ST.xy + _Noise_Texture_A_ST.zw;
			float2 panner99 = ( 1.0 * _Time.y * appendResult100 + uv0_Noise_Texture_A);
			float2 appendResult108 = (float2(0.0 , _Noise_B_VPanner));
			float2 panner109 = ( 1.0 * _Time.y * appendResult108 + uv0_Noise_Texture_A);
			float4 color53 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 color54 = IsGammaSpace() ? float4(0.5943396,0.5943396,0.5943396,0) : float4(0.3119799,0.3119799,0.3119799,0);
			float temp_output_83_0 = ( temp_output_82_0 - _Shadow_Offset );
			float4 lerpResult52 = lerp( color53 , color54 , step( _shadow_Range , ( ( temp_output_83_0 * ( 1.0 - temp_output_83_0 ) ) * 4.0 ) ));
			o.Emission = ( lerpResult170 + float4( 0,0,0,0 ) + ( ( ( lerpResult23 + ( 1.0 - step( temp_output_82_0 , _Step_B ) ) ) + saturate( ( ( step( _A_Range , tex2D( _Noise_Texture_A, panner99 ).r ) * _A_Opacity ) + ( step( _B_Range , tex2D( _Noise_Texture_B, panner109 ).r ) * _B_Opacity ) ) ) ) * lerpResult52 ) ).rgb;
			#ifdef _USE_DISSOLVE_ON
				float staticSwitch92 = i.uv_tex4coord.z;
			#else
				float staticSwitch92 = _Down_Range;
			#endif
			float2 uv0_Dissovle_Tex = i.uv_texcoord * _Dissovle_Tex_ST.xy + _Dissovle_Tex_ST.zw;
			float2 panner88 = ( 1.0 * _Time.y * float2( 0,0.15 ) + uv0_Dissovle_Tex);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth176 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth176 = abs( ( screenDepth176 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth_Fade ) );
			o.Alpha = ( ( temp_output_19_0 * step( temp_output_82_0 , staticSwitch92 ) ) * step( 0.5 , ( tex2D( _Dissovle_Tex, panner88 ).r + _Dissolve ) ) * saturate( distanceDepth176 ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
794;439;1221;680;1931.201;450.1134;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;72;-4414.175,-1123.983;Float;False;3234.897;714.1703;Ocean_Vertex;22;48;76;22;92;93;77;20;19;18;21;82;73;74;60;61;59;58;63;62;65;67;66;;0,0.6727629,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-4394.977,-917.7787;Float;False;Property;_Vertex_UPanner;Vertex_UPanner;17;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-4400.977,-835.779;Float;False;Property;_Vertex_VPanner;Vertex_VPanner;18;0;Create;True;0;0;False;0;0;-0.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;65;-4213.977,-893.7787;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;62;-4379.977,-1070.779;Float;False;0;58;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;63;-4039.976,-1034.779;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;58;-3845.023,-1044.264;Float;True;Property;_vertex_Tex;vertex_Tex;16;0;Create;True;0;0;False;0;51fe2c9d5b236124d9f9e7ea528b0bea;51fe2c9d5b236124d9f9e7ea528b0bea;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;59;-3541.708,-1041.774;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-3560.978,-852.1601;Float;False;Property;_Vertex_Str;Vertex_Str;19;0;Create;True;0;0;False;0;0.4094087;0.237;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;75;-2632.368,-2440.525;Float;False;1880.682;1211.031;Main_Base;24;106;105;117;114;115;118;103;111;97;104;110;112;109;99;108;100;107;98;102;113;24;23;26;27;;0.02426267,1,0,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-3295.307,-1061.774;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;74;-3349.653,-775.7958;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;102;-2630.865,-2083.089;Float;False;Property;_NoiseA_VPanner;NoiseA_VPanner;7;0;Create;True;0;0;False;0;0.5;-0.85;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-2580.561,-1595.182;Float;False;Property;_Noise_B_VPanner;Noise_B_VPanner;12;0;Create;True;0;0;False;0;0.5;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;149;-2707.024,769.3416;Float;False;2612.957;571.1505;Vertex;18;148;147;123;121;122;119;127;129;130;126;128;135;138;136;142;140;173;174;;0.4095531,0,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;98;-2622.346,-2340.791;Float;False;0;97;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;107;-2572.042,-1852.885;Float;False;0;97;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;108;-2394.928,-1633.195;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;100;-2445.234,-2121.101;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;150;-2393.98,118.1531;Float;False;2039.006;619.7252;Shadow;11;84;81;49;80;51;79;53;54;52;83;85;;0.9735079,0.759434,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;73;-3074.554,-1007.496;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;99;-2352.877,-2261.951;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;148;-2655.024,1105.72;Float;False;Property;_Vertex2_Vpanner;Vertex2_Vpanner;28;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;147;-2657.024,1004.72;Float;False;Property;_Vertex2_Upanner;Vertex2_Upanner;27;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;109;-2302.572,-1774.044;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-2343.98,452.7276;Float;False;Property;_Shadow_Offset;Shadow_Offset;21;0;Create;True;0;0;False;0;0;0.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;82;-2847.692,-1010.141;Float;True;False;True;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;121;-2599.313,851.3223;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;83;-2140.213,255.7323;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;123;-2421.313,1035.322;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;104;-1959.125,-2389.141;Float;False;Property;_A_Range;A_Range;9;0;Create;True;0;0;False;0;0.48;0.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;97;-2154.222,-2298.514;Float;True;Property;_Noise_Texture_A;Noise_Texture_A;6;0;Create;True;0;0;False;0;c2f5e06ce5d539b418dc5ebfbfeeee94;c4187630d55cc6344b665b2f0f38fe73;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;110;-2103.916,-1810.608;Float;True;Property;_Noise_Texture_B;Noise_Texture_B;11;0;Create;True;0;0;False;0;c10244bcb5987bd41b64e7758c3af36f;c10244bcb5987bd41b64e7758c3af36f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;112;-1935.82,-1948.234;Float;False;Property;_B_Range;B_Range;14;0;Create;True;0;0;False;0;0.41;0.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;84;-1972.443,443.0513;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;115;-1838.85,-1592.015;Float;False;Property;_B_Opacity;B_Opacity;15;0;Create;True;0;0;False;0;0.5;0.134;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;171;-2695.597,-3678.073;Float;False;2019.304;1201.11;Comment;8;160;159;158;162;168;169;163;170;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;120;-1982.11,-367.9105;Float;False;1351;447;Dissovle;8;87;89;88;86;91;96;90;95;;1,0,0,1;0;0
Node;AmplifyShaderEditor.StepOpNode;111;-1792.719,-1834.494;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;103;-1785.024,-2274.4;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;122;-2235.313,918.3222;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1956.714,-1025.751;Float;False;Property;_Step_B;Step_B;13;0;Create;True;0;0;False;0;0.5133145;0.641;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-2531.244,-1050.42;Float;False;Property;_Step_A;Step_A;8;0;Create;True;0;0;False;0;0.6378281;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-1641.472,-1991.324;Float;False;Property;_A_Opacity;A_Opacity;10;0;Create;True;0;0;False;0;1;0.607;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;160;-2346.756,-2929.826;Float;False;Property;_Fresnel_Power;Fresnel_Power;2;0;Create;True;0;0;False;0;1.9163;10;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;24;-1961.983,-1447.725;Float;False;Property;_Base_Color;Base_Color;5;1;[HDR];Create;True;0;0;False;0;0,0.4184885,1,0;0,0.4242868,0.5377358,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;-1557.04,-2269.676;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1707.805,621.8782;Float;False;Constant;_Float3;Float 3;11;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;20;-1650.028,-951.6866;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;119;-1959.971,921.8961;Float;True;Property;_Vertex_2_Tex;Vertex_2_Tex;26;0;Create;True;0;0;False;0;9d5a62bfcd403a84a89e299be1360dc3;fdb7f2284c843954baf647c1c33d72fe;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;159;-2356.88,-3015.883;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;1;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;19;-2213.24,-943.7277;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-1736.25,399.0892;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;87;-1932.109,-237.91;Float;False;0;86;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;89;-1895.109,-84.90971;Float;False;Constant;_Vector0;Vector 0;18;0;Create;True;0;0;False;0;0,0.15;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;127;-1761.072,1125.492;Float;False;Constant;_Float0;Float 0;25;0;Create;True;0;0;False;0;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;-1535.825,-1833.698;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;158;-1979.61,-3075.04;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-2539.16,-719.0723;Float;False;Property;_Down_Range;Down_Range;24;0;Create;True;0;0;False;0;1.023529;1.5;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;130;-1528.072,1224.492;Float;False;Constant;_Float2;Float 2;25;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1478.194,329.6877;Float;False;Property;_shadow_Range;shadow_Range;20;0;Create;True;0;0;False;0;0.99;0.95;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;129;-1528.072,1141.492;Float;False;Constant;_Float1;Float 1;25;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;126;-1596.072,927.492;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;23;-1676.95,-1461.67;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;88;-1695.109,-183.9102;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;105;-1299.276,-2276.832;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;22;-1395.4,-1042.495;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;93;-2449.216,-588.9248;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-1485.99,429.5966;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-1080.545,1029.431;Float;False;Property;_Vertex_2_Str;Vertex_2_Str;29;0;Create;True;0;0;False;0;0;0.51;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;128;-1349.586,866.0947;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;174;-1092.497,1138.79;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;92;-2240.57,-619.9509;Float;False;Property;_USE_Dissolve;USE_Dissolve;25;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;79;-1214.774,381.2282;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;106;-1065.456,-2277.615;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;175;-950.8701,-693.3164;Float;False;Property;_Depth_Fade;Depth_Fade;31;0;Create;True;0;0;False;0;0;2.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;54;-942.0137,370.8152;Float;False;Constant;_Color1;Color 1;12;0;Create;True;0;0;False;0;0.5943396,0.5943396,0.5943396,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;53;-949.2018,168.1531;Float;False;Constant;_Color0;Color 0;12;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;91;-1660.143,-39.0391;Float;False;Property;_Dissolve;Dissolve;23;0;Create;True;0;0;False;0;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-1245.88,-1460.364;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;162;-1663.367,-3075.837;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;86;-1483.742,-227.3452;Float;True;Property;_Dissovle_Tex;Dissovle_Tex;22;0;Create;True;0;0;False;0;fc949555820ad6345a3a5fb9625ef86d;fc949555820ad6345a3a5fb9625ef86d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;176;-726.1855,-706.7085;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;90;-1121.108,-210.9102;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;163;-1383.712,-3091.963;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;135;-928.9734,860.3728;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1051.108,-317.9105;Float;False;Constant;_Float6;Float 6;20;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;173;-799.3183,1100.115;Float;False;Property;_USE_Vertex;USE_Vertex;30;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;52;-628.8687,407.7186;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;76;-1988.488,-697.49;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;169;-1302.848,-3394.831;Float;False;Property;_Fresnel_OutColor;Fresnel_OutColor;3;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-974.9358,-1458.6;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;168;-1287.085,-3628.073;Float;False;Property;_Fresnel_Incolor;Fresnel_Incolor;4;1;[HDR];Create;True;0;0;False;0;0,0.4782095,1,0;0,0.2301888,0.4811321,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-587.1239,-1282.694;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;140;-686.2342,819.3416;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-1672.463,-706.5212;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;95;-866.1082,-216.9102;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-574.3181,1030.083;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;170;-962.5117,-3409.252;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;177;-444.1856,-715.7085;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;172;-254.9972,-1364.199;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;178;-471.5673,-923.7768;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;-275.5297,-775.98;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;-256.0666,830.7513;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;67.82997,-1255.386;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Water_Wave;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;65;0;66;0
WireConnection;65;1;67;0
WireConnection;63;0;62;0
WireConnection;63;2;65;0
WireConnection;58;1;63;0
WireConnection;59;0;58;0
WireConnection;60;0;59;0
WireConnection;60;1;61;0
WireConnection;108;1;113;0
WireConnection;100;1;102;0
WireConnection;73;0;60;0
WireConnection;73;1;74;0
WireConnection;99;0;98;0
WireConnection;99;2;100;0
WireConnection;109;0;107;0
WireConnection;109;2;108;0
WireConnection;82;0;73;0
WireConnection;83;0;82;0
WireConnection;83;1;85;0
WireConnection;123;0;147;0
WireConnection;123;1;148;0
WireConnection;97;1;99;0
WireConnection;110;1;109;0
WireConnection;84;0;83;0
WireConnection;111;0;112;0
WireConnection;111;1;110;1
WireConnection;103;0;104;0
WireConnection;103;1;97;1
WireConnection;122;0;121;0
WireConnection;122;2;123;0
WireConnection;117;0;103;0
WireConnection;117;1;118;0
WireConnection;20;0;82;0
WireConnection;20;1;21;0
WireConnection;119;1;122;0
WireConnection;19;0;82;0
WireConnection;19;1;18;0
WireConnection;81;0;83;0
WireConnection;81;1;84;0
WireConnection;114;0;111;0
WireConnection;114;1;115;0
WireConnection;158;2;159;0
WireConnection;158;3;160;0
WireConnection;126;0;119;1
WireConnection;126;1;127;0
WireConnection;23;1;24;0
WireConnection;23;2;19;0
WireConnection;88;0;87;0
WireConnection;88;2;89;0
WireConnection;105;0;117;0
WireConnection;105;1;114;0
WireConnection;22;0;20;0
WireConnection;80;0;81;0
WireConnection;80;1;49;0
WireConnection;128;0;126;0
WireConnection;128;3;129;0
WireConnection;128;4;130;0
WireConnection;92;1;77;0
WireConnection;92;0;93;3
WireConnection;79;0;51;0
WireConnection;79;1;80;0
WireConnection;106;0;105;0
WireConnection;26;0;23;0
WireConnection;26;1;22;0
WireConnection;162;0;158;0
WireConnection;86;1;88;0
WireConnection;176;0;175;0
WireConnection;90;0;86;1
WireConnection;90;1;91;0
WireConnection;163;0;162;0
WireConnection;135;0;128;0
WireConnection;173;1;138;0
WireConnection;173;0;174;4
WireConnection;52;0;53;0
WireConnection;52;1;54;0
WireConnection;52;2;79;0
WireConnection;76;0;82;0
WireConnection;76;1;92;0
WireConnection;27;0;26;0
WireConnection;27;1;106;0
WireConnection;55;0;27;0
WireConnection;55;1;52;0
WireConnection;48;0;19;0
WireConnection;48;1;76;0
WireConnection;95;0;96;0
WireConnection;95;1;90;0
WireConnection;136;0;135;0
WireConnection;136;1;173;0
WireConnection;170;0;168;0
WireConnection;170;1;169;0
WireConnection;170;2;163;0
WireConnection;177;0;176;0
WireConnection;172;0;170;0
WireConnection;172;2;55;0
WireConnection;78;0;48;0
WireConnection;78;1;95;0
WireConnection;78;2;177;0
WireConnection;142;0;140;0
WireConnection;142;1;136;0
WireConnection;0;2;172;0
WireConnection;0;9;78;0
WireConnection;0;11;142;0
ASEEND*/
//CHKSM=7C3869DFB573F0A2B7BEB4C2087E3FE603D56358