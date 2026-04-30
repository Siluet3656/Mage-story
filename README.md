--- README.md (原始)
# Unity Game Project

Проект игры, разработанный на игровом движке **Unity 2020.1.2f1**.

## 📋 Описание

Это 2D-игра, включающая систему управления персонажами, боевую систему с заклинаниями, систему статусов и эффекты.

## 🚀 Требования

- **Unity**: 2020.1.2f1 или совместимая версия
- **Платформа**: Windows, macOS, Linux

## 📁 Структура проекта

```
Assets/
├── Animations/          # Анимации персонажей и объектов
├── Materials/           # Материалы для рендеринга
├── Pallets/            # Тайловые карты (Tilemap)
├── Prefabs/            # Префабы игровых объектов
├── Resources/          # Ресурсы, загружаемые во время выполнения
├── Scenes/             # Сцены игры
├── ScriptableObjects/  # ScriptableObject ассеты с данными
├── Scripts/            # Исходный код проекта
│   ├── AllyStaff/      # Логика союзников
│   ├── Animations/     # Управление анимациями
│   ├── Data/           # Структуры данных
│   ├── Debugging/      # Отладочные утилиты
│   ├── Effects/        # Визуальные и звуковые эффекты
│   ├── EnemyStaff/     # Логика противников
│   ├── EntityStaff/    # Базовая логика сущностей
│   ├── ForShaders/     # Скрипты для шейдеров
│   ├── GameControl/    # Управление игрой
│   ├── Pathfinding/    # Система поиска пути
│   ├── PlayerStaff/    # Логика игрока
│   ├── Shard/          # Система фрагментов/осколков
│   ├── Spells/         # Система заклинаний
│   ├── Statuses/       # Система статусов и эффектов
│   └── View/           # Представление и UI
├── Shaders/            # Пользовательские шейдеры
└── Sprites/            # Спрайты и текстуры

ProjectSettings/        # Настройки проекта Unity
Packages/              # Зависимости Unity Package Manager
```

## 🎮 Основные возможности

- **Система персонажей**: Игрок, союзники и противники
- **Боевая система**: Заклинания и способности
- **Система статусов**: Баффы, дебаффы и эффекты состояния
- **Pathfinding**: Поиск пути для ИИ
- **Анимации**: Система анимаций персонажей и эффектов
- **ScriptableObjects**: Гибкая система данных игры

## 🛠️ Установка и запуск

1. Клонируйте репозиторий:
   ```bash
   git clone <repository-url>
   cd <project-folder>
   ```

2. Откройте проект в Unity Hub:
   - Запустите Unity Hub
   - Нажмите "Add" и выберите папку проекта
   - Дождитесь импорта ассетов и компиляции скриптов

3. Запустите сцену:
   - Откройте сцену из папки `Assets/Scenes/`
   - Нажмите Play в редакторе Unity

## 📦 Пакеты

Проект использует стандартные пакеты Unity. Список пакетов доступен в файле `Packages/manifest.json`.

## ⚙️ Настройки проекта

Основные настройки проекта находятся в папке `ProjectSettings/`:
- Физика 2D настроена в `Physics2DSettings.asset`
- Настройки качества в `QualitySettings.asset`
- Настройки ввода в `InputManager.asset`

## 📝 Лицензия

[Укажите лицензию вашего проекта]

## 👥 Авторы

[Укажите авторов проекта]

## 📞 Контакты

[Укажите контактную информацию]

+++ README.md (修改后)
# Unity Game Project

A comprehensive Unity game project featuring advanced character systems, combat mechanics, and spell management.

## Project Details

- **Unity Version**: 2020.1.2f1
- **Author**: SiluetSalat
- **License**: MIT

## Features

- **Character System**: Advanced player and NPC management
- **Combat System**: Real-time battle mechanics
- **Spell System**: Dynamic spell casting and effects
- **Status Effects**: Buffs, debuffs, and status management
- **Pathfinding**: AI navigation and movement

## Project Structure

```
├── Assets/                  # Main assets directory
│   ├── Scripts/             # C# scripts
│   ├── Scenes/              # Unity scenes
│   ├── Prefabs/             # Reusable game objects
│   ├── Materials/           # Material assets
│   ├── Textures/            # Image assets
│   ├── Models/              # 3D models
│   ├── Animations/          # Animation clips
│   └── Audio/               # Sound files
├── Packages/                # Unity packages
├── ProjectSettings/         # Project configuration
└── README.md                # This file
```

## Getting Started

### Prerequisites

- Unity Hub (latest version)
- Unity 2020.1.2f1
- Git

### Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd <project-directory>
   ```

2. Open the project in Unity Hub:
   - Click "Add" button
   - Navigate to the project folder
   - Select the folder and click "Add Project"

3. Open the project in Unity 2020.1.2f1

4. Wait for asset import and compilation to complete

### Running the Game

1. Open the main scene from `Assets/Scenes/`
2. Press the Play button in the Unity Editor
3. Use default controls to interact with the game

## Controls

- **WASD**: Movement
- **Mouse**: Camera control and targeting
- **Left Click**: Attack/Interact
- **Right Click**: Cast spell/Block
- **1-5**: Quick spell slots
- **Space**: Jump/Dodge
- **Esc**: Pause menu

## Development

### Recommended IDE

- **JetBrains Rider** (recommended)
- Visual Studio 2019+

### Code Style

- Follow C# naming conventions
- Use XML documentation comments
- Maintain consistent indentation (4 spaces)

## License

This project is licensed under the MIT License - see below for details:

Copyright (c) 2024 SiluetSalat

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

## Contact

- **Discord**: @siluetsalat
- **Author**: SiluetSalat

## Acknowledgments

- Unity Technologies for the game engine
- All contributors and supporters