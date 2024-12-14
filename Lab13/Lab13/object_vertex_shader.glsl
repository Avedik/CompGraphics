#version 330 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texCoord;
layout (location = 3) in vec3 offset;

uniform mat4 matr;
out vec2 TexCoord;

void main() {
	 gl_Position = matr * vec4(position + offset, 1.0f);
	 TexCoord = texCoord;
}