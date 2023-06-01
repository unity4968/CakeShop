// Compiled shader for all platforms, uncompressed size: 1.0KB

Shader "Mobile/Unlit2S" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader { 
 LOD 100
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="Vertex" "RenderType"="Opaque" }
  Cull off
  SetTexture [_MainTex] { combine texture }
 }
 
}
}