﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIDirector
{
    public static class CTimelineHelper
    {

        /// <summary>
        /// 插入排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="list">数组</param>
        /// <param name="comparison">比较算法</param>
        public static void InsertionSort<T>(IList<T> list, System.Comparison<T> comparison)
        {
            if (list == null)
                throw new System.ArgumentNullException("list");

            if (comparison == null)
                throw new System.ArgumentNullException("comparison");

            int count = list.Count;
            for (int j = 1; j < count; j++)
            {
                T key = list[j];

                int i = j - 1;
                for (; i >= 0 && comparison(list[i], key) > 0; i--)
                {
                    list[i + 1] = list[i];
                }
                list[i + 1] = key;
            }
        }
    }
}//namespace UIDirector
