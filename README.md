# Nombre del Proyecto

## Descripción

**Nombre del Proyecto** es una aplicación web diseñada para facilitar la gestión de turnos para negocios como peluquerías. Los negocios pueden crear cuentas de usuario, gestionar su calendario de turnos, y permitir a sus clientes reservar turnos disponibles, elegir servicios, y pagar por ellos directamente desde la aplicación.

## Características

- **Registro de Negocios:** Permite a los negocios crear y gestionar su perfil.
- **Gestión de Turnos:** Los negocios pueden crear y organizar turnos disponibles.
- **Reserva de Turnos:** Los clientes pueden ver los turnos disponibles y realizar reservas.
- **Selección de Servicios:** Los clientes pueden seleccionar los servicios que desean recibir.
- **Pago en Línea:** Opcionalmente, los clientes pueden pagar por los servicios a través de la aplicación.

## Tecnologías Utilizadas

- **Backend:** .NET Core, ASP.NET
- **Base de Datos:** MySQL
- **Control de Versiones:** GitHub
- **Despliegue:** por determinar

## Instalación y Configuración

1. **Clonar el repositorio:**

   ```bash
   git clone https://github.com/alanmoex/Turnos.git
2. **Configurar el entorno de desarrollo:**

    Asegúrate de tener las siguientes herramientas instaladas:
    * .NET Core SDK
    * MySQL

3. **Instalar dependencias:**

    ```bash
    cd backend
    dotnet restore
4. **Ejecutar la aplicación:**
   
    ```bash
    dotnet run
## Resumen del Workflow
* Rama `main`: Contiene el código en producción. Solo se fusionan aquí los cambios aprobados y testeados.
* Rama `development`: Base para la integración de nuevas características. Todas las nuevas funcionalidades y correcciones se desarrollan en ramas separadas creadas desde aquí.
* Ramas de características (`feature`): Utilizadas para desarrollar nuevas funcionalidades.
* Ramas de corrección (`bugfix`): Utilizadas para corregir errores en la rama development.
* Ramas de `hotfix`: Utilizadas para solucionar problemas críticos en producción.

Para mas informacion acerca del flujo de trabajo consulta el archivo [WORKFLOW](/docs/WORKFLOW.md)


## Licencia
Este proyecto está licenciado bajo la Licencia Apache 2.0. Consulta el archivo [LICENSE](LICENSE) para más detalles.
