// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Scroll/FX_Ganghwa_Add"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_Noise_UPanner("Noise_UPanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Main_Ins("Main_Ins", Float) = 1
		_Main_Power("Main_Power", Float) = 1
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
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
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Main_Tex;
		uniform float4 _Main_Tex_ST;
		uniform sampler2D _Noise_Tex;
		uniform float _Noise_UPanner;
		uniform float _Noise_VPanner;
		uniform float4 _Noise_Tex_ST;
		uniform float _Main_Power;
		uniform float _Main_Ins;
		uniform float4 _Main_Color;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float2 appendResult5 = (float2(_Noise_UPanner , _Noise_VPanner));
			float2 uv0_Noise_Tex = i.uv_texcoord * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
			float2 panner4 = ( 1.0 * _Time.y * appendResult5 + uv0_Noise_Tex);
			o.Emission = ( ( pow( ( tex2D( _Main_Tex, uv_Main_Tex ).r * tex2D( _Noise_Tex, panner4 ).r ) , _Main_Power ) * _Main_Ins ) * _Main_Color * i.vertexColor ).rgb;
			o.Alpha = i.vertexColor.a;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
1103;719;1031;680;911.6929;179.9929;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;6;-1943.907,292.681;Float;False;Property;_Noise_UPanner;Noise_UPanner;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1949.891,436.2737;Float;False;Property;_Noise_VPanner;Noise_VPanner;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;5;-1721.339,324.9896;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1780.76,85.66841;Float;False;0;8;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;4;-1498.769,188.5763;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;8;-1162.524,253.1933;Float;True;Property;_Noise_Tex;Noise_Tex;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1157.01,-71.78758;Float;True;Property;_Main_Tex;Main_Tex;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-808.4312,9.205379;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-710.8424,-155.2932;Float;False;Property;_Main_Power;Main_Power;6;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;14;-558.7423,-9.69319;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-454.7426,-146.1932;Float;False;Property;_Main_Ins;Main_Ins;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-328.6426,-4.493168;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-305.2431,-257.9931;Float;False;Property;_Main_Color;Main_Color;7;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;17;-427.4429,260.7071;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-84.24294,0.7068717;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;232.7,11.7;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Scroll/FX_Ganghwa_Add;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;8;5;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;6;0
WireConnection;5;1;7;0
WireConnection;4;0;2;0
WireConnection;4;2;5;0
WireConnection;8;1;4;0
WireConnection;9;0;1;1
WireConnection;9;1;8;1
WireConnection;14;0;9;0
WireConnection;14;1;13;0
WireConnection;10;0;14;0
WireConnection;10;1;11;0
WireConnection;15;0;10;0
WireConnection;15;1;16;0
WireConnection;15;2;17;0
WireConnection;0;2;15;0
WireConnection;0;9;17;4
ASEEND*/
//CHKSM=D80ECF6A915BF753FF8E4096C1AB654C7F5ED312