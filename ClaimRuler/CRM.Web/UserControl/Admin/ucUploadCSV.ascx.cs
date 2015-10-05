

namespace CRM.Web.UserControl.Admin {
	#region Namespace
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using CRM.Core;
	using CRM.Data.Account;
	using CRM.Data;
	using System.Transactions;
	using System.Data;
	using System.IO;
    using CRM.Data.Entities;

	#endregion

	public partial class ucUploadCSV : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {

		}

		protected void UploadTemplate2_bak() {
			// original format designed by Vivek
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			// 2013-02-09 tortega -- Added data validation prior to import. File is not aborted in the event of incorrect data.
			string str = null;

			int clientID = Core.SessionHelper.getClientId();

			// assume homeowner
			int policyType = 1;
			DateTime DateSubmitted = DateTime.Now;

			try {
				if (FileUpload1.HasFile) {

					string ext = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
					string ActualFileName = FileUpload1.PostedFile.FileName.Substring(FileUpload1.PostedFile.FileName.LastIndexOf(@"\") + 1);
					string FileNameWithoutExt = ActualFileName.Replace(ext, "");
					if (ext == ".csv") {

						if (!Directory.Exists(Server.MapPath("~//CSVLoad//"))) {
							Directory.CreateDirectory(Server.MapPath("~//CSVLoad//"));
						}
						FileUpload1.SaveAs(Server.MapPath("~//CSVLoad//" + FileUpload1.FileName));
						DataTable table = CSVReader.ReadCSVFile(Server.MapPath("~//CSVLoad//" + FileUpload1.FileName), true);
						using (TransactionScope scope = new TransactionScope()) {
							Leads objLead = null;
							for (int i = 0; i < table.Rows.Count; i++) {

								objLead = new Leads();

								//objLead.LeadStatus = 1;	// active
								objLead.OriginalLeadDate = DateTime.Now;
								objLead.TypeOfProperty = 18;	//residential
								objLead.UserId = Convert.ToInt32(Session["UserId"]);

								if (table.Rows[i]["Create Date"] != string.Empty) {

									DateTime.TryParse(table.Rows[i]["Create Date"].ToString(), out DateSubmitted);

									objLead.DateSubmitted = DateSubmitted;

									objLead.OriginalLeadDate = DateSubmitted;
								}

								if (table.Rows[i]["Last Name"] != string.Empty) {
									str = table.Rows[i]["Last Name"].ToString();
									if (str.Length > 50) {
										objLead.ClaimantLastName = str.Substring(0, 50);
										objLead.OwnerLastName = objLead.ClaimantLastName;
									}
									else {
										objLead.ClaimantLastName = str;
										objLead.OwnerLastName = str;
									}
								}

								if (table.Rows[i]["First Name"] != string.Empty) {
									str = table.Rows[i]["First Name"].ToString();
									if (str.Length > 50) {
										objLead.ClaimantFirstName = str.Substring(0, 50);
										objLead.OwnerFirstName = objLead.ClaimantFirstName;
									}
									else {
										objLead.ClaimantFirstName = str;
										objLead.OwnerFirstName = str;
									}
								}

								if (table.Rows[i]["Middle Name"] != string.Empty) {
									str = table.Rows[i]["Middle Name"].ToString();
									if (str.Length > 50) {
										objLead.ClaimantMiddleName = str.Substring(0, 50);
									}
									else {
										objLead.ClaimantMiddleName = str;
									}
								}


								if (table.Rows[i]["E-mail"] != string.Empty) {
									str = table.Rows[i]["E-mail"].ToString();
									if (str.Length > 100)
										objLead.EmailAddress = str.Substring(0, 100);
									else
										objLead.EmailAddress = str;
								}
								if (table.Rows[i]["Personal E-mail"] != string.Empty) {
									str = table.Rows[i]["Personal E-mail"].ToString();
									if (str.Length > 100)
										objLead.SecondaryEmail = str.Substring(0, 100);
									else
										objLead.SecondaryEmail = str;
								}

								if (table.Rows[i]["Mobile Phone"] != string.Empty) {
									str = table.Rows[i]["Mobile Phone"].ToString();
									if (str.Length > 20)
										objLead.SecondaryPhone = str.Substring(0, 20);
									else
										objLead.SecondaryPhone = str;
								}

								if (table.Rows[i]["Salutation"] != string.Empty) {
									str = table.Rows[i]["Salutation"].ToString();
									if (str.Length > 50)
										objLead.Salutation = str.Substring(0, 20);
									else
										objLead.Salutation = str;
								}


								string adjusterName = null;
								int? adjusterID = null;
								AdjusterMaster adjuster = null;

								if (table.Rows[i]["Adjuster Name"] != string.Empty)
									adjusterName = table.Rows[i]["Adjuster Name"].ToString();

								string adjusterEmail = null;
								string adjusterFax = null;
								string AdjusterPhone = null;
								string AdjusterCo = null;
								string AdjusterAddress = null;

								if (table.Rows[i]["Adjuster Email"] != string.Empty)
									adjusterEmail = table.Rows[i]["Adjuster Email"].ToString();

								if (table.Rows[i]["Adjuster Fax"] != string.Empty)
									adjusterFax = table.Rows[i]["Adjuster Fax"].ToString();

								if (table.Rows[i]["Adjuster Phone"] != string.Empty)
									AdjusterPhone = table.Rows[i]["Adjuster Phone"].ToString();

								if (table.Rows[i]["Adjuster Co"] != string.Empty)
									AdjusterCo = table.Rows[i]["Adjuster Co"].ToString();

								if (table.Rows[i]["Adj Address"] != string.Empty)
									AdjusterAddress = table.Rows[i]["Adj Address"].ToString();

								if (!string.IsNullOrEmpty(adjusterName)) {
									adjuster = AdjusterManager.GetByAdjusterName(adjusterName.Trim());
									if (adjuster.AdjusterId == 0) {
										// add adjuster
										adjuster = new AdjusterMaster();
										adjuster.Status = true;
										adjuster.AdjusterName = adjusterName.Trim();
										adjuster.ClientId = clientID;
										adjuster.InsertBy = objLead.UserId;
										adjuster.InsertDate = DateTime.Now;
										adjuster.isEmailNotification = true;
										adjuster.email = adjusterEmail;
										adjuster.Address1 = AdjusterAddress;
										adjuster.PhoneNumber = AdjusterPhone;
										adjuster.FaxNumber = adjusterFax;
										adjuster.CompanyName = AdjusterCo;
										adjuster = AdjusterManager.Save(adjuster);
									}

									adjusterID = adjuster.AdjusterId;
								}

								if (table.Rows[i]["Date of Loss"] != string.Empty) {
									DateTime lossDate = DateTime.MinValue;
									if (DateTime.TryParse(table.Rows[i]["Date of Loss"].ToString(), out lossDate))
										objLead.LossDate = lossDate;
								}

								if (table.Rows[i]["Loss Location"] != string.Empty)
									objLead.LossLocation = table.Rows[i]["Loss Location"].ToString();


								if (table.Rows[i]["Address 1"] != string.Empty)
									objLead.LossAddress = table.Rows[i]["Address 1"].ToString();

								if (table.Rows[i]["Address 2"] != string.Empty)
									objLead.LossAddress2 = table.Rows[i]["Address 2"].ToString();


								if (table.Rows[i]["City"] != string.Empty)
									objLead.CityName = table.Rows[i]["City"].ToString();

								if (table.Rows[i]["State"] != string.Empty)
									objLead.StateName = table.Rows[i]["State"].ToString();

								if (table.Rows[i]["ZIP Code"] != string.Empty)
									objLead.Zip = table.Rows[i]["ZIP Code"].ToString();

								if (table.Rows[i]["Company"] != string.Empty)
									objLead.BusinessName = table.Rows[i]["Company"].ToString();

								if (table.Rows[i]["Adjuster Name"] != string.Empty)
									adjusterName = table.Rows[i]["Adjuster Name"].ToString();

								if (table.Rows[i]["Phone"] != string.Empty) {
									str = table.Rows[i]["Phone"].ToString();
									if (str.Length > 20)
										objLead.PhoneNumber = str.Substring(0, 20);
									else
										objLead.PhoneNumber = str.ToString();
								}

								StatusMaster statusMaster = null;
								string statusName = null;

								if (table.Rows[i]["File Status"] != string.Empty) {
									statusName = table.Rows[i]["File Status"].ToString();
									statusMaster = StatusManager.GetByStatusName(statusName);

									if (statusMaster.StatusId == 0) {
										statusMaster = new StatusMaster();
										statusMaster.clientID = clientID;
										statusMaster.InsertBy = objLead.UserId;
										statusMaster.InsertDate = DateTime.Now;
										statusMaster.isCountable = true;
										statusMaster.Status = true;
										statusMaster.StatusName = statusName;

										statusMaster = StatusManager.Save(statusMaster);
									}
								}


								if (!string.IsNullOrEmpty(table.Rows[i]["Peril"].ToString())) {
									var id = TypeofDamageManager.getbyTypeOfDamage(table.Rows[i]["Peril"].ToString());
									objLead.TypeOfDamage = id.TypeOfDamage;

									//string dmgid = string.Empty;
									//string[] dmg = table.Rows[i]["Type of Damage"].ToString().Split(',');
									//for (int d = 0; d < dmg.Length; d++) {
									//     string dmgtext = dmg[d];
									//     var dmgdata = TypeofDamageManager.getbyTypeOfDamage(dmgtext);
									//     if (dmgdata != null && dmgdata.TypeOfDamage != null && dmgdata.TypeOfDamage.ToString() != string.Empty) {
									//          dmgid += dmgdata.TypeOfDamageId + ",";
									//     }
									//     else {
									//          TypeOfDamageMaster objdmg = new TypeOfDamageMaster();
									//          objdmg.TypeOfDamage = dmgtext.Length > 100 ? dmgtext.Substring(0, 100) : dmgtext;
									//          objdmg.Status = true;
									//          TypeOfDamageMaster sv = TypeofDamageManager.Save(objdmg);
									//          dmgid += sv.TypeOfDamageId + ",";
									//     }
									//}

									//objLead.TypeOfDamage = dmgid;

									str = table.Rows[i]["Peril"].ToString();
									if (str.Length > 250)
										objLead.TypeofDamageText = str.Substring(0, 250).Replace("/", ",");
									else
										objLead.TypeofDamageText = str.Replace("/", ",");
								}

								objLead.Status = 1;

								// 2013-08-29 tortega
								if (clientID > 0)
									objLead.ClientID = clientID;

								Leads newLead = LeadsManager.Save(objLead);

								if (newLead != null) {
									if (table.Rows[i]["Certified Mail Number"] != string.Empty)
										AddComments(newLead.LeadId, "Certified Mail Number: " + table.Rows[i]["Certified Mail Number"].ToString());


									//if (table.Rows[i]["Contract Signed Date"] != string.Empty)
									//     AddComments(newLead.LeadId, "Contract Signed Date: " + table.Rows[i]["Contract Signed Date"].ToString());

									if (table.Rows[i]["Last E-mail"] != string.Empty)
										AddComments(newLead.LeadId, "Last E-mail Date: " + table.Rows[i]["Last E-mail"].ToString());

									if (table.Rows[i]["Last Meeting"] != string.Empty)
										AddComments(newLead.LeadId, "Last Meeting Date: " + table.Rows[i]["Last Meeting"].ToString());

									if (table.Rows[i]["Letter Date"] != string.Empty)
										AddComments(newLead.LeadId, "Letter Date: " + table.Rows[i]["Letter Date"].ToString());

									// add policy
									CRM.Data.Entities.LeadPolicy policy = new CRM.Data.Entities.LeadPolicy();
									policy.PolicyType = 1;
									policy.LeadId = newLead.LeadId;
									policy.IsActive = true;
									policy.isAllDocumentUploaded = false;

									if (adjusterID != null)
										policy.AdjusterID = adjusterID;

									if (table.Rows[i]["Claim Number"] != string.Empty)
										policy.ClaimNumber = table.Rows[i]["Claim Number"].ToString();

									if (table.Rows[i]["Policy Number"] != string.Empty)
										policy.PolicyNumber = table.Rows[i]["Policy Number"].ToString();

									if (table.Rows[i]["Policy Period"] != string.Empty)
										policy.PolicyPeriod = table.Rows[i]["Policy Period"].ToString();

									if (table.Rows[i]["File Number"] != string.Empty)
										policy.InsurerFileNo = table.Rows[i]["File Number"].ToString();

									if (statusMaster != null)
										policy.LeadStatus = statusMaster.StatusId;

									if (table.Rows[i]["Ins Carrier"] != string.Empty)
										policy.InsuranceCompanyName = table.Rows[i]["Ins Carrier"].ToString();


									if (table.Rows[i]["Ins Co Address"] != string.Empty)
										policy.InsuranceAddress = table.Rows[i]["Ins Co Address"].ToString();

									if (table.Rows[i]["Ins Co City"] != string.Empty) {
										string cityName = table.Rows[i]["Ins Co City"].ToString();
										CityMaster cityMaster = City.GetByCityName(cityName);
										if (cityMaster != null && cityMaster.CityId > 0)
											policy.InsuranceCity = cityMaster.CityId;
									}

									if (table.Rows[i]["Ins Co State"] != string.Empty) {
										string stateName = table.Rows[i]["Ins Co State"].ToString();
										StateMaster stateMaster = State.Getstateid(stateName);
										if (stateMaster != null && stateMaster.StateId > 0)
											policy.InsuranceState = stateMaster.StateId;
									}

									if (table.Rows[i]["Ins Co Zip"] != string.Empty) {
										string zipCode = table.Rows[i]["Ins Co Zip"].ToString();
										ZipCodeMaster zipMaster = ZipCode.GetByZipCode(zipCode);
										if (zipMaster != null && zipMaster.ZipCodeID > 0)
											policy.InsuranceZipCode = zipMaster.ZipCodeID.ToString();
									}

									LeadPolicyManager.Save(policy);
								}

							}	//for (int i = 0; i < table.Rows.Count; i++) {

							scope.Complete();

						}
						string rootFolderPath = Server.MapPath("~//CSVLoad//");
						string filesToDelete = FileUpload1.FileName;
						string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
						foreach (string file in fileList) {
							System.IO.File.Delete(file);
						}
						lblSave.Text = "Data Saved Successfully !!!";
						lblSave.Visible = true;
					}
				}
			}
			catch (Exception ex) {
				lblError.Text = "There Is a problem in  data save  !!!";
				lblError.Visible = true;

				Core.EmailHelper.emailError(ex);
			}

		}

		protected bool columnExists(DataTable datatable, string columnName) {
			bool exists = false;

			exists = datatable.Columns.Cast<DataColumn>()
							.Any(c => c.ColumnName == columnName);


			return exists;
		}

		protected void UploadTemplate2() {
			// original format designed by Vivek
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			// 2013-02-09 tortega -- Added data validation prior to import. File is not aborted in the event of incorrect data.
			string str = null;
			int fileLineNumber = 0;
			int clientID = Core.SessionHelper.getClientId();

			// assume homeowner
			int policyType = 1;
			DateTime DateSubmitted = DateTime.Now;
			DateTime ContractDate = DateTime.Now;

			string columnValue = null;
			Mortgagee mortgagee = null;

			StatusMaster statusMaster = null;
			string statusName = null;

			try {
				if (FileUpload1.HasFile) {

					string ext = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
					string ActualFileName = FileUpload1.PostedFile.FileName.Substring(FileUpload1.PostedFile.FileName.LastIndexOf(@"\") + 1);
					string FileNameWithoutExt = ActualFileName.Replace(ext, "");
					if (ext == ".csv") {

						if (!Directory.Exists(Server.MapPath("~//CSVLoad//"))) {
							Directory.CreateDirectory(Server.MapPath("~//CSVLoad//"));
						}
						FileUpload1.SaveAs(Server.MapPath("~//CSVLoad//" + FileUpload1.FileName));
						DataTable table = CSVReader.ReadCSVFile(Server.MapPath("~//CSVLoad//" + FileUpload1.FileName), true);
						using (TransactionScope scope = new TransactionScope()) {
							Leads objLead = null;



							for (int i = 0; i < table.Rows.Count; i++) {
								fileLineNumber = i;

								objLead = new Leads();

								//objLead.LeadStatus = 1;	// active
								objLead.OriginalLeadDate = DateTime.Now;
								objLead.TypeOfProperty = 18;	//residential
								objLead.UserId = Convert.ToInt32(Session["UserId"]);

								try {

									if (columnExists(table, "Create Date") && table.Rows[i]["Create Date"] != string.Empty) {

										DateTime.TryParse(table.Rows[i]["Create Date"].ToString(), out DateSubmitted);

										objLead.DateSubmitted = DateSubmitted;

										objLead.OriginalLeadDate = DateSubmitted;
									}


									if (columnExists(table, "Last Name") && table.Rows[i]["Last Name"] != string.Empty) {
										str = table.Rows[i]["Last Name"].ToString();
										if (str.Length > 50) {
											objLead.ClaimantLastName = str.Substring(0, 50);
											objLead.OwnerLastName = objLead.ClaimantLastName;
										}
										else {
											objLead.ClaimantLastName = str;
											objLead.OwnerLastName = str;
										}
									}

									if (columnExists(table, "First Name") && table.Rows[i]["First Name"] != string.Empty) {
										str = table.Rows[i]["First Name"].ToString();
										if (str.Length > 50) {
											objLead.ClaimantFirstName = str.Substring(0, 50);
											objLead.OwnerFirstName = objLead.ClaimantFirstName;
										}
										else {
											objLead.ClaimantFirstName = str;
											objLead.OwnerFirstName = str;
										}
									}

									if (columnExists(table, "Middle Name") && table.Rows[i]["Middle Name"] != string.Empty) {
										str = table.Rows[i]["Middle Name"].ToString();
										if (str.Length > 50) {
											objLead.ClaimantMiddleName = str.Substring(0, 50);
										}
										else {
											objLead.ClaimantMiddleName = str;
										}
									}


									if (columnExists(table, "E-mail") && table.Rows[i]["E-mail"] != string.Empty) {
										str = table.Rows[i]["E-mail"].ToString();
										if (str.Length > 100)
											objLead.EmailAddress = str.Substring(0, 100);
										else
											objLead.EmailAddress = str;
									}
									if (columnExists(table, "Personal E-mail") && table.Rows[i]["Personal E-mail"] != string.Empty) {
										str = table.Rows[i]["Personal E-mail"].ToString();
										if (str.Length > 100)
											objLead.SecondaryEmail = str.Substring(0, 100);
										else
											objLead.SecondaryEmail = str;
									}

									if (columnExists(table, "Mobile Phone") && table.Rows[i]["Mobile Phone"] != string.Empty) {
										str = table.Rows[i]["Mobile Phone"].ToString();
										if (str.Length > 20)
											objLead.SecondaryPhone = str.Substring(0, 20);
										else
											objLead.SecondaryPhone = str;
									}

									if (columnExists(table, "Salutation") && table.Rows[i]["Salutation"] != string.Empty) {
										str = table.Rows[i]["Salutation"].ToString();
										if (str.Length > 50)
											objLead.Salutation = str.Substring(0, 20);
										else
											objLead.Salutation = str;
									}
								}
								catch (Exception ex) {
									lblError.Text = "There Is a problem in  data save  !!!";
									lblError.Visible = true;

									Core.EmailHelper.emailError(ex);

									throw new Exception(ex.Message);
								}

								string adjusterName = null;
								int? adjusterID = null;
								AdjusterMaster adjuster = null;

								if (columnExists(table, "Adjuster Name") && table.Rows[i]["Adjuster Name"] != string.Empty)
									adjusterName = table.Rows[i]["Adjuster Name"].ToString();

								string adjusterEmail = null;
								string adjusterFax = null;
								string AdjusterPhone = null;
								string AdjusterCo = null;
								string AdjusterAddress = null;

								if (columnExists(table, "Adjuster Email") && table.Rows[i]["Adjuster Email"] != string.Empty)
									adjusterEmail = table.Rows[i]["Adjuster Email"].ToString();

								if (columnExists(table, "Adjuster Fax") && table.Rows[i]["Adjuster Fax"] != string.Empty)
									adjusterFax = table.Rows[i]["Adjuster Fax"].ToString();

								if (columnExists(table, "Adjuster Phone") && table.Rows[i]["Adjuster Phone"] != string.Empty)
									AdjusterPhone = table.Rows[i]["Adjuster Phone"].ToString();

								if (columnExists(table, "Adjuster Co") && table.Rows[i]["Adjuster Co"] != string.Empty)
									AdjusterCo = table.Rows[i]["Adjuster Co"].ToString();

								if (columnExists(table, "Adj Address") && table.Rows[i]["Adj Address"] != string.Empty)
									AdjusterAddress = table.Rows[i]["Adj Address"].ToString();

								if (!string.IsNullOrEmpty(adjusterName)) {
									adjuster = AdjusterManager.GetByAdjusterName(adjusterName.Trim());
									if (adjuster.AdjusterId == 0) {
										// add adjuster
										adjuster = new AdjusterMaster();
										adjuster.Status = true;
										adjuster.AdjusterName = adjusterName.Trim();
										adjuster.ClientId = clientID;
										adjuster.InsertBy = objLead.UserId;
										adjuster.InsertDate = DateTime.Now;
										adjuster.isEmailNotification = true;
										adjuster.email = adjusterEmail;
										adjuster.Address1 = AdjusterAddress;
										adjuster.PhoneNumber = AdjusterPhone;
										adjuster.FaxNumber = adjusterFax;
										adjuster.CompanyName = AdjusterCo;
										adjuster = AdjusterManager.Save(adjuster);
									}

									adjusterID = adjuster.AdjusterId;
								}

								if (columnExists(table, "ExaminerName") && table.Rows[i]["ExaminerName"] != string.Empty) {
									InspectorMaster inspector = InspectorManager.GetByName(table.Rows[i]["ExaminerName"].ToString());
									if (inspector.InspectorId > 0) {
										objLead.InspectorName = inspector.InspectorId;
									}

								}

								//if (columnExists(table, "Loss Location") && table.Rows[i]["Loss Location"] != string.Empty)
								//     objLead.LossLocation = table.Rows[i]["Loss Location"].ToString();

								//if (columnExists(table, "Date of Loss") && table.Rows[i]["Date of Loss"] != string.Empty) {
								//     DateTime lossDate = DateTime.MinValue;
								//     if (DateTime.TryParse(table.Rows[i]["Date of Loss"].ToString(), out lossDate))
								//          objLead.LossDate = lossDate;
								//}

								if (columnExists(table, "Contract Signed Date") && table.Rows[i]["Contract Signed Date"] != string.Empty)
									objLead.ContractDate = Convert.ToDateTime(table.Rows[i]["Contract Signed Date"].ToString());

								if (columnExists(table, "Address 1") && table.Rows[i]["Address 1"] != string.Empty) {
									str = table.Rows[i]["Address 1"].ToString();

									if (str.Length > 100)
										objLead.MailingAddress = str.Substring(0, 99);
									else
										objLead.LossAddress = str.ToString();
								}


								try {

									if (columnExists(table, "City") && table.Rows[i]["City"] != string.Empty)
										objLead.MailingCity = table.Rows[i]["City"].ToString();

									if (columnExists(table, "State") && table.Rows[i]["State"] != string.Empty)
										objLead.MailingState = table.Rows[i]["State"].ToString();

									if (columnExists(table, "ZIP Code") && table.Rows[i]["ZIP Code"] != string.Empty)
										objLead.MailingZip = table.Rows[i]["ZIP Code"].ToString();

									if (columnExists(table, "Business Name") && table.Rows[i]["Business Name"] != string.Empty)
										objLead.BusinessName = table.Rows[i]["Business Name"].ToString();

									if (columnExists(table, "Adjuster Name") && table.Rows[i]["Adjuster Name"] != string.Empty)
										adjusterName = table.Rows[i]["Adjuster Name"].ToString();

									if (columnExists(table, "Phone") && table.Rows[i]["Phone"] != string.Empty) {
										str = table.Rows[i]["Phone"].ToString();
										if (str.Length > 20)
											objLead.PhoneNumber = str.Substring(0, 20);
										else
											objLead.PhoneNumber = str.ToString();
									}

									if (columnExists(table, "Loss Address") && table.Rows[i]["Loss Address"] != string.Empty) {
										str = table.Rows[i]["Loss Address"].ToString();

										if (str.Length > 100)
											objLead.LossAddress = str.Substring(0, 99);
										else
											objLead.LossAddress = str.ToString();
									}
									if (columnExists(table, "Loss City") && table.Rows[i]["Loss City"] != string.Empty)
										objLead.CityName = table.Rows[i]["Loss City"].ToString();

									if (columnExists(table, "Loss State") && table.Rows[i]["Loss State"] != string.Empty)
										objLead.StateName = table.Rows[i]["Loss State"].ToString();

									if (columnExists(table, "Loss Zip Code") && table.Rows[i]["Loss Zip Code"] != string.Empty) {
										str = table.Rows[i]["Loss Zip Code"].ToString();
										if (str.Length < 5)
											str = "0" + str;

										objLead.Zip = str;
									}

									if (columnExists(table, "Fax") && table.Rows[i]["Fax"] != string.Empty) {
										str = table.Rows[i]["Fax"].ToString();
										if (str.Length > 20)
											objLead.SecondaryPhone = str.Substring(0, 20);
										else
											objLead.SecondaryPhone = str.ToString();
									}




									if (columnExists(table, "File Status") && table.Rows[i]["File Status"] != string.Empty) {
										statusName = table.Rows[i]["File Status"].ToString();
										statusMaster = StatusManager.GetByStatusName(statusName);

										if (statusMaster.StatusId == 0) {
											statusMaster = new StatusMaster();
											statusMaster.clientID = clientID;
											statusMaster.InsertBy = objLead.UserId;
											statusMaster.InsertDate = DateTime.Now;
											statusMaster.isCountable = true;
											statusMaster.Status = true;
											statusMaster.StatusName = statusName;

											statusMaster = StatusManager.Save(statusMaster);
										}
									}
								}
								catch (Exception ex) {
									lblError.Text = "There Is a problem in  data save  !!!";
									lblError.Visible = true;

									Core.EmailHelper.emailError(ex);

									throw new Exception(ex.Message);
								}

								if (columnExists(table, "Peril") && !string.IsNullOrEmpty(table.Rows[i]["Peril"].ToString())) {
									var id = TypeofDamageManager.getbyTypeOfDamage(table.Rows[i]["Peril"].ToString());
									if (id.TypeOfDamageId > 0)
										objLead.TypeOfDamage = id.TypeOfDamageId.ToString() + ",";

									//string dmgid = string.Empty;
									//string[] dmg = table.Rows[i]["Type of Damage"].ToString().Split(',');
									//for (int d = 0; d < dmg.Length; d++) {
									//     string dmgtext = dmg[d];
									//     var dmgdata = TypeofDamageManager.getbyTypeOfDamage(dmgtext);
									//     if (dmgdata != null && dmgdata.TypeOfDamage != null && dmgdata.TypeOfDamage.ToString() != string.Empty) {
									//          dmgid += dmgdata.TypeOfDamageId + ",";
									//     }
									//     else {
									//          TypeOfDamageMaster objdmg = new TypeOfDamageMaster();
									//          objdmg.TypeOfDamage = dmgtext.Length > 100 ? dmgtext.Substring(0, 100) : dmgtext;
									//          objdmg.Status = true;
									//          TypeOfDamageMaster sv = TypeofDamageManager.Save(objdmg);
									//          dmgid += sv.TypeOfDamageId + ",";
									//     }
									//}

									//objLead.TypeOfDamage = dmgid;

									str = table.Rows[i]["Peril"].ToString();
									if (str.Length > 250)
										objLead.TypeofDamageText = str.Substring(0, 250).Replace("/", ",");
									else
										objLead.TypeofDamageText = str.Replace("/", ",");
								}

								objLead.Status = 1;

								// 2013-08-29 tortega
								if (clientID > 0)
									objLead.ClientID = clientID;

								Leads newLead = LeadsManager.Save(objLead);
								int policyTypeID = 1;

								if (newLead != null) {
									// add policy
									CRM.Data.Entities.LeadPolicy policy = new CRM.Data.Entities.LeadPolicy();

									if (columnExists(table, "PolicyType") && table.Rows[i]["PolicyType"] != string.Empty) {
										policyTypeID = getPolicyType(table.Rows[i]["PolicyType"].ToString());

									}

									policy.PolicyType = policyTypeID;

									policy.LeadId = newLead.LeadId;
									policy.IsActive = true;
									policy.isAllDocumentUploaded = false;

									if (adjusterID != null)
										policy.AdjusterID = adjusterID;


									if (columnExists(table, "LoanID") && table.Rows[i]["LoanID"] != string.Empty) {
										str = table.Rows[i]["LoanID"].ToString();

										if (str.Length > 50)
											policy.LoanNumber = str.Substring(0, 50);
										else
											policy.LoanNumber = str;
									}

									if (columnExists(table, "DateOfEvaluation") && table.Rows[i]["DateOfEvaluation"] != string.Empty) {
										DateTime SiteSurveyDate = DateTime.MinValue;
										if (DateTime.TryParse(table.Rows[i]["DateOfEvaluation"].ToString(), out SiteSurveyDate))
											policy.SiteSurveyDate = SiteSurveyDate;
									}

									if (columnExists(table, "Mortgagee") && table.Rows[i]["Mortgagee"] != string.Empty) {
										int id = getMortgageeID(table.Rows[i]["Mortgagee"].ToString(), clientID);
										if (id > 0)
											policy.MortgageeID = id;
										else {
											mortgagee = new Mortgagee();
											mortgagee.ClientID = clientID;
											mortgagee.Status = true;
											mortgagee.MortageeName = table.Rows[i]["Mortgagee"].ToString();

											mortgagee = MortgageeManager.Save(mortgagee);
											if (mortgagee.MortgageeID > 0)
												policy.MortgageeID = mortgagee.MortgageeID;
										}
									}

									if (columnExists(table, "Loss Location") && table.Rows[i]["Loss Location"] != string.Empty)
										policy.LossLocation = table.Rows[i]["Loss Location"].ToString();

									if (columnExists(table, "Date of Loss") && table.Rows[i]["Date of Loss"] != string.Empty) {
										DateTime lossDate = DateTime.MinValue;
										if (DateTime.TryParse(table.Rows[i]["Date of Loss"].ToString(), out lossDate))
											policy.LossDate = lossDate;
									}

									if (columnExists(table, "Policy Period") && table.Rows[i]["Policy Period"] != string.Empty)
										policy.PolicyPeriod = table.Rows[i]["Policy Period"].ToString();


									string PolicyExpirationDate = null;
									if (columnExists(table, "PolicyExpirationDate") && table.Rows[i]["PolicyExpirationDate"] != string.Empty)
										PolicyExpirationDate = table.Rows[i]["PolicyExpirationDate"].ToString();

									string effectiveDate = null;
									if (columnExists(table, "PolicyEffectiveDate") && table.Rows[i]["PolicyEffectiveDate"] != string.Empty)
										effectiveDate = table.Rows[i]["PolicyEffectiveDate"].ToString();

									str = string.Format("{0} - {1}", effectiveDate ?? "N/A", PolicyExpirationDate ?? "N/A");
									if (str.Length > 50)
										policy.PolicyPeriod = str.Substring(0, 50);
									else
										policy.PolicyPeriod = str;

									if (columnExists(table, "Claim Number") && table.Rows[i]["Claim Number"] != string.Empty) {
										str = table.Rows[i]["Claim Number"].ToString();
										if (str.Length > 50)
											policy.ClaimNumber = str.Substring(0, 50);
										else
											policy.ClaimNumber = str;
									}

									if (columnExists(table, "Policy Number") && table.Rows[i]["Policy Number"] != string.Empty) {
										str = table.Rows[i]["Policy Number"].ToString();
										if (str.Length > 100)
											policy.PolicyNumber = str.Substring(0, 100);
										else
											policy.PolicyNumber = str;
									}


									if (columnExists(table, "File Number") && table.Rows[i]["File Number"] != string.Empty) {
										str = table.Rows[i]["File Number"].ToString();
										if (str.Length > 20)
											policy.InsurerFileNo = str.Substring(0, 20);
										else
											policy.InsurerFileNo = str;
									}

									if (statusMaster != null)
										policy.LeadStatus = statusMaster.StatusId;

									if (columnExists(table, "Ins Carrier") && table.Rows[i]["Ins Carrier"] != string.Empty) {
										str = table.Rows[i]["Ins Carrier"].ToString();
										if (str.Length > 50)
											policy.InsuranceCompanyName = str.Substring(0, 50);
										else
											policy.InsuranceCompanyName = str;
									}

									if (columnExists(table, "Ins Co Address") && table.Rows[i]["Ins Co Address"] != string.Empty) {
										str = table.Rows[i]["Ins Co Address"].ToString();
										if (str.Length > 50)
											policy.InsuranceAddress = str.Substring(0, 50);
										else
											policy.InsuranceAddress = str;


									}

									if (columnExists(table, "Ins Co City") && table.Rows[i]["Ins Co City"] != string.Empty) {
										string cityName = table.Rows[i]["Ins Co City"].ToString();
										CityMaster cityMaster = City.GetByCityName(cityName);
										if (cityMaster != null && cityMaster.CityId > 0)
											policy.InsuranceCity = cityMaster.CityId;
									}

									if (columnExists(table, "Ins Co State") && table.Rows[i]["Ins Co State"] != string.Empty) {
										string stateName = table.Rows[i]["Ins Co State"].ToString();
										StateMaster stateMaster = State.Getstateid(stateName);
										if (stateMaster != null && stateMaster.StateId > 0)
											policy.InsuranceState = stateMaster.StateId;
									}

									if (columnExists(table, "Ins Co Zip") && table.Rows[i]["Ins Co Zip"] != string.Empty) {
										string zipCode = table.Rows[i]["Ins Co Zip"].ToString();
										ZipCodeMaster zipMaster = ZipCode.GetByZipCode(zipCode);
										if (zipMaster != null && zipMaster.ZipCodeID > 0)
											policy.InsuranceZipCode = zipMaster.ZipCodeID.ToString();
									}

									LeadPolicyManager.Save(policy);

									// add policy comments
									if (columnExists(table, "Notes") && table.Rows[i]["Notes"] != string.Empty) {
										char[] splitTokens = { '\n' };

										string[] lines = table.Rows[i]["Notes"].ToString().Split(splitTokens, StringSplitOptions.RemoveEmptyEntries);
										if (lines != null && lines.Length > 0)
											foreach (string line in lines) {

												string[] datetimeValues = line.Split(new char[] { ' ' });
												if (datetimeValues.Length > 3) {
													string datetime = string.Format("{0} {1} {2}", datetimeValues[0] ?? "", datetimeValues[1] ?? "", datetimeValues[2] ?? "");
													DateTime commentInsertDate = DateTime.Now;
													if (DateTime.TryParse(datetime, out commentInsertDate))
														AddComments(newLead.LeadId, policyTypeID, line, commentInsertDate);
													else
														AddComments(newLead.LeadId, policyTypeID, line);
													
												}
												else
													AddComments(newLead.LeadId, policyTypeID, line);
											}
										//AddComments(newLead.LeadId, policyTypeID, table.Rows[i]["Notes"].ToString().Replace("\n","<br/>");
									}

									if (columnExists(table, "Certified Mail Number") && table.Rows[i]["Certified Mail Number"] != string.Empty)
										AddComments(newLead.LeadId, policyTypeID, "Certified Mail Number: " + table.Rows[i]["Certified Mail Number"].ToString());


									//if (columnExists(table, "Contract Signed Date") && table.Rows[i]["Contract Signed Date"] != string.Empty)
									//     AddComments(newLead.LeadId, policyTypeID, "Contract Signed Date: " + table.Rows[i]["Contract Signed Date"].ToString());

									if (columnExists(table, "Last E-mail") && table.Rows[i]["Last E-mail"] != string.Empty)
										AddComments(newLead.LeadId, policyTypeID, "Last E-mail Date: " + table.Rows[i]["Last E-mail"].ToString());

									if (columnExists(table, "Last Meeting") && table.Rows[i]["Last Meeting"] != string.Empty)
										AddComments(newLead.LeadId, policyTypeID, "Last Meeting Date: " + table.Rows[i]["Last Meeting"].ToString());

									if (columnExists(table, "Letter Date") && table.Rows[i]["Letter Date"] != string.Empty)
										AddComments(newLead.LeadId, policyTypeID, "Letter Date: " + table.Rows[i]["Letter Date"].ToString());
								}

							}	//for (int i = 0; i < table.Rows.Count; i++) {

							scope.Complete();

						}
						string rootFolderPath = Server.MapPath("~//CSVLoad//");
						string filesToDelete = FileUpload1.FileName;
						string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
						foreach (string file in fileList) {
							System.IO.File.Delete(file);
						}
						lblSave.Text = "Data Saved Successfully !!!";
						lblSave.Visible = true;
					}
				}
			}
			catch (Exception ex) {
				lblError.Text = "There Is a problem in  data save  !!!";
				lblError.Visible = true;				
			}

		}

		protected int getMortgageeID(string mortgageeName, int clientID) {
			int id = 0;

			Mortgagee mortgagee = MortgageeManager.Get(mortgageeName, clientID);

			id = mortgagee == null ? 0 : mortgagee.MortgageeID;

			return id;
		}

		protected int getPolicyType(string policyType) {
			int policyTypeID = 0;
			switch (policyType) {
				case "Commercial Property":
					policyTypeID = 2;
					break;

				case "Homeowner":
					policyTypeID = 1;
					break;

				default:
					policyTypeID = 1;
					break;
			}
			return policyTypeID;
		}

		protected InspectorMaster AddInspector(CRM.Data.Entities.Leads lead, string inspectorName) {
			InspectorMaster inspector = new InspectorMaster();
			inspector.ClientID = lead.ClientID;
			inspector.InsertBy = lead.UserId;
			inspector.InsertDate = DateTime.Now;
			inspector.InspectorName = inspectorName;
			inspector.Status = true;

			inspector = InspectorManager.Save(inspector);

			return inspector;
		}

		protected ProducerMaster AddProducer(Leads lead, string producerName) {
			ProducerMaster producer = new ProducerMaster();
			producer.ClientId = lead.ClientID;
			producer.InsertBy = lead.UserId;
			producer.InsertDate = DateTime.Now;
			producer.ProducerName = producerName;
			producer.Status = 1;

			producer = ProducerManager.Save(producer);

			return producer;
		}

		protected void AddComments(int leadID, string commentText) {
			LeadComment objLeadComment = new LeadComment();

			objLeadComment.LeadId = leadID;

			objLeadComment.UserId = Convert.ToInt32(Session["UserId"]);

			objLeadComment.CommentText = commentText;

			objLeadComment.Status = 1;

			objLeadComment.PolicyType = 1;

			LeadComment objld = LeadCommentManager.Save(objLeadComment);
		}
		protected void AddComments(int leadID, int policyType, string commentText) {
			LeadComment objLeadComment = new LeadComment();

			objLeadComment.LeadId = leadID;

			objLeadComment.UserId = Convert.ToInt32(Session["UserId"]);

			objLeadComment.CommentText = commentText;

			objLeadComment.Status = 1;

			objLeadComment.PolicyType = policyType;



			LeadComment objld = LeadCommentManager.Save(objLeadComment);
		}
		protected void AddComments(int leadID, int policyType, string commentText, DateTime insertDate) {
			LeadComment objLeadComment = new LeadComment();

			objLeadComment.LeadId = leadID;

			objLeadComment.UserId = Convert.ToInt32(Session["UserId"]);

			objLeadComment.CommentText = commentText;

			objLeadComment.Status = 1;

			objLeadComment.PolicyType = policyType;

			objLeadComment.InsertDate = insertDate;

			LeadComment objld = LeadCommentManager.Save(objLeadComment);
		}

		protected void UploadTemplate1() {
			// original format designed by Vivek
			int adjusterID = 0;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			// 2013-02-09 tortega -- Added data validation prior to import. File is not aborted in the event of incorrect data.
			string str = null;

			int clientID = Core.SessionHelper.getClientId();

			// assume homeowner
			int policyType = 1;
			DateTime DateSubmitted = DateTime.Now;

			try {
				if (FileUpload1.HasFile) {

					string ext = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
					string ActualFileName = FileUpload1.PostedFile.FileName.Substring(FileUpload1.PostedFile.FileName.LastIndexOf(@"\") + 1);
					string FileNameWithoutExt = ActualFileName.Replace(ext, "");
					if (ext == ".csv") {

						if (!Directory.Exists(Server.MapPath("~//CSVLoad//"))) {
							Directory.CreateDirectory(Server.MapPath("~//CSVLoad//"));
						}
						FileUpload1.SaveAs(Server.MapPath("~//CSVLoad//" + FileUpload1.FileName));
						DataTable table = CSVReader.ReadCSVFile(Server.MapPath("~//CSVLoad//" + FileUpload1.FileName), true);
						using (TransactionScope scope = new TransactionScope()) {
							Leads objLead = null;
							for (int i = 0; i < table.Rows.Count; i++) {

								objLead = new Leads();


								objLead.OriginalLeadDate = DateTime.Now;
								objLead.UserId = Convert.ToInt32(Session["UserId"]);

								if (table.Rows[i]["DateSubmitted"] != string.Empty) {

									DateTime.TryParse(table.Rows[i]["DateSubmitted"].ToString(), out DateSubmitted);

									objLead.DateSubmitted = DateSubmitted;
								}


								str = table.Rows[i]["LFUUID"].ToString();
								if (str.Length > 50)
									objLead.LFUUID = table.Rows[i]["LFUUID"].ToString().Substring(0, 50);
								else
									objLead.LFUUID = table.Rows[i]["LFUUID"].ToString();

								if (table.Rows[i]["Original Lead Date"] != string.Empty) {
									objLead.OriginalLeadDate = Convert.ToDateTime(table.Rows[i]["Original Lead Date"]);
								}


								if (table.Rows[i]["Last Name"] != string.Empty) {
									str = table.Rows[i]["Last Name"].ToString();
									if (str.Length > 50)
										objLead.ClaimantLastName = table.Rows[i]["Last Name"].ToString().Substring(0, 50);
									else
										objLead.ClaimantLastName = table.Rows[i]["Last Name"].ToString();
								}

								if (table.Rows[i]["First Name"] != string.Empty) {
									str = table.Rows[i]["First Name"].ToString();
									if (str.Length > 50)
										objLead.ClaimantFirstName = table.Rows[i]["First Name"].ToString().Substring(0, 50);
									else
										objLead.ClaimantFirstName = table.Rows[i]["First Name"].ToString();
								}

								if (table.Rows[i]["Email Address"] != string.Empty) {
									str = table.Rows[i]["Email Address"].ToString();
									if (str.Length > 100)
										objLead.EmailAddress = str.Substring(0, 100);
									else
										objLead.EmailAddress = str;
								}
								if (table.Rows[i]["Secondary Email"] != string.Empty) {
									str = table.Rows[i]["Secondary Email"].ToString();
									if (str.Length > 100)
										objLead.SecondaryEmail = str.Substring(0, 100);
									else
										objLead.SecondaryEmail = str;
								}

								if (table.Rows[i]["Adjuster"] != string.Empty) {
									string adjusterName = table.Rows[i]["Adjuster"].ToString();
									AdjusterMaster adjuster = null;


									if (!string.IsNullOrEmpty(adjusterName)) {
										adjuster = AdjusterManager.GetByAdjusterName(adjusterName.Trim());
										if (adjuster.AdjusterId == 0) {
											// add adjuster
											adjuster = new AdjusterMaster();
											adjuster.Status = true;
											adjuster.AdjusterName = adjusterName.Trim();
											adjuster.ClientId = clientID;
											adjuster.InsertBy = objLead.UserId;
											adjuster.InsertDate = DateTime.Now;
											adjuster.isEmailNotification = true;

											adjuster = AdjusterManager.Save(adjuster);
										}

										adjusterID = adjuster.AdjusterId;
									}


								}
								if (table.Rows[i]["Lead Source"] != string.Empty) {

									string sourceName = table.Rows[i]["Lead Source"].ToString();

									var id = LeadSourceManager.GetByLeadSourceName(sourceName);
									if (id.LeadSourceId > 0)
										objLead.LeadSource = id.LeadSourceId;
									else {
										// add source										
										LeadSourceMaster leadSource = new LeadSourceMaster();
										if (clientID > 0)
											leadSource.ClientId = clientID;

										leadSource.InsertBy = Core.SessionHelper.getUserId();
										leadSource.InsertDate = DateTime.Now;
										leadSource.LeadSourceName = sourceName.Length > 100 ? sourceName.Substring(0, 100) : sourceName;
										leadSource.Status = true;

										LeadSourceMaster newLeadSource = LeadSourceManager.Save(leadSource);

										objLead.LeadSource = newLeadSource.LeadSourceId;
									}
								}
								if (table.Rows[i]["Primary Producer"] != string.Empty) {
									var id = ProducerManager.GetByProducerName(table.Rows[i]["Primary Producer"].ToString());
									if (id != null && id.ProducerId > 0)
										objLead.PrimaryProducerId = id.ProducerId;
									else {
										ProducerMaster producer = AddProducer(objLead, table.Rows[i]["Primary Producer"].ToString());
										objLead.PrimaryProducerId = producer.ProducerId;
									}
								}
								if (table.Rows[i]["Secondary Producer"] != string.Empty) {
									var id = SecondaryProducerManager.GetBySecondaryProducerName(table.Rows[i]["Secondary Producer"].ToString());
									if (id != null && id.SecondaryProduceId > 0)
										objLead.SecondaryProducerId = id.SecondaryProduceId;
									else {
										ProducerMaster producer = AddProducer(objLead, table.Rows[i]["Secondary Producer"].ToString());
										objLead.SecondaryProducerId = producer.ProducerId;
									}
								}
								if (table.Rows[i]["Inspector Name"] != string.Empty) {
									var id = InspectorManager.GetByName(table.Rows[i]["Inspector Name"].ToString());
									if (id != null && id.InspectorId > 0)
										objLead.InspectorName = id.InspectorId;
									else {
										InspectorMaster inspector = AddInspector(objLead, table.Rows[i]["Inspector Name"].ToString());
										objLead.InspectorName = inspector.InspectorId;
									}
								}
								if (table.Rows[i]["Inspector Cell"] != string.Empty) {
									str = table.Rows[i]["Inspector Cell"].ToString();
									if (str.Length > 20)
										str = objLead.InspectorCell = table.Rows[i]["Inspector Cell"].ToString().Substring(0, 20);
									else
										objLead.InspectorCell = table.Rows[i]["Inspector Cell"].ToString();
								}
								if (table.Rows[i]["Inspector Email"] != string.Empty) {
									str = table.Rows[i]["Inspector Email"].ToString(); ;
									if (str.Length > 100)
										objLead.InspectorEmail = table.Rows[i]["Inspector Email"].ToString().Substring(0, 100);
									else
										objLead.InspectorEmail = table.Rows[i]["Inspector Email"].ToString();
								}

								if (table.Rows[i]["Phone Number"] != string.Empty) {
									str = table.Rows[i]["Phone Number"].ToString();
									if (str.Length > 20)
										objLead.PhoneNumber = table.Rows[i]["Phone Number"].ToString().Substring(0, 20);
									else
										objLead.PhoneNumber = table.Rows[i]["Phone Number"].ToString();
								}
								if (table.Rows[i]["Secondary Phone"] != string.Empty) {
									str = table.Rows[i]["Secondary Phone"].ToString();
									if (str.Length > 20)
										objLead.SecondaryPhone = str.Substring(0, 20);
									else
										objLead.SecondaryPhone = str;
								}
								//if (table.Rows[i]["Webform Source"] != string.Empty) {
								//	//objLead.WebformSource = table.Rows[i]["Webform Source"].ToString();
								//	var id = WebFormSourceManager.GetByName(table.Rows[i]["Webform Source"].ToString());

								//	if (id != null && id.WebformSourceId > 0)
								//		objLead.WebformSource = id.WebformSourceId;
								//}

								if (!string.IsNullOrEmpty(table.Rows[i]["Type of Damage"].ToString())) {
									//objLead.TypeOfDamage = table.Rows[i]["Type of Damage"].ToString();
									var id = TypeofDamageManager.getbyTypeOfDamage(table.Rows[i]["Type of Damage"].ToString());
									objLead.TypeOfDamage = id.TypeOfDamage;

									//string dmgid = string.Empty;
									//string[] dmg = table.Rows[i]["Type of Damage"].ToString().Split(',');
									//for (int d = 0; d < dmg.Length; d++) {
									//     string dmgtext = dmg[d];
									//     var dmgdata = TypeofDamageManager.getbyTypeOfDamage(dmgtext);
									//     if (dmgdata != null && dmgdata.TypeOfDamage != null && dmgdata.TypeOfDamage.ToString() != string.Empty) {
									//          dmgid += dmgdata.TypeOfDamageId + ",";
									//     }
									//     else {
									//          TypeOfDamageMaster objdmg = new TypeOfDamageMaster();
									//          objdmg.TypeOfDamage = dmgtext.Length > 100 ? dmgtext.Substring(0, 100) : dmgtext;
									//          objdmg.Status = true;
									//          TypeOfDamageMaster sv = TypeofDamageManager.Save(objdmg);
									//          dmgid += sv.TypeOfDamageId + ",";
									//     }
									//}

									//objLead.TypeOfDamage = dmgid;

									str = table.Rows[i]["Type of Damage"].ToString();
									if (str.Length > 250)
										objLead.TypeofDamageText = table.Rows[i]["Type of Damage"].ToString().Substring(0, 250);
									else
										objLead.TypeofDamageText = table.Rows[i]["Type of Damage"].ToString();


									//string Damagetxt = string.Empty;
									//string DamageId = string.Empty;
									//for (int j = 0; j < chkTypeOfDamage.Items.Count; j++)
									//{
									//    if (chkTypeOfDamage.Items[j].Selected == true)
									//    {
									//        Damagetxt += chkTypeOfDamage.Items[j].Text + ',';
									//        DamageId += chkTypeOfDamage.Items[j].Value + ',';
									//    }
									//}
								}
								if (!string.IsNullOrEmpty(table.Rows[i]["Type of Property"].ToString())) {
									var id = PropertyTypeManager.GetByPropertyName(table.Rows[i]["Type of Property"].ToString());

									if (id != null && id.TypeOfPropertyId > 0) {
										objLead.TypeOfProperty = id.TypeOfPropertyId;

										if (id.TypeOfProperty.Contains("Home"))
											policyType = 1;

										if (id.TypeOfProperty.Contains("Commercial"))
											policyType = 2;

										if (id.TypeOfProperty.Contains("Flood"))
											policyType = 3;

										if (id.TypeOfProperty.Contains("Earthquake"))
											policyType = 4;
									}
								}
								if (table.Rows[i]["Market Value"] != string.Empty) {
									decimal MarketValue = 0;
									decimal.TryParse(table.Rows[i]["Market Value"].ToString(), out MarketValue);

									objLead.MarketValue = MarketValue;	//Convert.ToDecimal(table.Rows[i]["Market Value"]);
								}
								if (table.Rows[i]["Loss Address"] != string.Empty) {
									str = table.Rows[i]["Loss Address"].ToString();
									if (str.Length > 100)
										objLead.LossAddress = str.Substring(0, 100);
									else
										objLead.LossAddress = str;
								}

								if (table.Rows[i]["City"] != string.Empty)
									objLead.CityName = table.Rows[i]["City"].ToString();

								if (table.Rows[i]["State"] != string.Empty)
									objLead.StateName = table.Rows[i]["State"].ToString();

								if (table.Rows[i]["Zip"] != string.Empty)
									objLead.Zip = table.Rows[i]["Zip"].ToString();

								if (table.Rows[i]["Property Damage Estimate"] != string.Empty) {
									decimal PropertyDamageEstimate = 0;
									decimal.TryParse(table.Rows[i]["Property Damage Estimate"].ToString(), out PropertyDamageEstimate);

									objLead.PropertyDamageEstimate = PropertyDamageEstimate;// Convert.ToDecimal(table.Rows[i]["Property Damage Estimate"]);
								}
								//if (table.Rows[i]["Habitable"] != string.Empty) {
								//	//objLead.Habitable = table.Rows[i]["Habitable"].ToString();
								//	var id = HabitableManager.GetbyHabitable(table.Rows[i]["Habitable"].ToString());
								//	if (id != null && id.HabitableId > 0)
								//		objLead.Habitable = id.HabitableId;
								//}
								//if (table.Rows[i]["Wind Policy"] != string.Empty) {
								//	//objLead.WindPolicy = table.Rows[i]["Wind Policy"].ToString();
								//	//var id = WindPolicyManager.getbyWindPolicy(table.Rows[i]["Habitable"].ToString());
								//	var id = WindPolicyManager.getbyWindPolicy(table.Rows[i]["Wind Policy"].ToString());
								//	if (id != null && id.WindPolicyId > 0)
								//		objLead.WindPolicy = id.WindPolicyId;
								//}
								//if (table.Rows[i]["Flood Policy"] != string.Empty) {
								//	//objLead.FloodPolicy = table.Rows[i]["Flood Policy"].ToString();
								//	var id = FloodPolicyManager.GetByFloodPolicy(table.Rows[i]["Flood Policy"].ToString());
								//	if (id != null && id.FloodPolicyId > 0)
								//		objLead.FloodPolicy = id.FloodPolicyId;
								//}

								if (table.Rows[i]["Owner First Name"] != string.Empty)
									if (table.Rows[i]["Owner First Name"].ToString().Length > 50)
										objLead.OwnerFirstName = table.Rows[i]["Owner First Name"].ToString().Substring(0, 50);
									else
										objLead.OwnerFirstName = table.Rows[i]["Owner First Name"].ToString();

								if (table.Rows[i]["Owner Last Name"] != string.Empty) {
									str = table.Rows[i]["Owner Last Name"].ToString();
									objLead.OwnerLastName = str.Length > 50 ? str.Substring(0, 50) : str;
								}

								if (table.Rows[i]["Owner Phone Number"] != string.Empty) {
									str = table.Rows[i]["Owner Phone Number"].ToString();
									objLead.OwnerPhone = str.Length > 15 ? str.Substring(0, 15) : str;
								}

								if (table.Rows[i]["Last Contact Date"] != string.Empty)
									objLead.LastContactDate = Convert.ToDateTime(table.Rows[i]["Last Contact Date"].ToString());



								if (table.Rows[i]["Personal Referral"] != string.Empty) {
									str = table.Rows[i]["Personal Referral"].ToString();
									objLead.HearAboutUsDetail = str.Length > 250 ? str.Substring(0, 250) : str;
								}

								StatusMaster statusMaster = null;

								if (table.Rows[i]["Status"] != string.Empty) {
									string statusName = table.Rows[i]["Status"].ToString();
									statusName = statusName.Length > 100 ? statusName.Substring(0, 100) : statusName;
									statusMaster = StatusManager.GetByStatusName(statusName);

									if (statusMaster.StatusId == 0) {
										statusMaster = new StatusMaster();
										statusMaster.clientID = clientID;
										statusMaster.InsertBy = objLead.UserId;
										statusMaster.InsertDate = DateTime.Now;
										statusMaster.isCountable = true;
										statusMaster.isCountAsOpen = true;
										statusMaster.Status = true;
										statusMaster.StatusName = statusName;

										statusMaster = StatusManager.Save(statusMaster);
									}
								}



								objLead.Status = 1;
								objLead.InsertBy = objLead.UserId;

								// 2013-08-29 tortega
								if (clientID > 0)
									objLead.ClientID = clientID;

								Leads newLead = LeadsManager.Save(objLead);

								if (newLead != null) {
									LeadComment objLeadComment = null;

									// add policy
									CRM.Data.Entities.LeadPolicy policy = new CRM.Data.Entities.LeadPolicy();
									policy.PolicyType = 1;
									policy.LeadId = newLead.LeadId;
									policy.IsActive = true;
									policy.isAllDocumentUploaded = false;

									if (statusMaster != null)
										policy.LeadStatus = statusMaster.StatusId;

									if (adjusterID > 0)
										policy.AdjusterID = adjusterID;

									if (table.Rows[i]["Claims Number"] != string.Empty)
										policy.ClaimNumber = table.Rows[i]["Claims Number"].ToString();

									if (table.Rows[i]["Site Survey Date"] != string.Empty)
										policy.SiteSurveyDate = Convert.ToDateTime(table.Rows[i]["Site Survey Date"].ToString());

									LeadPolicyManager.Save(policy);

									if (!string.IsNullOrEmpty(table.Rows[i]["Reported to Insurer"].ToString())) {
										objLeadComment = new LeadComment();
										int LeadID = objLead.LeadId;

										objLeadComment.LeadId = LeadID;

										objLeadComment.UserId = objLead.UserId;

										objLeadComment.Status = 1;

										// 2013-08-29 tortega
										objLeadComment.PolicyType = policyType;

										str = table.Rows[i]["Reported to Insurer"].ToString();
										if (str.Length > 8000)
											objLeadComment.CommentText = "Reported to Insurer " + str.Substring(0, 8000);
										else
											objLeadComment.CommentText = "Reported to Insurer " + str;

										LeadComment objld = LeadCommentManager.Save(objLeadComment);
									}


									if (!string.IsNullOrEmpty(table.Rows[i]["Claimant Comments"].ToString())) {
										objLeadComment = new LeadComment();
										int LeadID = objLead.LeadId;

										objLeadComment.LeadId = LeadID;

										objLeadComment.UserId = objLead.UserId;

										objLeadComment.Status = 1;

										// 2013-08-29 tortega
										objLeadComment.PolicyType = policyType;

										str = table.Rows[i]["Claimant Comments"].ToString();
										if (str.Length > 8000)
											objLeadComment.CommentText = str.Substring(0, 8000);
										else
											objLeadComment.CommentText = str;

										LeadComment objld = LeadCommentManager.Save(objLeadComment);
									}
								}

							}	//for (int i = 0; i < table.Rows.Count; i++) {

							scope.Complete();

						}
						string rootFolderPath = Server.MapPath("~//CSVLoad//");
						string filesToDelete = FileUpload1.FileName;
						string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
						foreach (string file in fileList) {
							System.IO.File.Delete(file);
						}
						lblSave.Text = "Data Saved Successfully !!!";
						lblSave.Visible = true;
					}
				}
			}
			catch (Exception ex) {
				lblError.Text = "There Is a problem in  data save  !!!";
				lblError.Visible = true;

				Core.EmailHelper.emailError(ex);
			}


		}



		protected void btnUpload_Click(object sender, EventArgs e) {

			UploadTemplate2();
		}

		protected void btnCancel_Click(object sender, EventArgs e) {

		}
	}
}