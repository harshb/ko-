
Production = Backbone.Model.extend({

    initialize: function () {
        this.bind("error", this.notifyCollectionError);

    },

 
    //primary key
    idAttribute: "ID",

    //create or edit
    //if I did not specify url here, bb would follow a restful convention for urls
    //post : "/productions"
    //put & delete : "/productions/1"
    url: function () {
        return this.isNew() ? "/api/productions/create" : "/api/productions/edit/" + this.get("ID");
    },

    //set or save will fire validate
    validate: function (atts) {
        if ("Title" in atts & !atts.Title) {
            return "Title is required";
        }
    },
    notifyCollectionError: function (model, error) {

        //Raise event -- notifier view is listning for this event being raised
        this.collection.trigger("itemError", error);
    }
});


Productions = Backbone.Collection.extend({
    model: Production,
    url: "/api/productions"
});


productions = new Productions();

//for index functionality
ListView = Backbone.View.extend({

    initialize: function () {

        //scope of "this" is currentview for render mmethod
        _.bindAll(this, 'render');
        this.template = $("#listTemplate");

        //reset,add,remove events are built into bb
        //reset fired whenever data is pushed into collection
        this.collection.bind("reset", this.render);


    },
    events: {

        "click .production-link": "showForm",
        "click #new-production": "showCreate"
    },

    showCreate: function () {

        app.navigate("create", true);
        return false;
    },
    showForm: function (evt) {

        //get the id that was clicked
        var id = $(evt.currentTarget).data('production-id');

        //navigate
        app.navigate("edit/" + id, true);
        return false;

    },
    render: function () {

        var data = { items: this.collection.toJSON() };
        var html = this.template.tmpl(data);
        $(this.el).html(html);

        return this;
    },

    //thanks to: derik bailey's backbone.modelbinding.min.js
    close: function () {
        this.remove();
        this.unbind();

    }

});

//notify
NotifierView = Backbone.View.extend({
    initialize: function () {
        this.template = $("#notifierTemplate");
        this.className = "success";
        this.message = "Success";

        //_bindAll method rescopes "this" to the current view for each method specified
        _.bindAll(this, "render", "notifySave", "notifyError");

        //Notifier view subscribes to itemSaved and itemError events --its listening to these events being raised
        this.collection.bind("itemSaved", this.notifySave);
        this.collection.bind("itemError", this.notifyError);

    },
    events: {
        "click": "goAway"

    },

    goAway: function () {

        // $(this.el).fadeOut();
        $(this.el).delay(3000).fadeOut();
    },
    //hb
    /*
    notifySave: function (message) {

        this.message = message;
        this.render();
        this.goAway();
    },
    */
    notifySave: function (model) {
        this.message = model.get("Title") + " saved";
        this.render();
        this.goAway();
    },
    notifyError: function (message) {

        //this.message = model.get("Title") + " -there was a problem!";
        this.message = message;
        this.render();
        this.goAway();
    },

    render: function () {
        var html = this.template.tmpl({ message: this.message, className: this.className });
        $(this.el).html(html);
        return this;
    }
});
//for crud functionality
FormView = Backbone.View.extend({

    initialize: function () {

        //scope of this is this view for mentioned methods
        _.bindAll(this, 'render');
        this.template = $("#productionFormTemplate");


    },
    events: {

        // "change input": "updateModel",
        "submit #productionForm": "save"
    },


    save: function () {

        //combo : set  hidden field val to dd val
        var authorComboVal = formView.model.get("AuthorCombo_text");
        formView.model.set({ Author: authorComboVal })

        var channelComboVal = formView.model.get("ChannelIDCombo");
        formView.model.set({ ChannelID: channelComboVal })

        this.model.save(


        this.model.attributes,
            {
                success: function (model, response) {
                    
                    //raise an event called itemSaved -- Notify view will subscribe to it
                   //check for id here
                    model.collection.trigger("itemSaved", model);
                    //model.collection.trigger("itemSaved", "Saved");
                   // model.trigger("itemSaved", "Saved");
                    
                    //hb force refresh
                    productions.fetch();
                },
                error: function (model, response) {

                    //notify
                   // model.trigger("itemError", model);
                    model.trigger("itemError", "There was a problem saving ");
                }
            }

        );

        return false;
    },



    //combo
    fillCombo: function () {

        $.getJSON("/api/productions/authors", function (data) {
            //alert(data.length);
            var theauthor = formView.model.get("Author");


            var items = "<option selected " + theauthor + ">" + theauthor + "</option>";
            $.each(data, function (i, item) {
                items += "<option value='" + item.Value + "'>" + item.Text + "</option>";
            });
            $("#AuthorCombo").html(items);

            //set selected
            $("#AuthorCombo").val(theauthor);

        });

    },

    fillChannelsCombo: function () {

        $.getJSON("/api/productions/channels", function (data) {

            var thecombo = formView.model.get("ChannelID");

            //loop data to get selected value from key

            var items = "<option selected " + thecombo + ">" + thecombo + "</option>";
            $.each(data, function (i, item) {
                items += "<option value='" + item.Value + "'>" + item.Text + "</option>";
            });
            $("#ChannelIDCombo").html(items);

            //set selected
            $("#ChannelIDCombo").val(thecombo);

        });

    },


    render: function () {

        var html = this.template.tmpl(this.model.toJSON());
        $(this.el).html(html);
        this.$(".datepicker").datepicker();

        //combo
        this.fillCombo();
        this.fillChannelsCombo();

        // execute the model bindings
        //thanks to: derik bailey's backbone.modelbinding.min.js
        Backbone.ModelBinding.bind(this);
        return this;
    },
    //thanks to: derik bailey's backbone.modelbinding.min.js
    close: function () {
        this.remove();
        this.unbind();
        Backbone.ModelBinding.unbind(this);
    }

});


var ProductionAdmin = Backbone.Router.extend({

    initialize: function () {

        //Views are instattiated in router
        //listview is instanciated with empty collection, and el is specified
        listView = new ListView({ collection: productions, el: "#production-list" });

        //instantiate edit view
        formView = new FormView({ el: "#production-form" });
        //notify
        notifierView = new NotifierView({ collection: productions, el: "#notifications" });

    },
    routes: {
        "": "index", //default route
        "edit/:id": "edit",
        "create": "create"
    },

    index: function () {
        listView.render();
    },
    edit: function (id) {

        listView.render();
        //clear out form by clearing el
        $(formView.el).empty();

        //by default bb expects a lowervcase id as the identifier
        //set idAttribute on model
        var model = productions.get(id);
        formView.model = model;
        formView.render();

    },

    create: function (id) {

        var model = new Production();
        listView.render();
        $(formView.el).empty();
        formView.model = model;
        formView.render();

    }

});

jQuery(function () {


    // var listView = new ListView({ collection: productions });

    // $("#production-list").html(listView.render().el);




    //fetch is called, on completion of which reset event is triggered which in
    //turn triggers render of listview. Render will then push the templated html
    //into el and user sees html
    productions.fetch({

        success: function () {

            //create the router
            window.app = new ProductionAdmin();
            Backbone.history.start();

        },
        error: function () {

        }

    });
});



//You can trigger reset manually at console
//productions.reset ([{Title:"One"},{Title: "Two"}])

//you can check route in the console:
//app.navigate("edit/1", true)
//false will change url but not navigate
//this will give a url like /vidpub/productions#edit/1 -- can be bookmarked and 
//user can navigate back