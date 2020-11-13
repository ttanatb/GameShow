using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FaceSolver : MonoBehaviour
{
    [SerializeField]
    [OnValueChanged("SetBaseBonePos")]
    Transform m_faceBone = null;

    [SerializeField]
    Vector2 m_faceBounds = Vector2.zero;

    [SerializeField]
    Transform m_faceTarget = null;

    [ShowNonSerializedField]
    Vector3 m_baseLocalPos = Vector3.zero;

    void SetBaseBonePos()
    {
        m_baseLocalPos = m_faceBone.localPosition;
    }

    [Button]
    void ResetTarget()
    {
        m_faceBone.localPosition = m_baseLocalPos;
        m_faceTarget.transform.position = m_faceBone.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_faceBone)
            SetBaseBonePos();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_faceTarget.gameObject.activeSelf)
        {
            m_faceBone.transform.position = m_faceTarget.transform.position;
        }
        else
        {
            m_faceBone.localPosition = m_baseLocalPos;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        //Gizmos.DrawCube(m_faceTarget.position, new Vector3(m_faceBounds.x, m_faceBounds.y, 1.0f));
    }
}
