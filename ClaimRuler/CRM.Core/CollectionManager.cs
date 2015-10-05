#region Namespace
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

using CRM.Data;
using CRM.Data.Entities;
using System.Collections;

#endregion

namespace CRM.Core {

	
    
    
    public class CollectionManager {
		public static void FillCollection(ListControl control, string key, string value, object data, bool isAddSelect = true) {
            
            control.DataSource = data;
			control.DataValueField = key;
			control.DataTextField = value;
			control.DataBind();
          
			if (isAddSelect)
				control.Items.Insert(0, new ListItem("--- Select ---", "0"));
		}

		public static void FillCollection(Infragistics.Web.UI.ListControls.WebDropDown control, string key, string value, object data, bool isAddSelect = true) {
			control.DataSource = data;
			control.ValueField = key;
			control.TextField = value;
			control.DataBind();

			if (isAddSelect)
				control.Items.Insert(0, new Infragistics.Web.UI.ListControls.DropDownItem("--- Select ---", "0"));
		}

		public static void Fillchk(ListControl control, string key, string value, object data) {
			control.DataSource = data;
			control.DataValueField = key;
			control.DataTextField = value;
			control.DataBind();

		}

		public static string GetSelectedItemsID(CheckBoxList control) {
			string selectedValues = null;

			string[] s = control.Items.Cast<ListItem>()
			   .Where(item => item.Selected)
			   .Select(x => x.Value).ToArray();


			if (s != null)
				selectedValues = string.Join(",", s);

			return selectedValues;

		}

		public static void SetSelectedItems(CheckBoxList control, string selectedValues) {
			string[] ids = null;

			if (!string.IsNullOrEmpty(selectedValues)) {
				ids = selectedValues.Split(new char[] { ',' });

				foreach (string id in ids) {
					ListItem item = control.Items.FindByValue(id);
					if (item != null)
						item.Selected = true;
				}
			}
		
		}

		public static void FillStatus(ListControl control) {
			control.DataSource = Enum.GetNames(typeof(Globals.Status));
			control.DataBind();
		}

		public static void FillRange(ListControl control, int min = 1, int max = 10) {
			control.DataSource = GetRange(min, max);
			control.DataValueField = "key";
			control.DataTextField = "value";
			control.DataBind();
		}

		public static IDictionary<string, string> GetRange(int min = 1, int max = 10) {
			return Enumerable.Range(1, max).ToDictionary(k => k.ToString(), v => v.ToString());
		}
	}


    public class DropDownProperty
    {
        public int key { get; set; }
        public string Value { get; set; }

    }


}
