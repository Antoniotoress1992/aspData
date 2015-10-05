using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Core;
using CRM.Data.Entities;
using System.Data.Entity.Infrastructure;

namespace CRM.Data.Account
{
    public static class ExtensionMethods
    {
        public static void Add(this CRMEntities context, object entity) 
        {
            if (entity != null)
            {
                Type type = entity.GetType();
                context.Set(type).Add(entity);
            }
            else 
            {
                throw new NullReferenceException("Object can not be null");
            }
        }

        public static void DeleteObject(this CRMEntities context, object entity) 
        {
            if (entity != null)
            {
                Type type = entity.GetType();
                context.Set(type).Remove(entity);
            }
            else
            {
                throw new NullReferenceException("Object can not be null");
            }
        }

        public static void AttachTo(this CRMEntities context, string value, object entity)
        {
            if (entity != null)
            {
                Type type = entity.GetType();
                context.Set(type).Attach(entity);
            }
            else
            {
                throw new NullReferenceException("Object can not be null");
            }
        }
    }
}
