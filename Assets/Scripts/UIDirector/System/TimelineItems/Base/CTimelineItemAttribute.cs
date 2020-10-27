// ========================================================================
// UGUI Animation
// Created: 2016/10/19

// Brief: Timeline Item attributes
// ========================================================================
using UnityEngine;
using System;
using System.Collections.Generic;

namespace UIDirector
{

    [AttributeUsage(AttributeTargets.Class)]
    public class CTimelineItemAttribute : Attribute
    {
        private string m_subCategory;
        private string m_label; // name of items
        private CTimelineEnum.EItemType[] m_itemTypes;

        public CTimelineItemAttribute(string category, string label, params CTimelineEnum.EItemType[] itemTypes)
        {
            m_subCategory = category;
            m_label = label;
            m_itemTypes = itemTypes;
        }

        public string SubCategory
        {
            get { return m_subCategory; }
        }

        public string Label
        {
            get { return m_label; }
        }

        public CTimelineEnum.EItemType[] ItemTypes
        {
            get { return m_itemTypes; }
        }
    }

}//namespace UIDirector