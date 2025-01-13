using UnityEngine;

namespace MySampleEx
{
    // ��ϵ� ����� Ŭ���� �����ϰ� �ϳ� �÷��� �Ѵ�
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        public class SoundBank
        {
            public string name;
            public AudioClip[] clips;
        }

        // ����ӵ� ����
        public bool randomizePitch = true;
        public float pitchRandomRange = 0.2f;

        public float playDelay = 0f;
        public SoundBank defaultBank = new SoundBank();

        protected AudioSource m_AudioSource;

        public AudioSource MyAudioSource { get { return m_AudioSource; } }

        public AudioClip Clip { get; private set; }

        private void Awake()
        {
            Init();
        }

        void Init()
        { 
            m_AudioSource = GetComponent<AudioSource>();
        }

        public AudioClip PlayRandomClip(int bankId = 0)
        {
            return InternalPlayRandomClip(bankId);
        }

        AudioClip InternalPlayRandomClip(int bankId = 0)
        {
            var bank = defaultBank;

            if (bank.clips == null || bank.clips.Length == 0)
                return null;

            // Ŭ���� �ϳ��� �����ϰ� ����
            var clip = bank.clips[Random.Range(0, bank.clips.Length)];

            if(clip == null)
                return null;

            m_AudioSource.pitch = 
                randomizePitch ? Random.Range(1f - pitchRandomRange, 1f + pitchRandomRange) : 1f;
            m_AudioSource.clip = clip;
            m_AudioSource.PlayDelayed(playDelay);

            return clip;
        }
    }
}