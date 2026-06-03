
# Taller AutoNova

Taller AutoNova es un sistema de gestión para talleres automotrices desarrollado como proyecto académico. Su objetivo es optimizar la administración de citas, usuarios, vehículos, repuestos y mantenimientos, facilitando el trabajo de administradores, recepcionistas y mecanicos mediante una plataforma centralizada.

El proyecto sigue principios de arquitectura limpia y aplica patrones de diseño GoF para mejorar la mantenibilidad, escalabilidad y organización del código.



## Authors

- [@Valeria Rivera](https://www.github.com/Valeria-Riv) 
- [@Darlink Aguilar](https://www.github.com/darlink-aguilar) 
- [@Luis Guillermo Piedrahita](https://www.github.com/PIEDLA) 
- [@Andres Villa Marin](https://www.github.com/AndresVM1) 


## Tecnologías Utilizadas

| Categoría | Tecnologías |
|-----------|------------|
| ⚙️ Backend | C#, .NET, ASP.NET Core Web API, Entity Framework Core |
| 🎨 Frontend | React|
| 🗄️ Base de Datos | SQL Server |
| 🔧 Herramientas | Git, GitHub, Visual Studio, VS Code |
| 🏛️ Arquitectura | Clean Architecture |
| 🎯 Patrones GoF | Factory Method, Strategy, State, Observer |
## Funcionalidades Implementadas

- [x] Gestión de Usuarios
- [x] Gestión de Vehículos
- [x] Gestión de Citas
- [x] Gestión de Mantenimientos


## Estructura del Proyecto

```bash
TallerAutonova/
├── TallerAutonova.Domain/
│   ├── Entities/
│   ├── Enums/
│   ├── Interfaces/
│   │   ├── Repositories/
│   │   └── Services/
│   ├── Services/
│   └── Patterns/
│       ├── FactoryMethod/
│       ├── Strategy/
│       ├── State/
│       └── Observer/
├── TallerAutonova.DataAccess/
│   ├── Context/
│   └── Repositories/
└── TallerAutonova.API/
    ├── Controllers/
    ├── DTOs/
    └── Mappings/
```


## Requisitos de Instalacion
 - .NET SDK
 - SQL Server
 - Visual Studio 2022 o superior
 - Git
## Instalación y ejecución

Clona el repositorio

```bash
  git clone https://github.com/darlink-aguilar/TallerAutonova.git
```


Restaurar paquetes
```bash
  dotnet restore
```
Aplicar migraciones
```bash
  dotnet ef database update --project TallerAutonova.DataAccess --startup-project TallerAutonova.API
```

1. Ejecutar el BackEnd

Abrir la solución del backend en Visual Studio, luego abrir una terminal en la raíz del proyecto.

Ingresar a la carpeta de la API

```bash
  cd AutoNova.API
```
Iniciar el servidor

```bash
  dotnet run
```
2. Ejecutar el Frontend

Abrir el proyecto frontend en Visual Studio Code y abrir una terminal en la raíz del proyecto.

Iniciar el servidor de desarrollo:

```bash
  npm run dev
```


## Evidencias del MVP

### Inicio
![Inicio](Images/Inicio.jpeg)

### Administrador
![Administrador](Images/Admin.jpeg)

### Mecánico
![Mecánico](Images/Mecanico.jpeg)

### Recepcionista
![Recepcionista](Images/Recep.jpeg)