#version 330 core
in vec3 fragColor;
out vec4 color;

uniform vec4 uniformColor; // ��� �������� ������������

void main()
{
    //color = uniformColor; // uniform ��� �������� ������������
    color = vec4(fragColor, 1.0); // ��� ������������ ������������
}
