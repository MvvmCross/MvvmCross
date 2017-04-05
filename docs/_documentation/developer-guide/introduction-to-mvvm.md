---
layout: documentation
title: Introduction to MVVM
category: Developer-guide
---
Model-view-viewmodel (MVVM) is a software architectural pattern.

MVVM is a variation of Martin Fowler's Presentation Model design pattern.[1][2] Like Fowler's Presentation Model, MVVM abstracts a view's state and behavior.[1] However, whereas the Presentation Model abstracts a view (i.e., creates a view model) in a manner not dependent on a specific user-interface platform, MVVM was developed by Microsoft architects Ken Cooper and Ted Peters specifically to simplify event-driven programming of user interfaces-by exploiting features of Windows Presentation Foundation (WPF) (Microsoft's .NET graphics system) and Silverlight (WPF's Internet application derivative).[1]

John Gossman, one of Microsoft's WPF and Silverlight architects, announced MVVM on his blog in 2005.

MVVM and Presentation Model both derive from the model-view-controller pattern (MVC). MVVM facilitates a separation of development of the graphical user interface (either as markup language or GUI code) from development of the business logic or back-end logic (the data model). The view model of MVVM is a value converter;[3] meaning the view model is responsible for exposing (converting) the data objects from the model in such a way objects are easily managed and consumed. In this respect, the view model is more model than view, and handles most if not all of the view's display logic.[3] The view model may implement a mediator pattern, organizing access to the back-end logic around the set of use cases supported by the view.

