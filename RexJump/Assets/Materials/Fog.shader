// - no lighting
 // - no lightmap support
 // - no per-material color
 
 Shader "Unlit/Fog" 
 {
	 Properties 
	 {
	 	_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	 }
	SubShader 
	{

		Tags { "RenderType"="Opaque" }

		LOD 100
		     
		Pass 
		{  


			CGPROGRAM
			// #pragma surface surf Lambert vertex myVertexProgram
			#pragma vertex myVertexProgram
			#pragma fragment myFragmentProgram
			#pragma multi_compile_fog
             
			#include "UnityCG.cginc"
 
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			struct appdata_t // appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f // v2f
			{
				float4 vertex : SV_POSITION; 
				float2 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};

			v2f myVertexProgram (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
             
			fixed4 myFragmentProgram (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord) * _Color;
				UNITY_APPLY_FOG(i.fogCoord, col);
				UNITY_OPAQUE_ALPHA(col.a);
				return col;
			}
			ENDCG
		}
	}
 
}