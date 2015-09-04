### JsonLocalisation

The `JsonLocalisation` plugin provides a number of support classes to help load Json language text files for internationalization (i18n).

The `JsonLocalisation` plugin is a single PCL Assembly and isn't really a typical plugin - it doesn't itself register any singletons or services with the MvvmCross IoC container.

For advice on using the JsonLocalisation library, see:

- the Babel sample - https://github.com/slodge/MvvmCross-Tutorials/tree/master/Babel
- the N+1 video N=21 which discusses how i18n is built - http://slodge.blogspot.co.uk/2013/05/n21-internationalisation-i18n-n1-days.html

Notes:

- the standard JsonLocalisation implementation relies on the `ResourceLoader` and `Json` plugins for loading Json files from the application package contents.
- several alternative Localisation implementations have been suggested including:
  - using Microsoft Resx files - possibly while also using Vernacular from Rdio. For a detailed blog post on using Resx files in MvvmCross, see http://opendix.blogspot.co.uk/2013/05/using-resx-files-for-localization-in.html
  - using EmbeddedResources to store the Json files in the Core PCL assembly (see https://github.com/slodge/MvvmCross/issues/55)
  - using a single JSON file or a single CSV (Comma Separated Variables) file to store all languages in one single file (again see https://github.com/slodge/MvvmCross/issues/55)
  
  we *hope* that some of these alternatives will become open source realities in the future