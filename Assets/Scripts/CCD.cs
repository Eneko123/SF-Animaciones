
using UnityEngine;

public class CCD : MonoBehaviour
{
    [SerializeField] private Transform targetPos;
    [SerializeField] private Transform punta;
    [SerializeField] private Transform[] articulaciones;

    [SerializeField] private float offset = 0.25f;
    [SerializeField] private int maxIteraciones = 10;

    void Update()
    {
        for (int iteracion = 0; iteracion < maxIteraciones; iteracion++)
        {
            if (Vector3.Distance(targetPos.position, punta.position) <= offset)
                break;

            Iteracion();
        }
    }

    private void Iteracion()
    {
        for (int i = articulaciones.Length - 1; i >= 0; i--)
        {
            Transform articulacion = articulaciones[i];

            Vector3 direccionPunta = punta.position - articulacion.position;
            Vector3 direccionObjetivo = targetPos.position - articulacion.position;

            Quaternion rotacion = Quaternion.FromToRotation(direccionPunta, direccionObjetivo);

            articulacion.rotation = rotacion * articulacion.rotation;
        }
    }
}
