// ========================================================================
// UGUI Animation
// Created: 2016/10/19

// Brief: Track group attributes
// ========================================================================
using UnityEngine;
using System;
using System.Collections.Generic;

namespace UIDirector
{

    [AttributeUsage(AttributeTargets.Class)]
    public class CTimelineTrackGroupAttribute : Attribute
    {
        private string m_label;

        private List<CTimelineEnum.ETrackType> m_listTrackType = new List<CTimelineEnum.ETrackType>();

        public CTimelineTrackGroupAttribute(string label, params CTimelineEnum.ETrackType[] listTrackType)
        {
            m_label = label;
            m_listTrackType.AddRange(listTrackType);
        }

        public string Label
        {
            get { return m_label; }
        }

        public CTimelineEnum.ETrackType[] AllowedTrackType
        {
            get { return m_listTrackType.ToArray(); }
        }
    }

}//namespace UIDirector