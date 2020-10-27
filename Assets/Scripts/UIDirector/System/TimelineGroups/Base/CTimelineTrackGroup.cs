// ========================================================================
// UGUI Animation
// Created: 2016/10/18

// Brief: Abstract class : Organize timeline tracks
// ========================================================================
using UnityEngine;
using System;
using System.Collections.Generic;

namespace UIDirector
{

    public abstract class CTimelineTrackGroup : MonoBehaviour, IOptimizable
    {
        #region Fields
        [SerializeField]
        private int m_ordinal = -1; // ordering of groups

        [SerializeField]
        private bool m_canOptimize = true; // If true, this group will load all items into cache on Optimize().


        protected CTimelineTrack[] trackCache;

        protected List<Type> allowedTrackTypes;

        protected bool hasBeenOptimized = false;

        #endregion //Fields

        public bool CanOptimize 
        {
            get { return m_canOptimize; }
            set { m_canOptimize = value; } 
        }

        #region Interface
        public virtual void Optimize()
        {
            if (m_canOptimize)
            {
                trackCache = GetTracks();
                hasBeenOptimized = true;
            }
            foreach (CTimelineTrack item in GetTracks())
            {
                if (item is IOptimizable)
                {
                    (item as IOptimizable).Optimize();
                }
            }
        }
        #endregion //Interface 

        public static class CTimelineTrackComparer
        {
            public static int Compare(CTimelineTrack x, CTimelineTrack y)
            {
                if (x.Ordinal < y.Ordinal) return -1;
                else if (x.Ordinal > y.Ordinal) return 1;
                return 0;
            }
        }

        #region Virtual functions
        public virtual CTimelineTrack[] GetTracks()
        {
            if (hasBeenOptimized)
            {
                return trackCache;
            }

            List<CTimelineTrack> tracks = new List<CTimelineTrack>();
            foreach (Type t in GetAllowedTrackTypes())
            {
                var components = GetComponentsInChildren(t);
                foreach (var component in components)
                {
                    //if (component.transform.parent.name.Contains("Target Agent"))
                    //{
                    //    Debug.LogError("TimelineTrack: " + component.gameObject.name);
                    //}

                    tracks.Add((CTimelineTrack)component);
                }
            }

            UnityEngine.Profiling.Profiler.BeginSample("GetTracks.Compare");
            CTimelineHelper.InsertionSort(tracks, CTimelineTrackComparer.Compare);
            UnityEngine.Profiling.Profiler.EndSample();
            //tracks.Sort(
            //    delegate(CTimelineTrack track1, CTimelineTrack track2)
            //    {
            //        return track1.Ordinal - track2.Ordinal;
            //    }
            //    );
            return tracks.ToArray();
        }

        #endregion //Virtual functions

        #region Public functions
        public virtual void Initialize()
        {
            foreach (CTimelineTrack track in GetTracks())
            {
                track.Initialize();
            }
        }

        public virtual void UpdateTrackGroup(float time, float deltaTime)
        {
            foreach (CTimelineTrack track in GetTracks())
            {
                track.UpdateTrack(time, deltaTime);
            }
        }

        public virtual void Pause()
        {
            foreach (CTimelineTrack track in GetTracks())
            {
                track.Pause();
            } 
        }

        public virtual void Stop()
        {
            foreach (CTimelineTrack track in GetTracks())
            {
                track.Stop();
            }
        }

        public virtual void Resume()
        {
            foreach (CTimelineTrack track in GetTracks())
            {
                track.Resume();
            }
        }

        /// <summary>
        /// set this TrackGroup to the state of a given new running time.
        /// </summary>
        /// <param name="time"></param>
        public virtual void SetRunningTime(float time)
        {
            foreach (CTimelineTrack track in GetTracks())
            {
                track.SetTime(time);
            }
        }

        /// <summary>
        /// Get a list of important times for this track group within the given range
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public virtual List<float> GetMilestones(float from, float to)
        {
            List<float> times = new List<float>();
            foreach (CTimelineTrack track in GetTracks())
            {
                List<float> trackTimes = track.GetMilestone(from, to);
                foreach (float f in trackTimes)
                {
                    if (!times.Contains(f))
                    {
                        times.Add(f);
                    }
                }
            }
            times.Sort();
            return times;
        }
        
        public List<Type> GetAllowedTrackTypes()
        {
            if (allowedTrackTypes == null)
            {
                allowedTrackTypes = CTLRuntimeHelper.GetAllowedTrackTypes(this);
            }
            return allowedTrackTypes;
        }
        #endregion //Public functions
    }

}//namespace UIDirector