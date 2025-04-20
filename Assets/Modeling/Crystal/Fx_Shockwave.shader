// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify Shader/Skill/FX_shockwave"
{
	Properties
	{
		_noiseTex("noiseTex", 2D) = "white" {}
		_noise_Vpanner("noise_Vpanner", Float) = 0
		_noise_Upanner("noise_Upanner", Float) = 0
		_opacity("opacity", Float) = 3.12
		_dissolve("dissolve", Range( 0 , 1)) = 0.4
		_Emi_ins("Emi_ins", Float) = 1
		_Dissolve("Dissolve", Range( -1 , 1)) = 0
		[HDR]_Emi_Color("Emi_Color", Color) = (1,1,1,0)
		[Toggle]_USE_Custom("USE_Custom", Float) = 0
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

		uniform sampler2D _noiseTex;
		uniform float _noise_Upanner;
		uniform float _noise_Vpanner;
		uniform float4 _noiseTex_ST;
		uniform float _Dissolve;
		uniform float _USE_Custom;
		uniform float _dissolve;
		uniform float _Emi_ins;
		uniform float4 _Emi_Color;
		uniform float _opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult5 = (float2(_noise_Upanner , _noise_Vpanner));
			float2 uv0_noiseTex = i.uv_texcoord * _noiseTex_ST.xy + _noiseTex_ST.zw;
			float2 appendResult13 = (float2(uv0_noiseTex.x , ( uv0_noiseTex.x + ( uv0_noiseTex.y * -1.0 ) )));
			float2 panner3 = ( 1.0 * _Time.y * appendResult5 + appendResult13);
			float lerpResult44 = lerp( _Dissolve , i.uv_tex4coord.z , _USE_Custom);
			float temp_output_39_0 = saturate( ( tex2D( _noiseTex, panner3 ).r + lerpResult44 ) );
			float temp_output_19_0 = ( i.uv_texcoord.y + temp_output_39_0 );
			o.Emission = ( ( temp_output_19_0 + (-2.0 + (_dissolve - 0.0) * (1.0 - -2.0) / (1.0 - 0.0)) ) * _Emi_ins * _Emi_Color * i.vertexColor ).rgb;
			o.Alpha = ( i.vertexColor.a * saturate( ( temp_output_39_0 * _opacity * saturate( ( i.uv_texcoord.y + 0.0 ) ) ) ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
1101;807;1028;592;1414.937;407.324;1.381444;True;False
Node;AmplifyShaderEditor.RangedFloatNode;11;-2331.561,-120.7494;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-2342.297,-492.4274;Float;True;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2122.261,-174.0496;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1981.417,-330.0348;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1728.893,-167.6272;Float;False;Property;_noise_Upanner;noise_Upanner;3;0;Create;True;0;0;False;0;0;-0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1730.893,-55.6272;Float;False;Property;_noise_Vpanner;noise_Vpanner;2;0;Create;True;0;0;False;0;0;0.89;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;13;-1752.33,-473.6271;Float;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;5;-1432.893,-141.6272;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1051.464,288.4672;Float;False;Property;_USE_Custom;USE_Custom;11;1;[Toggle];Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;3;-1271.292,-280.7272;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;43;-1162.464,57.46722;Float;True;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;-1203.006,-83.04559;Float;False;Property;_Dissolve;Dissolve;9;0;Create;True;0;0;False;0;0;-0.109;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1062.293,-302.8273;Float;True;Property;_noiseTex;noiseTex;1;0;Create;True;0;0;False;0;c7d564bbc661feb448e7dcb86e2aa438;ff881a9bfd56a1f45b2262c56b7cfede;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;44;-823.464,-34.53278;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-786.1098,-792.9282;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;40;-696.3762,-436.7709;Float;False;Constant;_Float4;Float 4;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-702.5859,-250.6252;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-553.6762,-622.3708;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-176.5694,-509.8489;Float;False;Property;_dissolve;dissolve;7;0;Create;True;0;0;False;0;0.4;0.58;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-25.8311,-319.9268;Float;False;Constant;_Float3;Float 3;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-314.9672,-49.48708;Float;False;Property;_opacity;opacity;4;0;Create;True;0;0;False;0;3.12;2.69;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;42;-269.6762,-342.3709;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;39;-541.0063,-236.0456;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-51.9912,-406.1708;Float;False;Constant;_Float2;Float 2;7;0;Create;True;0;0;False;0;-2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;26;132.0688,-473.3319;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-213.7481,-751.8374;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-80.35497,-231.8773;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;564.5815,-811.0003;Float;False;Property;_Emi_ins;Emi_ins;8;0;Create;True;0;0;False;0;1;1.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;17;194.0829,-229.3099;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;34;489.6412,-418.4724;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;27;351.7917,-745.0887;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;33;565.0673,-1039.835;Float;False;Property;_Emi_Color;Emi_Color;10;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;23;172.014,-1404.751;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;20;-36.59775,-1400.343;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;271.9127,-1516.402;Float;False;Property;_emi_ins;emi_ins;6;0;Create;True;0;0;False;0;4.6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-20.43774,-1554.599;Float;False;Property;_emi_Dissolve;emi_Dissolve;5;0;Create;True;0;0;False;0;-0.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;689.7007,-248.3253;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;807.9969,-701.3054;Float;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;443.7972,-1410.627;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1041.607,-436.831;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Amplify Shader/Skill/FX_shockwave;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;8;2
WireConnection;10;1;11;0
WireConnection;14;0;8;1
WireConnection;14;1;10;0
WireConnection;13;0;8;1
WireConnection;13;1;14;0
WireConnection;5;0;6;0
WireConnection;5;1;7;0
WireConnection;3;0;13;0
WireConnection;3;2;5;0
WireConnection;1;1;3;0
WireConnection;44;0;37;0
WireConnection;44;1;43;3
WireConnection;44;2;46;0
WireConnection;36;0;1;1
WireConnection;36;1;44;0
WireConnection;41;0;18;2
WireConnection;41;1;40;0
WireConnection;42;0;41;0
WireConnection;39;0;36;0
WireConnection;26;0;28;0
WireConnection;26;3;29;0
WireConnection;26;4;30;0
WireConnection;19;0;18;2
WireConnection;19;1;39;0
WireConnection;15;0;39;0
WireConnection;15;1;16;0
WireConnection;15;2;42;0
WireConnection;17;0;15;0
WireConnection;27;0;19;0
WireConnection;27;1;26;0
WireConnection;23;0;20;0
WireConnection;23;1;22;0
WireConnection;20;0;19;0
WireConnection;35;0;34;4
WireConnection;35;1;17;0
WireConnection;31;0;27;0
WireConnection;31;1;32;0
WireConnection;31;2;33;0
WireConnection;31;3;34;0
WireConnection;24;0;23;0
WireConnection;24;1;25;0
WireConnection;0;2;31;0
WireConnection;0;9;35;0
ASEEND*/
//CHKSM=5727150E478FC15CD34A5C86D9572C44A6BF8BB8