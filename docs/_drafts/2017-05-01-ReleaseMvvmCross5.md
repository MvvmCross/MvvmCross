---
layout: post
title: MvvmCross 5.0 release!
date:   2017-05-01 11:37:35 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.0!
We're happy to announce the immediate availability of MvvmCross 5.0!
For the last 6 months we have been working on this release and we're really excited about it.

Let's have a look at a couple of things:

## Highlights

### Merge of repos
For a long time we have been looking at merging the different repositories of the project.
Besides a better overview one of the main advantages this brings is the ability to setup a proper CI process.
This is hugely beneficial in the release process itself and makes (patch) releasing MvvmCross a breeze. 

### New website with improved documentation
Over time we received a lot of feedback from developers that use MvvmCross on a day-2-day basis. Besides the usual
hugs and kisses the most common remark we often got was: "sir, you need to improve the documention". And as you all know: writing documention is hard. And not always the most funny thing you can think of. So our first focus was enabling *you* to help out improving the documentation. With 5.0 the documentation has landed in the GIT repo, making it possible to submit documentation (changes) just as you do with code: create a pull request. We're already seeing the benefits of this: the amount of community driven documentation changes has increased tenfold. 

Over the coming months we'll introduce a 'documentation policy' to make sure the documentation keeps on improving over time and keeps it uniformity.

### New iOS presenter

### Improved navigation

### Lifecycle / Event hooks

### Recyclerview features

### Generic and typed bindings

### Removal of WindowsPhone 8.x and Windows 8.x
As is usual with a major release it's time to say goodbye to old friends. Windows(Phone) 8 is depreceated for a long time; removing formal support for this platform is the right thing to do. 
### Removal of deprecated plugins
MvvmCross' powerful plugin framework has brought us many good things. However, over time certain plugins have become obsolete or considered not useful anymore. With 5.0 we've decided to remove the following plugins:

* AutoView 
* CrossUI
* Dialog
* SQLite plugin
* Bookmarks
* SoundEffects
* ThreadUtils
* JASidePanels

### Other improvements

* tvOS support
* Test projects in main repo
* Migrate Test.Core to PCL 
* Sidebar fixes

### Bugfixes

## Changelog

Below is a link to the complete changelog. More than 120 PR's made it in this release from over 30 developers. So a big hug to all these contributors!

https://github.com/MvvmCross/MvvmCross/pulls?q=is%3Apr+is%3Aclosed+milestone%3A5.0.0

## Open Collective

As you all know MvvmCross is an Open Source project, so that means we're not making any money out of it. But sometimes we're facing actual costs which is always difficult to arrange. To improve on this situation we've created the MvvmCross Open Collective - a place where you can donate your money to the project but also have full insight to what we're actually doing with it. We really hope you're going to join this Open Collective!

https://opencollective.com/mvvmcross
