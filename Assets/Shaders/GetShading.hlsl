#ifndef ADDITIONAL_LIGHT_INCLUDED
#define ADDITIONAL_LIGHT_INCLUDED


void GetShading_float(float3 WorldPos, out float ShadowAttenuation)
{
#ifdef SHADERGRAPH_PREVIEW
    ShadowAttenutation = 0;  
#else
    ShadowAttenutation = 1;
#endif
}

#endif