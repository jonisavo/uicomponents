﻿<h1 align="center">UIComponents</h1>

<p align="center">
    <img src="https://raw.githubusercontent.com/jonisavo/uicomponents/main/logo.png" alt="Logo" width="200px" height="200px" />
    <br />
    <i>A front-end framework for Unity's UIToolkit, powered by source generation.</i>
</p>

<p align="center">
	<a href="https://openupm.com/packages/io.savolainen.uicomponents/">
		<img src="https://img.shields.io/npm/v/io.savolainen.uicomponents?label=openupm&amp;registry_uri=https://package.openupm.com" alt="OpenUPM" />
	</a>
    <a href="https://github.com/jonisavo/uicomponents/actions/workflows/ci.yml">
        <img src="https://github.com/jonisavo/uicomponents/actions/workflows/ci.yml/badge.svg" alt="CI Status" />
    </a>
    <a href="https://codecov.io/gh/jonisavo/uicomponents">
      <img src="https://codecov.io/gh/jonisavo/uicomponents/branch/main/graph/badge.svg?token=A7DF04CF06" alt="Coverage Status" />
    </a>
</p>

<p align="center">
	<b>Note: UIComponents' API has not yet been fully stabilized. Expect breaking changes.</b>
</p>

## About

The goal of UIComponents is to ease the creation of reusable components when
working with Unity's new UIToolkit system. It offers ways to load UXML and USS
files automatically, and decouple your UI code from other systems via
dependency injection.

## Requirements

UIComponents requires Unity 2020.3 or newer. Unity's `com.unity.roslyn` package is used to enable
source generation in Unity 2020.

## Example usage

```c#
using UnityEngine.UIElements;
using UIComponents;

[UxmlName("Counter")] // A UxmlFactory implementation is generated.
[Layout("CounterComponent/CounterComponent")]
[Stylesheet("CounterComponent/CounterComponent.style")]
[Stylesheet("Common")]
[Dependency(typeof(ICounterService), provide: typeof(CounterService))]
public partial class CounterComponent : UIComponent, IOnAttachToPanel
{
    // The layout and stylesheets are loaded in the inherited
    // constructor. They are retrieved from Resources by default,
    // hence the lack of file extensions.
    
    // An UxmlTraits implementation is generated automatically for this class.
    [UxmlTrait(DefaultValue = "Increment")]
    public string IncrementText;
    
    // Queries are made after all assets have loaded.
    // The query calls are generated automatically for you.
    [Query("count-label")]
    public Label CountLabel;
    
    [Query("increment-button")]
    public Button IncrementButton;
    
    // An instance of CounterService is injected into this field
    // in the inherited constructor.
    [Provide]
    private ICounterService _counterService;
    
    // The OnInit method is called after all assets have loaded.
    // Any operations related to the DOM and stylesheets should
    // be done here.
    public override void OnInit()
    {
        CountLabel.text = _counterService.Count.ToString();
        IncrementButton.text = IncrementText;
    }
    
    // Event handlers are registered after all assets have loaded.
    // To listen for events, all you need to do is implement
    // a supported interface.
    public void OnAttachToPanel(AttachToPanelEvent evt)
    {
        CountLabel.text = _counterService.Count.ToString();
    }
}
```

Instantiation in code:

```c#
var container = new VisualElement();
container.Add(new CounterComponent());
```

Instantiation in UXML:

```xml
<ui:CounterComponent increment-text="+1" />
```

UIComponents are just VisualElements with some additional code in their
constructor for loading assets automatically, among other things.

## Testing

The UIComponents package has been designed with testability in mind. The `UIComponents.Testing`
assembly contains the `TestBed` helper class.

```c#
using UIComponents;
using UIComponents.Testing;
using NUnit.Framework;

[TestFixture]
public class MyComponentTests
{
    private TestBed _testBed;
    private ICounterService _counterService;
    
    [SetUp]
    public void SetUp()
    {
        // A mocking framework like NSubstitute is recommended.
        // Here we don't use a mock at all.
        _counterService = new CounterService();
        _testBed = TestBed.Create()
            .WithSingleton<ICounterService>(_counterService)
            .Build();
    }
    
    [UnityTest]
    public IEnumerator It_Initializes_Count_Label_On_Init()
    {
        _counterService.Count = 42;

        var component = _testBed.CreateComponent<MyComponent>();
        // Wait until the component has loaded.
        yield return component.WaitForInitializationEnumerator();
        Assert.That(component.CountLabel.text, Is.EqualTo("42"));
    }
}
```

## Installation

### With [OpenUPM](https://openupm.com/packages/io.savolainen.uicomponents/) (recommended)

```shell
openupm add io.savolainen.uicomponents
```

Alternatively, merge this snippet to your `Packages/manifest.json` file:

```json
{
    "scopedRegistries": [
        {
            "name": "package.openupm.com",
            "url": "https://package.openupm.com",
            "scopes": [
                "io.savolainen.uicomponents"
            ]
        }
    ],
    "dependencies": {
        "io.savolainen.uicomponents": "0.26.0"
    }
}
```

### With Git

Add this under `dependencies` in your `Packages/manifest.json` file:

```
"io.savolainen.uicomponents": "https://github.com/jonisavo/uicomponents.git#upm/v0.26.0"
```

This will install version 0.26.0.

To update, change `upm/v0.26.0` to point to the latest version.

### With .unitypackage

Download the latest `.unitypackage` from the [releases](https://github.com/jonisavo/uicomponents/releases) page.

To update, remove the existing files and extract the new `.unitypackage`.

NOTE: [com.unity.roslyn](https://docs.unity3d.com/Packages/com.unity.roslyn@0.2/manual/index.html), which is
required in Unity 2020, is not included in the `.unitypackage`. You can get the source from [this mirror](https://github.com/needle-mirror/com.unity.roslyn).

## Documentation

Refer to the [wiki](https://github.com/jonisavo/uicomponents/wiki) for documentation.

- [Layouts and stylesheets](https://github.com/jonisavo/uicomponents/wiki/2.-Layouts-and-stylesheets): see how UIComponents 
loads layouts and stylesheets automatically, and how you can use `[Query]` to query for elements.
- [Asset loading](https://github.com/jonisavo/uicomponents/wiki/3.-Asset-loading): UIComponents loads assets from Resources
by default. See how you can use different methods.
- [Dependency injection](https://github.com/jonisavo/uicomponents/wiki/4.-Dependency-injection): UIComponents comes
with a simple dependency injection system. See how you can use it to decouple your UI code from other logic.
- [Event interfaces](https://github.com/jonisavo/uicomponents/wiki/5.-Event-interfaces): a list of interfaces
whose methods will be automatically registered as event callbacks.
- [Logging](https://github.com/jonisavo/uicomponents/wiki/6.-Logging): for when you want to use something other
than `Debug.Log`.
- [Experimental features](https://github.com/jonisavo/uicomponents/wiki/7.-Experimental-features): new features that
are subject to change.
