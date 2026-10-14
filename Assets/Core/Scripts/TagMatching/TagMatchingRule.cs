using System;
using System.Collections.Generic;
using TestTechXplored.Core.Scripts.Board;

namespace TestTechXplored.Core.Scripts.TagMatching
{
    [Serializable]
    public struct TagMatchingRule
    {
        public List<TagValue> requiredTags;
    }
}