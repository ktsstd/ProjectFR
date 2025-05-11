// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Boss_Skill_03-2"
{
	Properties
	{
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Normal_UPanner("Normal_UPanner", Float) = 0
		_Normal_VPanner("Normal_VPanner", Float) = 0
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Main_UPanner("Main_UPanner", Float) = 0
		_Main_VPanner("Main_VPanner", Float) = 0
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
		_Color_Offset("Color_Offset", Float) = 1
		_Color_Range("Color_Range", Float) = 1
		_Opacity("Opacity", Float) = 1
		[Toggle(_USE_CUSTOM_ON)] _USE_CUSTOM("USE_CUSTOM", Float) = 0
		[Toggle(_USE_DISSOVLE_ON)] _USE_Dissovle("USE_Dissovle", Float) = 0
		_Dissovle("Dissovle", Range( -1 , 1)) = 0
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		_Distortion("Distortion", Range( 0 , 1)) = 0
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
		#pragma target 3.0
		#pragma shader_feature _USE_CUSTOM_ON
		#pragma shader_feature _USE_DISSOVLE_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Main_Tex;
		uniform sampler2D _Normal_Tex;
		uniform float _Normal_UPanner;
		uniform float _Normal_VPanner;
		uniform float4 _Normal_Tex_ST;
		uniform float _Distortion;
		uniform float _Main_UPanner;
		uniform float _Main_VPanner;
		uniform float4 _Main_Tex_ST;
		uniform float _Color_Offset;
		uniform float _Color_Range;
		uniform float4 _Main_Color;
		uniform float _Dissovle;
		uniform sampler2D _Mask_Tex;
		uniform float4 _Mask_Tex_ST;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult4 = (float2(_Normal_UPanner , _Normal_VPanner));
			float2 uv0_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 panner3 = ( 1.0 * _Time.y * appendResult4 + uv0_Normal_Tex);
			#ifdef _USE_CUSTOM_ON
				float staticSwitch9 = i.uv_tex4coord.w;
			#else
				float staticSwitch9 = _Distortion;
			#endif
			float3 temp_output_8_0 = ( (UnpackNormal( tex2D( _Normal_Tex, panner3 ) )).xyz * staticSwitch9 );
			float2 appendResult16 = (float2(_Main_UPanner , _Main_VPanner));
			float2 uv0_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float2 panner15 = ( 1.0 * _Time.y * appendResult16 + uv0_Main_Tex);
			float4 tex2DNode12 = tex2D( _Main_Tex, ( temp_output_8_0 + float3( panner15 ,  0.0 ) ).xy );
			o.Emission = ( ( ( saturate( pow( tex2DNode12.r , _Color_Offset ) ) * _Color_Range ) * _Main_Color ) * i.vertexColor ).rgb;
			#ifdef _USE_DISSOVLE_ON
				float staticSwitch26 = i.uv_tex4coord.z;
			#else
				float staticSwitch26 = _Dissovle;
			#endif
			float2 uv0_Mask_Tex = i.uv_texcoord * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
			o.Alpha = ( i.vertexColor.a * saturate( ( ( tex2DNode12.r + staticSwitch26 ) * tex2D( _Mask_Tex, ( temp_output_8_0 + float3( uv0_Mask_Tex ,  0.0 ) ).xy ).r * _Opacity ) ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
1101;849;1028;550;1934.53;782.9423;3.367628;True;False
Node;AmplifyShaderEditor.CommentaryNode;19;-3191.118,-92.9912;Float;False;1411.731;648.1678;Comment;11;1;7;9;10;8;2;3;4;5;6;11;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-3105.024,235.9198;Float;False;Property;_Normal_VPanner;Normal_VPanner;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-3126.351,124.3554;Float;False;Property;_Normal_UPanner;Normal_UPanner;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-3141.118,-42.99119;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;4;-2932.755,163.7311;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;3;-2762.127,-5.256155;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2529.155,226.0759;Float;False;Property;_Distortion;Distortion;15;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;11;-2474,353.1766;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-2493.784,0.1521206;Float;True;Property;_Normal_Tex;Normal_Tex;1;0;Create;True;0;0;False;0;51fe2c9d5b236124d9f9e7ea528b0bea;51fe2c9d5b236124d9f9e7ea528b0bea;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-1701.154,302.4171;Float;False;Property;_Main_UPanner;Main_UPanner;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1679.826,413.9815;Float;False;Property;_Main_VPanner;Main_VPanner;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1715.92,135.0704;Float;False;0;12;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;9;-2151.805,188.341;Float;False;Property;_USE_CUSTOM;USE_CUSTOM;11;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;-1507.558,341.7927;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;7;-2166.57,22.63494;Float;False;True;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1942.72,20.235;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;15;-1426.324,134.9846;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-1189.185,18.45027;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;12;-1042.01,-31.85909;Float;True;Property;_Main_Tex;Main_Tex;4;0;Create;True;0;0;False;0;b4663d0c344defd428d80e5d482870a2;2a9f3a23369c23b43a6a798abf597c4c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-817.8718,185.7488;Float;False;Property;_Color_Offset;Color_Offset;8;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1110.228,330.5703;Float;False;Property;_Dissovle;Dissovle;13;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-1600.574,625.0977;Float;False;0;30;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;20;-670.3188,-0.5166931;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-1275.502,578.9725;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;26;-797.8718,332.7488;Float;False;Property;_USE_Dissovle;USE_Dissovle;12;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-525.4076,318.2173;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-649.5168,708.562;Float;False;Property;_Opacity;Opacity;10;0;Create;True;0;0;False;0;1;2.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;30;-1037.588,583.4316;Float;True;Property;_Mask_Tex;Mask_Tex;14;0;Create;True;0;0;False;0;32074e1089a84d342b6b9c0f3e096ef1;32074e1089a84d342b6b9c0f3e096ef1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-510.8718,127.7488;Float;False;Property;_Color_Range;Color_Range;9;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;22;-517.8718,-5.25116;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-363.8718,-4.25116;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;-389.7655,-221.6405;Float;False;Property;_Main_Color;Main_Color;7;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0.6057316,0.7735849,0.6570035,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-399.1231,609.7222;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;35;-131.1577,466.954;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-151.3459,-6.515009;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;38;-74.50719,190.057;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;218.0756,60.6133;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;192.9574,359.6255;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;542.7581,57.30329;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Boss_Skill_03-2;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;5;0
WireConnection;4;1;6;0
WireConnection;3;0;2;0
WireConnection;3;2;4;0
WireConnection;1;1;3;0
WireConnection;9;1;10;0
WireConnection;9;0;11;4
WireConnection;16;0;17;0
WireConnection;16;1;18;0
WireConnection;7;0;1;0
WireConnection;8;0;7;0
WireConnection;8;1;9;0
WireConnection;15;0;14;0
WireConnection;15;2;16;0
WireConnection;13;0;8;0
WireConnection;13;1;15;0
WireConnection;12;1;13;0
WireConnection;20;0;12;1
WireConnection;20;1;21;0
WireConnection;31;0;8;0
WireConnection;31;1;32;0
WireConnection;26;1;27;0
WireConnection;26;0;11;3
WireConnection;29;0;12;1
WireConnection;29;1;26;0
WireConnection;30;1;31;0
WireConnection;22;0;20;0
WireConnection;23;0;22;0
WireConnection;23;1;24;0
WireConnection;33;0;29;0
WireConnection;33;1;30;1
WireConnection;33;2;34;0
WireConnection;35;0;33;0
WireConnection;39;0;23;0
WireConnection;39;1;40;0
WireConnection;37;0;39;0
WireConnection;37;1;38;0
WireConnection;41;0;38;4
WireConnection;41;1;35;0
WireConnection;0;2;37;0
WireConnection;0;9;41;0
ASEEND*/
//CHKSM=51B6896A734826A5328CEC4E3A26ED6A39F929E1