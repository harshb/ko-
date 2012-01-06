var vidpubSearch = {
    resetResults: function () {

        $(".search-results").empty();
    },

    getResults: function (query,controller) {

        $.getJSON('/' + controller, { query: query }, function (data) {
            //alert(data.length);
            var results = { controller: controller, items: data };
            $("#searchTemplate").tmpl(results).appendTo("#" + controller + "-search-results");
        });
    }

};


jQuery(function () {
    $('#searchForm').submit(function (e) {

        var val = $("#search").val();
        vidpubSearch.resetResults();
        if (val.length > 0) {
            vidpubSearch.getResults(val, "productions");
            vidpubSearch.getResults(val, "episodes");
            vidpubSearch.getResults(val, "customers");
        }   
        else {
            $('.search-results').first().html('Select something to search for');
        }
        
        e.preventDefault();

    });

});

