/*
Copyright Dinamenta, UAB. http://www.dhtmlx.com
To use this component please contact sales@dhtmlx.com to obtain license
*/
Scheduler.plugin(function(a){a.config.limit_start=null;a.config.limit_end=null;a.config.limit_view=!1;a.config.check_limits=!0;a.config.mark_now=!0;a.config.display_marked_timespans=!0;(a._temp_limit_scope=function(){function B(c,b,d,e,f){function h(b,a,c,d){var e=[];if(b&&b[a])for(var f=b[a],h=i(c,d,f),j=0;j<h.length;j++)e=g._add_timespan_zones(e,h[j].zones);return e}function i(b,a,c){var d=c[a]&&c[a][f]?c[a][f]:c[b]&&c[b][f]?c[b][f]:[];return d}var g=a,j=[],l={_props:"map_to",matrix:"y_property"},
m;for(m in l){var o=l[m];if(g[m])for(var n in g[m]){var p=g[m][n],k=p[o];c[k]&&(j=g._add_timespan_zones(j,h(b[n],c[k],d,e)))}}return j=g._add_timespan_zones(j,h(b,"global",d,e))}var v=null,u="dhx_time_block",w="default",C=function(c,b,a){b instanceof Date&&a instanceof Date?(c.start_date=b,c.end_date=a):(c.days=b,c.zones=a);return c},y=function(a,b,d){var e=typeof a=="object"?a:{days:a};e.type=u;e.css="";if(b){if(d)e.sections=d;e=C(e,a,b)}return e};a.blockTime=function(c,b,d){var e=y(c,b,d);return a.addMarkedTimespan(e)};
a.unblockTime=function(c,b,d){var b=b||"fullday",e=y(c,b,d);return a.deleteMarkedTimespan(e)};a.attachEvent("onBeforeViewChange",function(c,b,d,e){e=e||b;d=d||c;return a.config.limit_view&&(e.valueOf()>a.config.limit_end.valueOf()||this.date.add(e,1,d)<=a.config.limit_start.valueOf())?(setTimeout(function(){a.setCurrentView(a._date,d)},1),!1):!0});a.checkInMarkedTimespan=function(c,b,d){for(var b=b||w,e=!0,f=new Date(c.start_date.valueOf()),h=a.date.add(f,1,"day"),i=a._marked_timespans;f<c.end_date;f=
a.date.date_part(h),h=a.date.add(f,1,"day")){var g=+a.date.date_part(new Date(f)),j=f.getDay(),l=B(c,i,j,g,b);if(l)for(var m=0;m<l.length;m+=2){var o=a._get_zone_minutes(f),n=c.end_date>h||c.end_date.getDate()!=f.getDate()?1440:a._get_zone_minutes(c.end_date),p=l[m],k=l[m+1];if(p<n&&k>o&&(e=d=="function"?d(c,o,n,p,k):!1,!e))break}}return!e};var t=a.checkLimitViolation=function(c){if(!c)return!0;if(!a.config.check_limits)return!0;for(var b=a,d=b.config,e=[],e=c.rec_type?a.getRecDates(c):[c],f=!0,h=
0;h<e.length;h++){var i=!0,g=e[h];g._timed=a.isOneDayEvent(g);(i=d.limit_start&&d.limit_end?g.start_date.valueOf()>=d.limit_start.valueOf()&&g.end_date.valueOf()<=d.limit_end.valueOf():!0)&&(i=!a.checkInMarkedTimespan(g,u,function(a,c,d,e,f){var g=!0;if(c<=f&&c>=e){if(f==1440||d<f)g=!1;a._timed&&b._drag_id&&b._drag_mode=="new-size"?(a.start_date.setHours(0),a.start_date.setMinutes(f)):g=!1}if(d>=e&&d<f||c<e&&d>f)a._timed&&b._drag_id&&b._drag_mode=="new-size"?(a.end_date.setHours(0),a.end_date.setMinutes(e)):
g=!1;return g}));if(!i)b._drag_id=null,b._drag_mode=null,i=b.checkEvent("onLimitViolation")?b.callEvent("onLimitViolation",[g.id,g]):i;f=f&&i}return f};a.attachEvent("onMouseDown",function(a){return!(a=u)});a.attachEvent("onBeforeDrag",function(c){return!c?!0:t(a.getEvent(c))});a.attachEvent("onClick",function(c){return t(a.getEvent(c))});a.attachEvent("onBeforeLightbox",function(c){var b=a.getEvent(c);v=[b.start_date,b.end_date];return t(b)});a.attachEvent("onEventSave",function(c,b){if(!b.start_date||
!b.end_date){var d=a.getEvent(c);b.start_date=new Date(d.start_date);b.end_date=new Date(d.end_date)}if(b.rec_type){var e=a._lame_clone(b);a._roll_back_dates(e);return t(e)}return t(b)});a.attachEvent("onEventAdded",function(c){if(!c)return!0;var b=a.getEvent(c);if(!t(b)&&a.config.limit_start&&a.config.limit_end){if(b.start_date<a.config.limit_start)b.start_date=new Date(a.config.limit_start);if(b.start_date.valueOf()>=a.config.limit_end.valueOf())b.start_date=this.date.add(a.config.limit_end,-1,
"day");if(b.end_date<a.config.limit_start)b.end_date=new Date(a.config.limit_start);if(b.end_date.valueOf()>=a.config.limit_end.valueOf())b.end_date=this.date.add(a.config.limit_end,-1,"day");if(b.start_date.valueOf()>=b.end_date.valueOf())b.end_date=this.date.add(b.start_date,this.config.event_duration||this.config.time_step,"minute");b._timed=this.isOneDayEvent(b)}return!0});a.attachEvent("onEventChanged",function(c){if(!c)return!0;var b=a.getEvent(c);if(!t(b)){if(!v)return!1;b.start_date=v[0];
b.end_date=v[1];b._timed=this.isOneDayEvent(b)}return!0});a.attachEvent("onBeforeEventChanged",function(a){return t(a)});a.attachEvent("onBeforeEventCreated",function(c){var b=a.getActionData(c).date,d={_timed:!0,start_date:b,end_date:a.date.add(b,a.config.time_step,"minute")};return t(d)});a.attachEvent("onViewChange",function(){a._mark_now()});a.attachEvent("onSchedulerResize",function(){window.setTimeout(function(){a._mark_now()},1);return!0});a.attachEvent("onTemplatesReady",function(){a._mark_now_timer=
window.setInterval(function(){a._mark_now()},6E4)});a._mark_now=function(c){var b="dhx_now_time";this._els[b]||(this._els[b]=[]);var d=a._currentDate(),e=this.config;a._remove_mark_now();if(!c&&e.mark_now&&d<this._max_date&&d>this._min_date&&d.getHours()>=e.first_hour&&d.getHours()<e.last_hour){var f=this.locate_holder_day(d);this._els[b]=a._append_mark_now(f,d)}};a._append_mark_now=function(c,b){var d="dhx_now_time",e=a._get_zone_minutes(b),f={zones:[e,e+1],css:d,type:d};if(this._table_view){if(this._mode==
"month")return f.days=+a.date.date_part(b),a._render_marked_timespan(f,null,null)}else if(this._props&&this._props[this._mode]){for(var h=this._els.dhx_cal_data[0].childNodes,i=[],g=0;g<h.length-1;g++){var j=c+g;f.days=j;var l=a._render_marked_timespan(f,null,j)[0];i.push(l)}return i}else return f.days=c,a._render_marked_timespan(f,null,c)};a._remove_mark_now=function(){for(var a="dhx_now_time",b=this._els[a],d=0;d<b.length;d++){var e=b[d],f=e.parentNode;f&&f.removeChild(e)}this._els[a]=[]};a._marked_timespans=
{global:{}};a._get_zone_minutes=function(a){return a.getHours()*60+a.getMinutes()};a._prepare_timespan_options=function(c){var b=[],d=[];if(c.days=="fullweek")c.days=[0,1,2,3,4,5,6];if(c.days instanceof Array){for(var e=c.days.slice(),f=0;f<e.length;f++){var h=a._lame_clone(c);h.days=e[f];b.push.apply(b,a._prepare_timespan_options(h))}return b}if(!c||!(c.start_date&&c.end_date&&c.end_date>c.start_date||c.days!==void 0&&c.zones))return b;var i=0,g=1440;if(c.zones=="fullday")c.zones=[i,g];if(c.zones&&
c.invert_zones)c.zones=a.invertZones(c.zones);c.id=a.uid();c.css=c.css||"";c.type=c.type||w;var j=c.sections;if(j)for(var l in j){if(j.hasOwnProperty(l)){var m=j[l];m instanceof Array||(m=[m]);for(f=0;f<m.length;f++){var o=a._lame_copy({},c);o.sections={};o.sections[l]=m[f];d.push(o)}}}else d.push(c);for(var n=0;n<d.length;n++){var p=d[n],k=p.start_date,q=p.end_date;if(k&&q)for(var r=a.date.date_part(new Date(k)),x=a.date.add(r,1,"day");r<q;){o=a._lame_copy({},p);delete o.start_date;delete o.end_date;
o.days=r.valueOf();var s=k>r?a._get_zone_minutes(k):i,t=q>x||q.getDate()!=r.getDate()?g:a._get_zone_minutes(q);o.zones=[s,t];b.push(o);r=x;x=a.date.add(x,1,"day")}else{if(p.days instanceof Date)p.days=a.date.date_part(p.days).valueOf();p.zones=c.zones.slice();b.push(p)}}return b};a._get_dates_by_index=function(c,b,d){for(var e=[],b=a.date.date_part(new Date(b||a._min_date)),d=new Date(d||a._max_date),f=b.getDay(),h=c-f>=0?c-f:7-b.getDay()+c,i=a.date.add(b,h,"day");i<d;i=a.date.add(i,1,"week"))e.push(i);
return e};a._get_css_classes_by_config=function(a){var b=[];a.type==u&&(b.push(u),a.css&&b.push(u+"_reset"));b.push("dhx_marked_timespan",a.css);return b.join(" ")};a._get_block_by_config=function(a){var b=document.createElement("DIV");if(a.html)typeof a.html=="string"?b.innerHTML=a.html:b.appendChild(a.html);return b};a._render_marked_timespan=function(c,b,d){var e=[],f=a.config,h=this._min_date,i=this._max_date,g=!1;if(!f.display_marked_timespans)return e;if(!d&&d!==0){if(c.days<7)d=c.days;else{var j=
new Date(c.days),g=+j;if(!(+i>=+j&&+h<=+j))return e;d=j.getDay()}var l=h.getDay();l>d?d=7-(l-d):d-=l}var m=c.zones,o=a._get_css_classes_by_config(c);if(a._table_view&&a._mode=="month"){var n=[],p=[];if(b)n.push(b),p.push(d);else for(var p=g?[g]:a._get_dates_by_index(d),k=0;k<p.length;k++)n.push(this._scales[p[k]]);for(k=0;k<n.length;k++)for(var b=n[k],d=p[k],q=0;q<m.length;q+=2){var r=m[k],t=m[k+1];if(t<=r)return[];var s=a._get_block_by_config(c);s.className=o;var u=b.offsetHeight-1,v=b.offsetWidth-
1,w=Math.floor((this._correct_shift(d,1)-h.valueOf())/(864E5*this._cols.length)),y=this.locate_holder_day(d,!1)%this._cols.length,B=this._colsS[y],C=this._colsS.heights[w]+(this._colsS.height?this.xy.month_scale_height+2:2)-1;s.style.top=C+"px";s.style.lineHeight=s.style.height=u+"px";s.style.left=B+Math.round(r/1440*v)+"px";s.style.width=Math.round((t-r)/1440*v)+"px";b.appendChild(s);e.push(s)}}else{var z=d;if(this._props&&this._props[this._mode]&&c.sections&&c.sections[this._mode]){var A=this._props[this._mode],
z=A.order[c.sections[this._mode]];A.size&&z>A.position+A.size&&(z=0)}b=b?b:a.locate_holder(z);for(k=0;k<m.length;k+=2){r=Math.max(m[k],f.first_hour*60);t=Math.min(m[k+1],f.last_hour*60);if(t<=r)if(k+2<m.length)continue;else return[];s=a._get_block_by_config(c);s.className=o;var E=this.config.hour_size_px*24+1,D=36E5;s.style.top=Math.round((r*6E4-this.config.first_hour*D)*this.config.hour_size_px/D)%E+"px";s.style.lineHeight=s.style.height=Math.max(Math.round((t-r)*6E4*this.config.hour_size_px/D)%
E,1)+"px";b.appendChild(s);e.push(s)}}return e};a.markTimespan=function(c){var b=a._prepare_timespan_options(c);if(b.length){for(var d=[],e=0;e<b.length;e++){var f=b[e],h=a._render_marked_timespan(f,null,null);h.length&&d.push.apply(d,h)}return d}};a.unmarkTimespan=function(a){if(a)for(var b=0;b<a.length;b++){var d=a[b];d.parentNode&&d.parentNode.removeChild(d)}};a._marked_timespans_ids={};a.addMarkedTimespan=function(c){var b=a._prepare_timespan_options(c),d="global";if(b.length){var e=b[0].id,f=
a._marked_timespans,h=a._marked_timespans_ids;h[e]||(h[e]=[]);for(var i=0;i<b.length;i++){var g=b[i],j=g.days,l=g.zones,m=g.css,o=g.sections,n=g.type;g.id=e;if(o)for(var p in o){if(o.hasOwnProperty(p)){f[p]||(f[p]={});var k=o[p],q=f[p];q[k]||(q[k]={});q[k][j]||(q[k][j]={});if(!q[k][j][n]){q[k][j][n]=[];if(!a._marked_timespans_types)a._marked_timespans_types={};a._marked_timespans_types[n]||(a._marked_timespans_types[n]=!0)}var r=q[k][j][n];g._array=r;r.push(g);h[e].push(g)}}else{f[d][j]||(f[d][j]=
{});f[d][j][n]||(f[d][j][n]=[]);if(!a._marked_timespans_types)a._marked_timespans_types={};a._marked_timespans_types[n]||(a._marked_timespans_types[n]=!0);r=f[d][j][n];g._array=r;r.push(g);h[e].push(g)}}return e}};a._add_timespan_zones=function(a,b){var d=a.slice(),b=b.slice();if(!d.length)return b;for(var e=0;e<d.length;e+=2)for(var f=d[e],h=d[e+1],i=e+2==d.length,g=0;g<b.length;g+=2){var j=b[g],l=b[g+1];if(l>h&&j<=h||j<f&&l>=f)d[e]=Math.min(f,j),d[e+1]=Math.max(h,l),e-=2;else{if(!i)continue;var m=
f>j?0:2;d.splice(e+m,0,j,l)}b.splice(g--,2);break}return d};a._subtract_timespan_zones=function(a,b){for(var d=a.slice(),e=0;e<d.length;e+=2)for(var f=d[e],h=d[e+1],i=0;i<b.length;i+=2){var g=b[i],j=b[i+1];if(j>f&&g<h){var l=!1;f>=g&&h<=j&&d.splice(e,2);f<g&&(d.splice(e,2,f,g),l=!0);h>j&&d.splice(l?e+2:e,l?0:2,j,h);e-=2;break}}return d};a.invertZones=function(c){return a._subtract_timespan_zones([0,1440],c.slice())};a._delete_marked_timespan_by_id=function(c){var b=a._marked_timespans_ids[c];if(b)for(var d=
0;d<b.length;d++)for(var e=b[d],f=e._array,h=0;h<f.length;h++)if(f[h]==e){f.splice(h,1);break}};a._delete_marked_timespan_by_config=function(c){var b=a._marked_timespans,d=c.sections,e=c.days,f=c.type||w,h=[];if(d)for(var i in d){if(d.hasOwnProperty(i)&&b[i]){var g=d[i];b[i][g]&&b[i][g][e]&&b[i][g][e][f]&&(h=b[i][g][e][f])}}else b.global[e]&&b.global[e][f]&&(h=b.global[e][f]);for(var j=0;j<h.length;j++){var l=h[j],m=a._subtract_timespan_zones(l.zones,c.zones);if(m.length)l.zones=m;else{h.splice(j,
1);j--;for(var o=a._marked_timespans_ids[l.id],n=0;n<o.length;n++)if(o[n]==l){o.splice(n,1);break}}}};a.deleteMarkedTimespan=function(c){if(!arguments.length)a._marked_timespans={global:{}},a._marked_timespans_ids={},a._marked_timespans_types={};if(typeof c!="object")a._delete_marked_timespan_by_id(c);else{if(!c.start_date||!c.end_date){if(!c.days)c.days="fullweek";if(!c.zones)c.zones="fullday"}var b=[];if(c.type)b.push(c.type);else for(var d in a._marked_timespans_types)b.push(d);for(var e=a._prepare_timespan_options(c),
f=0;f<e.length;f++)for(var h=e[f],i=0;i<b.length;i++){var g=a._lame_clone(h);g.type=b[i];a._delete_marked_timespan_by_config(g)}}};a._get_types_to_render=function(a,b){var d=a?a:{},e;for(e in b||{})b.hasOwnProperty(e)&&(d[e]=b[e]);return d};a._get_configs_to_render=function(a){var b=[],d;for(d in a)a.hasOwnProperty(d)&&b.push.apply(b,a[d]);return b};a.attachEvent("onScaleAdd",function(c,b){if(!(a._table_view&&a._mode!="month")){var d=b.getDay(),e=b.valueOf(),f=this._mode,h=a._marked_timespans,i=[];
if(this._props&&this._props[f]){var g=this._props[f],j=g.options,l=a._get_unit_index(g,b),m=j[l],b=a.date.date_part(new Date(this._date)),d=b.getDay(),e=b.valueOf();if(h[f]&&h[f][m.key]){var o=h[f][m.key],n=a._get_types_to_render(o[d],o[e]);i.push.apply(i,a._get_configs_to_render(n))}}var p=h.global,k=p[e]||p[d];i.push.apply(i,a._get_configs_to_render(k));for(var q=0;q<i.length;q++)a._render_marked_timespan(i[q],c,b)}})})()});
