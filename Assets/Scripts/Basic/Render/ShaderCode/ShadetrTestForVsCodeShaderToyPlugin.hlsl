// Author:
// Title:

#ifdef GL_ES
precision mediump float;
#endif


uniform vec2 u_mouse;

void main() {
    vec2 st = gl_FragCoord.xy/iResolution.xy;
    st.x *= iResolution.x/iResolution.y;

    vec3 color = vec3(0.);
    color = vec3(st.x,st.y,abs(sin(iTime)));

    gl_FragColor = vec4(color,1.0);
}