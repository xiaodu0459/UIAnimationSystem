// ========================================================================
// UGUI Animation
// Created: 2016/11/09

// Brief: Set Position
// ========================================================================
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace UIDirector
{
    [CTimelineItemAttribute("Transform", "Set Position", CTimelineEnum.EItemType.AgentItem)]
    public class CUIWidgetPositionEvent : CTimelineAgentEvent {

        public Vector3 Position = Vector3.zero;
        public bool isRelative = false;

        public override void Trigger(GameObject agent)
        {
            RectTransform rect = agent.GetComponent<RectTransform>();
            if (rect != null)
            {
                //rect.DOAnchorPos(Position, 0.0f);   
                if (isRelative)
                {
                    rect.anchoredPosition = rect.anchoredPosition3D + Position;
                }
                else
                {
                    rect.anchoredPosition = Position;
                }                
            }
        }	    

    }

}//namespace UIDirector
