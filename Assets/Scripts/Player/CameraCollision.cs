using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [SerializeField]
    private Transform player; // El jugador que la c�mara sigue.

    [SerializeField]
    private LayerMask collisionLayers; // Capas con las que la c�mara puede colisionar.

    [SerializeField]
    private float minDistance = 0.5f; // Distancia m�nima de la c�mara al jugador.

    [SerializeField]
    private float maxDistance = 5f; // Distancia m�xima de la c�mara al jugador.

    [SerializeField]
    private float smoothSpeed = 10f; // Velocidad de interpolaci�n.

    private Vector3 desiredPosition;
    private Vector3 smoothVelocity;

    void LateUpdate()
    {
        // Determinar la posici�n deseada de la c�mara
        desiredPosition = player.position - transform.forward * maxDistance;

        // Realizar un raycast desde el jugador hacia la posici�n deseada de la c�mara
        if (Physics.Raycast(player.position, -transform.forward, out RaycastHit hit, maxDistance, collisionLayers))
        {
            // Si hay colisi�n, ajustar la posici�n deseada a la posici�n del impacto
            desiredPosition = player.position - transform.forward * (hit.distance - minDistance);
        }

        // Suavizar la posici�n de la c�mara
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref smoothVelocity, smoothSpeed * Time.deltaTime);
    }
}
