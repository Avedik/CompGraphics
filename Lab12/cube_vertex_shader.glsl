#version 330 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 color;

uniform mat4 matr;

out vec3 col;

void main() {
	 //gl_Position = matr* vec4(position, 1.0f);
	 gl_Position = vec4(position, 1.0f);
	 col = color;
}