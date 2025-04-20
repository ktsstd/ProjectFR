// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify Shader/Skill/FX_shockwave_Line"
{
	Properties
	{
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Main_Power("Main_Power", Float) = 1
		_Main_ins("Main_ins", Float) = 1
		_Noise_tex("Noise_tex", 2D) = "white" {}
		_Noise_UPanner("Noise_UPanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Dissolve("Dissolve", Range( 0 , 1)) = 0.5062953
		_Opacity_Power("Opacity_Power", Float) = 1
		_Opacity_Ins("Opacity_Ins", Float) = 1
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Distotion("Distotion", Range( 0 , 1)) = 0.1017399
		_Nomral_UPanner("Nomral_UPanner", Float) = 0
		_Nomral_VPanner("Nomral_VPanner", Float) = 0
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
		uniform float4 _Main_Tex_ST;
		uniform sampler2D _Normal_Tex;
		uniform float _Nomral_UPanner;
		uniform float _Nomral_VPanner;
		uniform float4 _Normal_Tex_ST;
		uniform float _Distotion;
		uniform sampler2D _Noise_tex;
		uniform float _Noise_UPanner;
		uniform float _Noise_VPanner;
		uniform float4 _Noise_tex_ST;
		uniform float _Dissolve;
		uniform float _Main_Power;
		uniform float _Main_ins;
		uniform float4 _Main_Color;
		uniform float _Opacity_Power;
		uniform float _Opacity_Ins;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv0_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float2 appendResult33 = (float2(uv0_Main_Tex.x , ( uv0_Main_Tex.y + i.uv_tex4coord.z )));
			float2 appendResult43 = (float2(_Nomral_UPanner , _Nomral_VPanner));
			float2 uv0_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 panner42 = ( 1.0 * _Time.y * appendResult43 + uv0_Normal_Tex);
			float3 temp_output_37_0 = ( (UnpackNormal( tex2D( _Normal_Tex, panner42 ) )).xyz * _Distotion );
			float4 tex2DNode1 = tex2D( _Main_Tex, ( float3( appendResult33 ,  0.0 ) + temp_output_37_0 ).xy );
			float2 appendResult8 = (float2(_Noise_UPanner , _Noise_VPanner));
			float2 uv0_Noise_tex = i.uv_texcoord * _Noise_tex_ST.xy + _Noise_tex_ST.zw;
			float2 panner7 = ( 1.0 * _Time.y * appendResult8 + uv0_Noise_tex);
			float temp_output_3_0 = ( tex2DNode1.r * ( tex2DNode1.r + tex2D( _Noise_tex, ( temp_output_37_0 + float3( panner7 ,  0.0 ) ).xy ).r + (-2.0 + (_Dissolve - 0.0) * (1.0 - -2.0) / (1.0 - 0.0)) ) );
			o.Emission = ( ( pow( temp_output_3_0 , _Main_Power ) * _Main_ins ) * _Main_Color * i.vertexColor ).rgb;
			o.Alpha = ( i.vertexColor.a * saturate( ( saturate( pow( saturate( temp_output_3_0 ) , _Opacity_Power ) ) * _Opacity_Ins ) ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
1101;807;1028;592;959.609;41.1484;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;45;-2387.8,294.8496;Float;False;Property;_Nomral_VPanner;Nomral_VPanner;14;0;Create;True;0;0;False;0;0;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-2376.202,185.3847;Float;False;Property;_Nomral_UPanner;Nomral_UPanner;13;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;41;-2250.284,-4.534781;Float;False;0;35;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;43;-2115.901,221.7046;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;42;-2003.633,-9.637892;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;35;-1794.329,-48.86201;Float;True;Property;_Normal_Tex;Normal_Tex;11;0;Create;True;0;0;False;0;51fe2c9d5b236124d9f9e7ea528b0bea;51fe2c9d5b236124d9f9e7ea528b0bea;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;34;-1722.995,-373.1101;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1706.995,-533.11;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-1322.718,423.1586;Float;False;Property;_Noise_UPanner;Noise_UPanner;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1322.718,531.0588;Float;False;Property;_Noise_VPanner;Noise_VPanner;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;36;-1443.538,-61.75007;Float;False;True;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1155.018,198.2586;Float;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;-1565.038,162.251;Float;False;Property;_Distotion;Distotion;12;0;Create;True;0;0;False;0;0.1017399;0.356;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-1323.686,-360.7523;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-1075.717,445.2589;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;33;-1080.998,-422.2809;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;7;-882.0178,362.0589;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1171.538,-13.75005;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-564.3896,595.9957;Float;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;False;0;-2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-768,-160;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-664.4247,501.8405;Float;False;Property;_Dissolve;Dissolve;8;0;Create;True;0;0;False;0;0.5062953;0.485;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-566.3896,690.9957;Float;False;Constant;_Float1;Float 1;11;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-727.6152,151.42;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;27;-376.5608,499.8933;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-571.7001,-114;Float;True;Property;_Main_Tex;Main_Tex;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-577.8757,217.8862;Float;True;Property;_Noise_tex;Noise_tex;5;0;Create;True;0;0;False;0;7dafa5b81fffb8a41aaed27d6d1e7b58;7dafa5b81fffb8a41aaed27d6d1e7b58;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;5;-254.7154,49.3589;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-26.35038,-107.7746;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;18;195.8527,440.83;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;225.4794,278.2999;Float;False;Property;_Opacity_Power;Opacity_Power;9;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;21;414.9794,413.2998;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;23;586.4423,426.6867;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;560.8797,309.8997;Float;False;Property;_Opacity_Ins;Opacity_Ins;10;0;Create;True;0;0;False;0;1;2.63;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;98.33301,-233.3302;Float;False;Property;_Main_Power;Main_Power;3;0;Create;True;0;0;False;0;1;1.54;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;817.0797,408.4998;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;399.9331,-242.4302;Float;False;Property;_Main_ins;Main_ins;4;0;Create;True;0;0;False;0;1;1.69;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;11;286.8329,-103.3303;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;24;942.7424,297.3867;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;572.8047,-338.535;Float;False;Property;_Main_Color;Main_Color;1;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;581.9333,-111.1303;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;17;649.5233,130.9795;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;1087.742,222.3867;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;873.8047,-177.5351;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1269.871,-50.59846;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Amplify Shader/Skill/FX_shockwave_Line;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;43;0;44;0
WireConnection;43;1;45;0
WireConnection;42;0;41;0
WireConnection;42;2;43;0
WireConnection;35;1;42;0
WireConnection;36;0;35;0
WireConnection;31;0;2;2
WireConnection;31;1;34;3
WireConnection;8;0;9;0
WireConnection;8;1;10;0
WireConnection;33;0;2;1
WireConnection;33;1;31;0
WireConnection;7;0;6;0
WireConnection;7;2;8;0
WireConnection;37;0;36;0
WireConnection;37;1;38;0
WireConnection;39;0;33;0
WireConnection;39;1;37;0
WireConnection;40;0;37;0
WireConnection;40;1;7;0
WireConnection;27;0;26;0
WireConnection;27;3;29;0
WireConnection;27;4;30;0
WireConnection;1;1;39;0
WireConnection;4;1;40;0
WireConnection;5;0;1;1
WireConnection;5;1;4;1
WireConnection;5;2;27;0
WireConnection;3;0;1;1
WireConnection;3;1;5;0
WireConnection;18;0;3;0
WireConnection;21;0;18;0
WireConnection;21;1;20;0
WireConnection;23;0;21;0
WireConnection;22;0;23;0
WireConnection;22;1;19;0
WireConnection;11;0;3;0
WireConnection;11;1;12;0
WireConnection;24;0;22;0
WireConnection;14;0;11;0
WireConnection;14;1;13;0
WireConnection;25;0;17;4
WireConnection;25;1;24;0
WireConnection;15;0;14;0
WireConnection;15;1;16;0
WireConnection;15;2;17;0
WireConnection;0;2;15;0
WireConnection;0;9;25;0
ASEEND*/
//CHKSM=F21D2A27554CC02DC4A613833879C4B5DDC7C03A