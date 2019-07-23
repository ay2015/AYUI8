using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDS.Models.UI
{
    public class dicts
    {
        #region Constructor
        public dicts() { }

        public dicts(string id, String value, String name, String systemname, String field, String ext)
        {
            this._id = id;
            this._value = value;
            this._name = name;
            this._systemname = systemname;
            this._field = field;
            this._ext = ext;
        }
        #endregion

        #region Attributes
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _systemname;

        public string SystemName
        {
            get { return _systemname; }
            set { _systemname = value; }
        }
        private string _field;

        public string Field
        {
            get { return _field; }
            set { _field = value; }
        }
        private string _ext;

        public string Ext
        {
            get { return _ext; }
            set { _ext = value; }
        }
        #endregion
    }
}
