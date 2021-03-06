﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace UI
{
    public struct Constants
    {
        public static readonly HashSet<char> Punctuation = new HashSet<char> { ',', '.', '?', '!' };
        public static readonly HashSet<char> Space = new HashSet<char> { ' ' };
    }

    public class DialogueModel
    {
        public float IntervalDelaySec;
        public string AudioClipName;
        public Vector2 PitchRange;
    }

    public class DialogueController : MonoBehaviour
    {
        private AudioManager m_audioManager = null;
        private DialogueUI m_yarnDialogueUI = null;


        private bool m_completedText = false;
        private DialogueModel m_dialogueModel = null;

        [SerializeField]
        private TextController m_dialogueText = null;
        [SerializeField]
        private TextController m_nameText = null;
        [SerializeField]
        private UnityEngine.UI.Image[] m_renderers = null;

        public void SetVisible(bool shouldShow)
        {
            foreach (var img in m_renderers)
                img.enabled = shouldShow;
            if (!shouldShow)
            {
                m_dialogueText.Clear();
                m_nameText.Clear();
            }
        }

        public void SetDialogue(string line, string name = "", DialogueModel model = null)
        {
            if (name != "")
                m_nameText.DisplayText(name);

            if (model != null)
                m_dialogueModel = model;

            m_completedText = false;
            m_dialogueText.DisplayText(line, model.IntervalDelaySec, () =>
            {
                m_completedText = true;
            });
        }

        public void SetDialogueTextModifier(TextModifier modifier)
        {
            m_dialogueText.SetTextModifier(modifier);
        }

        public void TriggerNextDialogue()
        {
            if (m_completedText)
            {
                m_yarnDialogueUI.MarkLineComplete();
            }
            else
            {
                m_dialogueText.SkipDisplayAnim();
            }
        }

        // TODO: might have to adjust this for dotween
        //private IEnumerator DoUpdateText(string text, StringBuilder stringBuilder)
        //{
        //    m_completedText = false;
        //    m_skipToEnd = false;

        //    stringBuilder.Clear();
        //    foreach (char c in text)
        //    {
        //        if (m_skipToEnd)
        //        {
        //            stringBuilder.Clear();
        //            stringBuilder.Append(text);
        //            m_dialogueText.text = stringBuilder.ToString();
        //            // Rebuild layout maybe?

        //            break;
        //        }

        //        stringBuilder.Append(c);
        //        m_dialogueText.text = stringBuilder.ToString();

        //        // Skip audio for 'space'
        //        if (!Constants.Space.Contains(c))
        //            m_audioManager.PlayOneShot(m_dialogueModel.AudioClipName, m_dialogueModel.PitchRange);

        //        // Add extra pause for punctuation
        //        float pauseSec = m_dialogueModel.IntervalDelaySec;
        //        if (Constants.Punctuation.Contains(c))
        //            pauseSec += Constants.PunctuationPauseSec;

        //        // TODO: poll for 'skip' call more often
        //        yield return new WaitForSeconds(pauseSec);
        //    }

        //    DisplayBlinker();

        //    // TODO: fire event on line completed ?

        //    m_completedText = true;
        //    yield return null;
        //}

        private void DisplayBlinker()
        {
            // Set anim that text is done
        }

        private void Start()
        {
            m_audioManager = AudioManager.Instance;
            m_yarnDialogueUI = YarnSingleton.Instance.DialogueUI;
        }
    }
}
