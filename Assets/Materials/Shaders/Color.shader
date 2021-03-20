Shader "Scholl's Viewport/Color"
{
    Properties
    {
        _Color ("Color", Color) = (1, 0, 1, .5)
    }
    
    SubShader 
    {
        Pass 
        {
            ColorMask 0
       
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
     
            float4 vert (const float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }
     
            half4 frag () : COLOR
            {
                return half4 (0, 0, 0, 0);
            }
            
            ENDCG  
        }
       
        Pass 
        {
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            float4 vert (const float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }
         
            fixed4 _Color;

            fixed4 frag () : SV_Target
            {
                return _Color;
            }
            
            ENDCG
        }
    }
}