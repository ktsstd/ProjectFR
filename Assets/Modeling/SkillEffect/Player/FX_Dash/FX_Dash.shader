// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Dash"
{
	Properties
	{
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_Noise_UPanner("Noise_UPanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Dissovle("Dissovle", Range( 0 , 1)) = 1
		_Noise_Power("Noise_Power", Float) = 1
		_Noise_Ins("Noise_Ins", Float) = 1
		[HDR]_Noise_Color("Noise_Color", Color) = (1,1,1,0)
		_Opacity("Opacity", Float) = 0
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Noise_Tex;
		uniform float _Noise_UPanner;
		uniform float _Noise_VPanner;
		uniform float4 _Noise_Tex_ST;
		uniform float _Dissovle;
		uniform float _Noise_Power;
		uniform float _Noise_Ins;
		uniform float4 _Noise_Color;
		uniform float _Opacity;
		uniform sampler2D _Mask_Tex;
		uniform float4 _Mask_Tex_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult4 = (float2(_Noise_UPanner , _Noise_VPanner));
			float2 uv0_Noise_Tex = i.uv_texcoord * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
			float2 panner3 = ( 1.0 * _Time.y * appendResult4 + uv0_Noise_Tex);
			float temp_output_9_0 = saturate( ( tex2D( _Noise_Tex, panner3 ).r + _Dissovle ) );
			o.Emission = ( ( pow( temp_output_9_0 , _Noise_Power ) * _Noise_Ins ) * _Noise_Color * i.vertexColor ).rgb;
			float2 uv0_Mask_Tex = i.uv_texcoord * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
			o.Alpha = ( i.vertexColor.a * saturate( pow( temp_output_9_0 , _Opacity ) ) * tex2D( _Mask_Tex, uv0_Mask_Tex ).r );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
0;733;1096;618;610.8694;141.0086;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;5;-1362.995,142.3975;Float;False;Property;_Noise_UPanner;Noise_UPanner;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1323.994,294.4976;Float;False;Property;_Noise_VPanner;Noise_VPanner;3;0;Create;True;0;0;False;0;0;2.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;4;-1123.794,163.1975;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1333.094,-33.10251;Float;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;3;-1026.294,42.2975;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-787.094,-20.10254;Float;True;Property;_Noise_Tex;Noise_Tex;1;0;Create;True;0;0;False;0;7dafa5b81fffb8a41aaed27d6d1e7b58;4e3fbc6478c01904481a44939521c54b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-765.9809,199.1047;Float;False;Property;_Dissovle;Dissovle;4;0;Create;True;0;0;False;0;1;0.54;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-470.881,18.10467;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-295.6195,-158.5585;Float;False;Property;_Noise_Power;Noise_Power;5;0;Create;True;0;0;False;0;1;-1.37;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;9;-333.3196,22.14153;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-365.8195,262.6414;Float;False;Property;_Opacity;Opacity;8;0;Create;True;0;0;False;0;0;9.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;17;-183.8194,145.6414;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-88.91962,-188.4585;Float;False;Property;_Noise_Ins;Noise_Ins;6;0;Create;True;0;0;False;0;1;3.21;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;10;-134.4197,-67.55849;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-499.7195,535.6415;Float;False;0;22;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;14;48.88041,-287.2585;Float;False;Property;_Noise_Color;Noise_Color;7;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0.9977055,1,0.02352941,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;16;108.6804,61.14145;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;19;-23.91954,291.2414;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;89.18045,-58.45851;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-189.0194,607.1414;Float;True;Property;_Mask_Tex;Mask_Tex;9;0;Create;True;0;0;False;0;None;d78129b0d4c0c7b4b99804b92bd05e39;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;366.0804,248.3414;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;297.1804,-64.9585;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;708.2999,-175.7;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Dash;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;5;0
WireConnection;4;1;6;0
WireConnection;3;0;2;0
WireConnection;3;2;4;0
WireConnection;1;1;3;0
WireConnection;7;0;1;1
WireConnection;7;1;8;0
WireConnection;9;0;7;0
WireConnection;17;0;9;0
WireConnection;17;1;18;0
WireConnection;10;0;9;0
WireConnection;10;1;11;0
WireConnection;19;0;17;0
WireConnection;12;0;10;0
WireConnection;12;1;13;0
WireConnection;22;1;21;0
WireConnection;20;0;16;4
WireConnection;20;1;19;0
WireConnection;20;2;22;1
WireConnection;15;0;12;0
WireConnection;15;1;14;0
WireConnection;15;2;16;0
WireConnection;0;2;15;0
WireConnection;0;9;20;0
ASEEND*/
//CHKSM=F20FF6BB34B78A2C11820F351FBC304E67F0218A