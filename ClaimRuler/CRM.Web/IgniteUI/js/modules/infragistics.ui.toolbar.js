﻿/*!@license
* Infragistics.Web.ClientUI Toolbar localization resources 14.1.20141.2031
*
* Copyright (c) 2011-2014 Infragistics Inc.
*
* http://www.infragistics.com/
*
*/
(function($){$.ig=$.ig||{};if(!$.ig.Toolbar){$.ig.Toolbar={};$.extend($.ig.Toolbar,{locale:{collapseButtonTitle:"Collapse",expandButtonTitle:"Expand"}})}})(jQuery);/*!@license
 * Infragistics.Web.ClientUI Toolbar 14.1.20141.2031
 *
 * Copyright (c) 2011-2014 Infragistics Inc.
 * <Licensing info>
 *
 * http://www.infragistics.com/
 * 
 * Depends on: 
 *   jquery-1.9.1.js
 *   jquery.ui.core.js
 *   jquery.ui.widget.js
 *   infragistics.util.js
 *   infragistics.ui.shared.js
 *   infragistics.ui.popover.js
 *   infragistics.ui.toolbarbutton.js
 *   infragistics.ui.splitbutton.js
 *   infragistics.ui.colorpicker.js
 *   infragistics.ui.colorpickersplitbutton.js
 *   infragistics.ui.combo.js
 */
if(typeof jQuery!=="function"){throw new Error("jQuery is undefined")}(function($){$.ig=$.ig||{};$.ig.igToolbarItemBaseDescriptor=Class.extend({settings:{width:null,height:null,props:{scope:{value:null}}},_updatedProperties:[],init:function(item){this.settings=$.extend(true,{},this.settings,item);this.name=item.name;this.type=item.type;if(this.settings.scope){this.settings.props.scope=this.settings.scope}},updateProperty:function(name,value){this.settings.props[name].value=value;this._updatedProperties.push(this.settings.props[name])},getProperty:function(name){return this.settings.props[name]},getUpdatedProperties:function(){return this._updatedProperties},getProperties:function(){return this.settings.props},callbackRenderer:function(){if(this.settings.callbackRenderer&&$.isFunction(this.settings.callbackRenderer)){return this.settings.callbackRenderer()}},handler:function(){return this.settings.handler}});$.ig.igToolbarButtonDescriptor=$.ig.igToolbarItemBaseDescriptor.extend({settings:{props:{onlyIcons:{value:true},labelText:{value:"&nbsp;"}}},init:function(item){this._super(item)}});$.ig.igToolbarSplitButtonDescriptor=$.ig.igToolbarItemBaseDescriptor.extend({settings:{props:{items:[]}},init:function(item){this._super(item)}});$.ig.igToolbarComboDescriptor=$.ig.igToolbarItemBaseDescriptor.extend({settings:{props:{valueKey:{value:"text"},textKey:{value:"value"},enableCheckboxes:{value:false},dropDownOnFocus:{value:true},selectedItems:{value:[{index:0}]},enableClearButton:{value:false},dataSource:{value:null},mode:{value:"dropdown"},dropDownAsChild:{value:true},focusOnSelect:{value:false}}},init:function(item){this._super(item);if(this.settings.dataSource){this.settings.props.dataSource.value=this.settings.dataSource}}});$.widget("ui.igToolbar",{options:{height:null,width:null,allowCollapsing:true,collapseButtonIcon:"ui-igbutton-collapsed",expandButtonIcon:"ui-igbutton-expanded",name:"",displayName:"",items:[],isExpanded:true},events:{toolbarButtonClick:"toolbarButtonClick",toolbarComboOpening:"toolbarComboOpening",toolbarComboSelected:"toolbarComboSelected",toolbarMouseDown:"toolbarMouseDown",toolbarCustomItemClick:"toolbarCustomItemClick",itemRemoved:"itemRemoved",itemAdded:"itemAdded",collapsing:"collapsing",collapsed:"collapsed",expanding:"expanding",expanded:"expanded",itemDisable:"itemDisable",itemEnabled:"itemEnabled"},css:{toolbarWidget:"ui-widget ui-widget-content ui-igtoolbar ui-corner-all",toolbarWrapperConteiner:"ui-widget ui-widget-content ui-igtoolbar ui-corner-all",toolbarCollapsedButton:"ui-state-default ui-igbutton-all-caps",igToolbarSeparator:"ig-toolbar-separator ui-widget-content"},_id:function(id){return this.element[0].id+id},widget:function(){return this.element},_create:function(){var toolbar=this.options;this._tbHash={};for(j=0;j<toolbar.items.length;j++){if(!toolbar.items[j].type){toolbar.items[j].type="custom"}itemDescriptor=toolbar.items[j]=this._getToolbarItemDescriptor(toolbar.items[j]);this._tbHash={toolbarOpts:toolbar};this._tbHash.isExpanded=false;for(property in itemDescriptor.getProperties()){if(itemDescriptor.getProperties().hasOwnProperty(property)){if(this._tbHash===undefined){this._tbHash={}}this._tbHash[property]=itemDescriptor}}}this._render();this._createItems()},getToolbarHash:function(){return this._tbHash},_getToolbarItemDescriptor:function(item){return new this._toolbarItemsDescriptors[item.type](item)},_toolbarItemsDescriptors:{button:$.ig.igToolbarButtonDescriptor,0:$.ig.igToolbarButtonDescriptor,combo:$.ig.igToolbarComboDescriptor,1:$.ig.igToolbarComboDescriptor,splitButton:$.ig.igToolbarSplitButtonDescriptor,2:$.ig.igToolbarSplitButtonDescriptor,splitButtonColor:$.ig.igToolbarSplitButtonDescriptor,3:$.ig.igToolbarSplitButtonDescriptor,custom:$.ig.igToolbarItemBaseDescriptor},_init:function(){this._attachEvents();if(!this.options.isExpanded){this.buttonsList.hide();this.collapseBtn.igToolbarButton("toggle").children(":first").switchClass(this.options.collapseButtonIcon,this.options.expandButtonIcon)}this._width=this.collapseBtn.outerWidth(true)+this.buttonsList.width();this._height=this.element.height()},_render:function(){var o=this.options;this.element.addClass(this.css.toolbarWidget);this.element.width(this.options.width).height(this.options.height);this.collapseBtn=$('<div tabIndex="0" id="'+this._id("_collapseButton")+'"></div>').appendTo(this.element).igToolbarButton({onlyIcons:true,labelText:"&nbsp;",title:$.ig.Toolbar.locale.collapseButtonTitle+" "+this.options.displayName,icons:{primary:o.collapseButtonIcon}});this.toolbarBody=this.element.find("#"+this._id("_toolbar"));this.buttonsList=$('<span id="'+this._id("_toolbar_buttons")+'" style="display:inline-block"></span>').appendTo(this.element)},_onCollapse:function(e){var noCancel,event,cancelableEvent,options=this.options,width,self=this,visibility,opacity;if(!options.allowCollapsing){return}if(options.isExpanded){event="collapsed";cancelableEvent="collapsing";options.isExpanded=false;width=this.element.height();this.collapseBtn.attr("title",$.ig.Toolbar.locale.expandButtonTitle+" "+this.options.displayName).children(":first").switchClass(this.options.collapseButtonIcon,this.options.expandButtonIcon);visibility="hidden";opacity="0.0"}else{event="expanded";cancelableEvent="expanding";options.isExpanded=true;width=this._width;this.buttonsList.show();this.collapseBtn.attr("title",$.ig.Toolbar.locale.collapseButtonTitle+" "+this.options.displayName).children(":first").switchClass(this.options.expandButtonIcon,this.options.collapseButtonIcon);visibility="visible";opacity="1"}e.stopPropagation();noCancel=this._trigger(this.events[cancelableEvent],e,{owner:this,toolbarElement:this.element,toolbar:{}});if(noCancel){this.element.css({overflow:"hidden"});this.element.animate({width:width},300,null,function(){if(!options.isExpanded){self.buttonsList.hide()}self._trigger(self.events[event],e,{owner:self,toolbarElement:self.element,toolbar:{}})})}},_setOption:function(name,value){$.Widget.prototype._setOption.apply(this,arguments);switch(name){case"allowCollapsing":this.options.allowCollapsing=value;break;case"items":this._updateItems(value);break}},_isSelectedAction:function(el,props,itemOptionObj){if(props.value){el.addClass("ui-state-active")}},_createItems:function(){var o=this.options,i,self=this,itemProps={},newItem,tbItemsHash={button:"igToolbarButton",combo:"igCombo",splitButton:"igSplitButton",splitButtonColor:"igColorPickerSplitButton"},tbItemsPropsTraversing=function(key,property){var scope=o.items[i].scope||self;if(property.action!==undefined&&$.isFunction(scope[property.action])){scope[property.action](newItem,property,itemProps);return}itemProps[key]=property.value};this.buttonsList.empty();for(i=0;i<o.items.length;i++){itemProps={};newItem=(o.items[i].callbackRenderer()||$('<div tabIndex="0"></div>')).attr("id",this._id("_item_"+o.items[i].name)).appendTo(this.buttonsList);$.each(o.items[i].getProperties(),tbItemsPropsTraversing);if(tbItemsHash.hasOwnProperty(o.items[i].type)){newItem[tbItemsHash[o.items[i].type]](itemProps);continue}}},_updateItems:function(items){var options=this.options,updProps,i,j,scope,el,key;for(i=0;i<items.length;i++){updProps=items[i].getUpdatedProperties();el=this.getItem(items[i].name);scope=options.items[i].scope||this;for(j=0;j<updProps.length;j++){if(updProps[j].action!==undefined&&$.isFunction(scope[updProps[j].action])){scope[updProps[j].action](el,updProps[j]);continue}if(items[i]instanceof $.ig.igToolbarButtonDescriptor){el.igToolbarButton("option",key,updProps[j])}if(options.items[i]instanceof $.ig.igToolbarComboDescriptor){el.igCombo("option",key,updProps[j])}}}},_tooltipAction:function(el,props,itemOptionObj){if(itemOptionObj!==undefined){itemOptionObj.title=props.value}else{el.igToolbarButton("option","title",props.value)}},_buttonIconAction:function(el,props,itemOptionObj){if(itemOptionObj!==undefined){itemOptionObj.icons={primary:props.value}}else{el.igToolbarButton("option","icons",{primary:props.value})}},_comboDataSourceAction:function(el,props,itemOptionObj){if(itemOptionObj!==undefined){itemOptionObj.dataSource=props.value}else{el.igCombo("option","dataSource",props.value)}},_comboWidthAction:function(el,props,itemOptionObj){if(itemOptionObj!==undefined){itemOptionObj.width=props.value}else{el.igCombo("option","width",props.value)}},_comboHeightAction:function(el,props,itemOptionObj){if(itemOptionObj!==undefined){itemOptionObj.height=props.value}else{el.igCombo("option","height",props.value)}},_spltBtnTooltipAction:function(el,props,itemOptionObj){},_comboSelectedItem:function(el,props,itemOptionObj){if(itemOptionObj!==undefined){itemOptionObj.selectedItems=[{value:props.value}]}else{el.igCombo("option","selectedItems",[{value:props.value}])}},_spltButtonColorAction:function(el,props,itemOptionObj){if(itemOptionObj!==undefined){itemOptionObj.defaultColor=props.value}else{el.igColorPickerSplitButton("option","defaultColor",props.value)}},_comboDropDownListWidth:function(el,props,itemOptionObj){if(itemOptionObj!==undefined){itemOptionObj.dropDownWidth=props.value}else{el.igCombo("option","dropDownWidth",props.value)}},_getWidgetType:function(el){var data,i;if(el===undefined){return}data=el.data();for(i in data){if(data.hasOwnProperty(i)&&data[i].widgetName){return data[i].widgetName}}},_isWidgetSupported:function(name){var i;for(i=0;i<this.supportedWidgets.length;i++){if(this.supportedWidgets[i].name===name){return true}}},_attachEvents:function(){var toolbarItemsEvents="igtoolbarbuttonclick igsplitbuttonclick igcolorpickersplitbuttoncolorselected";this.element.delegate(".ui-widget",toolbarItemsEvents,$.proxy(this._onToolbarItemInteraction,this));this.element.delegate(":ui-igCombo","igcomboselectionchanged",$.proxy(this._onComboListItemClick,this));this.collapseBtn.bind("igtoolbarbuttonclick",$.proxy(this._onCollapse,this))},_onToolbarItemInteraction:function(e,ui){var selectedItemValue,targetWidget=$(e.target).parentsUntil(":ui-igToolbar").eq(-2),selectedItemIndex,triggeredEvent,o=this.options;if(targetWidget.length===0){targetWidget=$(e.target)}selectedItemIndex=this.buttonsList.children().index(targetWidget);switch(e.type){case"igtoolbarbuttonclick":triggeredEvent=this.events.toolbarButtonClick;break;default:triggeredEvent=this.events.toolbarCustomItemClick;selectedItemValue=ui.value;break}this._trigger(triggeredEvent,e,{name:ui.name||o.items[selectedItemIndex].name,value:selectedItemValue,handler:o.items[selectedItemIndex].handler(),scope:o.items[selectedItemIndex].getProperty("scope"),itemProperties:o.items[selectedItemIndex].getProperties(),toolbarItem:targetWidget,toolbarName:o.name})},_onComboListItemClick:function(e,data){var toolbarItemIndex,toolbarItem;toolbarItemIndex=this.buttonsList.children().index($(e.currentTarget));toolbarItem=this.options.items[toolbarItemIndex];this._trigger(this.events.toolbarComboSelected,e,{name:toolbarItem.name,value:data.items[0].value,handler:toolbarItem.handler(),scope:toolbarItem.getProperty("scope"),itemProperties:toolbarItem.getProperties(),toolbarItem:data.owner,toolbarName:this.options.name})},getItem:function(index){var result;if(!isNaN(parseInt(index,10))){return this.buttonsList.children().eq(index)}if(typeof index==="string"){result=this.buttonsList.find("#"+this._id("_item_"+index));if(result.length){return result}}},addItem:function(item){if(this._isWidgetSupported(item.type)){this.options.items.push(item);this._createItems();this._trigger(this.events.itemAdded)}},removeItem:function(index){this.buttonsList.eq(index).remove();this._trigger(this.events.itemremoved)},disableItem:function(index,disabled){var item=this.getItem(index),widgetType=this._getWidgetType(item);if(widgetType){item[this._getWidgetType(item)]("option","disabled",disabled);this._trigger(this.events.itemDisable,{isDisabled:disabled})}},activateItem:function(index,activated){var item=this.getItem(index),action=activated?item.addClass:item.removeClass;action.call(this,"ui-state-active");item.igToolbarButton("options","isSelected",activated);this._trigger(this.events.itemActivated,{isActivated:activated})},deactivateAll:function(){this.buttonsList.find(".ui-igbutton.ui-state-active").removeClass("ui-state-active").igToolbarButton("option","isSelected",false)},_setCollapseExpandButtonIcon:function(){if(this.options.collapseButtonIcon){this.collapseBtn.switchClass("ui-icon-triangle-1-w",this.options.collapseButtonIcon)}},destroy:function(){$.Widget.prototype.destroy.apply(this,arguments);this.element.undelegate();this.element.unbind();delete this.buttonsList;delete this.collapseBtn;delete this.toolbarBody;this.element.remove()}});$.extend($.ui.igToolbar,{version:"14.1.20141.2031"})})(jQuery);