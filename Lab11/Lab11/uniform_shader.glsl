#version 330 core

uniform vec3 uniformcolor; // Uniform ���������� ��� �����
out vec4 color;


void main()
{
    color = vec4(uniformcolor, 1.0); // ������������� ���� ��������� � �����-������� ������ 1.0
}
