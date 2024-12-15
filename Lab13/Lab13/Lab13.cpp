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

struct Camera {
    glm::vec3 position; // Позиция камеры
    glm::vec3 front;    // Направление взгляда
    glm::vec3 up;       // Вектор "вверх"
    float yaw;          // Угол поворота по горизонтали
    float pitch;        // Угол поворота по вертикали
    float movementSpeed; // Скорость движения
    float mouseSensitivity; // Чувствительность мыши

    Camera(glm::vec3 startPos)
        : position(startPos), front(glm::vec3(0.0f, 0.0f, -1.0f)), up(glm::vec3(0.0f, 1.0f, 0.0f)),
        yaw(-90.0f), pitch(0.0f), movementSpeed(2.5f), mouseSensitivity(0.1f) {
        updateCameraVectors();
    }

    void updateCameraVectors() {
        glm::vec3 front;
        front.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch));
        front.y = sin(glm::radians(pitch));
        front.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch));
        this->front = glm::normalize(front);
        glm::vec3 right = glm::normalize(glm::cross(this->front, glm::vec3(0.0f, 1.0f, 0.0f)));
        this->up = glm::normalize(glm::cross(right, this->front)); // Пересчет вектора "вверх"
    }

    void processKeyboardInput(sf::Keyboard::Key key, float deltaTime) {
        float velocity = movementSpeed * deltaTime;
        if (key == sf::Keyboard::W)
            position += front * velocity; // Вперёд
        if (key == sf::Keyboard::S)
            position -= front * velocity; // Назад
        if (key == sf::Keyboard::A)
            position -= glm::normalize(glm::cross(front, up)) * velocity; // Влево
        if (key == sf::Keyboard::D)
            position += glm::normalize(glm::cross(front, up)) * velocity; // Вправо
        if (key == sf::Keyboard::Up)
            position += up * velocity; // Вверх
        if (key == sf::Keyboard::Down)
            position -= up * velocity; // Вниз
        updateCameraVectors();
    }

    void rotateCamera(float yawOffset, float pitchOffset) {
        yaw += yawOffset;
        pitch += pitchOffset;

        // pitch ограничен только для предотвращения инверсии
        if (pitch > 89.0f)
            pitch = 89.0f;
        if (pitch < -89.0f)
            pitch = -89.0f;

        // Обеспечение 360-градусного поворота по yaw
        if (yaw < 0.0f)
            yaw += 360.0f;
        if (yaw >= 360.0f)
            yaw -= 360.0f;

        updateCameraVectors();
    }

    glm::mat4 getViewMatrix() {
        return glm::lookAt(position, position + front, up);
    }
};


std::vector<std::vector<GLfloat>> verts;
std::vector<std::vector<GLfloat>> texs;
std::vector<std::vector<GLfloat>> normals;
std::vector<std::vector<GLint>> indVerts;
std::vector<std::vector<GLint>> indTexs;
std::vector<std::vector<GLint>> indNorms;

std::vector<std::vector<GLfloat>> figures;

GLuint vboInstance;
glm::vec2 translations[7] = { glm::vec3(0.0, 0.0, 0.0),
                              glm::vec3(1.0, 0.0, 0.0),
                              glm::vec3(0.0, 1.0, 0.0),
                              glm::vec3(0.0, 0.0, 1.0),
                              glm::vec3(1.0, 1.0, 0.0),
                              glm::vec3(1.0, 0.0, 1.0),
                              glm::vec3(0.0, 1.0, 1.0) };

std::string readAllFile(const std::string filename)
{
    std::fstream fs(filename);
    std::ostringstream sstr;
    sstr << fs.rdbuf();
    return sstr.str();
}

// Функция загрузки модели из OBJ файла
void loadOBJ(const std::string filename)
{
    GLint num = verts.size();

    verts.push_back(std::vector<GLfloat>());
    texs.push_back(std::vector<GLfloat>());
    normals.push_back(std::vector<GLfloat>());
    indVerts.push_back(std::vector<GLint>());
    indTexs.push_back(std::vector<GLint>());
    indNorms.push_back(std::vector<GLint>());

    std::fstream obj(filename);
    if (!obj.is_open())
    {
        std::cerr << "Не удалось загрузить obj файл" << std::endl;
        return;
    }

    std::stringstream sstr;
    sstr << obj.rdbuf();

    std::string str;
    GLfloat numb;
    GLint intNumb;

    while (sstr >> str)
    {
        if (str == "v")
        {
            sstr >> numb;
            verts[num].push_back(numb);
            sstr >> numb;
            verts[num].push_back(numb);
            sstr >> numb;
            verts[num].push_back(numb);
        }

        if (str == "vn")
        {
            sstr >> numb;
            normals[num].push_back(numb);
            sstr >> numb;
            normals[num].push_back(numb);
            sstr >> numb;
            normals[num].push_back(numb);
        }

        if (str == "vt")
        {
            sstr >> numb;
            texs[num].push_back(numb);
            sstr >> numb;
            texs[num].push_back(numb);
        }

        if (str == "f")
        {
            for (int i = 0; i < 3; ++i)
            {
                sstr >> intNumb;
                indVerts[num].push_back(intNumb - 1);
                sstr >> str;
                if (str[0] == '/' && str[1] == '/')
                {
                    indNorms[num].push_back(stoi(str.substr(2)) - 1);
                    indTexs[num].push_back(-1);
                }
                else
                {
                    indTexs[num].push_back(stoi(str.substr(1, str.find('/', 1) - 1)) - 1);
                    indNorms[num].push_back(stoi(str.substr(str.find('/', str.find('/', 1)) + 1)) - 1);
                }
            }
            //4
        }
    }
    figures.push_back(std::vector<GLfloat>());
    for (int i = 0; i < indVerts[num].size(); ++i)
    {
        figures[num].push_back(verts[num][3 * indVerts[num][i]]);
        figures[num].push_back(verts[num][3 * indVerts[num][i] + 1]);
        figures[num].push_back(verts[num][3 * indVerts[num][i] + 2]);

        figures[num].push_back(normals[num][3 * indNorms[num][i]]);
        figures[num].push_back(normals[num][3 * indNorms[num][i] + 1]);
        figures[num].push_back(normals[num][3 * indNorms[num][i] + 2]);

        if (indTexs[num][i] != -1)
        {
            figures[num].push_back(texs[num][2 * indTexs[num][i]]);
            figures[num].push_back(texs[num][2 * indTexs[num][i] + 1]);
        }
        else
        {
            figures[num].push_back(0);
            figures[num].push_back(0);
        }

    }
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
        std::cout << "Error attach shaders \n";
    }

    return shaderProgram;
}

void createShape(GLuint& VBO, GLuint& VAO, GLuint ind) {
    glGenVertexArrays(1, &VAO);
    glBindVertexArray(VAO);

    glGenBuffers(1, &VBO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);

    glBufferData(GL_ARRAY_BUFFER, figures[ind].size() * sizeof(GLfloat), figures[ind].data(), GL_STATIC_DRAW);

    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)0);
    glEnableVertexAttribArray(0);

    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
    glEnableVertexAttribArray(1);

    glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(6 * sizeof(GLfloat)));
    glEnableVertexAttribArray(2);

    if (ind == 1)
    {
        glGenBuffers(1, &vboInstance);
        glBindBuffer(GL_ARRAY_BUFFER, vboInstance);
        glBufferData(GL_ARRAY_BUFFER, sizeof(glm::vec2) * 7, &translations[0], GL_STATIC_DRAW);
        glVertexAttribPointer(3, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(GLfloat), (void*)0);
        glEnableVertexAttribArray(3);
        glVertexAttribDivisor(3, 1);
    }

    glBindBuffer(GL_ARRAY_BUFFER, NULL);
    glBindVertexArray(0);
}

int main() {
    sf::RenderWindow window(sf::VideoMode(500, 500), "Window");
    glewInit();
    glEnable(GL_DEPTH_TEST);

    GLuint vboCenter;
    GLuint vaoCenter;

    GLuint vboObj;
    GLuint vaoObj;

    GLuint texCenter;
    GLuint texObj;

    int w = 512;
    int h = 512;

    glEnable(GL_TEXTURE_2D);
    GLuint shaderCenterProgram = createShaderProgram("center_vertex_shader.glsl", "center_fragment_shader.glsl");
    GLuint shaderObjectProgram = createShaderProgram("object_vertex_shader.glsl", "object_fragment_shader.glsl");

    loadOBJ("sphere.obj");
    loadOBJ("cube.obj");

    createShape(vboCenter, vaoCenter, 0);
    createShape(vboObj, vaoObj, 1);

    GLuint* tex = &texCenter;
    for (std::string filename : {"container.jpg", "container.jpg"})
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
        tex = &texObj;
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

    Camera camera(glm::vec3(-3.0f, 0.0f, 10.0f)); // Начальная позиция камеры

    while (window.isOpen()) {

        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed) { window.close(); }
        }
        // Обработка ввода
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::W))
            camera.processKeyboardInput(sf::Keyboard::W, 0.1f); // движение вперёд
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::S))
            camera.processKeyboardInput(sf::Keyboard::S, 0.1f); // движение назад
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::A))
            camera.processKeyboardInput(sf::Keyboard::A, 0.1f); // движение влево
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::D))
            camera.processKeyboardInput(sf::Keyboard::D, 0.1f); // движение вправо
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Up))
            camera.processKeyboardInput(sf::Keyboard::Up, 0.1f); // движение вверх
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Down))
            camera.processKeyboardInput(sf::Keyboard::Down, 0.1f); // движение вниз

        if (sf::Keyboard::isKeyPressed(sf::Keyboard::R)) // Клавиша для горизонтального поворота
            camera.rotateCamera(1.0f, 0.0f); // Поворот вправо
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::F)) // Клавиша для вертикального поворота
            camera.rotateCamera(0.0f, 1.0f); // Поворот вверх
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::G)) // Клавиша для вертикального поворота вниз
            camera.rotateCamera(0.0f, -1.0f); // Поворот вниз

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        // Обновление матрицы вида
        glm::mat4 view = camera.getViewMatrix();
        glm::mat4 model = glm::mat4(1.0f);
        glm::mat4 projection = glm::perspective(glm::radians(45.0f), 800.0f / 600.0f, 0.1f, 100.0f);
        glm::mat4 mvp = projection * view * model;


        glUseProgram(shaderCenterProgram);
        glUniformMatrix4fv(glGetUniformLocation(shaderCenterProgram, "matr"), 1, GL_FALSE, glm::value_ptr(mvp));

        glBindVertexArray(vaoCenter);
        glBindBuffer(GL_ARRAY_BUFFER, vboCenter);
        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, texCenter);

        glDrawArrays(GL_TRIANGLES, 0, figures[0].size() / 8);

        glBindVertexArray(0);
        glBindBuffer(GL_ARRAY_BUFFER, NULL);
        glUseProgram(0);

        glUseProgram(shaderObjectProgram);
        glUniformMatrix4fv(glGetUniformLocation(shaderObjectProgram, "matr"), 1, GL_FALSE, glm::value_ptr(mvp));

        glBindVertexArray(vaoObj);
        glBindBuffer(GL_ARRAY_BUFFER, vboObj);
        glActiveTexture(GL_TEXTURE1);
        glBindTexture(GL_TEXTURE_2D, texObj);

        glDrawArraysInstanced(GL_TRIANGLES, 0, figures[1].size() / 8, 7);

        glBindVertexArray(0);
        glBindBuffer(GL_ARRAY_BUFFER, NULL);
        glUseProgram(0);

        window.display();
    }

    glDeleteBuffers(1, &vboCenter);
    glDeleteVertexArrays(1, &vaoCenter);
    glDeleteBuffers(1, &vboObj);
    glDeleteBuffers(1, &vboInstance);
    glDeleteVertexArrays(1, &vaoObj);
    glDeleteProgram(shaderCenterProgram);
    glDeleteProgram(shaderObjectProgram);

    return 0;
}