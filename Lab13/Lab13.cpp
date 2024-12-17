#include <GL/glew.h>
#include <SFML/Graphics.hpp>
#include <glm/glm.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <iostream>
#include <vector>
#include <fstream>
#include <sstream>
#include <SOIL.h>

#define _USE_MATH_DEFINES // для использования математических констант
#include <math.h>

// Структура для представления камеры в 3D пространстве
struct Camera {
    glm::vec3 position; // Позиция камеры в мировых координатах
    glm::vec3 front;    // Направление взгляда камеры
    glm::vec3 up;       // Вектор "вверх" для камеры
    float yaw;          // Угол поворота по горизонтали
    float pitch;        // Угол поворота по вертикали
    float movementSpeed; // Скорость движения камеры

    // Конструктор камеры, инициализирует позицию и векторы
    Camera(glm::vec3 startPos)
        : position(startPos), front(glm::vec3(0.0f, 0.0f, -1.0f)), up(glm::vec3(0.0f, 1.0f, 0.0f)),
        yaw(-90.0f), pitch(0.0f), movementSpeed(0.5f) {
        updateCameraVectors(); // Обновление векторов камеры
    }

    // Функция для обновления направления взгляда и вектора "вверх"
    void updateCameraVectors() {
        glm::vec3 front; // Вектор направления взгляда
        front.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch)); // Вычисление направления по оси X
        front.y = sin(glm::radians(pitch)); // Вычисление направления по оси Y
        front.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch)); // Вычисление направления по оси Z
        this->front = glm::normalize(front); // Нормализация вектора направления взгляда
        glm::vec3 right = glm::normalize(glm::cross(this->front, glm::vec3(0.0f, 1.0f, 0.0f))); // Вычисление вектора "право"
        this->up = glm::normalize(glm::cross(right, this->front)); // Пересчет вектора "вверх"
    }

    // Функция для обработки ввода с клавиатуры и перемещения камеры
    void processKeyboardInput(sf::Keyboard::Key key, float deltaTime) {
        float velocity = movementSpeed * deltaTime; // Вычисление скорости перемещения
        if (key == sf::Keyboard::W)
            position += front * velocity; // Движение вперёд
        if (key == sf::Keyboard::S)
            position -= front * velocity; // Движение назад
        if (key == sf::Keyboard::A)
            position -= glm::normalize(glm::cross(front, up)) * velocity; // Движение влево
        if (key == sf::Keyboard::D)
            position += glm::normalize(glm::cross(front, up)) * velocity; // Движение вправо
        if (key == sf::Keyboard::Up)
            position += up * velocity; // Движение вверх
        if (key == sf::Keyboard::Down)
            position -= up * velocity; // Движение вниз
        updateCameraVectors(); // Обновление векторов камеры
    }

    // Функция для вращения камеры
    void rotateCamera(float yawOffset, float pitchOffset) {
        yaw += yawOffset; // Обновление угла поворота по горизонтали
        pitch += pitchOffset; // Обновление угла поворота по вертикали

        // Ограничение угла поворота по вертикали для предотвращения инверсии
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

    // Функция для получения матрицы вида
    glm::mat4 getViewMatrix() {
        return glm::lookAt(position, position + front, up);
    }
};

struct Planet {
    float distance;      // Расстояние от Солнца
    float size;          // Размер планеты
    float orbitSpeed;    // Скорость орбитального вращения
    float rotationSpeed; // Скорость вращения вокруг своей оси
    float orbitAngle;    // Текущий угол на орбите
    float rotationAngle; // Текущий угол вращения вокруг своей оси
    GLuint texture;      // Текстура планеты
};

std::vector<Planet> planets;

std::vector<std::vector<GLfloat>> verts; // Вершины
std::vector<std::vector<GLfloat>> texs; // Текстурные координаты
std::vector<std::vector<GLfloat>> normals; // Нормали
std::vector<std::vector<GLint>> indVerts; // Индексы вершин
std::vector<std::vector<GLint>> indTexs; // Индексы текстур
std::vector<std::vector<GLint>> indNorms; // Индексы нормалей

std::vector<std::vector<GLfloat>> figures;

GLuint vboInstance;

std::string readAllFile(const std::string filename) {
    std::fstream fs(filename);
    std::ostringstream sstr;
    sstr << fs.rdbuf();
    return sstr.str();
}

void loadOBJ(const std::string filename) {
    GLint num = verts.size();

    // Инициализация векторов для новой модели
    verts.push_back(std::vector<GLfloat>());
    texs.push_back(std::vector<GLfloat>());
    normals.push_back(std::vector<GLfloat>());
    indVerts.push_back(std::vector<GLint>());
    indTexs.push_back(std::vector<GLint>());
    indNorms.push_back(std::vector<GLint>());

    std::fstream obj(filename);
    if (!obj.is_open()) {
        std::cerr << "Не удалось загрузить obj файл" << std::endl;
        return;
    }

    std::stringstream sstr;
    sstr << obj.rdbuf();

    std::string str;
    GLfloat numb;
    GLint intNumb;

    while (sstr >> str) {
        if (str == "v") { // Если строка начинается с 'v' (вершина)
            sstr >> numb; // Чтение координаты X
            verts[num].push_back(numb);
            sstr >> numb; // Чтение координаты Y
            verts[num].push_back(numb);
            sstr >> numb; // Чтение координаты Z
            verts[num].push_back(numb);
        }

        if (str == "vn") { // Если строка начинается с 'vn' (нормаль)
            sstr >> numb; // Чтение нормали по X
            normals[num].push_back(numb);
            sstr >> numb; // Чтение нормали по Y
            normals[num].push_back(numb);
            sstr >> numb; // Чтение нормали по Z
            normals[num].push_back(numb);
        }

        if (str == "vt") { // Если строка начинается с 'vt' (текстурная координата)
            sstr >> numb; // Чтение текстурной координаты U
            texs[num].push_back(numb);
            sstr >> numb; // Чтение текстурной координаты V
            texs[num].push_back(numb);
        }

        if (str == "f") { // Если строка начинается с 'f' (фигура)
            for (int i = 0; i < 3; ++i) { // Обработка треугольника (3 вершины)
                sstr >> intNumb; // Чтение индекса вершины
                indVerts[num].push_back(intNumb - 1); // Сохранение индекса вершины (с учетом смещения)
                sstr >> str; // Чтение текстурного индекса
                if (str[0] == '/' && str[1] == '/') { // Если текстурный индекс отсутствует
                    indNorms[num].push_back(stoi(str.substr(2)) - 1); // Сохранение индекса нормали
                    indTexs[num].push_back(-1); // Отметка отсутствия текстурного индекса
                }
                else { // Если текстурный индекс присутствует
                    indTexs[num].push_back(stoi(str.substr(1, str.find('/', 1) - 1)) - 1); // Сохранение текстурного индекса
                    indNorms[num].push_back(stoi(str.substr(str.find('/', str.find('/', 1)) + 1)) - 1); // Сохранение индекса нормали
                }
            }
        }
    }

    // Создание фигуры на основе загруженных данных
    figures.push_back(std::vector<GLfloat>());
    for (int i = 0; i < indVerts[num].size(); ++i) {
        // Добавление координат вершин
        figures[num].push_back(verts[num][3 * indVerts[num][i]]);
        figures[num].push_back(verts[num][3 * indVerts[num][i] + 1]);
        figures[num].push_back(verts[num][3 * indVerts[num][i] + 2]);

        // Добавление нормалей
        figures[num].push_back(normals[num][3 * indNorms[num][i]]);
        figures[num].push_back(normals[num][3 * indNorms[num][i] + 1]);
        figures[num].push_back(normals[num][3 * indNorms[num][i] + 2]);

        // Добавление текстурных координат
        if (indTexs[num][i] != -1) {
            figures[num].push_back(texs[num][2 * indTexs[num][i]]);
            figures[num].push_back(1.0 - texs[num][2 * indTexs[num][i] + 1]);
        }
        else {
            figures[num].push_back(0); // Если текстурные координаты отсутствуют, добавляем 0
            figures[num].push_back(0);
        }
    }
}

GLuint compileShader(const GLchar* source, GLenum type) {
    GLuint shader = glCreateShader(type); // Создание шейдера
    glShaderSource(shader, 1, &source, nullptr); // Привязка исходного кода шейдера
    glCompileShader(shader); // Компиляция шейдера

    GLint success;
    glGetShaderiv(shader, GL_COMPILE_STATUS, &success);
    if (!success) {
        char infoLog[512];
        glGetShaderInfoLog(shader, 512, nullptr, infoLog);
        std::cerr << "ERROR::SHADER::COMPILATION_FAILED\n" << infoLog << std::endl;
    }
    return shader; // Возврат ID скомпилированного шейдера
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

// Функция для создания фигуры (VAO и VBO)
void createShape(GLuint& VBO, GLuint& VAO, GLuint ind) {
    glGenVertexArrays(1, &VAO);
    glBindVertexArray(VAO);

    glGenBuffers(1, &VBO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);

    // Передача данных вершин в VBO
    glBufferData(GL_ARRAY_BUFFER, figures[ind].size() * sizeof(GLfloat), figures[ind].data(), GL_STATIC_DRAW);

    // Установка указателей для атрибутов вершин
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)0); // Позиции
    glEnableVertexAttribArray(0);

    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat))); // Нормали
    glEnableVertexAttribArray(1);

    glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(6 * sizeof(GLfloat))); // Текстурные координаты
    glEnableVertexAttribArray(2);

    glBindBuffer(GL_ARRAY_BUFFER, NULL);
    glBindVertexArray(0);
}

void initPlanets() {
    // Добавление планет с их параметрами (расстояние, размер, скорость орбиты, скорость вращения, углы)
    planets.push_back({ 16.0f, 0.6f, 0.02f, 0.5f, 0.0f, 0.0f });
    planets.push_back({ 25.0f, 0.8f, 0.015f, 0.4f, 0.0f, 0.0f });
}

int main() {
    sf::RenderWindow window(sf::VideoMode(800, 600), "Window");
    glewInit();
    glEnable(GL_DEPTH_TEST);

    GLuint vboCenter; // Переменная для хранения VBO центрального объекта ("Солнца")
    GLuint vaoCenter; // Переменная для хранения VAO центрального объекта ("Солнца")

    GLuint vboObj; // Переменная для хранения VBO планет
    GLuint vaoObj; // Переменная для хранения VAO планет

    GLuint texCenter; // Переменная для хранения текстуры центрального объекта ("Солнца")
    GLuint texObj; // Переменная для хранения текстуры планет

    int w = 512; // Ширина текстуры
    int h = 512; // Высота текстуры

    glEnable(GL_TEXTURE_2D); // Включение текстурирования


    // Создание шейдерной программы для Солнца
    GLuint shaderCenterProgram = createShaderProgram("center_vertex_shader.glsl", "center_fragment_shader.glsl");
    // Создание шейдерной программы для планет
    GLuint shaderObjectProgram = createShaderProgram("object_vertex_shader.glsl", "object_fragment_shader.glsl");

    // Загрузка моделей для Солнца и планет
    loadOBJ("dino.obj"); // Солнце
    loadOBJ("wolf.obj");   // Планеты

    // Создание VAO и VBO для Солнца и планет
    createShape(vboCenter, vaoCenter, 0);
    createShape(vboObj, vaoObj, 1);

    GLuint* tex = &texCenter;
    for (std::string filename : {"dino.png", "wolf.png"})
    {
        unsigned char* image = SOIL_load_image(filename.c_str(), &w, &h, 0, SOIL_LOAD_RGB);
        glGenTextures(1, tex);
        glBindTexture(GL_TEXTURE_2D, *tex);

        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT); // Установка параметров текстурирования
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, w, h, 0, GL_RGB, GL_UNSIGNED_BYTE, image); // Передача данных текстуры

        SOIL_free_image_data(image);
        tex = &texObj;
    }

    initPlanets();

    Camera camera(glm::vec3(0.0f, 0.0f, 10.0f)); // Начальная позиция камеры

    float sunRotationAngle = 0.0f; // Угол вращения Солнца

    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed) { window.close(); }
        }

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
            camera.rotateCamera(0.0f, 0.05f); // Поворот вверх
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::G)) // Клавиша для вертикального поворота вниз
            camera.rotateCamera(0.0f, -0.05f); // Поворот вниз

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        // Обновление угла вращения центрального объекта ("Солнца")
        sunRotationAngle += 0.1f;
        if (sunRotationAngle >= 360.0f)
            sunRotationAngle -= 360.0f; // Сброс угла вращения

        // Обновление матрицы вида
        glm::mat4 view = camera.getViewMatrix();
        glm::mat4 projection = glm::perspective(glm::radians(45.0f), 800.0f / 600.0f, 0.1f, 100.0f);
        glm::mat4 mvp = projection * view;

        // Отрисовка "Солнца"
        glUseProgram(shaderCenterProgram);
        glm::mat4 sunModel = glm::mat4(1.0f);
        sunModel = glm::rotate(sunModel, glm::radians(sunRotationAngle), glm::vec3(0.0f, 1.0f, 0.0f));
        glUniformMatrix4fv(glGetUniformLocation(shaderCenterProgram, "matr"), 1, GL_FALSE, glm::value_ptr(mvp * sunModel));

        glBindVertexArray(vaoCenter);
        glBindTexture(GL_TEXTURE_2D, texCenter);
        glDrawArrays(GL_TRIANGLES, 0, figures[0].size() / 8);

        // Обновление и отрисовка планет
        for (auto& planet : planets) {
            planet.orbitAngle += planet.orbitSpeed; // Обновление угла орбиты
            if (planet.orbitAngle > 360.0f)
                planet.orbitAngle -= 360.0f;

            planet.rotationAngle += planet.rotationSpeed; // Обновление угла вращения
            if (planet.rotationAngle > 360.0f)
                planet.rotationAngle -= 360.0f; // Сброс угла вращения

            glm::mat4 planetModel = glm::mat4(1.0f);
            planetModel = glm::translate(planetModel, glm::vec3(cos(glm::radians(planet.orbitAngle)) * planet.distance, 0.0f, sin(glm::radians(planet.orbitAngle)) * planet.distance)); // Положение на орбите
            planetModel = glm::scale(planetModel, glm::vec3(planet.size)); // Установка размера планеты

            // Установка вращения планеты вокруг своей оси
            planetModel = glm::rotate(planetModel, glm::radians(planet.rotationAngle), glm::vec3(0.0f, 1.0f, 0.0f));

            glUseProgram(shaderObjectProgram);
            glUniformMatrix4fv(glGetUniformLocation(shaderObjectProgram, "matr"), 1, GL_FALSE, glm::value_ptr(mvp * planetModel)); // Установка MVP матрицы
            glBindVertexArray(vaoObj);
            glBindTexture(GL_TEXTURE_2D, texObj); // Установка текстуры планеты
            glDrawArrays(GL_TRIANGLES, 0, figures[1].size() / 8); // Отрисовка планеты
        }

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
