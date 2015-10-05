﻿/*!@license
* Infragistics.Web.ClientUI Pivot Data Selector localization resources 14.1.20141.2031
*
* Copyright (c) 2011-2014 Infragistics Inc.
*
* http://www.infragistics.com/
*
*/

/*global jQuery */
(function ($) {
    $.ig = $.ig || {};

    if (!$.ig.PivotDataSelector) {
        $.ig.PivotDataSelector = {};

        $.extend($.ig.PivotDataSelector, {
            locale: {
                invalidBaseElement: " n'est pas pris en charge comme élément de base. Utiliser plutôt DIV.",
                catalog: "Catalogue",
                cube: "Cube",
                measureGroup: "Groupe de mesures",
                measureGroupAll: "(Tous)",
                rows: "Lignes",
                columns: "Colonnes",
                measures: "Mesures",
                filters: "Filtres",
                deferUpdate: "Différer la mise à jour",
                updateLayout: "Mettre à jour la disposition",
                selectAll: "Sélectionner tout"
            }
        });
    }
})(jQuery);