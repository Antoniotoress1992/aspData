/*
Copyright Dinamenta, UAB. http://www.dhtmlx.com
To use this component please contact sales@dhtmlx.com to obtain license
*/
Scheduler.plugin(function(b){(function(){function e(a){var b={},d;for(d in a)d.indexOf("_")!==0&&(b[d]=a[d]);return b}function g(){clearTimeout(f);f=setTimeout(function(){b.updateView()},1)}var f;b.backbone=function(){events.bind("reset",function(){b.clearAll();b.parse(events.toJSON(),"json")});events.bind("change",function(a,c){if(c.changes&&c.changes.id){var d=a.previous("id");b.changeEventId(d,a.id)}var e=a.id;b._init_event(b._events[e]=a.toJSON());g()});events.bind("remove",function(a){b._events[a.id]&&
b.deleteEvent(a.id)});events.bind("add",function(a){if(!b._events[a.id]){var c=a.toJSON();b._init_event(c);b.addEvent(c)}});b.attachEvent("onEventCreated",function(a){var c=new events.model(b.getEvent(a));b._events[a]=c.toJSON();return!0});b.attachEvent("onEventAdded",function(a){events.get(a)||events.add(new events.model(e(b.getEvent(a))));return!0});b.attachEvent("onEventChanged",function(a){var c=events.get(a),d=e(b.getEvent(a));c.set(d);return!0});b.attachEvent("onEventDeleted",function(a){events.get(a)&&
events.remove(a);return!0})}})()});
