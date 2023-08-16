using System.Text.Json;
using System.Reflection;

namespace Rugal.i18n.Core
{
    public class LangJson
    {
        #region Public Property
        public List<string> JsonFiles { get; set; }
        public Dictionary<string, object> JsonLang { get; set; }
        #endregion

        public LangJson()
        {
            JsonFiles = new List<string>();
        }
        public LangJson(string JsonPath) : this()
        {
            JsonFiles.Add(JsonPath);
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
        public virtual LangJson WithJson(string JsonPath)
        {
            JsonFiles.Add(JsonPath);
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
        #endregion

        #region Private Process
        private void LoadJsonFile(string FullJsonPath)
        {
            JsonLang ??= new Dictionary<string, object> { };

            var GetAssembly = Assembly.GetExecutingAssembly();
            var AssemblyName = GetAssembly.GetName().Name;

            FullJsonPath = $"{AssemblyName}.{FullJsonPath}".ToLower();
            var AllNames = GetAssembly.GetManifestResourceNames();
            var FindNames = AllNames
                .FirstOrDefault(Item => Item.ToLower() == FullJsonPath);

            if (FindNames is null)
                return;

            using var JsonStream = GetAssembly.GetManifestResourceStream(FindNames);

            using var Reader = new StreamReader(JsonStream);
            var JsonText = Reader.ReadToEnd();
            var GetLang = JsonSerializer.Deserialize<Dictionary<string, object>>(JsonText);
            //var GetLang = JsonSerializer.Deserialize<Dictionary<string, object>>(JsonStream, new JsonSerializerOptions()
            //{
            //    Encoder = Encoding.UTF8.GetEncoder(),
            //});

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
}
