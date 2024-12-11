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
    // else
        // return createShaderProgram("circle_vertex_shader.glsl", "circle_fragment_shader.glsl");
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

    //else
         //createShape(VBO, circle);
}

int main() {
    sf::RenderWindow window(sf::VideoMode(500, 500), "Window");
    glewInit();
    glEnable(GL_DEPTH_TEST);

    GLuint VBO;
    GLuint tex1;
    GLuint tex2;
    GLuint count = 0;
    int w = 512;
    int h = 512;
    float coef = 0.5;

    //glEnable(GL_TEXTURE_2D);
    glEnable(GL_DEPTH_TEST);
    GLuint shaderProgram = getShaderProgram(count);
    getShape(VBO, count);

    unsigned char* image1 = SOIL_load_image("container.jpg", &w, &h, 0, SOIL_LOAD_RGB);
    unsigned char* image2 = SOIL_load_image("container.jpg", &w, &h, 0, SOIL_LOAD_RGB);

    glGenTextures(1, &tex1);
    glGenTextures(1, &tex2);
    glBindTexture(GL_TEXTURE_2D, tex1);
    glTexImage2D(GL_TEXTURE_2D, 0, 3, w, h, 0, GL_RGB, GL_UNSIGNED_BYTE, image1);
    glBindTexture(GL_TEXTURE_2D, tex2);
    glTexImage2D(GL_TEXTURE_2D, 0, 3, w, h, 0, GL_RGB, GL_UNSIGNED_BYTE, image2);

    SOIL_free_image_data(image1);
    SOIL_free_image_data(image2);
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
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        glUseProgram(shaderProgram);

        glm::mat4 mvp = projection * view * model;
        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "matr"), 1, GL_FALSE, glm::value_ptr(mvp));

        glBindBuffer(GL_ARRAY_BUFFER, VBO);

        if (count == 0)
            glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (GLvoid*)0);
        else if (count == 1 || count == 2)
            glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)0);
        // else

        glEnableVertexAttribArray(0);

        if (count == 0)
            glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
        else if (count == 1 || count == 2)
            glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
        // else

        glEnableVertexAttribArray(1);

        if (count == 1 || count == 2)
        {
            glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(6 * sizeof(GLfloat)));
            glEnableVertexAttribArray(2);
        }

        if (count == 1 || count == 2)
        {
            glUniform1f(glGetUniformLocation(shaderProgram, "coef"), coef);
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

        //else

        window.display();
    }

    glDeleteBuffers(1, &VBO);
    glUseProgram(0);
    glDeleteProgram(shaderProgram);

    return 0;
}