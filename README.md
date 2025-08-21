# **Bienvenido a la Prueba Técnica de Million** 👋

Este proyecto ha sido desarrollado como parte de la prueba técnica para Million. A continuación, se detallan las tecnologías elegidas y los pasos para su ejecución.

---

## **Elecciones Tecnológicas y sus Razones**

### **Backend: .NET 8**

He elegido **.NET 8** por ser una versión de **soporte a largo plazo (LTS)**. Esta versión ofrece soporte y actualizaciones críticas (parches de seguridad y correcciones de errores) durante tres años después de su lanzamiento, lo que garantiza una mayor estabilidad y fiabilidad. A diferencia de .NET 9, que solo cuenta con 18 meses de soporte, .NET 8 ha tenido más tiempo en el mercado, lo que ha permitido que la mayoría de los errores importantes sean detectados y corregidos. Por estos factores, considero que es más prudente utilizar una versión LTS.

### **Frontend: Next.js**

Para el frontend, opté por **Next.js**. La razón principal es que simplifica enormemente el desarrollo al encargarse automáticamente del enrutamiento (_routing_), la optimización y la gestión del rendimiento que normalmente se deben configurar de forma manual con solo React.js. En este caso, prioricé la **practicidad** sobre el control manual de estos aspectos.

---

## **Preparación y Pasos para la Ejecución**

Para facilitar la configuración, he incluido un **DbSeeder** llamado `MillionTestDbSeeder` que utiliza la biblioteca **Bogus** para generar datos de prueba realistas. Este seeder crea nombres falsos y les asigna imágenes reales de forma automática.

### **Pasos de Configuración:**

1.  **Configurar el Seeder:**
    * Abre el archivo `Program.cs` del proyecto `MillionTestDbSeeder`.
    * Modifica el _string_ de conexión y el nombre de la base de datos. La aplicación se encargará de crear las colecciones automáticamente.
    * Ejecuta la aplicación `MillionTestDbSeeder`. Cuando se te solicite, ingresa el número de registros que deseas generar (por ejemplo: `3000`).
    * Espera a que el proceso termine; no suele tardar más de 2 minutos, aunque el tiempo variará según la cantidad de registros que elijas.

2.  **Ejecutar las Pruebas Unitarias:**
    * Verifica las pruebas unitarias. Notarás que dos pruebas en los casos `'OK'` de ambos controladores fallarán inicialmente.
    * Esto es intencional, ya que debes **modificar los IDs** que se consultan en las pruebas para que coincidan con un registro existente en tu base de datos.

3.  **Ejecutar la API y la Aplicación Web:**
    * Ahora puedes ejecutar la **API** y probarla usando **Swagger**.
    * Si lo prefieres, puedes ir directamente a la aplicación frontend.

4.  **Iniciar el [Frontend](https://github.com/jeremymartini/million-test-nextjs):**
    * Abre una terminal en la carpeta `"million-test-js"` y ejecuta el comando `npm run dev`.
    * Abre en tu navegador la URL que te indique la consola.
