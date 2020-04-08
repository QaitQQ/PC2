using CRMLibs;

using System.Collections.Generic;

namespace Client.Class
{
    public static class StaticForm
    {
        public static Partner SearchPartner(List<Partner> list, string Name) => list.Find(x => x.Name == Name);
    }




}
