Shader "Unlit/MetalballUnliShader"
{
	Properties
	{
		_R("R", float) = 1.0
		_Gravity("Gravity", float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 localVertex : COLOR0;
			};

			uniform float4 _Particles[1000];
			uniform int _Particles_Length = 10;
			float _R;
			float _Gravity;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.localVertex = v.vertex;
				return o;
			}

			float3 hsv2rgb(float3 c) {
              c = float3(c.x, clamp(c.yz, 0.0, 1.0));
              float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
              float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
              return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
            }
			
			fixed4 frag (v2f i) : SV_Target
			{
				half sum = 0;
				for (int it = 0; it < _Particles_Length; it++) {
					float len = length(i.vertex.xyz - _Particles[it].xyz);
					float value;
					if (len < _R) {
						value = 1;
					}
					else {
						 value = _Gravity / (len - _R);
					}
					sum += value;
				}
				sum = clamp(sum, 0.3, 1);
				fixed4 col = float4(hsv2rgb(float3(sum, 1, 1)), 1);
				return col;
			}

			

			ENDCG
		}
	}
}
