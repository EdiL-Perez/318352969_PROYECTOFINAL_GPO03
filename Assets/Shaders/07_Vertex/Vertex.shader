Shader "Custom/Vertex"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed("Speed",Range(0,5))=1
        _Frequency("Frequency",Range(0,5))=1
        _Ampltude("Amplitude",Range(0,5))=1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
      

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Speed;
            float _Frequency;
            float _Ampltude;

             float4 Flag(float4 vertexPosition,float2 uv)
             {
                     vertexPosition.y+=sin((uv.x-(_Time.y*_Speed))*_Frequency)*(uv.x*_Ampltude);
                     float4 vertex=UnityObjectToClipPos(vertexPosition);
                     return vertex;
             }


            v2f vert (appdata v)
            {
                v2f o;
                //o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex=Flag(v.vertex,v.uv);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                return col;
            }
            ENDCG
        }
    }
}
