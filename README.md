# UIComponents
#### A microframework for creating reusable components for Unity's UIElements.

```c#
[Layout("Assets/Editor/Components/MyComponent/MyComponent.uxml")]
[Stylesheet("Assets/Editor/Components/MyComponent/MyComponent.style.uss")]
[Stylesheet("Assets/Editor/Common.uss")]
class MyComponent : UIComponent
{
    // The layout and stylesheets are loaded in the inherited
    // constructor.
}
```

```c#
var container = new VisualElement();
container.Add(new MyComponent());
```

If you have a common asset path to your files, you can use `RelativeTo`:

```c#
[Layout("MyComponent/MyComponent.uxml", RelativeTo = AssetPaths.Components)]
[Stylesheet("MyComponent/MyComponent.style.uss", RelativeTo = AssetPaths.Components)]
[Stylesheet("Assets/Editor/Common.uss")]
class MyComponent : UIComponent {}
```

```c#
public static class AssetPaths
{
    public const string Components = "Assets/Editor/Components";
}
```

## API

### `UIComponent`

Automatically loads the specified layout and stylesheets in the constructor.

Offers two protected, virtual methods which can be overridden:

#### `VisualTreeAsset GetLayout()`

Returns the VisualTreeAsset used for the component. Called in `UIComponent`'s constructor.

#### `StyleSheet[] GetStyleSheets()`

Returns the StyleSheets used in the component. Called in `UIComponent`'s constructor.

### `LayoutAttribute`

Used to define the asset path for the `UIComponent`'s `VisualTreeAsset`.
The `RelativeTo` property can be used to prepend to the file path.

Only a single `LayoutAttribute` can be used.

```c#
[Layout("Assets/UIComponents/ComponentA.uxml")]
class ComponentA : UIComponent {}

[Layout("ComponentB.uxml", RelativeTo = AssetPaths.Components)]
class ComponentB : UIComponent {}

public static class AssetPaths
{
    public const string Components = "Assets/UIComponents";
}
```

### `StylesheetAttribute`

Used to define the asset path for a `StyleSheet` used by the `UIComponent`.
The `RelativeTo` property can be used to prepend to the file path.

Multiple `StylesheetAttribute`s can be used.

```c#
[Stylesheet("Assets/UIComponents/ComponentA.style.uss")]
[Stylesheet("Assets/Styles/MyStyle.uss")]
class ComponentA : UIComponent {}

[Stylesheet("ComponentB.style.uss", RelativeTo = AssetPaths.Components)]
[Stylesheet("MyStyle.uss", RelativeTo = AssetPaths.Styles)]
class ComponentB : UIComponent {}

public static class AssetPaths
{
    public const string Components = "Assets/UIComponents";
    public const string Styles = "Assets/Styles";
}
```