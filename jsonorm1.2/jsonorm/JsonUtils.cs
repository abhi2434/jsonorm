using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace jsonorm
{

    public class JsonUtils : ISession
    {
        private static JsonUtils _jsonUtils = null;
        private static JObject _ConfigData;
        private static string _path = "..\\Json Data\\config.json";

       public static JObject ConfigData
        {
            get
            {
                return _ConfigData;
            }
            set
            {
                _ConfigData = value;
            }
        }



        protected JsonUtils()
        {
            ConfigData = Load();
        }

        #region Private Methods


        private static JObject Load()
        {
            using (var sr = new StreamReader(_path))
            {
                ConfigData = JsonConvert.DeserializeObject<JObject>(sr.ReadToEnd());
                string xyz = JsonConvert.SerializeObject(ConfigData);

            }

            return ConfigData;
        }

        #endregion

        public static JsonUtils Instance
        {
            get
            {
                if (_jsonUtils == null)
                {
                    _jsonUtils = new JsonUtils();
                }
                return _jsonUtils;
            }
        }

        #region ISession Implementation

        public T GetElementById<T>(Func<T, bool> query, string entityname) where T : new()
        {
            T data = default(T);
            try
            {
                data = ConfigData[entityname].ToObject<List<T>>().Where(query).SingleOrDefault<T>();
            }
            catch (Exception e)
            {

            }
            return data;
        }

        public List<T> GetElementList<T>(string entityname)
        {

            List<T> data = default(List<T>);
            try
            {
                data = ConfigData[entityname].ToObject<List<T>>().ToList();
            }
            catch (Exception e)
            {

            }
            return data;
        }

        public List<T> GetElementList<T>(Func<T, bool> query, string entityname)
        {
            List<T> data = default(List<T>);
            try
            {
                data = ConfigData[entityname].ToObject<List<T>>().Where(query).ToList<T>();
            }
            catch (Exception e)
            {

            }
            return data;
        }

        public List<T> GetElementList<T>(string[] elementPath, string parameter)
        {

            ///Working
            ///
            List<JToken> data = ConfigData["."].ToList();
            List<T> returnList = new List<T>();
            try
            {
                foreach (string s in elementPath)
                {
                    data = data.Values(s).ToList();
                }

                data = data.SelectMany(c => c.Select(cin => cin)
                  .Where(m => m["TouchPointId"].ToString() == parameter))
                  .ToList();
                //returnList.Add(Converter.Instance.DeserializeObject<T>(data[0].ToString()));

                returnList = data.Select(m => Commons.Instance.DeserializeObject<T>(m.ToString())).ToList();

            }
            catch (Exception x)
            {

            }
            return returnList;

        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public IList<T> FindPage<T>(int pageStartRow, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IList<T> FindSortedPage<T>(int pageStartRow, int pageSize, string sortBy, bool descending)
        {
            throw new NotImplementedException();
        }

        public void Save<T>(T entity, string entityname)
        {

            try
            {
                if (!typeof(T).IsGenericType)
                {
                    JArray data = ConfigData[entityname].ToObject<JArray>();
                    data.Add(JToken.FromObject(entity));
                    //JArray JsonConfigarr = JArray.FromObject(data);
                    ConfigData[entityname] = data;
                }
                else
                    ConfigData[entityname] = JArray.FromObject(entity);


                using (StreamWriter streamwriter = new StreamWriter(_path))
                {
                    streamwriter.Write(JsonConvert.SerializeObject(ConfigData));
                    streamwriter.Close();
                }
            }
            catch (Exception e)
            {

            }


            //List<T> listofObject = new List<T>();
            //listofObject.Add(entity);

            ////string sss =Converter.Instance.SerializeObject(entity);
            //JArray JsonConfigarr = JArray.FromObject(listofObject);// (JArray)JToken.FromObject(entity);

            //ConfigData[entityname] = JsonConfigarr;

            //using (StreamWriter streamwriter = new StreamWriter(_path))
            //{
            //    streamwriter.Write(JsonConvert.SerializeObject(ConfigData));
            //    streamwriter.Close();
            //}
        }

        public void AddRange<T>(string resource, string entityname) where T : new()
        {
            JArray JsonConfigarr = JArray.Parse(resource);

            ConfigData[entityname] = JsonConfigarr;

            using (StreamWriter streamwriter = new StreamWriter(_path))
            {
                streamwriter.Write(JsonConvert.SerializeObject(ConfigData));
                streamwriter.Close();
            }
        }

        public void Update<T>(T entity, Func<T, bool> query, string entityname)
        {

            try
            {
                JArray Jdata = ConfigData[entityname].ToObject<JArray>();
                List<T> ldata = ConfigData[entityname].ToObject<List<T>>();

                Jdata.RemoveAt(ldata.IndexOf(ldata.Where(query).SingleOrDefault<T>()));
                JToken jto = JToken.FromObject(entity);
                Jdata.Add(jto);
                JArray JsonConfigarr = JArray.FromObject(Jdata);
                ConfigData[entityname] = JsonConfigarr;

                using (StreamWriter streamwriter = new StreamWriter(_path))
                {
                    streamwriter.Write(JsonConvert.SerializeObject(ConfigData));
                    streamwriter.Close();
                }
            }
            catch (Exception e)
            {

            }


            //int index = ConfigData[entityname].Children().ToList<JToken>().FindIndex(c => c[PropertyName].Value<string>() == id);

            //JArray item = (JArray)ConfigData[entityname];
            //if (index >= 0)
            //{
            //    item.RemoveAt(index);
            //    //item.Add((JToken)JObject.Parse(entity));
            //}
        }

        public void SaveOrUpdate<T>(T entity, Func<T, bool> query, string entityname) where T : new()
        {

            T temp = GetElementById<T>(query, entityname);

            if (temp != null)
                this.Update<T>(entity, query, entityname);
            else
                this.Save<T>(entity, entityname);
        }

        public void Delete<T>(T entity, string entityname)
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
