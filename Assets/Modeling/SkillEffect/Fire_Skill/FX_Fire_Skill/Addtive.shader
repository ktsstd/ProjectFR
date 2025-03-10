// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Add"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
		_Main_Power("Main_Power", Float) = 1
		[Toggle(_USE_DEPTH_FADE_ON)] _USE_Depth_Fade("USE_Depth_Fade", Float) = 0
		_Depth_Fade("Depth_Fade", Float) = 0
		_Main_Ins("Main_Ins", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha One
		
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature _USE_DEPTH_FADE_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform float4 _Main_Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
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
			float2 uv0_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 temp_cast_0 = (_Main_Power).xxxx;
			o.Emission = ( _Main_Color * ( pow( tex2D( _MainTex, uv0_MainTex ) , temp_cast_0 ) * _Main_Ins ) * i.vertexColor ).rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth24 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth24 = abs( ( screenDepth24 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth_Fade ) );
			#ifdef _USE_DEPTH_FADE_ON
				float staticSwitch23 = ( i.vertexColor.a * saturate( distanceDepth24 ) );
			#else
				float staticSwitch23 = i.vertexColor.a;
			#endif
			o.Alpha = staticSwitch23;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
1024;450;1359;921;884.7746;437.5644;1.175738;True;False
Node;AmplifyShaderEditor.RangedFloatNode;25;-637.9563,542.4286;Float;False;Property;_Depth_Fade;Depth_Fade;5;0;Create;True;0;0;False;0;0;0.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-853.7767,-11.77667;Float;True;0;8;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;24;-465.3849,515.123;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-585.5,-37.5;Float;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;8d21b35fab1359d4aa689ddf302e1b01;f38c1841d7ffa7240ba7ae5c416b4ade;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-410.5,-148.5;Float;False;Property;_Main_Power;Main_Power;3;0;Create;True;0;0;False;0;1;1.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;21;-130.7,228.9287;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;26;-178.1302,520.5841;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;11;-213.5,-33.5;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-102.7283,-144.203;Float;False;Property;_Main_Ins;Main_Ins;6;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;16.03765,504.3931;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;45.57467,-267.6478;Float;False;Property;_Main_Color;Main_Color;2;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;85.27167,-6.203033;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;23;163.4877,325.2684;Float;False;Property;_USE_Depth_Fade;USE_Depth_Fade;4;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;296.2947,-62.67667;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;551.6545,143.3657;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Add;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;8;5;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;24;0;25;0
WireConnection;8;1;10;0
WireConnection;26;0;24;0
WireConnection;11;0;8;0
WireConnection;11;1;12;0
WireConnection;22;0;21;4
WireConnection;22;1;26;0
WireConnection;27;0;11;0
WireConnection;27;1;28;0
WireConnection;23;1;21;4
WireConnection;23;0;22;0
WireConnection;13;0;14;0
WireConnection;13;1;27;0
WireConnection;13;2;21;0
WireConnection;2;2;13;0
WireConnection;2;9;23;0
ASEEND*/
//CHKSM=F03061AD485CFB54CFCD0083B9BD3BB5D877C448