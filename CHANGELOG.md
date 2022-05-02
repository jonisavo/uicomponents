

## [0.2.1](https://github.com/jonisavo/uicomponents/compare/v0.2.0...v0.2.1) (2022-05-02)


### Bug Fixes

* remove visibility modifier from interface members ([a51e751](https://github.com/jonisavo/uicomponents/commit/a51e7519e00846f7b31e6de9ef8d72c5985e4732))

# [0.2.0](https://github.com/jonisavo/uicomponents/compare/v0.1.0...v0.2.0) (2022-05-01)


### Bug Fixes

* **PathAttribute:** do not mutate Path instance variable in GetAssetPathForComponent ([cb01f27](https://github.com/jonisavo/uicomponents/commit/cb01f27d0b253b402adde8e9e36c35c1328aef2d))

### Features

* rework asset handling ([1c50b26](https://github.com/jonisavo/uicomponents/commit/1c50b2651250a8d6149db7e89ea7348b0c535764))
* rework dependency injection, create AssetDatabaseAssetLoader ([935703e](https://github.com/jonisavo/uicomponents/commit/935703e9b839fca7a7dc1b2d54f9c32dc753dcfd))
* allow specifying asset path with AssetPathAttribute ([336b6b9](https://github.com/jonisavo/uicomponents/commit/336b6b9140934e684496ba0b6fd225fd7cc1c910))
* add very basic dependency injection ([dc12f9d](https://github.com/jonisavo/uicomponents/commit/dc12f9d98422ad249ca84f6f9d398b65b3a7bdb2))
* **DependencyInjector:** add constructor for constructing injector using DependencyAttributes ([7b697b7](https://github.com/jonisavo/uicomponents/commit/7b697b7338f98e2339dfe2725aaadb3ee2658896))
* **DependencyInjector:** add TryProvide method ([9583b39](https://github.com/jonisavo/uicomponents/commit/9583b39147d1e6e08fcd602a9d083d56034b47f0))
* **DependencyInjector:** throw ArgumentNullException in SetDependency ([caa2e1b](https://github.com/jonisavo/uicomponents/commit/caa2e1b6c2f6a318aa2d33701f24b7079e75bd6e))
* **DependencyInjector:** throw exception in Provide<T> if no provider exists ([4ccebe5](https://github.com/jonisavo/uicomponents/commit/4ccebe50faac1ab17aec22c3816f1902c4c2e924))
* **examples:** add CounterComponent example ([bbb8f10](https://github.com/jonisavo/uicomponents/commit/bbb8f10401dfe62bf6a5f490c3b2bafa8f34df46))
* **UIComponent:** make AssetResolver public ([f2dc3b7](https://github.com/jonisavo/uicomponents/commit/f2dc3b7ae47dc59a527aa1093637f2fa9598ebcb))
* **UIComponent:** remove warning messages from when no layout or stylesheet files exist ([cf72d2d](https://github.com/jonisavo/uicomponents/commit/cf72d2de33bb2f4f09a5e9abbce06e40a50bed0d))


### BREAKING CHANGES

* The new ResourcesAssetResolver is
now used by default. AssetDatabaseAssetResolver has been
moved to the new UIComponents.Editor assembly.
* InjectDependencyAttribute has been renamed
to DependencyAttribute. The provider argument has been renamed
to provide. The majority of DependencyInjector has been reworked
completely.
* LayoutAttribute and StylesheetAttribute's
RelativeTo property has been removed.

## 0.1.0 [2022-04-25]

- Initial release