 (function() {

     var titleBoost = 1;
     var authorBoost = 1;
     var categoryBoost = 1;
     var content = 1;

     var contentLength = 300;
     var searchDelay = 200;

     function displaySearchResults(results, store) {
         var searchResults = document.getElementById('search-results');

         if (results.length) { // Are there any results?
             var appendString = '';

             for (var i = 0; i < results.length; i++) { // Iterate over the results
                 var item = store[results[i].ref];
                 appendString += '<li><a href=".' + item.url + '"><h3>' + item.title + '</h3></a>';
                 appendString += '<p>' + item.content.substring(0, contentLength) + '...</p></li>';
             }
             searchResults.innerHTML = appendString;
         } else {
             searchResults.innerHTML = '<li>No results found</li>';
         }
     }

     $("#search-box").keydown(throttle(function() {
         getResults($('#search-box').val());
     }, searchDelay));

     function throttle(f, delay) {
         var timer = null;
         return function() {
             var context = this,
                 args = arguments;
             clearTimeout(timer);
             timer = window.setTimeout(function() {
                     f.apply(context, args);
                 },
                 delay || 1000);
         };
     }

     function getResults(searchValue) {
         var searchTerm = searchValue;
         if (searchTerm) {
             // document.getElementById('search-box').setAttribute("value", searchTerm);
             // Initalize lunr with the fields it will be searching on. I've given title
             // a boost of 10 to indicate matches on this field are more important.
             var idx = lunr(function() {
                 this.field('id');
                 this.field('title', {
                     boost: titleBoost
                 });
                 this.field('author', {
                     boost: authorBoost
                 });
                 this.field('category', {
                     boost: categoryBoost
                 });
                 this.field('content', {
                     boost: content
                 });
             });

             for (var key in window.store) { // Add the data to lunr
                 idx.add({
                     'id': key,
                     'title': window.store[key].title,
                     'author': window.store[key].author,
                     'category': window.store[key].category,
                     'content': window.store[key].content
                 });

                 var results = idx.search(searchTerm); // Get lunr to perform a search
                 displaySearchResults(results, window.store); // We'll write this in the next section
             }
         }
     }
 })();