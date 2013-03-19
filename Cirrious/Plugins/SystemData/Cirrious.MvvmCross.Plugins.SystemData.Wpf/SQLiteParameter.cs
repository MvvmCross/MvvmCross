using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.SystemData.Wpf
{
    public class SQLiteParameter : Cirrious.MvvmCross.Plugins.SystemData.IDbDataParameter
    {
        private System.Data.IDbDataParameter parameter;

        internal SQLiteParameter(System.Data.IDbDataParameter dbDataParameter)
        {
            this.parameter = dbDataParameter;
        }

        public byte Precision
        {
            get
            {
                return parameter.Precision;
            }
            set
            {
                parameter.Precision = value;
            }
        }

        public byte Scale
        {
            get
            {
                return parameter.Scale;
            }
            set
            {
                parameter.Scale = value;
            }
        }

        public int Size
        {
            get
            {
                return parameter.Size;
            }
            set
            {
                parameter.Size = value;
            }
        }

        public bool IsNullable
        {
            get { return parameter.IsNullable; }
        }

        public string ParameterName
        {
            get
            {
                return parameter.ParameterName;
            }
            set
            {
                parameter.ParameterName = value;
            }
        }

        public string SourceColumn
        {
            get
            {
                return parameter.SourceColumn;
            }
            set
            {
                parameter.SourceColumn = value;
            }
        }

        public object Value
        {
            get
            {
               return parameter.Value;
            }
            set
            {
               parameter.Value = value;
            }
        }
    }
}
