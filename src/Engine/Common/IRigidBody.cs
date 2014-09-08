using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fusee.Engine
{
    public interface IRigidBody
    {
        void OnCollisionEnter(IRigidBodyImp rigidBodyImp);
        void OnCollisionExit();
    }
}
