// ========================================================================
// UGUI Animation
// Created: 2016/10/17

// Brief: Abstract class: Action which contains firetime and duration
// ========================================================================
using UnityEngine;
using System.Collections;

namespace UIDirector
{

    public abstract class CTimelineAction : CTimelineItem
    {
        #region Fields
        [SerializeField]
        protected float duration = 0f;
        #endregion //Fields

        #region Properties
        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public float EndTime
        {
            get { return fireTime + duration; }
        }
        #endregion //Properties

        public override void SetDefaults()
        {
            duration = 5f;
        }
    }

}//namespace UIDirector
