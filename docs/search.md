---
layout: default
---
{% assign docs_by_category = site.documentation | group_by: "category" | reverse %}

<form action="{{ "/search" | relative_url }}">
  <label for="search-box">Search</label>
  <input type="text" id="search-box" name="query" autocomplete="off">
</form>

<ul id="search-results">
  <h2 class="searching-text">Searching.....</h2>
</ul>

<script>
  window.store = {
    {% for category in docs_by_category %}
        {% for item in category.items %}
          "{{ item.url | slugify }}" :{
            "title": "{{ item.title | xml_escape }}",
            "content": {{ item.content | strip_html | strip_newlines | jsonify }},
            "url": "{{ item.url | xml_escape }}"
          }
          {% unless forloop.last %},{% endunless %}
        {% endfor %}
        ,
    {% endfor %}
    {% for post in site.posts %}
      "{{ post.url | slugify }}": {
        "title": "{{ post.title | xml_escape }}",
        "author": "{{ post.author | xml_escape }}",
        "category": "{{ post.category | xml_escape }}",
        "content": {{ post.content | strip_html | strip_newlines | jsonify }},
        "url": "{{ post.url | xml_escape }}"
      }
      {% unless forloop.last %},{% endunless %}
    {% endfor %}
  };
</script>
<script src="{{ "/js/jquery-3.2.1.min.js" | relative_url }}"></script>
<script src="{{ "/js/lunr.min.js" | relative_url }}"></script>
<script src="{{ "/js/search.js" | relative_url }}"></script>


    