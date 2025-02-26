// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "GUI/TextCurve"
{
	Properties {
        _MainTex ("Font Texture", 2D) = "white" {}
        _Color ("Text Color", Color) = (1,1,1,1)
		_OffSetV ("_OffSetV", Vector) = (0,0,0)
		_Dist("_Dist", float) = 60
    }
 
    SubShader {
 
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting Off Cull Off ZTest Always ZWrite Off Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha
 
        Pass { 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
 
            #include "UnityCG.cginc"
 
            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
 
            sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform float4 _Color;
			fixed3 _OffSetV;
			float _Dist;

			float4 getNewVertPosition( float4 p )
			{
				float3 pos = mul (unity_ObjectToWorld, p.xyz).xyz;
				float off = UnityObjectToViewPos(p.xyz).z/_Dist;
				pos += _OffSetV*off*off;
				p.xyz = mul (unity_WorldToObject, pos);
				return p;
			}

            v2f vert (appdata_t v)
            {
                v2f o;
				float4 position = getNewVertPosition( v.vertex );
				v.vertex.xyz = position.xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                return o;
            }
 
            half4 frag (v2f i) : COLOR
            {
                float4 col = _Color;
                col.a *= tex2D(_MainTex, i.texcoord).a;
                return col;
            }
            ENDCG
        }
    }  
 
    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting Off Cull Off ZTest Always ZWrite Off Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            Color [_Color]
            SetTexture [_MainTex] {
                combine primary, texture * primary
            }
        }
    }
}
