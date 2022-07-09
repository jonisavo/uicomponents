using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UIComponents.Cache;
using UIComponents.Experimental;
using Unity.PerformanceTesting;
using UnityEngine.UIElements;

namespace UIComponents.Benchmarks
{
    public class UIComponentFieldCacheBenchmarks
    {
        private class EmptyComponent : UIComponent {}

        private static void MeasureFieldCache<TComponent>() where TComponent : UIComponent
        {
            Measure.Method(() => { new FieldCache(typeof(TComponent)); })
                .SampleGroup(new SampleGroup("Time"))
                .MeasurementCount(100)
                .IterationsPerMeasurement(100)
                .GC()
                .Run();
        }
        
        [Test, Performance, Version(BenchmarkUtils.Version)]
        public void InitializeEmptyComponent()
        {
            MeasureFieldCache<EmptyComponent>();
        }

        private class ComponentWithFields : UIComponent
        {
            [Query("hello-world-label")]
            public readonly Label HelloWorldLabel;
            
            [Query(Name = "test-foldout")]
            public readonly Foldout TestFoldout;

            [Query]
            public readonly Label FirstLabel;

            [Query(Class = "text")]
            public readonly Label[] LabelsWithTextClass;

            [Query(Name = "hello-world-label", Class = "text")]
            public readonly Label HelloWorldLabelWithTextClass;

            [Query]
            public readonly List<Label> AllLabelsImplicit;

            [Query(Name = "hello-world-label")]
            [Query(Name = "foldout-content")]
            public readonly List<Label> AllLabelsExplicit;
            
            [Provide]
            public readonly IList StringProperty;
            [Provide]
            public readonly IList FloatProperty;
        }
        
        [Test, Performance, Version(BenchmarkUtils.Version)]
        public void InitializeComponentWithFields()
        {
            MeasureFieldCache<ComponentWithFields>();
        }
    }
}