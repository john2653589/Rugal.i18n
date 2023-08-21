using Rugal.i18n.Model;

namespace Rugal.i18n.Core
{
    public abstract class LangSwitch
    {
        public readonly LangModel Lang;
        private readonly Dictionary<Type, LangSwitchRole> TypeRoles = new();
        public LangSwitch(LangModel _Lang)
        {
            Lang = _Lang;
            Init();
        }
        public abstract void Init();
        public virtual string BaseLangMap<TModel>(TModel Data, string ColumnName)
        {
            var Role = TypeRoles[typeof(TModel)];
            var RolesFunc = Role.Get(ColumnName);

            if (RolesFunc.RolesFuncFrom == RolesFuncFromType.FromLangType)
            {
                var Func = RolesFunc.Func as Func<LanguageType, Func<TModel, string>>;
                var Result = Func.Invoke((LanguageType)Lang.LanguageType).Invoke(Data);
                return Result;
            }
            else if (RolesFunc.RolesFuncFrom == RolesFuncFromType.FromDataValue)
            {
                var Func = RolesFunc.Func as Func<TModel, string>;
                var Result = Func.Invoke(Data);
                return Result;
            }
            return null;
        }
        public virtual LangSwitch WithSwitch<TModel>(string ColumnName, Func<LanguageType, Func<TModel, string>> Func)
        {
            if (!TypeRoles.TryGetValue(typeof(TModel), out var Role))
            {
                Role = new LangSwitchRole(typeof(TModel));
                TypeRoles.Add(typeof(TModel), Role);
            }
            Role.With(ColumnName, Func);
            return this;
        }

        public LangSwitch WithSwitchValue<TModel>(string ColumnName, Func<TModel, string> Func)
        {
            if (!TypeRoles.TryGetValue(typeof(TModel), out var Role))
            {
                Role = new LangSwitchRole(typeof(TModel));
                TypeRoles.Add(typeof(TModel), Role);
            }
            Role.WithValue(ColumnName, Func);
            return this;
        }
    }
    public class LangSwitchRole
    {
        private readonly Dictionary<string, RolesFunc> Roles = new();
        public readonly Type ModelType;
        public LangSwitchRole(Type _ModelType)
        {
            ModelType = _ModelType;
        }
        public void With(string ColumnName, Delegate Func)
        {
            if (Roles.ContainsKey(ColumnName))
                Roles.Remove(ColumnName);

            Roles.Add(ColumnName, new RolesFunc()
            {
                Func = Func,
                RolesFuncFrom = RolesFuncFromType.FromLangType,
            });
        }
        public void WithValue(string ColumnName, Delegate Func)
        {
            if (Roles.ContainsKey(ColumnName))
                Roles.Remove(ColumnName);

            Roles.Add(ColumnName, new RolesFunc()
            {
                Func = Func,
                RolesFuncFrom = RolesFuncFromType.FromDataValue,
            });
        }
        public RolesFunc Get(string ColumnName)
        {
            var RoleFunc = Roles[ColumnName];
            return RoleFunc;
        }
    }

    public class RolesFunc
    {
        public Delegate Func { get; set; }
        public RolesFuncFromType RolesFuncFrom { get; set; }
    }

    public enum RolesFuncFromType
    {
        FromLangType,
        FromDataValue,
    }
}