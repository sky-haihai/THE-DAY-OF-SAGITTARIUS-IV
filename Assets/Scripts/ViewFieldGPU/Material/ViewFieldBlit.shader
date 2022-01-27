// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
//Shader "ViewFieldBlit"
Shader /*ase_name*/ "Xihe/FovObjectUnlit" /*end*/
{
	Properties
	{
		//_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
	LOD 100
		Cull Off
		

		Pass
		{
			CGPROGRAM
			
			#pragma target 3.0 
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				
			};

			//uniform sampler2D _MainTex;
			uniform fixed4 _Color;
			
			
			v2f vert ( appdata v )
			{
				v2f o;
				o.texcoord.xy = v.texcoord.xy;
				o.texcoord.zw = v.texcoord1.xy;
				
				// ase common template code
				
				
				o.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				//fixed4 myColorVar;
				// ase common template code
				
				
				//myColorVar = fixed4(1,0,0,1);
				return _Color;
			}
			ENDCG
		}
	}
	
	CustomEditor "ASEMaterialInspector"
	
}/*ASEBEGIN
Version=18934
363;73;826;499;-348.455;127.6188;1;True;False
Node;AmplifyShaderEditor.SimpleAddOpNode;63;-2550.815,809.2515;Inherit;False;2;2;0;INT;0;False;1;INT;1;False;1;INT;0
Node;AmplifyShaderEditor.SamplerNode;75;171.4985,-148.8339;Inherit;True;Property;_BattleCamRT;BattleCamRT;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwizzleNode;28;-2031.284,608.7582;Inherit;False;FLOAT2;0;2;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GlobalArrayNode;71;-2294.155,1290.024;Inherit;False;ShipPositions;0;4;2;False;False;0;1;False;Object;-1;4;0;INT;0;False;2;INT;0;False;1;INT;0;False;3;INT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-2581.69,1257.015;Inherit;False;2;2;0;INT;0;False;1;INT;3;False;1;INT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;38;-1832.151,1324.715;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.34,-0.59;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwizzleNode;34;-2034.975,1086.748;Inherit;False;FLOAT2;0;2;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.IntNode;62;-3021.831,881.9213;Inherit;False;Constant;_Index;Index;2;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.GlobalArrayNode;30;-2299.43,839.7036;Inherit;False;ShipPositions;0;4;2;False;False;0;1;False;Object;-1;4;0;INT;0;False;2;INT;0;False;1;INT;0;False;3;INT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.IntNode;73;-2639.072,948.1385;Inherit;False;Constant;_Int0;Int 0;2;0;Create;True;0;0;0;False;0;False;5;0;False;0;1;INT;0
Node;AmplifyShaderEditor.LengthOpNode;6;-1574.838,487.0774;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GlobalArrayNode;27;-2301.479,609.7095;Inherit;False;ShipPositions;0;5;2;False;False;0;1;False;Object;-1;4;0;INT;0;False;2;INT;0;False;1;INT;0;False;3;INT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;16;-394.5485,926.8898;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;182.8446,90.35935;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;18;578.387,-79.65258;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;64;-2580.964,1063.21;Inherit;False;2;2;0;INT;0;False;1;INT;2;False;1;INT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1855.616,519.374;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.3,-0.3;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;10;-707.2191,1090.94;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-1848.467,1057.126;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.34,-0.59;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;7;-1334.15,810.6004;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-183.438,847.4368;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;36;-1559.865,1347.673;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;32;-1338.524,1069.243;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;31;-1576.181,1080.084;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;29;-2044.28,826.8618;Inherit;False;FLOAT2;0;2;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;11;-1338.173,561.8881;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.15;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;37;-1322.208,1336.832;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.GlobalArrayNode;35;-2310.053,1085.848;Inherit;False;ShipPositions;0;4;2;False;False;0;1;False;Object;-1;4;0;INT;0;False;2;INT;0;False;1;INT;0;False;3;INT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LengthOpNode;12;-1571.807,821.4417;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1844.093,798.4835;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.34,-0.59;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-448.4977,1200.996;Inherit;False;Property;_FogDensity;FogDensity;2;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-985.8239,1084.677;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;72;-2022.477,1341.224;Inherit;False;FLOAT2;0;2;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;82;868.7539,159.5645;Float;False;True;-1;2;ASEMaterialInspector;100;11;ViewFieldBlit;e94913272661e2e4bae6388aae22e59e;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;63;0;62;0
WireConnection;28;0;27;0
WireConnection;71;0;70;0
WireConnection;71;1;73;0
WireConnection;70;0;62;0
WireConnection;38;1;72;0
WireConnection;34;0;35;0
WireConnection;30;0;63;0
WireConnection;30;1;73;0
WireConnection;6;0;5;0
WireConnection;27;0;62;0
WireConnection;27;1;73;0
WireConnection;16;0;10;0
WireConnection;18;0;75;0
WireConnection;18;1;13;0
WireConnection;64;0;62;0
WireConnection;5;1;28;0
WireConnection;10;0;8;0
WireConnection;33;1;34;0
WireConnection;7;0;12;0
WireConnection;17;0;16;0
WireConnection;17;1;19;0
WireConnection;36;0;38;0
WireConnection;32;0;31;0
WireConnection;31;0;33;0
WireConnection;29;0;30;0
WireConnection;11;0;6;0
WireConnection;37;0;36;0
WireConnection;35;0;64;0
WireConnection;35;1;73;0
WireConnection;12;0;9;0
WireConnection;9;1;29;0
WireConnection;8;0;11;0
WireConnection;8;1;7;0
WireConnection;8;2;32;0
WireConnection;8;3;37;0
WireConnection;72;0;71;0
ASEEND*/
//CHKSM=53C2E05F10FBC2E1DC6BC4ABDBBEC44221D28D14