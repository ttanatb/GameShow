using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CharTween;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine.Events;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextController : MonoBehaviour
    {
        const float kFadeDuration = 0.05f;
        const float kLocalMoveYFromPos = 3.0f;
        const float kPunctuationPauseSec = 0.2f;

        private AudioManager m_audioManager = null;

        TextMeshProUGUI m_text = null;
        CharTweener m_charTweener = null;
        Sequence m_displaySequence = null;
        UnityAction m_onLineDisplayedCb = null;

        string m_audioClipName = "";
        Vector2 m_pitchRange = Vector2.one;
        AudioClipInfo m_testAudioClipInfo = new AudioClipInfo();

        List<Tweener> m_modifiers = null;

        // TODO: add varying display anims (like more shouty?)
        public void DisplayText(string text, float intervalSec = 0.0f, UnityAction onLineDisplayedCb = null)
        {
            m_text.text = text;
            m_onLineDisplayedCb = onLineDisplayedCb;
            if (text == "" || intervalSec == 0.0f)
                return;

            ClearModifiers();
            m_displaySequence = DOTween.Sequence();
            for (int charIndex = 0; charIndex < text.Length; charIndex++)
            {
                char c = text[charIndex];
                if (!Constants.Space.Contains(c))
                    m_audioManager.PlayOneShot(m_testAudioClipInfo);

                float totalDelay = intervalSec;
                // Add extra pause for punctuation
                if (Constants.Punctuation.Contains(c))
                    totalDelay += kPunctuationPauseSec;

                m_displaySequence
                    .Join(m_charTweener.DOFade(charIndex, 0.0f, kFadeDuration).From().SetDelay(totalDelay));
                //.Join(m_charTweener.DOLocalMoveY(charIndex, kLocalMoveYFromPos, kFadeDuration).From());
            }

            m_displaySequence.AppendCallback(OnDisplayLineComplete).Play();
        }

        public void Clear()
        {
            m_text.text = "";
            ClearModifiers();
        }

        public void SkipDisplayAnim()
        {
            if (!m_displaySequence.active)
                return;

            m_displaySequence.Complete();
            OnDisplayLineComplete();
        }

        void OnDisplayLineComplete()
        {
            m_displaySequence.Kill(false);
            m_onLineDisplayedCb?.Invoke();
        }

        [Button]
        private void TestShake()
        {
            ShakeTextAt(0, 4);
        }

        public void ClearModifiers()
        {
            foreach (var tweener in m_modifiers)
            {
                tweener.fullPosition = 0;
                tweener.Kill();
            }
            m_modifiers.Clear();
        }

        public void SetTextModifier(TextModifier modifier)
        {
            switch (modifier.ModType)
            {
                case TextModifier.Type.kShake:
                    ShakeTextAt(modifier.StartingIndex, modifier.Count);
                    break;
            }
        }

        public void ShakeTextAt(int startingIndex, int count)
        {
            for (int i = startingIndex; i < startingIndex + count; i++)
            {
                m_modifiers.Add(
                    m_charTweener.DOShakePosition(i, 1, 1, 50, 90, false, false)
                    .SetLoops(-1, LoopType.Restart));
            }
        }

        // TODO: fix wave
        public void WaveTextAt(int startingIndex, int count)
        {
            for (int i = startingIndex; i < startingIndex + count; i++)
            {
                m_modifiers.Add(
                    m_charTweener.DOCircle(i, 6.0f, 1.0f)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Restart));
            }
        }

        // Start is called before the first frame update
        void Awake()
        {
            TryGetComponent(out m_text);
            m_charTweener = m_text.GetCharTweener();
            m_modifiers = new List<Tweener>();
        }

        void Start()
        {
            m_audioManager = AudioManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
