---
layout: documentation
title: Debugging with Symbols
category: Getting-started
order: 3
---

Starting MvvmCross 5.x we have switched to use GitLink to help you easily step into MvvmCross code, 
without needing to do much configuration on your part.

In short GitLink patches PDB files, to look at the MvvmCross GitHub repository with a specific commit hash.
Stepping into MvvmCross code, it then fetches the exact code for that commit!

### Enabling debugging with Symbols
As per the [GitLink README](https://github.com/GitTools/GitLink/blob/develop/README.md), the only thing you need to do 
to enable this, is to go to `Tools > Options > Debugging > General` 
and tick `Enable source server support`. That is it, you should now be able to step into MvvmCross code.

If you encounter problems with Symbols, please refer to the 
[Troubleshooting part of the GitLink README](https://github.com/GitTools/GitLink/blob/develop/README.md#troubleshooting).
