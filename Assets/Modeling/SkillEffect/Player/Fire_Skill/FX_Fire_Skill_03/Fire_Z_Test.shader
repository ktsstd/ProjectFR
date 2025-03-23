// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Fire_Ztest"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Normal_UPanner("Normal_UPanner", Float) = 0
		_Noise_Upanner("Noise_Upanner", Float) = 0
		_Normal_VPanner("Normal_VPanner", Float) = 0
		_Noise_Vpanner("Noise_Vpanner", Float) = 0
		_Distortion("Distortion", Range( 0 , 1)) = 0
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_Dissovle("Dissovle", Range( -1 , 1)) = 1
		_Noise_Power("Noise_Power", Float) = 1
		_Noise_Ins("Noise_Ins", Float) = 1
		[HDR]_Color_A("Color_A", Color) = (1,0.3088486,0,0)
		[HDR]_Color_B("Color_B", Color) = (1,1,1,0)
		_Opacity("Opacity", Float) = 1
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 uv_tex4coord;
		};

		uniform float4 _Color_A;
		uniform float4 _Color_B;
		uniform sampler2D _Main_Tex;
		uniform float4 _Main_Tex_ST;
		uniform sampler2D _Noise_Tex;
		uniform sampler2D _Normal_Tex;
		uniform float _Normal_UPanner;
		uniform float _Normal_VPanner;
		uniform float4 _Normal_Tex_ST;
		uniform float _Distortion;
		uniform float _Noise_Upanner;
		uniform float _Noise_Vpanner;
		uniform float4 _Noise_Tex_ST;
		uniform float _Noise_Power;
		uniform float _Noise_Ins;
		uniform float _Dissovle;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float4 tex2DNode1 = tex2D( _Main_Tex, uv_Main_Tex );
			float2 appendResult5 = (float2(_Normal_UPanner , _Normal_VPanner));
			float2 uv0_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 panner4 = ( 1.0 * _Time.y * appendResult5 + uv0_Normal_Tex);
			float2 appendResult14 = (float2(_Noise_Upanner , _Noise_Vpanner));
			float2 uv0_Noise_Tex = i.uv_texcoord * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
			float2 panner12 = ( 1.0 * _Time.y * appendResult14 + uv0_Noise_Tex);
			float temp_output_27_0 = saturate( ( pow( tex2D( _Noise_Tex, ( ( (UnpackNormal( tex2D( _Normal_Tex, panner4 ) )).xy * _Distortion ) + panner12 ) ).r , _Noise_Power ) * _Noise_Ins ) );
			float4 lerpResult32 = lerp( _Color_A , _Color_B , saturate( ( tex2DNode1.r * temp_output_27_0 ) ));
			o.Emission = ( lerpResult32 * i.vertexColor ).rgb;
			o.Alpha = ( i.vertexColor.a * saturate( ( ( tex2DNode1.r * ( _Dissovle + i.uv_tex4coord.z + temp_output_27_0 ) ) * _Opacity ) ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
434;1257;1191;663;1500.496;490.9199;2.338513;True;False
Node;AmplifyShaderEditor.CommentaryNode;40;-2791.721,248.6356;Float;False;1507.533;493.5999;;9;7;6;5;3;4;2;11;8;10;Normal_Tex;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-2741.721,626.2355;Float;False;Property;_Normal_VPanner;Normal_VPanner;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-2732.621,429.9351;Float;False;Property;_Normal_UPanner;Normal_UPanner;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-2581.821,298.6356;Float;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;5;-2416.721,557.3353;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;4;-2285.42,448.1354;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;41;-2082.628,785.4753;Float;False;1994.798;517.5281;;12;15;16;14;13;12;18;17;24;26;23;25;27;Noise_Tex;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2031.859,1187.004;Float;False;Property;_Noise_Vpanner;Noise_Vpanner;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2032.628,1011.994;Float;False;Property;_Noise_Upanner;Noise_Upanner;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-2010.619,384.6716;Float;True;Property;_Normal_Tex;Normal_Tex;2;0;Create;True;0;0;False;0;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1881.828,880.6938;Float;False;0;17;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;14;-1716.728,1139.394;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;8;-1696.121,403.9352;Float;False;True;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1724.188,526.1369;Float;False;Property;_Distortion;Distortion;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;12;-1585.427,1030.194;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1446.188,469.1369;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-1091.346,1003.011;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;17;-925.5745,930.5217;Float;True;Property;_Noise_Tex;Noise_Tex;8;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-711.9646,835.4753;Float;False;Property;_Noise_Power;Noise_Power;10;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-514.8304,844.3329;Float;False;Property;_Noise_Ins;Noise_Ins;11;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;23;-593.7034,968.3441;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-388.8303,958.3328;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;27;-252.8304,944.3326;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-617.8827,176.2126;Float;False;Property;_Dissovle;Dissovle;9;0;Create;True;0;0;False;0;1;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;22;-572.7986,311.606;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-307.6677,275.0207;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-588.1321,-15.40596;Float;True;Property;_Main_Tex;Main_Tex;1;0;Create;True;0;0;False;0;989607f8821f46d439a72855606fbdc5;989607f8821f46d439a72855606fbdc5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-117.9744,13.20956;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-76.77182,250.7976;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;84.07059,1020.595;Float;False;Property;_Opacity;Opacity;14;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;34;154.5766,2.19458;Float;False;Property;_Color_B;Color_B;13;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;31;275.5292,250.4547;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;271.6079,892.3134;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;33;151.5766,-160.8054;Float;False;Property;_Color_A;Color_A;12;1;[HDR];Create;True;0;0;False;0;1,0.3088486,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;32;423.5766,30.19458;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;30;470.9731,700.916;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;37;495.3077,915.6141;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;649.7725,897.5161;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;733.3253,202.638;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;891.1595,181.4;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Fire_Ztest;False;False;False;False;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;-1;7;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;False;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;6;0
WireConnection;5;1;7;0
WireConnection;4;0;3;0
WireConnection;4;2;5;0
WireConnection;2;1;4;0
WireConnection;14;0;15;0
WireConnection;14;1;16;0
WireConnection;8;0;2;0
WireConnection;12;0;13;0
WireConnection;12;2;14;0
WireConnection;10;0;8;0
WireConnection;10;1;11;0
WireConnection;18;0;10;0
WireConnection;18;1;12;0
WireConnection;17;1;18;0
WireConnection;23;0;17;1
WireConnection;23;1;24;0
WireConnection;25;0;23;0
WireConnection;25;1;26;0
WireConnection;27;0;25;0
WireConnection;21;0;20;0
WireConnection;21;1;22;3
WireConnection;21;2;27;0
WireConnection;19;0;1;1
WireConnection;19;1;21;0
WireConnection;28;0;1;1
WireConnection;28;1;27;0
WireConnection;31;0;28;0
WireConnection;38;0;19;0
WireConnection;38;1;39;0
WireConnection;32;0;33;0
WireConnection;32;1;34;0
WireConnection;32;2;31;0
WireConnection;37;0;38;0
WireConnection;29;0;30;4
WireConnection;29;1;37;0
WireConnection;36;0;32;0
WireConnection;36;1;30;0
WireConnection;0;2;36;0
WireConnection;0;9;29;0
ASEEND*/
//CHKSM=091C4A847040654A2A9533EDC205C397E09D54CE