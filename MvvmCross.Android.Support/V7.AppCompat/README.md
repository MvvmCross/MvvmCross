# Support v7 AppCompat

Notice, if you are using this package, please make sure to extend your Setup.cs file from 
`MvxAppCompatSetup` instead of `MvxAndroidSetup`. This will ensure that Target Bindings for AppCompat type of views, 
are properly registered and make bindings work.

```
public class Setup : MvxAppCompatSetup {

}
```
