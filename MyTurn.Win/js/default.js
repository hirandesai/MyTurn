var RootUrl = "http://192.168.4.134:9084/";
var APIUrl = RootUrl + "api/";


(function ()
{
    var toilets = [];
    "use strict";
    //#region SignalR Namespance
    WinJS.Namespace.define("MyTurn.SignalR",
    {
        hub: undefined,
        _init: function ()
        {
            $.connection.hub.url = RootUrl + "signalr";
            this.hub = $.connection.myTurnHub;
            this.hub.client.triggerToilet = function (toiletId, IsInUse)
            {
                if (toilets && toilets.length > 0)
                {
                    var toilet = toilets.filter(function (e)
                    {
                        return e.Id === toiletId;
                    });
                    if (toilet && toilet.length>0)
                    {
                        toilet[0].InUseClass = IsInUse ? "item red" : "item green";
                        var listView = document.querySelector(".itemslist").winControl;
                        listView.forceLayout();
                    }
                }
            };
            $.connection.hub.start().done(function (s)
            {

            }).fail(function (e)
            {

            });
        }
    });
    MyTurn.SignalR._init();
    //#endregion


    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;
    var nav = WinJS.Navigation;

    app.addEventListener("activated", function (args)
    {
        if (args.detail.kind === activation.ActivationKind.launch)
        {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated)
            {
                // TODO: This application has been newly launched. Initialize
                // your application here.
            } else
            {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }

            if (app.sessionState.history)
            {
                nav.history = app.sessionState.history;
            }
            args.setPromise(WinJS.UI.processAll().then(function ()
            {
                if (nav.location)
                {
                    nav.history.current.initialPlaceholder = true;
                    return nav.navigate(nav.location, nav.state);
                } else
                {
                    return nav.navigate(Application.navigator.home);
                }
            }));
        }
    });

    app.oncheckpoint = function (args)
    {
        // TODO: This application is about to be suspended. Save any state
        // that needs to persist across suspensions here. If you need to 
        // complete an asynchronous operation before your application is 
        // suspended, call args.setPromise().
        app.sessionState.history = nav.history;
    };

    app.start();

    //#region Code for Items Page
    WinJS.Namespace.define("MyTurn.Items",
    {
        appViewState: Windows.UI.ViewManagement.ApplicationViewState,
        ui: WinJS.UI,
        _init: function ()
        {
            this.ui.Pages.define("/pages/items/items.html",
            {
                ready: function (element, options)
                {
                    var listView = document.querySelector(".itemslist").winControl;
                    //#region Xhr Request for Toilets
                    WinJS.xhr(
                    {
                        type: "GET",
                        url: APIUrl + "toilets",
                        responseType: "json"
                    }).done(function completed(response)
                    {
                        if (response.status === 200)
                        {
                            var res = response.response;
                            if (typeof (res) === "string")
                            {
                                res = JSON.parse(res);
                            }
                            toilets = [];
                            res.forEach(function (item, e)
                            {
                                toilets.push({
                                    Id: item.Id
                                    , GroupName: item.Group.Name
                                    , ToiletIdentifier: item.Identifier
                                    , InUseClass: item.IsInUse ? "item red" : "item green"
                                });
                            });
                            var toiletsList = new WinJS.Binding.List(toilets);                            
                            var groupedToilets = toiletsList.createGrouped(function (item)
                            {
                                return item.GroupName;
                            }, function (item)
                            {
                                return {
                                    title: item.GroupName
                                };
                            }, function (left, right)
                            {
                                return left.toUpperCase().charCodeAt(0) - right.toUpperCase().charCodeAt(0);
                            });
                            //WinJS.UI.processAll();
                            listView.itemDataSource = groupedToilets.dataSource;
                            listView.itemTemplate = element.querySelector(".itemtemplate");
                            listView.groupDataSource = groupedToilets.groups.dataSource;
                            listView.groupHeaderTemplate = element.querySelector(".listLayoutTopHeaderTemplate");
                            listView.selectionMode = 'none';
                            listView.tapBehavior = 'none';
                            listView.layout = { type: WinJS.UI.ListLayout, groupHeaderPosition: 'top' };
                            listView.element.focus();
                            MyTurn.Items._initializeLayout(Windows.UI.ViewManagement.ApplicationView.value.snapped);
                        }
                    },
                    function error(request)
                    {
                        if (request.status === 200)
                        {
                            debugger;
                        }
                    },
                    function progress(request)
                    {
                        if (request.status === 200)
                        {

                        }
                    });
                    //#endregion
                },
                updateLayout: function (element, viewState, lastViewState)
                {
                    var listView = element.querySelector(".itemslist").winControl;
                    if (lastViewState !== viewState)
                    {
                        if (lastViewState === MyTurn.Items.appViewState.snapped || viewState === MyTurn.Items.appViewState.snapped)
                        {
                            var handler = function (e)
                            {
                                listView.removeEventListener("contentanimating", handler, false);
                                e.preventDefault();
                            }
                            listView.addEventListener("contentanimating", handler, false);
                            var firstVisible = listView.indexOfFirstVisible;
                            MyTurn.Items._initializeLayout(listView, viewState);
                            if (firstVisible >= 0 && listView.itemDataSource.list.length > 0)
                            {
                                listView.indexOfFirstVisible = firstVisible;
                            }
                        }
                    }
                },

            });
        },
        _initializeLayout: function (viewstate)
        {
            var listView = document.querySelector(".itemslist").winControl;
            if (viewstate === this.appViewState.snapped)
            {
                listView.layout = new this.ui.ListLayout();
            } else
            {
                listView.layout = new this.ui.GridLayout();
            }
        }
    });
    //#endregion
    MyTurn.Items._init();
})();
