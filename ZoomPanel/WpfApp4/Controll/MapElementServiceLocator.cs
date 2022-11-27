using ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapControls.Core;
using MapControls;
using MapControls.MapText;
using MapControls.MapLine;

namespace MapControls
{
    public class MapElementServiceLocator : IServiceLocator
    {
        IEnumerable<object> IServiceLocator.GetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }

        IEnumerable<TService> IServiceLocator.GetAllInstances<TService>()
        {
            throw new NotImplementedException();
        }

        object IServiceLocator.GetInstance(Type serviceType)
        {
            if (serviceType == typeof(MapTextViewModel))
                return new MapTextView();
            if (serviceType == typeof(MapLinesViewModel))
                return new MapLinesView();
            return null;
        }

        object IServiceLocator.GetInstance(Type serviceType, string key)
        {
            throw new NotImplementedException();
        }

        TService IServiceLocator.GetInstance<TService>()
        {
            throw new NotImplementedException();
        }

        TService IServiceLocator.GetInstance<TService>(string key)
        {
            throw new NotImplementedException();
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
