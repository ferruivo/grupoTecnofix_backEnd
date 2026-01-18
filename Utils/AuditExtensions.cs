using System;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.Utils
{
    public static class AuditExtensions
    {
        public static void EnsureCreationAudit(this object entity, ICurrentUserService currentUser)
        {
            if (entity == null) return;

            var type = entity.GetType();

            var dataProp = type.GetProperty("DataCadastro");
            if (dataProp != null &&
                (dataProp.PropertyType == typeof(DateTime) || dataProp.PropertyType == typeof(DateTime?)))
            {
                var val = dataProp.GetValue(entity);
                if (val == null || (DateTime)val == default)
                    dataProp.SetValue(entity, DateTime.Now);
            }

            var userProp = type.GetProperty("IdUsuarioCadastro");
            if (userProp != null &&
                (userProp.PropertyType == typeof(int) || userProp.PropertyType == typeof(int?)))
            {
                var val = userProp.GetValue(entity);
                var isEmpty = val == null
                              || (userProp.PropertyType == typeof(int) && (int)val == 0)
                              || (userProp.PropertyType == typeof(int?) && ((int?)val) == null);

                if (isEmpty)
                {
                    try
                    {
                        userProp.SetValue(entity, currentUser.GetUsuarioLogadoId());
                    }
                    catch
                    {
                        // If currentUser is not available (unauthenticated), do nothing
                    }
                }
            }
        }

        public static void EnsureUpdateAudit(this object entity, ICurrentUserService currentUser)
        {
            if (entity == null) return;

            var type = entity.GetType();

            var dataProp = type.GetProperty("DataAlteracao");
            if (dataProp != null &&
                (dataProp.PropertyType == typeof(DateTime) || dataProp.PropertyType == typeof(DateTime?)))
            {
                var val = dataProp.GetValue(entity);
                if (val == null || (DateTime)val == default)
                    dataProp.SetValue(entity, DateTime.Now);
            }

            var userProp = type.GetProperty("IdUsuarioAlteracao");
            if (userProp != null &&
                (userProp.PropertyType == typeof(int) || userProp.PropertyType == typeof(int?)))
            {
                var val = userProp.GetValue(entity);
                var isEmpty = val == null
                              || (userProp.PropertyType == typeof(int) && (int)val == 0)
                              || (userProp.PropertyType == typeof(int?) && ((int?)val) == null);

                if (isEmpty)
                {
                    try
                    {
                        userProp.SetValue(entity, currentUser.GetUsuarioLogadoId());
                    }
                    catch
                    {
                        // If currentUser is not available (unauthenticated), do nothing
                    }
                }
            }
        }
    }
}
