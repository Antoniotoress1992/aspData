﻿/*!@license
* Infragistics.Web.ClientUI Pivot Grid localization resources 14.1.20141.2031
*
* Copyright (c) 2011-2014 Infragistics Inc.
*
* http://www.infragistics.com/
*
*/

/*global jQuery */
(function ($) {
    $.ig = $.ig || {};

    if (!$.ig.PivotGrid) {
        $.ig.PivotGrid = {};

        $.extend($.ig.PivotGrid, {
            locale: {
                filtersHeader: "フィルター フィールドをここにドロップ",
                measuresHeader: "データ項目をここにドロップ",
                rowsHeader: "行フィールドをここにドロップ",
                columnsHeader: "列フィールドをここにドロップ",
                disabledFiltersHeader: "フィルター フィールド",
                disabledMeasuresHeader: "データ項目",
                disabledRowsHeader: "行フィールド",
                disabledColumnsHeader: "列フィールド",
                noSuchAxis: "指定した軸はありません。"
            }
        });
    }
})(jQuery);