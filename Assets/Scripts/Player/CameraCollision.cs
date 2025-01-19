using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [SerializeField]
    private Transform player; // El jugador que la cámara sigue.

    [SerializeField]
    private LayerMask collisionLayers; // Capas con las que la cámara puede colisionar.

    [SerializeField]
    private float minDistance = 0.5f; // Distancia mínima de la cámara al jugador.

    [SerializeField]
    private float maxDistance = 5f; // Distancia máxima de la cámara al jugador.

    [SerializeField]
    private float smoothSpeed = 10f; // Velocidad de interpolación.

    private Vector3 desiredPosition;
    private Vector3 smoothVelocity;

    void LateUpdate()
    {
        // Determinar la posición deseada de la cámara
        desiredPosition = player.position - transform.forward * maxDistance;

        // Realizar un raycast desde el jugador hacia la posición deseada de la cámara
        if (Physics.Raycast(player.position, -transform.forward, out RaycastHit hit, maxDistance, collisionLayers))
        {
            // Si hay colisión, ajustar la posición deseada a la posición del impacto
            desiredPosition = player.position - transform.forward * (hit.distance - minDistance);
        }

        // Suavizar la posición de la cámara
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref smoothVelocity, smoothSpeed * Time.deltaTime);
    }
}
