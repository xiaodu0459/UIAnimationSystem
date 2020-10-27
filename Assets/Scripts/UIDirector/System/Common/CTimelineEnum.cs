using UnityEngine;
using System.Collections;

namespace UIDirector
{

    public class CTimelineEnum
    {
        public enum EPlaybackMode
        {
            Disabled = 0, // base, nothing will happen at all
            Runtime = 1, // Unity is in runtime mode
            EditMode = 2, // Unity is in edit mode
            RuntimeAndEdit = 3, // Both runtime and edit mode
        }

        public enum ETrackType
        {
            GlobalTrack,
            AgentTrack,
            MultiAgentTrack,
        }

        public enum EItemType
        {
            GlobalItem,
            AgentItem,
        }
    }

}//namespace UIDirector