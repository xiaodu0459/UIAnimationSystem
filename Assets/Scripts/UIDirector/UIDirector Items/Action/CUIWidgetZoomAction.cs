// ========================================================================
// UGUI Animation
// Created: 2016/11/15

// Brief: Zoom to
// ========================================================================
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace UIDirector
{
    [CTimelineItemAttribute("Transform", "Zoom To", CTimelineEnum.EItemType.AgentItem)]
    public class CUIWidgetZoomAction : CTimelineAgentAction
    {
        public Ease easeType = Ease.Linear;

        public Vector3 ZoomTo = Vector3.zero;

        public class CAgentData
        {
            public Tweener m_Tweener;
            public Vector3 m_defaulScale;
        }
        private System.Collections.Generic.Dictionary<int, CAgentData> m_dicAgentData = new System.Collections.Generic.Dictionary<int, CAgentData>();
        //private Tweener m_Tweener;
        //private Vector3 m_defaulScale;
        CAgentData _getAgentData(int hashCode)
        {
            if (!m_dicAgentData.ContainsKey(hashCode))
            {
                CAgentData newData = new CAgentData();
                m_dicAgentData.Add(hashCode, newData);
            }

            return m_dicAgentData[hashCode];
        }

        public override void Initialize(GameObject agent)
        {
            base.Initialize(agent);

            RectTransform rect = agent.GetComponent<RectTransform>();
            if (rect != null)
            {
                _getAgentData(agent.GetHashCode()).m_defaulScale = rect.localScale;
            }
        }

        public override void Trigger(GameObject agent)
        {
            RectTransform rect = agent.GetComponent<RectTransform>();
            if (rect != null)
            {
                _getAgentData(agent.GetHashCode()).m_Tweener = rect.DOScale(ZoomTo, duration).SetEase(easeType);             
            }
        }

        public override void End(GameObject agent)
        {
            if (_getAgentData(agent.GetHashCode()).m_Tweener != null)
            {
                _getAgentData(agent.GetHashCode()).m_Tweener.Complete();
                _getAgentData(agent.GetHashCode()).m_Tweener = null;
            }
            else
            {
                RectTransform rect = agent.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.localScale = ZoomTo;
                }
            }
        }

        public override void SetTime(GameObject agent, float time, float deltaTime)
        {
            if (time == 0)
            {
                RectTransform rect = agent.GetComponent<RectTransform>();
                if (rect != null)
                {
                    //rect.DOScale(m_defaulScale, 0.0f);
                    rect.localScale = _getAgentData(agent.GetHashCode()).m_defaulScale;
                }
                Trigger(agent);
            }
            else if (Mathf.Approximately(time, this.Duration))
            {
                End(agent);
            }
            else if (time > 0 && time < this.Duration)
            {
                if (_getAgentData(agent.GetHashCode()).m_Tweener != null)
                {
                    _getAgentData(agent.GetHashCode()).m_Tweener.Goto(time);
                }
            }
        }

        public override void Pause(GameObject agent)
        {
            if (_getAgentData(agent.GetHashCode()).m_Tweener != null)
            {
                _getAgentData(agent.GetHashCode()).m_Tweener.Pause();
            }
        }

        public override void Resume(GameObject agent)
        {
            if (_getAgentData(agent.GetHashCode()).m_Tweener != null && !_getAgentData(agent.GetHashCode()).m_Tweener.IsPlaying())
            {
                _getAgentData(agent.GetHashCode()).m_Tweener.TogglePause();
            }
        }

        public override void Stop(GameObject agent)
        {
            if (revertWhenFinish && m_bInitialized)
            {
                RectTransform rect = agent.GetComponent<RectTransform>();
                if (rect != null)
                {
                    //rect.DOScale(m_defaulScale, 0.0f);
                    rect.localScale = _getAgentData(agent.GetHashCode()).m_defaulScale;
                }
            }

            base.Stop(agent);
        }
    }

}//namespace UIDirector