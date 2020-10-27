// ========================================================================
// UGUI Animation
// Created: 2016/11/09

// Brief: Rotate the agent to the angle during a time
// ========================================================================
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace UIDirector
{
    [CTimelineItemAttribute("Transform", "Rotate to", CTimelineEnum.EItemType.AgentItem)]
    public class CUIWidgetRotateAction : CTimelineAgentAction
    {
        public Ease easeType = Ease.Linear;

        public Vector3 RotationTo = Vector3.zero;

        public class CAgentData
        {
            public Tweener m_Tweener;
            public Vector3 m_defaultRotation;
        }
        private System.Collections.Generic.Dictionary<int, CAgentData> m_dicAgentData = new System.Collections.Generic.Dictionary<int, CAgentData>();
        //private Tweener m_Tweener;
        //private Vector3 m_defaultRotation;
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
                _getAgentData(agent.GetHashCode()).m_defaultRotation = rect.rotation.eulerAngles;
            }
        }

        public override void Trigger(GameObject agent)
        {
            RectTransform rect = agent.GetComponent<RectTransform>();
            if (rect != null)
            {
                _getAgentData(agent.GetHashCode()).m_Tweener = rect.DORotate(RotationTo, duration).SetEase(easeType);
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
                    rect.eulerAngles = RotationTo;
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
                    //rect.DORotate(RotationTo, 0.0f);
                    rect.eulerAngles = _getAgentData(agent.GetHashCode()).m_defaultRotation;
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
                    //rect.DORotate(RotationTo, 0.0f);
                    rect.eulerAngles = _getAgentData(agent.GetHashCode()).m_defaultRotation;
                }
            }

            base.Stop(agent);
        }
    }

}//namespace UIDirector