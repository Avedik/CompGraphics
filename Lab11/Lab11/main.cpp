#include <GL/glew.h>
#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <fstream>
#include <sstream>

// Четырёхугольник
const std::vector<GLfloat> quadVertices = {
    -0.5f, -0.5f,     1.0f, 0.0f, 0.0f,
     0.5f, -0.5f,     0.0f, 1.0f, 0.0f,
     0.5f,  0.5f,     0.0f, 0.0f, 1.0f,
    -0.5f,  0.5f,     1.0f, 1.0f, 0.0f
};

// Веер
const std::vector<GLfloat> fanVertices = {
      0.0f, 0.0f,    1.0f, 0.0f, 0.0f,
     -0.6f, 0.18f,   0.0f, 1.0f, 0.0f,
     -0.4f, 0.4f,    0.0f, 0.0f, 1.0f,
      0.0f, 0.5f,    1.0f, 1.0f, 0.0f,
      0.5f, 0.5f,    1.0f, 0.0f, 1.0f,
      0.8f, 0.0f,    0.0f, 1.0f, 1.0f
};

// Правильный пятиугольник
const std::vector<GLfloat> pentaVertices = {
     0.0f,   1.0f,   1.0f, 0.0f, 0.0f,
     0.95f,  0.31f,  0.0f, 1.0f, 0.0f,
     0.59f, -0.81f,  0.0f, 0.0f, 1.0f,
    -0.59f, -0.81f,  1.0f, 1.0f, 0.0f,
    -0.95f,  0.31f,  1.0f, 0.0f, 1.0f,
};

// Чтение из файла
std::string readAllFile(const std::string filename)
{
    std::fstream fs(filename);
    std::ostringstream sstr;
    sstr << fs.rdbuf();
    return sstr.str();
}

// Функция для компиляции шейдера
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

// Функция для создания шейдерной программы
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


// Функция для получения программы по индексу
GLuint getShaderProgram(GLuint count) {

    // Плоское закрашивание (const)
    if (count == 0 || count == 3 || count == 6)
        return createShaderProgram("const_shader.glsl");

    // Плоское закрашивание через uniform
    else if (count == 1 || count == 4 || count == 7)
        return createShaderProgram("uniform_shader.glsl");

    // Градиентное закрашивание
    else
        return createShaderProgram("fragment_gradient_shader.glsl");
}

// Функция для создания фигур
void createShape(GLuint& VBO, const std::vector<GLfloat>& vertices) {
    glGenBuffers(1, &VBO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(GLfloat), vertices.data(), GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, NULL);
}

// Функция для получения фигуры по индексу
void getShape(GLuint& VBO, GLuint count) {

    // Четырехугольник
    if (count == 0)
        createShape(VBO, quadVertices);

    // Веер
    else if (count == 3)
        createShape(VBO, fanVertices);

    // Правильный пятиугольник
    else
        createShape(VBO, pentaVertices);
}



int main() {

    sf::RenderWindow window(sf::VideoMode(500, 500), "Window");
    glewInit();

    GLuint VBO;
    GLuint count = 0;

    // Получение первой программы и фигуры
    GLuint shaderProgram = getShaderProgram(count);
    getShape(VBO, count);

    // Главный цикл
    while (window.isOpen()) {

        sf::Event event;
        // Цикл обработки событий
        while (window.pollEvent(event))
        {
            // Событие закрытия окна
            if (event.type == sf::Event::Closed) { window.close(); }
            // Событие нажатия кнопки мыши в окне
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

        // Сообщаем OpenGL как он должен интерпретировать вершинные данные
        glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(GLfloat), (void*)0); // Позиция
        glEnableVertexAttribArray(0);

        // Сообщаем OpenGL как он должен интерпретировать цветовые данные
        glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(GLfloat), (void*)(2 * sizeof(GLfloat))); // Цвет
        glEnableVertexAttribArray(1);

        // Получаем локатор uniform переменной
        GLuint colorLocation = glGetUniformLocation(shaderProgram, "uniformcolor");
        // Устанавливаем цвет
        glUseProgram(shaderProgram);
        glUniform3f(colorLocation, 1.0f, 0.0f, 0.0f); // Задаем цвет как красный


        if (count == 1 || count == 4 || count == 7)
            glUniform4f(glGetUniformLocation(shaderProgram, "uniformColor"), 0.5f, 0.5f, 0.5f, 1.0f);
        // Градиентное закрашивание
        else
            glUniform4f(glGetUniformLocation(shaderProgram, "uniformColor"), 0.5f, 0.5f, 0.5f, 1.0f);

        glBindBuffer(GL_ARRAY_BUFFER, NULL);

        // Передаем данные на видеокарту (рисуем)

        // Четырёхугольник
        if (count == 0 || count == 1 || count == 2)
            glDrawArrays(GL_QUADS, 0, 4);
        // Веер
        else if (count == 3 || count == 4 || count == 5)
            glDrawArrays(GL_TRIANGLE_FAN, 0, 6);
        // Правильный пятиугольник
        else
            glDrawArrays(GL_POLYGON, 0, 5);

        // Перерисовка окна
        window.display();
    }

    // Очистка ресурсов
    glDeleteBuffers(1, &VBO);
    glUseProgram(0);
    glDeleteProgram(shaderProgram);

    return 0;
}