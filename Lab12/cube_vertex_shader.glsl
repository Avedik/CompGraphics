#version 330 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 color;
layout (location = 2) in vec2 texCoord;

uniform mat4 matr;

out vec3 col;
out vec2 TexCoord;

void main() {
	 gl_Position = matr * vec4(position, 1.0f);
	 col = color;
	 TexCoord = texCoord;
}