// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify Shader/SBS/30week/FX_Tornado_pot_Front"
{
	Properties
	{
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_Noise_UPanner("Noise_UPanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Noise_U("Noise_U", Float) = 1.35
		_Opacity("Opacity", Float) = 1
		_Opacity_Power("Opacity_Power", Float) = 1
		[HDR]_Color_A("Color_A", Color) = (1,1,1,0)
		[HDR]_Color_B("Color_B", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
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
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Color_A;
		uniform float4 _Color_B;
		uniform sampler2D _Noise_Tex;
		uniform float _Noise_UPanner;
		uniform float _Noise_VPanner;
		uniform float4 _Noise_Tex_ST;
		uniform float _Noise_U;
		uniform float _Opacity_Power;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult9 = (float2(_Noise_UPanner , _Noise_VPanner));
			float2 uv0_Noise_Tex = i.uv_texcoord * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
			float2 appendResult8 = (float2(( uv0_Noise_Tex.x + ( uv0_Noise_Tex.y * _Noise_U ) ) , uv0_Noise_Tex.y));
			float2 panner10 = ( 1.0 * _Time.y * appendResult9 + appendResult8);
			float4 tex2DNode1 = tex2D( _Noise_Tex, panner10 );
			float4 lerpResult31 = lerp( _Color_A , _Color_B , tex2DNode1.r);
			o.Emission = ( lerpResult31 * i.vertexColor ).rgb;
			float temp_output_16_0 = ( 1.0 - i.uv_texcoord.y );
			o.Alpha = ( i.vertexColor.a * saturate( ( saturate( pow( saturate( ( ( tex2DNode1.r * temp_output_16_0 ) + temp_output_16_0 ) ) , _Opacity_Power ) ) * _Opacity ) ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
993;818;1190;581;1073.848;841.2548;2.124881;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-1604.106,-138.9277;Float;False;Property;_Noise_U;Noise_U;4;0;Create;True;0;0;False;0;1.35;0.89;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1621.328,-288.757;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-1367.59,-151.2022;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1205.296,-42.37745;Float;False;Property;_Noise_UPanner;Noise_UPanner;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1202.438,35.51902;Float;False;Property;_Noise_VPanner;Noise_VPanner;3;0;Create;True;0;0;False;0;0;-2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-1245.68,-287.6089;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-1052.196,-252.212;Float;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-1000.544,-8.651791;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;10;-804.3294,-136.8629;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-728.1123,163.83;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;16;-474.8146,213.429;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-602.6293,-163.3888;Float;True;Property;_Noise_Tex;Noise_Tex;1;0;Create;True;0;0;False;0;d66bfe10b05fc6e46b051b9afca6d29b;d9f54aef531c9fa48b2f689e6a44f87d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-278.4832,-24.30268;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-22.93075,40.53053;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;178.4773,130.3376;Float;False;Property;_Opacity_Power;Opacity_Power;6;0;Create;True;0;0;False;0;1;2.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;20;208.8789,45.26937;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;21;366.879,38.26938;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;22;528.2679,42.30957;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;482.4591,140.5393;Float;False;Property;_Opacity;Opacity;5;0;Create;True;0;0;False;0;1;2.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;675.8784,38.26938;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;28;390.6119,-583.8173;Float;False;Property;_Color_A;Color_A;7;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0.377998,0.3745995,0.4339623,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;29;396.5526,-408.7995;Float;False;Property;_Color_B;Color_B;8;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.1901174,0.1804913,0.2264151,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;25;807.9918,35.42036;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;26;757.8478,-251.7708;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;736.5374,-387.5652;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;991.0243,-110.6024;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;971.0519,-267.5448;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1186.687,-293.9969;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Amplify Shader/SBS/30week/FX_Tornado_pot_Front;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;2;2
WireConnection;4;1;3;0
WireConnection;7;0;2;1
WireConnection;7;1;4;0
WireConnection;8;0;7;0
WireConnection;8;1;2;2
WireConnection;9;0;5;0
WireConnection;9;1;6;0
WireConnection;10;0;8;0
WireConnection;10;2;9;0
WireConnection;16;0;14;2
WireConnection;1;1;10;0
WireConnection;12;0;1;1
WireConnection;12;1;16;0
WireConnection;17;0;12;0
WireConnection;17;1;16;0
WireConnection;20;0;17;0
WireConnection;21;0;20;0
WireConnection;21;1;19;0
WireConnection;22;0;21;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;25;0;24;0
WireConnection;31;0;28;0
WireConnection;31;1;29;0
WireConnection;31;2;1;1
WireConnection;27;0;26;4
WireConnection;27;1;25;0
WireConnection;30;0;31;0
WireConnection;30;1;26;0
WireConnection;0;2;30;0
WireConnection;0;9;27;0
ASEEND*/
//CHKSM=B88004472D4B6DDA68CBBB85929C3935B1BD7092