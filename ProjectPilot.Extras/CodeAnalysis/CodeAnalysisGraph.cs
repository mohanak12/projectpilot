using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPilot.Extras.CodeAnalysis
{
    public class CodeAnalysisGraph
    {
        public IDictionary<string, ICodeEntity> Entities
        {
            get { return entities; }
        }

        public ICodeEntity GetEntity (ICodeEntity entity)
        {
            if (entities.ContainsKey(entity.EntityFullName))
                return entities[entity.EntityFullName];

            entities.Add(entity.EntityFullName, entity);
            entity.UniqueId = entitiesUniqueIdCounter++;

            return entity;
        }

        private int entitiesUniqueIdCounter = 0;
        private Dictionary<string, ICodeEntity> entities = new Dictionary<string, ICodeEntity>();
    }
}
