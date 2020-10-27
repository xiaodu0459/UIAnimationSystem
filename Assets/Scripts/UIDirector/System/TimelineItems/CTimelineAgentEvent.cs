// ========================================================================
// UGUI Animation
// Created: 2016/11/07

// Brief: Abstract class: When firetime reached, trigger an event
// on an arbitrary agent
// ========================================================================

using UnityEngine;
using System.Collections;

namespace UIDirector
{

    public abstract class CTimelineAgentEvent : CTimelineItem
    {
        #region Fields
        protected bool m_bInitialized = false;
        #endregion //Fields

        public abstract void Trigger(GameObject agent);

        public virtual void Reverse(GameObject agent) { }

        public virtual void Initialize(GameObject agent) { m_bInitialized = true; }

        public virtual void Stop(GameObject agent) { }

        public CAgentTrackGroup AgentTrackGroup
        {
            get { return this.TimelineTrack.TrackGroup as CAgentTrackGroup; }
        }
    }

}//namespace UIDirector
