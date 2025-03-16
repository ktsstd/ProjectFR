// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Water_Play_Shock_Wave"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Main_Power("Main_Power", Float) = 11
		_Main_Ins("Main_Ins", Float) = 1
		_NOise_Tex("NOise_Tex", 2D) = "white" {}
		_Noise_UPanner("Noise_UPanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Normal_UPanner("Normal_UPanner", Float) = 0
		_Normal_VPanner("Normal_VPanner", Float) = 0
		_Distortion("Distortion", Range( 0 , 1)) = 0
		_Depth_Fade("Depth_Fade", Float) = 0
		_Chroma("Chroma", Range( -0.1 , 0.1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
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
			float4 uv2_tex4coord2;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform sampler2D _Main_Tex;
		uniform sampler2D _Normal_Tex;
		uniform float _Normal_UPanner;
		uniform float _Normal_VPanner;
		uniform float4 _Normal_Tex_ST;
		uniform float _Distortion;
		uniform float4 _Main_Tex_ST;
		uniform sampler2D _NOise_Tex;
		uniform float _Noise_UPanner;
		uniform float _Noise_VPanner;
		uniform float4 _NOise_Tex_ST;
		uniform float _Chroma;
		uniform float _Main_Power;
		uniform float _Main_Ins;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth_Fade;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult10 = (float2(_Normal_UPanner , _Normal_VPanner));
			float2 uv0_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 panner9 = ( 1.0 * _Time.y * appendResult10 + uv0_Normal_Tex);
			float2 temp_output_6_0 = ( (UnpackNormal( tex2D( _Normal_Tex, panner9 ) )).xy * _Distortion );
			float2 uv0_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float2 appendResult33 = (float2(_Noise_UPanner , _Noise_VPanner));
			float2 uv0_NOise_Tex = i.uv_texcoord * _NOise_Tex_ST.xy + _NOise_Tex_ST.zw;
			float2 panner30 = ( 1.0 * _Time.y * appendResult33 + uv0_NOise_Tex);
			float2 temp_output_36_0 = ( temp_output_6_0 + panner30 );
			float2 temp_cast_0 = (_Chroma).xx;
			float4 appendResult43 = (float4(tex2D( _NOise_Tex, ( temp_output_36_0 + _Chroma ) ).r , tex2D( _NOise_Tex, temp_output_36_0 ).g , tex2D( _NOise_Tex, ( temp_output_36_0 - temp_cast_0 ) ).b , 0.0));
			float4 temp_cast_1 = (_Main_Power).xxxx;
			o.Emission = ( pow( saturate( ( tex2D( _Main_Tex, ( temp_output_6_0 + uv0_Main_Tex ) ).r * ( appendResult43 + i.uv2_tex4coord2.z ) ) ) , temp_cast_1 ) * _Main_Ins * i.vertexColor ).xyz;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth13 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth13 = abs( ( screenDepth13 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth_Fade ) );
			o.Alpha = ( i.vertexColor.a * saturate( distanceDepth13 ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
492.6667;714;908;638;193.0706;504.0799;1.090895;True;False
Node;AmplifyShaderEditor.RangedFloatNode;11;-2507.533,-355.5494;Float;False;Property;_Normal_UPanner;Normal_UPanner;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2503.533,-254.5494;Float;False;Property;_Normal_VPanner;Normal_VPanner;9;0;Create;True;0;0;False;0;0;0.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-2318.533,-324.5494;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-2427.533,-537.5494;Float;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;9;-2175.533,-489.5494;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2820.619,116.7885;Float;False;Property;_Noise_UPanner;Noise_UPanner;5;0;Create;True;0;0;False;0;0;-0.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-2819.619,215.7886;Float;False;Property;_Noise_VPanner;Noise_VPanner;6;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-1858.766,-506.6028;Float;True;Property;_Normal_Tex;Normal_Tex;7;0;Create;True;0;0;False;0;645b0a2fda25d114599a2fba6417fe81;645b0a2fda25d114599a2fba6417fe81;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-2687.619,-129.2114;Float;False;0;39;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;33;-2636.619,129.7885;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1580.838,-402.179;Float;False;Property;_Distortion;Distortion;10;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;5;-1558.766,-501.6028;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;30;-2439.219,52.48856;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1253.026,-489.3762;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-2097.618,513.1176;Float;False;Property;_Chroma;Chroma;12;0;Create;True;0;0;False;0;0;0.041;-0.1;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-2048.23,210.351;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-1590.095,88.4548;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;39;-1563.633,-115.7903;Float;True;Property;_NOise_Tex;NOise_Tex;4;0;Create;True;0;0;False;0;None;fdb7f2284c843954baf647c1c33d72fe;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;-1600.452,496.8413;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;47;-1244.526,-261.6453;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;38;-1115.45,466.3411;Float;True;Property;_TextureSample1;Texture Sample 1;13;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;-1121.071,203.5425;Float;True;Property;_TextureSample0;Texture Sample 0;12;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;25;-1082.727,-27.69561;Float;True;Property;_Noise_Tex;Noise_Tex;9;0;Create;True;0;0;False;0;None;b21114d325524b24ebff225951b0e2fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-941.9661,-310.5028;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;35;-758.7286,507.0652;Float;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;43;-774.1516,249.752;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-590.3538,-22.02442;Float;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;1;-779.5858,-274.2064;Float;True;Property;_Main_Tex;Main_Tex;1;0;Create;True;0;0;False;0;None;9dd971b6f34bab246bd86c7d20cf1f4a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-464.1269,-239.8956;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-356.9304,160.9824;Float;False;Property;_Depth_Fade;Depth_Fade;11;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;13;-169.999,137.5584;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-322.3269,-363.0956;Float;False;Property;_Main_Power;Main_Power;2;0;Create;True;0;0;False;0;11;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;27;-308.727,-190.7956;Float;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-88.3269,-365.0956;Float;False;Property;_Main_Ins;Main_Ins;3;0;Create;True;0;0;False;0;1;1.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;16;71.2731,-136.6956;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;14;83.31173,138.9357;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;21;-167.3269,-265.0956;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;321.073,12.50442;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;257.0731,-253.7956;Float;False;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;678.3,-142.7;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Water_Play_Shock_Wave;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;9;0;8;0
WireConnection;9;2;10;0
WireConnection;4;1;9;0
WireConnection;33;0;31;0
WireConnection;33;1;34;0
WireConnection;5;0;4;0
WireConnection;30;0;29;0
WireConnection;30;2;33;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;36;0;6;0
WireConnection;36;1;30;0
WireConnection;40;0;36;0
WireConnection;40;1;42;0
WireConnection;41;0;36;0
WireConnection;41;1;42;0
WireConnection;38;0;39;0
WireConnection;38;1;41;0
WireConnection;37;0;39;0
WireConnection;37;1;36;0
WireConnection;25;0;39;0
WireConnection;25;1;40;0
WireConnection;3;0;6;0
WireConnection;3;1;47;0
WireConnection;43;0;25;1
WireConnection;43;1;37;2
WireConnection;43;2;38;3
WireConnection;26;0;43;0
WireConnection;26;1;35;3
WireConnection;1;1;3;0
WireConnection;24;0;1;1
WireConnection;24;1;26;0
WireConnection;13;0;15;0
WireConnection;27;0;24;0
WireConnection;14;0;13;0
WireConnection;21;0;27;0
WireConnection;21;1;22;0
WireConnection;20;0;16;4
WireConnection;20;1;14;0
WireConnection;19;0;21;0
WireConnection;19;1;23;0
WireConnection;19;2;16;0
WireConnection;0;2;19;0
WireConnection;0;9;20;0
ASEEND*/
//CHKSM=B1D8734B28A961F9B6D2D01B61E0613AFD4B3072