using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorTrigger : MonoBehaviour
{
    [SerializeField] private WheelOfFortune wheelOfFortune;

    private void OnTriggerStay2D(Collider2D other)
    {
        

        
            if (other.gameObject.CompareTag("Respin"))
            {
                Debug.Log("Столкновение с объектом Respin");
            }
            else if (other.gameObject.CompareTag("x2"))
            {
                Debug.Log("Столкновение с объектом x2");
            }
            else if (other.gameObject.CompareTag("x4"))
            {
                Debug.Log("Столкновение с объектом x4");
            }
            else if (other.gameObject.CompareTag("money1000"))
            {
                Debug.Log("Столкновение с объектом money1000");
            }
            else if (other.gameObject.CompareTag("money5000"))
            {
                Debug.Log("Столкновение с объектом money5000");
            }
            else
            {
                Debug.Log("проигрыш");
            }
        
    }
}
