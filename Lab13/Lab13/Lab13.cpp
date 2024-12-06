#include <SFML/Graphics.hpp>

int main()
{
    // Создаём окно
    sf::RenderWindow window(sf::VideoMode(200, 200), "Window");

    // Главный цикл
    while (window.isOpen())
    {
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

    return 0;
}