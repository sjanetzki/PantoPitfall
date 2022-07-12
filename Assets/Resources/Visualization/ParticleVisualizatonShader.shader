// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ParticleVisualizatonShader"
{
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
	 }
	 
    SubShader {
	    Tags { "Queue"="Transparent" }
	     
		Pass {
		    Stencil {
		        Ref 2
		        Comp NotEqual
		        Pass Replace
		    }

            Blend SrcAlpha OneMinusSrcAlpha     
    
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
            
            struct appdata_t {
                fixed4 color : COLOR;
                float4 vertex : POSITION;
                half2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                half4 pos : POSITION;
                half2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };
            
            v2f vert(appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                half2 uv = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord );
                o.uv = uv;
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : COLOR {
                half4 finalColor = i.color;
                finalColor.a = min(tex2D(_MainTex, i.uv).a, i.color.a);
                if (finalColor.a == 0.0)
                    discard;
                return finalColor;
            }
            ENDCG
		}

	}
	Fallback off
}