#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"

Texture2D _CameraDepthTexture;

// Could be e.g. the position of a primary camera or a shadow-casting light.
float3 GetCurrentViewPosition()
{
#if (defined(SHADERPASS) && (SHADERPASS != SHADERPASS_SHADOWS)) && (!UNITY_SINGLE_PASS_STEREO) // Can't use camera position when rendering stereo
    return GetPrimaryCameraPosition();
#else
    // This is a generic solution.
    // However, using '_WorldSpaceCameraPos' is better for cache locality,
    // and in case we enable camera-relative rendering, we can statically set the position is 0.
    return UNITY_MATRIX_I_V._14_24_34;
#endif
}

float4 VFXTransformPositionWorldToClip(float3 posWS)
{
    return TransformWorldToHClip(posWS);
}

float4 VFXTransformPositionObjectToClip(float3 posOS)
{
    float3 posWS = TransformObjectToWorld(posOS);
    return VFXTransformPositionWorldToClip(posWS);
}

float3 VFXTransformPositionWorldToView(float3 posWS)
{
    return TransformWorldToView(posWS);
}

float4x4 VFXGetObjectToWorldMatrix()
{
    return GetObjectToWorldMatrix();
}

float4x4 VFXGetWorldToObjectMatrix()
{
    return GetWorldToObjectMatrix();
}

float3x3 VFXGetWorldToViewRotMatrix()
{
    return (float3x3)GetWorldToViewMatrix();
}

float3 VFXGetViewWorldPosition()
{
    return GetCurrentViewPosition();
}

float4x4 VFXGetViewToWorldMatrix()
{
    return UNITY_MATRIX_I_V;
}

float VFXSampleDepth(float4 posSS)
{
    return LOAD_TEXTURE2D(_CameraDepthTexture, posSS.xy).r;
}

float VFXLinearEyeDepth(float depth)
{
    return LinearEyeDepth(depth, _ZBufferParams);
}

float4 VFXApplyShadowBias(float4 posCS)
{
    return posCS;
}

float4 VFXApplyFog(float4 color,float4 posSS,float3 posWS)
{
    return color; //TODO
}
