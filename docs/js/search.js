 (function() {

     var titleBoost = 1;
     var authorBoost = 1;
     var categoryBoost = 1;
     var content = 1;

     var contentLength = 300;
     var searchDelay = 600;

     function displaySearchResults(results, store) {
         var searchResults = document.getElementById('search-results');

         if (results.length) {
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

     $("#search-box").keydown(function() {
         if($("#search-box").val().length > 0)
            $(".searching-text").show();
         else
            $(".searching-text").hide();
     });

     
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
                 displaySearchResults(results, window.store); 
             }
         }
     }
 })();