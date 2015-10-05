﻿/*!@license
* Infragistics.Web.ClientUI FunnelChart 14.1.20141.2031
*
* Copyright (c) 2011-2014 Infragistics Inc.
*
* http://www.infragistics.com/
*
* Depends:
*     jquery-1.4.4.js
*     jquery.ui.core.js
*     jquery.ui.widget.js
*     infragistics.util.js
*/
if(typeof jQuery==="undefined"){throw new Error("jQuery is undefined")}(function($){$.widget("ui.igBaseChart",{css:{tooltip:"ui-widget-content ui-corner-all",unsupportedBrowserClass:"ui-html5-non-html5-supported-message ui-helper-clearfix ui-html5-non-html5"},events:{dataBinding:null,dataBound:null,updateTooltip:null,hideTooltip:null},options:{width:null,height:null,tooltipTemplate:null,maxRecCount:null,dataSource:null,dataSourceType:null,dataSourceUrl:null,responseTotalRecCountKey:null,responseDataKey:null},_create:function(){var key,v,size,chart,i=-1,self=this,elem=self.element,style=elem[0].style,o=self.options;self._old_state={style:{position:style.position,width:style.width,height:style.height},elems:elem.find("*")};if(!$.ig.util._isCanvasSupported()){$.ig.util._renderUnsupportedBrowser(this);return}chart=self._createChart();self.dataBind();while(i++<1){key=i===0?"width":"height";if(o[key]){size=key}else{v=elem[0].style[key];if(v&&v.indexOf("%")>0){self._setSize(chart,size=key,v)}}}if(!size){self._setSize(chart,"width")}this._beforeInitialOptions(chart,elem);this._setInitialOptions(chart);if(self.css.chart){elem.addClass(self.css.chart)}self._chart=chart;self._dataChange();this._provideContainer(chart,elem)},_beforeInitialOptions:function(chart,elem){},_provideContainer:function(chart,elem){chart.provideContainer(elem[0])},_setInitialOptions:function(chart){var o=this.options,self=this;for(var key in o){if(o.hasOwnProperty(key)){v=o[key];if(v!==null){self._set_option(chart,key,v)}}}},_fireTooltip:function(text,item,x,y){var arg,t=this._t_t;if(!text){t=this._t_t_e||t;if(t&&t.css("display")!=="none"&&this._trigger("hideTooltip",null,arg={owner:this,element:t,item:this._t_t_i})){t=arg.element||t;t.css("display","none")}return}if(!t){t=this._t_t=$("<div style='position:absolute;display:none;white-space:nowrap;'></div>").addClass(this.css.tooltip).appendTo(this.element)}text=$.ig.tmpl?$.ig.tmpl(text,item):text;x=this._trigger("updateTooltip",null,arg={owner:this,element:t,text:text,item:item,x:x,y:y});this._t_t_e=t=arg.element||t;this._t_t_i=arg.item;if(!x){t.css("display","none")}else{t.css({display:"block",left:arg.x+"px",top:arg.y+"px"});if(arg.text){t.html(arg.text)}}},findIndexOfItem:function(item){var ds=item?this.getData():null,i=ds?ds.length:0;while(i-->0){if(item===ds[i]){break}}return i},getDataItem:function(index){var ds=this.getData();return ds&&ds.length>index&&index>=0?ds[index]:null},getData:function(){return this._chart?this._chart.itemsSource():null},addItem:function(item){if(this._dataSource){this._dataEvt(1,true);this._dataSource.addRow(null,item,true)}return this},insertItem:function(item,index){if(this._dataSource){this._dataEvt(2,true);this._dataSource.insertRow(null,item,index,true)}return this},removeItem:function(index){if(this._dataSource){this._dataEvt(-1,true);this._dataSource.deleteRow(index,true)}return this},setItem:function(index,item){if(this._dataSource){this._dataEvt(0,true);this._dataSource.updateRow(index,item,true)}return this},notifySetItem:function(dataSource,index,newItem,oldItem){if(this._chart){this._chart.notifySetItem(dataSource,index,oldItem,newItem);this._dataEvt(0)}return this},notifyClearItems:function(dataSource){if(this._chart){this._chart.notifyClearItems(dataSource);this._dataEvt(-1)}return this},notifyInsertItem:function(dataSource,index,newItem){if(this._chart){this._chart.notifyInsertItem(dataSource,index,newItem);this._dataEvt(2)}return this},notifyRemoveItem:function(dataSource,index,oldItem){if(this._chart){this._chart.notifyRemoveItem(dataSource,index,oldItem);this._dataEvt(-1)}return this},_dataEvt:function(act,before){},_itemAdded:function(item,dataSource,dataSourceOwnerName){var owner=this._getDataSourceOwner(dataSourceOwnerName);if(owner){owner.notifyInsertItem(dataSource,dataSource.dataView().length-1,item.row);this._dataEvt(1)}},_itemInserted:function(item,dataSource,dataSourceOwnerName){var owner=this._getDataSourceOwner(dataSourceOwnerName);if(owner){owner.notifyInsertItem(dataSource,item.rowIndex,item.row);this._dataEvt(2)}},_itemUpdated:function(item,dataSource,dataSourceOwnerName){var owner=this._getDataSourceOwner(dataSourceOwnerName);if(owner){owner.notifySetItem(dataSource,item.rowIndex,item.oldRow,item.newRow);this._dataEvt(0)}},_itemRemoved:function(item,dataSource,dataSourceOwnerName){var owner=this._getDataSourceOwner(dataSourceOwnerName);if(owner){owner.notifyRemoveItem(dataSource,item.rowIndex,item.row);this._dataEvt(-1)}},_getValueKeyName:function(){return null},_getRemoteDataKeys:function(){return null},_getNotifyResizeName:function(){return null},_createChart:function(){return null},_set_option:function(chart,key,value){if(!key){return true}if(key.indexOf("dataSource")>=0||key.indexOf("response")>=0){if(this._chart){this.dataBind()}return true}if(key==="width"||key==="height"){this._setSize(chart,key,value);return true}if(key==="maxRecCount"){if(this._chart){this._dataChange()}return true}if(key==="tooltipTemplate"&&chart.toolTip){chart.toolTip(value);return true}if(!chart||!chart[key]||chart[key]()===value){return true}},_setSize:function(chart,key,val){$.ig.util.setSize(this.element,key,val,chart,this._getNotifyResizeName())},_getDataSourceOwner:function(dataSourceOwnerName){return this._chart},_dataChange:function(noFire,dataSourceOwnerName){var owner;if(!this._getDataSourceOwner){return}if(dataSourceOwnerName){owner=this._getDataSourceOwner(dataSourceOwnerName);this._dataChangeInternal(owner,noFire)}else{this._dataChangeInternal(this._chart,noFire)}},_dataChangeInternal:function(owner,noFire){var data,len,max=this.options.maxRecCount,ds=this._dataSource,chart=owner;if(!ds||!chart||!chart.itemsSource){return}data=ds.dataView();len=data?data.length:0;if(!len&&!this._dataLen){return}this._dataLen=len;if(len&&max&&max<len){noFire=[];while(max-->0){noFire[max]=data[max]}data=noFire}chart.itemsSource(data);if(noFire!=="no"){this._trigger("dataBound",null,{owner:this,dataSource:ds,data:data})}this._dataEvt(3)},chart:function(){return this._chart},dataBind:function(){this._dataBindInternal(this.options,null)},_dataBindInternal:function(options,dataSourceOwnerName){var field,ds0,dataOptions,vt,setting,bound,o=options,url=o.dataSourceUrl,key=o.responseDataKey,type=o.dataSourceType,valKeyName=this._getValueKeyName(),valKey=valKeyName?o[valKeyName]:null,ds=o.dataSource,dsStr=typeof ds==="string",keys=key?key.split("."):null,len=keys?keys.length-1:-1,i=-1;if(dsStr&&!type){ds=new $.ig.JSONPDataSource({dataSource:ds})}ds0=ds;while(ds0&&i++<len){ds0=ds0[keys[i]]}if(!ds0){ds0=ds;keys=null}field=ds0?ds0[0]:null;if(typeof field==="string"||typeof field==="number"||field&&field.getTime){i=ds0.length;field=ds0;ds0=[];valKey=valKey||"x";if(valKeyName){o[valKeyName]=valKey}while(i-->0){ds0[i]={};ds0[i][valKey]=field[i]}if(keys){field=ds;i=-1;while(++i<len){field=field[keys[i]]}field[keys[len]]=ds0}else{ds=ds0}}if(ds0&&!valKey&&valKeyName){for(valKey in ds0[0]){if(ds0[0].hasOwnProperty(valKey)){o[valKeyName]=valKey;break}}}dataOptions={callback:this._dataChange,dataSource:ds,type:type||undefined,responseDataKey:key,responseTotalRecCountKey:o.responseTotalRecCountKey,rowAdded:this._itemAdded,rowDeleted:this._itemRemoved,rowUpdated:this._itemUpdated,rowInserted:this._itemInserted};if(dataSourceOwnerName){dataOptions.callback=function(nofire){this._dataChange(nofire,dataSourceOwnerName)};dataOptions.rowAdded=function(item,dataSource){this._itemAdded(item,dataSource,dataSourceOwnerName)};dataOptions.rowDeleted=function(item,dataSource){this._itemRemoved(item,dataSource,dataSourceOwnerName)};dataOptions.rowUpdated=function(item,dataSource){this._itemUpdated(item,dataSource,dataSourceOwnerName)};dataOptions.rowInserted=function(item,dataSource){this._itemInserted(item,dataSource,dataSourceOwnerName)}}if(ds instanceof $.ig.DataSource){bound=ds._data&&ds._data.length;dataOptions.dataSource=ds.settings.dataSource;ds.settings=$.extend(true,{},ds.settings,dataOptions);ds.settings.callee=this}else{ds=new $.ig.DataSource(dataOptions);ds.settings.callee=this}if(!bound&&!this._trigger("dataBinding",null,{owner:this,dataSource:ds})){return}this._dataSource=ds;if(bound){this._dataChange("no",dataSourceOwnerName)}else{ds.dataBind()}if(url&&!this._urlBind){setting=ds.settings;setting.dataSource=url;setting.type="remoteUrl";ds._runtimeType=ds.analyzeDataSource();keys=this._getRemoteDataKeys();len=keys?keys.length:0;if(len>0){key=null;while(len-->0){i=keys[len];if(i){key=key?key+","+i:i}}if(key){setting.urlParamsEncoded=$.proxy(function(data,params){if(params&&params.filteringParams){params.filteringParams.keys=key}},this)}}this._urlBind=1;if(!o.dataSource){ds.dataBind()}}},destroy:function(){var key,style,chart=this._chart,old=this._old_state,elem=this.element;if(!old){return}elem.find("*").not(old.elems).remove();if(this.css.chart){elem.removeClass(this.css.chart)}old=old.style;style=elem[0].style;for(key in old){if(old.hasOwnProperty(key)){if(style[key]!==old[key]){style[key]=old[key]}}}if(chart){this._setSize(chart)}$.Widget.prototype.destroy.apply(this,arguments);if(chart&&chart.destroy){chart.destroy()}delete this._chart;delete this._old_state}});$.extend($.ui.igBaseChart,{version:"14.1.20141.2031"})})(jQuery);