﻿using System.Collections.Generic;

namespace Flubu.Builds.VSSolutionBrowsing
{
    /// <summary>
    /// Contains information of compile configuration.
    /// </summary>
    public class VSProjectConfiguration
    {
        public string Condition
        {
            get { return condition; }
            set { condition = value; }
        }
        
        public IDictionary<string, string> Properties
        {
            get { return properties; }
        }

        private Dictionary<string, string> properties = new Dictionary<string, string>();
        private string condition;
    }
}
