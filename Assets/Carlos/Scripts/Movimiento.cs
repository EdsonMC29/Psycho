using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float velocidad;
    // Start is called before the first frame update
    Rigidbody2D rb;
    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // O GetComponent<Rigidbody>() en 3D
    }

    void Update()
    {
        ProcesarMovimiento();
    }
    void ProcesarMovimiento()
    {
        float movHoriz = Input.GetAxis("Horizontal");
        float movVerti = Input.GetAxis("Vertical");
        Vector2 movimiento = new Vector2(movHoriz, movVerti);
        rb.velocity = movimiento * velocidad;

    }
}
