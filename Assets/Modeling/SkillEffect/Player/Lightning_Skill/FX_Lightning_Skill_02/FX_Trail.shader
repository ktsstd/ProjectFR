// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify Shader/Skill/Lightning_Skill_02_Trail"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Main_Power("Main_Power", Float) = 1
		_Main_Ins("Main_Ins", Float) = 1
		[HDR]_Color_A("Color_A", Color) = (1,1,1,0)
		[HDR]_Color_B("Color_B", Color) = (1,1,1,0)
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Normal_UPanner("Normal_UPanner", Float) = 0
		_Normal_VPanner("Normal_VPanner", Float) = 0
		_Dissovle("Dissovle", Range( 0 , 1)) = 1
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		_Distortion("Distortion", Range( 0 , 1)) = 0
		[Toggle(_USE_DISSOVLE_ON)] _USE_Dissovle("USE_Dissovle", Float) = 1
		_Opacity("Opacity", Float) = 1
		_gradiant_Tex("gradiant_Tex", 2D) = "white" {}
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
		#pragma target 3.0
		#pragma shader_feature _USE_DISSOVLE_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 uv2_tex4coord2;
		};

		uniform sampler2D _Main_Tex;
		uniform sampler2D _Normal_Tex;
		uniform float _Normal_UPanner;
		uniform float _Normal_VPanner;
		uniform float4 _Normal_Tex_ST;
		uniform float _Distortion;
		uniform float4 _Main_Tex_ST;
		uniform float _Main_Power;
		uniform float _Main_Ins;
		uniform float4 _Color_A;
		uniform float4 _Color_B;
		uniform float _Dissovle;
		uniform sampler2D _Mask_Tex;
		uniform float4 _Mask_Tex_ST;
		uniform float _Opacity;
		uniform sampler2D _gradiant_Tex;
		uniform float4 _gradiant_Tex_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult6 = (float2(_Normal_UPanner , _Normal_VPanner));
			float2 uv0_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 panner8 = ( 1.0 * _Time.y * appendResult6 + uv0_Normal_Tex);
			float2 temp_output_11_0 = ( (UnpackNormal( tex2D( _Normal_Tex, panner8 ) )).xy * _Distortion );
			float2 uv0_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float2 appendResult45 = (float2(uv0_Main_Tex));
			float4 tex2DNode1 = tex2D( _Main_Tex, ( temp_output_11_0 + uv0_Main_Tex + appendResult45 ) );
			float temp_output_19_0 = ( pow( tex2DNode1.r , _Main_Power ) * _Main_Ins );
			float4 lerpResult51 = lerp( _Color_A , _Color_B , temp_output_19_0);
			o.Emission = ( temp_output_19_0 * lerpResult51 * i.vertexColor ).rgb;
			#ifdef _USE_DISSOVLE_ON
				float staticSwitch33 = i.uv2_tex4coord2.x;
			#else
				float staticSwitch33 = _Dissovle;
			#endif
			float2 uv0_Mask_Tex = i.uv_texcoord * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
			float2 appendResult47 = (float2(( uv0_Mask_Tex.y + i.uv2_tex4coord2.y ) , uv0_Mask_Tex.x));
			float2 uv_gradiant_Tex = i.uv_texcoord * _gradiant_Tex_ST.xy + _gradiant_Tex_ST.zw;
			o.Alpha = ( i.vertexColor.a * saturate( ( ( ( tex2DNode1.r + staticSwitch33 ) * tex2D( _Mask_Tex, ( temp_output_11_0 + appendResult47 ) ).r * _Opacity * tex2D( _gradiant_Tex, uv_gradiant_Tex ).r ) + 0.0 ) ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
0;790.6667;1105;569;1340.167;869.0523;1.895051;True;False
Node;AmplifyShaderEditor.RangedFloatNode;4;-2723.917,-137.8411;Float;False;Property;_Normal_UPanner;Normal_UPanner;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-2688.885,12.18888;Float;False;Property;_Normal_VPanner;Normal_VPanner;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;-2455.435,-100.3336;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-2731.963,-319.2916;Float;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;8;-2319.657,-221.1104;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-2106.6,-239.2289;Float;True;Property;_Normal_Tex;Normal_Tex;6;0;Create;True;0;0;False;0;df614e5b7ac739548bdbeccf0e59a22f;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;3;-1752.646,-173.6744;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;46;-1540.9,-489.9303;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-1809.758,-54.53306;Float;False;Property;_Distortion;Distortion;11;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-1999.562,551.6379;Float;False;0;23;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;34;-1985.836,375.4041;Float;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1303.211,20.77527;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;45;-1167.8,-437.9302;Float;False;FLOAT2;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1413.721,-160.4365;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-1665.441,506.8076;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-1058.211,-67.22473;Float;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;47;-1431.462,502.3351;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1080.616,108.8909;Float;False;Property;_Dissovle;Dissovle;9;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-859.0377,-140.653;Float;True;Property;_Main_Tex;Main_Tex;1;0;Create;True;0;0;False;0;eb490102d51d97f49900d200922a9bbc;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-1259.245,440.011;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;33;-789.1696,154.9176;Float;False;Property;_USE_Dissovle;USE_Dissovle;12;0;Create;True;0;0;False;0;0;1;0;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;23;-1095.558,404.5245;Float;True;Property;_Mask_Tex;Mask_Tex;10;0;Create;True;0;0;False;0;f9a4ad82895690a42883a946bf14c927;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;35;-911.3112,606.3442;Float;False;Property;_Opacity;Opacity;13;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-556.2092,51.90603;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-647.981,-245.0502;Float;False;Property;_Main_Power;Main_Power;2;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;37;-1051.733,718.4556;Float;True;Property;_gradiant_Tex;gradiant_Tex;14;0;Create;True;0;0;False;0;5304b304f7048454c85499b757cb7352;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-597.7731,454.6638;Float;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;16;-464.3271,-124.4415;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-421.8398,-242.309;Float;False;Property;_Main_Ins;Main_Ins;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;-628.5898,-751.7875;Float;False;Property;_Color_A;Color_A;4;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-268.0969,510.852;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;52;-621.2128,-546.475;Float;False;Property;_Color_B;Color_B;5;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-266.9672,-128.553;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;51;-174.0586,-433.121;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;36;-115.7049,533.5262;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;24;-155.2746,137.4999;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-46.91558,-119.2104;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;155.4483,314.5785;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;373.6099,-31.9411;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Amplify Shader/Skill/Lightning_Skill_02_Trail;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;False;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;8;0;7;0
WireConnection;8;2;6;0
WireConnection;2;1;8;0
WireConnection;3;0;2;0
WireConnection;45;0;46;0
WireConnection;11;0;3;0
WireConnection;11;1;9;0
WireConnection;48;0;29;2
WireConnection;48;1;34;2
WireConnection;12;0;11;0
WireConnection;12;1;14;0
WireConnection;12;2;45;0
WireConnection;47;0;48;0
WireConnection;47;1;29;1
WireConnection;1;1;12;0
WireConnection;30;0;11;0
WireConnection;30;1;47;0
WireConnection;33;1;32;0
WireConnection;33;0;34;1
WireConnection;23;1;30;0
WireConnection;31;0;1;1
WireConnection;31;1;33;0
WireConnection;27;0;31;0
WireConnection;27;1;23;1
WireConnection;27;2;35;0
WireConnection;27;3;37;1
WireConnection;16;0;1;1
WireConnection;16;1;17;0
WireConnection;43;0;27;0
WireConnection;19;0;16;0
WireConnection;19;1;20;0
WireConnection;51;0;21;0
WireConnection;51;1;52;0
WireConnection;51;2;19;0
WireConnection;36;0;43;0
WireConnection;22;0;19;0
WireConnection;22;1;51;0
WireConnection;22;2;24;0
WireConnection;25;0;24;4
WireConnection;25;1;36;0
WireConnection;0;2;22;0
WireConnection;0;9;25;0
ASEEND*/
//CHKSM=D16A41DC7A1C20578F4E988BBF73B24184992106