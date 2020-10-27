// ========================================================================
// UGUI Animation
// Created: 2016/11/07

// Brief: Agent Track Group maintains agents and a set of tracks that affect that agent
// ========================================================================

using UnityEngine;
using System.Collections;

namespace UIDirector
{
    [CTimelineTrackGroupAttribute("Agent Track Group", CTimelineEnum.ETrackType.AgentTrack)]
    public class CAgentTrackGroup : CTimelineTrackGroup
    {
        [SerializeField]
        public Transform Agent;
    }

}//namespace UIDirector