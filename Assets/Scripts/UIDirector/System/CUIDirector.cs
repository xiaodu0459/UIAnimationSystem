// ========================================================================
// UGUI Animation
// Created: 2016/10/21

// Brief: UI director, main control of ui layout, animation, effect, etc.
// ========================================================================
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UIDirector
{
    public class CUIDirector : MonoBehaviour, IOptimizable
    {
        #region Delegate

        public event DirectorHander DirectorFinished;
        public event DirectorHander DirectorPaused;

        #endregion //Delegate

        #region Fields
        [SerializeField]
        private float m_duration = 30f; // duration of director in seconds

        [SerializeField]
        private float m_playbackSpeed = 1f; // multiplier for playback speed

        [SerializeField]
        private bool m_isSkippable = false; // flag: is skippable

        [SerializeField]
        private bool m_isLooping = false; // flag: is looping

        [SerializeField]
        private bool m_canOptimized = true; // flag : can be optimized

        [SerializeField]
        private float m_runningTime = 0f; // running time of the director in seconds;

        [SerializeField]
        private DirectorState m_state = DirectorState.Inactive; // the state of ui director

        [SerializeField]
        private float m_timeScale = 1.0f; // time scale

        public bool IsEditorMode = false;

        // has the director been optimized yet
        private bool m_hasBeenOptimized = false;

        // has the director been initialized yet
        private bool m_hasBeenInitialized = false;

        // cache for optimization
        private CTimelineTrackGroup[] m_trackgroupCache;

        private Coroutine m_updateCoroutine;

        // mask
        private UnityEngine.UI.Image m_ImgDirectorMask;
        #endregion //Fields

        #region Enum
        public enum DirectorState
        {
            Inactive,
            Playing,
            PreviewPlaying,
            Paused
        }
        #endregion //Enum

        #region Properties
        public bool CanOptimize
        {
            get { return m_canOptimized; }
            set { m_canOptimized = value; }
        }

        public float Duration
        {
            get { return this.m_duration; }
            set
            {
                this.m_duration = value;
                if (this.m_duration <= 0f)
                {
                    this.m_duration = 0.1f;
                }
            }
        }

        public DirectorState State
        {
            get
            {
                return this.m_state;
            }
        }

        public float RunningTime
        {
            get
            {
                return this.m_runningTime;
            }
            set
            {
                m_runningTime = Mathf.Clamp(value, 0, m_duration);
            }
        }

        // get all timeline trackgroup in this director
        public CTimelineTrackGroup[] TrackGroups
        {
            get
            {
                return base.GetComponentsInChildren<CTimelineTrackGroup>();
            }
        }
        #endregion //Properties

        #region Interface
        /// <summary>
        /// Cache all timeline tracks and items.
        /// </summary>
        public void Optimize()
        {
            if (m_canOptimized)
            {
                m_trackgroupCache = GetTrackGroups();
                m_hasBeenOptimized = true;
            }
            foreach (CTimelineTrackGroup tg in GetTrackGroups())
            {
                tg.Optimize();
            }
        }
        #endregion //Interface

        #region Mono
        void Awake()
        {
            m_ImgDirectorMask = this.gameObject.GetComponent<UnityEngine.UI.Image>();
            if (m_ImgDirectorMask != null)
            {
                m_ImgDirectorMask.enabled = false;
            }
        }

        void OnDisable()
        {
            //if (State == DirectorState.Playing || State == DirectorState.Paused)
            //    Skip();
            // make sure elpasedtime reset to -1
            m_hasBeenInitialized = false;
        }

        void OnDestroy()
        {
            if (!Application.isPlaying)
            {
                Stop();
            }
        }
        #endregion Mono

        #region Playback
        /// <summary>
        /// Plays/Resumes the director from inactive/paused states
        /// </summary>
        public void Play()
        {
            if (m_state == DirectorState.Inactive)
            {
                if (m_ImgDirectorMask != null)
                {
                    m_ImgDirectorMask.enabled = true;
                }

                StartCoroutine(_CoFreshPlay());
                UnityEngine.Debug.Log("Start Play UI Director");
            }
            else if (m_state == DirectorState.Paused)
            {
                m_state = DirectorState.Playing;
                m_updateCoroutine = StartCoroutine(_CoUpdate());
            }
        }

        /// <summary>
        /// Pause the playback of the director
        /// </summary>
        public void Pause()
        {
            if (m_state == DirectorState.Playing)
            {
                if (m_updateCoroutine != null) StopCoroutine(m_updateCoroutine);
            }
            if (m_state == DirectorState.PreviewPlaying || m_state == DirectorState.Playing)
            {
                foreach (CTimelineTrackGroup group in GetTrackGroups())
                {
                    group.Pause();
                }
            }
            m_state = DirectorState.Paused;

            // send event
            if (DirectorPaused != null)
            {
                DirectorPaused(this, new DirectorEventArgs());
            }
        }

        /// <summary>
        /// Skip the director to the end and stop it
        /// </summary>
        public void Skip()
        {
            if (m_isSkippable)
            {
                SetRunningTime(this.Duration);
                m_state = DirectorState.Inactive;
                Stop();
            }
        }

        /// <summary>
        /// Stop the director
        /// </summary>
        public void Stop()
        {
            this.RunningTime = 0f;

            foreach (CTimelineTrackGroup trackGroup in GetTrackGroups())
            {
                trackGroup.Stop();
            }

            DirectorState lastState = m_state;
            if (m_state == DirectorState.Playing)
            {
                if (m_updateCoroutine != null) StopCoroutine(m_updateCoroutine);
                if (m_state == DirectorState.Playing && m_isLooping)
                {
                    m_state = DirectorState.Inactive;
                    Play();
                }
                else
                {
                    m_state = DirectorState.Inactive;
                }
            }
            else
            {
                m_state = DirectorState.Inactive;
            }

            if (lastState != DirectorState.Inactive && m_state == DirectorState.Inactive)
            {
                if (DirectorFinished != null)
                {
                    DirectorFinished(this, new DirectorEventArgs());
                }
            }

            if (m_ImgDirectorMask != null)
            {
                m_ImgDirectorMask.enabled = false;
            }
        }

        /// <summary>
        /// Update the director by the amount of time
        /// </summary>
        /// <param name="deltaTime"></param>
        public void UpdateDirector(float deltaTime)
        {
            if (IsEditorMode)
                Time.timeScale = m_timeScale;

            this.RunningTime += (deltaTime * m_playbackSpeed);

            foreach (CTimelineTrackGroup group in GetTrackGroups())
            {
                group.UpdateTrackGroup(RunningTime, deltaTime * m_playbackSpeed);
            }

            if (this.RunningTime >= this.Duration || this.RunningTime < 0)
            {
                Stop();
            }
        }
        #endregion //Playback

        #region Public functions
        public CTimelineTrackGroup[] GetTrackGroups()
        {
            if (m_hasBeenOptimized)
            {
                return m_trackgroupCache;
            }

            return TrackGroups;
        }

        public void SetRunningTime(float time)
        {
            foreach (float milestone in _getMilestones(time))
            {
                foreach (CTimelineTrackGroup group in this.TrackGroups)
                {
                    group.SetRunningTime(milestone);
                }
            }

            this.RunningTime = time;
        }
        #endregion //Public functions

        #region Inner functions
        private void _initalize()
        {
            // Initialize each track group
            foreach (CTimelineTrackGroup trackGroup in this.TrackGroups)
            {
                trackGroup.Initialize();
            }
            m_hasBeenInitialized = true;
        }

        private List<float> _getMilestones(float time)
        {
            List<float> milestoneTimes = new List<float>();
            milestoneTimes.Add(time);

            foreach (CTimelineTrackGroup group in this.TrackGroups)
            {
                List<float> times = group.GetMilestones(RunningTime, time);
                foreach (float f in times)
                {
                    if (!milestoneTimes.Contains(f))
                        milestoneTimes.Add(f);
                }
            }

            milestoneTimes.Sort();
            if (time < RunningTime)
            {
                milestoneTimes.Reverse();
            }

            return milestoneTimes;
        }

        private IEnumerator _CoFreshPlay()
        {
            //yield return StartCoroutine(_CoPreparePlay());
            StartCoroutine(_CoPreparePlay());

            // !!! Lizzy error
            // wait for one frame
            //yield return null;

            if (m_state == DirectorState.Paused)
                yield break;

            // Begining playing
            m_state = DirectorState.Playing;
            m_updateCoroutine = StartCoroutine(_CoUpdate());

            yield break;
        }

        private IEnumerator _CoUpdate()
        {
            bool firstFrame = true;
            while (m_state == DirectorState.Playing)
            {
                if (firstFrame)
                {
                    UpdateDirector(0);
                    firstFrame = false;
                }
                else
                {
                    UpdateDirector(Time.deltaTime);
                }
                yield return null;
            }
            yield break;
        }

        private IEnumerator _CoPreparePlay()
        {
            if (!m_hasBeenOptimized)
            {
                Optimize();
            }
            if (!m_hasBeenInitialized)
            {
                _initalize();
            }
            yield return null;
        }
        #endregion //Inner functions
    }

    public delegate void DirectorHander(object sender, DirectorEventArgs args);

    public class DirectorEventArgs : EventArgs
    {
        public DirectorEventArgs()
        {

        }
    }
}//namespace UIDirector