

# [1.0.0-beta.8](https://github.com/jonisavo/uicomponents/compare/v1.0.0-beta.7...v1.0.0-beta.8) (2023-09-27)


### Bug Fixes

* **analyzers:** fix NullReferenceExceptions in UIC103 ([#110](https://github.com/jonisavo/uicomponents/issues/110)) ([67f5406](https://github.com/jonisavo/uicomponents/commit/67f540617cfad1012e348063c7f4b6ee1678918e))
* **core:** fix compile error inside AddressableAssetResolver in Unity 2020 ([7956148](https://github.com/jonisavo/uicomponents/commit/79561489ebb299709ef6c3a0f095fb8f46153b31))


### Features

* **analyzers:** add rule for disallowing readonly members with Provide or Query (UIC003) ([c04dce6](https://github.com/jonisavo/uicomponents/commit/c04dce684f1fe42af541d8e7c0a72bee908ecf23))

# [1.0.0-beta.7](https://github.com/jonisavo/uicomponents/compare/v1.0.0-beta.6...v1.0.0-beta.7) (2023-09-25)


### Features

* **core:** make DependencyAttribute's implementation argument optional ([#109](https://github.com/jonisavo/uicomponents/issues/109)) ([f797b4e](https://github.com/jonisavo/uicomponents/commit/f797b4e9d1330c5c39acfc060b9ae2de0951a14b))

# [1.0.0-beta.6](https://github.com/jonisavo/uicomponents/compare/v1.0.0-beta.5...v1.0.0-beta.6) (2023-09-22)


### Bug Fixes

* **core:** make DependencyConsumer's constructor protected ([7820b77](https://github.com/jonisavo/uicomponents/commit/7820b77c437f2394416e217c4a17335c34a61fa2))

# [1.0.0-beta.5](https://github.com/jonisavo/uicomponents/compare/v1.0.0-beta.4...v1.0.0-beta.5) (2023-09-21)


### Features

* add abstract DependencyConsumer base class for DI consumers ([#108](https://github.com/jonisavo/uicomponents/issues/108)) ([424143b](https://github.com/jonisavo/uicomponents/commit/424143be87d25f4b70b90c37e5049da338f3a4a7))
* drop requirement of Logger field from dependency injection codegen ([9489002](https://github.com/jonisavo/uicomponents/commit/94890023a4b1243b2f0ccd88010d09b16e426770))


### Performance Improvements

* **codegen:** do not iterate through Object class's members ([b562ca7](https://github.com/jonisavo/uicomponents/commit/b562ca713d18f9f48ce19f9bdd7a69b239008c93))

# [1.0.0-beta.4](https://github.com/jonisavo/uicomponents/compare/v1.0.0-beta.3...v1.0.0-beta.4) (2023-07-08)


### Bug Fixes

* disable auto reference in predefined assemblies ([1d56d8e](https://github.com/jonisavo/uicomponents/commit/1d56d8ef4a14dcad27ea6c359e48e21a99113e10))


### Features

* **testing:** allow instantiating with constructor arguments in TestBed ([9744ed6](https://github.com/jonisavo/uicomponents/commit/9744ed6ecbb105b61ba785f876e9c378214f1761))
* **testing:** rework TestBed to work with any class ([f7b4dd5](https://github.com/jonisavo/uicomponents/commit/f7b4dd5d1d689568e0268755b04dc40034e1ae56))


### BREAKING CHANGES

* **testing:** TestBed’s CreateComponent method has been renamed to Instantiate and it no longer takes a factory predicate. Its CreateComponentAsync method has been removed as well as it wasn’t useful in the synchronous Unity Test Framework.
* UIComponents' assemblies are no longer referenced automatically in predefined assemblies.

# [1.0.0-beta.3](https://github.com/jonisavo/uicomponents/compare/v1.0.0-beta.2...v1.0.0-beta.3) (2023-05-26)


### Bug Fixes

* **docs:** fix erroneous documentation comment in TestBed ([24ead3e](https://github.com/jonisavo/uicomponents/commit/24ead3ef18a684a202bdf278fcfd59ee64489fe7))


### Features

* add AssetDatabaseAssetResolver ([57153a8](https://github.com/jonisavo/uicomponents/commit/57153a8c90d17344773ee86e45e5f89095dc66c8))
* initialize UIComponents on first attach to panel ([e8a2c60](https://github.com/jonisavo/uicomponents/commit/e8a2c608420075a3c6a786dcef865b717543f034))
* **UIComponent:** return Task from Initialize ([77a7cb7](https://github.com/jonisavo/uicomponents/commit/77a7cb7d3436d42bed7a76609aba2878d488b7b6))


### Performance Improvements

* **UIComponent:** reduce allocations while loading style sheets ([e163110](https://github.com/jonisavo/uicomponents/commit/e163110842d9000dc054ab9432c67e68e268fd7a))
* **UIComponent:** use cached Task when no layouts are defined ([1d2edd4](https://github.com/jonisavo/uicomponents/commit/1d2edd403d1bc6901452e6b6b3577aed2366bdb5))


### BREAKING CHANGES

* UIComponents no longer start their initialization in the inherited constructor. Instead, it is started when they are first attached to a panel. Alternatively, the newly exposed Initialize method can be called to start initialization.

# [1.0.0-beta.2](https://github.com/jonisavo/uicomponents/compare/v1.0.0-beta.1...v1.0.0-beta.2) (2023-03-21)


### Bug Fixes

* include meta files in .unitypackage distribution ([856d398](https://github.com/jonisavo/uicomponents/commit/856d398961ed7883cae9a40d0cef62792cd4a24c))

# [1.0.0-beta.1](https://github.com/jonisavo/uicomponents/compare/v1.0.0-beta.0...v1.0.0-beta.1) (2023-03-12)


### Bug Fixes

* **UxmlTraitAttribute:** default trait name no longer has trailing or leading dashes ([74a73f7](https://github.com/jonisavo/uicomponents/commit/74a73f70f129c53dd2fa4870ae76b10a187f0d87))


### Features

* **UIComponent:** remove WaitForInitialization method ([d21e194](https://github.com/jonisavo/uicomponents/commit/d21e1941c3bb33dd1588d0e633a139bfeca7af68))
* **UxmlTraitAttribute:** remove obsolete DefaultValue property ([adb4715](https://github.com/jonisavo/uicomponents/commit/adb47154e99375fe527fbc7a6acc0afde436136d))


### BREAKING CHANGES

* **UxmlTraitAttribute:** UxmlTraitAttribute's obsolete DefaultValue property has been removed. Use field and property initializers instead.
* **UIComponent:** UIComponent's obsolete WaitForInitialization method has been removed. Use its InitializationTask instead.

# [1.0.0-beta.0](https://github.com/jonisavo/uicomponents/compare/v1.0.0-alpha.6...v1.0.0-beta.0) (2023-03-05)


### Features

* remove com.unity.roslyn from dependencies ([c99dbfd](https://github.com/jonisavo/uicomponents/commit/c99dbfd2b73fd3656cd794ab6efdca995c37d902))


### BREAKING CHANGES

* com.unity.roslyn is no longer a dependency. This means that Unity 2020 is no longer officially supported out-of-the-box.

# [1.0.0-alpha.6](https://github.com/jonisavo/uicomponents/compare/v1.0.0-alpha.5...v1.0.0-alpha.6) (2023-03-01)


### Features

* **analyzers:** add rule UIC101 and code fix ([321d154](https://github.com/jonisavo/uicomponents/commit/321d1549774d72c730a6cb5a72816063f257046f))
* **analyzers:** add rules UIC102 and UIC103 ([f8d57d0](https://github.com/jonisavo/uicomponents/commit/f8d57d0b9e1f25c4156ce6711c920e7ef0711dd1))
* **roslyn:** add Roslyn analyzer ([41533d7](https://github.com/jonisavo/uicomponents/commit/41533d70a3ec098ac6a27c7777babefc919a791c))

# [1.0.0-alpha.5](https://github.com/jonisavo/uicomponents/compare/v1.0.0-alpha.4...v1.0.0-alpha.5) (2022-11-26)


### Bug Fixes

* ignore using declarations with alias during codegen ([dc3331c](https://github.com/jonisavo/uicomponents/commit/dc3331c49c0709fb22c7b62e47a905caf5cfc9e4))


### Features

* move event interfaces to use source generation ([2221717](https://github.com/jonisavo/uicomponents/commit/22217176276a63eb4258b3395a4173bc2d9a0efc))
* **UxmlTraitAttribute:** mark DefaultValue property as obsolete ([5ea20df](https://github.com/jonisavo/uicomponents/commit/5ea20dfa050ecc0c4ddb95727c2dede09e915e84))
* **UxmlTraitAttribute:** use initializers as default value ([70925e2](https://github.com/jonisavo/uicomponents/commit/70925e2e193ae3e3a61846d62cdfbd3f94603d44))

# [1.0.0-alpha.4](https://github.com/jonisavo/uicomponents/compare/v1.0.0-alpha.3...v1.0.0-alpha.4) (2022-11-20)


### Bug Fixes

* remove ConditionalAttribute from various attributes ([6437399](https://github.com/jonisavo/uicomponents/commit/64373997168d466617fbc966b5dcdb33af251718))
* shorten type names in generated code ([03917ef](https://github.com/jonisavo/uicomponents/commit/03917efe1e12385d9137532552836b4469a9c8e4))

# [1.0.0-alpha.3](https://github.com/jonisavo/uicomponents/compare/v1.0.0-alpha.2...v1.0.0-alpha.3) (2022-11-20)


### Bug Fixes

* **ProvideAttribute:** fix NullReferenceException during codegen ([2b03eb7](https://github.com/jonisavo/uicomponents/commit/2b03eb7f44382d86cfea628086e064526cdeeb12))


### Features

* rework dependency injection to use source generation ([fe70a79](https://github.com/jonisavo/uicomponents/commit/fe70a7963589b479e5ae9862ec8a8e93aee78fdd))
* **samples:** add UXML code generation sample ([86ecd43](https://github.com/jonisavo/uicomponents/commit/86ecd43c48f4f8aaf375faede721e60083e7ba91))
* **UxmlNameAttribute:** use MeansImplicitUseAttribute ([ddb58b6](https://github.com/jonisavo/uicomponents/commit/ddb58b6aea148fb02458b27dc48f2a30b5f05a25))


### BREAKING CHANGES

* All UIComponent subclasses must now be declared partial. TestBed has been reworked to focus on testing one component at a time. `new TestBed<TComponent>` is used instantiate one now.

# [1.0.0-alpha.2](https://github.com/jonisavo/uicomponents/compare/v1.0.0-alpha.1...v1.0.0-alpha.2) (2022-10-30)


### Features

* **QueryAttribute:** annotate each assignment in generated code ([2a3a924](https://github.com/jonisavo/uicomponents/commit/2a3a924a87bbe031bb8e814ca1d69e594e2ccbbe))
* **QueryAttribute:** log error if no elements are found ([58582cc](https://github.com/jonisavo/uicomponents/commit/58582cc9a00b4a5cd6e9ea16945275b3cd1a91ae))

# [1.0.0-alpha.1](https://github.com/jonisavo/uicomponents/compare/v0.26.0...v1.0.0-alpha.1) (2022-10-23)


### Bug Fixes

* ignore abstract classes in code generation ([b53523b](https://github.com/jonisavo/uicomponents/commit/b53523b027d648a30bce2cf6c44215f18137eba6))
* resolve namespace conflicts in generated code ([a1eb455](https://github.com/jonisavo/uicomponents/commit/a1eb455893038c656116450e671e8bf0b5057b99))


### Features

* annotate generated code with GeneratedCodeAttribute ([75b0c35](https://github.com/jonisavo/uicomponents/commit/75b0c359ade0b4fa5cc86ee81f481a00ad74e71d))
* enable source generation in Unity 2020 ([f870d70](https://github.com/jonisavo/uicomponents/commit/f870d7018bb66826811e1b5ec0d25c6490c6a026))
* include samples in .unitypackage ([b1f1fb1](https://github.com/jonisavo/uicomponents/commit/b1f1fb1001a907428bfc9d983e83326c77939adb))
* **ProvideAttribute:** use source generation ([eed6208](https://github.com/jonisavo/uicomponents/commit/eed6208c30cf83e1742cb33c727f3dea2aa82510))
* **QueryAttribute:** now uses source generation instead of reflection ([a43e019](https://github.com/jonisavo/uicomponents/commit/a43e019224f6d667b87630d14a963c1d7266768c))
* remove UIComponent's static ClearCache method ([f236780](https://github.com/jonisavo/uicomponents/commit/f2367807c828fcff918bc5283920bd313ab8c9f1))
* remove Unity 2019 support ([2456730](https://github.com/jonisavo/uicomponents/commit/24567309f64399d7f057b35cc7aa935dff47e709))
* rename AssetPathAttribute to AssetPrefixAttribute ([eb0ccf1](https://github.com/jonisavo/uicomponents/commit/eb0ccf121b14018b5247f6d6046194407bddde59))
* rename IUIComponentLogger to ILogger, turn caller argument type to object ([5749d0f](https://github.com/jonisavo/uicomponents/commit/5749d0f981a8d6d9f05e1b76bc4eae606b86eced))
* rename TraitAttribute to UxmlTraitAttribute ([da47d4c](https://github.com/jonisavo/uicomponents/commit/da47d4cf030b956cbcbcd08df541fa4a3250d04e))
* **UIComponent:** apply effects with source generation ([b118405](https://github.com/jonisavo/uicomponents/commit/b1184057ecf8b9b516ff06eb469653fdaad385e3))
* **UIComponent:** remove GetTypeName method ([1f7343a](https://github.com/jonisavo/uicomponents/commit/1f7343a09ef312c30f73f602a5cfb59708ddfca5))
* **UIComponent:** use source generation for layouts and stylesheets ([a8d5e01](https://github.com/jonisavo/uicomponents/commit/a8d5e01bbad22ec447f7eaab81b3f20b8ec965a0))
* **UxmlNameAttribute:** move from UIComponents.Experimental namespace to UIComponents ([a484ec4](https://github.com/jonisavo/uicomponents/commit/a484ec43aa0f8436929967d4246ef1781a76f262))
* **UxmlTraitAttribute:** move from UIComponents.Experimental namespace to UIComponents ([e1ba54a](https://github.com/jonisavo/uicomponents/commit/e1ba54a206d9d50395670f65816fd6459a47c2b6))
* **UxmlTraitAttribute:** use a kebab-case version of member name by default ([7bfb5e0](https://github.com/jonisavo/uicomponents/commit/7bfb5e0a296e3dba8effa464aa2c54fae4651bd5))


### BREAKING CHANGES

* **UIComponent:** UIComponent's GetTypeName method has been removed as it was unnecessary. Use GetType().Name instead.
* The IUIComponentLogger interface has been renamed to ILogger. It's methods' second argument is now of type object rather than UIComponent.
* **UxmlTraitAttribute:** UxmlTraitAttribute has been moved from the UIComponents.Experimental namespace to UIComponents.
* **UxmlTraitAttribute:** UxmlNameAttribute now uses a kebab-case version of the member name by default.
* **UxmlNameAttribute:** UxmlNameAttribute has been moved from the UIComponents.Experimental namespace to UIComponents.
* AssetPathAttribute has been renamed to AssetPrefixAttribute.
* TraitAttribute has been renamed to UxmlTraitAttribute.
* Because of the removal of reflection, the UIComponent attribute cache has been removed. UIComponent's static ClearCache method has been removed, as well.
* **ProvideAttribute:** ProvideAttribute now uses source generation and requires a partial class.
* **UIComponent:** Effects use source generation, which means that RootClassAttribute now requires a partial class to function.
* **QueryAttribute:** QueryAttribute uses source generation now. Classes that use it must be partial.
* **UIComponent:** LayoutAttribute and StylesheetAttribute use source generation now and require classes to be partial. GetAssetPaths function has been removed from UIComponent. Only one AssetPath attribute can be used with a class.
* Unity 2019 support has been removed.

# [0.26.0](https://github.com/jonisavo/uicomponents/compare/v0.25.0...v0.26.0) (2022-10-13)


### Features

* **UIComponent:** wait for child components to be initialized ([fe9bdaa](https://github.com/jonisavo/uicomponents/commit/fe9bdaadd2fe39aeae60a9daeb8613b823dddce1))


### BREAKING CHANGES

* **UIComponent:** UIComponents now wait for their hierarchy to be initialized before calling OnInit.

# [0.25.0](https://github.com/jonisavo/uicomponents/compare/v0.24.0...v0.25.0) (2022-10-08)


### Features

* add experimental UxmlNameAttribute for generating UxmlFactory ([5df8799](https://github.com/jonisavo/uicomponents/commit/5df87996f32bc0db40cdfc525d842228ce47650d))
* **Addressables:** increase minimum Addressables version to 1.18.19 ([0622e2e](https://github.com/jonisavo/uicomponents/commit/0622e2e028dc1d08c5fa8e3038de97329ed92f19))


### BREAKING CHANGES

* **Addressables:** The minimum Addressables version for UIComponents.Addressables has been increased to 1.18.19. Addressables 1.17 does not work with UIComponent's asynchronous loading.

# [0.24.0](https://github.com/jonisavo/uicomponents/compare/v0.23.0...v0.24.0) (2022-10-02)


### Features

* add source generation for UxmlTraits and UxmlFactory (Unity 2021.2+)
* add IOnNavigationMove event interface ([174e2a0](https://github.com/jonisavo/uicomponents/commit/174e2a0a54e1d67396cd9399ae7c49d57af6c7e5))

# [0.23.0](https://github.com/jonisavo/uicomponents/compare/v0.22.0...v0.23.0) (2022-09-18)


### Features

* **IAssetResolver:** change AssetExists return type to Task<bool> ([3dfe624](https://github.com/jonisavo/uicomponents/commit/3dfe624c1966a2da23b0d166fc2b0a38dc10a38a))
* make most classes sealed ([c4ff845](https://github.com/jonisavo/uicomponents/commit/c4ff845cf36f5e08e03b32650d1b88dc36fcd5eb))


### Performance Improvements

* **AddressableAssetResolver:** cache existence check results ([ea1560c](https://github.com/jonisavo/uicomponents/commit/ea1560c08e24d4331808a2296a96415c738a7389))
* **UIComponent:** determine stylesheets asset paths in the same thread as the load operation ([33dcef6](https://github.com/jonisavo/uicomponents/commit/33dcef6f7526a3b5e5d5011cf531054c18d775d0))


### BREAKING CHANGES

* The following classes are now sealed: AssetPathAttribute, Dependency, DependencyAttribute, DependencyInjector, DiContainer, DiContext, LayoutAttribute, MissingProviderException, ProvideAttribute, QueryAttribute, RootClassAttribute, StylesheetAttribute, TestBed, TestBedBuilder, TestBedTimeoutException.
* **IAssetResolver:** IAssetResolver's AssetExists now returns Task<bool>.

# [0.22.0](https://github.com/jonisavo/uicomponents/compare/v0.21.1...v0.22.0) (2022-09-03)


### Features

* **UIComponent:** add InitializationTask property ([5596a42](https://github.com/jonisavo/uicomponents/commit/5596a42e835c1505b283449c95aafd47b705cf79))
* **UIComponent:** load bare components synchronously ([012945f](https://github.com/jonisavo/uicomponents/commit/012945ffc9b5ab99bd8fd21400b9137b100395fc))
* **UIComponent:** make WaitForInitialization() obsolete ([40646b6](https://github.com/jonisavo/uicomponents/commit/40646b69b7c0978b26a56cba83adb37ae1b4a6a0))

## [0.21.1](https://github.com/jonisavo/uicomponents/compare/v0.21.0...v0.21.1) (2022-08-29)


### Features

* **RootClassAttribute:** use BaseTypeRequired attribute ([1853095](https://github.com/jonisavo/uicomponents/commit/18530957f2dc37b51980f1550a05eb576eccc158))

# [0.21.0](https://github.com/jonisavo/uicomponents/compare/v0.20.0...v0.21.0) (2022-08-29)


### Bug Fixes

* **ProvideAttribute:** fix fields being populated after assets are loaded ([7f6519d](https://github.com/jonisavo/uicomponents/commit/7f6519dec749e58cff2be841d12105facdeaf605))


### Features

* **ProvideAttribute:** add CastFrom property ([7ec0b11](https://github.com/jonisavo/uicomponents/commit/7ec0b11426ca849f0ad062e01acb0c082351ce65))
* **ProvideAttribute:** move out of Experimental namespace ([53c7162](https://github.com/jonisavo/uicomponents/commit/53c7162a5cef5f42550d59bbfdf213b6cbbab0d8))


### BREAKING CHANGES

* **ProvideAttribute:** ProvideAttribute has been moved to the root UIComponents namespace.

# [0.20.0](https://github.com/jonisavo/uicomponents/compare/v0.19.0...v0.20.0) (2022-08-28)


### Features

* create new asmdef for UIComponents.Testing ([95571ec](https://github.com/jonisavo/uicomponents/commit/95571ec31700af346ec7afbde8f7777c7646f861))
* **TestBed:** add implicit conversion from TestBedBuilder ([99c5203](https://github.com/jonisavo/uicomponents/commit/99c520300b5475d5fd271de9f1dd11cafe6ca59d))
* **TestBed:** add SetSingletonOverride instance method ([8fe0b9d](https://github.com/jonisavo/uicomponents/commit/8fe0b9d2fcf628cd9e7ef1a072b9a745b9bc3919))
* **UIComponent:** add WaitForInitializationEnumerator method ([76ef531](https://github.com/jonisavo/uicomponents/commit/76ef5317984b9125c46848280961e5033ede1de2))
* **UIComponent:** load assets asynchronously ([b95192c](https://github.com/jonisavo/uicomponents/commit/b95192ca688e446550520861c9ffd828f80d05d4))


### BREAKING CHANGES

* Usage of UIComponents.Testing now requires including an assembly definition reference.
* **UIComponent:** Assets are now loaded asynchronously. Operations related to the DOM or stylesheets must now be done in the new virtual OnInit method.

# [0.19.0](https://github.com/jonisavo/uicomponents/compare/v0.18.0...v0.19.0) (2022-08-20)


### Features

* add TestBed ([b25a6ad](https://github.com/jonisavo/uicomponents/commit/b25a6adea6d2322e351efce64b5302f608adf1dc))
* **addressables:** set minimum Addressables version to 1.17.13 ([018923d](https://github.com/jonisavo/uicomponents/commit/018923dbd779bcc4863b05c60f7919d93ea323d0))


### Reverts

* Revert "test(benchmarks): ensure Addressables are initialized before running benchmarks" ([7f055a0](https://github.com/jonisavo/uicomponents/commit/7f055a07d7af2220506d698bdef67da76342411a))


### BREAKING CHANGES

* The new TestBed system replaces DependencyScope and DependencyInjector's static methods. They have been removed.
* **addressables:** Preview versions of Addressables 1.17 are no longer supported.

# [0.18.0](https://github.com/jonisavo/uicomponents/compare/v0.17.0...v0.18.0) (2022-07-12)


### Features

* add support for transient dependencies ([e20ee2b](https://github.com/jonisavo/uicomponents/commit/e20ee2b04ba0ff836e328e12f1f58a24ba304952))
* **DependencyInjector:** rename RestoreDefaultDependency to ResetProvidedInstance ([4be20c7](https://github.com/jonisavo/uicomponents/commit/4be20c7f1e217589f1b5bb19babc664c66fe6ae2))
* remove built-in AssetDatabase support ([ddc14b8](https://github.com/jonisavo/uicomponents/commit/ddc14b834bd6517bfa60e4a902feb8483a7bd783))


### BREAKING CHANGES

* AssetDatabaseAssetResolver has been removed, since working with hardcoded asset paths is difficult. If you want to use AssetDatabase, you can create your own IAssetResolver class.
* **DependencyInjector:** DependencyInjector's RestoreDefaultDependency method has been renamed to ResetProvidedInstance

# [0.17.0](https://github.com/jonisavo/uicomponents/compare/v0.16.0...v0.17.0) (2022-07-09)


### Features

* **docs:** add documentation link to new wiki ([a999223](https://github.com/jonisavo/uicomponents/commit/a999223f370abc4751e9e283c1e6f1d9fcebcf96))
* **QueryAttribute:** move out of the experimental namespace ([dc40380](https://github.com/jonisavo/uicomponents/commit/dc403802f34f9537c93f2e75630275663880048c))


### Performance Improvements

* **FieldCache:** reduce initialization time and GC allocations ([35a9ab8](https://github.com/jonisavo/uicomponents/commit/35a9ab8f828aef71b46cdb9895a2ca0ced8319a0))


### BREAKING CHANGES

* **QueryAttribute:** QueryAttribute has been moved from the UIComponents.Experimental namespace to UIComponents.

# [0.16.0](https://github.com/jonisavo/uicomponents/compare/v0.15.0...v0.16.0) (2022-06-24)


### Features

* add experimental ProvideAttribute ([0c54ea0](https://github.com/jonisavo/uicomponents/commit/0c54ea0d74d962462adcadfbbafc1357a0e9d67b))
* add RootClassAttribute ([983a425](https://github.com/jonisavo/uicomponents/commit/983a42507a2ff47709bee04ea25a8182ce8b1e9c))
* **DependencyInjector:** add non-generic Provide method ([891b4cd](https://github.com/jonisavo/uicomponents/commit/891b4cd798019dfa2f1bb3dd22c64c2c9e66f3f1))

# [0.15.0](https://github.com/jonisavo/uicomponents/compare/v0.14.0...v0.15.0) (2022-06-18)


### Bug Fixes

* **DependencyInjector:** fix construction when class has overridden dependency ([25412e5](https://github.com/jonisavo/uicomponents/commit/25412e5093c9b32494c010e1e6c72d30d523e6f1))


### Features

* add IUIComponentLogger dependency to UIComponent ([8e89652](https://github.com/jonisavo/uicomponents/commit/8e89652ed4ffa1b029512169b60b9445a1ee3498))
* **UIComponent:** add GetTypeName method ([b8a089c](https://github.com/jonisavo/uicomponents/commit/b8a089c8690da5d23381496a66d4fcbcac3f20ce))

# [0.14.0](https://github.com/jonisavo/uicomponents/compare/v0.13.0...v0.14.0) (2022-06-14)


### Features

* **QueryAttribute:** log error on non-VisualElement fields ([0855628](https://github.com/jonisavo/uicomponents/commit/0855628348002571bdf6439a365a8b689447b374))


### Performance Improvements

* **QueryAttribute:** reduce allocations on Unity 2019 ([9294cf6](https://github.com/jonisavo/uicomponents/commit/9294cf61a2c95b89d4486c776b512b39452b1786))

# [0.13.0](https://github.com/jonisavo/uicomponents/compare/v0.12.0...v0.13.0) (2022-06-12)


### Bug Fixes

* **samples:** remove mention of QueryClassAttribute ([aa95716](https://github.com/jonisavo/uicomponents/commit/aa957166a92834ad531d014ff38d3bc1311baa09))


### Features

* add event interfaces IOnAttachToPanel, IOnDetachFromPanel & IOnGeometryChanged ([ee16a5e](https://github.com/jonisavo/uicomponents/commit/ee16a5ef72eb2aae1d16a10fbee9d49e8f5d54e2))
* add IOnClick event interface ([0172a64](https://github.com/jonisavo/uicomponents/commit/0172a64684a23cfe56cb5fc37d38a3ec68fb4448))
* add IOnMouseEnter and IOnMouseLeave event interfaces ([34b370b](https://github.com/jonisavo/uicomponents/commit/34b370b905be9a4d4936e0e609afb89bff053b8b))
* **samples:** add event interfaces sample ([74780a2](https://github.com/jonisavo/uicomponents/commit/74780a2579e36f36ef28268269de17d2b97fdc79))

# [0.12.0](https://github.com/jonisavo/uicomponents/compare/v0.11.0...v0.12.0) (2022-06-10)


### Features

* **QueryAttribute:** query by type and class, support array and List<> fields ([52375d7](https://github.com/jonisavo/uicomponents/commit/52375d7518a4c23e6c05e39e8b316e3aa3f21e97))
* **QueryAttribute:** use MeansImplicitUseAttribute ([a123143](https://github.com/jonisavo/uicomponents/commit/a1231431b81844dbf3d1c4054569eeb7fa12b6c7))

# [0.11.0](https://github.com/jonisavo/uicomponents/compare/v0.10.0...v0.11.0) (2022-06-05)


### Bug Fixes

* **DependencyInjector:** fix SetDependency exception message ([9090c74](https://github.com/jonisavo/uicomponents/commit/9090c7493e996d655d2c64b06507d6b936e6248b))


### Features

* **DependencyInjector:** add RestoreDefaultDependency method ([ef1952f](https://github.com/jonisavo/uicomponents/commit/ef1952ff93cb2b953ea4e86f7997b7ca4f79e443))

# [0.10.0](https://github.com/jonisavo/uicomponents/compare/v0.9.0...v0.10.0) (2022-06-03)


### Features

* **StylesheetAttribute:** apply stylesheets from parents upward ([a27f75e](https://github.com/jonisavo/uicomponents/commit/a27f75e9db84e11d3107e597b2e4abd65a7d0a52))


### BREAKING CHANGES

* **StylesheetAttribute:** Stylesheets defined in parent classes are now loaded first. This means that child classes can override styles from their parents.

# [0.9.0](https://github.com/jonisavo/uicomponents/compare/v0.8.0...v0.9.0) (2022-05-27)


### Features

* add experimental QueryAttribute ([1b7e0d5](https://github.com/jonisavo/uicomponents/commit/1b7e0d52146ad57b8aa573e4eef0fcf7c57c0641))
* **DependencyInjector:** add RemoveInjector static method for testing ([e514533](https://github.com/jonisavo/uicomponents/commit/e51453394a99d7dc3372a7d3b2fa12ff43fddb3b))


### Performance Improvements

* implement new cache for UIComponents ([6b90eb2](https://github.com/jonisavo/uicomponents/commit/6b90eb224b755aae703da6b4795871986cecb560))

# [0.8.0](https://github.com/jonisavo/uicomponents/compare/v0.7.0...v0.8.0) (2022-05-13)


### Bug Fixes

* **UIComponent:** add null check to layout loading function ([6359299](https://github.com/jonisavo/uicomponents/commit/6359299d4022fd9c898dc55e0452d610f0ce5441))


### Features

* add samples ([f4c0ccf](https://github.com/jonisavo/uicomponents/commit/f4c0ccf541e2d1327599853424f76ddb240c9147))

# [0.7.0](https://github.com/jonisavo/uicomponents/compare/v0.6.0...v0.7.0) (2022-05-11)


### Features

* add DependencyScope utility class ([dd8233a](https://github.com/jonisavo/uicomponents/commit/dd8233adfea60e1b9081abe72bbe7d643edc2033))
* **package.json:** add changelog, documentation and license URLs ([09e5555](https://github.com/jonisavo/uicomponents/commit/09e55554a88b4a199db03f063a776de26c2af7f7))

# [0.6.0](https://github.com/jonisavo/uicomponents/compare/v0.5.0...v0.6.0) (2022-05-05)


### Features

* move to UIComponents namespace from UIComponents.Core ([7efa0db](https://github.com/jonisavo/uicomponents/commit/7efa0db51f6936ad991f09dabe62759bcdd82cb9))


### BREAKING CHANGES

* All core code is now located in the UIComponents namespace, as opposed to UIComponents.Core.

# [0.5.0](https://github.com/jonisavo/uicomponents/compare/v0.4.0...v0.5.0) (2022-05-05)


* feat(UIComponent)!: make GetLayout and GetStyleSheets private ([4251611](https://github.com/jonisavo/uicomponents/commit/42516118aa3ff9d530cb8bad13f0ccf96c8a2bb3))


### Features

* add Addressables support ([#7](https://github.com/jonisavo/uicomponents/issues/7)) ([6abab8c](https://github.com/jonisavo/uicomponents/commit/6abab8cdb46b73f4c944098a7fd9aad94900668b))


### BREAKING CHANGES

* UIComponent's GetLayout and GetStyleSheets functions are no longer protected and virtual. Since they are called in UIComponent's constructor, derived classes would not have been fully initialized when the overridden functions are called.

# [0.4.0](https://github.com/jonisavo/uicomponents/compare/v0.3.0...v0.4.0) (2022-05-03)


### Features

* **DependencyInjector:** add ClearDependency method ([f621469](https://github.com/jonisavo/uicomponents/commit/f6214697441d3c05ab6b21b919f48f42a7847d9f))

# [0.3.0](https://github.com/jonisavo/uicomponents/compare/v0.2.1...v0.3.0) (2022-05-02)


### Features

* **UIComponent:** add TryProvide method ([c367311](https://github.com/jonisavo/uicomponents/commit/c3673114e6246010846ffa57619e171ad85279c9))

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