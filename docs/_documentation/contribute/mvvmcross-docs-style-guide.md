---
layout: documentation
title: Style Guide
category: Contribute
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

### Adding images

Please add any images for the documentation in the `assets/docs` folder. Then you can reference you image like:

```
![My helpful screenshot]({{ site.url }}/assets/docs/screenshot.jpg)
```

## Summary

This style guide is intended to help contributors quickly create new articles for mvvmcross.com. It includes the most common syntax elements that are used, as well as overall document organization guidance. If you discover mistakes or gaps in this guide, please submit an issue.

[markdown]: https://daringfireball.net/projects/markdown/basics
