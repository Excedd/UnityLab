using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Clase para controlar la rotación de la cámara en un player FPS
/// Diseño del player: Capsula como gameobject padre con un script para moverse con WASD
/// Cámara como hijo con esta clase como componente
/// ¿Dudas? Discord: https://discord.gg/6gudMeSZW4
/// Twitter: @ElenaImagineer
/// </summary>
public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;//"velocidad" de movimiento de nuestra cámara
    public float clampAngle = 80;//ángulo para limitar la rotación en el eje X
    Quaternion localRotation;

    float rotY = 0;//para guardarme la rotación en Y
    float rotX = 0;//para guardarme la rotación en X

    void Start()
    {
        //estoy cogiendo la rotación inicial
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");//movimiento horizontal
        float mouseY = Input.GetAxis("Mouse Y");//movimiento vertical

        rotY += mouseX * mouseSensitivity * Time.deltaTime;//rotación en eje Y de la cámara a través del mov horizontal del ratón
        rotX -= mouseY * mouseSensitivity * Time.deltaTime;//rotación en eje X de la cámara a través del mov vertical del ratón

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);//límite inferior y superior del ángulo

        localRotation = Quaternion.Euler(rotX, 0, 0);//giro la cámara en eje X
    }
    void FixedUpdate()
    {
        transform.localRotation = localRotation;
        transform.parent.rotation = Quaternion.Euler(0, rotY, 0);//giro la capsula (y por lo tanto su hijo que es la
        //cámara en el eje Y)
    }
}
