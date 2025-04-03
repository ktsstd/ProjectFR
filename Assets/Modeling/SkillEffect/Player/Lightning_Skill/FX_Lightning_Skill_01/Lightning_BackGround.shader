// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Lightning_BackGround"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		_Tilling("Tilling", Vector) = (3,3,0,0)
		_TRi_UPanner("TRi_UPanner", Float) = 0.02
		_TRi_VPanner("TRi_VPanner", Float) = 0.01
		_Parallax_Offset("Parallax_Offset", Float) = 103.8
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_Noise_Upanner("Noise_Upanner", Float) = -0.2
		_Noise_Vpanner("Noise_Vpanner", Float) = 0
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		_Opacity("Opacity", Float) = 2.16
		_Main_Power("Main_Power", Float) = 2.84
		_Main_Ins("Main_Ins", Float) = 13.57
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
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
		#include "UnityCG.cginc"
		#pragma target 3.0
		#define ASE_TEXTURE_PARAMS(textureName) textureName

		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv_tex4coord;
		};

		uniform float4 _Main_Color;
		uniform sampler2D _Texture0;
		uniform float2 _Tilling;
		uniform float _TRi_UPanner;
		uniform float _TRi_VPanner;
		uniform float _Main_Power;
		uniform float _Main_Ins;
		uniform float _Parallax_Offset;
		uniform sampler2D _Noise_Tex;
		uniform float _Noise_Upanner;
		uniform float _Noise_Vpanner;
		uniform float4 _Noise_Tex_ST;
		uniform sampler2D _Mask_Tex;
		uniform float4 _Mask_Tex_ST;
		uniform float _Opacity;


		inline float4 TriplanarSamplingSF( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
			yNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			zNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float2 appendResult10 = (float2(_TRi_UPanner , _TRi_VPanner));
			float4 triplanar4 = TriplanarSamplingSF( _Texture0, ( ase_worldViewDir + float3( ( _Time.y * appendResult10 ) ,  0.0 ) ), ase_worldNormal, 1.0, _Tilling, 1.0, 0 );
			float4 temp_cast_1 = (_Main_Power).xxxx;
			float2 paralaxOffset13 = ParallaxOffset( 0 , _Parallax_Offset , ( float3( appendResult10 ,  0.0 ) + ase_worldViewDir ) );
			float4 temp_cast_3 = (_Main_Power).xxxx;
			o.Emission = ( _Main_Color * ( ( pow( triplanar4 , temp_cast_1 ) * _Main_Ins ) + ( pow( tex2D( _Texture0, paralaxOffset13 ) , temp_cast_3 ) * _Main_Ins ) ) * i.vertexColor ).rgb;
			float2 appendResult20 = (float2(_Noise_Upanner , _Noise_Vpanner));
			float2 uv0_Noise_Tex = i.uv_texcoord * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
			float2 panner19 = ( 1.0 * _Time.y * appendResult20 + uv0_Noise_Tex);
			float2 uv_Mask_Tex = i.uv_texcoord * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
			o.Alpha = ( i.vertexColor.a * saturate( ( ( tex2D( _Noise_Tex, panner19 ).r + i.uv_tex4coord.z ) * tex2D( _Mask_Tex, uv_Mask_Tex ).r * _Opacity ) ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
549.3334;794;902;558;879.5572;-174.531;1.692111;True;False
Node;AmplifyShaderEditor.RangedFloatNode;11;-1357.107,-269.7687;Float;False;Property;_TRi_UPanner;TRi_UPanner;3;0;Create;True;0;0;False;0;0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1351.722,-164.2354;Float;False;Property;_TRi_VPanner;TRi_VPanner;4;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1059.703,721.7651;Float;False;Property;_Noise_Upanner;Noise_Upanner;7;0;Create;True;0;0;False;0;-0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-1155.731,-255.7695;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;9;-1100.665,-398.9931;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;16;-1158.259,459.5397;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;22;-1083.126,813.6568;Float;False;Property;_Noise_Vpanner;Noise_Vpanner;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1033.893,180.9889;Float;False;Property;_Parallax_Offset;Parallax_Offset;5;0;Create;True;0;0;False;0;103.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;7;-827.1396,-546.5245;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-868.0609,-356.9954;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;20;-789.4333,788.4316;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-989.4326,565.0086;Float;False;0;17;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-890.2284,309.6041;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;19;-681.3252,474.9187;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-845.0764,-121.1516;Float;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;False;0;b55077122da18094b814d3538643a710;b55077122da18094b814d3538643a710;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-612.8424,-423.7616;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ParallaxOffsetHlpNode;13;-616.6165,230.3034;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;5;-637.879,-290.216;Float;False;Property;_Tilling;Tilling;2;0;Create;True;0;0;False;0;3,3;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TriplanarNode;4;-423.2342,-414.674;Float;True;Spherical;World;False;Top Texture 0;_TopTexture0;white;-1;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-79.55139,-560.597;Float;False;Property;_Main_Power;Main_Power;11;0;Create;True;0;0;False;0;2.84;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-328.0809,262.5069;Float;True;Property;_Noise_Tex;Noise_Tex;6;0;Create;True;0;0;False;0;8d21b35fab1359d4aa689ddf302e1b01;8d21b35fab1359d4aa689ddf302e1b01;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;24;-283.7164,871.9222;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-315.2979,-80.68237;Float;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;30;117.5649,-465.1675;Float;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;26;-367.6785,559.2897;Float;True;Property;_Mask_Tex;Mask_Tex;9;0;Create;True;0;0;False;0;6ea4737d63deadf4794ffc97c3188b9f;6ea4737d63deadf4794ffc97c3188b9f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;23;24.60196,295.5274;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;28;134.7734,-45.9046;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;32;301.3519,-597.3766;Float;False;Property;_Main_Ins;Main_Ins;12;0;Create;True;0;0;False;0;13.57;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;57.82772,59.5676;Float;False;Property;_Opacity;Opacity;10;0;Create;True;0;0;False;0;2.16;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;257.3862,247.6642;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;452.9099,-57.09407;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;483.6375,-456.5634;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;692.2567,-277.4578;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;35;520.1432,-758.9399;Float;False;Property;_Main_Color;Main_Color;13;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;37;681.3633,-96.62958;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;38;587.6814,254.1335;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;860.0129,-262.207;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;879.6208,103.8065;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1112.083,-136.2219;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Lightning_BackGround;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;8;0;9;0
WireConnection;8;1;10;0
WireConnection;20;0;21;0
WireConnection;20;1;22;0
WireConnection;15;0;10;0
WireConnection;15;1;16;0
WireConnection;19;0;18;0
WireConnection;19;2;20;0
WireConnection;6;0;7;0
WireConnection;6;1;8;0
WireConnection;13;1;14;0
WireConnection;13;2;15;0
WireConnection;4;0;1;0
WireConnection;4;9;6;0
WireConnection;4;3;5;0
WireConnection;17;1;19;0
WireConnection;3;0;1;0
WireConnection;3;1;13;0
WireConnection;30;0;4;0
WireConnection;30;1;29;0
WireConnection;23;0;17;1
WireConnection;23;1;24;3
WireConnection;28;0;3;0
WireConnection;28;1;29;0
WireConnection;25;0;23;0
WireConnection;25;1;26;1
WireConnection;25;2;27;0
WireConnection;33;0;28;0
WireConnection;33;1;32;0
WireConnection;31;0;30;0
WireConnection;31;1;32;0
WireConnection;34;0;31;0
WireConnection;34;1;33;0
WireConnection;38;0;25;0
WireConnection;36;0;35;0
WireConnection;36;1;34;0
WireConnection;36;2;37;0
WireConnection;39;0;37;4
WireConnection;39;1;38;0
WireConnection;0;2;36;0
WireConnection;0;9;39;0
ASEEND*/
//CHKSM=67A443A9A39CCB37DD569B3F85B637C74D3FC35A