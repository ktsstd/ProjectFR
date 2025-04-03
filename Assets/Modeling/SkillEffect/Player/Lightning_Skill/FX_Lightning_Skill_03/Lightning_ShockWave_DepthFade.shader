// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Lightning_Shock_Wave_DepthFade"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Main_Power("Main_Power", Float) = 11
		_Main_Ins("Main_Ins", Float) = 1
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
		_Normal_Tex("Normal_Tex", 2D) = "white" {}
		_Normal_UPanner("Normal_UPanner", Float) = 0
		_Normal_VPanner("Normal_VPanner", Float) = 0
		_Distortion("Distortion", Range( 0 , 1)) = 0
		_Chroma("Chroma", Range( -0.1 , 0.1)) = 0
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Noise_Texture("Noise_Texture", 2D) = "white" {}
		_Noise_UPanner("Noise_UPanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Depth_Fade("Depth_Fade", Float) = 0
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
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform sampler2D _Noise_Texture;
		uniform float _Noise_UPanner;
		uniform float _Noise_VPanner;
		uniform float4 _Noise_Texture_ST;
		uniform sampler2D _Main_Tex;
		uniform sampler2D _Normal_Tex;
		uniform float _Normal_UPanner;
		uniform float _Normal_VPanner;
		uniform float4 _Normal_Tex_ST;
		uniform float _Distortion;
		uniform float4 _Main_Tex_ST;
		uniform float _Chroma;
		uniform float _Main_Power;
		uniform float _Main_Ins;
		uniform float4 _Main_Color;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth_Fade;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult56 = (float2(_Noise_UPanner , _Noise_VPanner));
			float2 uv0_Noise_Texture = i.uv_texcoord * _Noise_Texture_ST.xy + _Noise_Texture_ST.zw;
			float2 panner57 = ( 1.0 * _Time.y * appendResult56 + uv0_Noise_Texture);
			float2 appendResult10 = (float2(_Normal_UPanner , _Normal_VPanner));
			float2 uv0_Normal_Tex = i.uv_texcoord * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
			float2 panner9 = ( 1.0 * _Time.y * appendResult10 + uv0_Normal_Tex);
			float2 uv0_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float2 appendResult33 = (float2(uv0_Main_Tex.x , ( uv0_Main_Tex.y + i.uv_tex4coord.z )));
			float4 temp_output_54_0 = ( ( (tex2D( _Normal_Tex, panner9 )).rgba * _Distortion ) + float4( appendResult33, 0.0 , 0.0 ) );
			float4 temp_cast_5 = (_Chroma).xxxx;
			float4 appendResult43 = (float4(tex2D( _Main_Tex, ( temp_output_54_0 + _Chroma ).rg ).r , tex2D( _Main_Tex, temp_output_54_0.rg ).g , tex2D( _Main_Tex, ( temp_output_54_0 - temp_cast_5 ).rg ).b , 0.0));
			float4 temp_cast_7 = (_Main_Power).xxxx;
			o.Emission = ( ( pow( saturate( ( tex2D( _Noise_Texture, panner57 ).r * appendResult43 ) ) , temp_cast_7 ) * _Main_Ins ) * _Main_Color * i.vertexColor ).xyz;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth65 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth65 = abs( ( screenDepth65 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth_Fade ) );
			o.Alpha = ( i.vertexColor.a * saturate( tex2D( _TextureSample2, uv_TextureSample2 ).r ) * saturate( distanceDepth65 ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
1055;801;1015;550;2662.592;308.0892;3.678268;True;False
Node;AmplifyShaderEditor.CommentaryNode;51;-3398.191,-693.9963;Float;False;1467.84;447.9999;Comment;9;11;8;10;9;4;5;6;7;12;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-3344.191,-360.9964;Float;False;Property;_Normal_VPanner;Normal_VPanner;7;0;Create;True;0;0;False;0;0;-0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-3348.191,-461.9965;Float;False;Property;_Normal_UPanner;Normal_UPanner;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-3268.191,-643.9963;Float;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;10;-3159.191,-430.9965;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;9;-3016.191,-595.9963;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;-2699.424,-613.0498;Float;True;Property;_Normal_Tex;Normal_Tex;5;0;Create;True;0;0;False;0;007b42d9a05209b4e8748c66b6775974;d8a54a2140770eb4c99af0add3f00190;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;52;-2799.179,239.5525;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-2796.916,-120.9104;Float;False;0;39;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-2668.664,-379.1984;Float;False;Property;_Distortion;Distortion;8;0;Create;True;0;0;False;0;0;0.3296553;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-2527.822,59.9046;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;5;-2399.424,-608.0497;Float;False;True;True;True;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;33;-2340.55,-98.48917;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-2093.685,-595.8231;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-2080.24,154.1477;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-2097.618,513.1176;Float;False;Property;_Chroma;Chroma;9;0;Create;True;0;0;False;0;0;0;-0.1;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-1621.835,-396.1253;Float;False;Property;_Noise_VPanner;Noise_VPanner;13;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-1656.574,-557.0217;Float;False;Property;_Noise_UPanner;Noise_UPanner;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;56;-1367.692,-571.6486;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;47;-1724.051,-730.9103;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;-1600.452,496.8413;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-1590.095,88.4548;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;39;-1563.633,-115.7903;Float;True;Property;_Main_Tex;Main_Tex;1;0;Create;True;0;0;False;0;None;e531d2537fad080479f6e6483e79baa5;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;57;-1179.37,-644.7833;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;38;-1115.45,466.3411;Float;True;Property;_TextureSample1;Texture Sample 1;13;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;-1121.071,203.5425;Float;True;Property;_TextureSample0;Texture Sample 0;12;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;25;-1082.727,-27.69561;Float;True;Property;_Noise_Tex;Noise_Tex;9;0;Create;True;0;0;False;0;None;b21114d325524b24ebff225951b0e2fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;43;-723.7154,158.05;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;1;-879.4551,-582.2584;Float;True;Property;_Noise_Texture;Noise_Texture;11;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-568.8406,-392.6746;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-632.139,574.4584;Float;False;Property;_Depth_Fade;Depth_Fade;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;27;-488.9719,-177.0627;Float;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-375.5421,-368.2455;Float;False;Property;_Main_Power;Main_Power;2;0;Create;True;0;0;False;0;11;22.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;65;-439.7662,550.1075;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;62;-456.7839,254.7202;Float;True;Property;_TextureSample2;Texture Sample 2;10;0;Create;True;0;0;False;0;3558b1add6f592a49934055f74406e0c;691cf5219e52ce443b5c0f65fd59fd59;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;-198.1905,-385.695;Float;False;Property;_Main_Ins;Main_Ins;3;0;Create;True;0;0;False;0;1;3.71;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;21;-318.3893,-244.4962;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;61;-33.87467,260.6711;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;16;-121.177,-21.78245;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;64;-60.03135,-533.8137;Float;False;Property;_Main_Color;Main_Color;4;1;[HDR];Create;True;0;0;False;0;1,1,1,0;2.696386,3.482202,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-92.64709,-252.2883;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;67;-179.2108,535.4968;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;167.8699,162.8555;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;90.6731,-255.0956;Float;False;3;3;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;350.4216,-131;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Lightning_Shock_Wave_DepthFade;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;8;5;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;9;0;8;0
WireConnection;9;2;10;0
WireConnection;4;1;9;0
WireConnection;53;0;29;2
WireConnection;53;1;52;3
WireConnection;5;0;4;0
WireConnection;33;0;29;1
WireConnection;33;1;53;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;54;0;6;0
WireConnection;54;1;33;0
WireConnection;56;0;58;0
WireConnection;56;1;59;0
WireConnection;41;0;54;0
WireConnection;41;1;42;0
WireConnection;40;0;54;0
WireConnection;40;1;42;0
WireConnection;57;0;47;0
WireConnection;57;2;56;0
WireConnection;38;0;39;0
WireConnection;38;1;41;0
WireConnection;37;0;39;0
WireConnection;37;1;54;0
WireConnection;25;0;39;0
WireConnection;25;1;40;0
WireConnection;43;0;25;1
WireConnection;43;1;37;2
WireConnection;43;2;38;3
WireConnection;1;1;57;0
WireConnection;24;0;1;1
WireConnection;24;1;43;0
WireConnection;27;0;24;0
WireConnection;65;0;66;0
WireConnection;21;0;27;0
WireConnection;21;1;22;0
WireConnection;61;0;62;1
WireConnection;63;0;21;0
WireConnection;63;1;23;0
WireConnection;67;0;65;0
WireConnection;60;0;16;4
WireConnection;60;1;61;0
WireConnection;60;2;67;0
WireConnection;19;0;63;0
WireConnection;19;1;64;0
WireConnection;19;2;16;0
WireConnection;0;2;19;0
WireConnection;0;9;60;0
ASEEND*/
//CHKSM=F74A6C1489471FFD61EC1C8CF59BE7C061229B3A