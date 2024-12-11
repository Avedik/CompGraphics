#version 330 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec4 color;

uniform float x_scale;
uniform float y_scale;
uniform mat4 matr;
    
out vec4 col;

void main() {
    vec3 pos = position * mat3(x_scale,0,0,
                               0,y_scale,0,
                               0,0,1);
    gl_Position = matr * vec4(pos, 1.0f);
    col = color;
}