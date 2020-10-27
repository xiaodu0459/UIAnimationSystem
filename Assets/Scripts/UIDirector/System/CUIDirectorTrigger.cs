// ========================================================================
// UGUI Animation
// Created: 2016/10/24

// Brief: UI director trigger, attach to the UI
// ========================================================================
using UnityEngine;
using System.Collections;

namespace UIDirector
{

    [RequireComponent(typeof(CUIDirector))]
    public class CUIDirectorTrigger : MonoBehaviour
    {
        public enum EStartMethod
        {
            OnStart,
            OnEnable,
            OnTrigger,   
            OnDisable,
            None
        }

        public EStartMethod StartMethod;
        public CUIDirector UIDirector;
        public GameObject TriggerObject;
        public string SkipButtonName = "Jump";
        public float[] StepSkipList;
        public float Delay = 0f;

        private bool m_hasTriggered = false;
        private bool m_bStart = false;    
        private bool m_bUIModuleStart = false;
        private int m_iStepSkipIndex = 0;

        private bool m_bAllowSkip = true;
        private bool m_bTouchOff = true;

        void Awake()
        {
            if (UIDirector == null) UIDirector = this.gameObject.GetComponent<CUIDirector>();
            if (UIDirector != null) UIDirector.Optimize();
        }

        // Use this for initialization
        void Start()
        {
            if (UIDirector != null) UIDirector.DirectorFinished += _OnDirectorFinished;

            if ((StartMethod == EStartMethod.OnStart) && UIDirector != null)
            {                
                m_hasTriggered = true;
                StartCoroutine(_CoPlayDirector());
            }

            m_bStart = true;
        }

        void OnEnable()
        {
            if (m_bUIModuleStart)
            {
                if (StartMethod == EStartMethod.OnEnable && UIDirector != null)
                {
                    if (TriggerObject != null && !TriggerObject.activeInHierarchy)
                        return;

                    this.UIDirector.Stop();

                    m_hasTriggered = true;                    
                    this.UIDirector.Play();
                    //StartCoroutine(_CoPlayDirector());              
                }
            }
        }

        void OnDisable()
        {
            if (Application.isPlaying && StartMethod == EStartMethod.OnEnable && UIDirector != null && m_hasTriggered)
            {
                if (TriggerObject != null && !TriggerObject.activeInHierarchy)
                    return;

                m_hasTriggered = false;                
                this.UIDirector.Pause();
                //this.UIDirector.Skip();
            }
        }

        void OnDestory()
        {
            if (UIDirector != null) UIDirector.DirectorFinished -= _OnDirectorFinished;
        }
               
        // Update is called once per frame
        void Update()
        {
            if (!m_bAllowSkip)
                return;

            if (!string.IsNullOrEmpty(SkipButtonName))
            {                
                if (Input.touchCount > 0)
                {
                    if (m_bTouchOff)
                    {
                        if (this.UIDirector != null && this.UIDirector.State == CUIDirector.DirectorState.Playing)
                        {
                            if (StepSkipList != null && StepSkipList.Length > 0)
                            {
                                if (m_iStepSkipIndex < StepSkipList.Length)
                                {
                                    if (StepSkipList[m_iStepSkipIndex] > UIDirector.RunningTime)
                                        this.UIDirector.SetRunningTime(StepSkipList[m_iStepSkipIndex]);

                                    m_iStepSkipIndex++;
                                }
                            }
                            else
                            {
                                this.UIDirector.Skip();
                            }
                        }                     
                    }

                    m_bTouchOff = false;
                }
                else
                {
                    m_bTouchOff = true;

                    // Check if the user wants to skip.                
                    if (Input.GetButtonDown(SkipButtonName))
                    {
                        if (this.UIDirector != null && this.UIDirector.State == CUIDirector.DirectorState.Playing)
                        {
                            if (StepSkipList != null && StepSkipList.Length > 0)
                            {
                                if (m_iStepSkipIndex < StepSkipList.Length)
                                {
                                    if (StepSkipList[m_iStepSkipIndex] > UIDirector.RunningTime)
                                    {
                                        this.UIDirector.SetRunningTime(StepSkipList[m_iStepSkipIndex]);
                                        //Debug.LogError(string.Format("Skip Step <{0}> <{1}>", m_iStepSkipIndex, StepSkipList[m_iStepSkipIndex]));
                                    }
                                    m_iStepSkipIndex++;
                                }
                            }
                            else
                            {
                                this.UIDirector.Skip();
                            }
                        }
                    }
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (StartMethod == EStartMethod.OnTrigger && !m_hasTriggered && other.gameObject == TriggerObject)
            {
                this.UIDirector.Stop();

                m_hasTriggered = true;
                this.UIDirector.Play();
            }
        }

        private IEnumerator _CoPlayDirector()
        {
            //yield return new WaitForSeconds(Delay);
            this.UIDirector.Play();
            yield break;
        }

        void Msg_OnTriggerEnter(GameObject target)
        {
            if (StartMethod == EStartMethod.OnTrigger && !m_hasTriggered && target == TriggerObject)
            {
                this.UIDirector.Stop();

                m_hasTriggered = true;
                this.UIDirector.Play(); 
            }
        }

        void Msg_OnUIModuleReady()
        {
            m_bUIModuleStart = true;
            OnEnable();
        }

        void Msg_OnUIModuleClose()
        {
            if (StartMethod == EStartMethod.OnDisable)
            {
                this.UIDirector.Stop();

                m_hasTriggered = true;
                this.UIDirector.Play();
            }
        }

        void Msg_OnSkippable(object isSkippable)
        {
            m_bAllowSkip = System.Convert.ToBoolean(isSkippable);
        }

        void Msg_OnSkip()
        {
            this.UIDirector.Skip();
        }

        void _OnDirectorFinished(object sender, DirectorEventArgs args)
        {
            m_hasTriggered = false;
            m_iStepSkipIndex = 0;

            SendMessageUpwards("OnDirectorFinish", this.gameObject.name, SendMessageOptions.DontRequireReceiver);
        }
    }

}//namespace UIDirector