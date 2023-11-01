﻿using UnityEngine;

namespace Common.Detector
{
    public interface IDetector
    {
        public bool Detect();

        public void DrawGizmos();

        public void Pause(float t);

        public void Resume();
    }
}