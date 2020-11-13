// Based on implementation by Anima2D.
// https://assetstore.unity.com/packages/essentials/unity-anima2d-79840#description

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBone : MonoBehaviour
{
    [SerializeField]
    private float m_length = 0.05f;

    [SerializeField]
    private float m_stiffnessForce = 0.07f;

    [SerializeField]
    private float m_dragForce = 0.4f;

    [SerializeField]
    private Vector3 m_springForce = new Vector3(0.0f, -0.0001f, 0.0f);

    // Prev transform
    private Quaternion m_baseLocalRotation = Quaternion.identity;


    // Prev/Curr tip pos
    private Vector3 m_currTipPos = Vector3.zero;
    private Vector3 m_prevTipPos = Vector3.zero;

    void Start()
    {
        m_baseLocalRotation = transform.localRotation;

        m_currTipPos = transform.position + transform.right * m_length;
        m_prevTipPos = m_currTipPos;
    }

    void LateUpdate()
    {
        float deltaTimeSqr = Time.deltaTime * Time.deltaTime;

        transform.localRotation = m_baseLocalRotation;

        Vector3 force = Vector3.zero;
        force += transform.right * m_stiffnessForce / deltaTimeSqr;
        force += (m_prevTipPos - m_currTipPos) * m_dragForce / deltaTimeSqr;
        force += m_springForce / deltaTimeSqr;

        m_currTipPos += (m_currTipPos - m_prevTipPos) + (force * deltaTimeSqr);

        // Clamp position
        m_currTipPos = Vector3.ClampMagnitude(m_currTipPos - transform.position, m_length)
            + transform.position;

        m_prevTipPos = m_currTipPos;

        // Update rotation.
        Quaternion rotToNewPos = Quaternion.FromToRotation(
            transform.right,
            m_currTipPos - transform.position);
        transform.rotation = rotToNewPos * transform.rotation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.05f);
        Gizmos.DrawSphere(transform.position + transform.right * m_length, 0.05f);
    }
}
