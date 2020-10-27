using UnityEngine;
using System.Collections.Generic;

namespace UIDirector
{
    [CTimelineTrackGroupAttribute("MultiAgent Track Group", CTimelineEnum.ETrackType.MultiAgentTrack)]
    public class CMultiAgentTrackGroup : CTimelineTrackGroup
    {
        [SerializeField]
        public List<Transform> Agents = new List<Transform>();        
    }

}//namespace UIDirector