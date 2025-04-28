// Made with Amplify Shader Editor v1.9.8.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DC/Doublecats_R_common_shader"
{
	Properties
	{
		[NoScaleOffset]_Base_Tex("Base_Tex", 2D) = "white" {}
		_Base_uv("Base_uv", Vector) = (1,1,0,0)
		_Base_speed1("Base_speed", Vector) = (1,0,0,0)
		[Toggle(_USE_CUSTOM1_ZW_MOVE_ON)] _Use_custom1_zw_move("Use_custom1_zw_move", Float) = 0
		[HDR]_R_light_color("R_light_color", Color) = (1,0.92775,0.6462264,0)
		[HDR]_R_dark_clolor("R_dark_clolor", Color) = (0.1509434,0.07064492,0.03203988,0)
		_Constart("Constart", Range( 0.45 , 3)) = 1
		[HDR]_Color("Color", Color) = (1,1,1,0)
		_Color_power("Color_power", Range( 0 , 4)) = 1
		_Distance("Distance", Range( 0 , 4)) = 2.635294
		_Alpha("Alpha", Range( 0 , 3)) = 1
		[NoScaleOffset]_Dissolve_Tex("Dissolve_Tex", 2D) = "white" {}
		_Hardness("Hardness", Range( 0 , 22)) = 11
		_Dissolve("Dissolve", Range( 0 , 1)) = 1
		[Toggle(_USE_CUSTOM1_X_DISSOLVE_ON)] _use_custom1_x_dissolve("use_custom1_x_dissolve", Float) = 0
		_Distort_Tex("Distort_Tex", 2D) = "bump" {}
		_Distort("Distort", Range( -2 , 2)) = 0
		[Toggle(_USE_CUSTOM1_Y_DISTORT_ON)] _use_custom1_y_distort("use_custom1_y_distort", Float) = 0
		_Mask("Mask", 2D) = "white" {}
		[Enum(off,0,on,1)]_Zwrite("Zwrite", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)]_Cull("Cull", Float) = 0
		[IntRange][Enum(UnityEngine.Rendering.CompareFunction)]_Ztest("Ztest", Float) = 4
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		

		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull [_Cull]
		ColorMask RGBA
		ZWrite [_Zwrite]
		ZTest [_Ztest]
		Offset 0 , 0
		

		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			#define ASE_VERSION 19801


			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _USE_CUSTOM1_ZW_MOVE_ON
			#pragma shader_feature_local _USE_CUSTOM1_Y_DISTORT_ON
			#pragma shader_feature_local _USE_CUSTOM1_X_DISSOLVE_ON


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _Zwrite;
			uniform float _Cull;
			uniform float _Ztest;
			uniform float4 _Color;
			uniform float4 _R_dark_clolor;
			uniform float4 _R_light_color;
			uniform sampler2D _Base_Tex;
			uniform float4 _Base_speed1;
			uniform float4 _Base_uv;
			uniform sampler2D _Distort_Tex;
			uniform float4 _Distort_Tex_ST;
			uniform float _Distort;
			uniform float _Constart;
			uniform float _Color_power;
			uniform float _Alpha;
			uniform sampler2D _Dissolve_Tex;
			uniform float _Hardness;
			uniform float _Dissolve;
			uniform sampler2D _Mask;
			uniform float4 _Mask_ST;
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float _Distance;


			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 vertexPos138 = v.vertex.xyz;
				float4 ase_positionCS138 = UnityObjectToClipPos( vertexPos138 );
				float4 screenPos138 = ComputeScreenPos( ase_positionCS138 );
				o.ase_texcoord3 = screenPos138;
				
				o.ase_texcoord1 = v.ase_texcoord;
				o.ase_texcoord2 = v.ase_texcoord1;
				o.ase_color = v.color;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}

			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float4 break143 = _Base_speed1;
				float mulTime141 = _Time.y * break143.z;
				float2 appendResult142 = (float2(break143.x , break143.y));
				float2 appendResult97 = (float2(_Base_uv.x , _Base_uv.y));
				float2 appendResult98 = (float2(_Base_uv.z , _Base_uv.w));
				float2 texCoord95 = i.ase_texcoord1.xy * appendResult97 + appendResult98;
				float2 panner140 = ( mulTime141 * appendResult142 + texCoord95);
				float4 texCoord92 = i.ase_texcoord2;
				texCoord92.xy = i.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult90 = (float2(texCoord92.x , texCoord92.y));
				#ifdef _USE_CUSTOM1_ZW_MOVE_ON
				float2 staticSwitch93 = ( panner140 + appendResult90 );
				#else
				float2 staticSwitch93 = panner140;
				#endif
				float2 uv_Distort_Tex = i.ase_texcoord1.xy * _Distort_Tex_ST.xy + _Distort_Tex_ST.zw;
				float3 tex2DNode125 = UnpackNormal( tex2D( _Distort_Tex, uv_Distort_Tex ) );
				float2 appendResult123 = (float2(tex2DNode125.r , tex2DNode125.g));
				float4 texCoord116 = i.ase_texcoord1;
				texCoord116.xy = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				#ifdef _USE_CUSTOM1_Y_DISTORT_ON
				float staticSwitch118 = texCoord116.w;
				#else
				float staticSwitch118 = _Distort;
				#endif
				float2 Distort126 = ( appendResult123 * staticSwitch118 );
				float4 tex2DNode20 = tex2D( _Base_Tex, ( staticSwitch93 + Distort126 ) );
				float4 lerpResult130 = lerp( _R_dark_clolor , _R_light_color , pow( tex2DNode20.r , _Constart ));
				float4 texCoord113 = i.ase_texcoord1;
				texCoord113.xy = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				#ifdef _USE_CUSTOM1_X_DISSOLVE_ON
				float staticSwitch114 = texCoord113.z;
				#else
				float staticSwitch114 = _Dissolve;
				#endif
				float lerpResult107 = lerp( _Hardness , -1.0 , staticSwitch114);
				float2 uv_Mask = i.ase_texcoord1.xy * _Mask_ST.xy + _Mask_ST.zw;
				float4 screenPos138 = i.ase_texcoord3;
				float4 ase_positionSSNorm = screenPos138 / screenPos138.w;
				ase_positionSSNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_positionSSNorm.z : ase_positionSSNorm.z * 0.5 + 0.5;
				float screenDepth138 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_positionSSNorm.xy ));
				float distanceDepth138 = abs( ( screenDepth138 - LinearEyeDepth( ase_positionSSNorm.z ) ) / ( _Distance ) );
				float4 appendResult88 = (float4((( _Color * lerpResult130 * _Color_power * i.ase_color )).rgb , saturate( ( i.ase_color.a * tex2DNode20.a * _Alpha * saturate( ( ( tex2D( _Dissolve_Tex, ( float2( 0,0 ) + Distort126 ) ).r * _Hardness ) - lerpResult107 ) ) * tex2D( _Mask, uv_Mask ).r * saturate( distanceDepth138 ) ) )));
				

				finalColor = appendResult88;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
	

	Fallback "True"
}
/*ASEBEGIN
Version=19801
Node;AmplifyShaderEditor.Vector4Node;144;-2728.793,-350.7201;Inherit;False;Property;_Base_speed1;Base_speed;2;0;Create;True;0;0;0;False;0;False;1,0,0,0;0,0.2,0.2,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;96;-2317.88,-762.0789;Inherit;False;Property;_Base_uv;Base_uv;1;0;Create;True;0;0;0;False;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;117;-2767.667,451.3081;Inherit;False;Property;_Distort;Distort;18;0;Create;True;0;0;0;False;0;False;0;0;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;116;-2684.846,609.9512;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;125;-3070.814,205.2634;Inherit;True;Property;_Distort_Tex;Distort_Tex;17;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.DynamicAppendNode;97;-2078.88,-775.0789;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;98;-2095.88,-557.0792;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;143;-2533.275,-329.3654;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;123;-2599.246,248.6011;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;118;-2361.784,486.9801;Inherit;False;Property;_use_custom1_y_distort;use_custom1_y_distort;19;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;142;-2139.123,-368.3029;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;141;-2032.123,-264.3029;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;95;-1898.88,-706.079;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;92;-2079.563,-138.4554;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;120;-2157.85,225.0249;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;140;-1826.123,-420.3029;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;90;-1651.025,-119.3887;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;126;-1970.598,180.2661;Inherit;False;Distort;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;149;-1408.53,849.4296;Inherit;False;126;Distort;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;91;-1432.025,-102.3887;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;113;-350.2611,1085.784;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;110;-615.7635,831.8236;Inherit;False;Property;_Dissolve;Dissolve;15;0;Create;True;0;0;0;False;0;False;1;0.576;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;145;-1143.478,742.5842;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;93;-1309.95,-328.4825;Inherit;False;Property;_Use_custom1_zw_move;Use_custom1_zw_move;3;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;99;-541.7515,371.2687;Inherit;True;Property;_Dissolve_Tex;Dissolve_Tex;11;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;b062c05e1c2fc7742a5bf63e9a9afcd6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;111;-371.5546,595.4609;Inherit;False;Property;_Hardness;Hardness;14;0;Create;True;0;0;0;False;0;False;11;11;0;22;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;127;-1005.449,-92.44879;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;114;-64.20009,934.1133;Inherit;False;Property;_use_custom1_x_dissolve;use_custom1_x_dissolve;16;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;136;356.4235,735.0688;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-87.90869,475.0484;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;107;-83.53351,722.2306;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;133;-589.4782,-159.7908;Inherit;False;Property;_Constart;Constart;6;0;Create;True;0;0;0;False;0;False;1;1;0.45;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;137;615.4236,909.0699;Inherit;False;Property;_Distance;Distance;9;0;Create;True;0;0;0;False;0;False;2.635294;0;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;20;-965.2712,-352.2458;Inherit;True;Property;_Base_Tex;Base_Tex;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;b062c05e1c2fc7742a5bf63e9a9afcd6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleSubtractOpNode;106;154.4665,484.2305;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;134;-392.2093,-373.0392;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;131;-736.7756,-664.9689;Inherit;False;Property;_R_dark_clolor;R_dark_clolor;5;1;[HDR];Create;True;0;0;0;False;0;False;0.1509434,0.07064492,0.03203988,0;0.7490196,0.3621354,0.345098,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.DepthFade;138;693.65,731.8209;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;132;-772.1206,-835.1239;Inherit;False;Property;_R_light_color;R_light_color;4;1;[HDR];Create;True;0;0;0;False;0;False;1,0.92775,0.6462264,0;32,23.18929,22.11518,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SaturateNode;108;347.3645,456.7877;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;42;12.78459,-642.9928;Inherit;False;Property;_Color;Color;7;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;1,0.3921568,0.3983661,0;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SaturateNode;139;973.1312,732.7918;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-119.1539,-298.591;Inherit;False;Property;_Color_power;Color_power;8;0;Create;True;0;0;0;False;0;False;1;4;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;30;-426.2529,-39.49192;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;129;250.8176,1082.969;Inherit;True;Property;_Mask;Mask;20;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;89;-244.3707,181.4384;Inherit;False;Property;_Alpha;Alpha;10;0;Create;True;0;0;0;False;0;False;1;3;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;130;-276.4476,-517.0469;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;856.2496,272.7903;Inherit;False;6;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;390.8635,-366.5229;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SwizzleNode;33;516.004,-252.5605;Inherit;False;FLOAT3;0;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;35;1010.638,287.6171;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;77;1144.94,-818.4952;Inherit;False;345.9991;319.9341;Comment;3;75;76;135;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;146;-1731.645,483.5063;Inherit;False;Property;_Dissolve_uv1;Dissolve_uv;12;0;Create;True;0;0;0;False;0;False;1,1,0,0;1.5,1.5,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;147;-1693.646,676.5063;Inherit;False;Property;_Dissolve_speed1;Dissolve_speed;13;0;Create;True;0;0;0;False;0;False;1,0,0,0;1,0,-0.1,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;75;1313.94,-737.4952;Inherit;False;Property;_Zwrite;Zwrite;21;1;[Enum];Create;True;0;2;off;0;on;1;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;88;895.9367,-259.1533;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;76;1325.338,-613.9609;Inherit;False;Property;_Cull;Cull;22;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;135;1197.546,-583.0758;Inherit;False;Property;_Ztest;Ztest;23;2;[IntRange];[Enum];Create;True;0;0;1;UnityEngine.Rendering.CompareFunction;True;0;False;4;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;148;-1417.488,616.2664;Inherit;False;uv_speed;-1;;56;;0;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1455.764,-236.2216;Float;False;True;-1;2;AmplifyShaderEditor.MaterialInspector;100;5;DC/Doublecats_R_common_shader;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;2;5;False;;10;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;True;_Cull;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;1;True;_Zwrite;True;3;True;_Ztest;True;True;0;False;;0;False;;True;2;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;True;2;=;=;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
WireConnection;97;0;96;1
WireConnection;97;1;96;2
WireConnection;98;0;96;3
WireConnection;98;1;96;4
WireConnection;143;0;144;0
WireConnection;123;0;125;1
WireConnection;123;1;125;2
WireConnection;118;1;117;0
WireConnection;118;0;116;4
WireConnection;142;0;143;0
WireConnection;142;1;143;1
WireConnection;141;0;143;2
WireConnection;95;0;97;0
WireConnection;95;1;98;0
WireConnection;120;0;123;0
WireConnection;120;1;118;0
WireConnection;140;0;95;0
WireConnection;140;2;142;0
WireConnection;140;1;141;0
WireConnection;90;0;92;1
WireConnection;90;1;92;2
WireConnection;126;0;120;0
WireConnection;91;0;140;0
WireConnection;91;1;90;0
WireConnection;145;1;149;0
WireConnection;93;1;140;0
WireConnection;93;0;91;0
WireConnection;99;1;145;0
WireConnection;127;0;93;0
WireConnection;127;1;126;0
WireConnection;114;1;110;0
WireConnection;114;0;113;3
WireConnection;109;0;99;1
WireConnection;109;1;111;0
WireConnection;107;0;111;0
WireConnection;107;2;114;0
WireConnection;20;1;127;0
WireConnection;106;0;109;0
WireConnection;106;1;107;0
WireConnection;134;0;20;1
WireConnection;134;1;133;0
WireConnection;138;1;136;0
WireConnection;138;0;137;0
WireConnection;108;0;106;0
WireConnection;139;0;138;0
WireConnection;130;0;131;0
WireConnection;130;1;132;0
WireConnection;130;2;134;0
WireConnection;31;0;30;4
WireConnection;31;1;20;4
WireConnection;31;2;89;0
WireConnection;31;3;108;0
WireConnection;31;4;129;1
WireConnection;31;5;139;0
WireConnection;32;0;42;0
WireConnection;32;1;130;0
WireConnection;32;2;43;0
WireConnection;32;3;30;0
WireConnection;33;0;32;0
WireConnection;35;0;31;0
WireConnection;88;0;33;0
WireConnection;88;3;35;0
WireConnection;0;0;88;0
ASEEND*/
//CHKSM=AD6D2A26D53ABA76C27E1E84089A9DAF963A2711