using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chittychat.Services
{
    public class Blah
    {
        //private UserTracker m_userTracker;

        public Blah(UserTracker userTracker)
        {
            Console.WriteLine(userTracker.Doggy);
        }
    }
}
