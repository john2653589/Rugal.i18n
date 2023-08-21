using System.Text.Json;
using System.Reflection;
using Rugal.i18n.Model;

namespace Rugal.i18n.Core
{
    public class LangJson
    {
        #region Public Property
        public LangSetting Setting { get; set; }
        public List<LangJsonInfo> JsonFiles { get; set; }
        public Dictionary<string, object> JsonLang { get; set; }
        #endregion

        public LangJson()
        {
            JsonFiles = new List<LangJsonInfo>();
        }
        #region Public Method
        public virtual string Get(string LangKey)
        {
            InitJson();

            if (!JsonLang.TryGetValue(LangKey, out var Value))
                return LangKey;

            if (Value is JsonElement JsonValue)
            {
                if (JsonValue.ValueKind != JsonValueKind.String)
                    return LangKey;
                var Result = JsonValue.GetString();
                return Result;
            }
            return Value?.ToString();
        }
        public virtual LangJson WithJson(string JsonPath, Assembly TargetAssembly = null)
        {
            TargetAssembly ??= Setting.TargetAssembly;
            var AddInfo = new LangJsonInfo()
            {
                JsonPath = JsonPath,
                TargetAssembly = TargetAssembly
            };
            JsonFiles.Add(AddInfo);
            return this;
        }
        public virtual LangJson WithJson(LangJsonInfo JsonInfo)
        {
            JsonFiles.Add(JsonInfo);
            return this;
        }
        public virtual LangJson InitJson(bool IsForce = false)
        {
            if (JsonLang is not null && !IsForce)
                return this;

            JsonLang?.Clear();
            foreach (var Item in JsonFiles)
                LoadJsonFile(Item);

            return this;
        }
        public virtual LangJson WithSetting(LangSetting _Setting)
        {
            Setting = _Setting;
            return this;
        }
        #endregion

        #region Private Process
        private void LoadJsonFile(LangJsonInfo Info)
        {
            JsonLang ??= new Dictionary<string, object> { };

            var FullJsonPath = $"{Info.AssemblyName}.{Info.JsonPath}".ToLower();
            var AllNames = Info.TargetAssembly.GetManifestResourceNames();
            var FindNames = AllNames
                .FirstOrDefault(Item => Item.ToLower() == FullJsonPath);

            if (FindNames is null)
                return;

            using var JsonStream = Info.TargetAssembly.GetManifestResourceStream(FindNames);

            using var Reader = new StreamReader(JsonStream);
            var JsonText = Reader.ReadToEnd();
            var GetLang = JsonSerializer.Deserialize<Dictionary<string, object>>(JsonText);

            if (GetLang is null)
                return;

            foreach (var Item in GetLang)
            {
                if (JsonLang.ContainsKey(Item.Key))
                    JsonLang.Remove(Item.Key);

                JsonLang.TryAdd(Item.Key, Item.Value);
            }
        }
        #endregion
    }
    public class LangJsonInfo
    {
        public string JsonPath { get; set; }
        public string AssemblyName => TargetAssembly.GetName().Name;
        public Assembly TargetAssembly { get; set; }
    }
}
