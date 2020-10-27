// ========================================================================
// UGUI Animation
// Created: 2016/11/15

// Brief: Set Scale
// ========================================================================
using UnityEngine;
using System.Collections;

namespace UIDirector
{

    [CTimelineItemAttribute("Transform", "Set Scale", CTimelineEnum.EItemType.AgentItem)]
    public class CUIWidgetScaleEvent : CTimelineAgentEvent
    {
        public Vector3 Scale = Vector3.one;

        public override void Trigger(GameObject agent)
        {
            RectTransform rect = agent.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.localScale = Scale;
            }
        }	
    }

}//namespace UIDirector