using System.Collections.Generic;
using DesperateDevs.Utils;
using Entitas.Serialization;

namespace Common
{
    public class BlueprintService :IService
    {
        private Dictionary<string, Blueprint> m_blueprintsDict;
        private readonly IBlueprints m_blueprints;
        
        public BlueprintService(IBlueprints blueprints)
        {
            m_blueprints = blueprints;
        }

        public void AddBlueprints(byte[] data)
        {
            var dict =  m_blueprints.ToBlueprints(data);
            if (m_blueprintsDict == null)
            {
                m_blueprintsDict = dict;
            }
            else
            {
                m_blueprintsDict.Merge(dict);
            }
        }
        
        public Blueprint GetBlueprint(string name)
        {
            Blueprint blueprint;
            m_blueprintsDict.TryGetValue(name, out blueprint);
            return blueprint;
        }
    }
}