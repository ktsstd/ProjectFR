// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Lightning_01"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Color_Power("Color_Power", Float) = 1
		_Color_Ins("Color_Ins", Float) = 1
		_Opacity("Opacity", Float) = 1
		[HDR]_Color_A("Color_A", Color) = (1,1,1,0)
		[HDR]_Color_B("Color_B", Color) = (1,0.5106778,0,0)
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_Noise_UPanner("Noise_UPanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Normal_UPanner("Normal_UPanner", Float) = 0
		_Normal_VPanner("Normal_VPanner", Float) = 0
		_Distortion("Distortion", Range( 0 , 1)) = 0
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

		uniform float4 _Color_A;
		uniform float4 _Color_B;
		uniform sampler2D _Main_Tex;
		uniform sampler2D _Normal_Tex;
		uniform float _Normal_UPanner;
		uniform float _Normal_VPanner;
		uniform float4 _Normal_Tex_ST;
		uniform float _Distortion;
		uniform float4 _Main_Tex_ST;
		uniform sampler2D _Noise_Tex;
		uniform float _Noise_UPanner;
		uniform float _Noise_VPanner;
		uniform float4 _Noise_Tex_ST;
		uniform float _Color_Power;
		uniform float _Color_Ins;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult5 = (float2(_Normal_UPanner , _Normal_VPanner));
			float2 uv0_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 panner4 = ( 1.0 * _Time.y * appendResult5 + uv0_Normal_Tex);
			float2 temp_output_9_0 = ( (UnpackNormal( tex2D( _Normal_Tex, panner4 ) )).xy * _Distortion );
			float2 uv0_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float2 appendResult16 = (float2(_Noise_UPanner , _Noise_VPanner));
			float2 uv0_Noise_Tex = i.uv_texcoord * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
			float2 panner15 = ( 1.0 * _Time.y * appendResult16 + uv0_Noise_Tex);
			float temp_output_25_0 = saturate( ( tex2D( _Main_Tex, ( temp_output_9_0 + uv0_Main_Tex ) ).r * ( tex2D( _Noise_Tex, ( temp_output_9_0 + panner15 ) ).r + i.uv_tex4coord.z ) ) );
			float4 lerpResult35 = lerp( _Color_A , _Color_B , saturate( ( saturate( pow( temp_output_25_0 , _Color_Power ) ) * _Color_Ins ) ));
			o.Emission = ( lerpResult35 * i.vertexColor ).rgb;
			o.Alpha = ( i.vertexColor.a * saturate( ( temp_output_25_0 * _Opacity ) ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
549.3334;798;902;561;1038.965;902.0884;3.233088;True;False
Node;AmplifyShaderEditor.RangedFloatNode;7;-2470.754,-17.53771;Float;False;Property;_Normal_VPanner;Normal_VPanner;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-2463.754,-126.5377;Float;False;Property;_Normal_UPanner;Normal_UPanner;11;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-2295.601,-277.5307;Float;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;5;-2192.755,-78.53772;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;4;-2016.388,-170.8393;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1761.797,550.5463;Float;False;Property;_Noise_UPanner;Noise_UPanner;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1768.797,659.5463;Float;False;Property;_Noise_VPanner;Noise_VPanner;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1712.204,-193.5396;Float;True;Property;_Normal_Tex;Normal_Tex;10;0;Create;True;0;0;False;0;51fe2c9d5b236124d9f9e7ea528b0bea;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;16;-1490.798,598.5462;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1433.64,-81.0343;Float;False;Property;_Distortion;Distortion;13;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;8;-1405.122,-191.6045;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1593.644,399.5535;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1156.64,-163.0343;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;15;-1314.431,506.2448;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1428.531,127.797;Float;False;0;19;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-917.6042,190.5099;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-591.2932,171.7473;Float;True;Property;_Noise_Tex;Noise_Tex;7;0;Create;True;0;0;False;0;8d21b35fab1359d4aa689ddf302e1b01;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;23;-490.0781,421.364;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-923.1378,-76.94229;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;19;-552.2442,-87.59508;Float;True;Property;_Main_Tex;Main_Tex;1;0;Create;True;0;0;False;0;f69e3eb59a11ed944891ff2a6bebf8d0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-218.9372,207.4023;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-137.7793,-47.13838;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;25;17.15886,-69.27243;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-27.10919,-270.3228;Float;False;Property;_Color_Power;Color_Power;2;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;26;161.0296,-309.0574;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;338.1013,-427.1053;Float;False;Property;_Color_Ins;Color_Ins;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;31;343.6351,-297.9905;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;493.0393,-301.6795;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;41.13725,137.3113;Float;False;Property;_Opacity;Opacity;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;520.7067,-866.0957;Float;False;Property;_Color_A;Color_A;5;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;37;437.7042,-661.3565;Float;False;Property;_Color_B;Color_B;6;1;[HDR];Create;True;0;0;False;0;1,0.5106778,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;34;642.444,-301.6794;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;229.2761,48.77533;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;39;974.4533,-268.4782;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;35;764.1811,-550.6866;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;30;384.2138,37.70824;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;1193.948,-117.2292;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;1157.059,-500.885;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1469.857,-429.3208;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Lightning_01;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;False;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;6;0
WireConnection;5;1;7;0
WireConnection;4;0;3;0
WireConnection;4;2;5;0
WireConnection;2;1;4;0
WireConnection;16;0;17;0
WireConnection;16;1;18;0
WireConnection;8;0;2;0
WireConnection;9;0;8;0
WireConnection;9;1;10;0
WireConnection;15;0;14;0
WireConnection;15;2;16;0
WireConnection;11;0;9;0
WireConnection;11;1;15;0
WireConnection;1;1;11;0
WireConnection;12;0;9;0
WireConnection;12;1;13;0
WireConnection;19;1;12;0
WireConnection;20;0;1;1
WireConnection;20;1;23;3
WireConnection;21;0;19;1
WireConnection;21;1;20;0
WireConnection;25;0;21;0
WireConnection;26;0;25;0
WireConnection;26;1;27;0
WireConnection;31;0;26;0
WireConnection;32;0;31;0
WireConnection;32;1;33;0
WireConnection;34;0;32;0
WireConnection;28;0;25;0
WireConnection;28;1;29;0
WireConnection;35;0;36;0
WireConnection;35;1;37;0
WireConnection;35;2;34;0
WireConnection;30;0;28;0
WireConnection;40;0;39;4
WireConnection;40;1;30;0
WireConnection;38;0;35;0
WireConnection;38;1;39;0
WireConnection;0;2;38;0
WireConnection;0;9;40;0
ASEEND*/
//CHKSM=11393DCE04389F4C776A7711D43464A7312437BE