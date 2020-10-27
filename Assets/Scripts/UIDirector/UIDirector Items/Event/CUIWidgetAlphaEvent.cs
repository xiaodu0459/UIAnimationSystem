using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

namespace UIDirector
{
    [CTimelineItemAttribute("Alpha", "Set Alpha", CTimelineEnum.EItemType.AgentItem)]
    public class CUIWidgetAlphaEvent : CTimelineAgentEvent
    {
        public float Alpha = 1.0f;

        public override void Trigger(GameObject agent)
        {
            CanvasGroup canvasGroup = agent.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Alpha;
            }
            else
            {
                Graphic[] graphics = agent.GetComponentsInChildren<Graphic>(true);
                foreach (Graphic g in graphics)
                {
                    g.color = new Color(g.color.r, g.color.g, g.color.b, Alpha);
                }
            }
        }        
    }

}//namespace UIDirector
