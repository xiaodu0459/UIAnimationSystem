// ========================================================================
// UGUI Animation
// Created: 2016/10/14

// Brief: Basic class of timeline track, controls global events and actions
// ========================================================================
using UnityEngine;
using System;
using System.Collections.Generic;

namespace UIDirector
{

    public abstract class CTimelineTrack : MonoBehaviour, IOptimizable
    {
        #region Fields
        [SerializeField]
        private int m_ordinal = -1; // ordering of tracks

        [SerializeField]
        private bool m_canOptimize = true; // If true, this track will load all items into cache on Optimize().

        public CTimelineEnum.EPlaybackMode PlaybackMode = CTimelineEnum.EPlaybackMode.RuntimeAndEdit;

        protected float elapsedTime = -1f; 
        protected CTimelineItem[] itemCache; // for optimization purpose
        protected List<Type> allowedTypes; // TimelineItem allowed types on this track
        protected bool hasBeenOptimized = false; // flags
        #endregion //Fields

        #region Properties
        public int Ordinal
        {
            get { return m_ordinal; }
            set { m_ordinal = value;}
        }

        public bool CanOptimize
        {
            get { return m_canOptimize; }
            set { m_canOptimize = value; }
        }

        public CTimelineTrackGroup TrackGroup
        {
            get
            {
                CTimelineTrackGroup group = null;
                if (transform.parent != null)
                {
                    group = transform.parent.GetComponentInParent<CTimelineTrackGroup>();
                    if (group == null)
                    {
                        Debug.LogError("No TrackGroup found on parent.", this);
                    }
                }
                else
                {
                    Debug.LogError("Track has no parent.", this);
                }
                return group;
            }
        }
        #endregion //Properties

        #region Interface
        public virtual void Optimize()
        {
            if (CanOptimize)
            {
                itemCache = GetTimelineItems();
                hasBeenOptimized = true;
            }
            foreach (CTimelineItem item in GetTimelineItems())
            {
                if (item is IOptimizable)
                {
                    (item as IOptimizable).Optimize();
                }
            }
        }
        #endregion //Interface 

        #region Public functions
        public virtual void Initialize()
        {
            elapsedTime = -1;
            foreach (CTimelineItem item in GetTimelineItems())
            {
                item.Initialize();
            }
        }

        public virtual void UpdateTrack(float runningTime, float deltaTime)
        {
            float previousTime = elapsedTime;
            elapsedTime = runningTime;

            foreach (CTimelineItem item in GetTimelineItems())
            {
                if (item is CTimelineGlobalEvent)
                {
                    CTimelineGlobalEvent globalEvent = item as CTimelineGlobalEvent;
                    if ((previousTime < globalEvent.FireTime) && (elapsedTime >= globalEvent.FireTime))
                    {
                        globalEvent.Trigger();
                    }
                    else if ((previousTime >= globalEvent.FireTime) && (elapsedTime < globalEvent.FireTime))
                    {
                        globalEvent.Reverse();
                    }
                }
            }

            foreach (CTimelineItem item in GetTimelineItems())
            {
                if (item is CTimelineGlobalAction)
                {
                    CTimelineGlobalAction globalAction = item as CTimelineGlobalAction;
                    if ((previousTime < globalAction.FireTime) && (elapsedTime >= globalAction.FireTime))
                    {
                        globalAction.Trigger();
                    }
                    else if ((previousTime < globalAction.EndTime) && (elapsedTime >= globalAction.EndTime))
                    {
                        globalAction.End();
                    }
                    else if ((previousTime > globalAction.FireTime) && (previousTime <= globalAction.EndTime) && (elapsedTime < globalAction.FireTime))
                    {
                        globalAction.ReverseTrigger();
                    }
                    else if ((previousTime > globalAction.EndTime) && (elapsedTime > globalAction.FireTime) && (elapsedTime <= globalAction.EndTime))
                    {
                        globalAction.ReverseEnd();
                    }
                    else
                    {
                        float t = runningTime - globalAction.FireTime;
                        globalAction.UpdateTime(t, deltaTime);
                    }
                }
            }
        }

        public virtual void Pause()
        {

        }

        public virtual void Resume()
        {

        }

        public virtual void Stop()
        {
            elapsedTime = -1;
            foreach (CTimelineItem item in GetTimelineItems())
            {
                item.Stop();
            }
        }

        public virtual void SetTime(float time)
        {
            float previousTime = elapsedTime;
            elapsedTime = time;

            foreach (CTimelineItem item in GetTimelineItems())
            {
                if (item is CTimelineGlobalEvent)
                {
                    // check if it is a global event.
                    CTimelineGlobalEvent globalEvent = item as CTimelineGlobalEvent;
                    if (globalEvent != null)
                    {
                        if ((previousTime < globalEvent.FireTime) && (elapsedTime >= globalEvent.FireTime))
                        {
                            globalEvent.Trigger();
                        }
                        else if ((previousTime >= globalEvent.FireTime) && (elapsedTime < globalEvent.FireTime))
                        {
                            globalEvent.Reverse();
                        }
                    }
                }//
                
            }

            foreach (CTimelineItem item in GetTimelineItems())
            {
                if (item is CTimelineGlobalAction)
                {
                    // check if it is a global action
                    CTimelineGlobalAction globalAction = item as CTimelineGlobalAction;
                    if (globalAction != null)
                    {
                        globalAction.SetTime((time - globalAction.FireTime), time - previousTime);
                    }
                }//
            }
        }

        public virtual List<float> GetMilestone(float from, float to)
        {
            bool isReverse = from > to;

            List<float> times = new List<float>();
            foreach (CTimelineItem item in GetTimelineItems())
            {
                if ((!isReverse && from < item.FireTime && to >= item.FireTime) || (isReverse && from > item.FireTime && to <= item.FireTime))
                {
                    if (!times.Contains(item.FireTime))
                    {
                        times.Add(item.FireTime);
                    }
                }

                if (item is CTimelineAction)
                {
                    float endTime = (item as CTimelineAction).EndTime;
                    if ((!isReverse && from < endTime && to >= endTime) || (isReverse && from > endTime && to <= endTime))
                    {
                        if (!times.Contains(endTime))
                        {
                            times.Add(endTime);
                        }
                    }
                }
            }
            times.Sort();
            return times;
        }

        public List<Type> GetAllowedItemTypes()
        {
            if (allowedTypes == null)
            {
                allowedTypes = CTLRuntimeHelper.GetAllowedItemTypes(this);
            }
            return allowedTypes;
        }

        public static class CTimelineItemComparer
        {
            public static int Compare(CTimelineItem x, CTimelineItem y)
            {
                if (x.FireTime < y.FireTime) return -1;
                else if (x.FireTime > y.FireTime) return 1;
                return 0;
            }
        }

        /// <summary>
        /// Get all allowed timeline item in this track
        /// </summary>
        /// <returns>timeline item list</returns>
        public CTimelineItem[] GetTimelineItems()
        {
            if (hasBeenOptimized)
            {
                return itemCache;
            }

            List<CTimelineItem> items = new List<CTimelineItem>();
            foreach (Type t in GetAllowedItemTypes())
            {
                var components = GetComponentsInChildren(t);
                foreach (var component in components)
                {                  
                    items.Add((CTimelineItem)component);
                }
            }
            UnityEngine.Profiling.Profiler.BeginSample("GetTimelineItems.Compare");
            CTimelineHelper.InsertionSort(items, CTimelineItemComparer.Compare);
            UnityEngine.Profiling.Profiler.EndSample();

            return items.ToArray();
        }
        #endregion //Public functions
    }

}//namespace UIDirector