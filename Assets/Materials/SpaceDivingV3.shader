Shader "Unlit/SpaceDivingV3"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
        _BaseColor("Base Color",Color) =(1,1,1,1)
        _Brightness("Brightness",float) = 0.001
        _ParticleCount("ParticleCount",float) = 20
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
            float _Brightness;
            int _ParticleCount;
        CBUFFER_END

        //TEXTURE2D(_MainTex);
        //SAMPLER(sampler_MainTex);
        
        struct VertexInput{
            float4 position: POSITION;
            float2 uv : TEXCOORD0;
        };
        
        struct VertexOutput{
            float4 position: SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            VertexOutput vert(VertexInput i){
                VertexOutput o;
                o.position=TransformObjectToHClip(i.position.xyz);
                o.uv=i.uv;
                return o;
            }
            
            float2 Hash12(float t){
                float x=frac(sin(t*674.3)*344.2);
                float y=frac(sin((t+x)*554.3)*234.1);
                
                return float2(x,y);
            }
            
            float4 frag(VertexOutput input) : SV_Target{
                //float4 baseTex=SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv);
                float3 col=float3(0,0,0);
                float r=_ScreenParams.x/_ScreenParams.y;
                float x=input.uv.x*r;
                input.uv=float2(x,input.uv.y);
                input.uv-=float2(r/2,0.5);
                
                float t=frac(_Time);
                for(float i=0;i<_ParticleCount;i++){
                    float2 dir=Hash12(i)-0.5;
                    float d= length(input.uv - dir*t*2);
                    
                    col+=_Brightness/d;
                }
                
                return float4(col,1);
            }
            
            ENDHLSL
        }
    }
}
