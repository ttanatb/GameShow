using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField]
    UI.DialogueController m_dialogueController = null;
    #endregion

    #region Inspector, Testing Fields
    [SerializeField]
    private string m_testDialogueNodeName = "test_start";

    [SerializeField]
    private float m_testIntervalDelaySec = 2.0f;

    [SerializeField]
    private string m_testName = "test_name";

    [Button]
    void TestDialogue()
    {
        CreateTestModel();
        SetNameToTestName();
        m_dialogueRunner.StartDialogue(m_testDialogueNodeName);
    }

    [Button]
    void TriggerNext()
    {
        m_dialogueController.TriggerNextDialogue();
    }

    [Button]
    void CreateTestModel()
    {
        m_model = new UI.DialogueModel
        {
            IntervalDelaySec = m_testIntervalDelaySec,
        };
    }

    [Button]
    void SetNameToTestName()
    {
        m_name = m_testName;
    }
    #endregion

    #region Private Fields
    private DialogueUI m_dialogueUI = null;
    private DialogueRunner m_dialogueRunner = null;
    private UI.DialogueModel m_model = null;
    private string m_name = "";

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_dialogueRunner = YarnSingleton.Instance.DialogueRunner;
        m_dialogueUI = YarnSingleton.Instance.DialogueUI;

        m_dialogueUI.onDialogueStart.AddListener(OnDialogueStart);
        m_dialogueUI.onDialogueEnd.AddListener(OnDialogueEnd);

        m_dialogueUI.onLineStart.AddListener(OnLineStart);
        m_dialogueUI.onLineUpdate.AddListener(OnLineUpdate);

        m_dialogueController.SetVisible(false);
    }

    private void OnDestroy()
    {
        m_dialogueUI.onDialogueStart.RemoveListener(OnDialogueStart);
        m_dialogueUI.onDialogueEnd.RemoveListener(OnDialogueEnd);

        m_dialogueUI.onLineStart.RemoveListener(OnLineStart);
        m_dialogueUI.onLineUpdate.RemoveListener(OnLineUpdate);
    }

    private void OnDialogueStart()
    {
        m_dialogueController.SetVisible(true);
    }

    private void OnDialogueEnd()
    {
        m_dialogueController.SetVisible(false);
    }

    private void OnLineStart()
    {
        // Nothing?
    }

    private void OnLineUpdate(string rawText)
    {
        List<TextModifier> modifiers = new List<TextModifier>();

        DialogueParser.Parse(rawText, out string line, out string name, modifiers);

        // TODO: parse line into name, line, and model?
        m_dialogueController.SetDialogue(line, name, m_model);

        foreach (var mod in modifiers)
        {
            m_dialogueController.SetDialogueTextModifier(mod);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
