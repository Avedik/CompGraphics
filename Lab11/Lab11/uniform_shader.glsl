#version 330 core

uniform vec3 uniformcolor; // Uniform переменная для цвета
out vec4 color;


void main()
{
    color = vec4(uniformcolor, 1.0); // Устанавливаем цвет фрагмента с альфа-каналом равным 1.0
}
