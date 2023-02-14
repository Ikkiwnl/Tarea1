# IA para videojuegos
## 2021.3.16f1 LTS


Ejercicios:

    Sigilo (Obligatorio, 65 puntos): Diagrama: Sigilo. Debe haber dos tipos de agentes. Agente infiltrador y Agente Guardia.

El agente infiltrador: Cuando el usuario dé click en la pantalla, el infiltrador se dirigirá hacia la posición del click, con un Arrive (esto es indispensable para poder comprobar al guardia). Si se da click en otra posición mientras se desplaza, debe cambiar de objetivo hacia el click más reciente. Una vez que llegue a la posición del click, debe quedarse quieto hasta que se dé otro click.

Agente Guardia: Guardia en estado normal: Debe estar en una posición fija. Debe tener un cono de visión de área limitada, en la dirección en la que está "viendo". Debe rotar cada cierto tiempo. p.e. cada 5 segundos. La rotación puede ser en ángulos de 45, 90 u otros grados. Pero si se les ocurre algo mejor, inténtenlo. Si el Infiltrador entra a su área de visión, el guardia cambia a estado de alerta.

Guardia en estado de alerta. Su cono de visión se vuelve un poco más amplio, pero no más largo/profundo. Después de un pequeño periodo de tiempo, se acercará a la última posición donde "vió" al infiltrador, con un Arrive. Si al llegar ahí ya no ha visto al infiltrador, volverá a su posición inicial, y al llegar ahí cambiará nuevamente a estado normal. Si, al contrario, mientras está en alerta, el infiltrador pasa 1 segundo (tiempo total, no contínuo, es decir, por ejemplo, podrían ser 0.5 segundos primero, y luego otros 0.5 segundos mientras el guardia se mueve durante el estado de alerta) dentro del área de visión del guardia, el guardia pasará al estado de Ataque.

Guardia en estado de ataque. Utilizará el steering behavior de pursuit hacia el infiltrador durante 5 segundos. Si toca al infiltrador, lo destruye (deben poder re-aparecer al infiltrador después de que lo destruyan!), debe volver a su posición inicial y al hacerlo, volver al estado Normal. Si se acaban los 5 segundos y no destruyó al infiltrador, debe volver a su posición inicial y al hacerlo, volver al estado Normal.

    Obstáculos (Obligatorio, 50 puntos): Un solo tipo de agente. Diagrama: Semi-obstacle avoidance.

Agente que evita obstáculos.

El agente debe seguir un objetivo. Les recomiendo que el objetivo sea la posición del mouse, o la posición del mouse al hacer click, pero pueden intentar otras cosas si lo desean, y usar Arrive hacia dicho objetivo. En la escena debe haber obstáculos para el agente.

Los obstáculos se pueden poner en el editor de unity, pero si los pueden poner también durante el juego sería más interesante. El agente no puede atravesar los obstáculos (procuren ponerles colliders para que la física impida que los atraviese). El agente debe evitar chocar con los obstáculos. Para ello, cuando el obstáculo este a cierto rango/distancia apliquen un steering behavior de Flee a su agente, para que haga flee respecto a dicho obstáculo, mientras sigue con su arrive hacia el objetivo. 3. Patrullage (Opcional, 35 puntos): Un solo agente. Diagrama: 1.

El agente se moverá entre N puntos en la escena (Waypoints), de manera cíclica. Los waypoints en la escena deben poderse poner y quitar durante el juego, y mostrar el orden en que el agente los visitará. Poder poner y quitar waypoints con el mouse. Al llegar cerca del waypoint actual, el agente debe cambiar al siguiente en el orden. Al visitar el "último" waypoint, debe dirigirse hacia el primero, y repetir todo el ciclo. Si ya no hay waypoints, el agente debe frenar hasta quedar quieto.

3. Patrullage (Opcional, 35 puntos): Un solo agente.
Diagrama: 1.

El agente se moverá entre N puntos en la escena (Waypoints), de manera cíclica.
Los waypoints en la escena deben poderse poner y quitar durante el juego, y mostrar el orden en que el agente los visitará.
Poder poner y quitar waypoints con el mouse.
Al llegar cerca del waypoint actual, el agente debe cambiar al siguiente en el orden.
Al visitar el "último" waypoint, debe dirigirse hacia el primero, y repetir todo el ciclo.
Si ya no hay waypoints, el agente debe frenar hasta quedar quieto.

