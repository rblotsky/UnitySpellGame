Shader "Custom/OutlineShader"
{
    Properties
    {
        _OutlineColour("Outline Colour", Color) = (1,1,1,1)
        _OutlineWidth("Outline Width", Float) = 0.03
    }

    SubShader
    {
        Tags {"RenderType"="Opaque" "Queue"="Geometry"}

        // Outline Pass
        Pass
        {
            Cull front

            CGPROGRAM

            // Includes //
            #include "UnityCG.cginc"

            // Compilation directives //
            #pragma vertex VertexProgram
            #pragma fragment frag2

            // Variables //
            float _OutlineWidth;
            fixed4 _OutlineColour;

            // Structs //
            // vertex shader inputs
            struct appdata
            {
                float4 position : POSITION; // vertex position
                float3 normal : NORMAL; // normal vector
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float4 vertex : SV_POSITION; // clip space position
                float3 normal : NORMAL;
            };

            // vertex shader
            float4 VertexProgram(appdata v) : SV_POSITION 
            {
                float4 clipPosition = UnityObjectToClipPos(v.position);
                float3 clipNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, v.normal));
                float2 scaleDir = normalize(v.position.xy - float4(0,0,0,1));
                float2 offset = normalize(clipNormal.xy)/_ScreenParams.xy *_OutlineWidth*clipPosition.w*2;
                clipPosition.xy += offset;

                return clipPosition;
            }

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag2(v2f i) : SV_Target
            {
                return _OutlineColour;
            }
            ENDCG
        }
    }

    // Fallback
    Fallback "Standard"
}
