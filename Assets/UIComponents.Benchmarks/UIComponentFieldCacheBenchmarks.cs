using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UIComponents.Cache;
using Unity.PerformanceTesting;
using UnityEngine.UIElements;

namespace UIComponents.Benchmarks
{
    public partial class UIComponentFieldCacheBenchmarks
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

        private partial class ComponentWithFields : UIComponent
        {
            [Query("hello-world-label")]
            public Label HelloWorldLabel;
            
            [Query(Name = "test-foldout")]
            public Foldout TestFoldout;

            [Query]
            public Label FirstLabel;

            [Query(Class = "text")]
            public Label[] LabelsWithTextClass;

            [Query(Name = "hello-world-label", Class = "text")]
            public Label HelloWorldLabelWithTextClass;

            [Query]
            public List<Label> AllLabels;

            [Provide]
            public IList StringProperty;
            [Provide]
            public IList FloatProperty;
        }
        
        [Test, Performance, Version(BenchmarkUtils.Version)]
        public void InitializeComponentWithFields()
        {
            MeasureFieldCache<ComponentWithFields>();
        }
    }
}
