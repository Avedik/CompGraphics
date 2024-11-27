#include <GL/glew.h>
#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <fstream>
#include <sstream>

// ×åòûð¸õóãîëüíèê
const std::vector<GLfloat> quadVertices = {
    -0.5f, -0.5f,     1.0f, 0.0f, 0.0f,
     0.5f, -0.5f,     0.0f, 1.0f, 0.0f,
     0.5f,  0.5f,     0.0f, 0.0f, 1.0f,
    -0.5f,  0.5f,     1.0f, 1.0f, 0.0f
};

// Âååð
const std::vector<GLfloat> fanVertices = {
      0.0f, 0.0f,    1.0f, 0.0f, 0.0f,
     -0.6f, 0.18f,   0.0f, 1.0f, 0.0f,
     -0.4f, 0.4f,    0.0f, 0.0f, 1.0f,
      0.0f, 0.5f,    1.0f, 1.0f, 0.0f,
      0.5f, 0.5f,    1.0f, 0.0f, 1.0f,
      0.8f, 0.0f,    0.0f, 1.0f, 1.0f
};

// Ïðàâèëüíûé ïÿòèóãîëüíèê
const std::vector<GLfloat> pentaVertices = {
     0.0f,   1.0f,   1.0f, 0.0f, 0.0f,
     0.95f,  0.31f,  0.0f, 1.0f, 0.0f,
     0.59f, -0.81f,  0.0f, 0.0f, 1.0f,
    -0.59f, -0.81f,  1.0f, 1.0f, 0.0f,
    -0.95f,  0.31f,  1.0f, 0.0f, 1.0f,
};

// ×òåíèå èç ôàéëà
std::string readAllFile(const std::string filename)
{
    std::fstream fs(filename);
    std::ostringstream sstr;
    sstr << fs.rdbuf();
    return sstr.str();
}

// Ôóíêöèÿ äëÿ êîìïèëÿöèè øåéäåðà
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

// Ôóíêöèÿ äëÿ ñîçäàíèÿ øåéäåðíîé ïðîãðàììû
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


// Ôóíêöèÿ äëÿ ïîëó÷åíèÿ ïðîãðàììû ïî èíäåêñó
GLuint getShaderProgram(GLuint count) {

    // Ïëîñêîå çàêðàøèâàíèå (const)
    if (count == 0 || count == 3 || count == 6)
        return createShaderProgram("const_shader.glsl");

    // Ïëîñêîå çàêðàøèâàíèå ÷åðåç uniform
    else if (count == 1 || count == 4 || count == 7)
        return createShaderProgram("uniform_shader.glsl");

    // Ãðàäèåíòíîå çàêðàøèâàíèå
    else
        return createShaderProgram("fragment_gradient_shader.glsl");
}

// Ôóíêöèÿ äëÿ ñîçäàíèÿ ôèãóð
void createShape(GLuint& VBO, const std::vector<GLfloat>& vertices) {
    glGenBuffers(1, &VBO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(GLfloat), vertices.data(), GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, NULL);
}

// Ôóíêöèÿ äëÿ ïîëó÷åíèÿ ôèãóðû ïî èíäåêñó
void getShape(GLuint& VBO, GLuint count) {

    // ×åòûðåõóãîëüíèê
    if (count == 0)
        createShape(VBO, quadVertices);

    // Âååð
    else if (count == 3)
        createShape(VBO, fanVertices);

    // Ïðàâèëüíûé ïÿòèóãîëüíèê
    else
        createShape(VBO, pentaVertices);
}



int main() {

    sf::RenderWindow window(sf::VideoMode(500, 500), "Window");
    glewInit();

    GLuint VBO;
    GLuint count = 0;

    // Ïîëó÷åíèå ïåðâîé ïðîãðàììû è ôèãóðû
    GLuint shaderProgram = getShaderProgram(count);
    getShape(VBO, count);

    // Ãëàâíûé öèêë
    while (window.isOpen()) {

        sf::Event event;
        // Öèêë îáðàáîòêè ñîáûòèé
        while (window.pollEvent(event))
        {
            // Ñîáûòèå çàêðûòèÿ îêíà
            if (event.type == sf::Event::Closed) { window.close(); }
            // Ñîáûòèå íàæàòèÿ êíîïêè ìûøè â îêíå
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

        // Ñîîáùàåì OpenGL êàê îí äîëæåí èíòåðïðåòèðîâàòü âåðøèííûå äàííûå
        glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(GLfloat), (void*)0); // Ïîçèöèÿ
        glEnableVertexAttribArray(0);

        // Ñîîáùàåì OpenGL êàê îí äîëæåí èíòåðïðåòèðîâàòü öâåòîâûå äàííûå
        glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(GLfloat), (void*)(2 * sizeof(GLfloat))); // Öâåò
        glEnableVertexAttribArray(1);

        // Ïîëó÷àåì ëîêàòîð uniform ïåðåìåííîé
        GLuint colorLocation = glGetUniformLocation(shaderProgram, "uniformcolor");
        // Óñòàíàâëèâàåì öâåò
        glUseProgram(shaderProgram);
        glUniform3f(colorLocation, 1.0f, 0.0f, 0.0f); // Çàäàåì öâåò êàê êðàñíûé


        if (count == 1 || count == 4 || count == 7)
            glUniform4f(glGetUniformLocation(shaderProgram, "uniformColor"), 0.5f, 0.5f, 0.5f, 1.0f);
        
        glBindBuffer(GL_ARRAY_BUFFER, NULL);

        // Ïåðåäàåì äàííûå íà âèäåîêàðòó (ðèñóåì)

        // ×åòûð¸õóãîëüíèê
        if (count == 0 || count == 1 || count == 2)
            glDrawArrays(GL_QUADS, 0, 4);
        // Âååð
        else if (count == 3 || count == 4 || count == 5)
            glDrawArrays(GL_TRIANGLE_FAN, 0, 6);
        // Ïðàâèëüíûé ïÿòèóãîëüíèê
        else
            glDrawArrays(GL_POLYGON, 0, 5);

        // Ïåðåðèñîâêà îêíà
        window.display();
    }

    // Î÷èñòêà ðåñóðñîâ
    glDeleteBuffers(1, &VBO);
    glUseProgram(0);
    glDeleteProgram(shaderProgram);

    return 0;
}
