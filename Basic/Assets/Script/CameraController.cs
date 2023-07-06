using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Clase para controlar la rotaci�n de la c�mara en un player FPS
/// Dise�o del player: Capsula como gameobject padre con un script para moverse con WASD
/// C�mara como hijo con esta clase como componente
/// �Dudas? Discord: https://discord.gg/6gudMeSZW4
/// Twitter: @ElenaImagineer
/// </summary>
public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;//"velocidad" de movimiento de nuestra c�mara
    public float clampAngle = 80;//�ngulo para limitar la rotaci�n en el eje X
    Quaternion localRotation;

    float rotY = 0;//para guardarme la rotaci�n en Y
    float rotX = 0;//para guardarme la rotaci�n en X

    void Start()
    {
        //estoy cogiendo la rotaci�n inicial
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");//movimiento horizontal
        float mouseY = Input.GetAxis("Mouse Y");//movimiento vertical

        rotY += mouseX * mouseSensitivity * Time.deltaTime;//rotaci�n en eje Y de la c�mara a trav�s del mov horizontal del rat�n
        rotX -= mouseY * mouseSensitivity * Time.deltaTime;//rotaci�n en eje X de la c�mara a trav�s del mov vertical del rat�n

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);//l�mite inferior y superior del �ngulo

        localRotation = Quaternion.Euler(rotX, 0, 0);//giro la c�mara en eje X
    }
    void FixedUpdate()
    {
        transform.localRotation = localRotation;
        transform.parent.rotation = Quaternion.Euler(0, rotY, 0);//giro la capsula (y por lo tanto su hijo que es la
        //c�mara en el eje Y)
    }
}
