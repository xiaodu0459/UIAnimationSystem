using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

namespace UIDirector
{
    [CTimelineItemAttribute("Alpha", "Fade To", CTimelineEnum.EItemType.AgentItem)]
    public class CUIWidgetFadeAction : CTimelineAgentAction
    {
        public Ease easeType = Ease.Linear;

        public float FadeTo = 1.0f;
        public bool isRelative = false;

        public class CAgentData
        {
            public Tweener m_Tweener = null;
            public float m_defaultCanvasAlpha = 1.0f;

            public List<Tweener> m_listTweeners = new List<Tweener>();            
            public Dictionary<Graphic, float> m_dicSrcAlpha = new Dictionary<Graphic, float>();
        }
        private System.Collections.Generic.Dictionary<int, CAgentData> m_dicAgentData = new System.Collections.Generic.Dictionary<int, CAgentData>();
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

            CanvasGroup canvasGroup = agent.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                _getAgentData(agent.GetHashCode()).m_defaultCanvasAlpha = canvasGroup.alpha;
            }
            else
            {
                Graphic[] graphics = agent.GetComponentsInChildren<Graphic>(true);
                foreach (Graphic g in graphics)
                {
                    if (_getAgentData(agent.GetHashCode()).m_dicSrcAlpha.ContainsKey(g)) continue;

                    CAgentItemTrack.TraceInfo(string.Format("Initialize: {0}: alpha [{1}] ", g.name, g.color.a));
                    _getAgentData(agent.GetHashCode()).m_dicSrcAlpha.Add(g, g.color.a);
                }
            }
        }

        public override void Trigger(GameObject agent)
        {
            CanvasGroup canvasGroup = agent.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                _getAgentData(agent.GetHashCode()).m_Tweener = canvasGroup.DOFade(FadeTo, duration).SetEase(easeType);
            }
            else
            {
                _getAgentData(agent.GetHashCode()).m_listTweeners.Clear();

                Graphic[] graphics = agent.GetComponentsInChildren<Graphic>(true);
                foreach (Graphic g in graphics)
                {
                    if (isRelative)
                    {
                        float srcAlpha = 1.0f;
                        if (_getAgentData(agent.GetHashCode()).m_dicSrcAlpha.ContainsKey(g)) srcAlpha = _getAgentData(agent.GetHashCode()).m_dicSrcAlpha[g];

                        CAgentItemTrack.TraceInfo(string.Format("{0}: {1} x {2} = {3}, {4}s", g.name, FadeTo, srcAlpha, FadeTo * srcAlpha, duration));
                        Tweener tweener = g.DOFade(FadeTo * srcAlpha, duration).SetEase(easeType);
                        _getAgentData(agent.GetHashCode()).m_listTweeners.Add(tweener);
                    }
                    else
                    {
                        CAgentItemTrack.TraceInfo(string.Format("{0}: {1}, {2}s", g.name, FadeTo, duration));
                        Tweener tweener = g.DOFade(FadeTo, duration).SetEase(easeType);
                        _getAgentData(agent.GetHashCode()).m_listTweeners.Add(tweener);
                    }
                }
            }            
        }

        public override void End(GameObject agent)
        {
            if (_getAgentData(agent.GetHashCode()).m_Tweener != null)
            {
                _getAgentData(agent.GetHashCode()).m_Tweener.Complete();
                _getAgentData(agent.GetHashCode()).m_Tweener = null;
            }
            else if (_getAgentData(agent.GetHashCode()).m_listTweeners.Count > 0)
            {
                foreach (Tweener tweener in _getAgentData(agent.GetHashCode()).m_listTweeners)
                {
                    if (tweener != null)
                        tweener.Complete();
                }
                _getAgentData(agent.GetHashCode()).m_listTweeners.Clear();
            }
            else
            {
                CanvasGroup canvasGroup = agent.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = FadeTo;
                }
                else
                {
                    Graphic[] graphics = agent.GetComponentsInChildren<Graphic>(true);
                    foreach (Graphic g in graphics)
                    {
                        float srcAlpha = 1.0f;
                        if (_getAgentData(agent.GetHashCode()).m_dicSrcAlpha.ContainsKey(g)) srcAlpha = _getAgentData(agent.GetHashCode()).m_dicSrcAlpha[g];

                        if (isRelative)
                        {
                            g.color = new Color(g.color.r, g.color.g, g.color.b, FadeTo * srcAlpha);
                        }
                        else
                        {
                            g.color = new Color(g.color.r, g.color.g, g.color.b, FadeTo);
                        }
                    }
                }
            }
        }        

        public override void SetTime(GameObject agent, float time, float deltaTime)
        {
            if (time == 0)
            {
                if (!m_bInitialized)
                {
                    Debug.LogError("<SetTime> Initialize before calling SetTime");
                    return;
                }

                CanvasGroup canvasGroup = agent.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    _getAgentData(agent.GetHashCode()).m_defaultCanvasAlpha = canvasGroup.alpha;
                }
                else
                {
                    Graphic[] graphics = agent.GetComponentsInChildren<Graphic>(true);
                    foreach (Graphic g in graphics)
                    {
                        float srcAlpha = 1.0f;
                        if (_getAgentData(agent.GetHashCode()).m_dicSrcAlpha.ContainsKey(g)) srcAlpha = _getAgentData(agent.GetHashCode()).m_dicSrcAlpha[g];
                        g.color = new Color(g.color.r, g.color.g, g.color.b, srcAlpha);
                    }
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
                else
                {
                    foreach (Tweener tweener in _getAgentData(agent.GetHashCode()).m_listTweeners)
                    {
                        if (tweener != null)
                            tweener.Goto(time);
                    }
                }
            }
        }

        public override void Pause(GameObject agent)
        {
            if (_getAgentData(agent.GetHashCode()).m_Tweener != null)
            {
                _getAgentData(agent.GetHashCode()).m_Tweener.Pause();
            }
            else
            {
                foreach (Tweener tweener in _getAgentData(agent.GetHashCode()).m_listTweeners)
                {
                    if (tweener != null) tweener.Pause();
                }
            }
        }

        public override void Resume(GameObject agent)
        {
            if (_getAgentData(agent.GetHashCode()).m_Tweener != null)
            {
                _getAgentData(agent.GetHashCode()).m_Tweener.TogglePause();
            }
            else
            {
                foreach (Tweener tweener in _getAgentData(agent.GetHashCode()).m_listTweeners)
                {
                    if (tweener != null && !tweener.IsPlaying()) tweener.TogglePause();
                }
            }
        }

        public override void Stop(GameObject agent) 
        {
            if (revertWhenFinish && m_bInitialized)
            {
                CanvasGroup canvasGroup = agent.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = _getAgentData(agent.GetHashCode()).m_defaultCanvasAlpha;
                }
                else
                {
                    Graphic[] graphics = agent.GetComponentsInChildren<Graphic>(true);
                    foreach (Graphic g in graphics)
                    {
                        float srcAlpha = 1.0f;
                        if (_getAgentData(agent.GetHashCode()).m_dicSrcAlpha.ContainsKey(g)) srcAlpha = _getAgentData(agent.GetHashCode()).m_dicSrcAlpha[g];
                        g.color = new Color(g.color.r, g.color.g, g.color.b, srcAlpha);
                    }
                }
            }

            base.Stop(agent);
        }
    }

}//namespace UIDirector