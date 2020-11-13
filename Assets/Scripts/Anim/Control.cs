using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Control : MonoBehaviour
{
    [OnValueChanged("SetTransform")]
    [SerializeField]
    private Transform m_boneTransform = null;

    [ShowNonSerializedField]
    private Vector3 m_basePos = Vector3.zero;

    private Quaternion m_baseRot = Quaternion.identity;
    private Quaternion m_startingRot = Quaternion.identity;

    private Transform m_boneParent = null;

    public enum UpdateFlag
    {
        None = 0,
        Position = 1 << 0,
        Rotation = 1 << 1,
    }

    [EnumFlags]
    public UpdateFlag m_flags;

    public Transform BoneTransform
    {
        get { return m_boneTransform; }
    }

    private void Start()
    {
        if (m_boneTransform)
            SetTransform();
    }

    void SetTransform()
    {
        transform.position = m_boneTransform.position;
        transform.rotation = m_boneTransform.rotation;
        m_startingRot = transform.localRotation;

        m_basePos = m_boneTransform.localPosition;
        m_baseRot = m_boneTransform.localRotation;

        m_boneParent = m_boneTransform.parent;
    }

    [Button]
    public void ResetTransform()
    {
        if (!m_boneTransform)
            return;
        m_boneTransform.localPosition = m_basePos;
        m_boneTransform.localRotation = m_baseRot;

        transform.position = m_boneTransform.position;
        transform.rotation = m_boneTransform.rotation;
        m_startingRot = transform.localRotation;
    }

    public void Move(Vector3 delta)
    {
        transform.position += delta;
    }

    [Button]
    public void UpdateAssociatedBone()
    {
        if (!m_boneTransform)
            return;

        if ((m_flags & UpdateFlag.Position) == UpdateFlag.Position)
        {
            // Nested positioning doesn't work but like fuck it
            Matrix4x4 mat = m_boneParent.worldToLocalMatrix;
            var convertedPos = mat.MultiplyVector(transform.position - (m_boneParent.position));

            m_boneTransform.localPosition = convertedPos;
        }
        else
        {
            transform.position = m_boneTransform.position;
        }

        if ((m_flags & UpdateFlag.Rotation) == UpdateFlag.Rotation)
        {
            m_boneTransform.localRotation = Quaternion.Euler(0f, 0f,
                transform.localRotation.eulerAngles.z - m_startingRot.eulerAngles.z + m_baseRot.eulerAngles.z);
        }
        else
        {
            transform.rotation = m_boneTransform.rotation;
        }
    }
}

