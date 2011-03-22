using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public interface Instantiatable<E>
    {
        void InstantiateFrom(E dbEntity);
    }
}