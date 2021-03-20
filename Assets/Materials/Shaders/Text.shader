Shader "Scholl's Viewport/Text" 
{
	Properties 
    {
        _MainTex ("Font Texture", 2D) = "white" {}
        _Color ("Text Color", Color) = (1,1,1,1)
    }
 
    SubShader {
                
        Pass 
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
 
            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 tex_coord : TEXCOORD0;
            };
 
            struct v2_f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 tex_coord : TEXCOORD0;
            };
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
 
            v2_f vert (const appdata_t v)
            {
                v2_f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color * _Color;
                o.tex_coord = TRANSFORM_TEX(v.tex_coord, _MainTex);
                return o;
            }
 
            fixed4 frag (const v2_f i) : SV_Target
            {
                fixed4 col = i.color;
                col.a *= tex2D(_MainTex, i.tex_coord).a;
                return col;
            }
            
            ENDCG
        }
    }
}