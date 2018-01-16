---
layout: documentation
title: Style Guide
category: Contributing
order: 2
---

This document provides an overview of how articles published on docs.mvvmcross.com should be formatted. You can actually use this file, itself, as a template when contributing articles.

## Article Structure

We are using Jekyll for our documentation, which means that each page needs to have a YAML front matter, which would look something like:

```
---
layout: documentation
title: Cool Docs
category: Some Category
---
```

Edit an existing documentation page, to find out what the current frontmatter looks like.

Please reuse categories and make sure the name of the category is spelled the same and has same casing as in the menu. If you are creating a new category, please make a new folder for it and put your new document in there.

Layout is always documentation, unless you are editing a page or blog post.

There are no strict rules about documentation. However a few guidelines to keep in mind when writing documentation:
- Keep titles an sentences, short and concise. For titles a maximum of 3 or 4 words is the optimal length.
- Describe the topic in chronological order.
- Do not stray from the topic being described too much.
- Do your best at using punctuation and proper grammar. 
- Use a spelling checker.

In the end contributions will be reviewed to iron out any issues.

## Documentation Syntax

The documentation uses the same markdown as GitHub uses. Please refer to [this markdown reference][markdown] for information on how to format a post.

### Adding relative links

To reference other pages inside the documentation use the following syntax:

```
[Getting started]({{ site.url }}/documentation/getting-started/getting-started)
```

### Adding images

Please add any images for the documentation in the `assets/docs` folder. Then you can reference you image like:

```
![My helpful screenshot]({{ site.url }}/assets/docs/screenshot.jpg)
```

## Setting up the MvvmCross GitHub Pages site locally with Jekyll

In some cases it might be more comfortable to work locally on updating the documentation pages instead of using the online GitHub editor. This is especially the case when working on bigger changes you'd most likely do on a separate branch and maybe spend multiple days working on. In those cases it might be usefull to be able to generate the site locally, so you can see what your changes look like when rendered in the browser. This means you will have to install `Jekyll`, detailed instructions on how to do so can be found here:

```
[Setting up your GitHub Pages site locally with Jekyll](https://help.github.com/articles/setting-up-your-github-pages-site-locally-with-jekyll/)
```

## Summary

This style guide is intended to help contributors quickly create new articles for mvvmcross.com. It includes the most common syntax elements that are used, as well as overall document organization guidance. If you discover mistakes or gaps in this guide, please submit an issue.

[markdown]: https://daringfireball.net/projects/markdown/basics
