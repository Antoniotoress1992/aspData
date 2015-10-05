using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account
{
    public class CheckPrimaryValue
    {

        #region To Check Primary Value
        public static List<Leads> CheckPrimaryValueExists(int LeadStatus, int Adjuster, int LeadSource, int PrimaryProducerId, int SecondaryProducerId, int InspectorName,
            int WebformSource, int TypeOfProperty, int SiteInspectionCompleted, int StateId, int Habitable, int WindPolicy, int FloodPolicy, int ReporterToInsurer, int SubStatus, int OwnerSame, int OtherSource)
        {
            List<Leads> result = new List<Leads>();
		  //if (LeadStatus > 0)
		  //{
		  //    result = DbContextHelper.DbContext.Leads.Where(x => x.LeadStatus == LeadStatus && x.Status == 1).ToList();
		  //}
		  //if (Adjuster > 0)
		  //{
		  //    result = DbContextHelper.DbContext.Leads.Where(x => x.Adjuster == Adjuster && x.Status == 1).ToList();
		  //}
            if (LeadSource > 0)
            {
                result = DbContextHelper.DbContext.Leads.Where(x => x.LeadSource == LeadSource && x.Status == 1).ToList();
            }
            if (PrimaryProducerId > 0)
            {
                result = DbContextHelper.DbContext.Leads.Where(x => x.PrimaryProducerId == PrimaryProducerId && x.Status == 1).ToList();
            }
            if (SecondaryProducerId > 0)
            {
                result = DbContextHelper.DbContext.Leads.Where(x => x.SecondaryProducerId == SecondaryProducerId && x.Status == 1).ToList();
            }
            if (InspectorName > 0)
            {
                result = DbContextHelper.DbContext.Leads.Where(x => x.InspectorName == InspectorName && x.Status == 1).ToList();
            }
            if (WebformSource > 0)
            {
                result = DbContextHelper.DbContext.Leads.Where(x => x.WebformSource == WebformSource && x.Status == 1).ToList();
            }
            if (TypeOfProperty > 0)
            {
                result = DbContextHelper.DbContext.Leads.Where(x => x.TypeOfProperty == TypeOfProperty && x.Status == 1).ToList();
            }
            if (SiteInspectionCompleted > 0)
            {
                result = DbContextHelper.DbContext.Leads.Where(x => x.SiteInspectionCompleted == SiteInspectionCompleted && x.Status == 1).ToList();
            }
            if (StateId > 0)
            {
                result = DbContextHelper.DbContext.Leads.Where(x => x.StateId == StateId && x.Status == 1).ToList();
            }
		  //if (Habitable > 0)
		  //{
		  //    result = DbContextHelper.DbContext.Leads.Where(x => x.Habitable == Habitable && x.Status == 1).ToList();
		  //}
		  //if (WindPolicy > 0)
		  //{
		  //    result = DbContextHelper.DbContext.Leads.Where(x => x.WindPolicy == WindPolicy && x.Status == 1).ToList();
		  //}

		  //if (FloodPolicy > 0)
		  //{
		  //    result = DbContextHelper.DbContext.Leads.Where(x => x.FloodPolicy == FloodPolicy && x.Status == 1).ToList();
		  //}
            if (ReporterToInsurer > 0)
            {
                result = DbContextHelper.DbContext.Leads.Where(x => x.ReporterToInsurer == ReporterToInsurer && x.Status == 1).ToList();
            }
		  //if (SubStatus > 0)
		  //{
		  //    result = DbContextHelper.DbContext.Leads.Where(x => x.SubStatus == SubStatus && x.Status == 1).ToList();
		  //}
            if (OwnerSame > 0)
            {
                result = DbContextHelper.DbContext.Leads.Where(x => x.OwnerSame == OwnerSame && x.Status == 1).ToList();
            }
            if (OtherSource > 0)
            {
                result = DbContextHelper.DbContext.Leads.Where(x => x.OtherSource == OtherSource && x.Status == 1).ToList();
            }
            return result;
        }


        public static List<Leads> checkTypeofDamage(string TypeofDamage)
        {
            List<Leads> result = new List<Leads>();
            if (TypeofDamage.Length >0)
            {
                result = DbContextHelper.DbContext.Leads.Where(x => x.TypeofDamageText.Contains(TypeofDamage) && x.Status == 1).ToList();
            }
            return result;
        }


        #endregion
    }
}
