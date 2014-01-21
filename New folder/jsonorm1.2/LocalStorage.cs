using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace jsonorm
{


    public class LocalStorage<T> where T : new()
    {

        protected ISession session
        {
            get
            {
                return JsonUtils.Instance;
            }
        }

        //protected string ServiceEndPoint { get; set; }

        public void Save(T entity, string entityname)
        {
            session.Save<T>(entity, entityname);
        }

        public void Save(string entity, string entityname)
        {
            T tentity = Commons.Instance.DeserializeObject<T>(entity);
            session.Save<T>(tentity, entityname);
        }

        public void SaveOrUpdate(T entity, Func<T, bool> query, string entityname)
        {
            session.SaveOrUpdate<T>(entity, query, entityname);
        }

        public T GetElementById(Func<T, bool> query, string entityname)
        {
            return session.GetElementById<T>(query, entityname);
        }

        public List<T> GetElementList(string entityname)
        {
            return session.GetElementList<T>(entityname);
        }

        public List<T> GetElementList(Func<T, bool> query, string entityname)
        {
            return session.GetElementList<T>(query, entityname);
        }



    }


    public class NotifyContainerUpdates
    {

        public NotifyContainerUpdates()
        {

        }

    }




}
