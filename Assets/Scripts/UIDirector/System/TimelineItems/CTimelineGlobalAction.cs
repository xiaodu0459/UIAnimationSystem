// ========================================================================
// UGUI Animation
// Created: 2016/10/17

// Brief: Called when running time hits the firetime of the action
// ========================================================================
using UnityEngine;
using System.Collections;

namespace UIDirector
{

    public abstract class CTimelineGlobalAction : CTimelineAction
    {
        #region abstract functions
        public abstract void Trigger();

        public abstract void End();        
        #endregion //abstract functions

        #region virtual functions
        public virtual void UpdateTime(float time, float deltaTime) { }

        public virtual void SetTime(float time, float deltaTime) { }

        public virtual void Pause() { }

        public virtual void Resume() { }

        public virtual void ReverseTrigger() { }

        public virtual void ReverseEnd() { }
        #endregion //virtual functions

        #region Public functions
        public int CompareTo(object other)
        {
            CTimelineGlobalAction otherAction = (CTimelineGlobalAction)other;
            return (int)(otherAction.FireTime - this.FireTime);
        }
        #endregion //Public functions
    }

}//namespace UIDirector
