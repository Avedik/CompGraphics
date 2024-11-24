#include <GL/glew.h>
#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <fstream>
#include <sstream>

std::string ReadAllFile(std::string filename)
{
    std::fstream fs(filename);
    std::ostringstream sstr;
    sstr << fs.rdbuf();
    return sstr.str();
}

// Функция для компиляции шейдера
GLuint CompileShader(const char* source, GLenum type) {
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

// Функция для создания шейдерной программы
GLuint CreateShaderProgram() {
    std::string vertexShaderSource = ReadAllFile("vertex_shader.glsl");
    std::string fragmentShaderSource = ReadAllFile("fragment_shader.glsl");

    GLuint vertexShader = CompileShader(vertexShaderSource.c_str(), GL_VERTEX_SHADER);
    GLuint fragmentShader = CompileShader(fragmentShaderSource.c_str(), GL_FRAGMENT_SHADER);

    GLuint shaderProgram = glCreateProgram();
    glAttachShader(shaderProgram, vertexShader);
    glAttachShader(shaderProgram, fragmentShader);
    glLinkProgram(shaderProgram);

    glDeleteShader(vertexShader);
    glDeleteShader(fragmentShader);

    return shaderProgram;
}

// Функция для создания фигур
void CreateShapes(GLuint& VAO, GLuint& VBO, const std::vector<float>& vertices) {
    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO);

    glBindVertexArray(VAO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(float), vertices.data(), GL_STATIC_DRAW);

    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)0); // Позиция
    glEnableVertexAttribArray(0);

    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)(2 * sizeof(float))); // Цвет
    glEnableVertexAttribArray(1);

    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);
}

int main() {
    sf::RenderWindow window(sf::VideoMode(200, 200), "Window");
    glewInit();

    GLuint shaderProgram = CreateShaderProgram();

    std::vector<float> quadVertices = {
        // Позиция       // Цвет
        -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, // Нижний левый (красный)
         0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // Нижний правый (зеленый)
         0.5f,  0.5f, 0.0f, 0.0f, 1.0f, // Верхний правый (синий)
        -0.5f,  0.5f, 1.0f, 1.0f, 0.0f  // Верхний левый (желтый)
    };

    GLuint quadVAO, quadVBO;
    CreateShapes(quadVAO, quadVBO, quadVertices);

    // Главный цикл
    while (window.isOpen()) {
        glClear(GL_COLOR_BUFFER_BIT);

        glUseProgram(shaderProgram);

        // Плоское закрашивание через uniform
        glUniform4f(glGetUniformLocation(shaderProgram, "uniformColor"), 0.5f, 0.5f, 0.5f, 1.0f);

        // Рисуем четырехугольник
        glBindVertexArray(quadVAO);
        glDrawArrays(GL_TRIANGLE_FAN, 0, 4); // Используем TRIANGLE_FAN для рисования четырехугольника
        glBindVertexArray(0);

        sf::Event event;
        // Цикл обработки событий
        while (window.pollEvent(event))
        {
            // Событие закрытия окна, 
            if (event.type == sf::Event::Closed)
                window.close();
        }

        // Перерисовка окна
        window.display();
    }

    // Очистка ресурсов
    glDeleteVertexArrays(1, &quadVAO);
    glDeleteBuffers(1, &quadVBO);
    glDeleteProgram(shaderProgram);
    return 0;
}
