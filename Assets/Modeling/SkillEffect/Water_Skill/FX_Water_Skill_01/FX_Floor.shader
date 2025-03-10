// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Floor"
{
	Properties
	{
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_Color_Offset("Color_Offset", Float) = 3.46
		_Color_Range("Color_Range", Float) = 1.35
		[HDR]_Color_A("Color_A", Color) = (0.3915094,0.6156207,1,0)
		[HDR]_Color_B("Color_B", Color) = (0,0.1493411,1,0)
		_Main_Power("Main_Power", Float) = 1
		_Noise_UPanner("Noise_UPanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Main_Ins("Main_Ins", Float) = 1
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		[Toggle(_USE_DISSOLVE_ON)] _USE_Dissolve("USE_Dissolve", Float) = 0
		_Dissovle("Dissovle", Range( 0 , 1)) = -1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
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
		#pragma shader_feature _USE_DISSOLVE_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
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
		uniform float _Color_Offset;
		uniform float _Color_Range;
		uniform sampler2D _Noise_Tex;
		uniform float _Noise_UPanner;
		uniform float _Noise_VPanner;
		uniform float4 _Noise_Tex_ST;
		uniform float _Main_Power;
		uniform float _Main_Ins;
		uniform float4 _Main_Color;
		uniform sampler2D _Mask_Tex;
		uniform float4 _Mask_Tex_ST;
		uniform float _Dissovle;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 lerpResult10 = lerp( _Color_A , _Color_B , saturate( ( saturate( pow( ( 1.0 - i.uv_texcoord.y ) , _Color_Offset ) ) * _Color_Range ) ));
			float2 appendResult20 = (float2(_Noise_UPanner , _Noise_VPanner));
			float2 uv0_Noise_Tex = i.uv_texcoord * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
			float2 panner18 = ( 1.0 * _Time.y * appendResult20 + uv0_Noise_Tex);
			float4 tex2DNode1 = tex2D( _Noise_Tex, panner18 );
			o.Emission = ( ( lerpResult10 + ( ( pow( tex2DNode1.r , _Main_Power ) * _Main_Ins ) * _Main_Color ) ) * i.vertexColor ).rgb;
			float2 uv0_Mask_Tex = i.uv_texcoord * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
			#ifdef _USE_DISSOLVE_ON
				float staticSwitch59 = i.uv_tex4coord.z;
			#else
				float staticSwitch59 = _Dissovle;
			#endif
			o.Alpha = ( i.vertexColor.a * ( tex2D( _Mask_Tex, uv0_Mask_Tex ).r * saturate( ( saturate( pow( ( 1.0 - ( i.uv_texcoord.y + 0.0 ) ) , 2.32 ) ) * 5.0 ) ) * ( tex2DNode1.r + staticSwitch59 ) ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
756;1010;1221;680;1753.171;153.7401;1.222584;True;False
Node;AmplifyShaderEditor.RangedFloatNode;21;-1592.421,217.7222;Float;False;Property;_Noise_VPanner;Noise_VPanner;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;46;-1709.414,1013.797;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-1601.421,93.72221;Float;False;Property;_Noise_UPanner;Noise_UPanner;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1652.382,-443.2596;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-1405.086,1024.016;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-1523.421,-92.27779;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;20;-1398.421,52.72222;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1425.673,-199.1812;Float;False;Property;_Color_Offset;Color_Offset;2;0;Create;True;0;0;False;0;3.46;1.83;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;3;-1417.082,-438.0592;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;4;-1225.682,-440.3595;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1165.004,1270.665;Float;False;Constant;_Float0;Float 0;16;0;Create;True;0;0;False;0;2.32;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;18;-1267.421,-76.27779;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;48;-1181.105,1022.969;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1039.636,-96.19837;Float;True;Property;_Noise_Tex;Noise_Tex;1;0;Create;True;0;0;False;0;7ead229491461f64fb8abd86dfcd7d4b;c2f5e06ce5d539b418dc5ebfbfeeee94;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-995.6725,-242.1812;Float;False;Property;_Color_Range;Color_Range;3;0;Create;True;0;0;False;0;1.35;1.78;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;6;-970.6726,-435.1813;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;49;-969.3245,1020.492;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-666.3494,114.2147;Float;False;Property;_Main_Power;Main_Power;6;0;Create;True;0;0;False;0;1;43.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;60;-1383.949,401.3132;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;15;-528.3494,-56.78535;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-777.3609,1270.665;Float;False;Constant;_Float1;Float 1;16;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-1388.84,265.6064;Float;False;Property;_Dissovle;Dissovle;13;0;Create;True;0;0;False;0;-1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-804.6725,-445.1813;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-458.0726,119.6714;Float;False;Property;_Main_Ins;Main_Ins;9;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;51;-738.9678,1022.969;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-301.2286,-56.86345;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-573.012,1029.161;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;13;-692.3711,-658.3011;Float;False;Property;_Color_B;Color_B;5;1;[HDR];Create;True;0;0;False;0;0,0.1493411,1,0;0,0.4103832,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;59;-1085.639,305.9515;Float;False;Property;_USE_Dissolve;USE_Dissolve;12;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;9;-596.0455,-439.1306;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;12;-663.3711,-851.3011;Float;False;Property;_Color_A;Color_A;4;1;[HDR];Create;True;0;0;False;0;0.3915094,0.6156207,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-1241.514,800.1553;Float;False;0;33;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;28;-285.0721,91.76954;Float;False;Property;_Main_Color;Main_Color;10;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;54;-395.9096,876.8289;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;33;-706.5505,619.2646;Float;True;Property;_Mask_Tex;Mask_Tex;11;0;Create;True;0;0;False;0;c10d6fdf8a415d240a0c2bad633abd52;c10d6fdf8a415d240a0c2bad633abd52;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;10;-329.3711,-528.3011;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-118.072,-54.23044;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;57;-693.1897,257.0483;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;29;-242.1489,299.7696;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-31.23394,-182.3542;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-300.5781,629.4302;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;137.8511,102.7696;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;74.01917,480.0015;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;394,-85;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Floor;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;47;0;46;2
WireConnection;20;0;19;0
WireConnection;20;1;21;0
WireConnection;3;0;2;2
WireConnection;4;0;3;0
WireConnection;4;1;5;0
WireConnection;18;0;17;0
WireConnection;18;2;20;0
WireConnection;48;0;47;0
WireConnection;1;1;18;0
WireConnection;6;0;4;0
WireConnection;49;0;48;0
WireConnection;49;1;50;0
WireConnection;15;0;1;1
WireConnection;15;1;16;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;51;0;49;0
WireConnection;23;0;15;0
WireConnection;23;1;24;0
WireConnection;52;0;51;0
WireConnection;52;1;53;0
WireConnection;59;1;58;0
WireConnection;59;0;60;3
WireConnection;9;0;7;0
WireConnection;54;0;52;0
WireConnection;33;1;45;0
WireConnection;10;0;12;0
WireConnection;10;1;13;0
WireConnection;10;2;9;0
WireConnection;27;0;23;0
WireConnection;27;1;28;0
WireConnection;57;0;1;1
WireConnection;57;1;59;0
WireConnection;14;0;10;0
WireConnection;14;1;27;0
WireConnection;34;0;33;1
WireConnection;34;1;54;0
WireConnection;34;2;57;0
WireConnection;31;0;14;0
WireConnection;31;1;29;0
WireConnection;32;0;29;4
WireConnection;32;1;34;0
WireConnection;0;2;31;0
WireConnection;0;9;32;0
ASEEND*/
//CHKSM=C07E2BD594EE679F15A32536A1A140131649BD3C