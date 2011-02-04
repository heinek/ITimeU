using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ITimeU.Tests
{
    /// <summary>
    /// A Class to allow simulation of SessionObject
    /// </summary>
    public class MockHttpSession : HttpSessionStateBase
    {
        Dictionary<string, object> m_SessionStorage = new Dictionary<string, object>();

        public override object this[string name]
        {
            get {
                if(m_SessionStorage.ContainsKey(name))
                    return m_SessionStorage[name];
                return null;
            }
            set { m_SessionStorage[name] = value; }
        }
    }
}
