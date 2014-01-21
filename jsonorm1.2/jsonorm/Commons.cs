using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jsonorm
{
    internal class Commons
    {
        protected Commons()
        {
        }

        private static Commons _commons;

        public static Commons Instance
        {
            get
            {
                if (_commons == null)
                    _commons = new Commons();
                return _commons;
            }

        }

        public T DeserializeObject<T>(string resources)
        {
            T t;
            t = JsonConvert.DeserializeObject<T>(resources);
            return t;
        }
    }
}
