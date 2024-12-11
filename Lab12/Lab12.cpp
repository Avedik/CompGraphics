#include <GL/glew.h>
#include <SFML/Graphics.hpp>
#include <glm/glm.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <iostream>
#include <vector>
#include <fstream>
#include <sstream>
#include <SOIL/SOIL.h>

#define _USE_MATH_DEFINES
#include "math.h"

GLuint VBO;
GLint XScale;
GLint YScale;

const std::vector<GLfloat> tetra{
       -1.0f, -1.0f, -1.0f, 0.0f, 0.0f, 1.0f,
       -1.0f, 1.0f, 1.0f,   0.0f, 1.0f, 0.0f,
        1.0f, -1.0f, 1.0f,  1.0f, 0.0f, 0.0f,

      -1.0f, -1.0f, -1.0f,   0.0f, 0.0f, 1.0f,
       -1.0f, 1.0f, 1.0f,    0.0f, 1.0f, 0.0f,
       1.0f, 1.0f, -1.0f,    1.0f, 1.0f, 1.0f,

       -1.0f, -1.0f, -1.0f, 0.0f, 0.0f, 1.0f,
       1.0f, -1.0f, 1.0f,   1.0f, 0.0f, 0.0f,
       1.0f, 1.0f, -1.0f,   1.0f, 1.0f, 1.0f,

       1.0f, -1.0f, 1.0f,   1.0f, 0.0f, 0.0f,
       1.0f, 1.0f, -1.0f,   1.0f, 1.0f, 1.0f,
       -1.0f, 1.0f, 1.0f,   0.0f, 1.0f, 0.0f,
};


const std::vector<GLfloat> cube{
       -1.0f, 1.0f, 1.0f,    0.0f, 1.0f, 0.0f,   1.0f, 0.0f,
       1.0f, 1.0f, 1.0f,    1.0f, 0.0f, 0.0f,   0.0f, 0.0f,
       1.0f, -1.0f, 1.0f,   1.0f, 1.0f, 0.0f,   0.0f, 1.0f,
       -1.0f, -1.0f, 1.0f,  0.0f, 0.0f, 1.0f,   1.0f, 1.0f,

       -1.0f, 1.0f, -1.0f,  0.5f, 0.5f, 0.5f,   1.0f, 0.0f,
       -1.0f, 1.0f, 1.0f,   0.0f, 1.0f, 0.0f,   0.0f, 0.0f,
       -1.0f, -1.0f, 1.0f,  0.0f, 0.0f, 1.0f,   0.0f, 1.0f,
       -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f,   1.0f, 1.0f,

        1.0f, -1.0f, 1.0f,   1.0f, 1.0f, 0.0f,   1.0f, 0.0f,
       -1.0f, -1.0f, 1.0f,  0.0f, 0.0f, 1.0f,    0.0f, 0.0f,
       -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f,    0.0f, 1.0f,
        1.0f, -1.0f, -1.0f,  0.0f, 1.0f, 1.0f,   1.0f, 1.0f,

       1.0f, 1.0f, 1.0f,    1.0f, 0.0f, 0.0f,   1.0f, 0.0f,
       1.0f, 1.0f, -1.0f,   1.0f, 1.0f, 1.0f,   0.0f, 0.0f,
       1.0f, -1.0f, -1.0f,  0.0f, 1.0f, 1.0f,   0.0f, 1.0f,
       1.0f, -1.0f, 1.0f,   0.0f, 1.0f, 1.0f,   1.0f, 1.0f,

       1.0f, 1.0f, -1.0f,   1.0f, 1.0f, 1.0f,   1.0f, 0.0f,
       -1.0f, 1.0f, -1.0f,  0.5f, 0.5f, 0.5f,   0.0f, 0.0f,
       -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f,   0.0f, 1.0f,
       1.0f, -1.0f, -1.0f,  0.0f, 1.0f, 1.0f,   1.0f, 1.0f,

       1.0f, 1.0f, 1.0f,    1.0f, 0.0f, 0.0f,   1.0f, 0.0f,
       -1.0f, 1.0f, 1.0f,   0.0f, 1.0f, 0.0f,   0.0f, 0.0f,
       -1.0f, 1.0f, -1.0f,  0.5f, 0.5f, 0.5f,   0.0f, 1.0f,
       1.0f, 1.0f, -1.0f,   1.0f, 1.0f, 1.0f,   1.0f, 1.0f,

};

std::vector<GLfloat> circle{};

float scaleX = 1.0;
float scaleY = 1.0;

void changeScales(float scaleXinc, float scaleYinc) {
    scaleX += scaleXinc;
    scaleY += scaleYinc;
}

std::vector<float> hsvToRgb(float hue, float saturation = 100.0, float value = 100.0)
{
    // Определяем, в каком секторе цветового круга находится hue
    int perc = (int)floor(hue / 60) % 6; // Получаем номер сектора (0-5)

    // Вычисляем промежуточные значения для преобразования
    float div = (100.0f - saturation) * value / 100.0;
    float prod = (value - div) * (((int)hue % 60) / 60.0);
    float sum = div + prod;
    float dec = value - prod;

    // В зависимости от сектора, возвращаем соответствующий цвет в формате RGBA
    switch (perc)
    {
    case 0:
        // Сектор 0: Красный
        return { value / 100.0f, sum / 100.0f, div / 100.0f, 1.0f };
    case 1:
        // Сектор 1: Желтый
        return { dec / 100.0f, value / 100.0f, div / 100.0f, 1.0f };
    case 2:
        // Сектор 2: Зеленый
        return { div / 100.0f, value / 100.0f, sum / 100.0f, 1.0f };
    case 3:
        // Сектор 3: Циановый
        return { div / 100.0f, dec / 100.0f, value / 100.0f, 1.0f };
    case 4:
        // Сектор 4: Синий
        return { sum / 100.0f, div / 100.0f, value / 100.0f, 1.0f };
    case 5:
        // Сектор 5: Магента
        return { value / 100.0f, div / 100.0f, dec / 100.0f, 1.0f };
    }

    // Если что-то пошло не так, возвращаем черный цвет (0, 0, 0, 0)
    return { 0, 0, 0 , 0 };
}

const int circleVertexCount = 360; // Количество вершин на окружности
const float radius = 0.5f; // Радиус круга

void initCircle() {
    // Центральная вершина (0, 0, 0)
    circle.push_back(0.0f); // X
    circle.push_back(0.0f); // Y
    circle.push_back(0.0f); // Z
    std::vector<float> centerColor = { 1.0f, 1.0f, 1.0f, 1.0f }; // Цвет для центра
    circle.insert(circle.end(), centerColor.begin(), centerColor.end());

    std::vector<float> color;
    // Генерация вершин по окружности
    for (int i = 0; i < circleVertexCount; ++i) {
        float angle = (i * 2.0f * M_PI) / circleVertexCount; // Угол в радианах
        float x = radius * cos(angle); // X координата
        float y = radius * sin(angle); // Y координата
        circle.push_back(x);
        circle.push_back(y);
        circle.push_back(0.0f); // Z координата

        // Добавляем цвет для каждой вершины
        color = hsvToRgb(i % 360);
        circle.insert(circle.end(), color.begin(), color.end());
    }

    // Добавляем еще одну вершину в конец массива вершин, которая будет совпадать
    // с первой вершиной на окружности
    circle.push_back(radius * cos(0.0f)); // X координата
    circle.push_back(radius * sin(0.0f)); // Y координата
    circle.push_back(0.0f); // Z координата
    circle.insert(circle.end(), color.begin(), color.end());
}

std::string readAllFile(const std::string filename)
{
    std::fstream fs(filename);
    std::ostringstream sstr;
    sstr << fs.rdbuf();
    return sstr.str();
}

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

GLuint createShaderProgram(const std::string vShader, const std::string frShader) {
    std::string vertexShaderSource = readAllFile(vShader);
    std::string fragmentShaderSource = readAllFile(frShader);

    GLuint vertexShader = compileShader(vertexShaderSource.c_str(), GL_VERTEX_SHADER);
    GLuint fragmentShader = compileShader(fragmentShaderSource.c_str(), GL_FRAGMENT_SHADER);

    GLuint shaderProgram = glCreateProgram();
    glAttachShader(shaderProgram, vertexShader);
    glAttachShader(shaderProgram, fragmentShader);
    glLinkProgram(shaderProgram);

    GLint link_ok;
    glGetProgramiv(shaderProgram, GL_LINK_STATUS, &link_ok);
    if (!link_ok) {
        std::cout << "error attach shaders \n";
    }

    return shaderProgram;
}


GLuint getShaderProgram(GLuint count) {
    if (count == 0)
        return createShaderProgram("tetra_vertex_shader.glsl", "tetra_fragment_shader.glsl");
    else if (count == 1)
        return createShaderProgram("cube_vertex_shader.glsl", "tex_fragment_shader.glsl");
    else if (count == 2)
        return createShaderProgram("cube_vertex_shader.glsl", "two_tex_fragment_shader.glsl");
    else
    {
        GLuint Program = createShaderProgram("circle_vertex_shader.glsl", "circle_fragment_shader.glsl");

        const char* unif_name = "x_scale";
        XScale = glGetUniformLocation(Program, unif_name);

        unif_name = "y_scale";
        YScale = glGetUniformLocation(Program, unif_name);

        initCircle();

        return Program;
    }
}

void createShape(GLuint& VBO, const std::vector<GLfloat>& vertices) {
    glGenBuffers(1, &VBO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(GLfloat), vertices.data(), GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, NULL);
}

void getShape(GLuint& VBO, GLuint count) {

    if (count == 0)
        createShape(VBO, tetra);

    else if (count == 1 || count == 2)
        createShape(VBO, cube);

    else
        createShape(VBO, circle);
}

int main() {
    sf::RenderWindow window(sf::VideoMode(500, 500), "Window");
    glewInit();
    glEnable(GL_DEPTH_TEST);

    GLuint tex1;
    GLuint tex2;
    GLuint count = 0;
    int w = 512;
    int h = 512;
    float coef = 0.5;

    glEnable(GL_TEXTURE_2D);
    glEnable(GL_DEPTH_TEST);
    GLuint shaderProgram = getShaderProgram(count);
    getShape(VBO, count);

    GLuint* tex = &tex1;
    for (std::string filename : {"container.jpg", "container1.jpg"})
    {
        unsigned char* image = SOIL_load_image(filename.c_str(), &w, &h, 0, SOIL_LOAD_RGB);
        glGenTextures(1, tex);
        glBindTexture(GL_TEXTURE_2D, *tex);

        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, w, h, 0, GL_RGB, GL_UNSIGNED_BYTE, image);

        SOIL_free_image_data(image);
        tex = &tex2;
    }

    glBindBuffer(GL_TEXTURE_2D, NULL);

    glm::mat4 model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(0.0f, 0.0f, 0.0f));

    glm::mat4 view = glm::lookAt(
        glm::vec3(5.0f, 5.0f, -5.0f),
        glm::vec3(0.0f, 0.0f, 0.0f),
        glm::vec3(0.0f, 1.0f, 0.0f)
    );

    glm::mat4 projection = glm::perspective(glm::radians(45.0f), 500.0f / 500.0f, 0.1f, 100.0f);

    glm::mat4 mvp = projection * view * model;

    while (window.isOpen()) {

        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed) { window.close(); }
            else if (event.type == sf::Event::MouseButtonPressed) {
                count = (count + 1) % 4;

                shaderProgram = getShaderProgram(count);
                getShape(VBO, count);
            }
            // Обработка клавиш для перемещения тетраэдра
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::W))
                model = glm::translate(model, glm::vec3(0.0f, 0.1f, 0.0f)); // Движение вверх
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::S))
                model = glm::translate(model, glm::vec3(0.0f, -0.1f, 0.0f)); // Движение вниз
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::D))
                model = glm::translate(model, glm::vec3(-0.1f, 0.0f, 0.0f)); // Движение влево
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::A))
                model = glm::translate(model, glm::vec3(0.1f, 0.0f, 0.0f)); // Движение вправо
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::R))
                model = glm::translate(model, glm::vec3(0.0f, 0.0f, 0.1f)); // Движение вперед
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::F))
                model = glm::translate(model, glm::vec3(0.0f, 0.0f, -0.1f)); // Движение назад


            if (sf::Keyboard::isKeyPressed(sf::Keyboard::Up))
                coef = (coef + 0.1f) <= 1.0f ? (coef + 0.1f) : 1.0f;
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::Down))
                coef = (coef - 0.1f) >= 0.0f ? (coef - 0.1f) : 0.0f;

            if (sf::Keyboard::isKeyPressed(sf::Keyboard::I))
                changeScales(0, 0.05);
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::K))
                changeScales(0, -0.05);
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::J))
                changeScales(-0.05, 0);
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::L))
                changeScales(0.05, 0);
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        glUseProgram(shaderProgram);

        mvp = count == 3 ? glm::mat4(1.0f) : projection * view * model;
        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "matr"), 1, GL_FALSE, glm::value_ptr(mvp));

        glBindBuffer(GL_ARRAY_BUFFER, VBO);

        if (count == 0)
            glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (GLvoid*)0);
        else if (count == 1 || count == 2)
            glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)0);
        else
            glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 7 * sizeof(GLfloat), (GLvoid*)0);

        glEnableVertexAttribArray(0);

        if (count == 0)
            glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
        else if (count == 1 || count == 2)
            glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
        else
            glVertexAttribPointer(1, 4, GL_FLOAT, GL_FALSE, 7 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));

        glEnableVertexAttribArray(1);

        if (count == 1 || count == 2)
        {
            glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(6 * sizeof(GLfloat)));
            glEnableVertexAttribArray(2);
            glUniform1f(glGetUniformLocation(shaderProgram, "coef"), coef);
        }
        else if (count == 3)
        {
            glUniform1f(XScale, scaleX);
            glUniform1f(YScale, scaleY);
        }

        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "matr"), 1, GL_FALSE, glm::value_ptr(mvp));
        glBindBuffer(GL_ARRAY_BUFFER, NULL);

        if (count == 0)
            glDrawArrays(GL_TRIANGLES, 0, 12);
        else if (count == 1)
        {
            glBindTexture(GL_TEXTURE_2D, tex1);
            glDrawArrays(GL_QUADS, 0, 24);
        }
        else if (count == 2)
        {
            glActiveTexture(GL_TEXTURE0);
            glBindTexture(GL_TEXTURE_2D, tex1);
            glUniform1i(glGetUniformLocation(shaderProgram, "ourTexture1"), 0);
            glActiveTexture(GL_TEXTURE1);
            glBindTexture(GL_TEXTURE_2D, tex2);
            glUniform1i(glGetUniformLocation(shaderProgram, "ourTexture2"), 1);

            glDrawArrays(GL_QUADS, 0, 24);
        }
        else
        {
            glDrawArrays(GL_TRIANGLE_FAN, 0, circleVertexCount + 2);
        }

        window.display();
    }

    glDeleteBuffers(1, &VBO);
    glUseProgram(0);
    glDeleteProgram(shaderProgram);

    return 0;
}