// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Fire_Wind"
{
	Properties
	{
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		[HDR]_Noise_Color("Noise_Color", Color) = (1,1,1,0)
		_Noise_Power("Noise_Power", Float) = 1
		_Noise_INs("Noise_INs", Float) = 1
		_Noise_Upanner("Noise_Upanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Normal_UPanner("Normal_UPanner", Float) = 0
		_Normal_VPanner("Normal_VPanner", Float) = 0
		_Distortion("Distortion", Range( 0 , 1)) = 0
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		_Gradation_Tex("Gradation_Tex", 2D) = "white" {}
		[Toggle(_USE_CUSTOM_ON)] _USE_CUSTOM("USE_CUSTOM", Float) = 0
		[Toggle(_USE_GRADATION_TEX_ON)] _USE_Gradation_Tex("USE_Gradation_Tex", Float) = 0
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
		#pragma shader_feature _USE_GRADATION_TEX_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 uv_tex4coord;
		};

		uniform sampler2D _Noise_Tex;
		uniform float _Noise_Upanner;
		uniform float _Noise_VPanner;
		uniform float4 _Noise_Tex_ST;
		uniform float _Noise_Power;
		uniform float _Noise_INs;
		uniform float4 _Noise_Color;
		uniform sampler2D _Mask_Tex;
		uniform sampler2D _Normal_Tex;
		uniform float _Normal_UPanner;
		uniform float _Normal_VPanner;
		uniform float4 _Normal_Tex_ST;
		uniform float _Distortion;
		uniform float4 _Mask_Tex_ST;
		uniform sampler2D _Gradation_Tex;
		uniform float4 _Gradation_Tex_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult8 = (float2(_Noise_Upanner , _Noise_VPanner));
			float2 uv0_Noise_Tex = i.uv_texcoord * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
			float2 panner7 = ( 1.0 * _Time.y * appendResult8 + uv0_Noise_Tex);
			o.Emission = ( ( ( pow( tex2D( _Noise_Tex, panner7 ).r , _Noise_Power ) * _Noise_INs ) * _Noise_Color ) * i.vertexColor ).rgb;
			float2 appendResult42 = (float2(_Normal_UPanner , _Normal_VPanner));
			float2 uv0_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 panner44 = ( 1.0 * _Time.y * appendResult42 + uv0_Normal_Tex);
			float2 uv0_Mask_Tex = i.uv_texcoord * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
			float2 appendResult49 = (float2(( uv0_Mask_Tex.x + i.uv_tex4coord.z ) , uv0_Mask_Tex.y));
			#ifdef _USE_CUSTOM_ON
				float2 staticSwitch47 = appendResult49;
			#else
				float2 staticSwitch47 = uv0_Mask_Tex;
			#endif
			float4 temp_cast_1 = (saturate( ( saturate( pow( ( 1.0 - ( i.uv_texcoord.x + 0.0 ) ) , 3.0 ) ) * 5.0 ) )).xxxx;
			float2 uv_Gradation_Tex = i.uv_texcoord * _Gradation_Tex_ST.xy + _Gradation_Tex_ST.zw;
			#ifdef _USE_GRADATION_TEX_ON
				float4 staticSwitch59 = tex2D( _Gradation_Tex, uv_Gradation_Tex );
			#else
				float4 staticSwitch59 = temp_cast_1;
			#endif
			o.Alpha = ( i.vertexColor.a * saturate( ( tex2D( _Mask_Tex, ( ( (UnpackNormal( tex2D( _Normal_Tex, panner44 ) )).xy * _Distortion ) + staticSwitch47 ) ).r * staticSwitch59 ) ) ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
0;714;1506;646;2130.88;240.1021;1.239606;True;False
Node;AmplifyShaderEditor.CommentaryNode;62;-3127.492,-325.0041;Float;False;1542.32;475.8137;;9;41;40;42;43;44;3;36;38;37;Normal_Tex;0,1,0.8095481,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;63;-1941.112,390.7038;Float;False;1856.303;556.9361;;11;50;51;52;54;53;56;57;55;58;5;59;Gradation_Tex;0,0.3744197,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-3077.492,-71.09052;Float;False;Property;_Normal_UPanner;Normal_UPanner;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;50;-1891.112,516.7495;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;40;-3059.292,35.80956;Float;False;Property;_Normal_VPanner;Normal_VPanner;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1631.784,548.67;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;64;-1521.656,-153.61;Float;False;1397.776;438.3929;;7;45;46;48;49;39;1;47;Mask_Tex;0.3557911,1,0,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;43;-3074.86,-225.6039;Float;False;0;3;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;42;-2857.793,-88.99045;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;52;-1418.512,538.1161;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1364.012,832.6399;Float;False;Constant;_Float0;Float 0;18;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;44;-2685.661,-275.004;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;60;-1640.638,-930.9681;Float;False;1648.313;563.9791;;12;10;9;6;8;7;2;12;11;14;16;13;15;Noise_Tex;1,0,0,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-1462.556,-44.61725;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;46;-1471.656,82.78288;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;53;-1220.712,539.5399;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-2417.553,-205.918;Float;True;Property;_Normal_Tex;Normal_Tex;7;0;Create;True;0;0;False;0;44116b33e8fe54b4390df94f441c325f;44116b33e8fe54b4390df94f441c325f;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-1239.157,157.8829;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1590.638,-675.4547;Float;False;Property;_Noise_Upanner;Noise_Upanner;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1572.438,-570.1545;Float;False;Property;_Noise_VPanner;Noise_VPanner;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;49;-1093.357,94.48293;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;55;-992.4082,541.7477;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;36;-1982.503,-192.1177;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-1370.938,-694.9546;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1588.007,-831.5681;Float;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;-2063.285,-81.07412;Float;False;Property;_Distortion;Distortion;10;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-1040.913,812.8063;Float;False;Constant;_Float1;Float 1;18;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1748.503,-110.1177;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;7;-1198.807,-880.9681;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-849.813,538.5065;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;47;-955.5566,-30.31727;Float;False;Property;_USE_CUSTOM;USE_CUSTOM;17;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;5;-605.0831,639.0552;Float;True;Property;_Gradation_Tex;Gradation_Tex;12;0;Create;True;0;0;False;0;a7ed44f5af8ebd347a113a44faa83a70;a7ed44f5af8ebd347a113a44faa83a70;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-773.8256,-806.3365;Float;False;Property;_Noise_Power;Noise_Power;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-614.847,-101.2716;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;58;-643.1131,538.5065;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-979.1412,-723.1074;Float;True;Property;_Noise_Tex;Noise_Tex;1;0;Create;True;0;0;False;0;7ead229491461f64fb8abd86dfcd7d4b;7ead229491461f64fb8abd86dfcd7d4b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-444.5472,-103.6101;Float;True;Property;_Mask_Tex;Mask_Tex;11;0;Create;True;0;0;False;0;8b39383068d86684c9f93ba8beb85259;8b39383068d86684c9f93ba8beb85259;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;11;-626.9022,-697.8204;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;59;-382.8085,440.7038;Float;False;Property;_USE_Gradation_Tex;USE_Gradation_Tex;18;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-557.8256,-810.3678;Float;False;Property;_Noise_INs;Noise_INs;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-68.24255,101.4714;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;16;-523.7283,-573.989;Float;False;Property;_Noise_Color;Noise_Color;2;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-394.8257,-704.3678;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-227.3253,-689.4871;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;32;293.6972,-256.3649;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;34;377.0298,13.83807;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;61;-1747.531,-1746.199;Float;False;1798.632;711.4319;;11;17;19;21;22;25;23;24;26;28;29;4;Gradation_Color;0.8349204,0,1,1;0;0
Node;AmplifyShaderEditor.PowerNode;22;-1187.563,-1305.75;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;19;-1430.874,-1287.767;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;4;-214.5652,-1342.625;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;537.9926,-511.5992;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1364.228,-1381.917;Float;False;Property;_Color_Offset;Color_Offset;15;0;Create;True;0;0;False;0;3.06;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;590.3799,-185.0279;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-1697.531,-1324.894;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;28;-473.5458,-1696.199;Float;False;Property;_Color_A;Color_A;13;1;[HDR];Create;True;0;0;False;0;1,0.1593346,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;23;-947.425,-1304.692;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;-505.531,-1476.872;Float;False;Property;_Color_B;Color_B;14;1;[HDR];Create;True;0;0;False;0;1,0.7666305,0.1792453,0;1,0.7666305,0.1792453,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-971.7565,-1389.322;Float;False;Property;_Color_Range;Color_Range;16;0;Create;True;0;0;False;0;1.35;4.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-801.4381,-1304.692;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;26;-539.0397,-1298.67;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;942.1299,-481.1747;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Fire_Wind;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;51;0;50;1
WireConnection;42;0;41;0
WireConnection;42;1;40;0
WireConnection;52;0;51;0
WireConnection;44;0;43;0
WireConnection;44;2;42;0
WireConnection;53;0;52;0
WireConnection;53;1;54;0
WireConnection;3;1;44;0
WireConnection;48;0;45;1
WireConnection;48;1;46;3
WireConnection;49;0;48;0
WireConnection;49;1;45;2
WireConnection;55;0;53;0
WireConnection;36;0;3;0
WireConnection;8;0;9;0
WireConnection;8;1;10;0
WireConnection;37;0;36;0
WireConnection;37;1;38;0
WireConnection;7;0;6;0
WireConnection;7;2;8;0
WireConnection;56;0;55;0
WireConnection;56;1;57;0
WireConnection;47;1;45;0
WireConnection;47;0;49;0
WireConnection;39;0;37;0
WireConnection;39;1;47;0
WireConnection;58;0;56;0
WireConnection;2;1;7;0
WireConnection;1;1;39;0
WireConnection;11;0;2;1
WireConnection;11;1;12;0
WireConnection;59;1;58;0
WireConnection;59;0;5;0
WireConnection;33;0;1;1
WireConnection;33;1;59;0
WireConnection;13;0;11;0
WireConnection;13;1;14;0
WireConnection;15;0;13;0
WireConnection;15;1;16;0
WireConnection;34;0;33;0
WireConnection;22;0;19;0
WireConnection;22;1;21;0
WireConnection;19;0;17;1
WireConnection;4;0;28;0
WireConnection;4;1;29;0
WireConnection;4;2;26;0
WireConnection;31;0;15;0
WireConnection;31;1;32;0
WireConnection;35;0;32;4
WireConnection;35;1;34;0
WireConnection;23;0;22;0
WireConnection;24;0;23;0
WireConnection;24;1;25;0
WireConnection;26;0;24;0
WireConnection;0;2;31;0
WireConnection;0;9;35;0
ASEEND*/
//CHKSM=2D90215AAF9F0672785344DF7CA9CF37873DC614