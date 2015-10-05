using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Repository;
using CRM.Data.Account;
using CRM.Data;
using CRM.Web.Utilities;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class AdjusterImport : System.Web.UI.Page {
		int clientID = 0;
		int userID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			clientID = SessionHelper.getClientId();

			userID = SessionHelper.getUserId();
		}

		protected void btnUpload_Click(object sender, EventArgs e) {
			string fileExtension = null;
			AdjusterMaster adjuster = null;
			string firstName = null;
			string lastName = null;
			AdjusterServiceArea serviceArea = null;
			AdjusterLicenseAppointmentType licenseAppointmentType = null;
			string licenseNumber = null;
			string licenseType = null;
			string licenseEffectiveDate = null;
			string licenseExpireDate = null;
			string appointmentType = null;
			StateMaster stateMaster = null;
			string str = null;

			lblMessage.Text = string.Empty;

			if (!fileUpload.HasFile) {
				lblMessage.Text = "No file selected.";
				lblMessage.CssClass = "error";
				return;
			}

			// validate file
			fileExtension = System.IO.Path.GetExtension(fileUpload.PostedFile.FileName);
			if (!fileExtension.Equals(".csv", StringComparison.OrdinalIgnoreCase)) {
				lblMessage.Text = "CSV file format allowed only.";
				lblMessage.CssClass = "error";
				return;
			}

			try {
				//save file in temp folder
				string destinationPath = Server.MapPath(string.Format("~//Temp//{0}.csv", Guid.NewGuid()));

				fileUpload.SaveAs(destinationPath);

				// load file for processing
				//DataTable table = CSVReader.ReadCSVFile(Server.MapPath("~//Temp//" + fileUpload.FileName), true);

				DataTable table = CSVHelper.ReadCSVFile(destinationPath);

				using (TransactionScope scope = new TransactionScope()) {
					for (int i = 0; i < table.Rows.Count; i++) {
						#region process each row
						adjuster = new AdjusterMaster();
						adjuster.ClientId = clientID;
						adjuster.Status = true;
						adjuster.InsertBy = userID;
						adjuster.InsertDate = DateTime.Now;

						if (table.Rows[i]["First_Name"] != string.Empty) 
							firstName = table.Rows[i]["First_Name"].ToString();							
						
						if (table.Rows[i]["Last_Name"] != string.Empty) 
							lastName = table.Rows[i]["Last_Name"].ToString();																	
						
						// skip blank rows
						if (string.IsNullOrEmpty(lastName) && string.IsNullOrEmpty(firstName))
							continue;

						adjuster.FirstName = firstName.Length < 50 ? firstName : firstName.Substring(0, 50);
						adjuster.LastName = lastName.Length < 50 ? lastName : lastName.Substring(0, 50);

						if (table.Rows[i]["Middle_Initial"] != string.Empty) {
							str = table.Rows[i]["Middle_Initial"].ToString();
							adjuster.MiddleInitial = str.Substring(0, 1);
						}

						adjuster.AdjusterName = string.Format("{0} {1} {2}", firstName, adjuster.MiddleInitial ?? "", lastName);

						if (table.Rows[i]["Suffix"] != string.Empty)
							adjuster.Suffix = table.Rows[i]["Suffix"].ToString().Substring(0,1);

						if (table.Rows[i]["Cell_Phone_Number"] != string.Empty) {
							str = table.Rows[i]["Cell_Phone_Number"].ToString();
							if (str.Length > 20)
								adjuster.PhoneNumber = str.Substring(0, 20);
							else
								adjuster.PhoneNumber = str;
						}
						if (table.Rows[i]["Email"] != string.Empty) {
							str = table.Rows[i]["Email"].ToString();
							adjuster.email = str.Length < 100 ? str : str.Substring(0, 100);
						}
						if (table.Rows[i]["Address_Line1"] != string.Empty) {
							str = table.Rows[i]["Address_Line1"].ToString();
							adjuster.Address1 = str.Length < 100 ? str : str.Substring(0, 100);
						}
						if (table.Rows[i]["City"] != string.Empty) {
							str = table.Rows[i]["City"].ToString();
							adjuster.Address1 = str.Length < 50 ? str : str.Substring(0, 50);
						}
						if (table.Rows[i]["State_Code"] != string.Empty) {
							str = table.Rows[i]["State_Code"].ToString();
							stateMaster = State.Getstateid(str);
							if (stateMaster.StateId > 0)
								adjuster.StateID = stateMaster.StateId;
							else
								adjuster.StateID = null;

						}
						if (table.Rows[i]["ZIP_Code"] != string.Empty) {
							str = table.Rows[i]["ZIP_Code"].ToString();
							adjuster.Address1 = str.Length < 10 ? str : str.Substring(0, 10);
						}

						// save adjuster
						adjuster = AdjusterManager.Save(adjuster);

						if (table.Rows[i]["License_Type"] != string.Empty)
							licenseType = table.Rows[i]["License_Type"].ToString();

						if (table.Rows[i]["License_Number"] != string.Empty)
							licenseNumber = table.Rows[i]["License_Number"].ToString();

						if (table.Rows[i]["License_Issue_Date"] != string.Empty)
							licenseEffectiveDate = table.Rows[i]["License_Issue_Date"].ToString();

						if (table.Rows[i]["License_Expires_Date"] != string.Empty)
							licenseExpireDate = table.Rows[i]["License_Expires_Date"].ToString();

						if (table.Rows[i]["Appointment_Type"] != string.Empty) {
							appointmentType = table.Rows[i]["Appointment_Type"].ToString();

							if (!string.IsNullOrEmpty(appointmentType)) {
								licenseAppointmentType = AdjusterLicenseAppointmentTypeManager.Get(appointmentType, clientID);
								if (licenseAppointmentType == null) {
									licenseAppointmentType = new AdjusterLicenseAppointmentType();
									licenseAppointmentType.IsActive = true;
									licenseAppointmentType.LicenseAppointmentType = appointmentType;
									licenseAppointmentType.ClientID = clientID;

									licenseAppointmentType = AdjusterLicenseAppointmentTypeManager.Save(licenseAppointmentType);
								}
							}
						}

						if (!string.IsNullOrEmpty(licenseType) && !string.IsNullOrEmpty(licenseNumber) && !string.IsNullOrEmpty(licenseEffectiveDate) &&
							!string.IsNullOrEmpty(licenseExpireDate) && !string.IsNullOrEmpty(appointmentType)) {

							serviceArea = new AdjusterServiceArea();

							serviceArea.AdjusterID = adjuster.AdjusterId;
							serviceArea.StateID = adjuster.StateID;
							serviceArea.LicenseNumber = licenseNumber.Length < 50 ? licenseNumber : licenseNumber.Substring(0, 50);
							serviceArea.LicenseType = licenseType.Length < 20 ? licenseType : licenseType.Substring(0, 20);

							DateTime effectiveDate = DateTime.MaxValue;
							DateTime expireDate = DateTime.MaxValue;

							if (DateTime.TryParse(licenseEffectiveDate, out effectiveDate))
								serviceArea.LicenseEffectiveDate = effectiveDate;

							if (DateTime.TryParse(licenseExpireDate, out expireDate))
								serviceArea.LicenseExpirationDate = expireDate;

							if (licenseAppointmentType != null)
								serviceArea.AppointmentTypeID = licenseAppointmentType.LicenseAppointmentTypeID;

							AdjusterStateLicenseManager.Save(serviceArea);
						}

						// create notes
						if (table.Rows[i]["Adjuster_Status_Code"] != string.Empty) {
							string adjuster_Status_Code = table.Rows[i]["Adjuster_Status_Code"].ToString();
							addNote(adjuster.AdjusterId, adjuster_Status_Code);							
						}
						if (table.Rows[i]["Adjuster_Reason_Code"] != string.Empty) {
							string sdjuster_Reason_Code = table.Rows[i]["Adjuster_Reason_Code"].ToString();
							addNote(adjuster.AdjusterId, sdjuster_Reason_Code);
						}

						#endregion
					}
					scope.Complete();

					lblMessage.Text = "Import completed successfully.";
					lblMessage.CssClass = "ok";
				}

			}
			catch (Exception ex) {
				lblMessage.Text = "Import not completed successfully.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}
		}

		private void addNote(int adjusterID, string noteText) {
			AdjusterNote note = new AdjusterNote();

			note.AdjusterID = adjusterID;
			
			note.NoteDate = DateTime.Now;
			
			note.UserID = userID;

			note.Notes = noteText;
			
			AdjusterNoteManager.Save(note);
		}
	}
}