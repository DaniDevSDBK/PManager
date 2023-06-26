# PManager

Descripción breve del proyecto en una o dos frases.

## Descripción

Este proyecto es un ejemplo de una aplicación sencilla creada como parte de un proyecto escolar. La aplicación [KeyVault] permite [gestionar tus usuarios y contraseñas]. Fue desarrollada como una demostración para ilustrar los conceptos aprendidos durante el curso.

## Características

- [Generador de Contraseñas]: Genera Contraseñas Aleatorias.
- [Guarda tus Contraseñas]: Guarda las contraseñas utilizando RSA.
- [2 roles de usuario]: (Planteado pero no implementado del todo).

## Capturas de Pantalla
<!DOCTYPE html>
<html>
<head>
  <style>
  
    .slider-container {
      width: 500px;
      height: 300px;
      overflow: hidden;
      position: relative;
    }

   
    .slider-image {
      width: 100%;
      height: 100%;
      object-fit: cover;
      transition: transform 0.3s ease-in-out;
    }

    
    .slider-nav {
      position: absolute;
      top: 50%;
      transform: translateY(-50%);
      width: 40px;
      height: 40px;
      background-color: rgba(0, 0, 0, 0.5);
      color: #fff;
      display: flex;
      justify-content: center;
      align-items: center;
      cursor: pointer;
    }

    .slider-nav.right {
      right: 0;
    }
  </style>
</head>
<body>
  <div class="slider-container">
    <img class="slider-image" src="image1.jpg" alt="Image 1">
    <img class="slider-image" src="image2.jpg" alt="Image 2">
    <img class="slider-image" src="image3.jpg" alt="Image 3">
    <div class="slider-nav" onclick="prevSlide()">&#10094;</div>
    <div class="slider-nav right" onclick="nextSlide()">&#10095;</div>
  </div>

  <script>
    var currentSlide = 0;
    var slides = document.getElementsByClassName("slider-image");

    function prevSlide() {
      slides[currentSlide].style.transform = "translateX(100%)";
      currentSlide = (currentSlide - 1 + slides.length) % slides.length;
      slides[currentSlide].style.transform = "translateX(-100%)";
      setTimeout(function () {
        slides[currentSlide].style.transform = "translateX(0)";
      }, 100);
    }
    
    function nextSlide() {
      slides[currentSlide].style.transform = "translateX(-100%)";
      currentSlide = (currentSlide + 1) % slides.length;
      slides[currentSlide].style.transform = "translateX(100%)";
      setTimeout(function () {
        slides[currentSlide].style.transform = "translateX(0)";
      }, 100);
    }
  </script>
</body>
</html>

## Tecnologías Utilizadas

- [Tecnología 1]: Descripción breve de la tecnología 1.
- [Tecnología 2]: Descripción breve de la tecnología 2.
- [Tecnología 3]: Descripción breve de la tecnología 3.

## Instalación

1. Clona el repositorio: `git clone https://github.com/tu-usuario/tu-repositorio.git`
2. Abre el proyecto en tu entorno de desarrollo preferido.
3. Configura las dependencias y entorno de ejecución según sea necesario.
4. Ejecuta la aplicación.

## Contribución

Las contribuciones son bienvenidas. Si deseas mejorar este proyecto, sigue estos pasos:

1. Haz un Fork del proyecto
2. Crea una nueva rama (`git checkout -b feature/nueva-funcionalidad`)
3. Realiza los cambios necesarios y realiza los commits (`git commit -am 'Agrega una nueva funcionalidad'`)
4. Haz push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Abre un Pull Request

## Créditos

Este proyecto fue desarrollado por [tu nombre] como parte de [nombre del curso o proyecto]. Puedes contactarme en [tu dirección de correo electrónico] si tienes alguna pregunta o sugerencia.

## Licencia

Este proyecto se distribuye bajo la Licencia [nombre de la licencia]. Consulta el archivo LICENSE para más información.
