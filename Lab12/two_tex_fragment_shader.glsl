#version 330 core

uniform sampler2D ourTexture1;
uniform sampler2D ourTexture2;

in vec3 col;
in vec2 TexCoord;
out vec4 color;
uniform float coef;

void main()
{
    color = coef* vec4(col,1.0f);
    //color = mix(texture(ourTexture1, TexCoord), texture(ourTexture2, TexCoord), coef);
}
