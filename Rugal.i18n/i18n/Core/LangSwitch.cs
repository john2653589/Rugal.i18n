using Rugal.i18n.Model;

namespace Rugal.i18n.Core
{
    public abstract class LangSwitch
    {
        private readonly LangModel Lang;
        private readonly Dictionary<Type, LangSwitchRole> TypeRoles = new();
        public LangSwitch(LangModel _Lang)
        {
            Lang = _Lang;
            Init();
        }
        public string BaseLangMap<TModel>(TModel Data, string ColumnName)
        {
            var Role = TypeRoles[typeof(TModel)];
            var Func = Role.Get(ColumnName) as Func<LanguageType, Func<TModel, string>>;
            var Result = Func.Invoke(Lang.LanguageType).Invoke(Data);
            return Result;
        }
        public abstract void Init();
        public LangSwitch WithSwitch<TModel>(string ColumnName, Func<LanguageType, Func<TModel, string>> Func)
        {
            if (!TypeRoles.TryGetValue(typeof(TModel), out var Role))
            {
                Role = new LangSwitchRole(typeof(TModel));
                TypeRoles.Add(typeof(TModel), Role);
            }
            Role.With(ColumnName, Func);
            return this;
        }
    }
    public class LangSwitchRole
    {
        private readonly Dictionary<string, Delegate> Roles = new();
        public readonly Type ModelType;
        public LangSwitchRole(Type _ModelType)
        {
            ModelType = _ModelType;
        }
        public void With(string ColumnName, Delegate Func)
        {
            if (Roles.ContainsKey(ColumnName))
                Roles.Remove(ColumnName);

            Roles.Add(ColumnName, Func);
        }
        public Delegate Get(string ColumnName)
        {
            var RoleFunc = Roles[ColumnName];
            return RoleFunc;
        }
    }
}