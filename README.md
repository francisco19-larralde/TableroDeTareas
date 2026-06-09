📋 Tablero de Tareas
Sistema de gestión de tareas desarrollado en C# / .NET, con arquitectura en capas y cobertura de tests unitarios.

📌 Descripción
Aplicación de escritorio que permite crear, listar, actualizar y eliminar tareas. Cada tarea tiene un título, descripción, estado (pendiente / en progreso / completada) y fecha de vencimiento. 

🏗️ Arquitectura
El proyecto sigue una arquitectura en capas que separa responsabilidades:
TableroDeTareas/
├── Model/          → Entidades del dominio (Tarea, Estado, etc.)
├── Data/           → Acceso a datos y repositorios
├── DTOS/           → Objetos de transferencia de datos
├── Servicio/       → Lógica de negocio
├── Proyecto1/      → Interfaz de usuario (presentación)
└── TestTablero/    → Tests unitarios

🛠️ Tecnologías

Lenguaje: C#
Framework: .NET
Tests:  xUnit 
IDE: Visual Studio
Control de versiones: Git / GitHub


▶️ Cómo ejecutar

Cloná el repositorio:

bash   git clone https://github.com/francisco19-larralde/TableroDeTareas.git

Abrí la solución en Visual Studio:

   TableroDeTareas.slnx

Establecé Proyecto1 como proyecto de inicio.
Ejecutá con F5 o desde el menú Debug → Start Debugging.


🧪 Tests
Los tests se encuentran en la carpeta TestTablero/. Para ejecutarlos:

Desde Visual Studio: Test → Run All Tests
Desde la terminal:

bash  dotnet test

📚 Conceptos aplicados

Programación Orientada a Objetos (herencia, encapsulamiento, polimorfismo)
Patrón de arquitectura en capas
Uso de DTOs para desacoplar capas
Tests unitarios
Manejo de colecciones y LINQ


👨‍💻 Autor
Francisco Larralde
GitHub · LinkedIn
