// ========================================================================
// UGUI Animation
// Created: 2016/10/21

// Brief: Track attribute
// ========================================================================
using UnityEngine;
using System;
using System.Collections.Generic;

namespace UIDirector
{

    [AttributeUsage(AttributeTargets.Class)]
    public class CTimelineTrackAttribute : Attribute
    {
        private string m_label;
        private List<CTimelineEnum.ETrackType> m_trackTypes = new List<CTimelineEnum.ETrackType>();
        private List<CTimelineEnum.EItemType> m_itemTypes = new List<CTimelineEnum.EItemType>();

        public CTimelineTrackAttribute(string label, CTimelineEnum.ETrackType[] trackTypes, params CTimelineEnum.EItemType[] allowedItemTypes)
        {
            this.m_label = label;
            this.m_trackTypes.AddRange(trackTypes);
            this.m_itemTypes.AddRange(allowedItemTypes);
        }

        public CTimelineTrackAttribute(string label, CTimelineEnum.ETrackType trackType, params CTimelineEnum.EItemType[] allowedItemTypes)
        {
            this.m_label = label;
            this.m_trackTypes.Add(trackType);
            this.m_itemTypes.AddRange(allowedItemTypes);
        }

        public string Label
        {
            get { return m_label; }
        }

        public CTimelineEnum.ETrackType[] TrackTypes
        {
            get { return m_trackTypes.ToArray(); }
        }

        public CTimelineEnum.EItemType[] AllowedItemTypes
        {
            get { return m_itemTypes.ToArray(); }
        }
    }

}//namespace UIDirector