using TestTechXplored.Core.Scripts.Board;

namespace TestTechXplored.Core.Scripts.TagMatching
{
    public static class BoardEntityMatcher
    {
        public static bool Matches(
            BoardEntityDefinition entityDefinition,
            TagMatchingRule rule)
        {
            if (entityDefinition == null ||
                entityDefinition.Tags == null ||
                rule.requiredTags == null ||
                rule.requiredTags.Count == 0)
                return false;

            for (var i = 0; i < rule.requiredTags.Count; i++)
            {
                var required = rule.requiredTags[i];
                bool found = false;

                for (var j = 0; j < entityDefinition.Tags.Count; j++)
                {
                    var actual = entityDefinition.Tags[j];
                    if (actual.tag == required.tag &&
                        actual.value == required.value)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }

            return true;
        }
    }
}
