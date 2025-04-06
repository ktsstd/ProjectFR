// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify Shader/Skill/FX_Thunder_Distortion"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Main_Power("Main_Power", Float) = 1
		_Main_Ins("Main_Ins", Float) = 1
		[HDR]_Main_A("Main_A", Color) = (1,1,1,0)
		[HDR]_Color_B("Color_B", Color) = (1,0,0,0)
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Noise_Vpanner("Noise_Vpanner", Float) = 0
		_Noise_Upanner("Noise_Upanner", Float) = 0
		_Normal_VPanner("Normal_VPanner", Float) = 0
		_Normal_UPanner("Normal_UPanner", Float) = 0
		_Distortion("Distortion", Range( 0 , 1)) = 0
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_Dissovle("Dissovle", Range( -1 , 1)) = 0
		_Opacity("Opacity", Float) = 1
		[Toggle(_USE_CUSTOM_ON)] _USE_CUSTOM("USE_CUSTOM", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha One
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _USE_CUSTOM_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Main_A;
		uniform float4 _Color_B;
		uniform sampler2D _Main_Tex;
		uniform sampler2D _Normal_Tex;
		uniform float _Normal_UPanner;
		uniform float _Normal_VPanner;
		uniform float4 _Normal_Tex_ST;
		uniform float _Distortion;
		uniform float4 _Main_Tex_ST;
		uniform float _Main_Power;
		uniform float _Main_Ins;
		uniform float _Dissovle;
		uniform sampler2D _Noise_Tex;
		uniform float _Noise_Upanner;
		uniform float _Noise_Vpanner;
		uniform float4 _Noise_Tex_ST;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult18 = (float2(_Normal_UPanner , _Normal_VPanner));
			float2 uv0_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 panner17 = ( 1.0 * _Time.y * appendResult18 + uv0_Normal_Tex);
			#ifdef _USE_CUSTOM_ON
				float staticSwitch42 = i.uv_tex4coord.w;
			#else
				float staticSwitch42 = _Distortion;
			#endif
			float2 uv0_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float4 tex2DNode1 = tex2D( _Main_Tex, ( ( (UnpackNormal( tex2D( _Normal_Tex, panner17 ) )).xy * staticSwitch42 ) + uv0_Main_Tex ) );
			float4 lerpResult11 = lerp( _Main_A , _Color_B , ( saturate( pow( tex2DNode1.r , _Main_Power ) ) * _Main_Ins ));
			o.Emission = ( lerpResult11 * i.vertexColor ).rgb;
			#ifdef _USE_CUSTOM_ON
				float staticSwitch43 = i.uv_tex4coord.z;
			#else
				float staticSwitch43 = _Dissovle;
			#endif
			float2 appendResult38 = (float2(_Noise_Upanner , _Noise_Vpanner));
			float2 uv0_Noise_Tex = i.uv_texcoord * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
			float2 panner37 = ( 1.0 * _Time.y * appendResult38 + uv0_Noise_Tex);
			o.Alpha = ( i.vertexColor.a * saturate( ( saturate( ( tex2DNode1.r * saturate( ( staticSwitch43 + tex2D( _Noise_Tex, panner37 ).r ) ) ) ) * _Opacity ) ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
499.3333;785.3334;734;582;3308.137;485.0999;2.829762;True;False
Node;AmplifyShaderEditor.RangedFloatNode;20;-3683.394,-79.74831;Float;False;Property;_Normal_VPanner;Normal_VPanner;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-3704.584,-185.6937;Float;False;Property;_Normal_UPanner;Normal_UPanner;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;18;-3438.894,-170.8571;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-3661.505,-334.2104;Float;False;0;14;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-2206.647,574.1128;Float;False;Property;_Noise_Upanner;Noise_Upanner;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-2185.457,680.0583;Float;False;Property;_Noise_Vpanner;Noise_Vpanner;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;17;-3232.433,-282.0281;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2661.819,-215.1679;Float;False;Property;_Distortion;Distortion;11;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;41;-2575.021,289.1443;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-2926.982,-366.5247;Float;True;Property;_Normal_Tex;Normal_Tex;6;0;Create;True;0;0;False;0;f200615d594ab2d4c982c844dd3d4d57;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;38;-1940.955,588.9495;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-2165.537,425.5963;Float;False;0;26;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;15;-2562.158,-306.9848;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1585.373,225.7348;Float;False;Property;_Dissovle;Dissovle;13;0;Create;True;0;0;False;0;0;-0.64;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;42;-2405.969,-4.382677;Float;False;Property;_USE_CUSTOM;USE_CUSTOM;15;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;37;-1734.495,477.7785;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;43;-1334.58,315.0266;Float;False;Property;_USE_CUSTOM;USE_CUSTOM;16;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;26;-1481.361,453.7945;Float;True;Property;_Noise_Tex;Noise_Tex;12;0;Create;True;0;0;False;0;04b2512dc2ed6df40a5896e0ea874266;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2328.825,-262.0916;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-1614.607,-22.38421;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-1298.203,-44.9845;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-1183.506,447.049;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;30;-957.2079,450.7894;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-916.4596,-110.4374;Float;False;Property;_Main_Power;Main_Power;2;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1109.086,-24.80396;Float;True;Property;_Main_Tex;Main_Tex;1;0;Create;True;0;0;False;0;1d5692f7487326d49b4e4ddd135defc1;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-781.4058,405.8899;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;2;-778.8783,0.3739834;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-545.944,-96.64199;Float;False;Property;_Main_Ins;Main_Ins;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-605.6614,517.1348;Float;False;Property;_Opacity;Opacity;14;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;9;-607.4464,12.45906;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;32;-631.8378,410.7934;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;-471.8285,-534.3684;Float;False;Property;_Main_A;Main_A;4;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-397.3407,2.869446;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-472.8149,-342.7398;Float;False;Property;_Color_B;Color_B;5;1;[HDR];Create;True;0;0;False;0;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-425.6989,435.3336;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;11;-237.7606,-246.7301;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;8;-365.7625,147.4746;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;35;-271.9129,436.9698;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-88.33264,302.3557;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;17.3348,-30.23744;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;204.406,-3.464509;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Amplify Shader/Skill/FX_Thunder_Distortion;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;False;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;8;5;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;0;19;0
WireConnection;18;1;20;0
WireConnection;17;0;16;0
WireConnection;17;2;18;0
WireConnection;14;1;17;0
WireConnection;38;0;39;0
WireConnection;38;1;40;0
WireConnection;15;0;14;0
WireConnection;42;1;22;0
WireConnection;42;0;41;4
WireConnection;37;0;36;0
WireConnection;37;2;38;0
WireConnection;43;1;31;0
WireConnection;43;0;41;3
WireConnection;26;1;37;0
WireConnection;21;0;15;0
WireConnection;21;1;42;0
WireConnection;24;0;21;0
WireConnection;24;1;25;0
WireConnection;29;0;43;0
WireConnection;29;1;26;1
WireConnection;30;0;29;0
WireConnection;1;1;24;0
WireConnection;28;0;1;1
WireConnection;28;1;30;0
WireConnection;2;0;1;1
WireConnection;2;1;3;0
WireConnection;9;0;2;0
WireConnection;32;0;28;0
WireConnection;4;0;9;0
WireConnection;4;1;5;0
WireConnection;33;0;32;0
WireConnection;33;1;34;0
WireConnection;11;0;7;0
WireConnection;11;1;10;0
WireConnection;11;2;4;0
WireConnection;35;0;33;0
WireConnection;13;0;8;4
WireConnection;13;1;35;0
WireConnection;12;0;11;0
WireConnection;12;1;8;0
WireConnection;0;2;12;0
WireConnection;0;9;13;0
ASEEND*/
//CHKSM=681F1790EE893E1E24EA72F7BBAD7E19A5E01C9D