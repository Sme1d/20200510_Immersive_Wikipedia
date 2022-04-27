// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,)' with 'UnityObjectToClipPos()'

Shader "ShaderLearning/PostProcessing_BlurEffect"
{
	Properties
	{
		//MainTex is the keywork lol :D
		_MainTex("Basic Texture", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_BlurStrength("Blur Strength", float) = 0
	}


		Subshader
		{

			  Pass
				{

					CGPROGRAM

					 #pragma vertex vert
					 #pragma fragment frag


				   sampler2D _MainTex;
				   half4 _Color;
				   float _BlurStrength;

					struct vertexInput
					{
						float4 vertex: POSITION;
						float2 uv: TEXCOORD0;
					};

					struct vertexOutput
					{
						float4 pos : SV_POSITION;
						float2 uv: TEXCOORD0;
					};

					vertexOutput vert(vertexInput v)
					{
						vertexOutput o;
						o.pos = UnityObjectToClipPos(v.vertex);
						o.uv = v.uv;
						return o;
					}


					fixed4 frag(vertexOutput i) : SV_TARGET
					{
						float4 col = 0;
						//iterate over blur samples
						for (float index = 0; index < 10; index++) {
							//get uv coordinate of sample
							float2 uv = i.uv + float2(0, (index / 9 - 0.5) * _BlurStrength);
							//add color at position to color
							col += tex2D(_MainTex, uv);
						}
						//divide the sum of values by the amount of samples
						col = col / 10;


						//black and white effect
						col = (col.r + col.g + col.b) / 3;
						//col = float4(0.5,0.5,0.5,0.5);
						return col;

						/*//tint

						float3 rendTex = tex2D(_MainTex, i.uv).rgb;
						float3 tintColor = fixed3(0.6, 0.6, 0.6);
						float3 greyScale = (rendTex.r + rendTex.g + rendTex.b) / 3;
						float3 tinted = greyScale * tintColor;

						return fixed4(tinted * 10, 1);*/

					}

				   ENDCG
			   }

		}
}


