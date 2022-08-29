﻿namespace UIComponents.Samples.Counter
{
    public class CounterService : ICounterService
    {
        private int _count;
    
        public void IncrementCount() => _count++;
        public int GetCount() => _count;
    }
}