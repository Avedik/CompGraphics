#version 330 core

in vec3 col;
in vec2 TexCoord;
out vec4 color;

uniform sampler2D ourTexture1;
uniform sampler2D ourTexture2;
uniform float coef;

void main()
{
      color = coef* vec4(col,1.0f);
    //color = texture(ourTexture1,TexCoord) * vec4(col,1.0f) * coef;
}
