# Gasolinera

## Descripción
Este proyecto simula el sistema de gestión de una gasolinera. Incluye funcionalidades para el manejo de bombas de combustible, transacciones de abastecimiento, y la generación y visualización de estadísticas relacionadas con las operaciones diarias de la gasolinera.

## Funcionalidades
- **Gestión de Bombas:** Activación y desactivación de bombas para el abastecimiento de combustible.
- **Transacciones de Abastecimiento:** Registro de cada operación de abastecimiento con detalles como fecha, cliente, cantidad, precio por litro y tipo de abastecimiento.
- **Estadísticas:** Generación de informes sobre el uso de las bombas y las transacciones, con capacidad para filtrar por fecha y mostrar datos acumulativos.
- **Interfaz de Usuario:** Una interfaz gráfica que permite interactuar con el sistema, configurar el puerto serial para la conexión con hardware externo, iniciar y terminar el abastecimiento, y visualizar estadísticas.

## Tecnologías Utilizadas
- C#
- Windows Forms para la interfaz de usuario
- .NET Framework
- Json
  
## Estructura del Proyecto
- `Bomba.cs`: Define la clase `Bomba` para manejar el estado y operaciones de una bomba.
- `ComandoBomba.cs`: Utilizado para enviar comandos a las bombas.
- `Estadisticas.cs`: Contiene métodos estáticos para generar diferentes informes de transacciones.
- `TransaccionAbastecimiento.cs`: Clase para manejar los detalles de las transacciones de abastecimiento.
- `PanelCentral.cs`: Gestiona las bombas y transacciones, y realiza cálculos de estadísticas.
- `Form1.cs`: Clase principal de la interfaz de usuario que interacciona con `PanelCentral` y maneja la lógica de la interfaz.

## Configuración y Uso
### Configuración Inicial
1. Clonar el repositorio.
2. Abrir el proyecto en Visual Studio.
3. Compilar y ejecutar el proyecto.

### Uso
- La interfaz principal permite seleccionar bombas, iniciar y terminar el abastecimiento, ingresar detalles del cliente y del tipo de abastecimiento.
- Las estadísticas se pueden visualizar en una pestaña dedicada, permitiendo seleccionar rangos de fecha para filtrar los resultados.

## Desarrollo Futuro
- Mejorar la integración con hardware externo para una simulación más realista.
- Implementar más características de seguridad y autenticación de usuarios.
- Extender las capacidades de reporte y análisis de datos.

## Autores
- Roberto José Ramos López
- Carlos Roberto Miranda Rios
- Shairy Analí Gómez Castillo
