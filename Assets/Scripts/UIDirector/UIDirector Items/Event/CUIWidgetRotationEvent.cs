// ========================================================================
// UGUI Animation
// Created: 2016/11/09

// Brief: Set Rotation
// ========================================================================
using UnityEngine;
using System.Collections;

namespace UIDirector
{
    [CTimelineItemAttribute("Transform", "Set Rotation", CTimelineEnum.EItemType.AgentItem)]
    public class CUIWidgetRotationEvent : CTimelineAgentEvent
    {
        public Vector3 Rotation = Vector3.zero;

        public override void Trigger(GameObject agent)
        {
            RectTransform rect = agent.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.Rotate(Rotation, 0.0f);
            }
        }	
    }

}//namespace UIDirector