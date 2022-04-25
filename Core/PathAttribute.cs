using System;

namespace UIComponents.Core
{
    public abstract class PathAttribute : Attribute
    {
        public string Path { get; protected set; }
        
        public string RelativeTo { get; set; }

        public string GetAssetPath()
        {
            if (Path == null)
                Path = string.Empty;

            if (!string.IsNullOrEmpty(RelativeTo))
                return string.Join("/", RelativeTo, Path);

            return Path;
        }
    }
}