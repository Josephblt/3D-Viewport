Shader "Scholl's Viewport/Wireframe"
{
    Properties
    {
        _Alpha ("Alpha", Range (0., 1.)) = 1.
        _Color ("Object Color", color) = (0, 0, 0, 0)
        _WireframeColor ("Wireframe Color", color) = (0, 0, 0, 0)
        _WireframeSmooth("Wireframe Smoothness", Range(0., 2.)) = 1
        _WireframeWidth ("Wireframe Width", Range(0., 2.)) = 1
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"

            struct appdata
			{
			    float4 vertex : POSITION;
			    float3 normal : NORMAL;
			    float2 uv : TEXCOORD0;
			};

			struct v2_f
			{
			    float2 uv : TEXCOORD0;
			    float4 vertex : SV_POSITION;
			};

            float distance_sq(const float2 pt1, const float2 pt2)
			{
				const float2 v = pt2 - pt1;
			    return dot(v,v);
			}

			float minimum_distance(const float2 v, const float2 w, const float2 p) {
				const float l2 = distance_sq(v, w);
				const float t = max(0, min(1, dot(p - v, w - v) / l2));
				const float2 projection = v + t * (w - v);
				return distance(p, projection);
			}
            
            v2_f vert (const appdata v)
			{
			    v2_f o;
			    o.vertex = UnityObjectToClipPos(v.vertex);
			    o.uv = v.uv;
			    return o;
			}
 
            fixed4 _Color;
            float _Alpha;
            fixed4 _WireframeColor;
            float _WireframeSmooth;
            float _WireframeWidth;            
 
            fixed4 frag (const v2_f i) : SV_Target
			{
				const float2 u_vector = float2(ddx(i.uv.x),ddy(i.uv.x));
				const float2 v_vector = float2(ddx(i.uv.y),ddy(i.uv.y));

				const float v_length = length(u_vector);
				const float u_length = length(v_vector);

				const float maximum_u_distance = _WireframeWidth * v_length;
				const float maximum_v_distance = _WireframeWidth * u_length;

				const float left_edge_u_distance = i.uv.x;
				const float right_edge_u_distance = 1.0-left_edge_u_distance;

				const float bottom_edge_v_distance = i.uv.y;
				const float top_edge_v_distance = 1.0 - bottom_edge_v_distance;

				const float minimum_u_distance = min(left_edge_u_distance,right_edge_u_distance);
				const float minimum_v_distance = min(bottom_edge_v_distance,top_edge_v_distance);

				const float normalized_u_distance = minimum_u_distance / maximum_u_distance;
				const float normalized_v_distance = minimum_v_distance / maximum_v_distance;

				const float closest_normalized_distance = min(normalized_u_distance,normalized_v_distance);
			    const float color_blend = 1.0 - smoothstep(1.0,1.0 + _WireframeSmooth/_WireframeWidth,closest_normalized_distance);

				fixed4 color = lerp(_Color, _WireframeColor, color_blend);
				color.a *= _Alpha;
                return color;
			}
 
            ENDCG
        }        
    }
}