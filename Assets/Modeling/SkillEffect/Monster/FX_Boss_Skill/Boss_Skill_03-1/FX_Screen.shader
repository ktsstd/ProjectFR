// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "amplifyShader/Skill/FX_Screen"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Alpha("Alpha", Float) = 1
		_Scale("Scale", Float) = 1
		_Color0("Color 0", Color) = (1,1,1,1)
		_TilingOffset("TilingOffset", Vector) = (1,1,0,0)
		_Mask_Tiling_Offset("Mask_Tiling_Offset", Vector) = (1,1,0,0)
		_Panner("Panner", Vector) = (1,1,0,0)
		_RGBA_Mask("RGBA_Mask", Vector) = (1,0,0,0)
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		_Mask_Scale("Mask_Scale", Float) = 1
		[Toggle(_USE_CUSTOM_ON)] _USE_Custom("USE_Custom", Float) = 0
		[Enum(OFF,0,ON,1)]_UV("UV", Int) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _USE_CUSTOM_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float4 screenPos;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Main_Tex;
		uniform float4 _Panner;
		uniform int _UV;
		uniform float4 _TilingOffset;
		uniform float4 _Color0;
		uniform float _Scale;
		uniform sampler2D _Mask_Tex;
		uniform float4 _Mask_Tiling_Offset;
		uniform float _Mask_Scale;
		uniform float _Alpha;
		uniform float4 _RGBA_Mask;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime7 = _Time.y * _Panner.z;
			float2 appendResult8 = (float2(_Panner.x , _Panner.y));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 appendResult5 = (float2(ase_screenPosNorm.x , ase_screenPosNorm.y));
			float2 appendResult33 = (float2(ase_screenPosNorm.x , ase_screenPosNorm.y));
			float2 temp_output_34_0 = (appendResult33*2.0 + -1.0);
			float2 break36 = temp_output_34_0;
			float2 appendResult39 = (float2(length( temp_output_34_0 ) , (0.0 + (atan2( break36.y , break36.x ) - 0.0) * (1.0 - 0.0) / (3.141593 - 0.0))));
			float2 lerpResult42 = lerp( appendResult5 , appendResult39 , (float)_UV);
			float2 appendResult4 = (float2(_TilingOffset.x , _TilingOffset.y));
			float2 appendResult6 = (float2(_TilingOffset.z , _TilingOffset.w));
			float2 panner11 = ( ( mulTime7 + _Panner.w ) * appendResult8 + (lerpResult42*appendResult4 + appendResult6));
			float4 tex2DNode13 = tex2D( _Main_Tex, panner11 );
			float2 appendResult27 = (float2(_Mask_Tiling_Offset.x , _Mask_Tiling_Offset.y));
			float2 appendResult29 = (float2(_Mask_Tiling_Offset.z , _Mask_Tiling_Offset.w));
			#ifdef _USE_CUSTOM_ON
				float staticSwitch40 = ( ( _RGBA_Mask.x * tex2DNode13.r ) + ( _RGBA_Mask.y * tex2DNode13.g ) + ( _RGBA_Mask.z * tex2DNode13.b ) + ( _RGBA_Mask.w * tex2DNode13.a ) );
			#else
				float staticSwitch40 = 1.0;
			#endif
			float temp_output_23_0 = ( ( tex2D( _Mask_Tex, (appendResult5*appendResult27 + appendResult29) ).r * _Mask_Scale ) * _Alpha * staticSwitch40 * i.vertexColor.a );
			float4 appendResult24 = (float4(( tex2DNode13 * _Color0 * _Scale ).rgb , temp_output_23_0));
			o.Emission = ( appendResult24 * i.vertexColor ).xyz;
			o.Alpha = temp_output_23_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
0;794;905;566;1838.208;484.9266;3.014781;True;False
Node;AmplifyShaderEditor.ScreenPosInputsNode;1;-3502.144,-110.5275;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;33;-3251.988,0.8134003;Float;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;34;-3031.571,9.239715;Float;True;3;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;36;-3339.205,247.9275;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ATan2OpNode;37;-3056.432,237.3987;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;35;-2766.451,9.705851;Float;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;38;-2825.409,283.5761;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;3.141593;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;43;-2303.942,-227.45;Float;False;Property;_UV;UV;12;1;[Enum];Create;True;2;OFF;0;ON;1;0;True;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.Vector4Node;3;-1811.844,264.7735;Float;False;Property;_Panner;Panner;7;0;Create;True;0;0;False;0;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;39;-2633.833,280.976;Float;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;2;-2182.42,213.1907;Float;False;Property;_TilingOffset;TilingOffset;5;0;Create;True;0;0;False;0;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;5;-2503.409,-67.48515;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;7;-1605.309,315.2431;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;-1995.413,271.0952;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;4;-1993.093,164.4608;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;42;-2030.846,-205.4532;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;9;-1719.223,-59.28342;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-1589.688,180.0526;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-1416.584,330.9701;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;26;-1318.846,812.4319;Float;False;Property;_Mask_Tiling_Offset;Mask_Tiling_Offset;6;0;Create;True;0;0;False;0;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;11;-1476.05,-148.8597;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;27;-1049.519,767.7021;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;12;-1047.999,304.2713;Float;False;Property;_RGBA_Mask;RGBA_Mask;8;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-1209.971,-206.1672;Float;True;Property;_Main_Tex;Main_Tex;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;29;-1029.839,946.3365;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-696.9442,497.5286;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-718.2202,376.9643;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-719.9932,56.05072;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-705.8092,219.1671;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;30;-877.649,726.9578;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-456.222,114.7136;Float;False;Constant;_Float0;Float 0;12;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;-596.5074,663.1836;Float;True;Property;_Mask_Tex;Mask_Tex;9;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;32;-535.7579,955.3399;Float;False;Property;_Mask_Scale;Mask_Scale;10;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-461.8994,205.2051;Float;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;19;-1106.204,-10.72078;Float;False;Property;_Color0;Color 0;4;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;40;-299.281,296.3419;Float;False;Property;_USE_Custom;USE_Custom;11;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1018.732,149.3331;Float;False;Property;_Scale;Scale;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-299.0739,837.5495;Float;False;Property;_Alpha;Alpha;2;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-306.0691,663.4165;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;44;-51.54528,246.1398;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-822.567,-140.3267;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-45.15702,626.0491;Float;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;-242.1584,-83.0914;Float;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;75.4809,-54.71187;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;267.0602,-91.04433;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;amplifyShader/Skill/FX_Screen;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;7;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;33;0;1;1
WireConnection;33;1;1;2
WireConnection;34;0;33;0
WireConnection;36;0;34;0
WireConnection;37;0;36;1
WireConnection;37;1;36;0
WireConnection;35;0;34;0
WireConnection;38;0;37;0
WireConnection;39;0;35;0
WireConnection;39;1;38;0
WireConnection;5;0;1;1
WireConnection;5;1;1;2
WireConnection;7;0;3;3
WireConnection;6;0;2;3
WireConnection;6;1;2;4
WireConnection;4;0;2;1
WireConnection;4;1;2;2
WireConnection;42;0;5;0
WireConnection;42;1;39;0
WireConnection;42;2;43;0
WireConnection;9;0;42;0
WireConnection;9;1;4;0
WireConnection;9;2;6;0
WireConnection;8;0;3;1
WireConnection;8;1;3;2
WireConnection;10;0;7;0
WireConnection;10;1;3;4
WireConnection;11;0;9;0
WireConnection;11;2;8;0
WireConnection;11;1;10;0
WireConnection;27;0;26;1
WireConnection;27;1;26;2
WireConnection;13;1;11;0
WireConnection;29;0;26;3
WireConnection;29;1;26;4
WireConnection;17;0;12;4
WireConnection;17;1;13;4
WireConnection;16;0;12;3
WireConnection;16;1;13;3
WireConnection;14;0;12;1
WireConnection;14;1;13;1
WireConnection;15;0;12;2
WireConnection;15;1;13;2
WireConnection;30;0;5;0
WireConnection;30;1;27;0
WireConnection;30;2;29;0
WireConnection;25;1;30;0
WireConnection;21;0;14;0
WireConnection;21;1;15;0
WireConnection;21;2;16;0
WireConnection;21;3;17;0
WireConnection;40;1;41;0
WireConnection;40;0;21;0
WireConnection;31;0;25;1
WireConnection;31;1;32;0
WireConnection;22;0;13;0
WireConnection;22;1;19;0
WireConnection;22;2;18;0
WireConnection;23;0;31;0
WireConnection;23;1;20;0
WireConnection;23;2;40;0
WireConnection;23;3;44;4
WireConnection;24;0;22;0
WireConnection;24;3;23;0
WireConnection;45;0;24;0
WireConnection;45;1;44;0
WireConnection;0;2;45;0
WireConnection;0;9;23;0
ASEEND*/
//CHKSM=645398A9F030744CD8936579AB69A527B4AA9EBE