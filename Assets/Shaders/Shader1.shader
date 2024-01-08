Shader"Custom/WallGlow"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _Color ("Main Color", Color) = (1,1,1,1)
        _Emission ("Emission", Color) = (0,0,0,0)
    }

    CGINCLUDE
#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
};

struct v2f
{
    float4 pos : POSITION;
    float4 color : COLOR;
};

uniform float _EmissionPower;

ENDCG

    SubShader
{
    Tags
        {
            "Queue" = "Overlay"
}

        Pass
        {
ZWrite On

ZTest LEqual

Cull Front

ColorMask RGB

Blend SrcAlpha
OneMinusSrcAlpha

            // Draw the actual object
            SetTexture[_MainTex]
{
    combineprimary

}

            // Draw the glow (emission)
            SetTexture [_MainTex]
            {
combine add
            }
ColorMask RGB

AlphaBlend On

ColorMaterial AmbientAndDiffuse

Lighting Off
            SetTexture [_MainTex] {
combine add, previous}
            SetTexture [_MainTex] {
combine add, previous}
            SetTexture [_MainTex] {
combine add, previous}

            // Set emission color and power
            SetTexture [_MainTex] {
combine previous* 2 + _Emission }
        }
    }
}
