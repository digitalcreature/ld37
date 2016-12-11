Shader "Unlit/Cable" {
	Properties {
		_Color ("Color", Color) = (1, 1, 1, 1)
		_BorderColor ("Border Color", Color) = (0, 0, 0, 1)
		_BorderWidth ("Border Width", Float) = 0.1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"


			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			fixed4 _Color;
			fixed4 _BorderColor;
			float _BorderWidth;

			v2f vert (appdata_base v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				float t = i.uv.y;
				if (t > 0.5) t = 1 - t;
				t = smoothstep(_BorderWidth - 0.05, _BorderWidth + 0.05, t);
				return _BorderColor + t * (_Color - _BorderColor);
			}

			ENDCG
		}
	}
}
