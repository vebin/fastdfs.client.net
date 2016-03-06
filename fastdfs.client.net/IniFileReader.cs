using System;
using System.Collections;
using System.IO;

namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class IniFileReader
    {
        private Hashtable paramTable;
        private string conf_filename;

        public IniFileReader(string conf_filename)
        {
            this.conf_filename = conf_filename;
            loadFromFile(conf_filename);
        }

        public string getConfFilename()
        {
            return this.conf_filename;
        }

        public string getStrValue(string name)
        {
            object obj;
            obj = this.paramTable[name];
            if (obj == null)
            {
                return null;
            }

            if (obj is string)
            {
                return (string)obj;
            }

            return (string)((ArrayList)obj)[0];
        }

        public int getIntValue(string name, int default_value)
        {
            string szValue = this.getStrValue(name);
            if (szValue == null)
            {
                return default_value;
            }

            return int.Parse(szValue);
        }

        public bool getBoolValue(string name, bool default_value)
        {
            string szValue = this.getStrValue(name);
            if (szValue == null)
            {
                return default_value;
            }

            return szValue.Equals("yes", StringComparison.OrdinalIgnoreCase) || szValue.Equals("on", StringComparison.OrdinalIgnoreCase) ||
                         szValue.Equals("true", StringComparison.OrdinalIgnoreCase) || szValue.Equals("1", StringComparison.OrdinalIgnoreCase);
        }

        public string[] getValues(string name)
        {
            object obj;
            string[] values;

            obj = this.paramTable[name];
            if (obj == null)
            {
                return null;
            }

            if (obj is string)
            {
                values = new string[1];
                values[0] = (string)obj;
                return values;
            }

            object[] objs = ((ArrayList)obj).ToArray();
            values = new string[objs.Length];
            Array.Copy(objs, 0, values, 0, objs.Length);
            return values;
        }

        private void loadFromFile(string conf_filename)
        {
            TextReader buffReader;
            string line;
            string[] parts;
            string name;
            string value;
            object obj;
            ArrayList valueList;

            buffReader = new StringReader(File.ReadAllText(conf_filename));
            this.paramTable = new Hashtable();

            try
            {
                while ((line = buffReader.ReadLine()) != null)
                {
                    line = line.Replace("\0", "").Trim();
                    if (line.Length == 0 || line[0] == '#')
                    {
                        continue;
                    }

                    parts = line.Split("=".ToCharArray(), 2);
                    if (parts.Length != 2)
                    {
                        continue;
                    }

                    name = parts[0].Replace("\0", "").Trim();
                    value = parts[1].Replace("\0", "").Trim();

                    obj = this.paramTable[name];
                    if (obj == null)
                    {
                        this.paramTable[name] = value;
                    }
                    else if (obj is string)
                    {
                        valueList = new ArrayList();
                        valueList.Add(obj);
                        valueList.Add(value);
                        this.paramTable[name] = valueList;
                    }
                    else
                    {
                        valueList = (ArrayList)obj;
                        valueList.Add(value);
                    }
                }
            }
            finally
            {
                buffReader.Close();
            }
        }
    }
}
