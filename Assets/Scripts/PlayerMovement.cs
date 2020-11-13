using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator m_animator = null;

    [SerializeField]
    float m_speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out m_animator);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            m_animator.SetBool("isWalking", true);
            transform.position += Vector3.right * m_speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            m_animator.SetBool("isWalking", true);
            transform.position -= Vector3.right * m_speed * Time.deltaTime;
        }
        else
        {
            m_animator.SetBool("isWalking", false);
        }
    }
}
