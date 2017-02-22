Shader "Custom/Jeks-GradientAlpha"
{
	Properties
	// Add and configure shader properties
	// Property name should start with an underscore followed by a capital letter, and lowercase so nothing else uses this convention
	// ...which prevents accidental duplicate names.
	{
		_ColorTop ("Top Color", Color) = (1, 1, 1, 1) // String name "Tint" with a type Color
		_ColorMid ("Mid Color", Color) = (1, 1, 1, 1)
		_ColorBot ("Bot Color", Color) = (1, 1, 1, 1)
		_Middle ("Middle", Range(0.001, 0.9999)) = 1
		_MainTex ("Texture", 2D) = "white" {} // Use Texture to the shader property
	}

	SubShader 
	// Group multiple shader variants together -- Ex. One subshader for Mobile, one for Desktop
	// Has to contain at least one pass. Pass is where an object actually gets rendered.
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "LightMode" = "ForwardBase" }

		Pass 
		{
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha
			// Blend SrcAlpha One

			CGPROGRAM
			// Consists of two programs: 
			// The vertex program responsible for processing the vertex data of mesh (shows vertices)
			// The fragment program responsible for coloring individual pixels that lie inside the mesh (shows pixel colors)

			#pragma vertex myVertexProgram
			#pragma fragment myFragmentProgram
			// tell the compiler which program to use, via pragma directives.

			// #include "UnityCG.cginc"
			#include "UnityStandardBRDF.cginc"
			// loads unity essential files and contains some generic functionality.

			fixed4 _ColorTop; // Add color variable so we can use the property applied
			fixed4 _ColorMid;
			fixed4 _ColorBot;
			float _Middle;
			sampler2D _MainTex; // Add access to the Texture to the shader
			float4 _MainTex_ST; // Add access to the Texture to the shader & Tiling and Offset

			struct Interpolators 
			{
				float4 position : SV_POSITION; 
				// Vertex program has to return the coordinates of a vertex, that's why we use float4
				// (float4 position : POSITION) vertex program has to produce a correct vertex position

				// float3 localPosition : TEXCOORD0;
				// (.. float3 localPosition) interpolation process, float3 because we only need X, Y, Z 
				// Use semantics TEXCOORD0 to tell the compiler how to interpret this data

				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				// We can make the UV coordinates visible, just like the local position, by interpreting them as color channels..
				// ...thus we replace the local position
				// Use semantics TEXCOORD0 to tell the compiler how to interpret this data
			};

			struct VertexData 
			{
				float4 position : POSITION;
				// Since we're trying to output the position of the vertex. We have to attach POSITION semantic...
				// ... from SV_POSITION, stands for SV = system value, POSITION = final vertex position
				float2 uv : TEXCOORD0;
				// float2 uv : TEXCOORD0, unity's default meshes have UV coordinates suitable for texture mapping
			};

			Interpolators myVertexProgram (VertexData v)
			{
				Interpolators i;
				/// i.localPosition = v.position.xyz; // Replaced by i.uv = v.uv;
				// To pass the data through the vertex program
				// The vertex program has to output the local position
				i.position = mul(UNITY_MATRIX_MVP, v.position); 
				// mul for multiply, correctly project our mesh onto the display.
				/// i.uv = v.uv * _MainTex_ST.xy; // multiply _MainTex_ST to use tiling and offset
				/// i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw; // Enable Offset portion add + .zw
				i.uv = TRANSFORM_TEX(v.uv, _MainTex); // shorthand macro for enabling Offset and Tiling
				return i;
			}

			fixed4 myFragmentProgram (Interpolators i) : SV_TARGET
			// The fragment program is supposed to output RGBA color value for one pixel, we change it to float4
			// Indicate where the final color should be written. Use SV_TARGET which is the default shader target
			// (Interpolators i) the output of the vertex program is used as input for the fragment program
			{
				// return float4 (i.uv, 1, 1);
				/// return tex2D (_MainTex, i.uv); // Sampling the texture with the UV coordinates is done in the fragment program, by using the tex2D function.
				// float4 fragProgram = tex2D(_MainTex, i.uv) * _ColorTop * _ColorMid;
				float4 color;
				color.rgb = i.color.rgb;
				color = lerp (_ColorBot, _ColorMid, i.uv.y / _Middle) * step(i.uv.y, _Middle);
				color += lerp (_ColorMid, _ColorTop, (i.uv.y - _Middle) / (1 - _Middle)) * step (_Middle, i.uv.y);
				color.a;
				// color.a = tex2D (_MainTex, i.uv).a * i.color.a;
				return color;
			}
			ENDCG
		}
	}
}