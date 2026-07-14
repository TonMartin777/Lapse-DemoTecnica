# Lapse, Demo Técnica: Práctica de Programación y Arquitectura

Este repositorio contiene un proyecto corto desarrollado en Unity, creado exclusivamente como pieza de portfolio técnico y entorno de aprendizaje.

## Objetivo del Proyecto

El propósito principal de este desarrollo **no** es ofrecer un videojuego completo o una experiencia jugable extensa, sino servir como un campo de pruebas para aplicar principios sólidos de programación, depuración de físicas y arquitectura de software.

Las partes más relevantes del proyecto són:
*   **Arquitectura Limpia:** Desacoplamiento de sistemas utilizando el Patrón Observer (UnityEvents) para evitar dependencias rígidas entre objetos interactuables.
*   **Patrones de Diseño:** Implementación del Patrón Singleton para centralizar gestores globales (como el estado del jugador o la interfaz) sin abusar de recursos en la jerarquía.
*   **Físicas y Matemáticas de Colisión:** Resolución de problemas inherentes al `CharacterController` de Unity. Esto incluye la sincronización de colisiones en plataformas móviles mediante la jerarquización de movimientos en el ciclo `FixedUpdate`, y la creación de fuerzas de anclaje algorítmicas para evitar saltos indeseados al descender por pendientes.

## Notas sobre el alcance

*   **Experiencia Corta:** Se trata de una prueba de concepto muy breve, pensada únicamente para demostrar la funcionalidad de las mecánicas y los *scripts*.
*   **Recursos de Terceros:** El apartado artístico, sonoro y creativo **no** es el objetivo de este proyecto. Todos los modelos 3D, texturas y efectos de sonido son *assets* de internet y bibliotecas gratuitas utilizados puramente como *placeholders* (marcadores visuales) para poder interactuar con el código.
*   **Enfoque:** La prioridad absoluta ha sido la limpieza del código en C#, la modularidad y el correcto funcionamiento del motor de físicas, dejando el diseño de niveles o la narrativa en un segundo plano.

## Tecnologías
*   **Motor:** Unity 3D
*   **Lenguaje:** C#
