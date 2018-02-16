﻿using Sitecore.Commerce.Connect.CommerceServer;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Globalization;

namespace Sitecore.Support.Commerce.Connect.CommerceServer
{
    public class ToSitecoreFieldValue : IToSitecoreFieldValue
    {
        public virtual string Convert(object source, TemplateFieldItem field)
        {
            if (field.Type.Equals("datetime", System.StringComparison.OrdinalIgnoreCase) && source is System.DateTime)
            {
                return DateUtil.ToIsoDate((System.DateTime)source, true);
            }
            if (field.Type.Equals("checkbox", System.StringComparison.OrdinalIgnoreCase) && source is bool)
            {
                if (!(bool)source)
                {
                    return "0";
                }
                return "1";
            }
            else
            {
                if (source == null)
                {
                    return string.Empty;
                }
                #region
                if (source is decimal)
                {
                    return ((decimal)source).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                if (source is double)
                {
                    return ((double)source).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                #endregion
                return source.ToString();
            }
        }

        public virtual string Convert(object value, string culture)
        {
            if (value == System.DBNull.Value)
            {
                return string.Empty;
            }
            if (value is System.DateTime)
            {
                return DateUtil.ToIsoDate((System.DateTime)value, true);
            }
            if (value is decimal)
            {
                return ((decimal)value).ToString(this.GetFormatProvider(culture));
            }
            if (value is float)
            {
                return ((float)value).ToString(this.GetFormatProvider(culture));
            }
            if (value is double)
            {
                return ((double)value).ToString(this.GetFormatProvider(culture));
            }
            return value.ToString();
        }

        private System.IFormatProvider GetFormatProvider(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                return System.Globalization.CultureInfo.InvariantCulture;
            }
            System.IFormatProvider result;
            try
            {
                result = System.Globalization.CultureInfo.GetCultureInfo(language);
            }
            catch (CultureNotFoundException ex)
            {
                string message = string.Format(System.Globalization.CultureInfo.InvariantCulture, "The specified culture '{0}' was not recognized as a valid format provider:  {1}", new object[]
                {
                    language,
                    ex.ToString()
                });
                Log.Warn(message, this);
                result = System.Globalization.CultureInfo.InvariantCulture;
            }
            return result;
        }
    }
}

