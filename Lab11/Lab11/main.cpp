#include <GL/glew.h>
#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <fstream>
#include <sstream>

// ��������������
const std::vector<GLfloat> quadVertices = {
    -0.5f, -0.5f,     1.0f, 0.0f, 0.0f,
     0.5f, -0.5f,     0.0f, 1.0f, 0.0f,
     0.5f,  0.5f,     0.0f, 0.0f, 1.0f,
    -0.5f,  0.5f,     1.0f, 1.0f, 0.0f
};

// ����
const std::vector<GLfloat> fanVertices = {
      0.0f, 0.0f,    1.0f, 0.0f, 0.0f,
     -0.6f, 0.18f,   0.0f, 1.0f, 0.0f,
     -0.4f, 0.4f,    0.0f, 0.0f, 1.0f,
      0.0f, 0.5f,    1.0f, 1.0f, 0.0f,
      0.5f, 0.5f,    1.0f, 0.0f, 1.0f,
      0.8f, 0.0f,    0.0f, 1.0f, 1.0f
};

// ���������� ������������
const std::vector<GLfloat> pentaVertices = {
     0.0f,   1.0f,   1.0f, 0.0f, 0.0f,
     0.95f,  0.31f,  0.0f, 1.0f, 0.0f,
     0.59f, -0.81f,  0.0f, 0.0f, 1.0f,
    -0.59f, -0.81f,  1.0f, 1.0f, 0.0f,
    -0.95f,  0.31f,  1.0f, 0.0f, 1.0f,
};

// ������ �� �����
std::string readAllFile(const std::string filename)
{
    std::fstream fs(filename);
    std::ostringstream sstr;
    sstr << fs.rdbuf();
    return sstr.str();
}

// ������� ��� ���������� �������
GLuint compileShader(const GLchar* source, GLenum type) {
    GLuint shader = glCreateShader(type);
    glShaderSource(shader, 1, &source, nullptr);
    glCompileShader(shader);

    GLint success;
    glGetShaderiv(shader, GL_COMPILE_STATUS, &success);
    if (!success) {
        char infoLog[512];
        glGetShaderInfoLog(shader, 512, nullptr, infoLog);
        std::cerr << "ERROR::SHADER::COMPILATION_FAILED\n" << infoLog << std::endl;
    }
    return shader;
}

// ������� ��� �������� ��������� ���������
GLuint createShaderProgram(const std::string filename) {
    std::string vertexShaderSource = readAllFile("vertex_shader.glsl");
    std::string fragmentShaderSource = readAllFile(filename);

    GLuint vertexShader = compileShader(vertexShaderSource.c_str(), GL_VERTEX_SHADER);
    GLuint fragmentShader = compileShader(fragmentShaderSource.c_str(), GL_FRAGMENT_SHADER);

    GLuint shaderProgram = glCreateProgram();
    glAttachShader(shaderProgram, vertexShader);
    glAttachShader(shaderProgram, fragmentShader);
    glLinkProgram(shaderProgram);

    return shaderProgram;
}


// ������� ��� ��������� ��������� �� �������
GLuint getShaderProgram(GLuint count) {

    // ������� ������������ (const)
    if (count == 0 || count == 3 || count == 6)
        return createShaderProgram("const_shader.glsl");

    // ������� ������������ ����� uniform
    else if (count == 1 || count == 4 || count == 7)
        return createShaderProgram("uniform_shader.glsl");

    // ����������� ������������
    else
        return createShaderProgram("fragment_gradient_shader.glsl");
}

// ������� ��� �������� �����
void createShape(GLuint& VBO, const std::vector<GLfloat>& vertices) {
    glGenBuffers(1, &VBO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(GLfloat), vertices.data(), GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, NULL);
}

// ������� ��� ��������� ������ �� �������
void getShape(GLuint& VBO, GLuint count) {

    // ���������������
    if (count == 0)
        createShape(VBO, quadVertices);

    // ����
    else if (count == 3)
        createShape(VBO, fanVertices);

    // ���������� ������������
    else
        createShape(VBO, pentaVertices);
}



int main() {

    sf::RenderWindow window(sf::VideoMode(500, 500), "Window");
    glewInit();

    GLuint VBO;
    GLuint count = 0;

    // ��������� ������ ��������� � ������
    GLuint shaderProgram = getShaderProgram(count);
    getShape(VBO, count);

    // ������� ����
    while (window.isOpen()) {

        sf::Event event;
        // ���� ��������� �������
        while (window.pollEvent(event))
        {
            // ������� �������� ����
            if (event.type == sf::Event::Closed) { window.close(); }
            // ������� ������� ������ ���� � ����
            else if (event.type == sf::Event::MouseButtonPressed) {
                count = (count + 1) % 9;
                shaderProgram = getShaderProgram(count);
                if (count == 0 || count == 3 || count == 6)
                    getShape(VBO, count);
            }
        }

        glClear(GL_COLOR_BUFFER_BIT);

        glUseProgram(shaderProgram);

        glBindBuffer(GL_ARRAY_BUFFER, VBO);

        // �������� OpenGL ��� �� ������ ���������������� ��������� ������
        glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(GLfloat), (void*)0); // �������
        glEnableVertexAttribArray(0);

        // �������� OpenGL ��� �� ������ ���������������� �������� ������
        glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(GLfloat), (void*)(2 * sizeof(GLfloat))); // ����
        glEnableVertexAttribArray(1);

        // �������� ������� uniform ����������
        GLuint colorLocation = glGetUniformLocation(shaderProgram, "uniformcolor");
        // ������������� ����
        glUseProgram(shaderProgram);
        glUniform3f(colorLocation, 1.0f, 0.0f, 0.0f); // ������ ���� ��� �������


        if (count == 1 || count == 4 || count == 7)
            glUniform4f(glGetUniformLocation(shaderProgram, "uniformColor"), 0.5f, 0.5f, 0.5f, 1.0f);
        // ����������� ������������
        else
            glUniform4f(glGetUniformLocation(shaderProgram, "uniformColor"), 0.5f, 0.5f, 0.5f, 1.0f);

        glBindBuffer(GL_ARRAY_BUFFER, NULL);

        // �������� ������ �� ���������� (������)

        // ��������������
        if (count == 0 || count == 1 || count == 2)
            glDrawArrays(GL_QUADS, 0, 4);
        // ����
        else if (count == 3 || count == 4 || count == 5)
            glDrawArrays(GL_TRIANGLE_FAN, 0, 6);
        // ���������� ������������
        else
            glDrawArrays(GL_POLYGON, 0, 5);

        // ����������� ����
        window.display();
    }

    // ������� ��������
    glDeleteBuffers(1, &VBO);
    glUseProgram(0);
    glDeleteProgram(shaderProgram);

    return 0;
}