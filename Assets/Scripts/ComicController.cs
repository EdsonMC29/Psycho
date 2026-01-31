using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicCameraController : MonoBehaviour
{
    [Header("Arrastra aquí los marcadores (V1, V2, V3...) en orden")]
    public Transform[] vinetas;

    [Header("Movimiento")]
    public bool movimientoSuave = true;
    public float velocidad = 6f; // más alto = más rápido

    private int index = 0;
    private float camZ;

    void Start()
    {
        camZ = transform.position.z;

        if (vinetas != null && vinetas.Length > 0)
        {
            // Coloca la cámara en la primera viñeta al iniciar
            Vector3 p = vinetas[index].position;
            transform.position = new Vector3(p.x, p.y, camZ);
        }
    }

    void Update()
    {
        if (vinetas == null || vinetas.Length == 0) return;

        // Flecha derecha -> siguiente viñeta
        if (Input.GetKeyDown(KeyCode.RightArrow))
            index = Mathf.Min(index + 1, vinetas.Length - 1);

        // Flecha izquierda -> viñeta anterior
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            index = Mathf.Max(index - 1, 0);

        Vector3 target = vinetas[index].position;
        target = new Vector3(target.x, target.y, camZ);

        if (movimientoSuave)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * velocidad);
        }
        else
        {
            transform.position = target; // salto instantáneo
        }
    }
}