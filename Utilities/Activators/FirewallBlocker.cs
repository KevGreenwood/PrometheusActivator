using NetFwTypeLib;


namespace PrometheusActivator.Utilities
{
    public static class FirewallManager
    {
        public static bool FirewallRuleExists(Product product)
        {
            try
            {
                INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                return fwPolicy.Rules.Cast<INetFwRule>().Any(rule => rule.Name == product.Name);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool AddFirewallBlockRule(Product product)
        {
            try
            {
                if (FirewallRuleExists(product))
                {
                    RemoveFirewallRule(product);
                }

                INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                Type ruleType = Type.GetTypeFromProgID("HNetCfg.FwRule");

                var inboundRule = CreateFirewallRule(product, ruleType, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN);
                var outboundRule = CreateFirewallRule(product, ruleType, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT);

                fwPolicy.Rules.Add(inboundRule);
                fwPolicy.Rules.Add(outboundRule);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveFirewallRule(Product product)
        {
            try
            {
                INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                var rulesToRemove = fwPolicy.Rules.Cast<INetFwRule>()
                                    .Where(rule => rule.Name == product.Name)
                                    .Select(rule => rule.Name)
                                    .ToList();

                foreach (var ruleName in rulesToRemove)
                {
                    fwPolicy.Rules.Remove(ruleName);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static INetFwRule CreateFirewallRule(Product product, Type ruleType, NET_FW_RULE_DIRECTION_ direction)
        {
            INetFwRule rule = (INetFwRule)Activator.CreateInstance(ruleType);
            rule.Name = product.Name;
            rule.Description = $"Blocks {(direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN ? "inbound" : "outbound")} network access for {product.Name}, limiting its online functionalities.";
            rule.ApplicationName = product.ExecutablePath;
            rule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            rule.Direction = direction;
            rule.Enabled = true;
            rule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;
            return rule;
        }
    }
}
